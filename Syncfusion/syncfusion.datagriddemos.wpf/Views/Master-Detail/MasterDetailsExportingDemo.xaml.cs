using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for MasterDetailsExportingDemo.xaml
    /// </summary>
    public partial class MasterDetailsExportingDemo : DemoControl
    {
        public MasterDetailsExportingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.customizeColumns != null)
                this.customizeColumns = null;

            if (this.button1 != null)
                this.button1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.customizeSelectedRow != null)
                this.customizeSelectedRow = null;

            if (this.button2 != null)
                this.button2 = null;

            base.Dispose(disposing);
        }
    }
}
