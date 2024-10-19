using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CustomFilterRowDemo.xaml
    /// </summary>
    public partial class CustomFilterRowDemo : DemoControl
    {
        public CustomFilterRowDemo(string themename) : base(themename)
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

            if (this.DataContext != null)
                this.DataContext = null;

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

            if (this.textBlock7 != null)
                this.textBlock7 = null;

            if (this.textBlock8 != null)
                this.textBlock8 = null;

            if (this.textBlock9 != null)
                this.textBlock9 = null;

            if (this.textBlock10 != null)
                this.textBlock10 = null;

            if (this.textBlock11 != null)
                this.textBlock11 = null;

            base.Dispose(disposing);
        }
    }
}
