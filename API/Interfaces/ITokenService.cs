using API.Entities;

namespace API.Interfaces
{
    /// <summary>
    /// Service that handles authentication tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates a new authentication token for the given <paramref name="user"/>.
        /// </summary>
        string CreateToken(AppUser user);
    }
}
