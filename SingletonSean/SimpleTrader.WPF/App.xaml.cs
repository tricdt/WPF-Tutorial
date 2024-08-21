using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleTrader.Domain.Services;
using SimpleTrader.WPF.HostBuilders;
using SimpleTrader.WPF.ViewModels;
using System.Windows;
namespace SimpleTrader.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = CreateHostBuilder().Build();
        }
        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .AddConfiguration()
                .AddFinanceAPI()
                .AddDbContext()
                .AddServices()
                .AddStores()
                .AddViewModels()
                .AddViews();
        }
        string _api_key = "UbaYiNzmGPpOt4JR3965DmMzL4664AlI";
        string baseUrl = "https://financialmodelingprep.com/api/v3/";
        protected override async void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            //IServiceProvider serviceProvider = CreateServiceProvider();
            //IDataService<Account> accountService = new AccountDataService(new EntityFramework.SimpleTraderDbContextFactory());
            //IStockPriceService stockPriceService = new StockPriceService(new FinancialModelingPrepAPI.FinancialModelingPrepHttpClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(baseUrl) }, new FinancialModelingPrepAPI.Models.FinancialModelingPrepAPIKey(_api_key)));
            //IBuyStockService buyStockService = new BuyStockService(stockPriceService, accountService);
            //IDataService<Account> accountService = serviceProvider.GetRequiredService<IDataService<Account>>();
            //IStockPriceService stockPriceService = serviceProvider.GetRequiredService<IStockPriceService>();
            //IBuyStockService buyStockService = serviceProvider.GetRequiredService<IBuyStockService>();
            //Account buyer = await accountService.Get(1);
            //buyStockService.BuyStock(buyer, "AAPL", 1);
            Window window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();
            base.OnStartup(e);
        }


        private HomeViewModel CreateHomeViewModel(IServiceProvider service)
        {
            return new HomeViewModel(MajorIndexListingViewModel.LoadMajorIndexViewModel(service.GetRequiredService<IMajorIndexService>()));
        }
    }

}
