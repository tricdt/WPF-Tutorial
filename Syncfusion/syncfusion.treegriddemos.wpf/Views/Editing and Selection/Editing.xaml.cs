using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for EditingDemo.xaml
    /// </summary>
    public partial class EditingDemo : DemoControl
    {
        public EditingDemo(string themename) : base(themename)
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

            if (this.textBlock != null)
                this.textBlock = null;

            if (this.checkBox != null)
                this.checkBox = null;

            if (this.editTriggerComboBox != null)
                this.editTriggerComboBox = null;

            base.Dispose(disposing);
        }
    }
}
