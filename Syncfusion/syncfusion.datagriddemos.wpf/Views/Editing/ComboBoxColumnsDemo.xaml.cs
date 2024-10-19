using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ComboBoxColumnsDemo.xaml
    /// </summary>
    public partial class ComboBoxColumnsDemo : DemoControl
    {
        public ComboBoxColumnsDemo(string themename) : base(themename)
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

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            base.Dispose(disposing);
        }
    }
}
