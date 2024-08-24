using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;

namespace SimpleTrader.WPF.Commands
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IRenavigator _renavigator;

        public LoginCommand(LoginViewModel loginViewModel, IRenavigator renavigator)
        {
            _loginViewModel = loginViewModel;
            _renavigator = renavigator;
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
            _renavigator.Renavigate();
        }
    }
}
