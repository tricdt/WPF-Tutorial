using SimpleViewModels.Commands;
using SimpleViewModels.Services;
using SimpleViewModels.ViewModels;
using System.Windows;
namespace SimpleViewModels
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            PriceService priceService = new PriceService();
            int currentMinute = 11;
            CreateCommand<BuyViewModel> createBuyCommand;
            CreateCommand<BuyViewModel> createCalculatePriceCommand;
            if (currentMinute % 2 == 1)
            {
                createCalculatePriceCommand = (vm) => new CalculatePriceCommand(vm, priceService);
                createBuyCommand = (vm) => new BuyCommand(vm, priceService);
            }
            else
            {
                CreateCommand<BuyViewModel> createStoreClosedCommand = (vm) => new StoreClosedCommand(vm);

                createCalculatePriceCommand = createStoreClosedCommand;
                createBuyCommand = createStoreClosedCommand;
            }
            BuyViewModel initialViewModel = new BuyViewModel(createCalculatePriceCommand, createBuyCommand);

            MainWindow = new MainWindow()
            {
                DataContext = initialViewModel
            };

            MainWindow.Show();

            base.OnStartup(e);
        }


    }

}
