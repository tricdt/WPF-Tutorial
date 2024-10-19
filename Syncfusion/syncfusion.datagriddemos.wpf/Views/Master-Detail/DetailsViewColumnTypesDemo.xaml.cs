using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DetailsViewColumnTypesDemo.xaml
    /// </summary>
    public partial class DetailsViewColumnTypesDemo : DemoControl
    {
        public DetailsViewColumnTypesDemo(string themename) : base(themename)
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

            base.Dispose(disposing);
        }
    }
}
