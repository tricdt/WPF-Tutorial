using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class RowStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var rating = (int)value;
            if (rating == 5)
                return Brushes.Transparent;

            Brush brush = rating < 5 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFD356")) :
                                       new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF70FCA0"));
            if (rating > 5)
            {
                rating = rating - 5;
                brush.Opacity = Math.Abs((double)rating * 2) / 10;
            }
            else
                brush.Opacity = Math.Abs((double)rating * 2) / 10;
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
