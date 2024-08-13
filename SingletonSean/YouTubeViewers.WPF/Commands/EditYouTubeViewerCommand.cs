using YouTubeViewers.Domain.Models;
using YouTubeViewers.WPF.Stores;
using YouTubeViewers.WPF.ViewModels;

namespace YouTubeViewers.WPF.Commands
{
    public class EditYouTubeViewerCommand : CommandBase
    {
        private readonly EditYouTubeViewerViewModel _editYouTubeViewerViewModel;
        private readonly YouTubeViewersStore _youTubeViewersStore;
        private readonly ModalNavigationStore _modalNavigationStore;

        public EditYouTubeViewerCommand(EditYouTubeViewerViewModel editYouTubeViewerViewModel,
            YouTubeViewersStore youTubeViewersStore, ModalNavigationStore modalNavigationStore)
        {
            _editYouTubeViewerViewModel = editYouTubeViewerViewModel;
            _youTubeViewersStore = youTubeViewersStore;
            _modalNavigationStore = modalNavigationStore;
        }

        public override async void Execute(object parameter)
        {
            YouTubeViewerDetailsFormViewModel formViewModel = _editYouTubeViewerViewModel.YouTubeViewerDetailsFormViewModel;
            formViewModel.IsSubmitting = true;
            YouTubeViewer youTubeViewer = new YouTubeViewer(_editYouTubeViewerViewModel.YouTubeViewerId, formViewModel.Username, formViewModel.IsSubscribed, formViewModel.IsMember);
            try
            {
                await Task.Delay(5000);
                await _youTubeViewersStore.Update(youTubeViewer);
                _modalNavigationStore.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                formViewModel.IsSubmitting = false;
            }
        }
    }
}
