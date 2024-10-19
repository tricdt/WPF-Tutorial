using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;

namespace syncfusion.datagriddemos.wpf
{
    public class DataGridFilterTrigger : TargetedTriggerAction<SfDataGrid>
    {
        protected override void Invoke(object parameter)
        {
            var viewModel = this.Target.DataContext as EmployeeInfoViewModel;
            viewModel.filterChanged += OnFilterChanged;
        }
        private void OnFilterChanged()
        {
            var viewModel = this.Target.DataContext as EmployeeInfoViewModel;
            if (this.Target.View != null)
            {
                this.Target.View.Filter = viewModel.FilerRecords;
                this.Target.View.RefreshFilter();
            }
        }
    }
}
