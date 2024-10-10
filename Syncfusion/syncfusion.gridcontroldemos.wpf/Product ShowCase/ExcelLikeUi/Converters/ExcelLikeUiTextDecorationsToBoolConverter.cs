using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiTextDecorationsToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (TextDecorationCollection)value == TextDecorations.Underline)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
                return TextDecorations.Underline;
            return null;
        }
    }
}
