using System.Windows.Data;

namespace syncfusion.datagriddemos.wpf
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string str = value.ToString();

                if (str == "UK.jpg")
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/UK.jpg";
                else if (str == "US.jpg")
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/US.jpg";
                else if (str == "Japan.jpg")
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/Japan.jpg";

                else if (str.Equals("Total Sales By Month"))
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/icon_total.png";
                else if (str.Equals("Average of Selected Rows"))
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/icon_add.png";
                else if (str.Equals("Summary of Selected Rows"))
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/icon_sum.png";
                else if (str.Equals("Percent of Selected Rows"))
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/icon_percentage.png";

                else if (str == "Sufficient")
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/Sufficient.png";
                else if (str == "Insufficient")
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/Insufficient.png";
                else if (str == "Perfect")
                    return @"/syncfusion.datagriddemos.wpf;component/Assets/datagrid/Perfect.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
