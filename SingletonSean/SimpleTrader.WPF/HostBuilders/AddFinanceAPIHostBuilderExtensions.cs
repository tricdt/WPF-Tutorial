using Microsoft.Extensions.Hosting;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddFinanceAPIHostBuilderExtensions
    {
        public static IHostBuilder AddFinanceAPI(this IHostBuilder host)
        {
            host.ConfigureServices((context, services) =>
            {

            });
            return host;
        }
    }
}
