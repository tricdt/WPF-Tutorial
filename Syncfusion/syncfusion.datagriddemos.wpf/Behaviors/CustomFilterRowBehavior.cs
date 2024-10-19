using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
namespace syncfusion.datagriddemos.wpf
{
    public class CustomFilterRowBehavior : Behavior<SfDataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            //Assign the new NumericFilterComboBoxRenderer custom renderer 
            this.AssociatedObject.FilterRowCellRenderers.Add("NumericComboBoxExt", new GridNumericFilterComboBoxRendererExt());
            //Assign the new DateTimeFilterComboBoxRenderer custom renderer
            this.AssociatedObject.FilterRowCellRenderers.Add("DateTimeComboBoxExt", new GridDateTimeComboBoxRendererExt());
            //Assign the new TextFilterComboBoxRenderer custom renderer 
            this.AssociatedObject.FilterRowCellRenderers.Add("TextComboBoxExt", new GridTextFilterComboBoxRendererExt());
            this.AssociatedObject.PreviewMouseDown += OnAssociatedObjectPreviewMouseDown;
        }

        private void OnAssociatedObjectPreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Get the point of SfDataGrid
            var point = e.GetPosition(this.AssociatedObject);
            //Get the VisualCointainer
            var visualContainer = this.AssociatedObject.GetVisualContainer();
            //Get the row and column index based on the point
            var rowColumnIndex = visualContainer.PointToCellRowColumnIndex(point);
            //Clear the filter based on FilerRow and RowHeader
            if (this.AssociatedObject.IsFilterRowIndex(rowColumnIndex.RowIndex) && this.AssociatedObject.ShowRowHeader && rowColumnIndex.ColumnIndex == 0)
                this.AssociatedObject.ClearFilters();
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewMouseDown -= OnAssociatedObjectPreviewMouseDown;
        }
    }
}
