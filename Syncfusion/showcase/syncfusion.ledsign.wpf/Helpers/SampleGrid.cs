using Syncfusion.Windows.Controls.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Shared;

namespace syncfusion.ledsign.wpf
{
    public class SampleGrid : GridControl
    {
        public SampleGrid()
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
                Model[0, i].CellValue = i - 3 + 1;
                for (int j = 1; j < Model.RowCount; j++)
                {
                    Model[j, i].CellType = "LedEdit";
                    Model[j, i].CellValue = (i - 3) % 16;
                }
            }
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

    }


    public class LedEditCellModel : GridCellModel<LedEditCellRenderer>
    {

    }

    public class LedEditCellRenderer : GridCellIntegerEditCellRenderer
    {
        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        {
            SampleGrid grid = GridControl as SampleGrid;
            base.OnRender(dc, rca, style);
        }
    }


    public class GroupLed
    {
        public SampleGrid GridLed { get; set; }
        public UpDown UpDown { get; set; }
        public GroupLed()
        {
            GridLed = new SampleGrid();
            UpDown = new UpDown();
            UpDown.TextAlignment = TextAlignment.Center;
            UpDown.MinValue = 1;
            UpDown.Value = 4;
            UpDown.NumberDecimalDigits = 0;
            UpDown.BorderThickness = new Thickness(0);
            UpDown.Focusable = false;
            UpDown.SetResourceReference(UpDown.BackgroundProperty, "PrimaryForeground");
        }

    }
}
