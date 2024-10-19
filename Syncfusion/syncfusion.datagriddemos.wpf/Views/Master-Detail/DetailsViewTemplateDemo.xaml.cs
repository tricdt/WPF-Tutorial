using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DetailsViewTemplateDemo.xaml
    /// </summary>
    public partial class DetailsViewTemplateDemo : DemoControl
    {
        public DetailsViewTemplateDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        public DetailsViewTemplateDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
