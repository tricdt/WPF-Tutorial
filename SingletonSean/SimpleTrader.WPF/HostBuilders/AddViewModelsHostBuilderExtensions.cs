using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.Domain.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;

namespace SimpleTrader.WPF.HostBuilders
{
    public static class AddViewModelsHostBuilderExtensions
    {

        public static IHostBuilder AddViewModels(this IHostBuilder host)
        {
            host.ConfigureServices(services =>
            {
                services.AddSingleton<PortfolioViewModel>();
                services.AddSingleton<MajorIndexListingViewModel>();
                services.AddSingleton<HomeViewModel>(CreateHomeViewModel);
                services.AddSingleton<BuyViewModel>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<SellViewModel>();
                services.AddSingleton<CreateViewModel<PortfolioViewModel>>(s => () => s.GetRequiredService<PortfolioViewModel>());
                services.AddSingleton<CreateViewModel<HomeViewModel>>(s => () => s.GetRequiredService<HomeViewModel>());
                services.AddSingleton<CreateViewModel<BuyViewModel>>(s => () => s.GetRequiredService<BuyViewModel>());
                services.AddSingleton<CreateViewModel<SellViewModel>>(s => () => s.GetRequiredService<SellViewModel>());
                services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => CreateLoginViewModel(s));
                services.AddSingleton<ISimpleTraderViewModelFactory, SimpleTraderViewModelFactory>();
                services.AddSingleton<ViewModelDelegateRenavigator<HomeViewModel>>();
                services.AddSingleton<CreateViewModel<RegisterViewModel>>(s => () => CreateRegisterViewModel(s));
                services.AddSingleton<ViewModelDelegateRenavigator<RegisterViewModel>>();
                services.AddSingleton<ViewModelDelegateRenavigator<LoginViewModel>>();
            });
            return host;
        }

        private static RegisterViewModel CreateRegisterViewModel(IServiceProvider service)
        {
            return new RegisterViewModel(service.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>(),
                service.GetRequiredService<ViewModelDelegateRenavigator<LoginViewModel>>());
        }

        private static LoginViewModel CreateLoginViewModel(IServiceProvider service)
        {
            return new LoginViewModel(
                service.GetRequiredService<ViewModelDelegateRenavigator<HomeViewModel>>(),
                service.GetRequiredService<ViewModelDelegateRenavigator<RegisterViewModel>>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider service)
        {
            return new HomeViewModel(MajorIndexListingViewModel.LoadMajorIndexViewModel(service.GetRequiredService<IMajorIndexService>()));
        }
    }
}
