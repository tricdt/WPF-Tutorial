﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for FormulaTestValues.xaml
    /// </summary>
    public partial class FormulaTestValues : DemoControl
    {
        public FormulaTestValues()
        {
            InitializeComponent();
            FormatGrid();
        }
        public FormulaTestValues(string themename) : base(themename)
        {
            InitializeComponent();
            FormatGrid();
        }

        Brush row1Brush = new LinearGradientBrush(Color.FromArgb(255, 0xee, 0x7a, 0x03), Color.FromArgb(255, 0xcc, 0x8c, 0x06), 0);
        Brush formulaNameBrush = new SolidColorBrush(Color.FromArgb(255, 0xc4, 0xd6, 0xe9));
        Brush rowBrush = new SolidColorBrush(Color.FromArgb(255, 0xed, 0xf0, 0xf6));
        Brush badCompareBrush = new SolidColorBrush(Color.FromArgb(255, 0xff, 0x99, 0x33));
        Brush goodCompareBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
        private Hashtable formulas;

        private void FormatGrid()
        {
            grid.Model.TableStyle.CellType = "FormulaCell";
            LoadTsvFile("good.tsv", grid);
            FormatHeaders();
            SetColumnWidthsRowHeightsStyles();
            PopulateNamesHashtable();
        }
        private void PopulateNamesHashtable()
        {
            GridFormulaEngine engine = ((GridCellFormulaModel)grid.Model.CellModels["FormulaCell"]).Engine;
            this.formulas = engine.LibraryFunctions.Clone() as Hashtable;

            this.formulas.Add("+ OPERATOR", "");
            this.formulas.Add("- OPERATOR", "");
            this.formulas.Add("* OPERATOR", "");
            this.formulas.Add("/ OPERATOR", "");
            this.formulas.Add("> OPERATOR", "");
            this.formulas.Add("< OPERATOR", "");
            this.formulas.Add(" = OPERATOR", "");
            this.formulas.Add(">= OPERATOR", "");
            this.formulas.Add("<= OPERATOR", "");
            this.formulas.Add("<> OPERATOR", "");
            this.formulas.Add("^ EXPONENT", "");
            this.formulas.Add("- UNARY MINUS", "");
        }

        #region format headers...

        private void FormatHeaders()
        {
            //subscribe to event to funish A, B, C, ... 1, 2, 3, ....
            grid.Model.QueryCellInfo += new GridQueryCellInfoEventHandler(Model_QueryCellInfo);

            //set some colors blues
            Color dark = Color.FromArgb(255, 0xc4, 0xd6, 0xe9);

            grid.Model.Options.FormulaDisplayBehavior = GridShowFormulaBehavior.WhenCurrent;
            grid.Model.FrozenRows = 2;
        }

        #endregion

        #region QueryCellInfo code - used for dynamic formatting

        void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex > 0)
            {
                //column header
                e.Style.CellValue = GridRangeInfo.GetAlphaLabel(e.Cell.ColumnIndex);
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (e.Cell.ColumnIndex == 0 && e.Cell.RowIndex > 0)
            {
                //row header
                e.Style.CellValue = e.Cell.RowIndex.ToString();
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
            }
            else if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex == 0)
            {
                //TopLeft Cell
            }
            else if (e.Cell.RowIndex == 1 && e.Cell.ColumnIndex > 0)
            {
                //row 1
                e.Style.Font.FontWeight = FontWeights.Bold;
            }
            else if (e.Cell.RowIndex > 1 && e.Cell.ColumnIndex > 0)
            {
                string s2 = this.grid.Model[e.Cell.RowIndex, 2].Text.Trim();
                string s3 = this.grid.Model[e.Cell.RowIndex, 3].Text.Trim();
                if (this.formulas.ContainsKey(s2.ToUpper()) && !s3.StartsWith("="))
                {
                    e.Style.Font.FontWeight = FontWeights.Bold;
                    e.Style.Background = formulaNameBrush;
                }
                else
                {
                    s2 = this.grid.Model[e.Cell.RowIndex, 1].Text;
                    if (s2.Length > 0)
                    {
                        e.Style.Background = rowBrush;
                    }
                }
            }
        }
        #endregion

        #region set up ColumnWidths, RowHeights and Styles

        private void SetColumnWidthsRowHeightsStyles()
        {
            grid.Model.ColumnWidths.DefaultLineSize = 130;
            grid.Model.ColumnWidths[0] = 35;
            grid.Model.ColumnWidths[4] = 325;
            grid.Model.RowHeights[1] = 25;
            grid.CellSpanBackgrounds.Add(new CellSpanBackgroundInfo(1, 1, 1, grid.Model.ColumnCount, false, false, row1Brush, null));
        }

        #endregion

        #region Load/Save TSV files
        private void LoadTsvFile(string fileName, GridControl gridControl1)
        {
            try
            {
                string tab = "\t";
                string cr = "\n";
                fileName = @"Data\GridControl\" + fileName;
                if (fileName.Length > 0)
                {
                    StreamReader reader = new StreamReader(fileName);
                    string s = reader.ReadToEnd();
                    reader.Close();
                    string[] lines = s.Split(new string[] { cr }, StringSplitOptions.None);
                    int rowCount = lines.GetLength(0);
                    if (rowCount > 0)
                    {
                        //reset grid
                        grid.UnloadArrangedCells();
                        gridControl1.Model.RowCount = 1;
                        gridControl1.Model.ColumnCount = 1;

                        gridControl1.Model.RowCount = rowCount;
                        int row = 1;
                        foreach (string s1 in lines)
                        {
                            string[] cols = s1.Split(new string[] { tab }, StringSplitOptions.None);

                            int colCount1 = cols.GetLength(0);

                            if (colCount1 > gridControl1.Model.ColumnCount)
                                gridControl1.Model.ColumnCount = colCount1;

                            if (colCount1 > 1)
                            {
                                for (int col = 1; col <= colCount1; ++col)
                                {
                                    gridControl1.Model[row, col].Text = cols[col - 1];
                                }
                            }
                            row++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
            }
        }
        #endregion

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
