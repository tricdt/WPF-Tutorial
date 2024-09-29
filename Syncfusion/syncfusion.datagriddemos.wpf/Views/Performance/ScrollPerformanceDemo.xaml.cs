using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ScrollPerformanceDemo.xaml
    /// </summary>
    public partial class ScrollPerformanceDemo : DemoControl
    {
        public ScrollPerformanceDemo(string themename) : base(themename)
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

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as StockDetailsViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }

            base.Dispose(disposing);
        }
    }
}
