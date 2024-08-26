using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NavigationMVVM.Services;
using NavigationMVVM.ViewModels;

namespace NavigationMVVM.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<LayoutNavigationService<HomeViewModel>>();
                services.AddSingleton<ModalNavigationService<LoginViewModel>>();
                services.AddSingleton<CloseModalNavigationService>();
            });
            return hostBuilder;
        }
    }
}
