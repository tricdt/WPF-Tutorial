using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DataPagingDemo.xaml
    /// </summary>
    public partial class DataPagingDemo : DemoControl
    {
        public DataPagingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.sfDataPager != null)
            {
                this.sfDataPager.Dispose();
                this.sfDataPager = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.OrientationComboBox != null)
                this.OrientationComboBox = null;


            base.Dispose(disposing);
        }
    }
}
