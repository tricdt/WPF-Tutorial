using System.Collections.ObjectModel;

namespace YouTubeViewers.WPF.ViewModels
{
    public class YouTubeViewersListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<YouTubeViewersListingItemViewModel> _youTubeViewersListingItemViewModels;
        public IEnumerable<YouTubeViewersListingItemViewModel> YouTubeViewersListingItemViewModels => _youTubeViewersListingItemViewModels;
        public YouTubeViewersListingViewModel()
        {
            _youTubeViewersListingItemViewModels = new ObservableCollection<YouTubeViewersListingItemViewModel>();
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel("Sala"));
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel("Sala1"));
            _youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel("Sala2"));
        }
    }
}
