using NavigationMVVM.Stores;
namespace NavigationMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel
        {
            get { return _navigationStore.CurrentViewModel; }
        }
        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += NavigationStore_CurrentViewModelChanged;
        }

        private void NavigationStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
