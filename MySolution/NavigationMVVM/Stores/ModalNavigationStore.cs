using NavigationMVVM.ViewModels;

namespace NavigationMVVM.Stores
{
    public class ModalNavigationStore
    {
        private ViewModelBase _currentModalViewModal;

        public ViewModelBase CurrentModalViewModel
        {
            get { return _currentModalViewModal; }
            set
            {
                _currentModalViewModal = value;
                CurrentModalViewModelChanged?.Invoke();
            }
        }
        public bool IsOpen => CurrentModalViewModel != null;
        public event Action CurrentModalViewModelChanged;
    }
}
