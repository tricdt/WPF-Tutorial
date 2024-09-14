using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ComplexPropertyBindingDemo.xaml
    /// </summary>
    public partial class ComplexPropertyBindingDemo : DemoControl
    {
        public ComplexPropertyBindingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.DataContext != null)
                this.DataContext = null;

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
