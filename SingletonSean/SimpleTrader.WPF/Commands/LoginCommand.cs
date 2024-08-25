using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
namespace SimpleTrader.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IRenavigator _renavigator;
        private readonly IAuthenticator _authenticator;

        public LoginCommand(LoginViewModel loginViewModel, IRenavigator renavigator, IAuthenticator authenticator)
        {
            _loginViewModel = loginViewModel;
            _renavigator = renavigator;
            _authenticator = authenticator;
            _loginViewModel.PropertyChanged += _loginViewModel_PropertyChanged;
        }

        private void _loginViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LoginViewModel.CanLogin))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return _loginViewModel.CanLogin && base.CanExecute(parameter);
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await _authenticator.Login(_loginViewModel.Username, _loginViewModel.Password);
            _renavigator.Renavigate();
        }
    }
}
