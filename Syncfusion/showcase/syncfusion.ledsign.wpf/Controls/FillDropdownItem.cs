using Syncfusion.Windows.Tools.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System;
using Syncfusion.SfSkinManager;

namespace syncfusion.ledsign.wpf
{
    public class FillDropdownItem : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<RibbonRadioButton>), typeof(FillDropdownItem), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(RibbonRadioButton), typeof(FillDropdownItem), new PropertyMetadata(null, OnSelectedItemChangedCallback));



        public FillDropdownItem()
        {
            this.DefaultStyleKey = typeof(FillDropdownItem);
        }

        public FillDropdownItem(LedEditMarkerMouseController SeriesController) : this()
        {
            ItemsSource = new ObservableCollection<RibbonRadioButton>();
            if((SeriesController.Filltype & SeriesType.CopySeries) == SeriesType.CopySeries)
            {
                RibbonRadioButton CopySeriesButton = new RibbonRadioButton() { Content = "CopySeries", GroupName = "FillSeriesGroup" };

                ItemsSource.Add(CopySeriesButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillSeries) == SeriesType.FillSeries)
            {
                RibbonRadioButton CopySeriesButton = new RibbonRadioButton() { Content = "FillSeries", GroupName = "FillSeriesGroup" };

                ItemsSource.Add(CopySeriesButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillFormatOnly) == SeriesType.FillFormatOnly)
            {
                RibbonRadioButton CopySeriesButton = new RibbonRadioButton() { Content = "FillFormatOnly", GroupName = "FillSeriesGroup" };

                ItemsSource.Add(CopySeriesButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillWithoutFormat) == SeriesType.FillWithoutFormat)
            {
                RibbonRadioButton CopySeriesButton = new RibbonRadioButton() { Content = "FillWithoutFormat", GroupName = "FillSeriesGroup" };

                ItemsSource.Add(CopySeriesButton);
            }
        }


        public RibbonRadioButton SelectedItem
        {
            get { return (RibbonRadioButton)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public ObservableCollection<RibbonRadioButton> ItemsSource
        {
            get { return (ObservableCollection<RibbonRadioButton>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnSelectedItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (e.NewValue as RibbonRadioButton).IsChecked = true;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}