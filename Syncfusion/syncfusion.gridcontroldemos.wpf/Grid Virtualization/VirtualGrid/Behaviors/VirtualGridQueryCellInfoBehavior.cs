using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    public class VirtualGridQueryCellInfoBehavior : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Model.QueryCellInfo += new GridQueryCellInfoEventHandler(Model_QueryCellInfo);
        }

        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            Dictionary<RowColumnIndex, object> committedValues = this.AssociatedObject.DataContext as Dictionary<RowColumnIndex, object>;
            if (committedValues != null)
            {
                if (e.Cell.RowIndex == 0)
                {
                    if (e.Cell.ColumnIndex > 0)
                        e.Style.CellValue = e.Cell.ColumnIndex;
                }
                else if (e.Cell.RowIndex > 0)
                {
                    if (e.Cell.ColumnIndex == 0)
                        e.Style.CellValue = e.Cell.RowIndex;
                    else if (e.Cell.ColumnIndex > 0)
                    {
                        if (committedValues.ContainsKey(e.Cell))
                            e.Style.CellValue = committedValues[e.Cell];
                        else
                            e.Style.CellValue = String.Format("{0}/{1}", e.Cell.RowIndex, e.Cell.ColumnIndex);
                    }
                }
            }
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.Model.QueryCellInfo -= new GridQueryCellInfoEventHandler(Model_QueryCellInfo);
        }
    }
}
