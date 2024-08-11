namespace YouTubeViewers.Domain.Commands
{
    public interface IDeleteYouTubeViewerCommand
    {
        Task Execute(Guid id);
    }
}
