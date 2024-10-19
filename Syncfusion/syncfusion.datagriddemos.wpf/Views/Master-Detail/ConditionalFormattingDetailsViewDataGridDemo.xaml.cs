using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ConditionalFormattingDetailsViewDataGridDemo.xaml
    /// </summary>
    public partial class ConditionalFormattingDetailsViewDataGridDemo : DemoControl
    {
        public ConditionalFormattingDetailsViewDataGridDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;


            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            //Release all managed resources
            if (this.Resources["unitpricestyle"] != null)
                (this.Resources["unitpricestyle"] as StyleConverterforUnitPrice).detailsViewDataGridDemo = null;
            if (this.Resources["quantitystyle"] != null)
                (this.Resources["quantitystyle"] as StyleConverterforQuantity).detailsViewDataGridDemo = null;

            this.Resources.Clear();

            base.Dispose(disposing);
        }
    }
}
