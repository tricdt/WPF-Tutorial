using System.Windows.Input;
using YouTubeViewers.WPF.Commands;
using YouTubeViewers.WPF.Stores;

namespace YouTubeViewers.WPF.ViewModels
{
    public class AddYouTubeViewerViewModel : ViewModelBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        public YouTubeViewerDetailsFormViewModel YouTubeViewerDetailsFormViewModel { get; }
        public AddYouTubeViewerViewModel(ModalNavigationStore modalNavigationStore)
        {
            ICommand submitCommand = new AddYouTubeViewerCommand();
            ICommand cancelCommand = new CloseModalCommand(modalNavigationStore);
            YouTubeViewerDetailsFormViewModel = new YouTubeViewerDetailsFormViewModel(submitCommand, cancelCommand);
        }
    }
}
