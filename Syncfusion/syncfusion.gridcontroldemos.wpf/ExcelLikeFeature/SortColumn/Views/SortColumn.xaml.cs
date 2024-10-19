using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for SortColumn.xaml
    /// </summary>
    public partial class SortColumn : DemoControl
    {
        public SortColumn()
        {
            InitializeComponent();
            AssignRandomData(gridControl1);
            gridControl1.Model.TableStyle.HorizontalAlignment = HorizontalAlignment.Right;
            gridControl1.Model.ColumnWidths.DefaultLineSize = 60;
            gridControl1.Model.TableStyle.ReadOnly = true;
            new GridControlSortHelper(gridControl1);
        }
        public SortColumn(string themename) : base(themename)
        {
            InitializeComponent();
            AssignRandomData(gridControl1);
            gridControl1.Model.TableStyle.HorizontalAlignment = HorizontalAlignment.Right;
            gridControl1.Model.ColumnWidths.DefaultLineSize = 60;
            gridControl1.Model.TableStyle.ReadOnly = true;
            new GridControlSortHelper(gridControl1);
        }
        private void AssignRandomData(GridControl grid)
        {
            int rows = 35;
            int cols = 25;

            grid.Model.RowCount = rows;
            grid.Model.ColumnCount = cols;

            Random r = new Random(1231123);
            GridCellData cellData = grid.Model.Data;

            for (int i = 1; i < rows; ++i)
            {
                GridStyleInfo style = new GridStyleInfo();
                style.CellValue = i;
                cellData[i, 1] = style.Store;
                style = new GridStyleInfo();
                style.CellValue = i;
                cellData[i, 2] = style.Store;
            }

            for (int i = 1; i < rows; ++i)
            {
                for (int j = 3; j < cols; ++j)
                {
                    GridStyleInfo style = new GridStyleInfo();
                    style.CellValue = r.Next(1000);
                    cellData[i, j] = style.Store;
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
