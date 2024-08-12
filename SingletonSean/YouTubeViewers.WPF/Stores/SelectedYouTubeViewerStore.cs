using YouTubeViewers.Domain.Models;

namespace YouTubeViewers.WPF.Stores
{
    public class SelectedYouTubeViewerStore
    {
        private readonly YouTubeViewersStore _youTubeViewersStore;
        private YouTubeViewer _selectedYoutubeViewer;

        public SelectedYouTubeViewerStore(YouTubeViewersStore youTubeViewersStore)
        {
            _youTubeViewersStore = youTubeViewersStore;
            _youTubeViewersStore.YouTubeViewerAdded += YouTubeViewersStore_YouTubeViewerAdded;
        }

        private void YouTubeViewersStore_YouTubeViewerAdded(YouTubeViewer youTubeViewer)
        {
            SelectedYoutubeViewer = youTubeViewer;
        }

        public YouTubeViewer SelectedYoutubeViewer
        {
            get { return _selectedYoutubeViewer; }
            set
            {
                _selectedYoutubeViewer = value;
                SelectedYouTubeViewerChanged?.Invoke();
            }
        }
        public event Action SelectedYouTubeViewerChanged;
    }
}
