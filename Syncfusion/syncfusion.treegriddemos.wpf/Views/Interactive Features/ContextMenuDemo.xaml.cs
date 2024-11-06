
using syncfusion.demoscommon.wpf;

namespace syncfusion.treegriddemos.wpf
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
            // Release all managed resources
            if (this.treeGrid != null)
            {
                this.treeGrid.Dispose();
                this.treeGrid = null;
            }

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as ContextMenuViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }

            base.Dispose(disposing);
        }
    }
}
