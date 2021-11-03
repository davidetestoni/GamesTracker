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
    public class LibraryRepository : ILibraryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public LibraryRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(Game game)
            => _context.Add(game);

        public void Update(Game game)
            => _context.Entry(game).State = EntityState.Modified;

        public async Task<Game> GetGameAsync(long id)
            => await _context.Games.FindAsync(id);

        public async Task<GameInfoDto> GetGameInfoAsync(long id)
            => await _context.Games
                .Where(g => g.Id == id)
                .ProjectTo<GameInfoDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        public async Task<IEnumerable<LibraryGameInfoDto>> GetAsync(string username)
        {
            // ProjectTo didn't seem to be working here, probably because of the includes
            var userGames = await _context.UserGames
                .Include(ug => ug.SourceUser)
                .Include(ug => ug.Game)
                .Where(ug => ug.SourceUser.UserName == username.ToLower())
                .ToListAsync();

            return userGames.Select(ug => _mapper.Map<LibraryGameInfoDto>(ug));
        }

        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;

        public async Task<UserGame> GetUserGameAsync(int userId, long gameId)
            => await _context.UserGames
                .Where(ug => ug.SourceUserId == userId && ug.GameId == gameId)
                .FirstOrDefaultAsync();

        public void Remove(UserGame userGame)
            => _context.Entry(userGame).State = EntityState.Deleted;
    }
}
