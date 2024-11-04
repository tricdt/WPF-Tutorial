using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ConditionalCheckBox.xaml
    /// </summary>
    public partial class ConditionalCheckBox : DemoControl
    {
        public ConditionalCheckBox()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

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

            if (this.CkbRecurisveCheck != null)
                this.CkbRecurisveCheck = null;

            if (this.checkBox1 != null)
                this.checkBox1 = null;

            base.Dispose(disposing);
        }
    }
}
