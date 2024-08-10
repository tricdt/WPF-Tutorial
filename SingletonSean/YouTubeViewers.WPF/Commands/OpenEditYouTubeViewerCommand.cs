using YouTubeViewers.WPF.Stores;
using YouTubeViewers.WPF.ViewModels;

namespace YouTubeViewers.WPF.Commands
{
    public class OpenEditYouTubeViewerCommand : CommandBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly YouTubeViewersListingItemViewModel _youTubeViewersListingItemViewModel;
        public OpenEditYouTubeViewerCommand(YouTubeViewersListingItemViewModel youTubeViewersListingItemViewModel, ModalNavigationStore modalNavigationStore)
        {
            _modalNavigationStore = modalNavigationStore;
            _youTubeViewersListingItemViewModel = youTubeViewersListingItemViewModel;
        }

        public override void Execute(object parameter)
        {
            EditYouTubeViewerViewModel editYouTubeViewerViewModel = new EditYouTubeViewerViewModel();
            _modalNavigationStore.CurrentViewModel = editYouTubeViewerViewModel;
        }
    }
}
