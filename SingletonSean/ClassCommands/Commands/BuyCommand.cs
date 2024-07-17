using ClassCommands.Stores;
using ClassCommands.ViewModels;
using System.ComponentModel;
namespace ClassCommands.Commands
{
    public class BuyCommand : BaseCommand
    {
        private readonly BuyViewModel _viewModel;
        private readonly IOwnedItemsStore _ownedItemsStore;

        public BuyCommand(BuyViewModel viewModel, IOwnedItemsStore ownedItemsStore)
        {
            _viewModel = viewModel;
            _ownedItemsStore = ownedItemsStore;
            _viewModel.PropertyChanged += _viewModel_PropertyChanged;
        }

        private void _viewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object parameter)
        {
            return _viewModel.IsValidBuy;
        }

        public override void Execute(object parameter)
        {
            _viewModel.StatusMessage = string.Empty;
            _viewModel.ErrorMessage = string.Empty;

            _ownedItemsStore.Buy(_viewModel.ItemName, _viewModel.Quantity);

            _viewModel.StatusMessage = $"Successfully bought {_viewModel.Quantity} {_viewModel.ItemName}.";
        }
    }
}
