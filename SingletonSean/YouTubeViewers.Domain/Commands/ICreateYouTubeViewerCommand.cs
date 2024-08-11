using YouTubeViewers.Domain.Models;

namespace YouTubeViewers.Domain.Commands
{
    public interface ICreateYouTubeViewerCommand
    {
        Task Execute(YouTubeViewer youTubeViewer);
    }
}
