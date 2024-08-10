using YouTubeViewers.WPF.Stores;

namespace YouTubeViewers.WPF.Commands
{
    public class CloseModalCommand : CommandBase
    {
        private readonly ModalNavigationStore _modalNavigationStore;
        public override void Execute(object parameter)
        {
            _modalNavigationStore.Close();
        }
        public CloseModalCommand(ModalNavigationStore modalNavigationStore)
        {
            _modalNavigationStore = modalNavigationStore;
        }
    }
}
