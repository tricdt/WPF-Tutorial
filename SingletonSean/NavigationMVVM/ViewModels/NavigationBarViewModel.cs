﻿using NavigationMVVM.Commands;
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
        public ICommand NavigatePeopleListingCommand { get; }
        public bool IsLoggedIn => _accountStore.IsLoggedIn;
        public NavigationBarViewModel(AccountStore accountStore,
            INavigationService homeNavigationService,
            INavigationService accountNavigationService,
            INavigationService loginNavigationService,
            INavigationService peopleListingNavigationService)
        {
            _accountStore = accountStore;
            NavigateHomeCommand = new NavigateCommand(homeNavigationService);
            NavigateAccountCommand = new NavigateCommand(accountNavigationService);
            NavigateLoginCommand = new NavigateCommand(loginNavigationService);
            LogoutCommand = new LogoutCommand(_accountStore);
            NavigatePeopleListingCommand = new NavigateCommand(peopleListingNavigationService);
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
