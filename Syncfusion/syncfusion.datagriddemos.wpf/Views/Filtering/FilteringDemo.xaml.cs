using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for FilteringDemo.xaml
    /// </summary>
    public partial class FilteringDemo : DemoControl
    {
        public FilteringDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.sfGrid != null)
            {
                //Release managed resources in EmployeeInfoViewModel.
                if (this.sfGrid.DataContext != null)
                {
                    var dataContext = this.sfGrid.DataContext as EmployeeInfoViewModel;
                    dataContext.Dispose();
                }
                this.sfGrid.Dispose();
                this.sfGrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            if (this.FilterBox != null)
                this.FilterBox = null;

            if (this.columnCombo != null)
                this.columnCombo = null;

            if (this.stringCombo != null)
                this.stringCombo = null;

            if (this.numericCombo != null)
                this.numericCombo = null;
            base.Dispose(disposing);
        }
    }
}
