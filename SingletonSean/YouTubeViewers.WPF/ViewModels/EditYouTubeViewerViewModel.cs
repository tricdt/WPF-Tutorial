using YouTubeViewers.WPF.Models;
using YouTubeViewers.WPF.Stores;
namespace YouTubeViewers.WPF.ViewModels
{
    public class EditYouTubeViewerViewModel : ViewModelBase
    {
        public YouTubeViewerDetailsFormViewModel YouTubeViewerDetailsFormViewModel { get; }
        public EditYouTubeViewerViewModel(YouTubeViewer youTubeViewer, ModalNavigationStore _modalNavigationStore)
        {

        }
    }
}
