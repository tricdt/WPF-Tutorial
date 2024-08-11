using System.Windows.Input;
using YouTubeViewers.WPF.Commands;
using YouTubeViewers.WPF.Stores;

namespace YouTubeViewers.WPF.ViewModels
{
    public class AddYouTubeViewerViewModel : ViewModelBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        private readonly YouTubeViewersStore _youTubeViewersStore;
        public YouTubeViewerDetailsFormViewModel YouTubeViewerDetailsFormViewModel { get; }
        public AddYouTubeViewerViewModel(ModalNavigationStore modalNavigationStore, YouTubeViewersStore youTubeViewersStore)
        {
            ICommand submitCommand = new AddYouTubeViewerCommand(this, youTubeViewersStore, modalNavigationStore);
            ICommand cancelCommand = new CloseModalCommand(modalNavigationStore);
            YouTubeViewerDetailsFormViewModel = new YouTubeViewerDetailsFormViewModel(submitCommand, cancelCommand);
            _youTubeViewersStore = youTubeViewersStore;
        }
    }
}
