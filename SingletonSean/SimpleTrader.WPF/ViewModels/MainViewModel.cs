using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Windows.Input;
namespace SimpleTrader.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly ISimpleTraderViewModelFactory _viewModelFactory;
        public ICommand UpdateCurrentViewModelCommand { get; }
        public MainViewModel(INavigator navigator, ISimpleTraderViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            _navigator.CurrentViewModelChanged += Navigator_CurrentViewModelChanged;
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        private void Navigator_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        public bool IsLoggedIn => false;
        public override void Dispose()
        {
            _navigator.CurrentViewModelChanged -= Navigator_CurrentViewModelChanged;
            base.Dispose();
        }
    }
}
