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
        void Add(Game game);
        void Update(Game game);
        Task<bool> SaveAllAsync();
        Task<UserGame> GetUserGameAsync(int userId, long gameId);
        Task<IEnumerable<LibraryGameInfoDto>> GetAsync(string username);
        Task<Game> GetGameAsync(long id);
        Task<GameInfoDto> GetGameInfoAsync(long id);
        void Remove(UserGame userGame);
    }
}
