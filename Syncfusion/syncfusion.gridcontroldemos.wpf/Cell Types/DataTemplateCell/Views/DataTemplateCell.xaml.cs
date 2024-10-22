﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for DataTemplateCell.xaml
    /// </summary>
    public partial class DataTemplateCell : DemoControl
    {
        public DataTemplateCell()
        {
            InitializeComponent();
            GridSettings();
        }

        public DataTemplateCell(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            Width = 1060;
            Height = 600;

            gridControl1.Model.RowCount = 15;
            gridControl1.Model.ColumnCount = 7;
            gridControl1.Model.RowHeights.DefaultLineSize = 32;
            gridControl1.Model.ColumnWidths.DefaultLineSize = 138;
            gridControl1.Model.ColumnWidths[0] = 138;
            gridControl1.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            this.LoadData();
            this.LoadCellValues();
        }

        private void LoadData()
        {
            var mod = gridControl1.Model;
            mod[0, 0].CellValue = "Country/Population";
            mod[1, 0].CellValue = "USA";
            mod[2, 0].CellValue = "India";
            mod[3, 0].CellValue = "China";
            mod[4, 0].CellValue = "Japan";
            mod[5, 0].CellValue = "Russia";
            mod[6, 0].CellValue = "Pakistan";
            mod[7, 0].CellValue = "Brazil";
            mod[8, 0].CellValue = "Indonesia";
            mod[9, 0].CellValue = "Nigeria";
            mod[10, 0].CellValue = "Bangladesh";
            mod[11, 0].CellValue = "Mexico";
            mod[12, 0].CellValue = "Philippines";
            mod[13, 0].CellValue = "Egypt";
            mod[14, 0].CellValue = "Unistates";
            mod[0, 1].CellValue = "Flag";
            mod[0, 2].CellValue = "2006 (%)";
            mod[0, 3].CellValue = "2007 (%)";
            mod[0, 4].CellValue = "2008 (%)";
            mod[0, 5].CellValue = "2009 (%)";
            mod[0, 6].CellValue = "2010 (%)";
        }

        public void LoadCellValues()
        {
            Random r = new Random();
            for (int row = 0; row <= 14; row++)
            {
                for (int coll = 0; coll <= 7; coll++)
                {
                    if (coll != 0 && row != 0 && coll != 1)
                        gridControl1.Model[row, coll].CellValue = ((double)r.Next(2, 18)).ToString();

                    if (coll == 0 || row == 0)
                    {
                        gridControl1.Model[row, coll].CellType = "DataBoundTemplate";
                        gridControl1.Model[row, coll].CellItemTemplateKey = "Headertemplate";
                    }

                    if (coll == 1 && row > 0)
                    {
                        gridControl1.Model[row, coll].CellValue = @"/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Flags/" + gridControl1.Model[row, 0].CellValue.ToString() + ".jpg";
                        gridControl1.Model[row, coll].CellType = "DataBoundTemplate";
                        gridControl1.Model[row, coll].CellItemTemplateKey = "ImageTemplate";
                        gridControl1.Model[row, coll].CellEditTemplateKey = "ImageTemplate";
                    }

                    if (coll == 2 && row > 0)
                    {
                        gridControl1.Model[row, coll].CellType = "DataBoundTemplate";
                        gridControl1.Model[row, coll].CellItemTemplateKey = "TextTemplate";
                        gridControl1.Model[row, coll].CellEditTemplateKey = "TextTemplate";
                    }

                    if (coll == 4 && row > 0)
                    {
                        gridControl1.Model[row, coll].CellType = "DataBoundTemplate";
                        gridControl1.Model[row, coll].CellItemTemplateKey = "ProgressbarTemplate";
                        gridControl1.Model[row, coll].CellEditTemplateKey = "ProgressbarTemplate";
                    }
                    if (coll == 6 && row > 0)
                    {
                        gridControl1.Model[row, coll].CellType = "DataBoundTemplate";
                        gridControl1.Model[row, coll].CellItemTemplateKey = "SliderTemplate";
                        gridControl1.Model[row, coll].CellEditTemplateKey = "SliderTemplate";
                    }
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
