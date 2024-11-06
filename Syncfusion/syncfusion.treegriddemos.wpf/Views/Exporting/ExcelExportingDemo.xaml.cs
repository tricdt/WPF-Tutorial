using syncfusion.demoscommon.wpf;


namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelExportingDemo.xaml
    /// </summary>
    public partial class ExcelExportingDemo : DemoControl
    {
        public ExcelExportingDemo(string themename) : base(themename)
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

            if (this.allowOutlining != null)
                this.allowOutlining = null;

            if (this.allowIndentColumn != null)
                this.allowIndentColumn = null;

            if (this.isGridLinesVisible != null)
                this.isGridLinesVisible = null;

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.nodeexpandMode != null)
                this.nodeexpandMode = null;

            if (this.customizeColumns != null)
                this.customizeColumns = null;

            if (this.button != null)
                this.button = null;

            base.Dispose(disposing);
        }
    }
}
