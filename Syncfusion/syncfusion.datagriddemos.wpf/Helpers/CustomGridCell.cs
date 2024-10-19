﻿using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Grid;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace syncfusion.datagriddemos.wpf
{
    public class CustomGridCell : GridCell
    {
        /// <summary>
        /// Get or sets the animation from Transparent to CadetBlue.
        /// </summary>
        public ColorAnimation ColorAnimation1 { get; set; }

        /// <summary>
        /// Gets or sets the animation from Transparent to Tomato.
        /// </summary>
        public ColorAnimation ColorAnimation2 { get; set; }

        /// <summary>
        /// PropertyDescriptorCollection of the view.
        /// </summary>
        public PropertyDescriptorCollection itemProperties;
        public override void OnApplyTemplate()
        {
            itemProperties = this.ColumnBase.Renderer.DataGrid.View.GetItemProperties();
            var BackgroundBrush = new SolidColorBrush { Color = Colors.Transparent };
            if (ColorAnimation1 == null)
                ColorAnimation1 = new ColorAnimation
                {
                    From = Colors.Transparent,
                    To = Colors.CadetBlue,
                    Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
                    AutoReverse = true,
                    FillBehavior = FillBehavior.HoldEnd
                };

            if (ColorAnimation2 == null)
                ColorAnimation2 = new ColorAnimation
                {
                    From = Colors.Transparent,
                    To = Colors.Tomato,
                    Duration = new Duration(TimeSpan.FromMilliseconds(1000)),
                    AutoReverse = true,
                    FillBehavior = FillBehavior.HoldEnd
                };
            this.Background = BackgroundBrush;
            this.BorderBrush = Brushes.LightGray;

            if (this.Content != null && this.Content is TextBlock)
            {
                (this.Content as TextBlock).Style = null;
            }
            UnWireEvents();
            WireEvents();
            base.OnApplyTemplate();
        }

        private void WireEvents()
        {
            this.DataContextChanged += CustomGridCell_DataContextChanged;
            var data = this.DataContext as INotifyPropertyChanged;
            if (data != null)
            {
                data.PropertyChanged += OnDataPropertyChanged;
            }
        }

        private void OnDataPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            var columnMappingName = this.ColumnBase.GridColumn.MappingName;
            var d = itemProperties.GetValue(this.DataContext, columnMappingName).ToString();

            //Begin the animation for Change Column
            if (e.PropertyName == columnMappingName && columnMappingName == "Change")
            {
                if (double.Parse(d) <= -0.5)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation1);
                else if (double.Parse(d) > -0.5)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation2);
            }

            //Begin the animation for LastTrade Column
            if (e.PropertyName == columnMappingName && columnMappingName == "LastTrade")
            {
                if (double.Parse(d) <= 25)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation2);
                else if (double.Parse(d) > 25)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation1);
            }

            //Begin the animation for Open Column
            if (e.PropertyName == columnMappingName && columnMappingName == "Open")
            {
                if (double.Parse(d) <= 25)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation1);
                else if (double.Parse(d) > 25)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation2);
            }

            //Begin the animation for PreviousClose Column
            if (e.PropertyName == columnMappingName && columnMappingName == "PreviousClose")
            {
                if (double.Parse(d) <= 10)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation2);
                else if (double.Parse(d) > 10)
                    this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ColorAnimation1);
            }
        }

        private void CustomGridCell_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UnWireEvents(e.OldValue);
            WireEvents();
        }

        private void UnWireEvents(object datContext = null)
        {
            this.DataContextChanged -= CustomGridCell_DataContextChanged;
            var data = (datContext ?? this.DataContext) as INotifyPropertyChanged;
            if (data != null)
                data.PropertyChanged -= OnDataPropertyChanged;
        }
    }

}
