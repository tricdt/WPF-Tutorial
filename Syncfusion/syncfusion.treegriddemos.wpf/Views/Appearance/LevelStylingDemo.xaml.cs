
using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for LevelStylingDemo.xaml
    /// </summary>
    public partial class LevelStylingDemo : DemoControl
    {
        public LevelStylingDemo()
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

            if (this.Resources["StyleConverter"] != null)
                (this.Resources["StyleConverter"] as CellStyleConverter).cellStyleDemo = null;

            this.Resources.Clear();

            base.Dispose(disposing);
        }
    }
}
