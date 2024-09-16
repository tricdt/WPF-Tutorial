﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Data;
using System.Globalization;
using System.Windows.Data;

namespace syncfusion.datagriddemos.wpf
{
    public class GridDateTimeRangeConverter : IValueConverter, IColumnAccessProvider
    {
        private string columnName;
        public string ColumnName { get => columnName; set => columnName = value; }

        public IPropertyAccessProvider PropertyReflector { get; set; }

        private DateGroupingMode groupingMode;
        public DateGroupingMode GroupMode
        {
            get => groupingMode; set => groupingMode = value;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateGroupValue = PropertyReflector.GetValue(value, columnName);
            if (dateGroupValue != null)
            {
                if (this.GroupMode == DateGroupingMode.Year)
                {
                    DateTime year = (DateTime)dateGroupValue;
                    return year.Year;
                }
                else if (this.GroupMode == DateGroupingMode.Month)
                {
                    DateTime month = (DateTime)dateGroupValue;
                    //return month.Month;
                    return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Month);
                }
                else if (this.GroupMode == DateGroupingMode.Week)
                {
                    DateTime week = (DateTime)dateGroupValue;
                    return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(week.DayOfWeek);
                }
                else
                {
                    DateTime date = (DateTime)dateGroupValue;
                    var dt = DateTime.Now;
                    var days = (int)Math.Floor((dt - date).TotalDays);
                    var dayofweek = (int)dt.DayOfWeek;
                    if (days > 0)
                    {
                        var diff = days - dayofweek;
                        if (days <= dayofweek)
                        {
                            if (days == 0)
                                return DateRange.Today;
                            if (days == 1)
                                return DateRange.Yesterday;
                            return GetValue(date.DayOfWeek.ToString());
                        }
                        if (diff > 0 && diff <= 7)
                            return DateRange.LastWeek;
                        if (diff > 7 && diff <= 14)
                            return DateRange.TwoWeeksAgo;
                        if (diff > 14 && diff <= 21)
                            return DateRange.ThreeWeeksAgo;
                        if (dt.Year == date.Year && dt.Month == date.Month)
                            return DateRange.EarlierofthisMonth;
                        if (DateTime.Now.AddMonths(-1).Month == date.Month && dt.Year == date.Year)
                            return DateRange.LastMonth;
                        return DateRange.Older;
                    }
                    else
                    {
                        var different = dayofweek - days;
                        if ((-days) <= dayofweek)
                        {
                            if (days == 0)
                                return DateRange.Today;
                            if (days == -1)
                                return DateRange.Tomorrow;
                            return GetValue(date.DayOfWeek.ToString());
                        }
                        if (different > 0 && different <= 7)
                            return DateRange.NextWeek;
                        if (different > 7 && different <= 14)
                            return DateRange.TwoWeeksAway;
                        if (different > 14 && different <= 21)
                            return DateRange.ThreeWeeksAway;
                        if (dt.Year == date.Year && dt.Month == date.Month)
                            return DateRange.LaterofThisMonth;
                        if (DateTime.Now.AddMonths(+1).Month == date.Month && dt.Year == date.Year)
                            return DateRange.NextMonth;
                        return DateRange.BeyondNextMonth;
                    }
                }
            }
            return null;
        }
        public DateRange GetValue(string day)
        {
            if (day == "Sunday")
                return DateRange.Sunday;
            else if (day == "Monday")
                return DateRange.Monday;
            else if (day == "Tuesday")
                return DateRange.TuesDay;
            else if (day == "Wednesday")
                return DateRange.WednesDay;
            else if (day == "Thursday")
                return DateRange.Thursday;
            else if (day == "Friday")
                return DateRange.Friday;
            else
                return DateRange.Saturday;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
