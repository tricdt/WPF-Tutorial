using AsyncCommands.Commands;
using AsyncCommands.Services;
using System.Windows.Input;

namespace AsyncCommands.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }
        public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);
        public ICommand LoginCommand { get; }
        public LoginViewModel()
        {
            LoginCommand = new LoginCommand(this, new AuthenticationService(), (ex) => StatusMessage = ex.Message);
        }
    }
}
