using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.ledsign.wpf
{
    public class SimpleGridLed : GridControl
    {
        public SimpleGridLed()
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
    }

    public class LedEditCellModel : GridCellModel<LedEditCellRenderer>
    {

    }

    public class LedEditCellRenderer : GridCellIntegerEditCellRenderer
    {
        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        {
            base.OnRender(dc, rca, style); 
        }
        public override void OnInitializeContent(IntegerTextBox uiElement, GridRenderStyleInfo style)
        {
            base.OnInitializeContent(uiElement, style);
            uiElement.MaxValue = 15;
            uiElement.MinValue = 0;
        }
    }

    public class GroupLed
    {
        private SimpleGridLed _GridLed;

        public SimpleGridLed GridLed
        {
            get { return _GridLed; }
            set { _GridLed = value; }
        }


        private UpDown _UpDown;

        public UpDown UpDown
        {
            get { return _UpDown; }
            set { _UpDown = value; }
        }

    }
}
