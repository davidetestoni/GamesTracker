using API.Entities;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IGameRepository
    {
        void Add(Game game);
        void Update(Game game);
        Task<bool> SaveAllAsync();
        Task<Game> GetGameByIdAsync(long id);
    }
}
