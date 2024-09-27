using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
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
            if (this.DataContext != null)
                this.DataContext = null;

            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.columnsizerCombo != null)
                this.columnsizerCombo = null;

            base.Dispose(disposing);
        }
    }
}
