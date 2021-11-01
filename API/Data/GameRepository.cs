using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GameRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(Game game)
            => _context.Add(game);

        public async Task<Game> GetGameAsync(int id)
            => await _context.Games.FindAsync(id);

        public async Task<GameInfoDto> GetGameInfoAsync(int id)
            => await _context.Games
                .Where(g => g.Id == id)
                .ProjectTo<GameInfoDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
