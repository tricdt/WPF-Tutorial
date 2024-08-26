using NavigationMVVM.ViewModels;

namespace NavigationMVVM.Stores
{
    public class NavigationStore
    {
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
        public event Action CurrentViewModelChanged;
        public NavigationStore()
        {
            CurrentViewModel = new AddPersonViewModel();
        }

    }
}
