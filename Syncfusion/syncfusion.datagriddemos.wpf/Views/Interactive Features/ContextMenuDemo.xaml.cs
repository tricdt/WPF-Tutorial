using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ContextMenuDemo.xaml
    /// </summary>
    public partial class ContextMenuDemo : DemoControl
    {
        public ContextMenuDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        public ContextMenuDemo()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.datagrid != null)
            {
                this.datagrid.Dispose();
                this.datagrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            base.Dispose(disposing);
        }
    }
}
