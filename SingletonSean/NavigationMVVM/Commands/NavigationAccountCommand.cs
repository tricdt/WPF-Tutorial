using NavigationMVVM.Stores;
namespace NavigationMVVM.Commands
{
    public class NavigationAccountCommand : CommandBase
    {
        private readonly NavigationStore _navigationStore;
        public NavigationAccountCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        public override void Execute(object parameter)
        {
            //_navigationStore.CurrentViewModel = new AccountViewModel(_navigationStore);
        }
    }
}
