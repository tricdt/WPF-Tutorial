using Microsoft.Extensions.DependencyInjection;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.FinancialModelingPrepAPI;
using SimpleTrader.FinancialModelingPrepAPI.Models;
using SimpleTrader.FinancialModelingPrepAPI.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Net.Http;
using System.Windows;
namespace SimpleTrader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        string _api_key = "UbaYiNzmGPpOt4JR3965DmMzL4664AlI";
        string baseUrl = "https://financialmodelingprep.com/api/v3/";
        protected override async void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();
            //IDataService<Account> accountService = new AccountDataService(new EntityFramework.SimpleTraderDbContextFactory());
            //IStockPriceService stockPriceService = new StockPriceService(new FinancialModelingPrepAPI.FinancialModelingPrepHttpClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(baseUrl) }, new FinancialModelingPrepAPI.Models.FinancialModelingPrepAPIKey(_api_key)));
            //IBuyStockService buyStockService = new BuyStockService(stockPriceService, accountService);
            IDataService<Account> accountService = serviceProvider.GetRequiredService<IDataService<Account>>();
            IStockPriceService stockPriceService = serviceProvider.GetRequiredService<IStockPriceService>();
            IBuyStockService buyStockService = serviceProvider.GetRequiredService<IBuyStockService>();
            Account buyer = await accountService.Get(1);
            buyStockService.BuyStock(buyer, "AAPL", 1);
            Window window = serviceProvider.GetService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }
        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<HttpClient>(s => new HttpClient() { BaseAddress = new Uri(baseUrl) });
            services.AddSingleton<FinancialModelingPrepAPIKey>(s => new FinancialModelingPrepAPIKey(_api_key));
            services.AddSingleton<FinancialModelingPrepHttpClient>();
            services.AddSingleton<SimpleTraderDbContextFactory>();
            services.AddSingleton<IDataService<Account>, AccountDataService>();
            services.AddSingleton<IStockPriceService, StockPriceService>();
            services.AddSingleton<IBuyStockService, BuyStockService>();
            services.AddSingleton<IMajorIndexService, MajorIndexService>();
            services.AddSingleton<ISimpleTraderViewModelFactory<PortfolioViewModel>, PortfolioViewModelFactory>();
            services.AddSingleton<ISimpleTraderViewModelFactory<HomeViewModel>, HomeViewModelFactory>();
            services.AddSingleton<ISimpleTraderViewModelFactory<MajorIndexListingViewModel>, MajorIndexListingViewModelFactory>();
            services.AddSingleton<ISimpleTraderViewModelAbstractFactory, SimpleTraderViewModelAbstractFactory>();
            services.AddScoped<INavigator, Navigator>();
            services.AddScoped<MainViewModel>();
            services.AddScoped<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainViewModel>()
            });
            return services.BuildServiceProvider();
        }
    }

}
