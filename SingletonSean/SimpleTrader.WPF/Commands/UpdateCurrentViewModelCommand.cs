using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels.Factories;
using System.Windows.Input;
namespace SimpleTrader.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        string _api_key = "UbaYiNzmGPpOt4JR3965DmMzL4664AlI";
        string baseUrl = "https://financialmodelingprep.com/api/v3/";
        private readonly INavigator _navigator;
        private readonly ISimpleTraderViewModelFactory _viewModelFactory;
        public UpdateCurrentViewModelCommand(INavigator navigator, ISimpleTraderViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is ViewType)
            {
                ViewType viewType = (ViewType)parameter;

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }
    }
}
