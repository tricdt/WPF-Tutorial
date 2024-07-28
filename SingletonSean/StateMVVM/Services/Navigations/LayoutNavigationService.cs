using MVVMEssentials.ViewModels;
using StateMVVM.Stores;
using StateMVVM.ViewModels;

namespace StateMVVM.Services.Navigations
{
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly CreateViewModel<TViewModel> _createViewModel;
        private readonly CreateViewModel<NavigationBarViewModel> _createNavigationBarViewModel;
        private readonly CreateViewModel<GlobalMessageViewModel> _createGlobalMessageViewModel;
        public LayoutNavigationService(NavigationStore navigationStore,
            CreateViewModel<TViewModel> createViewModel,
            CreateViewModel<NavigationBarViewModel> createNavigationBarViewModel,
        CreateViewModel<GlobalMessageViewModel> createGlobalMessageViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
            _createNavigationBarViewModel = createNavigationBarViewModel;
            _createGlobalMessageViewModel = createGlobalMessageViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = new LayoutViewModel(_createNavigationBarViewModel(), _createViewModel(), _createGlobalMessageViewModel());
        }
    }
}
