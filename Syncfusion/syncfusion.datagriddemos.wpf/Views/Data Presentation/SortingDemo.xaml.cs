using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for SortingDemo.xaml
    /// </summary>
    public partial class SortingDemo : DemoControl
    {
        public SortingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.dataGrid != null)
            {
                if (this.DataContext != null)
                    this.DataContext = null;

                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.CkbAllowSort != null)
                this.CkbAllowSort = null;

            if (this.CkbCustomerId != null)
                this.CkbCustomerId = null;

            if (this.CkbEnableTriStateSorting != null)
                this.CkbEnableTriStateSorting = null;

            if (this.CkbShowSortNumbers != null)
                this.CkbShowSortNumbers = null;

            if (this.CkbOrderId != null)
                this.CkbOrderId = null;

            if (this.CmbSortClickAction != null)
                this.CmbSortClickAction = null;

            if (this.textBlock != null)
                this.textBlock = null;

            base.Dispose(disposing);
        }
    }
}
