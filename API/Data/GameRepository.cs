using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<Game> GetGameAsync(long id)
            => await _context.Games.FindAsync(id);

        public async Task<GameInfoDto> GetGameInfoAsync(long id)
            => await _context.Games
                .Where(g => g.Id == id)
                .ProjectTo<GameInfoDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<GameInfoDto>> GetLibraryAsync(string username)
            => await _context.Games
                .Include(g => g.PlayedBy)
                .Where(g => g.PlayedBy.Any(ug => ug.SourceUser.UserName == username.ToLower()))
                .ProjectTo<GameInfoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;
    }
}
