﻿using Syncfusion.Windows.Tools.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.gridcontroldemos.wpf
{
    [DesignTimeVisible(false)]
    public class FillDropDownItem : Control
    {
        GridFillSeriesMouseController SeriesController = null;
        SplitButtonAdv SplitButton;
        public FillDropDownItem()
        {
            this.DefaultStyleKey = typeof(FillDropDownItem);
        }
        public FillDropDownItem(GridFillSeriesMouseController SeriesController) : this()
        {
            this.SeriesController = SeriesController;
            ItemsSource = new ObservableCollection<RadioButton>();


            if ((SeriesController.Filltype & SeriesType.CopySeries) == SeriesType.CopySeries)
            {
                RadioButton CopySeriesButton = new RadioButton() { Content = "CopySeries", GroupName = "FillSeriesGroup" };
                CopySeriesButton.IsChecked = (SeriesController.InnerFillType & SeriesType.CopySeries) == SeriesType.CopySeries;
                CopySeriesButton.Checked += RadioButtonChecked;
                ItemsSource.Add(CopySeriesButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillSeries) == SeriesType.FillSeries)
            {
                RadioButton FillSeriesButton = new RadioButton() { Content = "FillSeries", GroupName = "FillSeriesGroup" };
                FillSeriesButton.IsChecked = (SeriesController.InnerFillType & SeriesType.FillSeries) == SeriesType.FillSeries;
                FillSeriesButton.Checked += RadioButtonChecked;
                ItemsSource.Add(FillSeriesButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillFormatOnly) == SeriesType.FillFormatOnly)
            {
                RadioButton FillFormatOnlyButton = new RadioButton() { Content = "FillFormatOnly", GroupName = "FillSeriesGroup" };
                FillFormatOnlyButton.IsChecked = (SeriesController.InnerFillType & SeriesType.FillFormatOnly) == SeriesType.FillFormatOnly;
                FillFormatOnlyButton.Checked += RadioButtonChecked;
                ItemsSource.Add(FillFormatOnlyButton);
            }

            if ((SeriesController.Filltype & SeriesType.FillWithoutFormat) == SeriesType.FillWithoutFormat)
            {
                RadioButton FillWithoutFormatButton = new RadioButton() { Content = "FillWithoutFormat", GroupName = "FillSeriesGroup" };
                FillWithoutFormatButton.IsChecked = (SeriesController.InnerFillType & SeriesType.FillWithoutFormat) == SeriesType.FillWithoutFormat;
                FillWithoutFormatButton.Checked += RadioButtonChecked;
                ItemsSource.Add(FillWithoutFormatButton);
            }
        }
        public ObservableCollection<RadioButton> ItemsSource
        {
            get { return (ObservableCollection<RadioButton>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<RadioButton>), typeof(FillDropDownItem), new PropertyMetadata());

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(FillDropDownItem), new PropertyMetadata(false));

        void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (SeriesController != null)
                SeriesController.FillOptionChanged((sender as RadioButton).Content.ToString());
            SplitButton.IsDropDownOpen = false;
        }
        public override void OnApplyTemplate()
        {
            SplitButton = this.GetTemplateChild("dropdownSplitter") as SplitButtonAdv;
        }
    }
}
