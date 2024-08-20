using Microsoft.Extensions.Hosting;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddDbContextHostBuilderExtensions
    {
        public static IHostBuilder AddDbContext(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) => { });
            return host;
        }
    }
}
