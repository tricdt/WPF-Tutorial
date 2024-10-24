﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.GridCommon;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for GridStyleInfoAtWork.xaml
    /// </summary>
    public partial class GridStyleInfoAtWork : DemoControl
    {
        public GridStyleInfoAtWork()
        {
            InitializeComponent();
            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;
            tableBg.ItemsSource = getColors();
            headerBg.ItemsSource = getColors();
            RowColumnBg.ItemsSource = getColors();
        }

        public GridStyleInfoAtWork(string themename) : base(themename)
        {
            InitializeComponent();
            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;
            tableBg.ItemsSource = getColors();
            headerBg.ItemsSource = getColors();
            RowColumnBg.ItemsSource = getColors();
        }

        public String[] getColors()
        {
            String[] colors = { "LightBlue", "LightPink", "LightSeaGreen", "MintCream", "LightGray", "LightGreen", "LightSalmon", "LightSteelBlue", "NavajoWhite" };
            return colors;
        }
        private void RowColBackground()
        {
            if (RowColumnBg.SelectedItem != null)
            {
                GridBaseStyle basestyle = new GridBaseStyle();
                basestyle.Name = "Applied Background";
                basestyle.StyleInfo = new GridStyleInfo() { Background = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(RowColumnBg.SelectedItem.ToString())) };
                grid.Model.BaseStylesMap.Add(basestyle);
                try
                {
                    var index = Convert.ToInt32(ColumnRowIndex.Text);

                    this.grid.InvalidateCells();
                    if (ColumnRowIndex.Text != "")
                    {
                        for (int i = 1; i < grid.Model.ColumnCount; i++)
                        {
                            grid.Model[index, i].BaseStyle = "Applied Background";
                            grid.Model.InvalidateCell(new RowColumnIndex(index, i));
                        }
                        for (int i = 1; i < grid.Model.RowCount; i++)
                        {
                            grid.Model[i, index].BaseStyle = "Applied Background";
                            grid.Model.InvalidateCell(new RowColumnIndex(i, index));
                        }
                    }
                }
                catch
                {
                    ColumnRowIndex.Text = " ";
                }
            }
        }

        private void HeaderBackground()
        {
            if (headerBg.SelectedItem != null)
            {
                this.grid.Model.HeaderStyle.Background = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(headerBg.SelectedItem.ToString()));
                this.grid.InvalidateCells();
            }
        }

        private void TableBackground()
        {
            if (tableBg.SelectedItem != null)
            {
                this.grid.Model.TableStyle.Background = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(tableBg.SelectedItem.ToString()));
                this.grid.InvalidateCells();

            }
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            RowColBackground();
            HeaderBackground();
            TableBackground();
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
