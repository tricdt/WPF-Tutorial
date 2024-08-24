using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.State.Navigators;
using System.Windows.Input;

namespace SimpleTrader.WPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username = "SingletonSean";
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
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        private string _password;
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
                OnPropertyChanged(nameof(CanLogin));
            }
        }

        public bool CanLogin => !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password);

        public ICommand LoginCommand { get; }
        public ICommand ViewRegisterCommand { get; }
        public LoginViewModel(IRenavigator loginRenavigator, IRenavigator registerRenavigator)
        {
            LoginCommand = new LoginCommand(this, loginRenavigator);
            ViewRegisterCommand = new RenavigateCommand(registerRenavigator);
        }
    }
}
