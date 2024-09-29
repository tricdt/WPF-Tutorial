using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class ChangeForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = value as double?;
            if ((parameter as string) != null && parameter.ToString() == "ConditionalFormattingDemo")
            {
                if (data != null && data > 3000000.00)
                    return new SolidColorBrush(Colors.Green);
                else
                    return new SolidColorBrush(Colors.Red);
            }
            else
            {
                var theme = SfSkinManager.GetTheme(parameter as SfDataGrid);
                if (theme != null && theme.ThemeName.Equals("Windows11Light"))
                {
                    if (data < 0.0)
                        return new SolidColorBrush(Color.FromRgb(196, 43, 28));
                    else
                        return new SolidColorBrush(Color.FromRgb(15, 123, 15));
                }
                else if (theme != null && theme.ThemeName.Equals("Windows11Dark"))
                {

                    if (data < 0.0)
                        return new SolidColorBrush(Color.FromRgb(255, 153, 164));
                    else
                        return new SolidColorBrush(Color.FromRgb(108, 203, 95));

                }
                else
                {
                    if (data < 0.0)
                        return new SolidColorBrush(Colors.Red);
                    else
                        return new SolidColorBrush(Colors.Green);
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
