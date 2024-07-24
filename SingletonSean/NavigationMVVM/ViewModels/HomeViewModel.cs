using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using System.Windows.Input;
namespace NavigationMVVM.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public string WelcomeMessage => "Welcome to my application.";
        //public NavigationBarViewModel NavigationBarViewModel { get; }
        public ICommand NavigateLoginCommand { get; }
        public HomeViewModel(INavigationService loginNavigationService)
        {
            //NavigationBarViewModel = navigationBarViewModel;
            //NavigateAccountCommand = new NavigationAccountCommand(navigationStore);
            //NavigateAccountCommand = new NavigateCommand<AccountViewModel>(navigationStore, () => new AccountViewModel(navigationStore));
            //NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigationStore, () => new LoginViewModel());
            //NavigateLoginCommand = new NavigateCommand<LoginViewModel>(new NavigationService<LoginViewModel>(navigationStore, () => new LoginViewModel(accountStore, navigationStore)));

            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
        }
    }
}
