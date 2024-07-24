using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using System.Windows.Input;
namespace NavigationMVVM.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
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
            }
        }

        public ICommand LoginCommand { get; }
        public LoginViewModel(AccountStore accountStore, INavigationService accountNavigationService)
        {
            //LoginCommand = new LoginCommand(this, new Services.NavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(accountStore, navigationStore)));
            //NavigationService<AccountViewModel> navigationService = new NavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(accountStore, navigationStore));
            //LoginCommand = new LoginCommand(this, accountStore, navigationService);
            LoginCommand = new LoginCommand(this, accountStore, accountNavigationService);
        }
    }
}
