using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ResizeToFit.xaml
    /// </summary>
    public partial class ResizeToFit : DemoControl
    {
        public ResizeToFit()
        {
            InitializeComponent();
            this.InitGrid();
        }
        public ResizeToFit(string themename) : base(themename)
        {
            InitializeComponent();
            this.InitGrid();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.grid.Model.ResizeColumnsToFit(GridRangeInfo.Table(), GridResizeToFitOptions.None);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.grid.Model.ResizeRowsToFit(GridRangeInfo.Table(), GridResizeToFitOptions.None);
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