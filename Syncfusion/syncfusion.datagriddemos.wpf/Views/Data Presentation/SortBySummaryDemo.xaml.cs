using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for SortBySummaryDemo.xaml
    /// </summary>
    public partial class SortBySummaryDemo : DemoControl
    {
        public SortBySummaryDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.DataContext != null)
                this.DataContext = null;

            //Release all managed resources
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }

            if (this.SumAggregate != null)
                this.SumAggregate = null;

            if (this.AvgAggregate != null)
                this.AvgAggregate = null;
            base.Dispose(disposing);
        }
    }
}
