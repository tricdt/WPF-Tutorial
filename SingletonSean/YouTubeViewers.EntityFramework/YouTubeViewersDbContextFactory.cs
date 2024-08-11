using Microsoft.EntityFrameworkCore;

namespace YouTubeViewers.EntityFramework
{
    public class YouTubeViewersDbContextFactory
    {
        private readonly DbContextOptions _options;

        public YouTubeViewersDbContextFactory(DbContextOptions options)
        {
            _options = options;
        }

        public YouTubeViewersDbContext Create()
        {
            return new YouTubeViewersDbContext(_options);
        }
    }
}
