﻿
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class HorizontalAlignmentToCenterAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (HorizontalAlignment)value == HorizontalAlignment.Center)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return HorizontalAlignment.Center;
        }
    }
}
