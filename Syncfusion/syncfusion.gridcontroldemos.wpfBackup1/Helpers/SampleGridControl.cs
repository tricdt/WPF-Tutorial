using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    public class SampleGridControl : GridControl
    {
        public SampleGridControl()
        {
            RowHeights.DefaultLineSize = 20;
            RowHeights.LineCount = 65535;
            ColumnWidths.DefaultLineSize = 65;
            ColumnWidths.LineCount = 100;
            Model.TableStyle.CellType = "FormulaCell";
            Model.TableStyle.Borders = new CellBordersInfo()
            {
                All = new Pen(Brushes.Gray, 0.15d)
            };
            Model.ColumnWidths[0] = 35d;
            Model.Options.ExcelLikeSelectionFrame = true;
            Model.Options.ExcelLikeCurrentCell = true;
            Model.TableStyle.VerticalAlignment = VerticalAlignment.Bottom;
            // Model.TableStyle.BorderMargins.Bottom = 2;
            Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
        }
        protected override void OnQueryCellInfo(GridQueryCellInfoEventArgs e)
        {
            base.OnQueryCellInfo(e);
            if (e.Style.RowIndex == 0 && e.Style.ColumnIndex == 0)
            {
                e.Style.Background = new SolidColorBrush(Colors.White);
                return;
            }
            else if (e.Style.RowIndex == 0)
            {
                e.Style.CellValue = GridRangeInfo.GetAlphaLabel(e.Cell.ColumnIndex);
                e.Style.Background = new SolidColorBrush(Colors.White);
                e.Style.Foreground = Brushes.Black;
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
                e.Style.VerticalAlignment = VerticalAlignment.Center;

            }
            else if (e.Style.ColumnIndex == 0)
            {
                e.Style.CellValue = e.Style.RowIndex;
                e.Style.Background = new SolidColorBrush(Colors.White);
                e.Style.Foreground = Brushes.Black;
                e.Style.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
        protected override void OnSelectionChanged(GridSelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (e.Reason == GridSelectionReason.MouseDown || e.Reason == GridSelectionReason.SetCurrentCell || e.Reason == GridSelectionReason.MouseMove || e.Reason == GridSelectionReason.SelectRange || e.Reason == GridSelectionReason.MouseUp)
            {
                this.InvalidateCell(GridRangeInfo.Row(0));
                this.InvalidateCell(GridRangeInfo.Col(0));
                GridRangeInfo range = e.Range;
                if (e.Range.IsCols)
                {
                    range = GetExpandedRange(range);
                }
                if (e.Range.IsRows)
                {
                    range = GetExpandedRange(range);
                }
                if (e.Range.IsTable)
                {
                    range = GetExpandedRange(range);
                }
                if (e.Range == null || e.Range.IsEmpty)
                {
                    CellLocationText = "";
                }
                else if ((e.Range.Height == 1 && e.Range.Width == 1) || e.Reason == GridSelectionReason.MouseUp)
                {
                    CellLocationText = string.Format("{0}{1}", GridRangeInfo.GetAlphaLabel(range.Left), range.Top);
                }
                else
                {
                    CellLocationText = string.Format("{0}R x {1}C", range.Height, range.Width);
                }
            }
        }
        protected override void OnPrepareRenderCell(GridPrepareRenderCellEventArgs e)
        {
            base.OnPrepareRenderCell(e);
            if (e.Cell.RowIndex == 0 && Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Col(e.Cell.ColumnIndex)))
            {
                e.Style.Background = Brushes.Red;
                e.Style.Font.FontWeight = FontWeights.Bold;
            }
            else if (e.Cell.ColumnIndex == 0 && Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Row(e.Cell.RowIndex)))
            {
                e.Style.Background = Brushes.LightGray;
                e.Style.Font.FontWeight = FontWeights.Bold;
            }
        }
        private GridRangeInfo GetExpandedRange(GridRangeInfo range)
        {

            if (range.IsCols)
            {
                range = range.ExpandRange(range.Top + 1, range.Left, Model.RowCount, range.Left);
            }

            if (range.IsRows)
            {
                range = range.ExpandRange(range.Top, range.Left + 1, range.Top, Model.ColumnCount);
            }

            if (range.IsTable)
            {
                range = range.ExpandRange(range.Top + 1, range.Left + 1, Model.RowCount, Model.ColumnCount);
            }
            return range;
        }
        private string cellLocationText;

        public event EventHandler CellLocationTextChanged;
        public string CellLocationText
        {
            get { return cellLocationText; }
            set
            {
                cellLocationText = value;
                if (CellLocationTextChanged != null)
                    CellLocationTextChanged(this, EventArgs.Empty);
            }
        }
    }
}
