using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.TreeGrid;

namespace syncfusion.treegriddemos.wpf
{
    public class QueryCoveredRangeBehavior : Behavior<SfTreeGrid>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.QueryCoveredRange += AssociatedObject_QueryCoveredRange;
            base.OnAttached();
        }

        private void AssociatedObject_QueryCoveredRange(object? sender, TreeGridQueryCoveredRangeEventArgs e)
        {
            var treeNode = this.AssociatedObject.GetNodeAtRowIndex(e.RowColumnIndex.RowIndex);
            if (treeNode != null && treeNode.HasChildNodes)
            {
                if (e.RowColumnIndex.ColumnIndex >= 1 && e.RowColumnIndex.ColumnIndex <= this.AssociatedObject.Columns.Count)
                {
                    e.Range = new TreeGridCoveredCellInfo(0, this.AssociatedObject.Columns.Count, e.RowColumnIndex.RowIndex);
                    e.Handled = true;
                }
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.QueryCoveredRange -= AssociatedObject_QueryCoveredRange;
            base.OnDetaching();
        }
    }
}
