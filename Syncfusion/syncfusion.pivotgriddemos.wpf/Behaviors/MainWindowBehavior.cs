

namespace syncfusion.pivotgriddemos.wpf
{
    using Microsoft.Xaml.Behaviors;
    using Syncfusion.Windows.Controls.PivotGrid;

    class MainWindowBehavior : Behavior<PivotGridControl>
    {
        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to RowPivotsOnlyModelhook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += (s, e) =>
            {
                RowPivotsOnlyViewModel vm = this.AssociatedObject.DataContext as RowPivotsOnlyViewModel;
                AssociatedObject.ItemSource = vm.Data;
            };
        }
    }
}
