using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.ledsign.wpf
{
    public class FillDropdownItem : Control
    {
        public FillDropdownItem()
        {
            this.DefaultStyleKey = typeof(FillDropdownItem);
        }

        public FillDropdownItem(LedEditMarkerMouseController SeriesController) : this()
        {
            ItemsSource = new ObservableCollection<string> { "Copy", "Cut" };
        }




        public ObservableCollection<string> ItemsSource
        {
            get { return (ObservableCollection<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<string>), typeof(FillDropdownItem), new PropertyMetadata(null));



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
