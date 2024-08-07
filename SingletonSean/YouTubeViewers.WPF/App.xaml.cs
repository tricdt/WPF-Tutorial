using System.Windows;
using YouTubeViewers.WPF.ViewModels;
namespace YouTubeViewers.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(new Stores.ModalNavigationStore())
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
