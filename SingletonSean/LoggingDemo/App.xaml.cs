using LoggingDemo.Commands;
using LoggingDemo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;
using System.Windows.Input;
namespace LoggingDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .UseSerilog((host, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .WriteTo.Debug()
                        .WriteTo.File("test.txt", rollingInterval: RollingInterval.Day)
                        .MinimumLevel.Error()
                        .MinimumLevel.Override("LoggingDemo.Commands", Serilog.Events.LogEventLevel.Debug);
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ICommand, MakeSandwichCommand>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                }).Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            _host.Start();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
