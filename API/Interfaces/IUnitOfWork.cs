using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Provides the repositories to access the persistent storage.
    /// </summary>
    public interface IUnitOfWork
    {
        // NOTE: The unit of work pattern gives a transactional approach to repositories. When a request 
        // comes in, it's considered a transaction. Repositories will fetch data and possibly mark entities
        // as created, modified or removed, but the actual queries will be performed when calling
        // the Complete() method of the unit of work, usually at the end. This way we have a single
        // DataContext for all repositories (we also avoid problems related to having multiple contexts).

        IUserRepository UserRepository { get; }
        IGameRepository GameRepository { get; }
        ILibraryRepository LibraryRepository { get; }

        /// <summary>
        /// Saves changes to the persistent storage.
        /// </summary>
        Task<bool> Complete();
    }
}
