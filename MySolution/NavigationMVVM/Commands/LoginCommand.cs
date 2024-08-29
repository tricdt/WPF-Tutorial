using NavigationMVVM.Models;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using NavigationMVVM.ViewModels;
namespace NavigationMVVM.Commands
{
    public class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly INavigationService _navigationService;
        private readonly AccountStore _accountStore;
        public LoginCommand(LoginViewModel loginViewModel, INavigationService navigationService, AccountStore accountStore)
        {
            _loginViewModel = loginViewModel;
            _navigationService = navigationService;
            _accountStore = accountStore;
        }

        public override void Execute(object parameter)
        {
            Account account = new Account()
            {
                Email = $"{_loginViewModel.Username}@test.com",
                Username = _loginViewModel.Username
            };
            _accountStore.CurrentAccount = account;
            _navigationService.Navigate();
        }
    }
}
