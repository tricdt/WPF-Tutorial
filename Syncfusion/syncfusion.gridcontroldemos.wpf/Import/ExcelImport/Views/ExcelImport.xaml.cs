﻿using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Grid.Converter;
using Syncfusion.Windows.Tools.Controls;
using Syncfusion.XlsIO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelImport.xaml
    /// </summary>
    public partial class ExcelImport : RibbonWindow, IExcelImport
    {
        private GridModel[] gridModelCollection;
        public ExcelImport()
        {
            InitializeComponent();
            this.DataContext = new ExcelImportViewModel(this);
            this.ForegroundColorPicker.SelectedBrushChanged += ForegroundColorPicker_SelectedBrushChanged;
            this.BackgroundColorPicker.SelectedBrushChanged += BackgroundColorPicker_SelectedBrushChanged;
            this.ForeColorSplitButton.Click += new RoutedEventHandler(ForeColorSplitButton_Click);
            this.BGColorSplitButton.Click += new RoutedEventHandler(BGColorSplitButton_Click);
        }

        private void ForegroundColorPicker_SelectedBrushChanged(object? sender, SelectedBrushChangedEventArgs e)
        {
            var color = this.ForegroundColorPicker.Color;
            GridControl ActiveGrid = ((tControl.SelectedItem as TabItem).Content as ScrollViewer).Content as GridControl;
            if (ActiveGrid != null)
            {
                GridRangeInfoList rangeList = ActiveGrid.Model.SelectedRanges;
                foreach (GridRangeInfo range in rangeList)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            ActiveGrid.Model[row, col].Foreground = new SolidColorBrush(color);
                        }
                    }
                    ActiveGrid.InvalidateCell(range);
                }
                ActiveGrid.InvalidateVisual();
            }
            this.ForeColorSplitButton.IsDropDownOpen = false;
        }
        private void BackgroundColorPicker_SelectedBrushChanged(object? sender, SelectedBrushChangedEventArgs e)
        {
            var color = this.BackgroundColorPicker.Color;
            GridControl ActiveGrid = ((tControl.SelectedItem as TabItem).Content as ScrollViewer).Content as GridControl;
            if (ActiveGrid != null)
            {
                GridRangeInfoList rangeList = ActiveGrid.Model.SelectedRanges;
                foreach (GridRangeInfo range in rangeList)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            ActiveGrid.Model[row, col].Background = new SolidColorBrush(color);
                        }
                    }
                    ActiveGrid.InvalidateCell(range);
                }
                ActiveGrid.InvalidateVisual();
            }
            this.BGColorSplitButton.IsDropDownOpen = false;
        }
        private void ForeColorSplitButton_Click(object sender, RoutedEventArgs e)
        {
            var color = this.ForegroundColorPicker.Color;
            GridControl ActiveGrid = ((tControl.SelectedItem as TabItem).Content as ScrollViewer).Content as GridControl;
            if (ActiveGrid != null)
            {
                GridRangeInfoList rangeList = ActiveGrid.Model.SelectedRanges;
                foreach (GridRangeInfo range in rangeList)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            ActiveGrid.Model[row, col].Foreground = new SolidColorBrush(color);
                        }
                    }
                    ActiveGrid.InvalidateCell(range);
                }
                ActiveGrid.InvalidateVisual();
            }
            this.ForeColorSplitButton.IsDropDownOpen = false;
        }
        private void BGColorSplitButton_Click(object sender, RoutedEventArgs e)
        {
            var color = this.BackgroundColorPicker.Color;
            GridControl ActiveGrid = ((tControl.SelectedItem as TabItem).Content as ScrollViewer).Content as GridControl;
            if (ActiveGrid != null)
            {
                GridRangeInfoList rangeList = ActiveGrid.Model.SelectedRanges;
                foreach (GridRangeInfo range in rangeList)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            ActiveGrid.Model[row, col].Background = new SolidColorBrush(color);
                        }
                    }
                    ActiveGrid.InvalidateCell(range);
                }
                ActiveGrid.InvalidateVisual();
            }
            this.BGColorSplitButton.IsDropDownOpen = false;
        }

        public GridModel[] GridModelCollection
        {
            get
            {
                return gridModelCollection;
            }
        }

        public void LoadWorkbook(IWorkbook Workbook)
        {
            tControl.Items.Clear();
            gridModelCollection = GridModelImportExtensions.ImportFromExcelToVirtualGrid(Workbook);
            for (int i = 0; i < Workbook.Worksheets.Count; i++)
            {
                GridControl grid = new GridControl();
                grid.Model = GridModelCollection[i];
                grid.Model.ColumnWidths[0] = 35d;
                grid.Model.Options.WrapCell = false;
                grid.Model.Options.EnableFloatingCell = true;
                grid.Model.Options.ExcelLikeFreezePane = true;
                grid.Model.Options.ExcelLikeCurrentCell = true;
                grid.Model.Options.AllowExcelLikeResizing = true;
                grid.Model.Options.ExcelLikeSelectionFrame = true;
                grid.Model.CommandStack.Enabled = true;
                grid.Model.TableStyle.TextWrapping = TextWrapping.NoWrap;
                grid.Model.TableStyle.FloatCellMode = GridFloatCellsMode.OnDemandCalculation;
                grid.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
                grid.Model.Options.CopyPasteOption = CopyPaste.CopyText | CopyPaste.CutText | CopyPaste.PasteText;
                GridCommentService.SetShowComment(grid, true);
                BehaviorCollection GridBehaviors = Interaction.GetBehaviors(grid);
                GridBehaviors.Add(new GridCellImportingBehavior());
                GridBehaviors.Add(new GridCellExportingBehavior());
                GridBehaviors.Add(new CurrentCellEditingBehavior());
                GridBehaviors.Add(new CellRequestNavigateBehavior());
                GridBehaviors.Add(new CurrentCellSyncBehavior());
                TabItemExt tab = new TabItemExt();
                tab.Header = Workbook.Worksheets[i].Name;
                ScrollViewer sv = new ScrollViewer();
                sv.CanContentScroll = true;
                sv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                sv.Content = grid;
                tab.Content = sv;
                if (Workbook.Worksheets[i].Visibility != WorksheetVisibility.Visible)
                {
                    tab.Visibility = Visibility.Collapsed;
                }
                tControl.Items.Add(tab);
            }
        }

        public void GidCellRequestNavigate(string SheetName, int RowIndex, int ColumnIndex)
        {
            TabItemExt tab = null;
            int modelcount = -1;
            foreach (TabItemExt item in tControl.Items)
            {
                modelcount++;
                if (item.Header.ToString() == SheetName)
                {
                    tab = item;
                    break;
                }
            }
            tControl.SelectedIndex = modelcount;
            GridControl gridctrl = ((tab.Content as ScrollViewer).Content as GridControl);
            if (tab != null && gridctrl != null && RowIndex > 0 && ColumnIndex > 0)
            {
                gridctrl.CurrentCell.MoveTo(RowIndex, ColumnIndex);
                gridctrl.CurrentCell.ScrollInView();
            }
        }

        public void ExecuteCopyCommand(int ActiveTabIndex)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            if (ActiveGridModel != null)
            {
                string CliboardText;
                int row, col;
                ActiveGridModel.TextDataExchange.CopyTextToBuffer(out CliboardText, ActiveGridModel.SelectedRanges, out row, out col, false);
                Clipboard.SetText(CliboardText);
            }
        }

        public void ExecuteCutCommand(int ActiveTabIndex)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            if (ActiveGridModel != null)
            {
                string CliboardText;
                int row, col;
                ActiveGridModel.TextDataExchange.CopyTextToBuffer(out CliboardText, ActiveGridModel.SelectedRanges, out row, out col, true);
                Clipboard.SetText(CliboardText);
            }
        }

        public void ExecutePasteCommand(int ActiveTabIndex)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            if (ActiveGridModel != null)
            {
                string CliboardText = Clipboard.GetText();
                ActiveGridModel.TextDataExchange.PasteTextFromBuffer(CliboardText, ActiveGridModel.SelectedRanges);
            }
        }

        public void ExecuteFontSizeCommand(int ActiveTabIndex, bool IsIncrement)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            {
                if (ActiveGridModel != null && !ActiveGridModel.CurrentCellState.GridControl.CurrentCell.IsInMoveTo)
                {
                    foreach (GridRangeInfo range in ActiveGridModel.SelectedRanges)
                    {
                        for (int row = range.Top; row <= range.Bottom; row++)
                        {
                            for (int col = range.Left; col <= range.Right; col++)
                            {
                                if (IsIncrement)
                                    ActiveGridModel[row, col].Font.FontSize += 1;
                                else
                                    ActiveGridModel[row, col].Font.FontSize -= 1;
                            }
                        }
                        ActiveGridModel.InvalidateCell(range);
                    }
                }
            }
        }

        public void CurrentCellStyleChanged(int ActiveTabIndex, string propertyName, object value)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            if (ActiveGridModel != null && ActiveGridModel.CurrentCellState.GridControl != null
                && !ActiveGridModel.CurrentCellState.GridControl.CurrentCell.IsInMoveTo)
            {
                foreach (GridRangeInfo range in ActiveGridModel.SelectedRanges)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            switch (propertyName)
                            {
                                case "FontFamily":
                                    ActiveGridModel[row, col].Font.FontFamily = (FontFamily)value;
                                    break;
                                case "FontSize":
                                    ActiveGridModel[row, col].Font.FontSize = (double)value;
                                    break;
                                case "FontWeight":
                                    ActiveGridModel[row, col].Font.FontWeight = (FontWeight)value;
                                    break;
                                case "FontStyle":
                                    ActiveGridModel[row, col].Font.FontStyle = (FontStyle)value;
                                    break;
                                case "TextDecorations":
                                    ActiveGridModel[row, col].Font.TextDecorations = (TextDecorationCollection)value;
                                    break;
                                case "Background":
                                    ActiveGridModel[row, col].Background = new SolidColorBrush(BackgroundColorPicker.Color);
                                    break;
                                case "Foreground":
                                    ActiveGridModel[row, col].Foreground = new SolidColorBrush(ForegroundColorPicker.Color);
                                    break;
                                case "HorizontalAlignment":
                                    ActiveGridModel[row, col].HorizontalAlignment = (HorizontalAlignment)value;
                                    if ((HorizontalAlignment)value == HorizontalAlignment.Left)
                                        ActiveGridModel[row, col].CellValue2 = ExcelHAlign.HAlignLeft;
                                    break;
                                case "VerticalAlignment":
                                    ActiveGridModel[row, col].VerticalAlignment = (VerticalAlignment)value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ActiveGridModel.InvalidateCell(range);
                }
            }
        }

        public void ExecuteUndoCommand(int ActiveTabIndex)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            if (!ActiveGridModel.CommandStack.InTransaction)
            {
                ActiveGridModel.CommandStack.Undo();
            }
        }

        public void ExecuteRedoCommand(int ActiveTabIndex)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            if (!ActiveGridModel.CommandStack.InTransaction)
            {
                ActiveGridModel.CommandStack.Redo();
            }
        }

        public void ExecutePrintCommand(int ActiveTabIndex)
        {
            GridModel ActiveGridModel = GridModelCollection[ActiveTabIndex];
            ActiveGridModel.ActiveGridView.Print();
        }
    }
}
