using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.Domain.Services.TransactionServices;
using SimpleTrader.EntityFramework.Services;
using SimpleTrader.FinancialModelingPrepAPI.Services;
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
            IDataService<Account> accountService = new AccountDataService(new EntityFramework.SimpleTraderDbContextFactory());
            IStockPriceService stockPriceService = new StockPriceService(new FinancialModelingPrepAPI.FinancialModelingPrepHttpClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(baseUrl) }, new FinancialModelingPrepAPI.Models.FinancialModelingPrepAPIKey(_api_key)));
            IBuyStockService buyStockService = new BuyStockService(stockPriceService, accountService);
            Account buyer = await accountService.Get(1);
            buyStockService.BuyStock(buyer, "AAPL", 1);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
