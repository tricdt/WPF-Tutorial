using syncfusion.demoscommon.wpf;
namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for StylingDemo.xaml
    /// </summary>
    public partial class StylingDemo : DemoControl
    {
        public StylingDemo()
        {
            InitializeComponent();
            //Syncfusion.Windows.Tools.Controls.SortDirectionToVisibilityConverter
        }
        protected override void Dispose(bool disposing)
        {
            if (this.DataContext != null)
                this.DataContext = null;

            //Release all managed resources
            if (this.sfGrid != null)
            {
                this.sfGrid.Dispose();
                this.sfGrid = null;
            }
            base.Dispose(disposing);
        }
    }
}
