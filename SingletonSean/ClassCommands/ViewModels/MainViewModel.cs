using ClassCommands.Services;
using ClassCommands.Stores;

namespace ClassCommands.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public BuyViewModel BuyViewModel { get; set; }
        public SellViewModel SellViewModel { get; set; }
        public MainViewModel()
        {
            OwnedItemsStore ownedItemsStore = new OwnedItemsStore();
            PriceService priceService = new PriceService();
            BuyViewModel = new BuyViewModel(ownedItemsStore, priceService);
            SellViewModel = new SellViewModel(ownedItemsStore, priceService);
        }
    }
}
