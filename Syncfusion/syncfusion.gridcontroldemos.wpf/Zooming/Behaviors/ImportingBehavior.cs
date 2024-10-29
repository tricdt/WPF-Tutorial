using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ImportingBehavior : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Model.QueryCellInfo += Model_QueryCellInfo;
            base.OnAttached();
        }

        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            List<Order> orders = this.AssociatedObject.DataContext as List<Order>;
            if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex == 0)
            { }
            else if (e.Cell.RowIndex == 0)
            {
                e.Style.CellValue = Order.GetPropertyName(e.Cell.ColumnIndex);
            }
            else if (e.Cell.ColumnIndex == 0)
            {
                e.Style.CellValue = e.Cell.RowIndex;
            }
            else
            {
                e.Style.CellValue = orders[e.Cell.RowIndex - 1][e.Cell.ColumnIndex];
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Model.QueryCellInfo -= Model_QueryCellInfo;
            base.OnDetaching();
        }
    }
}
