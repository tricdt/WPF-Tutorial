using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CellStyleDemo.xaml
    /// </summary>
    public partial class CellStyleDemo : DemoControl
    {
        public CellStyleDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.treeGrid != null)
            {
                this.treeGrid.Dispose();
                this.treeGrid = null;
            }

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as EmployeeInfoViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }


            if (this.label1 != null)
                this.label1 = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            if (this.textBlock4 != null)
                this.textBlock4 = null;

            if (this.Resources["StyleConverter"] != null)
                (this.Resources["StyleConverter"] as CellStyleConverter).cellStyleDemo = null;

            this.Resources.Clear();

            base.Dispose(disposing);
        }
    }
}
