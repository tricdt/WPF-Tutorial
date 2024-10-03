using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CutCopyPasteDemo.xaml
    /// </summary>
    public partial class CutCopyPasteDemo : DemoControl
    {
        public CutCopyPasteDemo(string themename) : base(themename)
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

            if (this.CopyOptionComboBox != null)
            {
                this.CopyOptionComboBox.Dispose();
                this.CopyOptionComboBox = null;
            }

            if (this.PasteOptionComboBox != null)
            {
                this.PasteOptionComboBox.Dispose();
                this.PasteOptionComboBox = null;
            }

            if (this.textBlock1 != null)
                this.textBlock1 = null;

            if (this.SelectionUnit != null)
                this.SelectionUnit = null;

            if (this.textBlock2 != null)
                this.textBlock2 = null;

            if (this.cmbSelectionMode != null)
                this.cmbSelectionMode = null;

            if (this.textBlock3 != null)
                this.textBlock3 = null;

            if (this.textBlock4 != null)
                this.textBlock4 = null;

            if (this.textBlock5 != null)
                this.textBlock5 = null;

            if (this.Clipboardcontent != null)
                this.Clipboardcontent = null;

            base.Dispose(disposing);
        }
    }
}
