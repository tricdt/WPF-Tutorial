using syncfusion.demoscommon.wpf;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for Printing.xaml
    /// </summary>
    public partial class Printing : DemoControl
    {
        public Printing()
        {
            InitializeComponent();
            this.gc.Model.RowCount = 30;
            this.gc.Model.ColumnCount = 20;
            this.gc.Model.QueryCellInfo += (s, e) =>
            {
                if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 0)
                    e.Style.CellValue = string.Format("R{0}C{1}", e.Cell.RowIndex, e.Cell.ColumnIndex);
            };
            //this.gc.Model.ColumnWidths[0] = 80;
            this.gc.ColumnWidths[0] = 30d;
        }

        public Printing(string themename) : base(themename)
        {
            InitializeComponent();
            this.gc.Model.RowCount = 30;
            this.gc.Model.ColumnCount = 20;
            this.gc.Model.QueryCellInfo += (s, e) =>
            {
                if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 0)
                    e.Style.CellValue = string.Format("R{0}C{1}", e.Cell.RowIndex, e.Cell.ColumnIndex);
            };
            //this.gc.Model.ColumnWidths[0] = 80;
            this.gc.ColumnWidths[0] = 30d;

        }

        protected override void Dispose(bool disposing)
        {
            if (this.gc != null)
            {
                this.gc.Dispose();
                this.gc = null;
            }
            base.Dispose(disposing);
        }
    }
}
