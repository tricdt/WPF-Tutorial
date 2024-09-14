﻿
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class StatusConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="info">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo info)
        {
            var theme = SfSkinManager.GetTheme(parameter as SfDataGrid);
            if (theme != null && theme.ThemeName.Equals("Windows11Light"))
            {
                if (value == null)
                    return null;
                if ((string)value == "Active")
                    return new SolidColorBrush(Color.FromRgb(15, 123, 15));

                else if ((string)value == "Inactive")
                    return new SolidColorBrush(Color.FromRgb(196, 43, 28));
            }
            else if (theme != null && theme.ThemeName.Equals("Windows11Dark"))
            {
                if (value == null)
                    return null;
                if ((string)value == "Active")
                    return new SolidColorBrush(Color.FromRgb(108, 203, 95));
                else if ((string)value == "Inactive")
                    return new SolidColorBrush(Color.FromRgb(255, 153, 164));
            }
            else
            {
                if (value == null)
                    return null;
                if ((string)value == "Active")
                    return new SolidColorBrush(Colors.Green);
                else if ((string)value == "Inactive")
                    return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Black);

        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="info">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
