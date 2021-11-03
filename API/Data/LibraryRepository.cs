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

        public void Add(UserGame userGame)
            => _context.Add(userGame);

        public void Update(UserGame userGame)
            => _context.Entry(userGame).State = EntityState.Modified;

        // ProjectTo didn't seem to be working here, probably because Select overrides Include
        public async Task<IEnumerable<UserGame>> GetAllAsync(string username)
            => await _context.UserGames
                .Include(ug => ug.SourceUser)
                .Include(ug => ug.Game)
                .Where(ug => ug.SourceUser.UserName == username.ToLower())
                .ToListAsync();

        public async Task<bool> SaveAllAsync()
            => await _context.SaveChangesAsync() > 0;

        public async Task<UserGame> GetUserGameAsync(string username, long gameId)
            => await _context.UserGames
                .Include(ug => ug.SourceUser)
                .Include(ug => ug.Game)
                .Where(ug => ug.SourceUser.UserName == username.ToLower() && ug.GameId == gameId)
                .FirstOrDefaultAsync();

        public void Remove(UserGame userGame)
            => _context.Entry(userGame).State = EntityState.Deleted;
    }
}
