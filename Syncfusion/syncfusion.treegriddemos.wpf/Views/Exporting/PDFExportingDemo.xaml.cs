using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for PDFExportingDemo.xaml
    /// </summary>
    public partial class PDFExportingDemo : DemoControl
    {
        public PDFExportingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            // Release all managed resources
            if (this.treeGrid != null)
            {
                this.treeGrid.Dispose();
                this.treeGrid = null;
            }

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as EmployeeInfoViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }

            if (this.autoRowHeight != null)
                this.autoRowHeight = null;

            if (this.autoColumnWidth != null)
                this.autoColumnWidth = null;

            if (this.exportFormat != null)
                this.exportFormat = null;

            if (this.repeatHeader != null)
                this.repeatHeader = null;

            if (this.fitAllColumns != null)
                this.fitAllColumns = null;

            if (this.pageHeaderandFooter != null)
                this.pageHeaderandFooter = null;

            if (this.customizeColumns != null)
                this.customizeColumns = null;

            if (this.button != null)
                this.button = null;

            base.Dispose(disposing);
        }
    }
}
