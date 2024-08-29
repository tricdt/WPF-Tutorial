using NavigationMVVM.Stores;

namespace NavigationMVVM.Commands
{
    public class LogoutCommand : CommandBase
    {
        private readonly AccountStore _accountStore;
        public override void Execute(object parameter)
        {
            _accountStore.Logout();
        }
        public LogoutCommand(AccountStore accountStore)
        {
            _accountStore = accountStore;
        }
    }
}
