using Microsoft.Extensions.Hosting;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddViewsHostBuilderExtensions
    {
        public static IHostBuilder AddViews(this IHostBuilder host)
        {
            host.ConfigureServices(services => { });
            return host;
        }
    }
}
