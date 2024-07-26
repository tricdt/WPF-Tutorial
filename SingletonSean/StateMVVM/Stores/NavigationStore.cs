using MVVMEssentials.ViewModels;
namespace StateMVVM.Stores
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

    }
}
