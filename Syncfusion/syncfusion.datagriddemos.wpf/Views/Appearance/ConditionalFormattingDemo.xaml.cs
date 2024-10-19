using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ConditionalFormattingDemo.xaml
    /// </summary>
    public partial class ConditionalFormattingDemo : DemoControl
    {
        public ConditionalFormattingDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            //Release all managed resources
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            if (this.DataContext != null)
            {
                var dataContext = this.DataContext as SalesInfoViewModel;
                dataContext.Dispose();
                this.DataContext = null;
            }

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            if (this.textBlock4 != null)
                this.textBlock4 = null;

            if (this.textBlock5 != null)
                this.textBlock5 = null;

            if (this.textBlock6 != null)
                this.textBlock6 = null;

            if (this.Resources["tableSummaryStyleSelector"] != null)
                (this.Resources["tableSummaryStyleSelector"] as ConditionalFormattingStyleSelector).conditionalFormattingDemo = null;
            if (this.Resources["customstyle_QS2"] != null)
                (this.Resources["customstyle_QS2"] as StyleConverterforQS2).conditionalFormattingDemo = null;
            if (this.Resources["customstyle_QS3"] != null)
                (this.Resources["customstyle_QS3"] as StyleConverterforQS3).conditionalFormattingDemo = null;
            if (this.Resources["customstyle_QS4"] != null)
                (this.Resources["customstyle_QS4"] as StyleConverterforQS4).conditionalFormattingDemo = null;

            this.Resources.Clear();

            base.Dispose(disposing);
        }
    }
}
