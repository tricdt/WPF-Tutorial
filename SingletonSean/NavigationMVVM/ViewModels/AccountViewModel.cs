using NavigationMVVM.Commands;
using NavigationMVVM.Services;
using NavigationMVVM.Stores;
using System.Windows.Input;
namespace NavigationMVVM.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        public NavigationBarViewModel NavigationBarViewModel { get; }
        private readonly AccountStore _accountStore;
        public string Username => _accountStore.CurrentAccount?.Username;
        public string Email => _accountStore.CurrentAccount?.Email;
        public ICommand NavigateHomeCommand { get; }
        public AccountViewModel(AccountStore accountStore, INavigationService homeNavigationService)
        {
            _accountStore = accountStore;
            //NavigationBarViewModel = navigationBarViewModel;
            //NavigateHomeCommand = new NavigationHomeCommand(_navigationStore);
            //NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
            //NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new Services.NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(accountStore, navigationStore)));
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            _accountStore.CurrentAccountChange += OnCurrentAccountChanged;
        }



        public override void Dispose()
        {
            _accountStore.CurrentAccountChange -= OnCurrentAccountChanged;
            base.Dispose();
        }
        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Username));
        }
    }
}
