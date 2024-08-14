using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using YouTubeViewers.Domain.Commands;
using YouTubeViewers.Domain.Queries;
using YouTubeViewers.EntityFramework;
using YouTubeViewers.EntityFramework.Commands;
using YouTubeViewers.EntityFramework.Queries;
using YouTubeViewers.WPF.HostBuilders;
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

        private readonly IHost _host;
        public App()
        {
            string CONNECTION_STRING = "Data Source=ODEGAARD\\SQLEXPRESS;Initial Catalog=YoutubeViewer;Integrated Security=True;Trust Server Certificate=True";
            _youtubeViewersStore = new YouTubeViewersStore(new GetAllYouTubeViewersQuery(_youtubeViewersDbContextFactory),
                new CreateYouTubeViewerCommand(_youtubeViewersDbContextFactory),
                new UpdateYouTubeViewerCommand(_youtubeViewersDbContextFactory),
                new DeleteYouTubeViewerCommand(_youtubeViewersDbContextFactory));
            _selectedYouTubeViewerStore = new SelectedYouTubeViewerStore(_youtubeViewersStore);

            _host = Host.CreateDefaultBuilder().AddDbContext().ConfigureServices((context, services) =>
            {
                services.AddSingleton<IGetAllYouTubeViewersQuery, GetAllYouTubeViewersQuery>();
                services.AddSingleton<ICreateYouTubeViewerCommand, CreateYouTubeViewerCommand>();
                services.AddSingleton<IUpdateYouTubeViewerCommand, UpdateYouTubeViewerCommand>();
                services.AddSingleton<IDeleteYouTubeViewerCommand, DeleteYouTubeViewerCommand>();
                services.AddSingleton<SelectedYouTubeViewerStore>();
                services.AddSingleton<YouTubeViewersStore>();
                services.AddSingleton<ModalNavigationStore>();
                services.AddSingleton<MainViewModel>();
                services.AddTransient<YouTubeViewersViewModel>(CreateYouTubeViewersViewModel);
                services.AddSingleton<MainWindow>(services => new MainWindow()
                {
                    DataContext = services.GetRequiredService<MainViewModel>()
                });
            }).Build();

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            YouTubeViewersDbContextFactory youTubeViewersDbContextFactory =
                _host.Services.GetRequiredService<YouTubeViewersDbContextFactory>();
            using (YouTubeViewersDbContext context = youTubeViewersDbContextFactory.Create())
            {
                context.Database.Migrate();
            }


            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
            base.OnStartup(e);
        }
        private YouTubeViewersViewModel CreateYouTubeViewersViewModel(IServiceProvider services)
        {
            return YouTubeViewersViewModel.LoadViewModel(services.GetRequiredService<SelectedYouTubeViewerStore>(),
                services.GetRequiredService<ModalNavigationStore>(), services.GetRequiredService<YouTubeViewersStore>());

        }
    }

}
