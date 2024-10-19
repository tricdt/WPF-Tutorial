using Syncfusion.Windows.Data;
using System.Collections;
using System.ComponentModel;
namespace syncfusion.gridcontroldemos.wpf
{
    public class DayChangeAggregate : ISummaryAggregate
    {
        public DayChangeAggregate()
        {

        }
        public double DayChange
        {
            get;
            set;
        }
        public Action<IEnumerable, string, PropertyDescriptor> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var enumerableItems = items as IEnumerable<Queries.CurrentHoldings>;
                if (pd.Name == "DayChange")
                {
                    this.DayChange = enumerableItems.DayChange<Queries.CurrentHoldings>(q => q.DayChange);
                }
            };
        }
    }
    public static class LinqExtensions
    {
        public static double DayChange<T>(this IEnumerable<T> values, Func<T, double?> selector)
        {
            double ret = 0;
            if (values != null && values.Count() > 0)
            {
                ret = values.Select(selector).Sum(d =>
                {
                    if (d.HasValue)
                    {
                        return d.Value;
                    }
                    return 0.0;
                });
            }
            return ret;
        }
    }
}
