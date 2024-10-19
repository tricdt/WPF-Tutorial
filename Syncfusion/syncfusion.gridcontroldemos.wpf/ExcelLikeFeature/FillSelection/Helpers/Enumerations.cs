namespace syncfusion.gridcontroldemos.wpf
{
    public enum MovingDirection
    {
        None,
        Left,
        Down,
        Right,
        Up
    }
    public enum SeriesType
    {
        CopySeries = 4,
        FillSeries = 6,
        FillFormatOnly = 8,
        FillWithoutFormat = 10
    }
    public static class FillSeriesExtensions
    {
        public static int GetDateFormat(this string value)
        {
            int format = 0;
            string[] str = DateTime.Parse(value).GetDateTimeFormats();
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == value)
                {
                    return i;
                }
            }
            return format;
        }
    }
}
