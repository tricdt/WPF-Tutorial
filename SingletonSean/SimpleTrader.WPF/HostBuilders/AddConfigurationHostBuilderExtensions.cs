using Microsoft.Extensions.Hosting;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder host)
        {
            host.ConfigureServices(services => { });
            return host;
        }
    }
}
