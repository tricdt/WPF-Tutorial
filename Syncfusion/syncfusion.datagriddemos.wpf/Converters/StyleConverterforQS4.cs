using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class StyleConverterforQS4 : IValueConverter
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
                if (_value < 6600000 && _value > 1000000)
                    return conditionalFormattingDemo.Resources["Brush3"];
            }
            return new SolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
