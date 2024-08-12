using System.Collections.ObjectModel;
using YouTubeViewers.Domain.Models;
using YouTubeViewers.WPF.Stores;
namespace YouTubeViewers.WPF.ViewModels
{
    public class YouTubeViewersListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<YouTubeViewersListingItemViewModel> _youTubeViewersListingItemViewModels;
        private readonly SelectedYouTubeViewerStore _selectedYouTubeViewerStore;
        private readonly YouTubeViewersStore _youTubeViewersStore;
        private readonly ModalNavigationStore _modalNavigationStore;
        public IEnumerable<YouTubeViewersListingItemViewModel> YouTubeViewersListingItemViewModels => _youTubeViewersListingItemViewModels;
        private YouTubeViewersListingItemViewModel _selectedYouTubeViewerListingItemViewModel;

        public YouTubeViewersListingItemViewModel SelectedYouTubeViewerListingItemViewModel
        {
            get { return _selectedYouTubeViewerListingItemViewModel; }
            set
            {
                _selectedYouTubeViewerListingItemViewModel = value;
                OnPropertyChanged(nameof(SelectedYouTubeViewerListingItemViewModel));
                _selectedYouTubeViewerStore.SelectedYoutubeViewer = _selectedYouTubeViewerListingItemViewModel?.YouTubeViewer;
            }
        }



        public YouTubeViewersListingViewModel(SelectedYouTubeViewerStore selectedYouTubeViewerStore,
            ModalNavigationStore modalNavigationStore, YouTubeViewersStore youTubeViewersStore)
        {
            _modalNavigationStore = modalNavigationStore;
            _selectedYouTubeViewerStore = selectedYouTubeViewerStore;
            _youTubeViewersListingItemViewModels = new ObservableCollection<YouTubeViewersListingItemViewModel>();
            _youTubeViewersStore = youTubeViewersStore;
            _youTubeViewersStore.YouTubeViewersLoaded += _youTubeViewersStore_YouTubeViewersLoaded;
            _youTubeViewersStore.YouTubeViewerAdded += _youTubeViewersStore_YouTubeViewerAdded;
            //_youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer(new Guid(), "Sala", true, false), modalNavigationStore));
            //_youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer(new Guid(), "SingletonSean", false, true), modalNavigationStore));
            //_youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer(new Guid(), "TriNguyen", true, false), modalNavigationStore));
        }

        private void _youTubeViewersStore_YouTubeViewerAdded(YouTubeViewer youTubeViewer)
        {
            AddYouTubeViewer(youTubeViewer);
        }

        private void _youTubeViewersStore_YouTubeViewersLoaded()
        {
            _youTubeViewersListingItemViewModels.Clear();

            foreach (YouTubeViewer youTubeViewer in _youTubeViewersStore.YouTubeViewers)
            {
                AddYouTubeViewer(youTubeViewer);
            }
        }
        private void AddYouTubeViewer(YouTubeViewer youTubeViewer)
        {
            YouTubeViewersListingItemViewModel itemViewModel =
                new YouTubeViewersListingItemViewModel(youTubeViewer, _modalNavigationStore);
            _youTubeViewersListingItemViewModels.Add(itemViewModel);
        }
    }
}
