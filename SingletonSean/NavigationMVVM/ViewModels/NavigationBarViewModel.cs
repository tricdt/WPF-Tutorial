using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using System.Windows.Input;

namespace NavigationMVVM.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private readonly AccountStore _accountStore;
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateLoginCommand { get; }
        public ICommand LogoutCommand { get; }
        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public NavigationBarViewModel(AccountStore accountStore,
            INavigationService homeNavigationService,
            INavigationService accountNavigationService,
            INavigationService loginNavigationService)
        {
            _accountStore = accountStore;
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(homeNavigationService);
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(accountNavigationService);
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
            LogoutCommand = new LogoutCommand(_accountStore);
            _accountStore.CurrentAccountChange += OnCurrentAccountChanged;
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
        }
        public override void Dispose()
        {
            _accountStore.CurrentAccountChange -= OnCurrentAccountChanged;
            base.Dispose();
        }
    }
}
