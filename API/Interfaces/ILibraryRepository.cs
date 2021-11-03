using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Repository for games in users' libraries.
    /// </summary>
    public interface ILibraryRepository
    {
        void Add(UserGame userGame);
        void Update(UserGame userGame);
        Task<bool> SaveAllAsync();
        Task<UserGame> GetUserGameAsync(string username, long gameId);
        Task<IEnumerable<UserGame>> GetAllAsync(string username);
        void Remove(UserGame userGame);
    }
}
