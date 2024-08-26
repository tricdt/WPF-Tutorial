using Microsoft.Extensions.Hosting;

namespace NavigationMVVM.HostBuilders
{
    public static class AddConfigurationHostBuilderExtensions
    {
        public static IHostBuilder AddConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration(c =>
            {

            });
            return hostBuilder;
        }
    }
}
