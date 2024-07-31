using LoggingDemo.Commands;
using LoggingDemo.ViewModels;
using Microsoft.Extensions.Logging;
using Serilog;
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
                LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .WriteTo.File("test.txt", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Error()
                .MinimumLevel.Override("LoggingDemo.Commands", Serilog.Events.LogEventLevel.Debug);
                ;
                builder.AddSerilog(loggerConfiguration.CreateLogger());
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
