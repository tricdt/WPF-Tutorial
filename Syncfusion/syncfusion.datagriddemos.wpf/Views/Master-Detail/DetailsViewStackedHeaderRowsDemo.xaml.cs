using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DetailsViewStackedHeaderRowsDemo.xaml
    /// </summary>
    public partial class DetailsViewStackedHeaderRowsDemo : DemoControl
    {
        public DetailsViewStackedHeaderRowsDemo(string themename) : base(themename)
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

            base.Dispose(disposing);
        }
    }
}
