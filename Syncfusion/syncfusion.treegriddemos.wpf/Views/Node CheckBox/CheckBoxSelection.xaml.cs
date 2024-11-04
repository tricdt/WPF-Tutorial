using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CheckBoxSelection.xaml
    /// </summary>
    public partial class CheckBoxSelection : DemoControl
    {
        public CheckBoxSelection(string themename) : base(themename)
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

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.cmbSelectionMode != null)
                this.cmbSelectionMode = null;

            base.Dispose(disposing);
        }
    }
}
