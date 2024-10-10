﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush SolidBrush = new SolidColorBrush((Color)value);
            return SolidBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
