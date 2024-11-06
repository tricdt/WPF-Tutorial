﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.ComponentModel;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for IntegerEditCell.xaml
    /// </summary>
    public partial class IntegerEditCell : DemoControl
    {
        GridStyleInfo g = new GridStyleInfo();
        public IntegerEditCell()
        {
            InitializeComponent();
            GridSettings();
        }

        public IntegerEditCell(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            grid.Model.RowCount = 50;
            grid.Model.ColumnCount = 25;

            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(1, 1, 3, 6));
            grid.Model[1, 1].CellValue = "Integer Edit";
            grid.Model[1, 1].Foreground = Brushes.Black;
            grid.Model[1, 1].Background = Brushes.LightBlue;
            grid.Model[1, 1].Font.FontSize = 18;
            grid.Model[1, 1].HorizontalAlignment = HorizontalAlignment.Center;
            grid.Model[1, 1].VerticalAlignment = VerticalAlignment.Center;
            grid.Model[1, 1].Font.FontWeight = FontWeights.Bold;

            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(4, 1, 5, 6));
            var cell = this.grid.Model[4, 1];
            cell.CellValue = "This sample shows how integer edit cells behave.We can set number group separator for effectively displaying the numbers in the needed format";
            cell.HorizontalAlignment = HorizontalAlignment.Center;
            cell.Font.FontWeight = FontWeights.Bold;

            grid.Model[12, 2].CellType = "IntegerEdit";
            grid.Model[12, 2].IsEditable = true;
            grid.Model[12, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = "," };
            int[] sizes = { 2, 3, 4 };
            grid.Model[12, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[12, 2].CellValue = 1;

            g = grid.Model[6, 2];

            grid.Model[8, 2].CellType = "IntegerEdit";
            grid.Model[8, 2].IsEditable = true;
            grid.Model[8, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = "," };
            grid.Model[8, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[8, 2].CellValue = 222222;

            grid.Model[10, 2].CellType = "IntegerEdit";
            grid.Model[10, 2].IsEditable = true;
            grid.Model[10, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = "," };
            grid.Model[10, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[10, 2].CellValue = 1000;

            grid.Model[6, 2].CellType = "IntegerEdit";
            grid.Model[6, 2].IsEditable = true;
            grid.Model[6, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = "," };
            grid.Model[6, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[6, 2].CellValue = 10000;
            grid.CurrentCellActivated += new GridRoutedEventHandler(grid_CurrentCellActivated);
        }
        void grid_CurrentCellActivated(object sender, SyncfusionRoutedEventArgs args)
        {
            GridControlBase current_cell = args.Source as GridControlBase;
            if (grid.Model[current_cell.CurrentCell.RowIndex, current_cell.CurrentCell.ColumnIndex].CellType == "IntegerEdit")
            {
                g = grid.Model[current_cell.CurrentCell.RowIndex, current_cell.CurrentCell.ColumnIndex];
                SetSeperatorText();
                SetNumberOfDigitsText();
            }
        }
        void SetSeperatorText()
        {
            NumSeperator.Text = g.NumberFormat.NumberGroupSeparator;
        }

        void SetNumberOfDigitsText()
        {
            String s = "";
            foreach (int i in g.NumberFormat.NumberGroupSizes)
                s += i + " ";
            NoLimit.Text = s;
        }
        private void SetAll(object sender, RoutedEventArgs e)
        {
            if (NoLimit.Text != "")
                SetNumberLimit();
            if (NumSeperator.Text != "")
                SetSeperator();
        }

        private void SetNumberLimit()
        {
            try
            {
                String s = NoLimit.Text;
                List<int> nos = new List<int>();
                int counter = 0;
                foreach (string subString in s.Split(' '))
                {
                    if (subString == "" || subString == " ")
                        break;
                    nos.Add(int.Parse(subString));
                }
                int[] size = new int[nos.Count];
                foreach (int no in nos)
                {
                    size[counter] = no;
                    counter++;
                }
                InitializeNumberFormat(g.NumberFormat.NumberGroupSeparator, size);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter valid Input");
                NoLimit.Text = "";
            }
        }

        private void SetSeperator()
        {
            InitializeNumberFormat(NumSeperator.Text, g.NumberFormat.NumberGroupSizes);
        }
        private void InitializeNumberFormat(String separator, int[] format)
        {
            g.NumberFormat = new NumberFormatInfo { NumberGroupSeparator = separator, NumberGroupSizes = format };
            this.grid.CurrentCell.EndEdit();
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
