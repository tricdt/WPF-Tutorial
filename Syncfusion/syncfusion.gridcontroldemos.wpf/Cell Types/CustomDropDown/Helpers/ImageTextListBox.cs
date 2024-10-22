using System.Windows.Controls;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ImageTextListBox : ListBox
    {
        public CustomeDropDown DropDown
        {
            get;
            private set;
        }
        public ImageTextListBox(CustomeDropDown dropdown)
        {
            this.DropDown = dropdown;
        }

        internal void NotifyListBoxItemEnter(ImageTextListBoxListBoxItem item)
        {
            item.Focus();
            this.HighlightedItem = item;
            this.SelectedIndex = this.ItemContainerGenerator.IndexFromContainer(item);
        }

        internal void NotifyListBoxItemMouseUp(ImageTextListBoxListBoxItem item)
        {
            this.SelectedItem = item;
            this.DropDown.Close();
        }

        private WeakReference highlightedItem = null;
        private ImageTextListBoxListBoxItem HighlightedItem
        {
            get
            {
                if (this.highlightedItem == null)
                {
                    return null;
                }
                return this.highlightedItem.Target as ImageTextListBoxListBoxItem;
            }
            set
            {
                ImageTextListBoxListBoxItem item = (this.highlightedItem != null) ? (this.highlightedItem.Target as ImageTextListBoxListBoxItem) : null;
                if (item != null)
                {
                    item.IsHighlighted = false;
                }
                if (value != null)
                {
                    this.highlightedItem = new WeakReference(value);
                    value.IsHighlighted = true;
                }
                else
                {
                    this.highlightedItem = null;
                }
            }
        }


    }
}