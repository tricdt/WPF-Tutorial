using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;

namespace syncfusion.datagriddemos.wpf
{
    public class DataGridRowHeightBehavior : Behavior<SfDataGrid>
    {
        GridRowSizingOptions gridRowResizingOptions = new GridRowSizingOptions();
        List<string> excludeColumns = new List<string>();
        double Height = double.NaN;
        protected override void OnAttached()
        {
            this.AssociatedObject.ItemsSourceChanged += AssociatedObject_ItemsSourceChanged;
            this.AssociatedObject.QueryRowHeight += AssociatedObject_QueryRowHeight;
        }

        private void AssociatedObject_QueryRowHeight(object? sender, QueryRowHeightEventArgs e)
        {
            if (this.AssociatedObject.IsTableSummaryIndex(e.RowIndex))
            {
                e.Height = 50;
                e.Handled = true;
            }
            else if (this.AssociatedObject.GridColumnSizer.GetAutoRowHeight(e.RowIndex, gridRowResizingOptions, out Height))
            {
                if (Height > this.AssociatedObject.RowHeight)
                {
                    e.Height = Height;
                    e.Handled = true;
                }
            }
        }

        private void AssociatedObject_ItemsSourceChanged(object? sender, GridItemsSourceChangedEventArgs e)
        {
            foreach (var column in this.AssociatedObject.Columns)
                if (!column.MappingName.Equals("ProductName"))
                    excludeColumns.Add(column.MappingName);

            gridRowResizingOptions.ExcludeColumns = excludeColumns;
        }
    }
}
