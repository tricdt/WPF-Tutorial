using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NavigationMVVM.HostBuilders;
using NavigationMVVM.Services;
using NavigationMVVM.ViewModels;
using System.Windows;
namespace NavigationMVVM
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

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            INavigationService navigationService = _host.Services.GetRequiredService<LayoutNavigationService<HomeViewModel>>();
            navigationService.Navigate();
            base.OnStartup(e);
        }
    }

}
