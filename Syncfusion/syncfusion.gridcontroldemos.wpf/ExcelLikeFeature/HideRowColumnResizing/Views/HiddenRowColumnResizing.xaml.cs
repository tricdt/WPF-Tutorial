﻿using syncfusion.demoscommon.wpf;
using System.Windows.Controls;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for HiddenRowColumnResizing.xaml
    /// </summary>
    public partial class HiddenRowColumnResizing : DemoControl
    {
        public HiddenRowColumnResizing()
        {
            InitializeComponent();
            GridSettings();
        }

        public HiddenRowColumnResizing(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            this.gridControl.Model.RowCount = 30;
            this.gridControl.Model.ColumnCount = 25;
            this.gridControl.Model.Options.ExcelLikeCurrentCell = true;
            this.gridControl.Model.Options.ExcelLikeSelectionFrame = true;
            this.gridControl.Model.Options.HiddenBorderBrush = Brushes.Black;
            this.gridControl.Model.Options.HiddenBorderThickness = 4;
            this.gridControl.Model.Options.AllowExcelLikeResizing = true;

            for (int i = 1; i < 30; i++)
            {
                for (int j = 1; j < 25; j++)
                {
                    this.gridControl.Model[i, j].CellValue = string.Format("R{0}C{1}", i, j);
                }
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button bt = sender as Button;
            if (bt.Content.ToString().Equals("Hide Rows and Columns"))
            {
                bt.Content = "Show Rows and Columns";
                this.gridControl.ColumnWidths.SetHidden(3, 4, true);
                this.gridControl.RowHeights.SetHidden(3, 4, true);

            }
            else
            {
                bt.Content = "Hide Rows and Columns";
                this.gridControl.ColumnWidths.SetHidden(3, 4, false);
                this.gridControl.RowHeights.SetHidden(3, 4, false);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.gridControl != null)
            {
                this.gridControl.Dispose();
                this.gridControl = null;
            }
            base.Dispose(disposing);
        }
    }
}
