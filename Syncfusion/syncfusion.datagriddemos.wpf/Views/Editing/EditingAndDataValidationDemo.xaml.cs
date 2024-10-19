using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for EditingAndDataValidationDemo.xaml
    /// </summary>
    public partial class EditingAndDataValidationDemo : DemoControl
    {
        public EditingAndDataValidationDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.sfdatagrid != null)
            {
                this.sfdatagrid.Dispose();
                this.sfdatagrid = null;
            }

            base.Dispose(disposing);
        }
    }
}
