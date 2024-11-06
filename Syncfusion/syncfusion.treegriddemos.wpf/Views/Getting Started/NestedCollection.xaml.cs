using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
{
    /// <summary>
    /// Interaction logic for NestedCollection.xaml
    /// </summary>
    public partial class NestedCollectionDemo : DemoControl
    {
        public NestedCollectionDemo(string themename) : base(themename)
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

            if (this.button1 != null)
                this.button1 = null;

            if (this.button2 != null)
                this.button2 = null;

            base.Dispose(disposing);
        }
    }
}
