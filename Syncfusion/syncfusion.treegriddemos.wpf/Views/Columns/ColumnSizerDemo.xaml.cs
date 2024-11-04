using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ColumnSizerDemo.xaml
    /// </summary>
    public partial class ColumnSizerDemo : DemoControl
    {
        public ColumnSizerDemo(string themename) : base(themename)
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

            if (this.label != null)
                this.label = null;

            if (this.columnsizerCombo != null)
                this.columnsizerCombo = null;

            base.Dispose(disposing);
        }
    }
}
