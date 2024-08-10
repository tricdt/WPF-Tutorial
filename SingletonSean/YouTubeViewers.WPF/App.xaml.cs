using System.Windows;
using YouTubeViewers.WPF.Stores;
using YouTubeViewers.WPF.ViewModels;
namespace YouTubeViewers.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly SelectedYouTubeViewerStore _selectedYouTubeViewerStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        public App()
        {
            _selectedYouTubeViewerStore = new SelectedYouTubeViewerStore();
            _modalNavigationStore = new ModalNavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_modalNavigationStore, new YouTubeViewersViewModel(_selectedYouTubeViewerStore, _modalNavigationStore))
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
