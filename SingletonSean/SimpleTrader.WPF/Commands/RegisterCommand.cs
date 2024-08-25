using SimpleTrader.Domain.Services.AuthenticationServices;
using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;

namespace SimpleTrader.WPF.Commands
{
    public class RegisterCommand : AsyncCommandBase
    {
        private readonly RegisterViewModel _registerViewModel;
        private readonly IAuthenticator _authenticator;
        private readonly IRenavigator _renavigator;

        public RegisterCommand(RegisterViewModel registerViewModel, IAuthenticator authenticator, IRenavigator renavigator)
        {
            _registerViewModel = registerViewModel;
            _authenticator = authenticator;
            _renavigator = renavigator;
            _registerViewModel.PropertyChanged += RegisterViewModel_PropertyChanged;
        }

        private void RegisterViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RegisterViewModel.CanRegister))
            {
                OnCanExecuteChanged();
            }
        }

        public override bool CanExecute(object parameter)
        {
            return _registerViewModel.CanRegister;
        }
        public override async Task ExecuteAsync(object parameter)
        {
            _registerViewModel.ErrorMessage = string.Empty;
            try
            {
                RegistrationResult registrationResult = await _authenticator.Register(
                    _registerViewModel.Email,
                    _registerViewModel.Username,
                    _registerViewModel.Password,
                    _registerViewModel.ConfirmPassword);
                switch (registrationResult)
                {
                    case RegistrationResult.Success:
                        _renavigator.Renavigate();
                        break;
                    case RegistrationResult.PasswordsDoNotMatch:
                        _registerViewModel.ErrorMessage = "Password does not match confirm password.";
                        break;
                    case RegistrationResult.EmailAlreadyExists:
                        _registerViewModel.ErrorMessage = "An account for this email already exists.";
                        break;
                    case RegistrationResult.UsernameAlreadyExists:
                        _registerViewModel.ErrorMessage = "An account for this username already exists.";
                        break;
                    default:
                        _registerViewModel.ErrorMessage = "Registration failed.";
                        break;
                }
            }
            catch (Exception)
            {
                _registerViewModel.ErrorMessage = "Registration failed.";
            }
        }
    }
}
