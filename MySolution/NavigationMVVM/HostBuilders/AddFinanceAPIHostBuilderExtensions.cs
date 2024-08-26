using Microsoft.Extensions.Hosting;

namespace NavigationMVVM.HostBuilders
{
    public static class AddFinanceAPIHostBuilderExtensions
    {
        public static IHostBuilder AddFinanceAPI(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {

            });
            return hostBuilder;
        }
    }
}
