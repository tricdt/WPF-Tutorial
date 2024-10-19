using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for SummariesDemo.xaml
    /// </summary>
    public partial class SummariesDemo : DemoControl
    {
        public SummariesDemo()
        {
            InitializeComponent();
        }
        public SummariesDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.DataContext != null)
                this.DataContext = null;

            //Release all managed resources
            if (this.datagrid != null)
            {
                this.datagrid.Dispose();
                this.datagrid = null;
            }

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.comboBox != null)
                this.comboBox = null;

            base.Dispose(disposing);
        }
    }
}