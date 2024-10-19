using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExcelLikeUiHorizontalAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (HorizontalAlignment)value == StringToHorizontalAlignment(parameter))
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "Left") return HorizontalAlignment.Left;
            else if (parameter.ToString() == "Right") return HorizontalAlignment.Right;
            return HorizontalAlignment.Center;
        }
        private HorizontalAlignment StringToHorizontalAlignment(object param)
        {
            if (param.ToString() == "Left") return HorizontalAlignment.Left;
            else if (param.ToString() == "Right") return HorizontalAlignment.Right;
            return HorizontalAlignment.Center;
        }
    }
}
