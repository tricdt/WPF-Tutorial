
namespace syncfusion.gridcontroldemos.wpf
{
    using syncfusion.demoscommon.wpf;
    using Syncfusion.Windows.Controls.Grid;

    /// <summary>
    /// Interaction logic for DragandDrop.xaml
    /// </summary>
    public partial class DragandDrop : DemoControl
    {
        public DragandDrop()
        {
            InitializeComponent();
            this.InitGrid();
            this.grid.QueryAllowDragColumn += grid_QueryAllowDragColumn;
        }
        public DragandDrop(string themename) : base(themename)
        {
            InitializeComponent();
            this.InitGrid();
            this.grid.QueryAllowDragColumn += grid_QueryAllowDragColumn;
        }
        void grid_QueryAllowDragColumn(object sender, GridQueryDragColumnHeaderEventArgs e)
        {
            //To disable dragging of First column
            if (e.Column == 0 && e.Reason == GridQueryDragColumnHeaderReason.HitTest)
                e.AllowDrag = false;

            //To disable dropping in First column
            if (e.InsertBeforeColumn == 0 &&
                (e.Reason == GridQueryDragColumnHeaderReason.MouseUp || e.Reason == GridQueryDragColumnHeaderReason.MouseMove))
                e.AllowDrag = false;
        }

        private void InitGrid()
        {
            this.grid.AllowDragColumns = true;
            this.grid.Model.RowCount = 35;
            this.grid.Model.ColumnCount = 25;

            for (int i = 1; i < 35; i++)
            {
                for (int j = 1; j < 25; j++)
                {
                    this.grid.Model[i, j].CellValue = string.Format("Row{0} Col{1}", i, j);
                }
            }
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
