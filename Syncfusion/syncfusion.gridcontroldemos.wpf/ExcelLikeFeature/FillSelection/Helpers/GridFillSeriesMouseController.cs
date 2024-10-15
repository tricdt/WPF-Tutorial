using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
#if SILVERLIGHT
using System.Windows.Browser;
using Syncfusion.Windows;
#endif
namespace syncfusion.gridcontroldemos.wpf
{
    public class GridFillSeriesMouseController : IMouseController, IDisposable
    {
        #region Private Properties
        private Cursor SelectCross = null;
        protected GridControlBase grid;
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
        public SeriesType InnerFillType;
        protected Popup codePopup = new Popup();
        #endregion
        #region Public Properties
        public GridModel Gridmodel
        {
            get { return this.grid.Model; }
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
        #endregion
        #region Constructor
        public GridFillSeriesMouseController(GridControlBase grid)
        {
            this.grid = grid;
            this.Filltype = SeriesType.FillSeries | SeriesType.CopySeries | SeriesType.FillFormatOnly | SeriesType.FillWithoutFormat;
            this.InnerFillType = SeriesType.CopySeries;
            this.Gridmodel.Options.ExcelLikeCurrentCell = true;
            this.Gridmodel.Options.ExcelLikeSelectionFrame = true;
            this.Gridmodel.Options.AllowSelection = GridSelectionFlags.Any;
            this.Gridmodel.Options.ListBoxSelectionMode = GridSelectionMode.None;
            this.grid.SelectionChanged += new GridSelectionChangedEventHandler(Grid_SelectionChanged);
        }

        private void Grid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (codePopup != null)
                codePopup.IsOpen = false;
        }
        #endregion
        #region IMouseController
        public string Name => "ExcelMarkerMouseController";

        public Cursor Cursor
        {
            get
            {
                if (SelectCross == null)
                {
                    try
                    {
                        Type type = typeof(GridFillSeriesMouseController);
#if !SILVERLIGHT
                        Stream stream = type.Module.Assembly.GetManifestResourceStream("syncfusion.gridcontroldemos.wpf.Assets.GridControl.Cross.cur");
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

            GridRangeInfo range = grid.Model.SelectedCells;
            if (range == null || range.IsEmpty)
            {
                return 0;
            }

            RowColumnIndex pos = grid.PointToCellRowColumnIndex(point, true);
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
            return grid.ScrollRows.GetLineNearCorner(point.Y, HitTestPrecision);
        }

        public virtual VisibleLineInfo HitColTest(Point point)
        {
            return grid.ScrollColumns.GetLineNearCorner(point.X, HitTestPrecision);
        }

        public virtual bool IsNotNested(VisibleLineInfo dragLine)
        {
            var lineSizeCollection = this.Gridmodel.RowHeights as LineSizeCollection;
            var result = lineSizeCollection.GetNestedLines(dragLine.LineIndex) != null;
            return result;
        }


        public void MouseDown(MouseControllerEventArgs e)
        {
            //flag = 0;
            Buttonside = e.Button;
            this.IsMouseDown = true;
            MoveDir = MovingDirection.None;
            this.grid.CurrentCell.Deactivate();
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
            RowColumnIndex Currentcell = grid.PointToCellRowColumnIndex(e.Location);
            if (IsMouseDown && Currentcell != null)
            {
                if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Down) && Currentcell.RowIndex > MouseDownRange.Bottom)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(Currentcell.RowIndex, MouseDownRange.Right), Currentcell, MovingDirection.Down);
                    grid.ScrollInView(new RowColumnIndex(Math.Min(grid.Model.RowCount, Currentcell.RowIndex), Currentcell.ColumnIndex));
                }
                else if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Up) && Currentcell.RowIndex < MouseDownRange.Top)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(Currentcell.RowIndex, MouseDownRange.Right), Currentcell, MovingDirection.Up);
                    grid.ScrollInView(new RowColumnIndex(Math.Max(grid.Model.FrozenRows + 1, Currentcell.RowIndex - 1), Currentcell.ColumnIndex));
                }
                else if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Right) && Currentcell.ColumnIndex > MouseDownRange.Right)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(MouseDownRange.Top, Currentcell.ColumnIndex), Currentcell, MovingDirection.Right);
                    grid.ScrollInView(new RowColumnIndex(Currentcell.RowIndex, Math.Min(grid.Model.ColumnCount, Currentcell.ColumnIndex + 1)));
                }
                else if ((MoveDir == MovingDirection.None || MoveDir == MovingDirection.Left) && Currentcell.ColumnIndex < MouseDownRange.Left)
                {
                    this.AddSelectedRanges(GridRangeInfo.Cell(MouseDownRange.Top, Currentcell.ColumnIndex), Currentcell, MovingDirection.Left);
                    grid.ScrollInView(new RowColumnIndex(Currentcell.RowIndex, Math.Max(grid.Model.FrozenColumns + 1, Currentcell.ColumnIndex - 1)));
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
                    GridStyleInfoStore styleinfostore = (GridStyleInfoStore)this.grid.Model[Row, Column].Store.Clone();
                    //this.CellData[this.grid.Model[Row, Column].CellRowColumnIndex] = styleinfostore;
                    BackUpCellValues.Add(CellRowColumnIndex, styleinfostore);//this.CellData[this.grid.Model[Row, Column].CellRowColumnIndex]);
                }
            }
            this.InnerFillType = SeriesType.FillSeries;
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
            Rect rangerect = grid.RangeToRect(ScrollAxisRegion.Body, ScrollAxisRegion.Body, Gridmodel.SelectedCells, true, true);
            codePopup.Child = new FillDropDownItem(this);
