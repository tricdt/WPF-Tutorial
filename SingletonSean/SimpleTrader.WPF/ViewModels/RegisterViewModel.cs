using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.State.Authenticators;
using SimpleTrader.WPF.State.Navigators;
using System.Windows.Input;

namespace SimpleTrader.WPF.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private string _email = "salaelectronics@gmail.com";
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private string _username = "odergaard";
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private string _password = "123456";
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(CanRegister));
            }
        }

        private string _confirmPassword = "123456";
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                OnPropertyChanged(nameof(CanRegister));
            }
        }
        public MessageViewModel ErrorMessageViewModel { get; }
        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }
        public bool CanRegister => !string.IsNullOrEmpty(Email) &&
            !string.IsNullOrEmpty(Username) &&
            !string.IsNullOrEmpty(Password) &&
            !string.IsNullOrEmpty(ConfirmPassword);
        public ICommand RegisterCommand { get; }
        public ICommand ViewLoginCommand { get; }
        public RegisterViewModel(IRenavigator registerRenavigator, IRenavigator loginRenavigator, IAuthenticator authenticator)
        {
            ViewLoginCommand = new RenavigateCommand(loginRenavigator);
            RegisterCommand = new RegisterCommand(this, authenticator, loginRenavigator);
            ErrorMessageViewModel = new MessageViewModel();
        }
    }
}
