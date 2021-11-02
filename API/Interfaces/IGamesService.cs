using API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IGamesService
    {
        Task<VideoGame> GetGameAsync(long id);
        Task<VideoGameDetails> GetGameDetailsAsync(long id);
        string GetImageUrl(string imageId, GameCoverSize size);
        string GetImageUrl(string screenshotId, GameScreenshotSize size);
        Task<IEnumerable<VideoGame>> SearchGamesAsync(string searchString);
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
