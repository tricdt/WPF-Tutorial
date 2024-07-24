using NavigationMVVM.ViewModels;

namespace NavigationMVVM.Stores
{
    public class ModalNavigationStore
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel?.Dispose();
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        public bool IsOpen => _currentViewModel != null;
        public event Action CurrentViewModelChanged;
        public void Close()
        {
            CurrentViewModel = null;
        }
        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();

        }
    }
}
