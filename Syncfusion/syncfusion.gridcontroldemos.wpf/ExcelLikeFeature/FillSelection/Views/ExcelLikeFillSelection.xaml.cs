using syncfusion.demoscommon.wpf;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelLikeFillSelection.xaml
    /// </summary>
    public partial class ExcelLikeFillSelection : DemoControl
    {
        public ExcelLikeFillSelection()
        {
            InitializeComponent();
            InitializeGridControl();
        }
        public ExcelLikeFillSelection(string themename) : base(themename)
        {
            InitializeComponent();
            InitializeGridControl();
        }

        private void InitializeGridControl()
        {
            gridControl1.Model.ColumnCount = 30;
            gridControl1.Model.RowCount = 40;
            gridControl1.Model.RowHeights.DefaultLineSize = 32;
            this.gridControl1.Model.ColumnWidths[0] = 40;
            gridControl1.Model.Options.ExcelLikeSelectionFrame = true;
            gridControl1.Model.Options.ExcelLikeSelection = true;
            GridFillSeriesMouseController controller = new GridFillSeriesMouseController(this.gridControl1);
            this.gridControl1.MouseControllerDispatcher.Add(controller);
        }
        protected override void Dispose(bool disposing)
        {
            if (this.gridControl1 != null)
            {
                this.gridControl1.Dispose();
                this.gridControl1 = null;
            }
            base.Dispose(disposing);
        }
    }
}
