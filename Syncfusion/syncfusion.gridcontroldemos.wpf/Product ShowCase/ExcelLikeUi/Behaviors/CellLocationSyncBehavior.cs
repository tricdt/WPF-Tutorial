using Microsoft.Xaml.Behaviors;

namespace syncfusion.gridcontroldemos.wpf
{
    public class CellLocationSyncBehavior : Behavior<SampleGridControl>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.CellLocationTextChanged += OnAssociatedObjectCellLocationTextChanged;
        }

        private void OnAssociatedObjectCellLocationTextChanged(object? sender, EventArgs e)
        {
            var dataContext = AssociatedObject.DataContext as ExcelLikeUiViewModel;
            if (dataContext != null)
            {
                dataContext.CellLocationText = this.AssociatedObject.CellLocationText;
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.CellLocationTextChanged -= OnAssociatedObjectCellLocationTextChanged;
            base.OnDetaching();
        }
    }
}
