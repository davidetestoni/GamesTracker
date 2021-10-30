using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IGamesService
    {
        Task<VideoGame> GetGame(int id);
        Task<IEnumerable<VideoGame>> SearchGames(string searchString);
    }
}
