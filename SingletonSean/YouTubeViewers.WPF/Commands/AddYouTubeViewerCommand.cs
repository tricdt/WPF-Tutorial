
using YouTubeViewers.Domain.Models;
using YouTubeViewers.WPF.Stores;
using YouTubeViewers.WPF.ViewModels;

namespace YouTubeViewers.WPF.Commands
{
    public class AddYouTubeViewerCommand : AsyncCommandBase
    {
        private readonly AddYouTubeViewerViewModel _addYouTubeViewerViewModel;
        private readonly YouTubeViewersStore _youTubeViewersStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public AddYouTubeViewerCommand(AddYouTubeViewerViewModel addYouTubeViewerViewModel,
            YouTubeViewersStore youTubeViewersStore,
            ModalNavigationStore modalNavigationStore)
        {
            _addYouTubeViewerViewModel = addYouTubeViewerViewModel;
            _youTubeViewersStore = youTubeViewersStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            YouTubeViewerDetailsFormViewModel formViewModel = _addYouTubeViewerViewModel.YouTubeViewerDetailsFormViewModel;
            formViewModel.IsSubmitting = true;
            formViewModel.ErrorMessage = null;
            YouTubeViewer youTubeViewer = new YouTubeViewer(Guid.NewGuid(), formViewModel.Username, formViewModel.IsSubscribed, formViewModel.IsMember);
            try
            {
                await Task.Delay(5000);
                await _youTubeViewersStore.Add(youTubeViewer);
                _modalNavigationStore.Close();
            }
            catch (Exception)
            {
                formViewModel.ErrorMessage = "Failed to add YouTube viewer. Please try again later.";
            }
            finally
            {
                formViewModel.IsSubmitting = false;
            }
        }
    }
}
