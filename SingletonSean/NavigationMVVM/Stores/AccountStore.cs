using NavigationMVVM.Models;

namespace NavigationMVVM.Stores
{
    public class AccountStore
    {
        public event Action CurrentAccountChange;
        private Account _currentAccount;
        public Account CurrentAccount
        {
            get => _currentAccount;
            set
            {
                _currentAccount = value;
                CurrentAccountChange?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;
        public void Logout()
        {
            CurrentAccount = null;
        }
    }
}
