using YouTubeViewers.WPF.Models;
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
            YouTubeViewer youTubeViewer = _youTubeViewersListingItemViewModel.YouTubeViewer;
            EditYouTubeViewerViewModel editYouTubeViewerViewModel = new EditYouTubeViewerViewModel(youTubeViewer, _modalNavigationStore);
            _modalNavigationStore.CurrentViewModel = editYouTubeViewerViewModel;
        }
    }
}
