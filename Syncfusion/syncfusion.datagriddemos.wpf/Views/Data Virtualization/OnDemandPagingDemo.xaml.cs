using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for OnDemandPagingDemo.xaml
    /// </summary>
    public partial class OnDemandPagingDemo : DemoControl
    {
        public OnDemandPagingDemo(string themename) : base(themename)
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

            if (this.sfDataPager != null)
            {
                this.sfDataPager.Dispose();
                this.sfDataPager = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
