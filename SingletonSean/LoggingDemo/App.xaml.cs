using LoggingDemo.Commands;
using LoggingDemo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            .ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Error);
                builder.AddFilter("LoggingDemo.Commands", LogLevel.Debug);
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<ICommand, MakeSandwichCommand>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>(s => new MainWindow()
                {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            //ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.ClearProviders();
            //    builder.AddDebug();
            //    builder.SetMinimumLevel(LogLevel.Error);
            //    builder.AddFilter("LoggingDemo.Commands", LogLevel.Debug);
            //});
            _host.Start();
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();


            base.OnStartup(e);
        }

    }

}
