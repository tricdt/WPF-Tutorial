using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    public class ExportingBehavior : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Model.CommitCellInfo += Model_CommitCellInfo;
            base.OnAttached();
        }

        private void Model_CommitCellInfo(object sender, GridCommitCellInfoEventArgs e)
        {
            List<Order> orders = this.AssociatedObject.DataContext as List<Order>;
            if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 0 && e.Style.HasCellValue)
            {
                orders[e.Cell.RowIndex - 1][e.Cell.ColumnIndex] = e.Style.CellValue.ToString();
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Model.CommitCellInfo -= Model_CommitCellInfo;
            base.OnDetaching();
        }
    }
}
