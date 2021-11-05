using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IDistributedCache _cache;

        public AccountController(DataContext context, ITokenService tokenService, IMapper mapper,
            IEmailService emailService, IDistributedCache cache)
        {
            _context = context;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _cache = cache;
        }

        // NOTE: If we pass e.g. strings as parameters it will expect to find them in the query string
        // so if we want to get them from the body we need to pass them as objects (DTOs)

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName))
            {
                return BadRequest("Username is taken");
            }

            if (await EmailExists(registerDto.Email))
            {
                return BadRequest("This email address is already in use");
            }

            var user = _mapper.Map<AppUser>(registerDto);

            using var hmac = new HMACSHA512();

            user.UserName = registerDto.UserName.ToLower();
            user.Email = registerDto.Email.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Send the registration email
            // We can implement email activation here if needed
            await _emailService.SendAsync(user.Email, "Thank you for registering", "We hope you will enjoy this website!");

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("request-password-reset")]
        public async Task<ActionResult> RequestPasswordReset(ResetPasswordRequestDto resetPasswordRequestDto)
        {
            // Make sure the user exists
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == resetPasswordRequestDto.UserName.ToLower());

            if (user is null)
            {
                return NotFound("User not found");
            }

            // Generate the code
            var bytes = new byte[4];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var code = Convert.ToHexString(bytes);

            // Send the code to the user via email
            await _emailService.SendAsync(user.Email, "Reset password", $"The code to reset your password is: {code}");

            // Cache the code for 1 hour or until the user calls the reset-password endpoint
            var expiration = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync($"{user.UserName}_password_reset_code", code, expiration);

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<UserDto>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            // Search for the user for which we want to reset the password
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == resetPasswordDto.UserName.ToLower());

            if (user is null)
            {
                return NotFound("User not found");
            }

            // Search for the code in the cache
            var code = await _cache.GetStringAsync($"{user.UserName}_password_reset_code");

            if (code is null || code != resetPasswordDto.TempCode)
            {
                return Unauthorized("Invalid code");
            }

            // Update the user's password and salt in the database
            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.NewPassword));
            user.PasswordSalt = hmac.Key;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Invalidate the cached code
            await _cache.RemoveAsync($"{user.UserName}_password_reset_code");

            // Automatically login the user
            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

            if (user is null)
            {
                return Unauthorized("Invalid username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            if (!computedHash.SequenceEqual(user.PasswordHash))
            {
                return Unauthorized("Invalid password");
            }

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
            => await _context.Users.AnyAsync(x => x.UserName == username.ToLower());

        private async Task<bool> EmailExists(string email)
            => await _context.Users.AnyAsync(x => x.Email == email.ToLower());
    }
}
