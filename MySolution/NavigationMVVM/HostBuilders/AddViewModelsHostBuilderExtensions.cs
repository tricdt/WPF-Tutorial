using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NavigationMVVM.Services;
using NavigationMVVM.ViewModels;

namespace NavigationMVVM.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<NavigationBarViewModel>();
                services.AddSingleton<HomeViewModel>(s => CreateHomeViewModel(s));
                services.AddSingleton<LoginViewModel>(s => CreateLoginViewModel(s));
                services.AddSingleton<CreateViewModel<HomeViewModel>>(s => () => s.GetRequiredService<HomeViewModel>());
                services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
                services.AddSingleton<CreateViewModel<NavigationBarViewModel>>(s => () => s.GetRequiredService<NavigationBarViewModel>());
            });
            return hostBuilder;
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider s)
        {
            CompositeNavigationService navigationService = new CompositeNavigationService(s.GetRequiredService<CloseModalNavigationService>(), s.GetRequiredService<LayoutNavigationService<HomeViewModel>>());

            return new LoginViewModel(navigationService);
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider s)
        {
            return new HomeViewModel(s.GetRequiredService<ModalNavigationService<LoginViewModel>>());
        }
    }
}
