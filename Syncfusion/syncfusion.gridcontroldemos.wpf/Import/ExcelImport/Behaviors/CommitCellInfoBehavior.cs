using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    public class CommitCellInfoBehavior : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Model.CommitCellInfo += new GridCommitCellInfoEventHandler(Model_CommitCellInfo);
            base.OnAttached();
        }

        private void Model_CommitCellInfo(object sender, GridCommitCellInfoEventArgs e)
        {
            Dictionary<RowColumnIndex, object> committedValues = this.AssociatedObject.DataContext as Dictionary<RowColumnIndex, object>;
            if (committedValues != null && e.Style.HasCellValue)
            {
                committedValues[e.Cell] = e.Style.CellValue;
                e.Handled = true;
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Model.CommitCellInfo -= new GridCommitCellInfoEventHandler(Model_CommitCellInfo);
            base.OnDetaching();
        }
    }
}
