using API.Models;
using API.Models.Pagination;
using System.Threading.Tasks;

namespace API.Interfaces
{
    /// <summary>
    /// Interacts with the games database provider.
    /// </summary>
    public interface IGamesService
    {
        Task<VideoGame> GetGameAsync(long id);
        Task<VideoGameDetails> GetGameDetailsAsync(long id);
        string GetImageUrl(string imageId, GameCoverSize size);
        string GetImageUrl(string screenshotId, GameScreenshotSize size);
        Task<PagedList<VideoGame>> SearchGamesAsync(GamesParams gamesParams);
    }

    public enum GameCoverSize
    {
        Thumb,
        Small,
        Big
    }

    public enum GameScreenshotSize
    {
        Medium,
        Big,
        Huge
    }
}
