﻿using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class StyleConverterforQuantity : IValueConverter
    {
        internal ConditionalFormattingDetailsViewDataGridDemo detailsViewDataGridDemo;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (detailsViewDataGridDemo == null)
                detailsViewDataGridDemo = (ConditionalFormattingDetailsViewDataGridDemo)Activator.CreateInstance(typeof(ConditionalFormattingDetailsViewDataGridDemo));
            double _value;
            if (!String.IsNullOrEmpty(value.ToString()))
            {
                _value = double.Parse(value.ToString(), NumberStyles.Currency, culture);
                if (_value < 6)
                    return detailsViewDataGridDemo.Resources["Brush1"];
            }
            return new SolidColorBrush();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
