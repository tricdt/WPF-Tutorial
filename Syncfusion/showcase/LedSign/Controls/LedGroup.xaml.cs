using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Windows.Controls;
using System.Windows.Media;

namespace LedSign
{
    /// <summary>
    /// Interaction logic for LedSign.xaml
    /// </summary>
    public partial class LedGroup : UserControl
    {
        public LedGroup()
        {
            InitializeComponent();
            InitGridLed();
        }
        Dictionary<RowColumnIndex, object> committedValues = new Dictionary<RowColumnIndex, object>();

        private void InitGridLed()
        {
            grid.Model.RowCount = 100;
            grid.Model.ColumnCount = 50;
            //grid.Model.CellModels.Add("CustomIntegerEdit", new CustomIntegerEditCellModel());
            grid.Model.ColumnWidths.DefaultLineSize = 25;
            grid.Model.RowHeights.DefaultLineSize = 25;
            grid.AllowDragDrop = false;
            //IMouseController controller = grid.MouseControllerDispatcher.Find("ResizeRowsMouseController");
            //grid.MouseControllerDispatcher.Remove(controller);
            //controller = grid.MouseControllerDispatcher.Find("ResizeColumnsMouseController");
            //grid.MouseControllerDispatcher.Remove(controller);
            grid.Model.ColumnWidths[0] = 30d;
            grid.Model.ColumnWidths[1] = 40d;
            for (int i = 3; i < grid.Model.ColumnCount; i++)
            {
                grid.Model[0, i].CellType = "Header";
                grid.Model[0, i].CellValue = i - 2;
                grid.Model[0, i].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                grid.Model[0, i].VerticalAlignment = System.Windows.VerticalAlignment.Center;
            }
            for (int i = 1; i < grid.Model.RowCount; i++)
            {
                for (int j = 1; j < grid.Model.ColumnCount; j++)
                {
                    if (j == 1)
                    {
                        grid.Model[i, j].CellValue = 200;
                    }
                    else if (j == 2)
                    {
                        GridStyleInfo style = grid.Model[i, j];
                        //style.CellType = "CustomIntegerEdit";
                        style.CellValue = 1;
                        style.CellType = "IntegerEdit";
                        style.IntegerEdit.MaxValue = 99;
                        style.IntegerEdit.MinValue = 0;
                        grid.Model[i, j].IsEditable = true;
                        style.CellItemTemplateKey = "TextTemplate";
                    }
                    else
                    {
                        GridStyleInfo style = grid.Model[i, j];
                        //style.CellType = "CustomIntegerEdit";
                        style.CellValue = 0;
                        style.CellType = "IntegerEdit";
                        style.IntegerEdit.MaxValue = 15;
                        style.IntegerEdit.MinValue = 0;
                        grid.Model[i, j].IsEditable = true;
                    }
                    grid.Model[i, j].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    grid.Model[i, j].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                }
            }
            grid.PrepareRenderCell += OnGridPrepareRenderCell;
            grid.QueryCellInfo += OnGridQueryCellInfo;
            grid.CommitCellInfo += OnGridCommitCellInfo;
        }

        private void OnGridCommitCellInfo(object sender, GridCommitCellInfoEventArgs e)
        {
            //if (e.Style.HasCellValue)
            //{
            //    committedValues[e.Cell] = e.Style.CellValue;
            //    e.Handled = true;
            //}
        }

        private void OnGridQueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {

        }

        private void OnGridPrepareRenderCell(object sender, GridPrepareRenderCellEventArgs e)
        {
            if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex > 2)
            {
                e.Style.Background = Brushes.BlueViolet;
                e.Style.Foreground = Brushes.WhiteSmoke;
            }
            if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 2)
            {
                if (e.Style.CellValue != null && e.Style.CellValue.ToString() != "")
                {
                    Int16 pwm16 = Convert.ToInt16(e.Style.CellValue.ToString());
                    Byte pwm = Convert.ToByte(pwm16 * 16);
                    if (e.Cell.ColumnIndex > 2 && e.Cell.RowIndex > 0)
                    {
                        e.Style.Background = new SolidColorBrush(Color.FromArgb(255, pwm, 0, 0));
                        e.Style.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                    }
                }
            }
        }


    }
}
