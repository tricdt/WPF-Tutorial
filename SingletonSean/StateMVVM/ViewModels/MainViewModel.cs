﻿using MVVMEssentials.ViewModels;
using StateMVVM.Stores;
namespace StateMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

    }
}
