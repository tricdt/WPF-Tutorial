using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ImageTextListBoxListBoxItem : ListBoxItem, INotifyPropertyChanged
    {
        private string text = string.Empty;
        private ImageSource image;
        public ImageTextListBoxListBoxItem()
        {

        }

        static ImageTextListBoxListBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageTextListBoxListBoxItem), new FrameworkPropertyMetadata(typeof(ImageTextListBoxListBoxItem)));
        }

        // Using a DependencyProperty as the backing store for IsHighlighted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsHighlightedProperty =
            DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(ImageTextListBoxListBoxItem), new PropertyMetadata(null));

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            set { SetValue(IsHighlightedProperty, value); }
        }



        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.RaisePropertyChangedEvent(new PropertyChangedEventArgs("Text"));
                }
            }
        }

        public ImageSource Image
        {
            get { return this.image; }
            set { this.image = value; }
        }

        public override string ToString()
        {
            return this.text;
        }

        private void RaisePropertyChangedEvent(PropertyChangedEventArgs args)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, args);
            }
        }
        internal ImageTextListBox ListBox
        {
            get
            {
                return (ItemsControl.ItemsControlFromItemContainer(this) as Selector) as ImageTextListBox;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            var parentListBox = this.ListBox;
            if (parentListBox != null)
            {
                parentListBox.NotifyListBoxItemEnter(this);
            }
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            var parentListBox = this.ListBox;
            if (parentListBox != null)
            {
                parentListBox.NotifyListBoxItemMouseUp(this);
            }

            e.Handled = true;
            base.OnMouseLeftButtonUp(e);
        }
    }
}