using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace YouTubeViewers.EntityFramework
{
    public class YouTubeViewersDesignTimeDbContextFactory : IDesignTimeDbContextFactory<YouTubeViewersDbContext>
    {
        string CONNECTION_STRING = "Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=YoutubeViewer;Integrated Security=True;Trust Server Certificate=True";
        public YouTubeViewersDbContext CreateDbContext(string[] args = null)
        {
            return new YouTubeViewersDbContext(new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options);
        }
    }
}
