using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    public class CellRequestNavigateBehavior : Behavior<GridControl>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.Model.CellRequestNavigate += new CellRequestNavigateEventHandler(Model_CellRequestNavigate);
        }

        void Model_CellRequestNavigate(object sender, CellRequestNavigateEventArgs e)
        {
            var dataContext = AssociatedObject.DataContext as ExcelImportViewModel;
            if (dataContext != null && dataContext.MainView != null)
                dataContext.MainView.GidCellRequestNavigate(e.Name, e.RowIndex, e.ColumnIndex);
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Model.CellRequestNavigate -= new CellRequestNavigateEventHandler(Model_CellRequestNavigate);
        }
    }
}
