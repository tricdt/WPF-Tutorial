
namespace syncfusion.pivotgriddemos.wpf
{
    using syncfusion.demoscommon.wpf;
    using System.Windows;
    using System.Windows.Controls;

    public partial class CellSelection : DemoControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CellSelection"/> class.
        /// </summary>
        public CellSelection()
        {
            InitializeComponent();
            this.pivotGrid1.Loaded += PivotGrid1_Loaded;
        }

        protected override void Dispose(bool disposing)
        {
            // Release all resources
            this.chkBoxAllowSelection.Checked -= chkBoxAllowSelection_Checked;
            this.chkBoxAllowSelection.Unchecked -= chkBoxAllowSelection_Unchecked;
            this.chkBoxAllowSelectionwithheaders.Checked -= chkBoxAllowSelectionwithheaders_Checked;
            this.chkBoxAllowSelectionwithheaders.Unchecked -= chkBoxAllowSelection_Unchecked;
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
            this.SchemaDesigner.PivotControl = this.pivotGrid1;
        }

        private void chkBoxAllowSelection_Unchecked(object sender, RoutedEventArgs e)
        {
            this.lstSelectedItems.ItemsSource = null;
        }

        private void chkBoxAllowSelection_Checked(object sender, RoutedEventArgs e)
        {
            this.pivotGrid1.AllowSelectionWithHeaders = false;
            this.pivotGrid1.AllowSelection = (sender as RadioButton).IsChecked ?? false;
        }

        private void chkBoxAllowSelectionwithheaders_Checked(object sender, RoutedEventArgs e)
        {
            this.pivotGrid1.AllowSelection = false;
            this.pivotGrid1.AllowSelectionWithHeaders = (sender as RadioButton).IsChecked ?? false;
        }
    }
}