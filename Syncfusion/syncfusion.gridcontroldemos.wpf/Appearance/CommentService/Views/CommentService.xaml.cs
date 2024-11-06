using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using System.Data;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for CommentService.xaml
    /// </summary>
    public partial class CommentService : DemoControl
    {
        Random random;
        DataTable dataTable;
        public CommentService()
        {
            InitializeComponent();
            Width = 1000;
            Height = 600;

            this.LoadData();
            gridControl1.Model.RowCount = dataTable.Rows.Count;
            gridControl1.Model.ColumnCount = dataTable.Columns.Count;
            gridControl1.Model.RowHeights.DefaultLineSize = 30;
            gridControl1.Model.ColumnWidths.DefaultLineSize = 115;
            gridControl1.Model.ColumnWidths[0] = 115;
            gridControl1.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            gridControl1.QueryCellInfo += new GridQueryCellInfoEventHandler(GridControl1_QueryCellInfo);
        }

        public CommentService(string themename) : base(themename)
        {
            InitializeComponent();
            Width = 1000;
            Height = 600;

            this.LoadData();
            gridControl1.Model.RowCount = dataTable.Rows.Count;
            gridControl1.Model.ColumnCount = dataTable.Columns.Count;
            gridControl1.Model.RowHeights.DefaultLineSize = 30;
            gridControl1.Model.ColumnWidths.DefaultLineSize = 115;
            gridControl1.Model.ColumnWidths[0] = 115;
            gridControl1.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            gridControl1.QueryCellInfo += new GridQueryCellInfoEventHandler(GridControl1_QueryCellInfo);
        }

        private void GridControl1_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            GridCommentStyleInfo styleinfo = new GridCommentStyleInfo();

            if (e.Style.ColumnIndex == 0 || e.Style.RowIndex == 0)
            {
                e.Style.CellType = "DataBoundTemplate";
                e.Style.CellItemTemplateKey = "TextBlocktemplate";
            }

            if (e.Style.RowIndex == 0)
                e.Style.CellValue = dataTable.Columns[e.Style.ColumnIndex];
            else
            {
                e.Style.CellValue = dataTable.Rows[e.Style.RowIndex][e.Style.ColumnIndex];

                if ((e.Style.ColumnIndex == 2 || e.Style.ColumnIndex == 4 || e.Style.ColumnIndex == 6) && e.Style.RowIndex > 0)
                {
                    string comment = " " + dataTable.Rows[e.Style.RowIndex][0].ToString() + ": \n Population rate in " + dataTable.Columns[e.Style.ColumnIndex] + " is " + e.Style.CellValue.ToString();
                    if (e.Style.ColumnIndex == 2)
                    {
                        e.Style.Background = Brushes.WhiteSmoke;
                        styleinfo.TopLeftCommentBrush = Brushes.Black;
                        styleinfo.BottomRightCommentBrush = Brushes.Green;
                        styleinfo.TopLeftComment = styleinfo.BottomRightComment = comment;
                        e.Style.GridCommentStyleInfo = styleinfo;
                    }
                    else if (e.Style.ColumnIndex == 6)
                    {
                        styleinfo.BottomLeftCommentBrush = Brushes.BlueViolet;
                        styleinfo.TopRightCommentBrush = Brushes.DarkMagenta;
                        styleinfo.BottomLeftComment = styleinfo.TopRightComment = comment;
                        e.Style.GridCommentStyleInfo = styleinfo;
                    }
                    else
                        e.Style.Comment = comment;
                }
            }
        }

        private void LoadData()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Country"));
            dataTable.Columns.Add(new DataColumn("2005"));
            dataTable.Columns.Add(new DataColumn("2006"));
            dataTable.Columns.Add(new DataColumn("2007"));
            dataTable.Columns.Add(new DataColumn("2008"));
            dataTable.Columns.Add(new DataColumn("2009"));
            dataTable.Columns.Add(new DataColumn("2010"));
            dataTable.Columns.Add(new DataColumn("2011"));
            random = new Random();

            var row = dataTable.NewRow();
            row["Country"] = "USA";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "India";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "China";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Japan";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Russia";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Canada";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Germany";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Iran";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Thailand";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Bangladesh";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Nigeria";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Mexico";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Egypt";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "France";
            LoadCellValues(row);
            dataTable.Rows.Add(row);
        }

        public void LoadCellValues(DataRow dataRow)
        {
            for (int row = 1; row <= 7; row++)
            {
                dataRow[row] = ((double)random.Next(2, 18)).ToString() + "%";
            }
        }

        protected override void Dispose(bool disposing)
        {
            gridControl1.QueryCellInfo -= new GridQueryCellInfoEventHandler(GridControl1_QueryCellInfo);
            if (this.gridControl1 != null)
            {
                this.gridControl1.Dispose();
                this.gridControl1 = null;
            }
            base.Dispose(disposing);
        }
    }
}
