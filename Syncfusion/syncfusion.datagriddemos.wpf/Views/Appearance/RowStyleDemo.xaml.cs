using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for RowStyleDemo.xaml
    /// </summary>
    public partial class RowStyleDemo : DemoControl
    {
        public RowStyleDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.SfGrid != null)
            {
                //Release managed resources in EmployeeInfoViewModel.
                if (SfGrid.DataContext != null)
                {
                    var dataContext = SfGrid.DataContext as EmployeeInfoViewModel;
                    dataContext.Dispose();
                }
                this.SfGrid.Dispose();
                this.SfGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
