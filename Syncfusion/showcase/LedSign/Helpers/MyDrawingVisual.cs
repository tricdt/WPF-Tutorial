using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;

namespace LedSign.Helpers
{
    public class MyDrawingVisual
    {

        public static void DrawTriangle(DrawingContext drawingContext, Rect rect, string displayText, GridStyleInfo style)
        {
            StreamGeometry geom = new StreamGeometry();
            using (StreamGeometryContext gc = geom.Open())
            {
                // Start new object, filled=true, closed=true
                gc.BeginFigure(new Point(rect.Left, rect.Bottom), true, true);

                // isStroked=true, isSmoothJoin=true
                gc.LineTo(new Point(rect.Right, rect.Bottom), true, true);
                gc.LineTo(new Point((rect.Left + rect.Right) / 2, rect.Top), true, true);
            }
            drawingContext.DrawGeometry(Brushes.DarkRed, new Pen(), geom);
        }

        public static void DrawPolygon(DrawingContext dc, Rect rect, GridRenderStyleInfo style)
        {
            StreamGeometry geom = new StreamGeometry();
            Int16 pwm16 = Convert.ToInt16(style.CellValue.ToString());
            Byte pwm = Convert.ToByte(pwm16 * 16);
            using (StreamGeometryContext gc = geom.Open())
            {
                // Start new object, filled=true, closed=true
                gc.BeginFigure(new Point(rect.Left + rect.Width / 3, rect.Bottom), true, true);

                // isStroked=true, isSmoothJoin=true
                gc.LineTo(new Point(rect.Right - rect.Width / 3, rect.Bottom), true, true);
                gc.LineTo(new Point(rect.Right, rect.Bottom - rect.Width / 3), true, true);
                gc.LineTo(new Point(rect.Right, rect.Top + rect.Width / 3), true, true);
                gc.LineTo(new Point(rect.Right - rect.Width / 3, rect.Top), true, true);
                gc.LineTo(new Point(rect.Left + rect.Width / 3, rect.Top), true, true);
                gc.LineTo(new Point(rect.Left, rect.Top + rect.Width / 3), true, true);
                gc.LineTo(new Point(rect.Left, rect.Bottom - rect.Width / 3), true, true);
            }
            dc.DrawGeometry(new SolidColorBrush(Color.FromArgb(pwm, 255, 0, 0)), new Pen(), geom);
        }
        //using (StreamGeometryContext gc = geom.Open())
        //{
        //    // Start new object, filled=true, closed=true
        //    gc.BeginFigure(new Point(150.0, 150.0), true, true);

        //    // isStroked=true, isSmoothJoin=true
        //    gc.LineTo(new Point(210.0, 105.0), true, true);
        //    gc.LineTo(new Point(270.0, 150.0), true, true);
        //    gc.LineTo(new Point(270.0, 45.0), true, true);
        //    gc.LineTo(new Point(210.0, 90.0), true, true);
        //    gc.LineTo(new Point(150.0, 45.0), true, true);
        //}
    }
}
