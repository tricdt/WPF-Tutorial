using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System.Globalization;
using System.Windows.Input;
namespace LedSign
{
    public class LedEditExcelMarkerMouseController : IMouseController
    {
        GridControlBase Grid;
        public LedEditExcelMarkerMouseController(GridControlBase grid)
        {
            this.Grid = grid;
            Grid.Model.ColumnWidths.DefaultLineSize = 25;
            Grid.Model.RowHeights.DefaultLineSize = 25;
            //Grid.ShowGridLines = false;
            grid.AllowDragDrop = false;
            IMouseController controller = grid.MouseControllerDispatcher.Find("ResizeRowsMouseController");
            grid.MouseControllerDispatcher.Remove(controller);
            controller = grid.MouseControllerDispatcher.Find("ResizeColumnsMouseController");
            grid.MouseControllerDispatcher.Remove(controller);
            grid.Model.ColumnWidths[0] = 30d;
            grid.Model.ColumnWidths[1] = 40d;
            grid.Model.Options.ShowCurrentCell = false;
            grid.Model.Options.ExcelLikeCurrentCell = false;
            GridCellNestedGridModel gridModel = new GridCellNestedGridModel(GridNestedAxisLayout.Normal, GridNestedAxisLayout.Normal);
            grid.Model.CellModels.Add("ScrollGrid", gridModel);
            grid.Model[0, 0].CellType = "ScrollGrid";

            GridModel model = new GridModel();
            model.ColumnCount = 1;
            model.RowCount = 1;
            model.TableStyle.Borders.Left = null;
            model.TableStyle.Borders.Bottom = null;
            model.ColumnWidths[0] = 95;
            model.RowHeights[0] = 25;
            model.Options.ShowCurrentCell = false;
            model.Options.ExcelLikeCurrentCell = false;

            GridStyleInfo styleInfo = new GridStyleInfo();
            styleInfo.CellType = "UpDownEdit";
            styleInfo.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            styleInfo.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            styleInfo.NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
            styleInfo.UpDownEdit.MinValue = 1;
            styleInfo.CellValue = 4;
            model.Data[0, 0] = styleInfo.Store;
            grid.Model[0, 0].CellValue = model;
            grid.CoveredCells.Add(new Syncfusion.Windows.Controls.Cells.CoveredCellInfo(0, 0, 0, 2));

            //GridStyleInfo numLedStyleInfo = grid.Model[0, 0];
            //numLedStyleInfo.CellType = "UpDownEdit";
            //numLedStyleInfo.NumberFormat = new NumberFormatInfo { NumberGroupSeparator = " ", NumberDecimalDigits = 0 };
            //numLedStyleInfo.UpDownEdit.MinValue = 1;
            //numLedStyleInfo.CellValue = 4;
            //grid.Model.TableStyle.Borders.Right = null;
            //grid.Model.TableStyle.Borders.Bottom = null;
            for (int i = 3; i < grid.Model.ColumnCount; i++)
            {
                grid.Model[0, i].CellType = "Header";
                grid.Model[0, i].CellValue = i - 2;
                grid.Model[0, i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                grid.Model[0, i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
        }

        public string Name => "LedEditExcelMarkerMouseController";

        public Cursor Cursor => throw new NotImplementedException();

        public bool SupportsCancelMouseCapture => false;

        public bool SupportsMouseTracking => false;

        public void CancelMode()
        {
        }

        public int HitTest(MouseControllerEventArgs mouseEventArgs, IMouseController controller)
        {
            return 0;
        }

        public void MouseDown(MouseControllerEventArgs e)
        {
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
        }

        public void MouseUp(MouseControllerEventArgs e)
        {
        }

        public void RestoreMode()
        {
        }
    }
    public class NumberLedCellModel : GridCellModel<NumberLedCellRenderer> { }
    public class NumberLedCellRenderer : GridCellUpDownCellRenderer
    {

    }
}
