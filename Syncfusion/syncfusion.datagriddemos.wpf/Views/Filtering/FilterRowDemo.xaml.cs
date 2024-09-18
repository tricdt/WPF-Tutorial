using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for FilterRowDemo.xaml
    /// </summary>
    public partial class FilterRowDemo : DemoControl
    {
        public FilterRowDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.sfgrid != null)
            {
                this.sfgrid.Dispose();
                this.sfgrid = null;
            }

            base.Dispose(disposing);
        }
    }
}
