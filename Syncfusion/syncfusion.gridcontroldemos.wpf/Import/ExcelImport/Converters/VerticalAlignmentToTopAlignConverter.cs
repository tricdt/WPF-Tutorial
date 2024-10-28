
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class VerticalAlignmentToTopAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && (VerticalAlignment)value == VerticalAlignment.Top)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return VerticalAlignment.Top;
        }
    }
}
