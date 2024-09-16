﻿namespace syncfusion.demoscommon.wpf
{
    public enum DateRange
    {
        BeyondNextMonth = 0,
        NextMonth = 1,
        LaterofThisMonth = 2,
        ThreeWeeksAway = 3,
        TwoWeeksAway = 4,
        NextWeek = 5,
        Sunday = 6,
        Monday = 7,
        TuesDay = 8,
        WednesDay = 9,
        Thursday = 10,
        Friday = 11,
        Saturday = 12,
        Tomorrow = 13,
        Today = 14,
        Yesterday = 15,
        LastWeek = 23,
        TwoWeeksAgo = 24,
        ThreeWeeksAgo = 25,
        EarlierofthisMonth = 26,
        LastMonth = 27,
        Older = 28
    }

    /// <summary>
    /// GroupingMode provide the options for the Grouping of Date-Time column.
    /// </summary>
    public enum DateGroupingMode
    {
        Range,
        Month,
        Week,
        Year
    }
}
