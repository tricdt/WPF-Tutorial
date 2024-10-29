﻿
using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for GridSerialization.xaml
    /// </summary>
    public partial class GridSerialization : DemoControl
    {
        public GridSerialization()
        {
            InitializeComponent();
            GridSettings();
        }
        public GridSerialization(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }
        void GridSettings()
        {

            Random r = new Random();
            grid.Model.RowCount = 50;
            grid.Model.ColumnCount = 17;
            grid.Model.RowHeights[1] = 50;
            grid.Model.ColumnWidths[2] = 100;
            GridStyleInfo ci = new GridStyleInfo();
            for (int row = 1; row < 50; row++)
            {
                for (int col = 1; col < 17; col++)
                {
                    if (r.Next(1, 4) == 2)
                    {
                        grid.Model[row, col].CellValue = r.Next(10, 100);
                    }
                    else if (r.Next(1, 4) == 3)
                    {
                        grid.Model[row, col].CellValue = "Text" + r.Next(10, 100).ToString();
                    }
                    else
                    {
                        grid.Model[row, col].CellValue = (r.Next(1000, 10000) * .01);
                    }

                    if (r.Next(10, 14) == 12)
                    {
                        grid.Model[row, col].Font.FontStyle = FontStyles.Italic;
                        grid.Model[row, col].Font.FontWeight = FontWeights.Bold;
                        grid.Model[row, col].Font.FontSize = 13;
                    }

                    if (r.Next(10, 14) == 11)
                    {
                        grid.Model[row, col].Borders.Bottom = new Pen(new SolidColorBrush(Colors.Gray), 3);
                        grid.Model[row, col].Borders.Top = new Pen(new SolidColorBrush(Colors.LightGray), 3);
                        grid.Model[row, col].Borders.Right = new Pen(new SolidColorBrush(Colors.DarkGray), 3);
                        grid.Model[row, col].Borders.Left = new Pen(new SolidColorBrush(Colors.Yellow), 3);
                    }

                    if (r.Next(10, 14) == 13)
                    {
                        grid.Model[row, col].Background = new SolidColorBrush(Colors.Orange);
                        grid.Model[row, col].Foreground = new SolidColorBrush(Colors.Red);
                    }

                    if (r.Next(10, 14) == 13)
                    {
                        grid.Model[row, col].HorizontalAlignment = HorizontalAlignment.Right;
                    }
                }
            }
            grid.Model.ColumnWidths[3] = 150;
            this.grid.Model.Serialize("testfile.xml");
            this.grid.QueryCellInfo += grid_QueryCellInfo;
        }
        void grid_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Style.RowIndex == 0 && e.Style.ColumnIndex == 0)
            {
                return;
            }
            else if (e.Style.RowIndex == 0)
            {
                e.Style.CellValue = GridRangeInfo.GetAlphaLabel(e.Cell.ColumnIndex);
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
                e.Style.VerticalAlignment = VerticalAlignment.Center;
            }
            else if (e.Style.ColumnIndex == 0)
            {
                e.Style.CellValue = e.Style.RowIndex;
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
                e.Style.VerticalAlignment = VerticalAlignment.Center;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.grid.Model.Serialize("testfile.xml");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.grid.Model.RowCount = 20;
            this.grid.Model.ColumnCount = 8;

            for (int i = 1; i < 20; i++)
            {
                for (int j = 1; j < 80; j++)
                {
                    var style = new GridStyleInfo();
                    style.Background = new SolidColorBrush(Colors.Yellow);
                    style.Foreground = new SolidColorBrush(Colors.Green);
                    style.CellValue = String.Format("Row {0} Col {1}", i, j);
                    this.grid.Model[i, j] = style;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.grid.CurrentCell.Deactivate();
            this.grid.Model.Deserialize("testfile.xml");
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
