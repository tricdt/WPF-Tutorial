using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for GroupingDemo.xaml
    /// </summary>
    public partial class GroupingDemo : DemoControl
    {
        public GroupingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            if (this.DataContext != null)
                this.DataContext = null;

            //Release all managed resources
            if (this.sfGrid != null)
            {
                this.sfGrid.Dispose();
                this.sfGrid = null;
            }

            if (this.AFC_ChkBox != null)
                this.AFC_ChkBox = null;

            if (this.Name_ChkBx != null)
                this.Name_ChkBx = null;

            base.Dispose(disposing);
        }
    }
}
