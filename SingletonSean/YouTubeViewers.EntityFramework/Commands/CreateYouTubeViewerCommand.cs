using YouTubeViewers.Domain.Commands;
using YouTubeViewers.Domain.Models;
using YouTubeViewers.EntityFramework.DTOs;
namespace YouTubeViewers.EntityFramework.Commands
{
    public class CreateYouTubeViewerCommand : ICreateYouTubeViewerCommand
    {
        private readonly YouTubeViewersDbContextFactory _contextFactory;
        public CreateYouTubeViewerCommand(YouTubeViewersDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task Execute(YouTubeViewer youTubeViewer)
        {
            using (YouTubeViewersDbContext context = _contextFactory.Create())
            {
                YouTubeViewerDTO youTubeViewerDTO = new YouTubeViewerDTO()
                {
                    Id = youTubeViewer.Id,
                    Username = youTubeViewer.Username,
                    IsSubscribed = youTubeViewer.IsSubscribed,
                    IsMember = youTubeViewer.IsMember,
                };
                context.YouTubeViewers.Add(youTubeViewerDTO);
                await context.SaveChangesAsync();
            }
        }
    }
}
