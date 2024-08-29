using System.Collections.ObjectModel;

namespace CustomControl.Controls
{
    public class Items : ObservableCollection<Item>
    {
        public Items()
        {
            Add(new Item { Value = 80.23 });
            Add(new Item { Value = 126.17 });
            Add(new Item { Value = 130.21 });
            Add(new Item { Value = 115.28 });
            Add(new Item { Value = 131.21 });
            Add(new Item { Value = 135.22 });
            Add(new Item { Value = 120.27 });
            Add(new Item { Value = 110.25 });
            Add(new Item { Value = 90.20 });
        }
    }
}
