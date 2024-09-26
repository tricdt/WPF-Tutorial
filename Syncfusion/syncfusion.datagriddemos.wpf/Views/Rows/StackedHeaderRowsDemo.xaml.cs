using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for StackedHeaderRowsDemo.xaml
    /// </summary>
    public partial class StackedHeaderRowsDemo : DemoControl
    {
        public StackedHeaderRowsDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
