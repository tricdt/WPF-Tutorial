using Microsoft.Extensions.Hosting;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddStoresHostBuilderExtensions
    {
        public static IHostBuilder AddStores(this IHostBuilder host)
        {
            host.ConfigureServices(services => { });
            return host;
        }
    }
}
