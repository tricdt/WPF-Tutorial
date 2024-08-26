using Microsoft.Extensions.Hosting;

namespace NavigationMVVM.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {

            });
            return hostBuilder;
        }
    }
}
