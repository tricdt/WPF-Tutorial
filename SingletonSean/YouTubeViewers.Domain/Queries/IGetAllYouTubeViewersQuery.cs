using YouTubeViewers.Domain.Models;

namespace YouTubeViewers.Domain.Queries
{
    public interface IGetAllYouTubeViewersQuery
    {
        Task<IEnumerable<YouTubeViewer>> Execute();
    }
}
