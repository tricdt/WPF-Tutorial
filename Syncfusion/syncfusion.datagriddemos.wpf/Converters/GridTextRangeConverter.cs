using Syncfusion.Data;
using System.Globalization;
using System.Windows.Data;

namespace syncfusion.datagriddemos.wpf
{
    public class GridTextRangeConverter : IColumnAccessProvider, IValueConverter
    {
        private string columnName;
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }
        private IPropertyAccessProvider propertyReflector;
        public IPropertyAccessProvider PropertyReflector
        {
            get { return propertyReflector; }
            set { propertyReflector = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textGroupValue = PropertyReflector.GetValue(value, columnName);
            if (textGroupValue != null)
            {
                char start = textGroupValue.ToString().First();
                return start;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