#if SILVERLIGHT
			codePopup.HorizontalOffset = rangerect.Right + this.grid.PointFromRootVisual().X + 1;
            codePopup.VerticalOffset = rangerect.Bottom + this.grid.PointFromRootVisual().Y + 1;
#else
            codePopup.PlacementTarget = this.grid;
            codePopup.Placement = PlacementMode.Bottom;
            codePopup.VerticalOffset = 2;
            codePopup.HorizontalOffset = rangerect.Width + 1;
            codePopup.MaxWidth = 40;
            codePopup.PlacementRectangle = rangerect;
#endif
            codePopup.IsOpen = true;
        }

        private void FillData()
        {
            switch (InnerFillType)
            {
                case SeriesType.FillSeries:
                    {
                        this.CopyStyleValueBase(true, false);
                        this.FillSeries();
                        this.RefreshCells(this.SelectedRange);
                        break;
                    }
                case SeriesType.CopySeries:
                    {
                        this.CopyStyleValueBase(true, true);
                        this.RefreshCells(this.SelectedRange);
                        break;
                    }
                case SeriesType.FillFormatOnly:
                    {
                        this.CopyStyleValueBase(true, false);
                        this.RefreshCells(this.SelectedRange);
                        break;
                    }
                case SeriesType.FillWithoutFormat:
                    {
                        this.CopyStyleValueBase(false, false);
                        this.FillSeries();
                        this.RefreshCells(this.SelectedRange);
                        break;
                    }
            }
        }

        public virtual void RefreshCells(GridRangeInfo Range)
        {
            this.Gridmodel.InvalidateCell(Range);
#if SILVERLIGHT
            this.Gridmodel.InvalidateVisual(true);
#endif
        }

        private void FillSeries()
        {
            var number = 0d;
            DateTime date;
            if (Internalmovdir == MovingDirection.Down)
            {
                for (int column = CurrentBaseRange.Left; column <= CurrentBaseRange.Right; column++)
                {
                    var ValueType = "";
                    bool isTop = true; double diff = 0;
                    int format = 0;
                    for (int top = CurrentBaseRange.Top; top <= CurrentBaseRange.Bottom; top++)
                    {
                        var cellvalue = grid.Model[top, column].CellValue != null ? grid.Model[top, column].CellValue.ToString() : string.Empty;
                        if (double.TryParse(cellvalue, out number) && (ValueType == string.Empty || ValueType == "number"))
                        {
                            ValueType = "Number";
                            if (CurrentBaseRange.Height == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[top - 1, column].CellValue;
                                diff = diff + number - double.Parse(cvalue != null ? cvalue.ToString() : string.Empty);
                            }
                            isTop = false;
                        }
                        else if (DateTime.TryParse(cellvalue, out date) && (ValueType == string.Empty || ValueType == "date"))
                        {
                            ValueType = "Date";
#if !SILVERLIGHT
                            format = this.grid.Model[top, column].Text.GetDateFormat();
#endif
                            if (CurrentBaseRange.Height == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[top - 1, column].CellValue;
                                diff = diff + date.Day - DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).Day;
                            }
                            isTop = false;
                        }
                        else
                        {
                            ValueType = this.grid.Model[top, column].Text.GetType().Name;
                            break;
                        }
                    }

                    if (ValueType == "Number")
                    {
                        diff = CurrentBaseRange.Height <= 1 ? 1 : diff / (CurrentBaseRange.Height - 1);
                        FillNumber(column, diff);
                    }
                    else if (ValueType == "Date")
                    {
                        diff = CurrentBaseRange.Height <= 1 ? 1 : diff / (CurrentBaseRange.Height - 1);
                        FillDate(column, diff, format);
                    }
                    else
                    {
                        FillCopy(column);
                    }
                }
            }
            else if (Internalmovdir == MovingDirection.Right)
            {
                for (int row = CurrentBaseRange.Top; row <= CurrentBaseRange.Bottom; row++)
                {
                    var ValueType = "";
                    bool isTop = true; double diff = 0;
                    int format = 0;
                    for (int column = CurrentBaseRange.Left; column <= CurrentBaseRange.Right; column++)
                    {
                        var cellvalue = grid.Model[row, column].CellValue != null ? grid.Model[row, column].CellValue.ToString() : string.Empty;
                        if (double.TryParse(cellvalue, out number) && (ValueType == string.Empty || ValueType == "number"))
                        {
                            ValueType = "Number";
                            if (CurrentBaseRange.Width == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[row, column - 1].CellValue;
                                diff = diff + number - double.Parse(cvalue != null ? cvalue.ToString() : string.Empty);
                            }
                            isTop = false;
                        }
                        else if (DateTime.TryParse(cellvalue, out date) && (ValueType == string.Empty || ValueType == "date"))
                        {
                            ValueType = "Date";
#if !SILVERLIGHT
                            format = this.grid.Model[row, column].Text.GetDateFormat();
#endif
                            if (CurrentBaseRange.Width == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[row, column - 1].CellValue;
                                diff = diff + date.Day - DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).Day;
                            }
                            isTop = false;
                        }
                        else
                        {
                            ValueType = this.grid.Model[row, column].Text.GetType().Name;
                            break;
                        }
                    }

                    if (ValueType == "Number")
                    {
                        diff = CurrentBaseRange.Width <= 1 ? 1 : diff / (CurrentBaseRange.Width - 1);
                        FillNumber(row, diff);
                    }
                    else if (ValueType == "Date")
                    {
                        diff = CurrentBaseRange.Width <= 1 ? 1 : diff / (CurrentBaseRange.Width - 1);
                        FillDate(row, diff, format);
                    }
                    else
                    {
                        FillCopy(row);
                    }
                }
            }
            else if (Internalmovdir == MovingDirection.Up)
            {
                for (int column = CurrentBaseRange.Left; column <= CurrentBaseRange.Right; column++)
                {
                    var ValueType = "";
                    bool isTop = true; double diff = 0;
                    int format = 0;
                    for (int bottom = CurrentBaseRange.Bottom; bottom >= CurrentBaseRange.Top; bottom--)
                    {
                        var cellvalue = grid.Model[bottom, column].CellValue != null ? grid.Model[bottom, column].CellValue.ToString() : string.Empty;
                        if (double.TryParse(cellvalue, out number) && (ValueType == string.Empty || ValueType == "number"))
                        {
                            ValueType = "Number";
                            if (CurrentBaseRange.Height == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[bottom + 1, column].CellValue;
                                diff = diff + number - double.Parse(cvalue != null ? cvalue.ToString() : string.Empty);
                            }
                            isTop = false;
                        }
                        else if (DateTime.TryParse(cellvalue, out date) && (ValueType == string.Empty || ValueType == "date"))
                        {
                            ValueType = "Date";
#if !SILVERLIGHT
                            format = this.grid.Model[bottom, column].Text.GetDateFormat();
#endif
                            if (CurrentBaseRange.Height == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[bottom + 1, column].CellValue;
                                diff = diff + date.Day - DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).Day;
                            }
                            isTop = false;
                        }
                        else
                        {
                            ValueType = this.grid.Model[bottom, column].Text.GetType().Name;
                            break;
                        }
                    }

                    if (ValueType == "Number")
                    {
                        diff = CurrentBaseRange.Height <= 1 ? 1 : diff / (CurrentBaseRange.Height - 1);
                        FillNumber(column, diff);
                    }
                    else if (ValueType == "Date")
                    {
                        diff = CurrentBaseRange.Height <= 1 ? 1 : diff / (CurrentBaseRange.Height - 1);
                        FillDate(column, diff, format);
                    }
                    else
                    {
                        FillCopy(column);
                    }
                }
            }
            else if (Internalmovdir == MovingDirection.Left)
            {
                for (int row = CurrentBaseRange.Top; row <= CurrentBaseRange.Bottom; row++)
                {
                    var ValueType = "";
                    bool isTop = true; double diff = 0;
                    int format = 0;
                    for (int column = CurrentBaseRange.Right; column >= CurrentBaseRange.Left; column--)
                    {
                        var cellvalue = grid.Model[row, column].CellValue != null ? grid.Model[row, column].CellValue.ToString() : string.Empty;
                        if (double.TryParse(cellvalue, out number) && (ValueType == string.Empty || ValueType == "number"))
                        {
                            ValueType = "Number";
                            if (CurrentBaseRange.Width == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[row, column + 1].CellValue;
                                diff = diff + number - double.Parse(cvalue != null ? cvalue.ToString() : string.Empty);
                            }
                            isTop = false;
                        }
                        else if (DateTime.TryParse(cellvalue, out date) && (ValueType == string.Empty || ValueType == "date"))
                        {
                            ValueType = "Date";
#if !SILVERLIGHT
                            format = this.grid.Model[row, column].Text.GetDateFormat();
#endif
                            if (CurrentBaseRange.Width == 1)
                            {
                                diff = 1;
                                break;
                            }
                            if (!isTop)
                            {
                                var cvalue = this.grid.Model[row, column + 1].CellValue;
                                diff = diff + date.Day - DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).Day;
                            }
                            isTop = false;
                        }
                        else
                        {
                            ValueType = this.grid.Model[row, column].Text.GetType().Name;
                            break;
                        }
                    }

                    if (ValueType == "Number")
                    {
                        diff = CurrentBaseRange.Width <= 1 ? 1 : diff / (CurrentBaseRange.Width - 1);
                        FillNumber(row, diff);
                    }
                    else if (ValueType == "Date")
                    {
                        diff = CurrentBaseRange.Width <= 1 ? 1 : diff / (CurrentBaseRange.Width - 1);
                        FillDate(row, diff, format);
                    }
                    else
                    {
                        FillCopy(row);
                    }
                }
            }
        }

        private void FillNumber(int rowcolline, double diff)
        {
            var range = this.grid.Model.SelectedCells;
            if (Internalmovdir == MovingDirection.Down)
            {
                for (int row = CurrentBaseRange.Bottom + 1; row <= range.Bottom; row++)
                {
                    this.grid.Model[row, rowcolline].DisplayMember = this.grid.Model[row - 1, rowcolline].DisplayMember;
                    this.grid.Model[row, rowcolline].ValueMember = this.grid.Model[row - 1, rowcolline].ValueMember;
                    var cvalue = this.grid.Model[row - 1, rowcolline].CellValue;
                    FillCellValue(row, rowcolline, double.Parse(cvalue != null ? cvalue.ToString() : string.Empty) + diff);
                }
            }
            else if (Internalmovdir == MovingDirection.Up)
            {
                for (int row = CurrentBaseRange.Top - 1; row >= range.Top; row--)
                {
                    this.grid.Model[row, rowcolline].DisplayMember = this.grid.Model[row + 1, rowcolline].DisplayMember;
                    this.grid.Model[row, rowcolline].ValueMember = this.grid.Model[row + 1, rowcolline].ValueMember;
                    var cvalue = this.grid.Model[row + 1, rowcolline].CellValue;
                    FillCellValue(row, rowcolline, double.Parse(cvalue != null ? cvalue.ToString() : string.Empty) + diff);
                }
            }
            else if (Internalmovdir == MovingDirection.Right)
            {
                for (int column = CurrentBaseRange.Right + 1; column <= range.Right; column++)
                {
                    this.grid.Model[rowcolline, column].DisplayMember = this.grid.Model[rowcolline, column - 1].DisplayMember;
                    this.grid.Model[rowcolline, column].ValueMember = this.grid.Model[rowcolline, column - 1].ValueMember;
                    var cvalue = this.grid.Model[rowcolline, column - 1].CellValue;
                    FillCellValue(rowcolline, column, double.Parse(cvalue != null ? cvalue.ToString() : string.Empty) + diff);
                }
            }
            else if (Internalmovdir == MovingDirection.Left)
            {
                for (int column = CurrentBaseRange.Left - 1; column >= range.Left; column--)
                {
                    this.grid.Model[rowcolline, column].DisplayMember = this.grid.Model[rowcolline, column + 1].DisplayMember;
                    this.grid.Model[rowcolline, column].ValueMember = this.grid.Model[rowcolline, column + 1].ValueMember;
                    var cvalue = this.grid.Model[rowcolline, column + 1].CellValue;
                    FillCellValue(rowcolline, column, double.Parse(cvalue != null ? cvalue.ToString() : string.Empty) + diff);
                }
            }
        }

        private void FillDate(int rowcolline, double diff, int format)
        {
            var range = this.grid.Model.SelectedCells;
            if (Internalmovdir == MovingDirection.Down)
            {
                for (int row = CurrentBaseRange.Bottom + 1; row <= range.Bottom; row++)
                {
                    var cvalue = this.grid.Model[row - 1, rowcolline].CellValue;
#if SILVERLIGHT
					FillCellValue(row, rowcolline, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)));
#else
                    FillCellValue(row, rowcolline, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)).GetDateTimeFormats()[format]);
