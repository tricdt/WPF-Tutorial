using syncfusion.demoscommon.wpf;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for SelectionMarker.xaml
    /// </summary>
    public partial class SelectionMarker : DemoControl
    {
        public SelectionMarker()
        {
            InitializeComponent();
            sampleGrid1.Model.RowCount = 30;
            sampleGrid1.Model.ColumnCount = 25;
            sampleGrid1.Model.ColumnWidths.SetHidden(5, 10, true);
            sampleGrid1.Model.Options.HiddenBorderBrush = Brushes.Black;
            sampleGrid1.Model.Options.HiddenBorderThickness = 2d;
            sampleGrid1.Model.Options.AllowExcelLikeResizing = true;
        }
        public SelectionMarker(string themename) : base(themename)
        {
            InitializeComponent();
            sampleGrid1.Model.RowCount = 30;
            sampleGrid1.Model.ColumnCount = 25;
            sampleGrid1.Model.ColumnWidths.SetHidden(5, 10, true);
            sampleGrid1.Model.Options.HiddenBorderBrush = Brushes.Black;
            sampleGrid1.Model.Options.HiddenBorderThickness = 2d;
            sampleGrid1.Model.Options.AllowExcelLikeResizing = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.sampleGrid1 != null)
            {
                this.sampleGrid1.Dispose();
                this.sampleGrid1 = null;
            }
            base.Dispose(disposing);
        }
    }
}
