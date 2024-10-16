using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.ComponentModel;
using System.Windows.Input;
namespace syncfusion.gridcontroldemos.wpf
{
    public class GridControlSortHelper
    {
        GridControl grid;
        int sortedColumn = -1;
        ListSortDirection sortDirection = ListSortDirection.Ascending;
        public GridControlSortHelper(GridControl grid)
        {
            this.grid = grid;
            this.grid.PreviewMouseDown += new MouseButtonEventHandler(Grid_PreviewMouseDown);
            grid.Model.CellModels.Add("SortHeader", new GridCellModel<GridCellSortHeaderRenderer>());
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            GridControl gridControl = sender as GridControl;
            if (gridControl.CurrentCell.IsEditing)
            {
                gridControl.CurrentCell.Deactivate();
            }
            RowColumnIndex cell = grid.PointToCellRowColumnIndex(e.GetPosition(grid));
            if (cell.RowIndex == 0 && cell.ColumnIndex > 0)
            {
                if (cell.ColumnIndex == sortedColumn)
                {
                    sortDirection = sortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                }
                else
                {
                    sortDirection = ListSortDirection.Ascending;
                }
                sortedColumn = cell.ColumnIndex;
                gridControl.SortColumn(sortedColumn, sortDirection);
                e.Handled = true;
            }
        }
    }

    public static class ExtensionClass
    {
        public static int sortedColumn;
        static ListSortDirection sortDir = ListSortDirection.Ascending;
        class SortKey
        {
            public IComparable Value { get; set; }
            public int RowIndex { get; set; }
        }
        public static void SortColumn(this GridControl gridControl, int sortedColumnIndex, ListSortDirection sortDirection)
        {
            sortedColumn = sortedColumnIndex;
            sortDir = sortDirection;
            InitSortKeys(gridControl, sortedColumnIndex, sortDirection);
            gridControl.QueryCellInfo += new GridQueryCellInfoEventHandler(GridControl_QueryCellInfo);
        }

        private static void GridControl_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            GridControl grid = sender as GridControl;
            if (sortedColumn > -1 && e.Cell.RowIndex > 0)
            {
                int row = sortKeys[e.Cell.RowIndex - 1].RowIndex;
                if (grid.Model.Data[row, e.Cell.ColumnIndex] != null)
                {
                    e.Style.CellValue = grid.Model.Data[row, e.Cell.ColumnIndex].GetValue(GridStyleInfoStore.CellValueProperty);
                }
                else
                {
                    e.Style.CellValue = "";
                }
            }
            else
                if (sortedColumn == e.Cell.ColumnIndex && e.Cell.RowIndex == 0)
            {
                e.Style.CellType = "SortHeader";
                e.Style.Tag = sortDir;
            }
        }

        private static List<SortKey> sortKeys;

        private static void InitSortKeys(GridControl grid, int sortedColumnIndex, ListSortDirection sortDirection)
        {
            sortKeys = new List<SortKey>();
            sortKeys.Clear();
            for (int i = 1; i < grid.Model.RowCount; ++i)
            {
                GridStyleInfoStore o = grid.Model.Data[i, sortedColumnIndex];
                sortKeys.Add(new SortKey
                {
                    RowIndex = i,
                    Value = (o == null) ? null : o.GetValue(GridStyleInfoStore.CellValueProperty) as IComparable
                });
            }

            sortKeys.Sort(new SortComparer(sortDirection));
            grid.InvalidateCells();
            grid.InvalidateVisual();
        }
        class SortComparer : IComparer<SortKey>
        {
            ListSortDirection sd;
            public SortComparer(ListSortDirection sd)
            {
                this.sd = sd;
            }
            public int Compare(SortKey? x, SortKey? y)
            {
                int c = 0;
                if (x == null && y == null)
                    c = 0;
                else if (x == null)
                    c = -1;
                else if (y == null)
                    c = 1;
                else
                {
                    if (x.Value == null && y.Value == null)
                    {
                        c = 0;
                    }
                    else if (x.Value == null)
                    {
                        c = -1;
                    }
                    else if (y.Value == null)
                    {
                        c = 1;
                    }

                    else if (!(x.Value is int && y.Value is int))
                    {
                        c = 0;
                    }
                    else if (x.Value.GetType() != y.Value.GetType())
                    {
                        IComparable o = Convert.ChangeType(y.Value, x.Value.GetType()) as IComparable;
                        if (o != null)
                        {
                            c = x.Value.CompareTo(o);
                        }
                        else
                        {
                            o = Convert.ChangeType(x.Value, y.Value.GetType()) as IComparable;
                            c = o.CompareTo(y.Value);
                        }
                    }
                    else
                    {
                        c = x.Value.CompareTo(y.Value);
                    }
                }
                if (c == 0)
                {
                    c = x.GetHashCode().CompareTo(y.GetHashCode());
                }
                if (sd == ListSortDirection.Descending)
                    c = -c;

                return c;
            }
        }
    }
}
