using NavigationMVVM.Services;
using NavigationMVVM.ViewModels;

namespace NavigationMVVM.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase where TViewModel : ViewModelBase
    {
        private readonly INavigationService<TViewModel> _navigationService;

        public NavigateCommand(INavigationService<TViewModel> navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
