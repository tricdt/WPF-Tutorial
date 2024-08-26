using NavigationMVVM.Services;

namespace NavigationMVVM.Commands
{
    public class NavigateCommand : CommandBase
    {
        private readonly INavigationService _navigationService;
        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
