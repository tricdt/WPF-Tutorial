using Syncfusion.Data;
using System.ComponentModel;

namespace syncfusion.treegriddemos.wpf
{
    public class CustomComparer : IComparer<object>, ISortDirection
    {
        private ListSortDirection _SortDirection;
        public ListSortDirection SortDirection
        {
            get { return _SortDirection; }
            set { _SortDirection = value; }
        }
        public int Compare(object? x, object? y)
        {
            var item1 = x as EmployeeInfo;
            var item2 = y as EmployeeInfo;
            var value1 = item1.FirstName;
            var value2 = item2.FirstName;
            int c = 0;
            if (value1 != null && value2 == null)
            {
                c = 1;
            }
            else if (value1 == null && value2 != null)
            {
                c = -1;
            }
            else if (value1 != null && value2 != null)
            {
                c = value1.Length.CompareTo(value2.Length);
            }

            if (SortDirection == ListSortDirection.Descending)
                c = -c;

            return c;
        }
    }
}
