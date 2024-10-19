using syncfusion.demoscommon.wpf;
namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for AlternateRowStyleDemo.xaml
    /// </summary>
    public partial class AlternateRowStyleDemo : DemoControl
    {
        public AlternateRowStyleDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.DataContext != null)
                this.DataContext = null;

            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (rowBackgroundPicker != null)
                rowBackgroundPicker = null;

            if (alternatingRowBackgroundPicker != null)
                alternatingRowBackgroundPicker = null;

            base.Dispose(disposing);
        }
    }
}
