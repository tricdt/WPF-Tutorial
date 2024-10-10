using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiHorizontalAlignmentToRightAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (HorizontalAlignment)value == HorizontalAlignment.Right)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return HorizontalAlignment.Right;
        }
    }
}
