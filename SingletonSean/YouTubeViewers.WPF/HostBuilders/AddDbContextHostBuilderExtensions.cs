using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YouTubeViewers.EntityFramework;

namespace YouTubeViewers.WPF.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                string connectionString = context.Configuration.GetConnectionString("sqlserver");
                services.AddSingleton<DbContextOptions>(new DbContextOptionsBuilder().UseSqlServer(connectionString).Options);
                services.AddSingleton<YouTubeViewersDbContextFactory>();
            });
            return hostBuilder;
        }
    }
}
