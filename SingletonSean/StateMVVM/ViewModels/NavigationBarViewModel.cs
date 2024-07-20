using StateMVVM.Commands;
using StateMVVM.Services.Navigations;
using System.Windows.Input;
namespace StateMVVM.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand NavigatePostHomeCommand { get; }
        public ICommand NavigatePostListingCommand { get; }

        public NavigationBarViewModel(INavigationService postsHomeNavigationService,
            INavigationService postListingNavigationService)
        {
            NavigatePostHomeCommand = new NavigateCommand(postsHomeNavigationService);
            NavigatePostListingCommand = new NavigateCommand(postListingNavigationService);
        }
    }
}
