using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IDistributedCache _cache;

        public AccountController(IUnitOfWork unitOfWork, ITokenService tokenService,
            IMapper mapper, IEmailService emailService, IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
            _emailService = emailService;
            _cache = cache;
        }

        // NOTE: If we pass e.g. strings as parameters it will expect to find them in the query string,
        // unless we use attributes to explicitly tell the controller where to read them from,
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

            _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.Complete();

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
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(resetPasswordRequestDto.UserName);

            if (user is null)
            {
                return BadRequest("Invalid username");
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
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(resetPasswordDto.UserName);

            if (user is null)
            {
                return BadRequest("Invalid username");
            }

            // Search for the code in the cache
            var code = await _cache.GetStringAsync($"{user.UserName}_password_reset_code");

            if (code is null || code != resetPasswordDto.TempCode)
            {
                return BadRequest("Invalid code");
            }

            // Update the user's password and salt in the database
            using var hmac = new HMACSHA512();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(resetPasswordDto.NewPassword));
            user.PasswordSalt = hmac.Key;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.Complete();

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
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(loginDto.UserName);

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
            => await _unitOfWork.UserRepository.GetUserByUsernameAsync(username) is not null;

        private async Task<bool> EmailExists(string email)
            => await _unitOfWork.UserRepository.GetUserByEmailAsync(email) is not null;
    }
}
