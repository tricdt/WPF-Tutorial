using NavigationMVVM.Stores;
using NavigationMVVM.ViewModels;

namespace NavigationMVVM.Services
{
    public class ModalNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly CreateViewModel<TViewModel> _createViewModel;
        private readonly ModalNavigationStore _modalNavigationStore;

        public ModalNavigationService(CreateViewModel<TViewModel> createViewModel, ModalNavigationStore modalNavigationStore)
        {
            _createViewModel = createViewModel;
            _modalNavigationStore = modalNavigationStore;
        }

        public void Navigate()
        {
            _modalNavigationStore.CurrentModalViewModel = _createViewModel();
        }
    }
}
