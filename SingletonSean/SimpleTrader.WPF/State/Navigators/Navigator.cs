using SimpleTrader.WPF.Commands;
using SimpleTrader.WPF.Models;
using SimpleTrader.WPF.ViewModels;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Windows.Input;
namespace SimpleTrader.WPF.State.Navigators
{
    public class Navigator : ObservableObject, INavigator
    {
        public event Action CurrentViewModelChanged;
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }
            set
            {
                _currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public ICommand UpdateCurrentViewModelCommand { get; set; }
        public Navigator(ISimpleTraderViewModelFactory viewModelFactory)
        {
            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(this, viewModelFactory);
        }
    }
}
