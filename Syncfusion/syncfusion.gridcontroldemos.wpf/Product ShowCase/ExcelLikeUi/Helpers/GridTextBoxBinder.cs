using Syncfusion.Windows.ComponentModel;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace syncfusion.gridcontroldemos.wpf
{
    public class GridTextBoxBinder : IDisposable
    {
        public GridTextBoxBinder()
        {

        }
        private GridControlBase lastGridWithFocus = null;
        private TextBox formulaBox = null;
        public TextBox FormulaBox
        {
            get { return formulaBox; }
        }
        private List<GridControlBase> grids;

        public List<GridControlBase> Grids
        {
            get
            {
                if (grids == null)
                {
                    grids = new List<GridControlBase>();
                }
                return grids;
            }
        }

        public void Wire(GridControlBase grid, TextBox tb)
        {
            UnWire();
            Hook(grid);
            HookTextBox(tb);
        }
        public void Wire(IEnumerable<GridControlBase> grids, TextBox tb)
        {
            UnWire();
            foreach (GridControlBase grid in grids)
            {
                Hook(grid);
            }
            HookTextBox(tb);
        }

        private void HookTextBox(TextBox tb)
        {
            this.formulaBox = tb;
            this.formulaBox.AcceptsReturn = false;
            tb.GotFocus += new RoutedEventHandler(tb_GotFocus);
            tb.LostFocus += new RoutedEventHandler(tb_LostFocus);
            tb.TextChanged += new TextChangedEventHandler(tb_TextChanged);
            tb.PreviewKeyDown += new KeyEventHandler(tb_PreviewKeyDown);
        }

        private void tb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                e.Handled = true;
                lastGridWithFocus.CurrentCell.MoveDown();
            }
        }

        private bool inTextChanged = false;
        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (inCurrentCellChanged || inCurrentCellMoved)
                return;

            inTextChanged = true;

            GridControlBase grid = lastGridWithFocus;
            if (grid != null)
            {
                GridCurrentCell cc = grid.CurrentCell;

                if (grid.CurrentCell.HasCurrentCell)
                    cc.Renderer.SetControlText(FormulaBox.Text, false);
            }

            inTextChanged = false;
        }

        private void tb_LostFocus(object sender, RoutedEventArgs e)
        {
            GridControlBase grid = lastGridWithFocus;
            if (grid != null)
            {
                GridCurrentCell cc = grid.CurrentCell;
                cc.EndEdit();
            }
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            GridControlBase grid = lastGridWithFocus;
            if (grid != null)
            {
                GridCurrentCell cc = grid.CurrentCell;
                cc.BeginEdit();
            }
        }

        private void Hook(GridControlBase grid)
        {
            if (!Grids.Contains(grid))
            {
                Grids.Add(grid);
                grid.CurrentCellChanged += new GridRoutedEventHandler(grid_CurrentCellChanged);
                grid.CurrentCellMoved += new GridCurrentCellMovedEventHandler(grid_CurrentCellMoved);
                grid.IsVisibleChanged += new DependencyPropertyChangedEventHandler(grid_IsVisibleChanged);
            }
        }

        private void grid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            GridControlBase grid = sender as GridControlBase;
            if (grid != null)
            {
                lastGridWithFocus = grid;
                GridCurrentCell cc = grid.CurrentCell;
                FormulaBox.Text = grid.Model[cc.CellRowColumnIndex.RowIndex, cc.CellRowColumnIndex.ColumnIndex].Text;
            }
        }

        private bool inCurrentCellMoved = false;
        private void grid_CurrentCellMoved(object sender, GridCurrentCellMovedEventArgs args)
        {
            inCurrentCellMoved = true;
            GridControlBase grid = sender as GridControlBase;
            if (grid != null)
            {
                lastGridWithFocus = grid;
                GridCurrentCell cc = grid.CurrentCell;
                FormulaBox.Text = grid.Model[cc.CellRowColumnIndex.RowIndex, cc.CellRowColumnIndex.ColumnIndex].Text;
            }
            inCurrentCellMoved = false;
        }

        private bool inCurrentCellChanged = false;
        private void grid_CurrentCellChanged(object sender, SyncfusionRoutedEventArgs args)
        {
            if (inTextChanged || inCurrentCellMoved)
                return;
            inCurrentCellChanged = true;
            GridControlBase grid = args.Source as GridControlBase;
            if (grid != null)
            {
                lastGridWithFocus = grid;
                GridCurrentCell cc = grid.CurrentCell;
                FormulaBox.Text = cc.Renderer.ControlText;
            }
            inCurrentCellChanged = false;
        }

        private void UnWire()
        {
            GridControlBase[] allGrids = new GridControlBase[Grids.Count];
            allGrids = Grids.ToArray();
            foreach (GridControlBase grid in allGrids)
            {
                Unhook(grid);
            }
            UnhookTextBox();
        }

        private void UnhookTextBox()
        {
            if (FormulaBox != null)
            {
                FormulaBox.GotFocus += new RoutedEventHandler(tb_GotFocus);
                FormulaBox.LostFocus += new RoutedEventHandler(tb_LostFocus);
                FormulaBox.TextChanged += new TextChangedEventHandler(tb_TextChanged);
                FormulaBox.PreviewKeyDown += new KeyEventHandler(tb_PreviewKeyDown);
            }
        }

        private void Unhook(GridControlBase grid)
        {
            if (Grids.Contains(grid))
            {
                Grids.Remove(grid);
                grid.CurrentCellChanged -= new GridRoutedEventHandler(grid_CurrentCellChanged);
                grid.CurrentCellMoved -= new GridCurrentCellMovedEventHandler(grid_CurrentCellMoved);
                grid.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(grid_IsVisibleChanged);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
