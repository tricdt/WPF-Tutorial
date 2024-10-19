using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for FilterStatusBarDemo.xaml
    /// </summary>
    public partial class FilterStatusBarDemo : DemoControl
    {
        public FilterStatusBarDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        public FilterStatusBarDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.sfgrid != null)
            {
                this.sfgrid.Dispose();
                this.sfgrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.FilterStatusBar != null)
                this.FilterStatusBar = null;

            base.Dispose(disposing);
        }
    }
}
