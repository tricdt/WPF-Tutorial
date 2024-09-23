using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DetailsViewDataTableBindingDemo.xaml
    /// </summary>
    public partial class DetailsViewDataTableBindingDemo : DemoControl
    {
        public DetailsViewDataTableBindingDemo(string themename) : base(themename)
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
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
