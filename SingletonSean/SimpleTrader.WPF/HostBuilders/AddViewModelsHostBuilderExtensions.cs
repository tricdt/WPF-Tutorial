using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.Domain.Services;
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
                services.AddSingleton((Func<IServiceProvider, CreateViewModel<PortfolioViewModel>>)(s => () => s.GetRequiredService<PortfolioViewModel>()));
                services.AddSingleton((Func<IServiceProvider, CreateViewModel<HomeViewModel>>)(s => () => s.GetRequiredService<HomeViewModel>()));
                services.AddSingleton((Func<IServiceProvider, CreateViewModel<BuyViewModel>>)(s => () => s.GetRequiredService<BuyViewModel>()));
                services.AddSingleton<CreateViewModel<SellViewModel>>(s => () => s.GetRequiredService<SellViewModel>());
                services.AddSingleton<CreateViewModel<LoginViewModel>>(s => () => s.GetRequiredService<LoginViewModel>());
                services.AddSingleton<ISimpleTraderViewModelFactory, SimpleTraderViewModelFactory>();
            });
            return host;
        }



        private static HomeViewModel CreateHomeViewModel(IServiceProvider service)
        {
            return new HomeViewModel(MajorIndexListingViewModel.LoadMajorIndexViewModel(service.GetRequiredService<IMajorIndexService>()));
        }
    }
}
