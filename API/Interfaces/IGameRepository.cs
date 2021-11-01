using API.DTOs;
using API.Entities;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IGameRepository
    {
        void Add(Game game);
        Task<bool> SaveAllAsync();
        Task<Game> GetGameAsync(int id);
        Task<Game> GetGameAsync(long id);
        Task<GameInfoDto> GetGameInfoAsync(long id);
    }
}
