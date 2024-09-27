using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for UnBoundRowDemo.xaml
    /// </summary>
    public partial class UnBoundRowDemo : DemoControl
    {
        public UnBoundRowDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();
            //Release all managed resources
            if (this.sfDataGrid != null)
            {
                this.sfDataGrid.Dispose();
                this.sfDataGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
