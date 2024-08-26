using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using NavigationMVVM.ViewModels;

namespace NavigationMVVM.HostBuilders
{
    public static class AddServicesHostBuilderExtensions
    {
        public static IHostBuilder AddServices(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<INavigationService>(s => CreateHomeNavigationService(s));
            });
            return hostBuilder;
        }

        private static INavigationService CreateHomeNavigationService(IServiceProvider s)
        {
            return new NavigationService<HomeViewModel>(s.GetRequiredService<NavigationStore>(), s.GetRequiredService<CreateViewModel<HomeViewModel>>());
        }
    }
}
