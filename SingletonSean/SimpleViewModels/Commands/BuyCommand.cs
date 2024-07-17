using SimpleViewModels.Exceptions;
using SimpleViewModels.Services;
using SimpleViewModels.ViewModels;
using System.ComponentModel;
namespace SimpleViewModels.Commands
{
    public class BuyCommand : BaseCommand
    {
        private readonly BuyViewModel _viewModel;
        private readonly PriceService _priceService;

        public BuyCommand(BuyViewModel viewModel, PriceService priceService)
        {
            _viewModel = viewModel;
            _priceService = priceService;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private void _viewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BuyViewModel.IsValidBuy))
            {
                OnCanExecuteChanged();
            }
        }

        public override void Execute(object? parameter)
        {
            _viewModel.ClearMessages();

            try
            {
                double price = _priceService.GetPrice(_viewModel.ItemName);
                double totalPrice = price * _viewModel.Quantity;

                _viewModel.StatusMessage = $"Successfully bought {_viewModel.Quantity} {_viewModel.ItemName} for {totalPrice:C}.";
            }
            catch (ItemPriceNotFoundException)
            {
                _viewModel.ErrorMessage = $"Failed to buy item. Unable to find price of {_viewModel.ItemName}.";
            }
        }
    }
}
