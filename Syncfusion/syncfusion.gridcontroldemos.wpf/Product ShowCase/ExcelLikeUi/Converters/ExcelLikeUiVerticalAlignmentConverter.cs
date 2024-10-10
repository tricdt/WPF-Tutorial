using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiVerticalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (VerticalAlignment)value == StringToVerticalAlignment(parameter))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "Top") return VerticalAlignment.Top;
            else if (parameter.ToString() == "Center") return VerticalAlignment.Center;
            return VerticalAlignment.Bottom;
        }
        private VerticalAlignment StringToVerticalAlignment(object param)
        {
            if (param.ToString() == "Top") return VerticalAlignment.Top;
            else if (param.ToString() == "Center") return VerticalAlignment.Center;
            return VerticalAlignment.Bottom;
        }
    }
}
