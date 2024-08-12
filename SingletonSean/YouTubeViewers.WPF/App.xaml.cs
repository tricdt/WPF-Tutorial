using Microsoft.EntityFrameworkCore;
using System.Windows;
using YouTubeViewers.EntityFramework;
using YouTubeViewers.EntityFramework.Commands;
using YouTubeViewers.EntityFramework.Queries;
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
        private readonly YouTubeViewersDbContextFactory _youtubeViewersDbContextFactory;
        private readonly YouTubeViewersStore _youtubeViewersStore;
        public App()
        {
            string CONNECTION_STRING = "Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=YoutubeViewer;Integrated Security=True;Trust Server Certificate=True";
            _youtubeViewersDbContextFactory = new YouTubeViewersDbContextFactory(new DbContextOptionsBuilder().UseSqlServer(CONNECTION_STRING).Options);
            _modalNavigationStore = new ModalNavigationStore();
            _youtubeViewersStore = new YouTubeViewersStore(new GetAllYouTubeViewersQuery(_youtubeViewersDbContextFactory),
                new CreateYouTubeViewerCommand(_youtubeViewersDbContextFactory),
                new UpdateYouTubeViewerCommand(_youtubeViewersDbContextFactory),
                new DeleteYouTubeViewerCommand(_youtubeViewersDbContextFactory));
            _selectedYouTubeViewerStore = new SelectedYouTubeViewerStore(_youtubeViewersStore);

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            using (YouTubeViewersDbContext context = _youtubeViewersDbContextFactory.Create())
            {
                context.Database.Migrate();
            }

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_modalNavigationStore, YouTubeViewersViewModel.LoadViewModel(_selectedYouTubeViewerStore, _modalNavigationStore, _youtubeViewersStore))
            };
            MainWindow.Show();
            base.OnStartup(e);
        }
    }

}
