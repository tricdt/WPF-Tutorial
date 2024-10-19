using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for AutoCellMergeDemo.xaml
    /// </summary>
    public partial class AutoCellMergeDemo : DemoControl
    {
        public AutoCellMergeDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();
            //Release all managed resources
            if (this.sfgrid != null)
            {
                //Release managed resources in EmployeeInfoViewModel.
                if (this.sfgrid.DataContext != null)
                {
                    var dataContext = this.sfgrid.DataContext as EmployeeInfoViewModel;
                    dataContext.Dispose();
                }
                this.sfgrid.Dispose();
                this.sfgrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
