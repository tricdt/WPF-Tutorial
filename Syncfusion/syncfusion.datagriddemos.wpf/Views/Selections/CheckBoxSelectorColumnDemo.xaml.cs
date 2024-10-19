using syncfusion.demoscommon.wpf;
namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CheckBoxSelectorColumnDemo.xaml
    /// </summary>
    public partial class CheckBoxSelectorColumnDemo : DemoControl
    {
        public CheckBoxSelectorColumnDemo(string themename) : base(themename)
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

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.cmbSelectionMode != null)
                this.cmbSelectionMode = null;

            base.Dispose(disposing);
        }
    }
}
