using ClassCommands.Commands;
using ClassCommands.Exceptions;
using ClassCommands.Services;
using ClassCommands.Stores;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClassCommands.ViewModels
{
    public class BuyViewModel : ViewModelBase, ICalculatePriceViewModel
    {
        private readonly OwnedItemsStore _ownedItemsStore;
        private readonly PriceService _priceService;
        public IEnumerable<string> BuyableItems { get; }
        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                OnPropertyChanged(nameof(ItemName));
                OnPropertyChanged(nameof(IsValidBuy));
                OnPropertyChanged(nameof(CanCalculatePrice));
            }
        }
        private int _quantity;

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(IsValidBuy));
                OnPropertyChanged(nameof(CanCalculatePrice));
            }
        }

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool IsValidBuy => !string.IsNullOrEmpty(ItemName) && Quantity > 0;
        public bool CanCalculatePrice => IsValidBuy;
        public ICommand CalculatePriceCommand { get; }
        public ICommand BuyCommand { get; }
        public BuyViewModel(IOwnedItemsStore ownedItemsStore, IPriceService priceService)
        {

            BuyableItems = new ObservableCollection<string>
            {
                "apple",
                "shirt",
                "phone",
                "burrito",
                "pillow"
            };
            //CalculatePriceCommand = new CallbackCommand(CalculatePrice, () => IsValidBuy);
            //BuyCommand = new CallbackCommand(Buy, () => IsValidBuy);
            BuyCommand = new BuyCommand(this, ownedItemsStore);
            CalculatePriceCommand = new CalculatePriceCommand(this, priceService);
        }

        private void Buy()
        {
            StatusMessage = string.Empty;
            ErrorMessage = string.Empty;

            _ownedItemsStore.Buy(ItemName, Quantity);

            StatusMessage = $"Successfully bought {Quantity} {ItemName}.";
        }

        private void CalculatePrice()
        {
            StatusMessage = string.Empty;
            ErrorMessage = string.Empty;

            try
            {
                double itemPrice = _priceService.GetPrice(ItemName);
                double totalPrice = itemPrice * Quantity;

                StatusMessage = $"The total price of {Quantity} {ItemName} is {totalPrice:C}.";
            }
            catch (ItemPriceNotFoundException)
            {
                ErrorMessage = $"Unable to find price of {ItemName}.";
            }
        }
    }
}
