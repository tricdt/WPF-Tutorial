using YouTubeViewers.WPF.Stores;
using YouTubeViewers.WPF.ViewModels;
namespace YouTubeViewers.WPF.Commands
{
    public class OpenAddYouTubeViewerCommand : CommandBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly YouTubeViewersStore _youTubeViewersStore;
        public OpenAddYouTubeViewerCommand(ModalNavigationStore modalNavigationStore, YouTubeViewersStore youTubeViewersStore)
        {
            _modalNavigationStore = modalNavigationStore;
            _youTubeViewersStore = youTubeViewersStore;
        }

        public override void Execute(object parameter)
        {
            AddYouTubeViewerViewModel addYouTubeViewerViewModel = new AddYouTubeViewerViewModel(_modalNavigationStore, _youTubeViewersStore);
            _modalNavigationStore.CurrentViewModel = addYouTubeViewerViewModel;
        }
    }
}
