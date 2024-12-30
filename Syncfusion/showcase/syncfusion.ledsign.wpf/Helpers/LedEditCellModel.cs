using Syncfusion.SfSkinManager;
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
    public class LedEditCellModel : GridCellModel<LedEditCellRenderer>
    {

    }

    public class LedEditCellRenderer : GridCellIntegerEditCellRenderer
    {
        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        {
            Rect rect = rca.SubtractBorderMargins(rca.CellRect, new Thickness(1, 0, 1, 2));
            SampleGrid grid = GridControl as SampleGrid;
            string themeName = SfSkinManager.GetTheme(grid).ThemeName;
            SolidColorBrush ledColor = grid.LedColor;
            int light = int.Parse(style.CellValue.ToString());
            GridRangeInfo range = GridControl.Model.SelectedCells;
            Geometry ledGeometry = grid.LedGeometry;
            ledGeometry.Transform = new TranslateTransform(rca.CellRect.X, rca.CellRect.Y);
            if ((rca.ColumnIndex < range.Left || rca.ColumnIndex > range.Right || rca.RowIndex < range.Top || rca.RowIndex > range.Bottom))
            {
                Byte lightR, lightG, lightB;
                if (themeName == "Windows11Light")
                {
                    lightR = Convert.ToByte(255 - (255 - ledColor.Color.R) * light / 16);
                    lightG = Convert.ToByte(255 - (255 - ledColor.Color.G) * light / 16);
                    lightB = Convert.ToByte(255 - (255 - ledColor.Color.B) * light / 16);
                }
                else
                {
                    lightR = Convert.ToByte(ledColor.Color.R * light / 16);
                    lightG = Convert.ToByte(ledColor.Color.G * light / 16);
                    lightB = Convert.ToByte(ledColor.Color.B * light / 16);
                }
                dc.DrawGeometry(new SolidColorBrush(Color.FromRgb(lightR, lightG, lightB)), new Pen(), PathGeometry.CreateFromGeometry(ledGeometry));
            }
            base.OnRender(dc, rca, style);
        }
        public override void OnInitializeContent(IntegerTextBox uiElement, GridRenderStyleInfo style)
        {
            string themeName = SfSkinManager.GetTheme(GridControl).ThemeName;
            base.OnInitializeContent(uiElement, style);
            uiElement.MaxValue = 15;
            uiElement.MinValue = 0;
            uiElement.CornerRadius = new CornerRadius(5);
            if(themeName == "Windows11Light")
            {
                uiElement.Foreground = Brushes.White;
                uiElement.Background = Brushes.Black;
            }else
            {
                uiElement.Foreground = Brushes.Black;
                uiElement.Background = Brushes.White;
            }
        }
    }
}
