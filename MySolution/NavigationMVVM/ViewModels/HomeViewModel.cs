using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using System.Windows.Input;

namespace NavigationMVVM.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public string WelcomeMessage => "Welcome to my application.";

        public ICommand NavigateLoginCommand { get; }
        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateLoginCommand = new NavigateCommand(navigationService);
        }
    }
}
