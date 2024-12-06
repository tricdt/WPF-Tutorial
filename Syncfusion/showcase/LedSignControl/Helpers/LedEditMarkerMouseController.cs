using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LedSignControl
{
    public class LedEditMarkerMouseController : IMouseController
    {
        protected GridControlBase Grid;
        private Cursor SelectCross = null;
        protected MovingDirection MoveDir = MovingDirection.None;
        private double HitTestPrecision = 4;
#if SILVERLIGHT
        private MouseButtons? Buttonside;
#else
        private MouseButton? Buttonside;
#endif
        private bool IsMouseDown = false;
        protected GridRangeInfo CurrentBaseRange = null;
        protected GridRangeInfo MouseDownRange = GridRangeInfo.Empty;
        private MovingDirection Internalmovdir = MovingDirection.None;
        protected Dictionary<RowColumnIndex, GridStyleInfoStore> BackUpCellValues = new Dictionary<RowColumnIndex, GridStyleInfoStore>();
        protected GridCellData CellData;
        protected Popup codePopup = new Popup();
        public SeriesType InnerFillType;
        public GridModel Gridmodel
        {
            get { return this.Grid.Model; }
        }

        public GridRangeInfo SelectedRange
        {
            get { return this.Gridmodel.SelectedCells; }
        }

        private SeriesType _Filltype;
        public SeriesType Filltype
        {
            get { return _Filltype; }
            set { _Filltype = value; }
        }

        public LedEditMarkerMouseController(GridControlBase grid)
        {
            this.Grid = grid;
            this.Filltype = SeriesType.FillSeries | SeriesType.CopySeries | SeriesType.FillFormatOnly | SeriesType.FillWithoutFormat;
            this.InnerFillType = SeriesType.CopySeries;
            Grid.Model.Options.ExcelLikeSelectionFrame = true;
            Grid.Model.Options.ExcelLikeCurrentCell = true;
        }
        public string Name => "LedEditMarkerMouseController";

        public Cursor Cursor
        {
            get
            {
                if (SelectCross == null)
                {
                    try
                    {
                        Type type = typeof(LedEditMarkerMouseController);
#if !SILVERLIGHT
                        Stream stream = type.Module.Assembly.GetManifestResourceStream("LedSignControl.Helpers.Cross.cur");
                        SelectCross = new Cursor(stream);
#else
                        SelectCross = Cursors.Stylus;
#endif
                    }
                    catch (System.Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                        throw exception;
                    }
                }
                return SelectCross;
            }
        }

        public bool SupportsCancelMouseCapture => false;

        public bool SupportsMouseTracking => false;

        public void CancelMode()
        {
            throw new NotImplementedException();
        }

        public int HitTest(MouseControllerEventArgs mouseEventArgs, IMouseController controller)
        {
            Point point = mouseEventArgs.Location;

            GridRangeInfo range = Grid.Model.SelectedCells;
            if (range == null || range.IsEmpty)
            {
                return 0;
            }

            RowColumnIndex pos = Grid.PointToCellRowColumnIndex(point, true);
            if (pos != RowColumnIndex.Empty && MoveDir != MovingDirection.None)
            {
                return 1;
            }

            VisibleLineInfo hitRow = HitRowTest(point);
            if (hitRow != null && !this.IsNotNested(hitRow) && range.Bottom == hitRow.LineIndex)
            {
                VisibleLineInfo column = HitColTest(point);
                if (column != null && range.Right == column.LineIndex)
                {
                    return 1;
                }
            }

            return 0;
        }

        public virtual VisibleLineInfo HitRowTest(Point point)
        {
            return Grid.ScrollRows.GetLineNearCorner(point.Y, HitTestPrecision);
        }

        public virtual VisibleLineInfo HitColTest(Point point)
        {
            return Grid.ScrollColumns.GetLineNearCorner(point.X, HitTestPrecision);
        }

        public virtual bool IsNotNested(VisibleLineInfo dragLine)
        {
            var lineSizeCollection = this.Gridmodel.RowHeights as LineSizeCollection;
            var result = lineSizeCollection.GetNestedLines(dragLine.LineIndex) != null;
            return result;
        }

        int flag = 0;
        public void MouseDown(MouseControllerEventArgs e)
        {
            flag = 0;
            Buttonside = e.Button;
            this.IsMouseDown = true;
            MoveDir = MovingDirection.None;
            this.Grid.CurrentCell.Deactivate();
            this.CurrentBaseRange = this.MouseDownRange = this.Gridmodel.SelectedCells;
        }

        public void MouseHover(MouseControllerEventArgs e)
        {
        }

        public void MouseHoverEnter(MouseEventArgs e)
        {
        }

        public void MouseHoverLeave(MouseEventArgs e)
        {
        }

        public void MouseMove(MouseControllerEventArgs e)
        {
            RowColumnIndex Currentcell = Grid.PointToCellRowColumnIndex(e.Location);
            if (IsMouseDown && Currentcell != null)
            {
                if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Down) && Currentcell.RowIndex > MouseDownRange.Bottom)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(Currentcell.RowIndex, MouseDownRange.Right), Currentcell, MovingDirection.Down);
                    Grid.ScrollInView(new RowColumnIndex(Math.Min(Grid.Model.RowCount, Currentcell.RowIndex), Currentcell.ColumnIndex));
                }
                else if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Up) && Currentcell.RowIndex < MouseDownRange.Top)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(Currentcell.RowIndex, MouseDownRange.Right), Currentcell, MovingDirection.Up);
                    Grid.ScrollInView(new RowColumnIndex(Math.Max(Grid.Model.FrozenRows + 1, Currentcell.RowIndex - 1), Currentcell.ColumnIndex));
                }
                else if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Right) && Currentcell.ColumnIndex > MouseDownRange.Right)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(MouseDownRange.Top, Currentcell.ColumnIndex), Currentcell, MovingDirection.Right);
                    Grid.ScrollInView(new RowColumnIndex(Currentcell.RowIndex, Math.Min(Grid.Model.ColumnCount, Currentcell.ColumnIndex + 1)));
                }
                else if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Left) && Currentcell.ColumnIndex < MouseDownRange.Left)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(MouseDownRange.Top, Currentcell.ColumnIndex), Currentcell, MovingDirection.Left);
                    Grid.ScrollInView(new RowColumnIndex(Currentcell.RowIndex, Math.Max(Grid.Model.FrozenColumns + 1, Currentcell.ColumnIndex - 1)));
                }
                else
                {
                    if (MouseDownRange.Contains(GridRangeInfo.Cell(Currentcell.RowIndex, Currentcell.ColumnIndex)))
                    {
                        MoveDir = MovingDirection.None;
                        Gridmodel.Selections.Clear();
                        Gridmodel.Selections.Add(MouseDownRange);
                    }
                }
            }
        }

        public virtual void AddSelectedRanges(GridRangeInfo NewRange, RowColumnIndex Currentcell, MovingDirection Direction)
        {
            MoveDir = Direction;
            Gridmodel.Selections.Clear();
            Gridmodel.Selections.Add(MouseDownRange.UnionRange(NewRange));
        }

        public void MouseUp(MouseControllerEventArgs e)
        {
            Internalmovdir = MoveDir;
            BackUpCellValues.Clear();
            CellData = new GridCellData();
            for (int Row = Gridmodel.SelectedCells.Top; Row <= Gridmodel.SelectedCells.Bottom; Row++)
            {
                for (int Column = this.SelectedRange.Left; Column <= this.SelectedRange.Right; Column++)
                {
                    var CellRowColumnIndex = new RowColumnIndex(Row, Column);
                    GridStyleInfoStore styleinfostore = (GridStyleInfoStore)this.Grid.Model[Row, Column].Store.Clone();
                    //this.CellData[this.grid.Model[Row, Column].CellRowColumnIndex] = styleinfostore;
                    BackUpCellValues.Add(CellRowColumnIndex, styleinfostore);//this.CellData[this.grid.Model[Row, Column].CellRowColumnIndex]);
                }
            }
            //this.InnerFillType = SeriesType.FillSeries;
            this.FillData();
            MoveDir = MovingDirection.None;
            IsMouseDown = false;

#if !SILVERLIGHT
            if (Buttonside == MouseButton.Left)
#else
                        if (Buttonside == MouseButtons.Left)
#endif
            {
                OpenFilterDropDown();
            }
        }
        private void OpenFilterDropDown()
        {
            Rect rangerect = Grid.RangeToRect(ScrollAxisRegion.Body, ScrollAxisRegion.Body, Gridmodel.SelectedCells, true, true);
            codePopup.Child = new FillDropdownItem(this);
#if SILVERLIGHT
			codePopup.HorizontalOffset = rangerect.Right + this.grid.PointFromRootVisual().X + 1;
            codePopup.VerticalOffset = rangerect.Bottom + this.grid.PointFromRootVisual().Y + 1;
#else
            codePopup.PlacementTarget = this.Grid;
            codePopup.Placement = PlacementMode.Bottom;
            codePopup.VerticalOffset = 2;
            codePopup.HorizontalOffset = rangerect.Width + 1;
            //codePopup.MaxWidth = 40;
            codePopup.PlacementRectangle = rangerect;
#endif
            codePopup.IsOpen = true;
        }


        public void FillOptionChanged(string InnerFillType)
        {
            codePopup.IsOpen = false;
            if (InnerFillType == "FillSeries")
                this.InnerFillType = SeriesType.FillSeries;
            else if (InnerFillType == "CopySeries")
                this.InnerFillType = SeriesType.CopySeries;
            else if (InnerFillType == "FillFormatOnly")
                this.InnerFillType = SeriesType.FillFormatOnly;
            else if (InnerFillType == "FillWithoutFormat")
                this.InnerFillType = SeriesType.FillWithoutFormat;

            this.FillData();
        }
        private void FillData()
        {

        }

        public void RestoreMode()
        {
        }
    }
    public enum MovingDirection
    {
        None,
        Left,
        Down,
        Right,
        Up
    }
    public enum SeriesType
    {
        CopySeries = 4,
        FillSeries = 6,
        FillFormatOnly = 8,
        FillWithoutFormat = 10
    }
}
