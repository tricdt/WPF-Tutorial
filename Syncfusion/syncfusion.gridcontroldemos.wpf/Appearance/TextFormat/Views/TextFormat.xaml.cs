﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for TextFormat.xaml
    /// </summary>
    public partial class TextFormat : DemoControl
    {
        public TextFormat()
        {
            InitializeComponent();
            InitializeGridControl();
        }

        public TextFormat(string themename) : base(themename)
        {
            InitializeComponent();
            InitializeGridControl();
        }

        private void InitializeGridControl()
        {
            grid.Model.TableStyle.CellType = "Static";
            grid.Model.RowCount = 30;
            grid.Model.ColumnCount = 25;
            int rowIndex = 1;
            int colIndex = 2;
            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(rowIndex, colIndex, rowIndex, colIndex + 1));
            grid.Model[rowIndex, colIndex].CellValue = "Text Formats";
            grid.Model[rowIndex, colIndex].Background = Brushes.DarkBlue;
            grid.Model[rowIndex, colIndex].Foreground = Brushes.White;
            grid.Model[rowIndex, colIndex].Font.FontSize = 15;
            grid.Model[rowIndex, colIndex].HorizontalAlignment = HorizontalAlignment.Center;
            rowIndex = rowIndex + 3;
            colIndex++;

            //NumberFormat
            string[] NumberFormat = new string[]
            {

                "0.00",
                "C",
                "0.00;(0.00)",
                "###0.##%",
                "#0.#E+00",
                "10:##,##0.#"
            };

            GridModel model = this.grid.Model;
            foreach (string format in NumberFormat)
            {
                model[rowIndex - 1, colIndex].Text = format;
                model[rowIndex - 1, colIndex].Background = Brushes.Green;
                model[rowIndex - 1, colIndex].Foreground = Brushes.White;

                model[rowIndex - 1, colIndex - 1].Text = "Format";
                model[rowIndex - 1, colIndex - 1].Background = Brushes.Green;
                model[rowIndex - 1, colIndex - 1].Foreground = Brushes.White;

                model[rowIndex, colIndex - 1].Text = "Example";
                model[rowIndex, colIndex - 1].Background = Brushes.Green;
                model[rowIndex, colIndex - 1].Foreground = Brushes.White;
                ///Value
                model[rowIndex, colIndex].Format = format;
                model[rowIndex, colIndex].CellValue = Math.PI;
                model[rowIndex, colIndex].CellValueType = typeof(double);
                rowIndex += 3;
            }

            rowIndex = 1;
            colIndex = 5;
            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(rowIndex, colIndex, rowIndex, colIndex + 1));
            model[rowIndex, colIndex].Text = "DateTime Formats";
            grid.Model[rowIndex, colIndex].Background = Brushes.DarkBlue;
            grid.Model[rowIndex, colIndex].Foreground = Brushes.White;
            grid.Model[rowIndex, colIndex].Font.FontSize = 15;
            model[rowIndex, colIndex].HorizontalAlignment = HorizontalAlignment.Center;
            rowIndex = rowIndex + 3;
            colIndex++;

            //DateTimeFormat
            string[] DateTimeFormat = new string[]
            {
                "d",
                "D",
                "f",
                "dddd, dd MMMM yyyy",
                "t",
                "s"
            };
            foreach (string format in DateTimeFormat)
            {
                model[rowIndex - 1, colIndex].Text = format;
                model[rowIndex - 1, colIndex].Background = Brushes.Green;
                model[rowIndex - 1, colIndex].Foreground = Brushes.White;

                model[rowIndex - 1, colIndex - 1].Text = "Format";
                model[rowIndex - 1, colIndex - 1].Background = Brushes.Green;
                model[rowIndex - 1, colIndex - 1].Foreground = Brushes.White;

                model[rowIndex, colIndex - 1].Text = "Example";
                model[rowIndex, colIndex - 1].Background = Brushes.Green;
                model[rowIndex, colIndex - 1].Foreground = Brushes.White;
                ///Value
                model[rowIndex, colIndex].Format = format;
                model[rowIndex, colIndex].CellValue = DateTime.Now;
                model[rowIndex, colIndex].CellValueType = typeof(DateTime);
                rowIndex += 3;
            }

            rowIndex -= 1;
            colIndex = 2;
            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(rowIndex, colIndex, rowIndex, colIndex + 4));
            model[rowIndex, colIndex].Text = "Format using the FormatProvider property";
            grid.Model[rowIndex, colIndex].Background = Brushes.DarkBlue;
            grid.Model[rowIndex, colIndex].Foreground = Brushes.White;
            grid.Model[rowIndex, colIndex].Font.FontSize = 15;
            model[rowIndex, colIndex].HorizontalAlignment = HorizontalAlignment.Center;
            rowIndex += 3;
            colIndex += 1;
            string[] CustomFormat = new string[]
                {
                    "{0:ES}",
                    "{0:US}"
                };
            foreach (string format in CustomFormat)
            {
                model[rowIndex - 1, colIndex].Text = format;
                model[rowIndex - 1, colIndex].Background = Brushes.Green;
                model[rowIndex - 1, colIndex].Foreground = Brushes.White;

                model[rowIndex - 1, colIndex - 1].Text = "Format";
                model[rowIndex - 1, colIndex - 1].Background = Brushes.Green;
                model[rowIndex - 1, colIndex - 1].Foreground = Brushes.White;

                model[rowIndex, colIndex - 1].Text = "Example";
                model[rowIndex, colIndex - 1].Background = Brushes.Green;
                model[rowIndex, colIndex - 1].Foreground = Brushes.White;
                ///Value
                model[rowIndex, colIndex].Format = format;
                model[rowIndex, colIndex].CellValue = Math.PI;
                model[rowIndex, colIndex].FormatProvider = new CustomNumberFormat();
                model[rowIndex, colIndex].CellValueType = typeof(double);
                rowIndex += 3;
            }

            rowIndex -= 6;
            colIndex = 6;
            string[] AccountFormat = new string[]
                {
                    "{0:H}",
                    "{0:I}"
                };
            foreach (string format in AccountFormat)
            {
                model[rowIndex - 1, colIndex].Text = format;
                model[rowIndex - 1, colIndex].Background = Brushes.Green;
                model[rowIndex - 1, colIndex].Foreground = Brushes.White;

                model[rowIndex - 1, colIndex - 1].Text = "Format";
                model[rowIndex - 1, colIndex - 1].Background = Brushes.Green;
                model[rowIndex - 1, colIndex - 1].Foreground = Brushes.White;

                model[rowIndex, colIndex - 1].Text = "Example";
                model[rowIndex, colIndex - 1].Background = Brushes.Green;
                model[rowIndex, colIndex - 1].Foreground = Brushes.White;
                ///Value
                model[rowIndex, colIndex].Format = format;
                model[rowIndex, colIndex].CellValue = 104254567890;
                model[rowIndex, colIndex].FormatProvider = new AccountNumberFormat();
                model[rowIndex, colIndex].CellValueType = typeof(Int64);
                rowIndex += 3;
            }

            grid.Model.ColumnWidths[3] = 140d;
            grid.Model.ColumnWidths[6] = 250;
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
