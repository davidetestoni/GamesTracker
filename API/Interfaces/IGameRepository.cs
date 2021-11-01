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
        Task<GameInfoDto> GetGameInfoAsync(int id);
    }
}
