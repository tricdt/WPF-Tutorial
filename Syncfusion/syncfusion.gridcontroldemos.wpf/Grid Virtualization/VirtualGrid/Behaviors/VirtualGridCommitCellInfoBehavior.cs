using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    public class VirtualGridCommitCellInfoBehavior : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Model.CommittedCellInfo += new GridCommitCellInfoEventHandler(Model_CommittedCellInfo);
        }

        private void Model_CommittedCellInfo(object sender, GridCommitCellInfoEventArgs e)
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
            base.OnDetaching();
            this.AssociatedObject.Model.CommittedCellInfo -= new GridCommitCellInfoEventHandler(Model_CommittedCellInfo);
        }
    }
}
