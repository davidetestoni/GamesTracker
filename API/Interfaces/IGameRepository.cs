using API.DTOs;
using API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Repository for games in users' libraries.
    /// </summary>
    public interface IGameRepository
    {
        void Add(Game game);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<GameInfoDto>> GetLibraryAsync(string username);
        Task<Game> GetGameAsync(long id);
        Task<GameInfoDto> GetGameInfoAsync(long id);
    }
}
