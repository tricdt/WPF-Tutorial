using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for AddNewRowDemo.xaml
    /// </summary>
    public partial class AddNewRowDemo : DemoControl
    {
        public AddNewRowDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();
            //Release all managed resources
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.AddNewPositionCombo != null)
                this.AddNewPositionCombo = null;

            base.Dispose(disposing);
        }
    }
}
