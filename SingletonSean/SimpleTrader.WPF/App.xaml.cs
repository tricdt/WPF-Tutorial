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

        protected override void OnStartup(StartupEventArgs e)
        {
            IDataService<Account> accountService = new AccountDataService(new EntityFramework.SimpleTraderDbContextFactory());
            IStockPriceService stockPriceService = new StockPriceService(new FinancialModelingPrepAPI.FinancialModelingPrepHttpClient(new System.Net.Http.HttpClient(), new FinancialModelingPrepAPI.Models.FinancialModelingPrepAPIKey("")));
            IBuyStockService buyStockService = new BuyStockService(stockPriceService, accountService);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
