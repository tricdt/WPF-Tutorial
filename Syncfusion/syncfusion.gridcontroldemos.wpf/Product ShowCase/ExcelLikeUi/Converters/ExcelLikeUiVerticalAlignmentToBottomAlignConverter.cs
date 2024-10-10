using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiVerticalAlignmentToBottomAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (VerticalAlignment)value == VerticalAlignment.Bottom)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return VerticalAlignment.Bottom;
        }
    }
}
