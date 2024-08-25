using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Windows.Input;
namespace SimpleTrader.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly ISimpleTraderViewModelFactory _viewModelFactory;
        private readonly IAuthenticator _authenticator;
        public ICommand UpdateCurrentViewModelCommand { get; }
        public MainViewModel(INavigator navigator, ISimpleTraderViewModelFactory viewModelFactory, IAuthenticator authenticator)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
            _authenticator = authenticator;
            _navigator.CurrentViewModelChanged += Navigator_CurrentViewModelChanged;
            authenticator.StateChanged += Authenticator_StateChanged;
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewType.Login);
        }

        private void Authenticator_StateChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private void Navigator_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ViewModelBase CurrentViewModel => _navigator.CurrentViewModel;

        public bool IsLoggedIn => _authenticator.IsLoggedIn;
        public override void Dispose()
        {
            _navigator.CurrentViewModelChanged -= Navigator_CurrentViewModelChanged;
            base.Dispose();
        }
    }
}
