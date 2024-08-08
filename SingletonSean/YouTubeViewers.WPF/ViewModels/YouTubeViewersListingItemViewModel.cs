using System.Windows.Input;
using YouTubeViewers.WPF.Models;
namespace YouTubeViewers.WPF.ViewModels
{
    public class YouTubeViewersListingItemViewModel : ViewModelBase
    {
        private YouTubeViewer _youTubeViewer;

        public YouTubeViewer YouTubeViewer
        {
            get { return _youTubeViewer; }
            set { _youTubeViewer = value; }
        }

        public string Username { get; }
        private bool _isDeleting;
        public bool IsDeleting
        {
            get
            {
                return _isDeleting;
            }
            set
            {
                _isDeleting = value;
                OnPropertyChanged(nameof(IsDeleting));
            }
        }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public YouTubeViewersListingItemViewModel(YouTubeViewer youTubeViewer)
        {
            Username = youTubeViewer.Username;
            IsDeleting = false;
        }
    }
}
