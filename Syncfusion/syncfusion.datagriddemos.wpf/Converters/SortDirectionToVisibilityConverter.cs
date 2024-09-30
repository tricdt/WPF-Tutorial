using Syncfusion.Windows.Tools.Controls;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace syncfusion.datagriddemos.wpf
{
    public class SortDirectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Visibility.Hidden;
            if (((SortingDirection)value == SortingDirection.Ascending && parameter.ToString() == "Ascending") || ((SortingDirection)value == SortingDirection.Descending && parameter.ToString() == "Descending"))
            {
                return Visibility.Visible;
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
