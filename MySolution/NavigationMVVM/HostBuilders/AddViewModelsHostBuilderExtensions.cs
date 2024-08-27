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
                services.AddSingleton<NavigationBarViewModel>(s => CreateNavigationBarViewModel(s));
                services.AddSingleton<AccountViewModel>();
                services.AddSingleton<HomeViewModel>(s => CreateHomeViewModel(s));
                services.AddSingleton<LoginViewModel>(s => CreateLoginViewModel(s));
                services.AddSingleton<PeopleListingViewModel>();
                services.AddSingleton<CreateViewModel<HomeViewModel>>(s => () => s.GetRequiredService<HomeViewModel>());
                services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
                services.AddSingleton<CreateViewModel<AccountViewModel>>(s => () => s.GetRequiredService<AccountViewModel>());
                services.AddSingleton<CreateViewModel<NavigationBarViewModel>>(s => () => s.GetRequiredService<NavigationBarViewModel>());
                services.AddSingleton<CreateViewModel<PeopleListingViewModel>>(s => () => s.GetRequiredService<PeopleListingViewModel>());
            });
            return hostBuilder;
        }

        private static NavigationBarViewModel CreateNavigationBarViewModel(IServiceProvider s)
        {
            return new NavigationBarViewModel(s.GetRequiredService<LayoutNavigationService<HomeViewModel>>(),
                s.GetRequiredService<LayoutNavigationService<AccountViewModel>>(), s.GetRequiredService<ModalNavigationService<LoginViewModel>>(),
                s.GetRequiredService<LayoutNavigationService<PeopleListingViewModel>>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider s)
        {
            CompositeNavigationService navigationService = new CompositeNavigationService(s.GetRequiredService<CloseModalNavigationService>(), s.GetRequiredService<LayoutNavigationService<AccountViewModel>>());

            return new LoginViewModel(navigationService);
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider s)
        {
            return new HomeViewModel(s.GetRequiredService<ModalNavigationService<LoginViewModel>>());
        }
    }
}