#endif   
                }
            }
            else if (Internalmovdir == MovingDirection.Up)
            {
                for (int row = CurrentBaseRange.Top - 1; row >= range.Top; row--)
                {
                    var cvalue = this.grid.Model[row + 1, rowcolline].CellValue;
#if SILVERLIGHT
                    FillCellValue(row, rowcolline, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)));
#else
                    FillCellValue(row, rowcolline, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)).GetDateTimeFormats()[format]);
#endif 
                }
            }
            else if (Internalmovdir == MovingDirection.Right)
            {
                for (int column = CurrentBaseRange.Right + 1; column <= range.Right; column++)
                {
                    var cvalue = this.grid.Model[rowcolline, column - 1].CellValue;
#if SILVERLIGHT
					FillCellValue(rowcolline, column, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)));
#else
                    FillCellValue(rowcolline, column, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)).GetDateTimeFormats()[format]);
#endif 
                }
            }
            else if (Internalmovdir == MovingDirection.Left)
            {
                for (int column = CurrentBaseRange.Left - 1; column >= range.Left; column--)
                {
                    var cvalue = this.grid.Model[rowcolline, column + 1].CellValue;
#if SILVERLIGHT
                    FillCellValue(rowcolline, column, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)));
#else
                    FillCellValue(rowcolline, column, (DateTime.Parse(cvalue != null ? cvalue.ToString() : string.Empty).AddDays(diff)).GetDateTimeFormats()[format]);
