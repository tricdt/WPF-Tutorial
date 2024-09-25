using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DataVirtualizationDemo.xaml
    /// </summary>
    public partial class DataVirtualizationDemo : DemoControl
    {
        public DataVirtualizationDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.sfDataGrid != null)
            {
                //Release managed resources in EmployeeInfoViewModel.
                if (this.sfDataGrid.DataContext != null)
                {
                    var dataContext = this.sfDataGrid.DataContext as EmployeeInfoViewModel;
                    dataContext.Dispose();
                }
                this.sfDataGrid.Dispose();
                this.sfDataGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
