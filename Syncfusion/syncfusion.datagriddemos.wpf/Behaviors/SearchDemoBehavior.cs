using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;
using System.Windows.Input;

namespace syncfusion.datagriddemos.wpf
{
    public class SearchDemoBehavior : Behavior<SearchPanelDemo>
    {
        SfDataGrid dataGrid;
        SearchControl searchControl;
        protected override void OnAttached()
        {
            var window = this.AssociatedObject;
            this.dataGrid = window.FindName("dataGrid") as SfDataGrid;
            this.dataGrid.KeyDown += OnDataGridKeyDown;
            this.searchControl = window.FindName("searchControl") as SearchControl;
        }

        private void OnDataGridKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.F) && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                searchControl.UpdateSearchControlVisiblity(true);
            else if (e.Key == Key.Escape)
                searchControl.UpdateSearchControlVisiblity(false);

        }
        protected override void OnDetaching()
        {
            this.dataGrid.KeyDown -= OnDataGridKeyDown;
        }
    }
}
