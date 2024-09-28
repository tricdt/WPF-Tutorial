using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DataValidationDemo.xaml
    /// </summary>
    public partial class DataValidationDemo : DemoControl
    {
        public DataValidationDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.grid != null)
            {
                this.grid.Dispose();
                this.grid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            if (this.textBlock4 != null)
                this.textBlock4 = null;

            if (this.textBlock5 != null)
                this.textBlock5 = null;

            if (this.textBlock6 != null)
                this.textBlock6 = null;

            if (this.textBlock6 != null)
                this.textBlock6 = null;

            if (this.ValidationCombo != null)
                this.ValidationCombo = null;

            base.Dispose(disposing);
        }
    }
}
