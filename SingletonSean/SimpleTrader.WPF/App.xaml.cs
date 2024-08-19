using Microsoft.Extensions.DependencyInjection;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.FinancialModelingPrepAPI.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
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
            IDataService<Account> accountService = new AccountDataService(new EntityFramework.SimpleTraderDbContextFactory());
            IStockPriceService stockPriceService = new StockPriceService(new FinancialModelingPrepAPI.FinancialModelingPrepHttpClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(baseUrl) }, new FinancialModelingPrepAPI.Models.FinancialModelingPrepAPIKey(_api_key)));
            IBuyStockService buyStockService = new BuyStockService(stockPriceService, accountService);
            Account buyer = await accountService.Get(1);
            buyStockService.BuyStock(buyer, "AAPL", 1);
            Window window = serviceProvider.GetService<MainWindow>();
            window.Show();
            //MainWindow = new MainWindow()
            //{
            //    DataContext = new MainViewModel(new Navigator())
            //};
            //MainWindow.Show();
            base.OnStartup(e);
        }
        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();
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
