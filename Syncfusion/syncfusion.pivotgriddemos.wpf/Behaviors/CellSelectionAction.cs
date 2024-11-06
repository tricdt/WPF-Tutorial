
namespace syncfusion.pivotgriddemos.wpf
{
    using Microsoft.Xaml.Behaviors;
    using Syncfusion.Windows.Controls.PivotGrid;
    using System.Windows.Controls;

    public class CellSelectionAction : TargetedTriggerAction<ListBox>
    {
        protected override void Invoke(object parameter)
        {
            PivotGridSelectionChangedEventArgs eventArgs = parameter as PivotGridSelectionChangedEventArgs;
            if (eventArgs != null)
            {
                this.Target.ItemsSource = eventArgs.SelectedItems;
            }
        }
    }
}