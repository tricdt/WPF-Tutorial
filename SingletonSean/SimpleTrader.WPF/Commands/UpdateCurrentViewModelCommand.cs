using SimpleTrader.FinancialModelingPrepAPI;
using SimpleTrader.FinancialModelingPrepAPI.Services;
using SimpleTrader.WPF.State.Navigators;
using SimpleTrader.WPF.ViewModels;
using System.Windows.Input;
namespace SimpleTrader.WPF.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        string _api_key = "UbaYiNzmGPpOt4JR3965DmMzL4664AlI";
        string baseUrl = "https://financialmodelingprep.com/api/v3/";
        private INavigator _navigator;
        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
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

                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = new HomeViewModel(MajorIndexListingViewModel.LoadMajorIndexViewModel(new MajorIndexService(new FinancialModelingPrepHttpClient(new System.Net.Http.HttpClient() { BaseAddress = new Uri(baseUrl) }, new FinancialModelingPrepAPI.Models.FinancialModelingPrepAPIKey(_api_key)))));
                        break;
                    case ViewType.Portfolio:
                        _navigator.CurrentViewModel = new PortfolioViewModel();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
