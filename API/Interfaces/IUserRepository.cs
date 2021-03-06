using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserInfoDto>> GetUserInfosAsync();
        Task<UserInfoDto> GetUserInfoAsync(string username);
        void Add(AppUser user);
    }
}
