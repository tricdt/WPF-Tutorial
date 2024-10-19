using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for SearchPanelDemo.xaml
    /// </summary>
    public partial class SearchPanelDemo : DemoControl
    {
        public SearchPanelDemo()
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

            if (this.searchControl != null)
                this.searchControl = null;


            base.Dispose(disposing);
        }
    }
}
