using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class StyleConverterforQS2 : IValueConverter
    {
        internal ConditionalFormattingDemo conditionalFormattingDemo;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (conditionalFormattingDemo == null)
                conditionalFormattingDemo = (ConditionalFormattingDemo)Activator.CreateInstance(typeof(ConditionalFormattingDemo));

            double _value;
            if (!String.IsNullOrEmpty(value as string))
            {
                _value = double.Parse(value.ToString(), NumberStyles.Currency);
                if (_value < 4000000.00 && _value > 1000000.00)
                    return conditionalFormattingDemo.Resources["Brush1"];
            }
            return new SolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
