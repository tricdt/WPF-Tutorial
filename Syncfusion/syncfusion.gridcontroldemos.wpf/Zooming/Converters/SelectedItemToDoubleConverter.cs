using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace syncfusion.gridcontroldemos.wpf
{
    public class SelectedItemToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem)
            {
                string zoom = (value as ComboBoxItem).Content.ToString().Replace("%", "");
                double zoomscale = double.Parse(zoom);
                return zoomscale / 100;
            }
            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
