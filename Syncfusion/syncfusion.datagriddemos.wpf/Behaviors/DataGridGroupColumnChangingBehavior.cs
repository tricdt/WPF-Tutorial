using Microsoft.Xaml.Behaviors;
using syncfusion.demoscommon.wpf;
using Syncfusion.UI.Xaml.Grid;
namespace syncfusion.datagriddemos.wpf
{
    public class DataGridGroupColumnChangingBehavior : Behavior<SfDataGrid>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.GroupColumnDescriptions.CollectionChanged += GroupColumnDescriptions_CollectionChanged;
        }

        private void GroupColumnDescriptions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                var groupDesc = e.NewItems[0] as GroupColumnDescription;
                if (groupDesc.ColumnName == "OrderID")
                {
                    groupDesc.Converter = new GridNumericRangeConverter() { GroupInterval = 10 };
                    groupDesc.Comparer = new CustomGroupNumericComparer();
                }
                if (groupDesc.ColumnName == "CustomerID")
                {
                    groupDesc.Converter = new GridTextRangeConverter();
                    groupDesc.Comparer = new CustomGroupTextComparer();
                }
                if (groupDesc.ColumnName == "OrderDate")
                {
                    groupDesc.Converter = new GridDateTimeRangeConverter() { GroupMode = DateGroupingMode.Week };
                    groupDesc.Comparer = new CustomGroupDateTimeComparer() { GroupMode = DateGroupingMode.Week };
                }
                if (groupDesc.ColumnName == "ShippedDate")
                {
                    groupDesc.Converter = new GridDateTimeRangeConverter() { GroupMode = DateGroupingMode.Range };
                    groupDesc.Comparer = new CustomGroupDateTimeComparer() { GroupMode = DateGroupingMode.Range };
                }
                if (groupDesc.ColumnName == "Freight")
                {
                    groupDesc.Converter = new GridNumericRangeConverter() { GroupInterval = 10 };
                    groupDesc.Comparer = new CustomGroupNumericComparer();
                }
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.GroupColumnDescriptions.CollectionChanged -= GroupColumnDescriptions_CollectionChanged;
        }
    }
}