#endif
                }
            }
        }

        private void FillCopy(int rowcolline)
        {
            var range = this.grid.Model.SelectedCells;
            if (Internalmovdir == MovingDirection.Down)
            {
                for (int row = CurrentBaseRange.Bottom + 1; row <= range.Bottom; row++)
                {
                    var cellrowcolindex = new RowColumnIndex(CurrentBaseRange.Top + ((row - CurrentBaseRange.Bottom - 1) % CurrentBaseRange.Height), rowcolline);
                    this.grid.Model[row, rowcolline].DisplayMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].DisplayMember;
                    this.grid.Model[row, rowcolline].ValueMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].ValueMember;
                    CopyCellValue(row, rowcolline, cellrowcolindex);
                }
            }
            else if (Internalmovdir == MovingDirection.Up)
            {
                for (int row = CurrentBaseRange.Top - 1; row >= range.Top; row--)
                {
                    var cellrowcolindex = new RowColumnIndex(CurrentBaseRange.Bottom - ((CurrentBaseRange.Top - row + 1) % CurrentBaseRange.Height), rowcolline);
                    this.grid.Model[row, rowcolline].DisplayMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].DisplayMember;
                    this.grid.Model[row, rowcolline].ValueMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].ValueMember;
                    var cvalue = this.grid.Model[row + 1, rowcolline].CellValue;
                    CopyCellValue(row, rowcolline, cellrowcolindex);
                }
            }
            else if (Internalmovdir == MovingDirection.Right)
            {
                for (int column = CurrentBaseRange.Right + 1; column <= range.Right; column++)
                {
                    var cellrowcolindex = new RowColumnIndex(rowcolline, CurrentBaseRange.Left + ((column - CurrentBaseRange.Right - 1) % CurrentBaseRange.Width));
                    this.grid.Model[rowcolline, column].DisplayMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].DisplayMember;
                    this.grid.Model[rowcolline, column].ValueMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].ValueMember;
                    CopyCellValue(rowcolline, column, cellrowcolindex);
                }
            }
            else if (Internalmovdir == MovingDirection.Left)
            {
                for (int column = CurrentBaseRange.Left - 1; column >= range.Left; column--)
                {
                    var cellrowcolindex = new RowColumnIndex(rowcolline, CurrentBaseRange.Right - ((CurrentBaseRange.Left - column + 1) % CurrentBaseRange.Width));
                    this.grid.Model[rowcolline, column].DisplayMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].DisplayMember;
                    this.grid.Model[rowcolline, column].ValueMember = this.grid.Model[cellrowcolindex.RowIndex, cellrowcolindex.ColumnIndex].ValueMember;
                    CopyCellValue(rowcolline, column, cellrowcolindex);
                }
            }
        }

        public virtual void CopyStyleValueBase(bool CanCopyStyle, bool CanCopyValues)
        {
            if (Internalmovdir == MovingDirection.Down)
            {
                for (int row = CurrentBaseRange.Bottom + 1; row <= this.SelectedRange.Bottom; row++)
                {
                    for (int column = CurrentBaseRange.Left; column <= CurrentBaseRange.Right; column++)
                    {
                        RowColumnIndex cellrowcolindex;
                        if (CanCopyStyle)
                        {
                            cellrowcolindex = new RowColumnIndex(CurrentBaseRange.Top + ((row - CurrentBaseRange.Bottom - 1) % CurrentBaseRange.Height), column);
                            this.Gridmodel[row, column] = new GridStyleInfo(this.BackUpCellValues[cellrowcolindex]);
                        }
                        else
                            this.Gridmodel[row, column] = new GridStyleInfo(this.BackUpCellValues[new RowColumnIndex(row, column)]);

                        if (CanCopyValues)
                            cellrowcolindex = new RowColumnIndex(CurrentBaseRange.Top + ((row - CurrentBaseRange.Bottom - 1) % CurrentBaseRange.Height), column);
                        else
                            cellrowcolindex = new RowColumnIndex(row, column);

                        CopyCellValue(row, column, cellrowcolindex);
                    }
                }
            }
            else if (Internalmovdir == MovingDirection.Up)
            {
                for (int row = CurrentBaseRange.Top - 1; row >= this.SelectedRange.Top; row--)
                {
                    for (int column = CurrentBaseRange.Left; column <= CurrentBaseRange.Right; column++)
                    {
                        RowColumnIndex cellrowcolindex;
                        if (CanCopyStyle)
                        {
                            cellrowcolindex = new RowColumnIndex(CurrentBaseRange.Bottom - ((CurrentBaseRange.Top - row + 1) % CurrentBaseRange.Height), column);
                            this.Gridmodel[row, column] = new GridStyleInfo(this.BackUpCellValues[cellrowcolindex]);
                        }

                        if (CanCopyValues)
                            cellrowcolindex = new RowColumnIndex(CurrentBaseRange.Bottom - ((CurrentBaseRange.Top - row + 1) % CurrentBaseRange.Height), column);
                        else
                            cellrowcolindex = new RowColumnIndex(row, column);
                        CopyCellValue(row, column, cellrowcolindex);
                    }
                }
            }
            else if (Internalmovdir == MovingDirection.Right)
            {
                for (int column = CurrentBaseRange.Right + 1; column <= this.SelectedRange.Right; column++)
                {
                    for (int row = CurrentBaseRange.Top; row <= CurrentBaseRange.Bottom; row++)
                    {
                        RowColumnIndex cellrowcolindex;
                        if (CanCopyStyle)
                        {
                            cellrowcolindex = new RowColumnIndex(row, CurrentBaseRange.Left + ((column - CurrentBaseRange.Right - 1) % CurrentBaseRange.Width));
                            this.Gridmodel[row, column] = new GridStyleInfo(this.BackUpCellValues[cellrowcolindex]);
                        }

                        if (CanCopyValues)
                            cellrowcolindex = new RowColumnIndex(row, CurrentBaseRange.Left + ((column - CurrentBaseRange.Right - 1) % CurrentBaseRange.Width));
                        else
                            cellrowcolindex = new RowColumnIndex(row, column);
                        CopyCellValue(row, column, cellrowcolindex);
                    }
                }
            }
            else if (Internalmovdir == MovingDirection.Left)
            {
                for (int column = CurrentBaseRange.Left - 1; column >= this.SelectedRange.Left; column--)
                {
                    for (int row = CurrentBaseRange.Top; row <= CurrentBaseRange.Bottom; row++)
                    {
                        RowColumnIndex cellrowcolindex;
                        if (CanCopyStyle)
                        {
                            cellrowcolindex = new RowColumnIndex(row, CurrentBaseRange.Right - ((CurrentBaseRange.Left - column + 1) % CurrentBaseRange.Width));
                            CopyCellStyle(row, column, cellrowcolindex);
                        }

                        if (CanCopyValues)
                            cellrowcolindex = new RowColumnIndex(row, CurrentBaseRange.Right - ((CurrentBaseRange.Left - column + 1) % CurrentBaseRange.Width));
                        else
                            cellrowcolindex = new RowColumnIndex(row, column);
                        CopyCellValue(row, column, cellrowcolindex);
                    }
                }
            }
        }
        public virtual void CopyCellStyle(int row, int col, RowColumnIndex backrowcolIndex)
        {
            this.Gridmodel[row, col] = new GridStyleInfo(this.BackUpCellValues[backrowcolIndex]);
        }

        public virtual void CopyCellValue(int row, int col, RowColumnIndex backrowcolIndex)
        {
            this.Gridmodel[row, col].CellValue = new GridStyleInfo(this.BackUpCellValues[backrowcolIndex]).CellValue;
        }

        public virtual void FillCellValue(int row, int col, object value)
        {
            this.Gridmodel[row, col].CellValue = value;
        }
        public void RestoreMode()
        {
        }
        #endregion
        public void FillOptionChanged(string InnerFillType)
        {
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
        #region IDispose
        public void Dispose()
        {
            codePopup = null;
            grid.SelectionChanged -= new GridSelectionChangedEventHandler(Grid_SelectionChanged);
        }
        #endregion
    }
}
