using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for TradingGridDemo.xaml
    /// </summary>
    public partial class TradingGridDemo : DemoControl
    {
        public TradingGridDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.datagrid != null)
            {
                this.datagrid.Dispose();
                this.datagrid = null;
            }

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.scrollModeTrigger != null)
                this.scrollModeTrigger = null;

            base.Dispose(disposing);
        }
    }
}
