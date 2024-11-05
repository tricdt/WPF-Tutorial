

namespace syncfusion.pivotgriddemos.wpf
{
    using syncfusion.demoscommon.wpf;
    using System.Windows;
    using System.Windows.Controls;

    public partial class PivotGridDemo : DemoControl
    {
        public PivotGridDemo()
        {
            InitializeComponent();
            this.pivotGrid1.Loaded += PivotGrid1_Loaded;
        }

        protected override void Dispose(bool disposing)
        {
            // Release all resources           
            if (this.pivotGrid1 != null)
            {
                this.pivotGrid1.Loaded -= PivotGrid1_Loaded;
                this.pivotGrid1.Dispose();
                this.pivotGrid1 = null;
            }
            base.Dispose(disposing);
        }

        private void PivotGrid1_Loaded(object sender, RoutedEventArgs e)
        {
            this.schemaDesigner1.PivotControl = this.pivotGrid1;
        }

        private void fieldListOption_Checked(object sender, RoutedEventArgs e)
        {
            this.schemaDesigner1.ShowDisplayFieldsOnly = (sender as CheckBox).IsChecked ?? false;
        }

        private void RowHeaderAreaAutoSizing_Checked(object sender, RoutedEventArgs e)
        {
            this.pivotGrid1.AllowRowHeaderAreaAutoSizing = (sender as CheckBox).IsChecked ?? false;
        }
    }
}
