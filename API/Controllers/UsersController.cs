using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoDto>>> GetUsers()
            => Ok(await _userRepository.GetUserInfosAsync());

        [HttpGet("{username}")]
        public async Task<ActionResult<UserInfoDto>> GetUser(string username)
            => await _userRepository.GetUserInfoAsync(username);
    }
}
