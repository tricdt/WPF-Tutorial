using YouTubeViewers.Domain.Models;

namespace YouTubeViewers.WPF.Stores
{
    public class SelectedYouTubeViewerStore
    {
        private YouTubeViewer _selectedYoutubeViewer;

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
