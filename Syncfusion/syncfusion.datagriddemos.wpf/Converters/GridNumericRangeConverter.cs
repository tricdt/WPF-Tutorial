using Syncfusion.Data;
using System.Globalization;
using System.Windows.Data;

namespace syncfusion.datagriddemos.wpf
{
    public class GridNumericRangeConverter : IValueConverter, IColumnAccessProvider
    {
        private int groupInterval;

        public int GroupInterval
        {
            get { return groupInterval; }
            set { groupInterval = value; }
        }
        private string columnName;

        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }
        private IPropertyAccessProvider _propertyReflector;

        public IPropertyAccessProvider PropertyReflector
        {
            get { return _propertyReflector; }
            set { _propertyReflector = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this.GroupInterval == 0) return null;
            var numericGroupValue = PropertyReflector.GetValue(value, columnName);
            if (numericGroupValue != null)
            {
                var columnvalue = System.Convert.ToInt32(numericGroupValue);
                var i = (int)columnvalue / this.GroupInterval;
                int GroupingIntervalFrom = (int)i * this.GroupInterval;
                int GroupingIntervalTo = ((int)i + 1) * this.GroupInterval;
                return GroupingIntervalFrom.ToString(CultureInfo.InvariantCulture) + " to " + GroupingIntervalTo.ToString(CultureInfo.InvariantCulture);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
