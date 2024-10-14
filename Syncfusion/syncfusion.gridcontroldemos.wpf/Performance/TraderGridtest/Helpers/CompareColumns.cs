﻿namespace syncfusion.gridcontroldemos.wpf
{
    public class CompareColumns
    {
        public static int CompareNullableObjects(object x, object y)
        {
            return _Compare(x, y);
        }
        internal static int _Compare(object x, object y)
        {
            int cmp = 0;
            bool xIsNull = (x == null || x is DBNull);
            bool yIsNull = (y == null || y is DBNull);

            if (yIsNull && xIsNull)
                cmp = 0;
            else if (xIsNull)
                cmp = -1;
            else if (yIsNull)
                cmp = 1;
            else if (x is IComparable)
            {
                try
                {
                    cmp = ((IComparable)x).CompareTo(y);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return cmp;
        }
    }
}
