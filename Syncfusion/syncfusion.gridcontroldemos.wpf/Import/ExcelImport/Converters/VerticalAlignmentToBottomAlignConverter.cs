
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class VerticalAlignmentToBottomAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (VerticalAlignment)value == VerticalAlignment.Bottom)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return VerticalAlignment.Bottom;
        }
    }
}
