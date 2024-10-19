﻿using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class RangeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var range = value as double?;

            var theme = SfSkinManager.GetTheme(parameter as SfDataGrid);
            if (theme != null && theme.ThemeName.Equals("Windows11Light"))
            {
                if (range < 0.0)
                    return new SolidColorBrush(Color.FromRgb(196, 43, 28));
                else
                    return new SolidColorBrush(Color.FromRgb(15, 123, 15));
            }
            else if (theme != null && theme.ThemeName.Equals("Windows11Dark"))
            {

                if (range < 0.0)
                    return new SolidColorBrush(Color.FromRgb(255, 153, 164));
                else
                    return new SolidColorBrush(Color.FromRgb(108, 203, 95));

            }
            else
            {
                if (range < 0.0)
                    return new SolidColorBrush(Colors.Red);
                else
                    return new SolidColorBrush(Colors.Green);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
