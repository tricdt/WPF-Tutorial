using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.TreeGrid;
using System.Diagnostics;

namespace syncfusion.treegriddemos.wpf
{
    public class SfTreeGridBehavior : Behavior<SfTreeGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
            this.AssociatedObject.CurrentCellRequestNavigate += AssociatedObject_CurrentCellRequestNavigate;
        }

        private void AssociatedObject_CurrentCellRequestNavigate(object? sender, Syncfusion.UI.Xaml.Grid.CurrentCellRequestNavigateEventArgs e)
        {
            string address = "https://en.wikipedia.org/wiki/" + e.NavigateText;
            Process.Start(new ProcessStartInfo(address));
        }

        private void AssociatedObject_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var viewModel = this.AssociatedObject.DataContext as EmployeeInfoViewModel;
            if (viewModel != null)
                viewModel.filterChanged = OnFilterChanged;
        }

        private void OnFilterChanged()
        {
            var treeGrid = this.AssociatedObject as SfTreeGrid;
            var viewModel = this.AssociatedObject.DataContext as EmployeeInfoViewModel;
            if (treeGrid.View != null)
            {
                treeGrid.View.Filter = viewModel.FilerRecords;
                treeGrid.View.RefreshFilter();
            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            this.AssociatedObject.CurrentCellRequestNavigate -= AssociatedObject_CurrentCellRequestNavigate;
            base.OnDetaching();
        }
    }
}
