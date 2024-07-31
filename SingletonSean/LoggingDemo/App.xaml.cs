using LoggingDemo.Commands;
using LoggingDemo.ViewModels;
using System.Windows;
namespace LoggingDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        MakeSandwichCommand makeSandwichCommand = new MakeSandwichCommand();
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(makeSandwichCommand)
            };
            MainWindow.Show();


            base.OnStartup(e);
        }

    }

}
