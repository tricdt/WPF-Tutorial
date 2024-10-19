using System.Windows;
using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    public class ConditionalFormattingStyleSelector : StyleSelector
    {
        internal ConditionalFormattingDemo conditionalFormattingDemo;
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (conditionalFormattingDemo == null)
                conditionalFormattingDemo = (ConditionalFormattingDemo)Activator.CreateInstance(typeof(ConditionalFormattingDemo));
            return conditionalFormattingDemo.Resources["normaltableSummaryCell"] as Style;
        }
    }
}
