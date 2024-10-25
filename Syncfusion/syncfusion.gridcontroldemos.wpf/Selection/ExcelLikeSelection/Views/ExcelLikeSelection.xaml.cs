using syncfusion.demoscommon.wpf;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelLikeSelection.xaml
    /// </summary>
    public partial class ExcelLikeSelection : DemoControl
    {
        public ExcelLikeSelection()
        {
            InitializeComponent();
            GridSettings();
        }
        public ExcelLikeSelection(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }
        void GridSettings()
        {
            gridControl1.Model.ColumnCount = 25;
            gridControl1.Model.RowCount = 100;
            this.gridControl1.Model.Options.ExcelLikeCurrentCell = true;
            this.gridControl1.Model.Options.ExcelLikeSelectionFrame = true;
            GridExcelMarkerMouseController controller = new GridExcelMarkerMouseController(this.gridControl1);
            this.gridControl1.MouseControllerDispatcher.Add(controller);

            Random r = new Random();
            for (int row = 1; row < 400; row++)
            {
                for (int col = 1; col < 400; col++)
                {
                    if (r.Next(100) > 60)
                        gridControl1.Model[row, col].Text = r.Next(5000, 10000).ToString();
                }
            }
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
