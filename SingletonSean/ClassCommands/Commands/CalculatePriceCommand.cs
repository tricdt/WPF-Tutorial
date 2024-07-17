using ClassCommands.Exceptions;
using ClassCommands.Services;
using ClassCommands.ViewModels;
using System.Windows.Input;

namespace ClassCommands.Commands
{
    public class CalculatePriceCommand : BaseCommand, ICommand
    {
        private readonly ICalculatePriceViewModel _viewModel;
        private readonly IPriceService _priceService;

        public CalculatePriceCommand(ICalculatePriceViewModel viewModel, IPriceService priceService)
        {
            _viewModel = viewModel;
            _priceService = priceService;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private void _viewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return _viewModel.CanCalculatePrice;
        }
        public override void Execute(object parameter)
        {
            _viewModel.StatusMessage = string.Empty;
            _viewModel.ErrorMessage = string.Empty;

            try
            {
                double itemPrice = _priceService.GetPrice(_viewModel.ItemName);
                double totalPrice = itemPrice * _viewModel.Quantity;

                _viewModel.StatusMessage = $"The total price of {_viewModel.Quantity} {_viewModel.ItemName} is {totalPrice:C}.";
            }
            catch (ItemPriceNotFoundException)
            {
                _viewModel.ErrorMessage = $"Unable to find price of {_viewModel.ItemName}.";
            }
        }
    }
}
