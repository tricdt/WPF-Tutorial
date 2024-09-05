using System.Windows;

namespace syncfusion.demoscommon.wpf
{
    /// <summary>
    /// Convert between boolean and visibility
    /// </summary>
    public class BooleanToVisibilityConverter : BoolToObjectConverter
    {
        public BooleanToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }
    }
}
