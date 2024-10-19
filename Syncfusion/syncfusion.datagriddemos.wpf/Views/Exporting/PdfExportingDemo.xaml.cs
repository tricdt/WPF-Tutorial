using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for PdfExportingDemo.xaml
    /// </summary>
    public partial class PdfExportingDemo : DemoControl
    {
        public PdfExportingDemo(string themename) : base(themename)
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

            if (this.ColumnWidth != null)
                this.ColumnWidth = null;

            if (this.RowHeight != null)
                this.RowHeight = null;

            if (this.ExportGroup != null)
                this.ExportGroup = null;

            if (this.ExportGroupSummary != null)
                this.ExportGroupSummary = null;

            if (this.ExportTableSummary != null)
                this.ExportTableSummary = null;

            if (this.RepeatHeader != null)
                this.RepeatHeader = null;

            if (this.FitAllColumns != null)
                this.FitAllColumns = null;

            if (this.Button1 != null)
                this.Button1 = null;

            if (this.Button2 != null)
                this.Button2 = null;

            base.Dispose(disposing);
        }
    }
}
