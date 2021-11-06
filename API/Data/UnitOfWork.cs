using API.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);
        public IGameRepository GameRepository => new GameRepository(_context);
        public ILibraryRepository LibraryRepository => new LibraryRepository(_context, _mapper);

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Complete()
            => await _context.SaveChangesAsync() > 0;
    }
}
