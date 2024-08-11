using YouTubeViewers.Domain.Models;

namespace YouTubeViewers.Domain.Commands
{
    public interface IUpdateYouTubeViewerCommand
    {
        Task Execute(YouTubeViewer youTubeViewer);
    }
}
