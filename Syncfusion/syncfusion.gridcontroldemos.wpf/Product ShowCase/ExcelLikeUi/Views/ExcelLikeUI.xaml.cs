using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for ExcelLikeUI.xaml
    /// </summary>
    public partial class ExcelLikeUI : RibbonWindow, IExcelLikeUi
    {
        public ExcelLikeUI()
        {
            InitializeComponent();
            this.DataContext = new ExcelLikeUiViewModel(this);
            this.ForegroundColorPicker.SelectedBrushChanged += OnForegroundColorPickerSelectedBrushChanged;
            this.BackgroundColorPicker.SelectedBrushChanged += OnBackgroundColorPickerSelectedBrushChanged;
            this.ForeColorSplitButton.Click += OnForeColorSplitButtonClick;
            this.BGColorSplitButton.Click += OnBGColorSplitButtonClick;
            this.BackgroundColorPicker.MoreColorWindowOpening += OnBackgroundColorPickerMoreColorWindowOpening;
            this.ForegroundColorPicker.MoreColorWindowOpening += OnForegroundColorPickerMoreColorWindowOpening;
        }

        private void OnForegroundColorPickerMoreColorWindowOpening(object sender, MoreColorCancelEventArgs args)
        {
            this.ForeColorSplitButton.IsDropDownOpen = false;
        }

        private void OnBackgroundColorPickerMoreColorWindowOpening(object sender, MoreColorCancelEventArgs args)
        {
            this.BGColorSplitButton.IsDropDownOpen = false;
        }

        private void OnBGColorSplitButtonClick(object sender, RoutedEventArgs e)
        {
            this.BGColorSplitButton.IsDropDownOpen = false;
        }

        private void OnForeColorSplitButtonClick(object sender, RoutedEventArgs e)
        {
            this.ForeColorSplitButton.IsDropDownOpen = false;
        }

        private void OnBackgroundColorPickerSelectedBrushChanged(object? sender, SelectedBrushChangedEventArgs e)
        {
            (this.DataContext as ExcelLikeUiViewModel).BackgroundCommand.Execute(null);
            this.BGColorSplitButton.IsDropDownOpen = false;
        }

        private void OnForegroundColorPickerSelectedBrushChanged(object? sender, SelectedBrushChangedEventArgs e)
        {
            (this.DataContext as ExcelLikeUiViewModel).ForegroundCommand.Execute(null);
            this.ForeColorSplitButton.IsDropDownOpen = false;
        }

        public void ColorPickerSelectedBrushChanged(string propertyName, Brush value)
        {
            if (propertyName == "Background")
            {
                this.BackgroundColorPicker.SelectedBrush = value;
            }
            else
            {
                this.ForegroundColorPicker.SelectedBrush = value;
            }
        }

        public void CurrentCellStyleChanged(int ActiveTabIndex, string propertyName, object value)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;

            if (!ActiveGrid.CurrentCell.HasCurrentCell)
                return;

            if (ActiveGrid != null && !ActiveGrid.Model.CurrentCellState.GridControl.CurrentCell.IsInMoveTo)
            {
                foreach (GridRangeInfo range in ActiveGrid.Model.SelectedRanges)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            switch (propertyName)
                            {
                                case "FontFamily":
                                    ActiveGrid.Model[row, col].Font.FontFamily = (FontFamily)value;
                                    break;
                                case "FontSize":
                                    {
                                        ActiveGrid.Model[row, col].Font.FontSize = (double)value;
                                        ActiveGrid.Model.ResizeRowsToFit(GridRangeInfo.Cell(row, col), GridResizeToFitOptions.None);
                                    }
                                    break;
                                case "FontWeight":
                                    ActiveGrid.Model[row, col].Font.FontWeight = (FontWeight)value;
                                    break;
                                case "FontStyle":
                                    ActiveGrid.Model[row, col].Font.FontStyle = (FontStyle)value;
                                    break;
                                case "TextDecorations":
                                    ActiveGrid.Model[row, col].Font.TextDecorations = (TextDecorationCollection)value;
                                    break;
                                case "Background":
                                    ActiveGrid.Model[row, col].Background = new SolidColorBrush(BackgroundColorPicker.Color);
                                    break;
                                case "Foreground":
                                    ActiveGrid.Model[row, col].Foreground = new SolidColorBrush(ForegroundColorPicker.Color);
                                    break;
                                case "HorizontalAlignment":
                                    ActiveGrid.Model[row, col].HorizontalAlignment = (HorizontalAlignment)value;
                                    break;
                                case "VerticalAlignment":
                                    ActiveGrid.Model[row, col].VerticalAlignment = (VerticalAlignment)value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    ActiveGrid.Model.InvalidateCell(range);
                }
            }
        }

        public void ExecuteCopyCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;
            if (ActiveGrid != null)
            {
                string CliboardText;
                int row, col;
                ActiveGrid.Model.TextDataExchange.CopyTextToBuffer(out CliboardText, ActiveGrid.Model.SelectedRanges, out row, out col, false);
                Clipboard.SetText(CliboardText);
            }
        }

        public void ExecuteCutCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;
            if (ActiveGrid != null)
            {
                string CliboardText;
                int row, col;
                ActiveGrid.Model.TextDataExchange.CopyTextToBuffer(out CliboardText, ActiveGrid.Model.SelectedRanges, out row, out col, true);
                Clipboard.SetText(CliboardText);
                //WPF-61741-Added condition to set the selectedrange as empty while cut the value.
                ActiveGrid.Model[ActiveGrid.Model.SelectedRanges.ActiveRange.Top, ActiveGrid.Model.SelectedRanges.ActiveRange.Left].CellValue = string.Empty;
            }
        }

        public void ExecuteFontSizeCommand(int ActiveTabIndex, bool IsIncrement)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;

            // Skip if the current cell is not set.
            if (!ActiveGrid.CurrentCell.HasCurrentCell)
                return;

            if (ActiveGrid != null && !ActiveGrid.Model.CurrentCellState.GridControl.CurrentCell.IsInMoveTo)
            {
                foreach (GridRangeInfo range in ActiveGrid.Model.SelectedRanges)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            if (IsIncrement)
                                ActiveGrid.Model[row, col].Font.FontSize += 1;
                            else
                                ActiveGrid.Model[row, col].Font.FontSize -= 1;
                        }
                    }
                    ActiveGrid.Model.InvalidateCell(range);
                }
            }
        }


        public void ExecuteIndentCommand(int ActiveTabIndex, bool IsIncrement)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;

            if (!ActiveGrid.CurrentCell.HasCurrentCell)
                return;

            if (ActiveGrid != null && !ActiveGrid.Model.CurrentCellState.GridControl.CurrentCell.IsInMoveTo)
            {
                foreach (GridRangeInfo range in ActiveGrid.Model.SelectedRanges)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            GridStyleInfo cell = ActiveGrid.Model[row, col];
                            double left = 0.0;
                            if (cell.HasTextMargins)
                                left = cell.TextMargins.Left;
                            if (IsIncrement)
                                cell.TextMargins = new CellMarginsInfo(left + 10, 0, 0, 0);
                            else if (left >= 10)
                                cell.TextMargins = new CellMarginsInfo(left - 10, 0, 0, 0);
                        }
                    }
                    ActiveGrid.Model.InvalidateCell(range);
                }
            }
        }

        public void ExecuteOrientationCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;

            if (!ActiveGrid.CurrentCell.HasCurrentCell)
                return;

            if (ActiveGrid != null && !ActiveGrid.Model.CurrentCellState.GridControl.CurrentCell.IsInMoveTo)
            {
                foreach (GridRangeInfo range in ActiveGrid.Model.SelectedRanges)
                {
                    for (int row = range.Top; row <= range.Bottom; row++)
                    {
                        for (int col = range.Left; col <= range.Right; col++)
                        {
                            GridStyleInfo cell = ActiveGrid.Model[row, col];
                            cell.Font.Orientation += 90;
                        }
                    }
                    ActiveGrid.Model.InvalidateCell(range);
                }
            }
        }

        public void ExecutePasteCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;
            if (ActiveGrid != null)
            {
                string CliboardText = Clipboard.GetText();
                ActiveGrid.Model.TextDataExchange.PasteTextFromBuffer(CliboardText, ActiveGrid.Model.SelectedRanges);
            }
        }

        public void ExecutePrintCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;
            ActiveGrid.Print();
        }

        public void ExecuteRedoCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;

            if (!ActiveGrid.Model.CommandStack.InTransaction)
            {
                ActiveGrid.Model.CommandStack.Redo();
            }

        }

        public void ExecuteUndoCommand(int ActiveTabIndex)
        {
            SampleGridControl ActiveGrid = ((Tab1.Items[ActiveTabIndex] as TabItem).Content as ScrollViewer).Content as SampleGridControl;

            if (!ActiveGrid.Model.CommandStack.InTransaction)
            {
                ActiveGrid.Model.CommandStack.Undo();
            }
        }

        public void Initialize()
        {
            ExcelLikeUiGridExcelMarkerMouseController controller = new ExcelLikeUiGridExcelMarkerMouseController(this.grid1);
            ExcelLikeUiGridExcelMarkerMouseController controller2 = new ExcelLikeUiGridExcelMarkerMouseController(this.grid2);
            ExcelLikeUiGridExcelMarkerMouseController controller3 = new ExcelLikeUiGridExcelMarkerMouseController(this.grid3);
            this.grid1.Model.CommandStack.Enabled = true;
            this.grid2.Model.CommandStack.Enabled = true;
            this.grid3.Model.CommandStack.Enabled = true;
            this.grid1.MouseControllerDispatcher.Add(controller);
            this.grid2.MouseControllerDispatcher.Add(controller2);
            this.grid3.MouseControllerDispatcher.Add(controller3);
            this.grid1.Model.Options.CopyPasteOption = CopyPaste.CutCell | CopyPaste.CopyCellData | CopyPaste.IncludeStyle;
            this.grid2.Model.Options.CopyPasteOption = CopyPaste.CutCell | CopyPaste.CopyCellData | CopyPaste.IncludeStyle;
            this.grid3.Model.Options.CopyPasteOption = CopyPaste.CutCell | CopyPaste.CopyCellData | CopyPaste.IncludeStyle;
            GridTextBoxBinder binder = new GridTextBoxBinder();
            binder.Wire(new GridControlBase[] { grid1, grid2, grid3 }, Formulacell);
        }
    }
}
