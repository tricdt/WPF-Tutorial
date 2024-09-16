using syncfusion.demoscommon.wpf;
using Syncfusion.Data;
using System.ComponentModel;

namespace syncfusion.datagriddemos.wpf
{
    public class CustomGroupDateTimeComparer : IComparer<object>, IColumnAccessProvider, ISortDirection
    {
        private DateGroupingMode groupMode;
        public DateGroupingMode GroupMode
        {
            get { return this.groupMode; }
            set { groupMode = value; }
        }
        public string ColumnName { get; set; }
        public IPropertyAccessProvider PropertyReflector { get; set; }
        public ListSortDirection SortDirection { get; set; }

        public int Compare(object? x, object? y)
        {
            var group = (x as Group);
            var group1 = (y as Group);
            for (int i = 0; i < (x as Group).GetTopLevelGroup().GroupDescriptions.Count; i++)
            {
                if (group.Records == null)
                {
                    group = group.Groups.FirstOrDefault() as Group;
                    group1 = group1.Groups.FirstOrDefault() as Group;
                }
            }
            object record = group.Records.FirstOrDefault().Data;
            object record1 = group1.Records.FirstOrDefault().Data;
            var key1 = PropertyReflector.GetValue(record, ColumnName);
            var key2 = PropertyReflector.GetValue(record1, ColumnName);
            var ColumnType = key1.GetType();
            int compareFirstValue = 0, compareSecondValue = 0;
            DateTime date = (DateTime)key1;
            DateTime date1 = (DateTime)key2;
            if (GroupMode == DateGroupingMode.Month)
            {
                compareFirstValue = date.Month;
                compareSecondValue = date1.Month;
            }
            else if (GroupMode == DateGroupingMode.Year)
            {
                compareFirstValue = date.Year;
                compareSecondValue = date.Year;
            }
            else if (GroupMode == DateGroupingMode.Week)
            {
                compareFirstValue = (int)(DateRange)date.DayOfWeek;
                compareSecondValue = (int)(DateRange)date1.DayOfWeek;
            }
            else
            {
                var dt = (DateRange)group.Key;
                var dt1 = (DateRange)group1.Key;
                compareFirstValue = (int)dt;
                compareSecondValue = (int)dt1;
            }
            var diff = compareFirstValue.CompareTo(compareSecondValue);

            if (diff > 0)
                return SortDirection == ListSortDirection.Ascending ? 1 : -1;
            if (diff == -1)
                return SortDirection == ListSortDirection.Ascending ? -1 : 1;
            return 0;
        }
    }
    public class CustomGroupNumericComparer : IComparer<object>, IColumnAccessProvider, ISortDirection
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
        private ListSortDirection sortDirection;
        public ListSortDirection SortDirection
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }

        public int Compare(object? x, object? y)
        {
            var group = (x as Group);
            var group1 = (y as Group);
            for (int i = 0; i < (x as Group).GetTopLevelGroup().GroupDescriptions.Count; i++)
            {
                if (group.Records == null)
                {
                    group = group.Groups.FirstOrDefault() as Group;
                    group1 = group1.Groups.FirstOrDefault() as Group;
                }
            }
            object record = group.Records.FirstOrDefault().Data;
            object record1 = group1.Records.FirstOrDefault().Data;
            var key1 = PropertyReflector.GetValue(record, ColumnName);
            var key2 = PropertyReflector.GetValue(record1, ColumnName);
            int compareFirstValue = Convert.ToInt32(key1);
            int compareSecondValue = Convert.ToInt32(key2);
            var diff = compareFirstValue.CompareTo(compareSecondValue);

            if (diff > 0)
                return SortDirection == ListSortDirection.Ascending ? 1 : -1;
            if (diff == -1)
                return SortDirection == ListSortDirection.Ascending ? -1 : 1;
            return 0;
        }
    }
    public class CustomGroupTextComparer : IComparer<object>, IColumnAccessProvider, ISortDirection
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
        private ListSortDirection sortDirection;
        public ListSortDirection SortDirection
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }

        public int Compare(object? x, object? y)
        {
            var group = (x as Group);
            var group1 = (y as Group);
            for (int i = 0; i < (x as Group).GetTopLevelGroup().GroupDescriptions.Count; i++)
            {
                if (group.Records == null)
                {
                    group = group.Groups.FirstOrDefault() as Group;
                    group1 = group1.Groups.FirstOrDefault() as Group;
                }
            }
            object record = group.Records.FirstOrDefault().Data;
            object record1 = group1.Records.FirstOrDefault().Data;
            var key1 = PropertyReflector.GetValue(record, ColumnName);
            var key2 = PropertyReflector.GetValue(record1, ColumnName);
            char first = key1.ToString().First();
            char second = key2.ToString().First();
            int compareFirstValue = Convert.ToInt32(first);
            int compareSecondValue = Convert.ToInt32(second);
            var diff = compareFirstValue.CompareTo(compareSecondValue);

            if (diff > 0)
                return SortDirection == ListSortDirection.Ascending ? 1 : -1;
            if (diff == -1)
                return SortDirection == ListSortDirection.Ascending ? -1 : 1;
            return 0;
        }
    }
}
