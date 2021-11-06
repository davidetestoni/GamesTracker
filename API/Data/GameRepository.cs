using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public void Add(Game game)
            => _context.Add(game);

        public async Task<Game> GetGameByIdAsync(long id)
            => await _context.Games.FindAsync(id);

        public void Update(Game game)
            => _context.Entry(game).State = EntityState.Modified;
    }
}
