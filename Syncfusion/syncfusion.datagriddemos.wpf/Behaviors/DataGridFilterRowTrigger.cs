using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.UI.Xaml.ScrollAxis;

namespace syncfusion.datagriddemos.wpf
{
    public class DataGridFilterRowTrigger : TargetedTriggerAction<SfDataGrid>
    {
        protected override void Invoke(object parameter)
        {
            this.Target.MoveCurrentCell(new RowColumnIndex(this.Target.GetFilterRowIndex(), this.Target.GetFirstColumnIndex()));
            this.Target.SelectionController.CurrentCellManager.BeginEdit();
        }
    }
}
