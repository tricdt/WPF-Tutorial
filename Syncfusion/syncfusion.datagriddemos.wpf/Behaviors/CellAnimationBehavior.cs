using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;

namespace syncfusion.datagriddemos.wpf
{
    public class CellAnimationBehavior : Behavior<SfDataGrid>
    {
        protected override void OnAttached()
        {
            (this.AssociatedObject as SfDataGrid).RowGenerator = new CellAnimationCustomRowGenerator(this.AssociatedObject as SfDataGrid);
            (this.AssociatedObject as SfDataGrid).Loaded += OnLoaded;
            (this.AssociatedObject as SfDataGrid).Unloaded += OnUnloaded;
            base.OnAttached();
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.AssociatedObject.DataContext as CellAnimationViewModel).StopTimer();
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.AssociatedObject.DataContext as CellAnimationViewModel).StartTimer();
        }
        protected override void OnDetaching()
        {
            (this.AssociatedObject as SfDataGrid).Loaded -= OnLoaded;
            (this.AssociatedObject as SfDataGrid).Unloaded -= OnUnloaded;
            base.OnDetaching();
        }
    }
}
