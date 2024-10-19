using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CellSelectionDemo.xaml
    /// </summary>
    public partial class CellSelectionDemo : DemoControl
    {
        public CellSelectionDemo(string themename) : base(themename)
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

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.cmbSelectionMode != null)
                this.cmbSelectionMode = null;

            if (this.ckbSelectionOnPointerPressed != null)
                this.ckbSelectionOnPointerPressed = null;

            base.Dispose(disposing);
        }
    }
}
