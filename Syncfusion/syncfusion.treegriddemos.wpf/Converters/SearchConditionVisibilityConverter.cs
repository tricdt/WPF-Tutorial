﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.treegriddemos.wpf
{
    public class SearchConditionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = value as int?;
            string text = parameter.ToString();
            if (text.Equals("SearchCondition"))
            {
                if (index > 0)
                    return Visibility.Visible;
            }
            else if (text.Equals("NumericComboBox"))
            {
                if (index == 3 || index == 6)
                    return Visibility.Visible;
            }
            else if (text.Equals("StringComboBox"))
            {
                if (index == 1 || index == 2 || index == 4 || index == 5)
                    return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
