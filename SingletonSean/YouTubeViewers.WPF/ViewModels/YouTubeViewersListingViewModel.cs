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
            get
            {
                return _youTubeViewersListingItemViewModels
                    .FirstOrDefault(y => y.YouTubeViewer?.Id == _selectedYouTubeViewerStore.SelectedYoutubeViewer?.Id);
            }
            set
            {
                _selectedYouTubeViewerStore.SelectedYoutubeViewer = value?.YouTubeViewer;
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
            _youTubeViewersStore.YouTubeViewerDeleted += YouTubeViewersStore_YouTubeViewerDeleted;
            _youTubeViewersListingItemViewModels.CollectionChanged += YouTubeViewersListingItemViewModels_CollectionChanged;
            _selectedYouTubeViewerStore.SelectedYouTubeViewerChanged += SelectedYouTubeViewerStore_SelectedYouTubeViewerChanged;
            //_youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer(new Guid(), "Sala", true, false), modalNavigationStore));
            //_youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer(new Guid(), "SingletonSean", false, true), modalNavigationStore));
            //_youTubeViewersListingItemViewModels.Add(new YouTubeViewersListingItemViewModel(new YouTubeViewer(new Guid(), "TriNguyen", true, false), modalNavigationStore));
        }

        private void YouTubeViewersStore_YouTubeViewerDeleted(Guid id)
        {
            YouTubeViewersListingItemViewModel itemViewModel = _youTubeViewersListingItemViewModels.FirstOrDefault(y => y.YouTubeViewer?.Id == id);

            if (itemViewModel != null)
            {
                _youTubeViewersListingItemViewModels.Remove(itemViewModel);
            }
        }

        private void SelectedYouTubeViewerStore_SelectedYouTubeViewerChanged()
        {
            OnPropertyChanged(nameof(SelectedYouTubeViewerListingItemViewModel));
        }

        private void YouTubeViewersListingItemViewModels_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedYouTubeViewerListingItemViewModel));
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
                new YouTubeViewersListingItemViewModel(youTubeViewer, _modalNavigationStore, _youTubeViewersStore);
            _youTubeViewersListingItemViewModels.Add(itemViewModel);
        }
    }
}
