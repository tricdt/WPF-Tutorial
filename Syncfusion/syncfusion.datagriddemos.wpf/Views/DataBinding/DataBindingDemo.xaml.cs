using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for DataBindingDemo.xaml
    /// </summary>
    public partial class DataBindingDemo : DemoControl
    {
        public DataBindingDemo()
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            if (this.textBlock != null)
                textBlock = null;

            if (this.comboBinding != null)
                this.comboBinding = null;

            if (this.dataGridArea != null)
                this.dataGridArea = null;

            base.Dispose(disposing);
        }
    }
}
