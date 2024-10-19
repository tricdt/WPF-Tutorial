using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for GettingStarted.xaml
    /// </summary>
    public partial class GettingStarted : DemoControl
    {
        public GettingStarted(string themename) : base(themename)
        {
            InitializeComponent();

        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            if (this.DataContext != null)
                this.DataContext = null;

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
