using NavigationMVVM.Stores;
namespace NavigationMVVM.Commands
{
    class NavigationHomeCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;

        public NavigationHomeCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            //_navigationStore.CurrentViewModel = new HomeViewModel(_navigationStore);
        }
    }
}
