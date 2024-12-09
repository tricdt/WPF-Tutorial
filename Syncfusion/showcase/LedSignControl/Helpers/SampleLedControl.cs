using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LedSignControl
{
    public class SampleLedControl : GridControl
    {
        public static readonly DependencyProperty LedCountProperty = DependencyProperty.Register("LedCount", typeof(int), typeof(SampleLedControl), new PropertyMetadata(4, OnLedCountChangedCallback));
        public static readonly DependencyProperty LedShapeProperty = DependencyProperty.Register("LedShape", typeof(LEDSHAPE), typeof(SampleLedControl), new PropertyMetadata(LEDSHAPE.Rectangle, OnLedShapeChangedCallback));
        public static readonly DependencyProperty LedColorProperty = DependencyProperty.Register("LedColor", typeof(SolidColorBrush), typeof(SampleLedControl), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 0, 0)), OnLedColorChangedCallback));
        public static readonly DependencyProperty LedStartProperty = DependencyProperty.Register("LedStart", typeof(int), typeof(SampleLedControl), new PropertyMetadata(1, OnLedStartChangedCallback));
        public static readonly DependencyProperty LedThemeProperty = DependencyProperty.Register("LedTheme", typeof(LEDTHEME), typeof(SampleLedControl), new PropertyMetadata(LEDTHEME.LIGHT, OnLedThemeChangedCallback));

        public int LedCount
        {
            get { return (int)GetValue(LedCountProperty); }
            set { SetValue(LedCountProperty, value); }
        }
        public LEDSHAPE LedShape
        {
            get { return (LEDSHAPE)GetValue(LedShapeProperty); }
            set { SetValue(LedShapeProperty, value); }
        }
        public SolidColorBrush LedColor
        {
            get { return (SolidColorBrush)GetValue(LedColorProperty); }
            set { SetValue(LedColorProperty, value); }
        }
        public int LedStart
        {
            get { return (int)GetValue(LedStartProperty); }
            set { SetValue(LedStartProperty, value); }
        }

        public LEDTHEME LedTheme
        {
            get { return (LEDTHEME)GetValue(LedThemeProperty); }
            set { SetValue(LedThemeProperty, value); }
        }

        private DelegateCommand<object> _ContextMenuCommand;
        public DelegateCommand<object> ContextMenuCommand
        {
            get
            {
                if (_ContextMenuCommand == null)
                {
                    _ContextMenuCommand = new DelegateCommand<object>(OnContextMenuInfoExcute);
                }
                return _ContextMenuCommand;
            }
        }

        public SampleLedControl()
        {
            Model.RowCount = 60;
            Model.ColumnCount = 7;
            Model.ColumnWidths.DefaultLineSize = 25;
            Model.RowHeights.DefaultLineSize = 25;
            Model.ColumnWidths[0] = 30d;
            Model.ColumnWidths[1] = 40d;

            Model.CellModels.Add("LedEdit", new LedEditCellModel());
            Model.TableStyle.HorizontalAlignment = HorizontalAlignment.Center;
            Model.TableStyle.VerticalAlignment = VerticalAlignment.Center;
            AllowDragDrop = false;
            Model.TableStyle.Borders = new CellBordersInfo()
            {
                All = new Pen(Brushes.Gray, 0.05d)
            };


            Model.CoveredRanges.Add(new CoveredCellInfo(0, 0, 0, 2));

            for (int i = 1; i < Model.RowCount; i++)
            {
                Model[i, 0].CellType = "Header";
                Model[i, 0].CellValue = i;
                Model[i, 1].CellValue = 200;
                Model[i, 2].CellValue = 1;
            }
            for (int i = 3; i < Model.ColumnCount; i++)
            {
                Model[0, i].CellType = "Header";
                Model[0, i].CellValue = i - 3 + LedStart;
                for (int j = 1; j < Model.RowCount; j++)
                {
                    Model[j, i].CellType = "LedEdit";
                    Model[j, i].CellValue = (i - 3) % 16;
                }
            }

            LedEditMarkerMouseController ledEditController = new LedEditMarkerMouseController(this);
            Model.CommandStack.Enabled = true;
            MouseControllerDispatcher.Add(ledEditController);

            //IMouseController controller = MouseControllerDispatcher.Find("ResizeRowsMouseController");
            //MouseControllerDispatcher.Remove(controller);
            //controller = MouseControllerDispatcher.Find("ResizeColumnsMouseController");
            //MouseControllerDispatcher.Remove(controller);

            Model.EnableContextMenu = true;
            Model.QueryContextMenuInfo += Model_QueryContextMenuInfo;
        }


        private void OnContextMenuInfoExcute(object obj)
        {
        }

        private static void OnLedCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SampleLedControl grid = (d as SampleLedControl);
            if (grid.Model.ColumnCount < Convert.ToInt32(e.NewValue) + 3)
            {
                for (int i = grid.Model.ColumnCount; i < Convert.ToInt32(e.NewValue) + 3; i++)
                {
                    grid.Model.InsertColumns(grid.Model.ColumnCount, 1);
                    GridStyleInfo style = grid.Model[0, grid.Model.ColumnCount - 1];
                    style.CellValue = grid.LedStart + i - 3;
                    style.CellType = "Header";
                    if (grid.LedTheme == LEDTHEME.LIGHT)
                    {
                        style.Background = Brushes.WhiteSmoke;
                        style.Foreground = Brushes.Black;
                    }
                    else
                    {
                        style.Background = Brushes.Black;
                        style.Foreground = Brushes.WhiteSmoke;
                    }
                    for (int j = 1; j < grid.Model.RowCount; j++)
                    {
                        style = grid.Model[j, grid.Model.ColumnCount - 1];
                        style.CellType = "LedEdit";
                        style.CellValue = (grid.Model.ColumnCount - 4) % 16;
                    }
                }
            }
            else
            {
                grid.Model.ColumnCount = Convert.ToInt32(e.NewValue) + 3;
            }
            grid.Width = (grid.Model.ColumnCount - 3) * 25 + 105;
            grid.InvalidateCells();
        }

        private static void OnLedShapeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SampleLedControl).InvalidateCells();
        }
        private static void OnLedColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SampleLedControl).InvalidateCells();
        }

        private static void OnLedStartChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SampleLedControl grid = d as SampleLedControl;
            for (int i = 3; i < grid.Model.ColumnCount - 1; i++)
            {
                grid.Model[0, i].CellValue = grid.LedStart + i - 3;
                grid.InvalidateCell(GridRangeInfo.Cell(0, i));
            }
        }

        private static void OnLedThemeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SampleLedControl grid = (d as SampleLedControl);
            if ((LEDTHEME)(e.NewValue) == LEDTHEME.DARK)
            {
                grid.Model.TableStyle.Foreground = Brushes.WhiteSmoke;
                grid.Model.TableStyle.Background = Brushes.Black;
                for (int i = 3; i < grid.Model.ColumnCount; i++)
                {
                    grid.Model[0, i].Background = Brushes.Black;
                }
                for (int i = 0; i < grid.Model.RowCount; i++)
                {
                    grid.Model[i, 0].Background = Brushes.Black;
                }
            }
            else
            {
                grid.Model.TableStyle.Foreground = Brushes.Black;
                grid.Model.TableStyle.Background = Brushes.WhiteSmoke;
                for (int i = 3; i < grid.Model.ColumnCount; i++)
                {
                    grid.Model[0, i].Background = Brushes.WhiteSmoke;
                }
                for (int i = 0; i < grid.Model.RowCount; i++)
                {
                    grid.Model[i, 0].Background = Brushes.WhiteSmoke;
                }
            }
            grid.InvalidateCells();
        }

        private void Model_QueryContextMenuInfo(object sender, GridQueryContextMenuInfoEventArgs e)
        {
            List<MenuItem> ContextMenuItems = new List<MenuItem>();
            MenuItem menuitem1 = new MenuItem { Background = Brushes.LightYellow, Header = "SortColumn", Command = ContextMenuCommand };
            MenuItem menuitem2 = new MenuItem { Background = Brushes.LightYellow, Header = "Copy", Command = ContextMenuCommand, CommandParameter = "Copy" };
            MenuItem menuitem3 = new MenuItem { Background = Brushes.LightYellow, Header = "Paste", Command = ContextMenuCommand, CommandParameter = "Paste" };
            int columnIndex = e.Cell.ColumnIndex;
            string parameter = "Sort" + "_" + columnIndex.ToString();
            menuitem1.CommandParameter = parameter;

            if (e.Cell.RowIndex == 0)
            {
                ContextMenuItems.Add(menuitem1);
            }
            else
            {
                ContextMenuItems.Add(menuitem2);
                ContextMenuItems.Add(menuitem3);
            }

            e.Style.ContextMenuItems = ContextMenuItems;
        }

        private GridRangeInfo oldRange { get; set; }
        protected override void OnSelectionChanged(GridSelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            if (e.Reason == GridSelectionReason.MouseDown || e.Reason == GridSelectionReason.SetCurrentCell || e.Reason == GridSelectionReason.MouseMove || e.Reason == GridSelectionReason.SelectRange || e.Reason == GridSelectionReason.MouseUp)
            {
                InvalidateCell(GridRangeInfo.Row(0));
                InvalidateCell(GridRangeInfo.Col(0));
                if (oldRange != null)
                {
                    for (int row = oldRange.Top; row <= oldRange.Bottom; row++)
                    {
                        for (int col = oldRange.Left; col <= oldRange.Right; col++)
                        {
                            if (!Model.SelectedRanges.Contains(GridRangeInfo.Cell(row, col)))
                            {
                                InvalidateCell(GridRangeInfo.Cell(row, col));
                            }
                        }
                    }
                }
                if (oldRange != null)
                {
                    foreach (GridRangeInfo range in Model.SelectedRanges)
                    {
                        for (int row = range.Top; row <= range.Bottom; row++)
                        {
                            for (int col = range.Left; col <= range.Right; col++)
                            {
                                if (!oldRange.Contains(GridRangeInfo.Cell(row, col)))
                                {
                                    InvalidateCell(GridRangeInfo.Cell(row, col));
                                }
                            }
                        }
                    }
                }
            }

            oldRange = e.Range;
        }

        protected override void OnResizingRows(GridResizingRowsEventArgs args)
        {
            base.OnResizingRows(args);
            args.AllowResize = false;
        }

        protected override void OnResizingColumns(GridResizingColumnsEventArgs args)
        {
            base.OnResizingColumns(args);
            args.AllowResize = false;
        }

        protected override void OnSelectionChanging(GridSelectionChangingEventArgs e)
        {
            base.OnSelectionChanging(e);
            if (e.Range.Left == 1)
            {
                if (e.Range.Contains(GridRangeInfo.Cells(e.Range.Top, 2, e.Range.Bottom, 2)))
                {
                    e.Cancel = true;
                }
            }
            if (e.Range.Left == 2)
            {
                if (e.Range.Contains(GridRangeInfo.Cells(e.Range.Top, 3, e.Range.Bottom, 3)))
                {
                    e.Cancel = true;
                }
            }
        }

        protected override void OnPrepareRenderCell(GridPrepareRenderCellEventArgs e)
        {
            base.OnPrepareRenderCell(e);
            if (LedTheme == LEDTHEME.LIGHT)
            {
                if (e.Cell.RowIndex == 0 && Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Col(e.Cell.ColumnIndex)))
                {
                    e.Style.Background = Brushes.LightGray;
                    e.Style.Font.FontWeight = FontWeights.Bold;
                }
                else if (e.Cell.ColumnIndex == 0 && Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Row(e.Cell.RowIndex)))
                {
                    e.Style.Background = Brushes.LightGray;
                    e.Style.Font.FontWeight = FontWeights.Bold;
                }
            }
            else
            {
                if (e.Cell.RowIndex == 0 && Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Col(e.Cell.ColumnIndex)))
                {
                    e.Style.Background = Brushes.BlueViolet;
                    e.Style.Font.FontWeight = FontWeights.Bold;
                }
                else if (e.Cell.ColumnIndex == 0 && Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Row(e.Cell.RowIndex)))
                {
                    e.Style.Background = Brushes.BlueViolet;
                    e.Style.Font.FontWeight = FontWeights.Bold;
                }
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Delete && CurrentCell != null && CurrentCell.HasCurrentCell)
            {
                foreach (GridRangeInfo SelectedRange in Model.SelectedRanges)
                {
                    GridRangeInfo ExpandedRange = ExpandSelectedCellsRange(SelectedRange);
                    for (int row = ExpandedRange.Top; row <= ExpandedRange.Bottom; row++)
                    {
                        for (int col = ExpandedRange.Left; col <= ExpandedRange.Right; col++)
                        {
                            var style = Model[row, col];
                            if (!style.ReadOnly && style.Enabled)
                                style.CellValue = 0;
                        }
                    }
                }
                Model.ActiveGridView.InvalidateCells();
            }
        }
    }

    public class LedDot
    {
        public LEDSHAPE LedShape { get; set; }
        public LEDCOLOR LedColor { get; set; }

    }
    public enum LEDSHAPE
    {
        Rectangle, Circle, TriAngle, Polygon
    }

    public enum LEDCOLOR
    {
        RED, GREEN, BLUE
    }
    public enum LEDTHEME
    {
        LIGHT, DARK
    }

    public class LedEditCellModel : GridCellModel<LedEditCellRenderer>
    {

    }

    public class LedEditCellRenderer : GridCellIntegerEditCellRenderer
    {
        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        {
            Rect rect = rca.SubtractBorderMargins(rca.CellRect, new Thickness(1, 0, 1, 2));

            LedPaint.DrawLed(dc, rect, style);
            //LedPaint.DrawLedPolygonViewPort(dc, rect, style);
            //Draw Polygon
            //PathFigure pf = new PathFigure();
            //pf.StartPoint = new Point(rect.Left + rect.Width / 3, rect.Bottom);
            //pf.Segments.Add(new LineSegment(new Point(rect.Right - rect.Width / 3, rect.Bottom), true));
            //pf.Segments.Add(new LineSegment(new Point(rect.Right, rect.Bottom - rect.Height / 3), true));
            //pf.Segments.Add(new LineSegment(new Point(rect.Right, rect.Top + rect.Height / 3), true));
            //pf.Segments.Add(new LineSegment(new Point(rect.Right - rect.Width / 3, rect.Top), true));
            //pf.Segments.Add(new LineSegment(new Point(rect.Left + rect.Width / 3, rect.Top), true));
            //pf.Segments.Add(new LineSegment(new Point(rect.Left, rect.Top + rect.Height / 3), true));
            //pf.Segments.Add(new LineSegment(new Point(rect.Left, rect.Bottom - rect.Height / 3), true));
            //pf.IsClosed = true;
            //PathGeometry pg = new PathGeometry();
            //pg.Figures.Add(pf);
            //dc.DrawGeometry(Brushes.Red, new Pen(), pg);
            var model = GridControl;
            LEDSHAPE ledShape = (GridControl as SampleLedControl).LedShape;
            LEDTHEME ledTheme = (GridControl as SampleLedControl).LedTheme;
            SolidColorBrush ledColor = (GridControl as SampleLedControl).LedColor;
            int light = Convert.ToInt16(style.CellValue.ToString());

            string themename = SfSkinManager.GetTheme(base.GridControl).ThemeName;
            GridRangeInfo range = GridControl.Model.SelectedCells;

            if ((rca.ColumnIndex < range.Left || rca.ColumnIndex > range.Right || rca.RowIndex < range.Top || rca.RowIndex > range.Bottom))
            {
                if (ledTheme == LEDTHEME.LIGHT)
                {
                    Byte lightR = Convert.ToByte(255 - (255 - ledColor.Color.R) * light / 16);
                    Byte lightG = Convert.ToByte(255 - (255 - ledColor.Color.G) * light / 16);
                    Byte lightB = Convert.ToByte(255 - (255 - ledColor.Color.B) * light / 16);
                    switch (ledShape)
                    {
                        case LEDSHAPE.Rectangle:

                            //dc.DrawRoundedRectangle(new SolidColorBrush(Color.FromRgb(lightR, lightG, lightB)), new Pen(), rca.CellRect, 5, 5);
                            //else { }
                            //dc.DrawRoundedRectangle(base.GridControl.Model.Options.HighlightSelectionAlphaBlend, new Pen(), rca.CellRect, 5, 5);
                            break;
                        case LEDSHAPE.Circle:
                            dc.DrawEllipse(new SolidColorBrush(Color.FromRgb(lightR, lightG, lightB)), new Pen(), new Point((rca.CellRect.Left + rca.CellRect.Right) / 2, (rca.CellRect.Top + rca.CellRect.Bottom) / 2), (rca.CellRect.Right - rca.CellRect.Left) / 2, (rca.CellRect.Bottom - rca.CellRect.Top) / 2);
                            break;
                        case LEDSHAPE.Polygon:
                            break;
                    }
                }
                if (ledTheme == LEDTHEME.DARK)
                {
                    Byte lightR = Convert.ToByte(ledColor.Color.R * light / 16);
                    Byte lightG = Convert.ToByte(ledColor.Color.G * light / 16);
                    Byte lightB = Convert.ToByte(ledColor.Color.B * light / 16);
                    switch (ledShape)
                    {
                        case LEDSHAPE.Rectangle:

                            //dc.DrawRoundedRectangle(new SolidColorBrush(Color.FromRgb(lightR, lightG, lightB)), new Pen(), rca.CellRect, 5, 5);
                            //else { }
                            //dc.DrawRoundedRectangle(base.GridControl.Model.Options.HighlightSelectionAlphaBlend, new Pen(), rca.CellRect, 5, 5);
                            break;
                        case LEDSHAPE.Circle:
                            dc.DrawEllipse(new SolidColorBrush(Color.FromRgb(lightR, lightG, lightB)), new Pen(), new Point((rca.CellRect.Left + rca.CellRect.Right) / 2, (rca.CellRect.Top + rca.CellRect.Bottom) / 2), (rca.CellRect.Right - rca.CellRect.Left) / 2, (rca.CellRect.Bottom - rca.CellRect.Top) / 2);
                            break;
                        case LEDSHAPE.Polygon:
                            break;
                    }
                }
            }

            base.OnRender(dc, rca, style);
        }
        public override void OnInitializeContent(IntegerTextBox uiElement, GridRenderStyleInfo style)
        {
            base.OnInitializeContent(uiElement, style);
            uiElement.MaxValue = 15;
            uiElement.MinValue = 0;
        }

        private void OnDrawingLed(DrawingContext dc, int pwm, LEDCOLOR color, LEDSHAPE shape, string themename)
        {
        }
    }
}
