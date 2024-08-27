using NavigationMVVM.ViewModels;
using System.Globalization;
using System.Windows.Data;

namespace NavigationMVVM.Converters
{
    public class EqualValueToParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
                if (value is LayoutViewModel)
                    return (value as LayoutViewModel).ContentViewModel.ToString() == parameter.ToString();
                else
                    return value.ToString() == parameter.ToString();
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
