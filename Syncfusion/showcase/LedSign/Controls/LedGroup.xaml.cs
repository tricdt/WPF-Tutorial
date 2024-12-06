using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
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
            LedGroupSetting();
            SfSkinManager.SetTheme(grid, new Theme("Windows11Light"));
            //InitGridLed();
        }

        private void LedGroupSetting()
        {
            grid.Model.RowCount = 2000;
            grid.Model.ColumnCount = 80;

            LedEditExcelMarkerMouseController ledSigncontroller = new LedEditExcelMarkerMouseController(grid);
            grid.MouseControllerDispatcher.Add(ledSigncontroller);
        }

        Dictionary<RowColumnIndex, object> committedValues = new Dictionary<RowColumnIndex, object>();

        private void InitGridLed()
        {
            grid.Model.RowCount = 2000;
            grid.Model.ColumnCount = 80;
            grid.Model.ColumnWidths.DefaultLineSize = 25;
            grid.Model.RowHeights.DefaultLineSize = 25;
            LedEditExcelMarkerMouseController ledSigncontroller = new LedEditExcelMarkerMouseController(grid);
            grid.ShowGridLines = false;
            grid.Model.CellModels.Add("DataTemplate", new DataTemplateCellModel());
            grid.Model.CellModels.Add("CustomIntegerEdit", new CustomIntegerEditCellModel());
            grid.Model.CellModels.Add("LedEdit", new LedEditCellModel());
            grid.Model.CellModels.Add("Virtualized", new VirtualizedCellModel());
            grid.AllowDragDrop = false;
            IMouseController controller = grid.MouseControllerDispatcher.Find("ResizeRowsMouseController");
            grid.MouseControllerDispatcher.Remove(controller);
            controller = grid.MouseControllerDispatcher.Find("ResizeColumnsMouseController");
            grid.MouseControllerDispatcher.Remove(controller);
            grid.Model.ColumnWidths[0] = 30d;
            grid.Model.ColumnWidths[1] = 40d;
            //grid.Model.Options.ExcelLikeSelection = true;
            //grid.Model.Options.ExcelLikeSelectionFrame = true;
            grid.Model.TableStyle.Borders.Right = null;
            grid.Model.TableStyle.Borders.Bottom = null;
            var themename = SfSkinManager.GetTheme(grid).ThemeName;
            if (SfSkinManager.GetTheme(grid).ThemeName == "Windows11Dark")
            {
                grid.Model.TableStyle.Background = new SolidColorBrush(Color.FromArgb(255, 10, 0, 0));
            }
            else if (SfSkinManager.GetTheme(grid).ThemeName == "Windows11Light")
            {
                grid.Model.TableStyle.Background = new SolidColorBrush(Color.FromArgb(255, 240, 255, 255));
            }
            //grid.Model.Options.ShowCurrentCell = false;
            //grid.Model.Options.ExcelLikeCurrentCell = false;
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
                        //style.CellItemTemplateKey = "TextTemplate";
                    }
                    else
                    {
                        GridStyleInfo style = new GridStyleInfo();
                        style.CellValue = (j - 3) % 16;
                        //style.CellType = "CustomIntegerEdit";
                        //style.CellType = "IntegerEdit";
                        style.CellType = "LedEdit";
                        style.Tag = new LedDot() { LedColor = new SolidColorBrush(Color.FromRgb(255, 0, 0)), LedShape = LEDSHAPE.Rectangle };
                        //style.CellType = "Virtualized";
                        style.CellItemTemplateKey = "lededit";
                        style.IntegerEdit.MaxValue = 15;
                        style.IntegerEdit.MinValue = 0;
                        grid.Model[i, j] = style;
                        grid.Model[i, j].IsEditable = true;
                    }
                    grid.Model[i, j].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    grid.Model[i, j].VerticalAlignment = System.Windows.VerticalAlignment.Center;
                }
            }
            grid.PrepareRenderCell += OnGridPrepareRenderCell;
            grid.QueryCellInfo += OnGridQueryCellInfo;
            grid.CommitCellInfo += OnGridCommitCellInfo;
            //grid.SelectionChanging += Grid_SelectionChanging;
            grid.SelectionChanged += Grid_SelectionChanged;
        }

        private void Grid_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            grid.InvalidateCell(GridRangeInfo.Cells(Math.Min(e.Range.Top, oldRange.Top), Math.Min(e.Range.Left, oldRange.Left), Math.Max(e.Range.Bottom, oldRange.Bottom), Math.Max(e.Range.Right, oldRange.Right)));
            oldRange = e.Range;
        }

        GridRangeInfo oldRange = new GridRangeInfo();

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
            //if (e.Cell.ColumnIndex > 2 && e.Cell.RowIndex > 0)
            //{
            //    if (committedValues.ContainsKey(e.Cell))
            //        e.Style.CellValue = committedValues[e.Cell];
            //}
        }

        private void OnGridPrepareRenderCell(object sender, GridPrepareRenderCellEventArgs e)
        {
            //if (e.Cell.RowIndex == 0 && e.Cell.ColumnIndex > 2)
            //{
            //    e.Style.Background = Brushes.BlueViolet;
            //    e.Style.Foreground = Brushes.WhiteSmoke;
            //}
            //if (e.Cell.RowIndex > 0 && e.Cell.ColumnIndex > 2)
            //{
            //    if (e.Style.CellValue != null && e.Style.CellValue.ToString() != "")
            //    {
            //        Int16 pwm16 = Convert.ToInt16(e.Style.CellValue.ToString());
            //        Byte pwm = Convert.ToByte(pwm16 * 16);
            //        if (e.Cell.ColumnIndex > 2 && e.Cell.RowIndex > 0)
            //        {
            //            //e.Style.Font.FontWeight = FontWeights.Bold;
            //            //e.Style.Font.FontStyle = FontStyles.Italic;
            //            e.Style.Background = new SolidColorBrush(Color.FromArgb(255, pwm, 0, 0));
            //            e.Style.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            //        }
            //    }
            //}
        }

        private void ScrollViewer_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }
    }
    public class LedDot
    {
        public LEDSHAPE LedShape { get; set; }
        public SolidColorBrush LedColor { get; set; }

    }
    public enum LEDSHAPE
    {
        Rectangle, Circle, TriAngle, Polygon
    }
}
