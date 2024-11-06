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
    /// Interaction logic for DoubleEditCell.xaml
    /// </summary>
    public partial class DoubleEditCell : DemoControl
    {
        GridStyleInfo g = new GridStyleInfo();
        public DoubleEditCell()
        {
            InitializeComponent();
            GridSettings();
        }

        public DoubleEditCell(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;

            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(1, 1, 3, 4));
            grid.Model[1, 1].CellValue = "Double Edit";
            grid.Model[1, 1].Foreground = Brushes.Black;
            grid.Model[1, 1].Background = Brushes.LightBlue;
            grid.Model[1, 1].Font.FontSize = 18;
            grid.Model[1, 1].HorizontalAlignment = HorizontalAlignment.Center;
            grid.Model[1, 1].Font.FontWeight = FontWeights.Bold;

            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(4, 1, 5, 4));
            grid.Model[4, 1].CellValue = "This sample showcases all the properties that can be set for Double Edit Celltypes.";
            grid.Model[4, 1].Foreground = Brushes.Black;
            grid.Model[4, 1].Font.FontSize = 12;
            grid.Model[4, 1].HorizontalAlignment = HorizontalAlignment.Left;
            grid.Model[4, 1].Font.FontWeight = FontWeights.Bold;

            int[] sizes = { 2, 3, 4 };
            grid.Model[6, 2].CellType = "DoubleEdit";
            grid.Model[6, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = ",", NumberDecimalSeparator = ".", NumberDecimalDigits = 4 };
            grid.Model[6, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[6, 2].CellValue = 2345.00;

            grid.Model[8, 2].CellType = "DoubleEdit";
            grid.Model[8, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = ",", NumberDecimalSeparator = ".", NumberDecimalDigits = 4 };
            grid.Model[8, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[8, 2].CellValue = 12;

            grid.Model[10, 2].CellType = "DoubleEdit";
            grid.Model[10, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = ",", NumberDecimalSeparator = ".", NumberDecimalDigits = 1 };
            grid.Model[10, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[10, 2].CellValue = 100;

            grid.Model[12, 2].CellType = "DoubleEdit";
            grid.Model[12, 2].NumberFormat = new NumberFormatInfo { NumberGroupSeparator = ",", NumberDecimalSeparator = ".", NumberDecimalDigits = 0 };
            grid.Model[12, 2].NumberFormat.NumberGroupSizes = sizes;
            grid.Model[12, 2].CellValue = 12345678.00;

            grid.CurrentCellActivated += new GridRoutedEventHandler(Grid_CurrentCellActivated);
        }

        private void Grid_CurrentCellActivated(object sender, SyncfusionRoutedEventArgs args)
        {
            GridControlBase current_cell = args.Source as GridControlBase;
            if (grid.Model[current_cell.CurrentCell.RowIndex, current_cell.CurrentCell.ColumnIndex].CellType == "DoubleEdit")
            {
                g = grid.Model[current_cell.CurrentCell.RowIndex, current_cell.CurrentCell.ColumnIndex];
                SetNumberSeperatorText();
                SetNumberOfDigitsText();
                SetNumberOfDecimalDigitsText();
                SetDecimalSeperatorText();
            }
        }

        public void SetNumberSeperatorText()
        {
            NumberSeparator.Text = g.NumberFormat.NumberGroupSeparator;
        }

        public void SetNumberOfDigitsText()
        {
            String s = "";
            foreach (int i in g.NumberFormat.NumberGroupSizes)
                s += i + " ";
            NumberGroup.Text = s;
        }

        public void SetNumberOfDecimalDigitsText()
        {
            NoOfDecimalDigits.Text = Convert.ToString(g.NumberFormat.NumberDecimalDigits);
        }

        public void SetDecimalSeperatorText()
        {
            DecimalSeparator.Text = g.NumberFormat.NumberDecimalSeparator;
        }

        private void SetAll(object sender, RoutedEventArgs e)
        {
            int DecimalDigit = 0;

            String DecimalSeperator = ".";


            String IntegerSeperator = ",";


            int[] IntegerGroup = GetNumberGroupSizes();

            int count = NumberSeparator.Text.Count();

            int count3 = DecimalSeparator.Text.Count();


            try
            {
                int count2 = NoOfDecimalDigits.Text.Count();

                if (!string.IsNullOrEmpty(NoOfDecimalDigits.Text) && count2 < 3)
                    DecimalDigit = Convert.ToInt32(NoOfDecimalDigits.Text);
                else
                {
                    MessageBox.Show("please enter valid input for DecimalDigit ");
                    NoOfDecimalDigits.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Input string for decimal digit is not in correct format");
            }
            try
            {

                if (!string.IsNullOrEmpty(DecimalSeparator.Text) && count3 < 4)
                    DecimalSeperator = DecimalSeparator.Text;
                else
                {
                    MessageBox.Show("please enter valid input for  DecimalSeperator");
                    DecimalSeparator.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Input string for  DecimalSeperator is not in correct format");
            }
            try
            {



                if (!string.IsNullOrEmpty(NumberSeparator.Text) && count < 4)
                {
                    IntegerSeperator = NumberSeparator.Text;
                }

                else
                {
                    MessageBox.Show("please enter valid input for NumberSeparator");
                    NumberSeparator.Text = "";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Input string for NumberSeparator is not in correct format");
            }
            try
            {


                initialiseNumberFormat(DecimalDigit, DecimalSeperator, IntegerGroup, IntegerSeperator);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter valid input");

            }
        }

        private void initialiseNumberFormat(int DecimalDigit, String DecimalSeperator, int[] IntegerGroup, String IntegerSeperator)
        {
            try
            {
                if (g != null)
                {
                    g.NumberFormat = new NumberFormatInfo
                    {
                        NumberDecimalDigits = DecimalDigit,
                        NumberDecimalSeparator = DecimalSeperator,
                        NumberGroupSeparator = IntegerSeperator,
                        NumberGroupSizes = IntegerGroup
                    };
                    this.grid.InvalidateCell(new RowColumnIndex(g.RowIndex, g.ColumnIndex));
                }
            }
            catch (Exception)
            { }
        }

        private int[] GetNumberGroupSizes()
        {
            String s = NumberGroup.Text;
            {
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
                return size;
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
