using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for FreezePanesDemo.xaml
    /// </summary>
    public partial class FreezePanesDemo : DemoControl
    {
        public FreezePanesDemo(string themename) : base(themename)
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
            base.Dispose(disposing);
        }
    }
}
