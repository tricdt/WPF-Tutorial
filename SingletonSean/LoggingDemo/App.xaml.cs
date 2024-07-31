using LoggingDemo.Commands;
using LoggingDemo.ViewModels;
using Microsoft.Extensions.Logging;
using System.Windows;
namespace LoggingDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Error);
                builder.AddFilter("LoggingDemo.Commands", LogLevel.Debug);
            });
            ILogger<MakeSandwichCommand> makeSandwichCommandLogger = loggerFactory.CreateLogger<MakeSandwichCommand>();
            MakeSandwichCommand makeSandwichCommand = new MakeSandwichCommand(makeSandwichCommandLogger);
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(makeSandwichCommand)
            };
            MainWindow.Show();


            base.OnStartup(e);
        }

    }

}
