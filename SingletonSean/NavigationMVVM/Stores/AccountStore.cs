using NavigationMVVM.Models;

namespace NavigationMVVM.Stores
{
    public class AccountStore
    {
        private Account _currentAccount;
        public Account CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value;
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;
    }
}
