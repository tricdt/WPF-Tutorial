using syncfusion.demoscommon.wpf;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for FormulaRangeSelection.xaml
    /// </summary>
    public partial class FormulaRangeSelection : DemoControl
    {
        FormulaSupportWithRangeSelectionsHelper helper;
        public FormulaRangeSelection()
        {
            InitializeComponent();
            InitializeGrid();
        }

        public FormulaRangeSelection(string themename) : base(themename)
        {
            InitializeComponent();
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;
            grid.Model.ColumnWidths[4] = 90;
            //put some random values in column 1 & 2
            Random r = new Random();
            for (int row = 1; row <= 40; ++row)
            {
                for (int col = 1; col <= 2; ++col)
                {
                    this.grid.Model[row, col].CellValue = r.Next(1000);
                    this.grid.Model[row, col].CellValueType = typeof(int);
                }
            }
            helper = new FormulaSupportWithRangeSelectionsHelper(grid);
        }

        protected void OnClosed(EventArgs e)
        {
            helper.Dispose();
            // base.OnClosed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.grid != null)
            {
                this.grid.Dispose();
                this.grid = null;
            }
            base.Dispose(disposing);
        }
    }
}
