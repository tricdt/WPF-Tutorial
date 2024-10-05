using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for PrintingDemo.xaml
    /// </summary>
    public partial class PrintingDemo : DemoControl
    {
        public PrintingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.syncgrid != null)
            {

                this.syncgrid.Dispose();
                this.syncgrid = null;
            }

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as OrderInfoViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }

            if (this.AllowFitCkb != null)
                this.AllowFitCkb = null;

            if (this.button != null)
                this.button = null;

            if (this.button1 != null)
                this.button1 = null;

            if (this.AllowRepeatHeaderCkb != null)
                this.AllowRepeatHeaderCkb = null;

            if (this.AllowPrintByDrawingCkb != null)
                this.AllowPrintByDrawingCkb = null;

            if (this.PrintStackedHeaderCkb != null)
                this.PrintStackedHeaderCkb = null;


            base.Dispose(disposing);
        }
    }
}
