﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.GridCommon;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for GridProperties.xaml
    /// </summary>
    public partial class GridProperties : DemoControl
    {
        public GridProperties()
        {
            InitializeComponent();
            GridSettings();
        }

        public GridProperties(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;

            for (int i = 1; i < 35; i++)
                for (int j = 1; j < 25; j++)
                    grid.Model[i, j].CellValue = "row" + i + "col" + j;

            fg.ItemsSource = getColors();
            lc.ItemsSource = getColors();
            bg.ItemsSource = getColors();
        }
        public String[] getColors()
        {
            String[] colors = { "Yellow", "Blue", "Green", "Red", "Pink", "Maroon", "Brown", "Black", "Violet" };
            return colors;
        }

        private void RHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            this.grid.ColumnWidths.SetHidden(0, 0, true);
            this.grid.InvalidateCells();
        }

        private void RowHeaderChecked(object sender, RoutedEventArgs e)
        {
            this.grid.ColumnWidths.SetHidden(0, 0, false);
            this.grid.InvalidateCells();
        }

        private void CHeader_Checked(object sender, RoutedEventArgs e)
        {
            this.grid.RowHeights.SetHidden(0, 0, false);
            this.grid.InvalidateCells();
        }

        private void CHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            this.grid.RowHeights.SetHidden(0, 0, true);
            this.grid.InvalidateCells();
        }

        private void HLine_Checked(object sender, RoutedEventArgs e)
        {
            Pen bLinePen = new Pen(Brushes.LightGray, 1.0);
            if (this.grid.Model.TableStyle.Borders.Bottom != null)
            {
                bLinePen.Brush = this.grid.Model.TableStyle.Borders.Bottom.Brush;
            }
            else if (lc.SelectedItem != null)
            {
                bLinePen.Brush = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(lc.SelectedItem.ToString()));
            }

            this.grid.Model.TableStyle.Borders.Bottom = bLinePen;
            this.grid.InvalidateCells();
        }

        private void HLine_Unchecked(object sender, RoutedEventArgs e)
        {
            this.grid.Model.TableStyle.Borders.Bottom = null;
            this.grid.InvalidateCells();
        }

        private void VLine_Checked(object sender, RoutedEventArgs e)
        {
            Pen rLinePen = new Pen(Brushes.LightGray, 1.0);
            if (this.grid.Model.TableStyle.Borders.Right != null)
            {
                rLinePen.Brush = this.grid.Model.TableStyle.Borders.Right.Brush;
            }
            else if (lc.SelectedItem != null)
            {
                rLinePen.Brush = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(lc.SelectedItem.ToString()));
            }

            this.grid.Model.TableStyle.Borders.Right = rLinePen;
            this.grid.InvalidateCells();
        }

        private void VLine_Unchecked(object sender, RoutedEventArgs e)
        {
            this.grid.Model.TableStyle.Borders.Right = null;
            this.grid.InvalidateCells();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)sender;
            String text = item.Content.ToString();
            GridSelectionFlags flag = GridSelectionFlags.Any;
            switch (text)
            {
                case "Any":
                    flag = GridSelectionFlags.Any;
                    break;
                case "Cell":
                    flag = GridSelectionFlags.Cell;
                    break;
                case "Column":
                    flag = GridSelectionFlags.Column;
                    break;
                case "Keyboard":
                    flag = GridSelectionFlags.Keyboard;
                    break;
                case "Mix range type":
                    flag = GridSelectionFlags.MixRangeType;
                    break;
                case "Multiple":
                    flag = GridSelectionFlags.Multiple;
                    break;
                case "None":
                    flag = GridSelectionFlags.None;
                    break;
                case "Row":
                    flag = GridSelectionFlags.Row;
                    break;
                case "Shift":
                    flag = GridSelectionFlags.Shift;
                    break;
                case "Table":
                    flag = GridSelectionFlags.Table;
                    break;
            }
            grid.Model.Options.AllowSelection = flag;
            this.grid.InvalidateCells();
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)sender;
            String text = item.Content.ToString();
            GridCellActivateAction flag = GridCellActivateAction.ClickOnCell;
            switch (text)
            {
                case "OnClick":
                    flag = GridCellActivateAction.ClickOnCell;
                    break;
                case "OnDoubleClick":
                    flag = GridCellActivateAction.DblClickOnCell;
                    break;
                case "None":
                    flag = GridCellActivateAction.None;
                    break;
                case "Position caret":
                    flag = GridCellActivateAction.PositionCaret;
                    break;
                case "Multiple":
                    flag = GridCellActivateAction.SelectAll;
                    break;
                case "SetCurrent":
                    flag = GridCellActivateAction.SetCurrent;
                    break;

            }
            grid.Model.Options.ActivateCurrentCellBehavior = flag;
            this.grid.InvalidateCells();
        }

        private void bg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.grid.Model.TableStyle.Background = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(bg.SelectedItem.ToString()));
            this.grid.InvalidateCells();
        }

        private void fg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.grid.Model.TableStyle.Foreground = new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(fg.SelectedItem.ToString()));
            this.grid.InvalidateCells();
        }

        private void lc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.grid.Model.TableStyle.Borders.Right != null)
            {
                this.grid.Model.TableStyle.Borders.Right = new Pen(new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(lc.SelectedItem.ToString())), 1.0);
            }
            if (this.grid.Model.TableStyle.Borders.Bottom != null)
            {
                this.grid.Model.TableStyle.Borders.Bottom = new Pen(new SolidColorBrush(GridUtil.GetXamlConvertedValue<Color>(lc.SelectedItem.ToString())), 1.0);
            }
            this.grid.InvalidateCells();
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
