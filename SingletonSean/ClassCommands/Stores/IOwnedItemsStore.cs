using ClassCommands.Models;

namespace ClassCommands.Stores
{
    public interface IOwnedItemsStore
    {
        IEnumerable<OwnedItem> OwnedItems { get; }

        event Action OwnedItemsChanged;

        void Buy(string itemName, int quantity);
        void Sell(string itemName, int quantity);
    }
}