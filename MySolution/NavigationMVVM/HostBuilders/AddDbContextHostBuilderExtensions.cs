using Microsoft.Extensions.Hosting;

namespace NavigationMVVM.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {

            });
            return hostBuilder;
        }
    }
}
