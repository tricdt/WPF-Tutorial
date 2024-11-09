using Syncfusion.Windows.Tools.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace LedSignControl
{
    [DesignTimeVisible(false)]
    public class FillDropdownItem : Control
    {
        public ObservableCollection<CheckListBoxItem> ItemsSource
        {
            get { return (ObservableCollection<CheckListBoxItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<CheckListBoxItem>), typeof(FillDropdownItem), new PropertyMetadata());


        public SeriesType SelectedItem
        {
            get { return (SeriesType)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(SeriesType), typeof(FillDropdownItem), new PropertyMetadata(null));


        LedEditMarkerMouseController SeriesController = null;
        public FillDropdownItem()
        {
            this.DefaultStyleKey = typeof(FillDropdownItem);
        }
        Binding binding;
        public FillDropdownItem(LedEditMarkerMouseController SeriesController) : this()
        {
            this.SeriesController = SeriesController;
            SelectedItem = SeriesController.InnerFillType;
            ItemsSource = new ObservableCollection<CheckListBoxItem>();

            if ((SeriesController.Filltype & SeriesType.CopySeries) == SeriesType.CopySeries)
            {
                binding = new Binding("SelectedItem");
                binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FillDropdownItem), 1);
                binding.Converter = new FillTypeConverter();
                binding.ConverterParameter = "CopySeries";
                binding.Mode = BindingMode.TwoWay;
                CheckListBoxItem CopySeriesButton = new CheckListBoxItem() { Content = "CopySeries" };
                CopySeriesButton.Selected += OnCheckListBoxItem_Selected;
                CopySeriesButton.SetBinding(CheckListBoxItem.IsCheckedProperty, binding);
                ItemsSource.Add(CopySeriesButton);
            }
            if ((SeriesController.Filltype & SeriesType.FillSeries) == SeriesType.FillSeries)
            {
                binding = new Binding("SelectedItem");
                binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FillDropdownItem), 1);
                binding.Converter = new FillTypeConverter();
                binding.ConverterParameter = "FillSeries";
                binding.Mode = BindingMode.TwoWay;
                CheckListBoxItem FillSeriesButton = new CheckListBoxItem() { Content = "FillSeries" };
                FillSeriesButton.Selected += OnCheckListBoxItem_Selected;
                FillSeriesButton.SetBinding(CheckListBoxItem.IsCheckedProperty, binding);
                ItemsSource.Add(FillSeriesButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillFormatOnly) == SeriesType.FillFormatOnly)
            {
                binding = new Binding("SelectedItem");
                binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FillDropdownItem), 1);
                binding.Converter = new FillTypeConverter();
                binding.ConverterParameter = "FillFormatOnly";
                binding.Mode = BindingMode.TwoWay;
                CheckListBoxItem FillFormatOnlyButton = new CheckListBoxItem() { Content = "FillFormatOnly" };
                FillFormatOnlyButton.Selected += OnCheckListBoxItem_Selected;
                FillFormatOnlyButton.SetBinding(CheckListBoxItem.IsCheckedProperty, binding);
                ItemsSource.Add(FillFormatOnlyButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillWithoutFormat) == SeriesType.FillWithoutFormat)
            {
                binding = new Binding("SelectedItem");
                binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FillDropdownItem), 1);
                binding.Converter = new FillTypeConverter();
                binding.ConverterParameter = "FillWithoutFormat";
                binding.Mode = BindingMode.TwoWay;
                CheckListBoxItem FillWithoutFormatButton = new CheckListBoxItem() { Content = "FillWithoutFormat" };
                FillWithoutFormatButton.Selected += OnCheckListBoxItem_Selected;
                FillWithoutFormatButton.SetBinding(CheckListBoxItem.IsCheckedProperty, binding);
                ItemsSource.Add(FillWithoutFormatButton);
            }
        }

        private void OnCheckListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            if (SeriesController != null)
            {
                SeriesController.FillOptionChanged((sender as CheckListBoxItem).Content.ToString());
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
