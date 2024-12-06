using System.Globalization;
using System.Windows.Data;

namespace LedSignControl
{
    public class FillTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (SeriesType)value == SeriesType.CopySeries && parameter.ToString() == "CopySeries")
                return true;
            else if (value != null && (SeriesType)value == SeriesType.FillSeries && parameter.ToString() == "FillSeries")
                return true;
            else if (value != null && (SeriesType)value == SeriesType.FillFormatOnly && parameter.ToString() == "FillFormatOnly")
                return true;
            else if (value != null && (SeriesType)value == SeriesType.FillWithoutFormat && parameter.ToString() == "FillWithoutFormat")
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "CopySeries")
                return SeriesType.CopySeries;
            else if (parameter.ToString() == "FillSeries")
                return SeriesType.FillSeries;
            else if (parameter.ToString() == "FillFormatOnly")
                return SeriesType.FillFormatOnly;
            return SeriesType.FillWithoutFormat;
        }
    }
}
