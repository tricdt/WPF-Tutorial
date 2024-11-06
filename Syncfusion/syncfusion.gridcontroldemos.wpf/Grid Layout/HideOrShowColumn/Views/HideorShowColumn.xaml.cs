﻿
using syncfusion.demoscommon.wpf;
using System.Windows;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for HideorShowColumn.xaml
    /// </summary>
    public partial class HideorShowColumn : DemoControl
    {
        public HideorShowColumn()
        {
            InitializeComponent();
            InitializeGrid();
            initializeRowColumnCount();
            grid.Model.QueryCellInfo += new Syncfusion.Windows.Controls.Grid.GridQueryCellInfoEventHandler(Model_QueryCellInfo);
        }
        public HideorShowColumn(string themename) : base(themename)
        {
            InitializeComponent();
            InitializeGrid();
            initializeRowColumnCount();
            grid.Model.QueryCellInfo += new Syncfusion.Windows.Controls.Grid.GridQueryCellInfoEventHandler(Model_QueryCellInfo);
        }
        void Model_QueryCellInfo(object sender, Syncfusion.Windows.Controls.Grid.GridQueryCellInfoEventArgs e)
        {
            if (e.Cell.ColumnIndex == 0 && e.Cell.RowIndex == 0)
                e.Style.CellValue = "";
            else if (e.Cell.ColumnIndex == 0)
                e.Style.CellValue = e.Cell.RowIndex;
            else if (e.Cell.RowIndex == 0)
                e.Style.CellValue = e.Cell.ColumnIndex;
        }

        public void InitializeGrid()
        {
            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;

            for (int i = 1; i < 35; i++)
                for (int j = 1; j < 25; j++)
                    grid.Model[i, j].CellValue = "row" + i + "col" + j;

            ColHide.MaxValue = grid.Model.ColumnCount - 1;
            RowHide.MaxValue = grid.Model.RowCount - 1;
        }

        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            if (RowHide.IsEnabled == true)
            {
                var needsRefresh = false;
                int rowValue = (int)RowHide.Value;

                if (rowValue > 0 && rowValue < this.grid.Model.RowCount)
                {
                    this.grid.Model.RemoveRows(0, rowValue);
                    needsRefresh = true;
                }
                else
                {
                    MessageBox.Show("Enter the values properly");
                    RowHide.Focus();
                    needsRefresh = false;
                }

                int colValue = (int)ColHide.Value;

                if (colValue > 0 && colValue < this.grid.Model.ColumnCount)
                {
                    this.grid.Model.RemoveColumns(0, colValue);
                    needsRefresh = true;
                }
                else
                {
                    MessageBox.Show("Enter the values properly");
                    ColHide.Focus();
                    needsRefresh = false;
                }

                if (needsRefresh)
                {
                    grid.InvalidateCells();
                    RowHide.IsEnabled = ColHide.IsEnabled = false;
                    HideButton.Content = "Show Grid";
                    initializeRowColumnCount();
                }
            }
            else
            {
                InitializeGrid();
                initializeRowColumnCount();
                RowHide.IsEnabled = ColHide.IsEnabled = true;
                HideButton.Content = "Hide Rows/Columns";
                if (this.grid.CurrentCell != null)
                {
                    this.grid.CurrentCell.Deactivate();
                }
            }
        }

        private void initializeRowColumnCount()
        {
            RowCount.Text = Convert.ToString(grid.Model.RowCount);
            ColCount.Text = Convert.ToString(grid.Model.ColumnCount);
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
