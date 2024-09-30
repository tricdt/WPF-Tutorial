using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CellAnimationDemo.xaml
    /// </summary>
    public partial class CellAnimationDemo : DemoControl
    {
        public CellAnimationDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.datagrid != null)
            {
                this.datagrid.Dispose();
                this.datagrid = null;
            }
            base.Dispose(disposing);
        }
    }
}
