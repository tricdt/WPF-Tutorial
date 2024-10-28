
using System.Windows;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class FontWeightToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value != null) && (FontWeight)value == FontWeights.Bold)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value != null) && (bool)value)
                return FontWeights.Bold;
            return FontWeights.Normal;
        }
    }
}
