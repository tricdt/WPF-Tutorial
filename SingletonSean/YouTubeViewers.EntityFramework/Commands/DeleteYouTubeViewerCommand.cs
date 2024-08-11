using YouTubeViewers.Domain.Commands;
using YouTubeViewers.EntityFramework.DTOs;

namespace YouTubeViewers.EntityFramework.Commands
{
    public class DeleteYouTubeViewerCommand : IDeleteYouTubeViewerCommand
    {
        private readonly YouTubeViewersDbContextFactory _contextFactory;
        public DeleteYouTubeViewerCommand(YouTubeViewersDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task Execute(Guid id)
        {
            using (YouTubeViewersDbContext context = _contextFactory.Create())
            {
                YouTubeViewerDTO youTubeViewerDTO = new YouTubeViewerDTO()
                {
                    Id = id,
                };

                context.YouTubeViewers.Remove(youTubeViewerDTO);
                await context.SaveChangesAsync();
            }
        }
    }
}
