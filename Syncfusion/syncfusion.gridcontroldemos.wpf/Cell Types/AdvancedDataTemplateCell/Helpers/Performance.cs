﻿
namespace syncfusion.gridcontroldemos.wpf
{
    public class Performance
    {
        public Performance() { }
        public Performance(DateTime date, double value) { Date = date; AssetValue = value; }
        public DateTime Date { get; set; }
        public double AssetValue { get; set; }
    }
}
