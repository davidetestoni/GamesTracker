using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
            => await _context.Users.FindAsync(id);

        public async Task<AppUser> GetUserByUsernameAsync(string username)
            => await _context.Users.SingleOrDefaultAsync(u => u.UserName == username.ToLower());

        public async Task<AppUser> GetUserByEmailAsync(string email)
            => await _context.Users.SingleOrDefaultAsync(u => u.Email == email.ToLower());

        // This improves the performances by crafting a query that only gets the attributes we need
        public async Task<UserInfoDto> GetUserInfoAsync(string username)
            => await _context.Users
                .Where(u => u.UserName == username.ToLower())
                .ProjectTo<UserInfoDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<UserInfoDto>> GetUserInfosAsync()
            => await _context.Users
                .ProjectTo<UserInfoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
            => await _context.Users.ToListAsync();

        /// <summary>
        /// Returns true if any changes have been saved.
        /// </summary>
        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;

        public void Add(AppUser user)
            => _context.Add(user);

        public void Update(AppUser user)
            => _context.Entry(user).State = EntityState.Modified;
    }
}
