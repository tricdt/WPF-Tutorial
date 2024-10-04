using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ExportingDemo.xaml
    /// </summary>
    public partial class ExportingDemo : DemoControl
    {
        public ExportingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.customizeSelectedRow != null)
                this.customizeSelectedRow = null;

            if (this.allowOutlining != null)
                this.allowOutlining = null;

            if (this.customizeColumns != null)
                this.customizeColumns = null;

            if (this.customizeSelectedRow != null)
                this.customizeSelectedRow = null;

            if (this.Button1 != null)
                this.Button1 = null;

            if (this.Button2 != null)
                this.Button2 = null;



            base.Dispose(disposing);
        }
    }
}
