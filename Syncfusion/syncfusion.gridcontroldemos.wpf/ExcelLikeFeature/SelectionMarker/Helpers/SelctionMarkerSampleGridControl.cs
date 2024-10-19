using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    public class SelectionMarkerSampleGridControl : GridControl
    {
        Pen boldBorder;
        double zeroWidth = .1;
        public SelectionMarkerSampleGridControl()
        {
            this.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
            Model.SelectionChanged += new GridSelectionChangedEventHandler(Model_SelectionChanged);
            this.ColumnWidths.LineSizeChanged += new RangeChangedEventHandler(ColumnWidths_LineSizeChanged);
            this.ResizingColumns += new GridResizingColumnsEventHandler(SelectionMarkerSampleGridControl_ResizingColumns);
            boldBorder = new Pen(Brushes.Black, 2);
            boldBorder.Freeze();
        }
        GridRangeInfo lastActiveRange = GridRangeInfo.Empty;
        private double lastWidth = double.MaxValue;
        private int lastColumn = int.MinValue;
        private void SelectionMarkerSampleGridControl_ResizingColumns(object sender, GridResizingColumnsEventArgs args)
        {
            lastWidth = (args.Reason == GridResizeCellsReason.MouseMove) ? args.Width : double.MaxValue;
            lastColumn = (args.Reason == GridResizeCellsReason.MouseMove) ? args.Columns.Left : int.MinValue;
            if (args.Reason == GridResizeCellsReason.MouseUp ||
                 args.Reason == GridResizeCellsReason.ResetHide ||
                args.Reason == GridResizeCellsReason.ResetDefault ||
                args.Width < zeroWidth)
            {

                if (!lastActiveRange.IsEmpty)
                {
                    this.InvalidateCell(lastActiveRange);
                    this.InvalidateVisual();
                }
            }
        }

        private void ColumnWidths_LineSizeChanged(object sender, RangeChangedEventArgs e)
        {
            double width = this.ColumnWidths[e.From];
            if (width == 0)
            {
                foreach (GridRangeInfo range in this.Model.SelectedRanges)
                {
                    GridRangeInfo intersect = range.IntersectRange(GridRangeInfo.Col(e.From));
                    for (int row = intersect.Top; row <= intersect.Bottom; row++)
                    {
                        for (int col = intersect.Left; col <= intersect.Right; col++)
                        {
                            if (col + 1 != this.Model.ColumnCount)
                            {
                                this.Model[row, col + 1].Borders.Left = new Pen(Brushes.Black, 2);
                            }
                            else
                            {
                                this.Model[row, col - 1].Borders.Right = new Pen(Brushes.Black, 2);
                            }
                        }
                    }
                }
                this.Model.InvalidateVisual(true);
            }
            else
            {
                foreach (GridRangeInfo range in this.Model.SelectedRanges)
                {
                    GridRangeInfo intersect = range.IntersectRange(GridRangeInfo.Col(e.From));
                    double w = this.Model.ColumnWidths[intersect.Left];
                    if (w == 0)
                    {
                        return;
                    }
                    for (int row = intersect.Top; row <= intersect.Bottom; row++)
                    {
                        for (int col = intersect.Left; col <= intersect.Right; col++)
                        {
                            if (col + 1 != this.Model.ColumnCount)
                            {
                                this.Model[row, col + 1].Borders.Left = null;
                            }
                            else
                            {
                                this.Model[row, col - 1].Borders.Right = null;
                            }
                        }
                    }
                }
                this.Model.InvalidateVisual(true);
            }
        }
        private GridRangeInfo oldRange { get; set; }
        private void Model_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            //invalidate last range to remove old borders
            if (oldRange != null)
            {
                for (int row = oldRange.Top; row <= oldRange.Bottom; row++)
                {
                    for (int col = oldRange.Left; col <= oldRange.Right; col++)
                    {
                        if (!this.Model.SelectedRanges.Contains(GridRangeInfo.Cell(row, col)))
                        {
                            if (col + 1 != this.Model.ColumnCount)
                            {
                                this.Model[row, col + 1].Borders.Left = null;
                            }
                            else
                            {
                                this.Model[row, col - 1].Borders.Right = null;
                            }
                        }
                    }
                }
                InvalidateCell(oldRange);
            }

            foreach (GridRangeInfo range in this.Model.SelectedRanges)
            {
                for (int row = range.Top; row <= range.Bottom; row++)
                {
                    for (int col = range.Left; col <= range.Right; col++)
                    {
                        double width = this.Model.ColumnWidths[col];

                        if (width == 0)
                        {
                            if (col + 1 != this.Model.ColumnCount)
                            {
                                this.Model[row, col + 1].Borders.Left = new Pen(Brushes.Black, 2);
                            }
                            else
                            {
                                this.Model[row, col - 1].Borders.Right = new Pen(Brushes.Black, 2);
                            }
                        }
                    }
                }
            }
            oldRange = e.Range;
            this.Model.InvalidateVisual(true);
        }
    }
}
