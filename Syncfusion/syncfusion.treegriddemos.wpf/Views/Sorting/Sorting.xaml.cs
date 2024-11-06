using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for Sorting.xaml
    /// </summary>
    public partial class SortingDemo : DemoControl
    {
        public SortingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            // Release all managed resources
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

            if (this.CkbAllowSort != null)
                this.CkbAllowSort = null;

            if (this.CkbEnableTriStateSorting != null)
                this.CkbEnableTriStateSorting = null;

            if (this.CkbShowSortNumbers != null)
                this.CkbShowSortNumbers = null;

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.CmbSortClickAction != null)
                this.CmbSortClickAction = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.CkbSalary != null)
                this.CkbSalary = null;

            if (this.CkbTitle != null)
                this.CkbTitle = null;

            base.Dispose(disposing);
        }
    }
}
