using SimpleTrader.WPF.State.Navigators;

namespace SimpleTrader.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public INavigator Navigator { get; set; }
        public MainViewModel(INavigator navigator)
        {
            Navigator = navigator;
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
            Navigator.CurrentViewModelChanged += Navigator_CurrentViewModelChanged;
        }

        private void Navigator_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ViewModelBase CurrentViewModel => Navigator.CurrentViewModel;

    }
}
