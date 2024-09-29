using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;

namespace syncfusion.datagriddemos.wpf
{
    public class CheckBoxSelectorColumnTrigger : TargetedTriggerAction<SfDataGrid>
    {
        protected override void Invoke(object parameter)
        {
            var items = this.Target.ItemsSource as List<ProductInfo>;
            this.Target.SelectedItems.Add(items[4]);
            this.Target.SelectedItems.Add(items[6]);
            this.Target.SelectedItems.Add(items[10]);
            this.Target.SelectedItems.Add(items[15]);
            this.Target.SelectedItems.Add(items[16]);
        }
    }
}
