using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;

namespace LedSignControl
{
    public class LedPaint
    {
        public static void DrawLedPolygon(DrawingContext dc, Rect rect, GridStyleInfo style)
        {

            //Draw Polygon
            PathFigure pf = new PathFigure();
            pf.StartPoint = new Point(rect.Left + rect.Width / 3, rect.Bottom);
            pf.Segments.Add(new LineSegment(new Point(rect.Right - rect.Width / 3, rect.Bottom), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Right, rect.Bottom - rect.Height / 3), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Right, rect.Top + rect.Height / 3), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Right - rect.Width / 3, rect.Top), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Left + rect.Width / 3, rect.Top), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Left, rect.Top + rect.Height / 3), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Left, rect.Bottom - rect.Height / 3), true));
            pf.IsClosed = true;
            PathGeometry pg = new PathGeometry();
            pg.Figures.Add(pf);
            dc.DrawGeometry(Brushes.Red, new Pen(), pg);
        }
        public static void DrawLed(DrawingContext dc, Rect rect, GridStyleInfo style)
        {
            PathFigure pf = new PathFigure();
            pf.IsClosed = true;
            pf.StartPoint = new Point(rect.Left, rect.Bottom);
            pf.Segments.Add(new LineSegment(new Point(rect.Right, rect.Bottom), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Right, rect.Bottom - 6), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Right - 3, rect.Bottom - 6), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Right - 3, rect.Bottom - 15), true));
            pf.Segments.Add(new ArcSegment(new Point(rect.Left + 3, rect.Bottom - 15), new Size((rect.Width - 6) / 2, (rect.Height - 6) / 2), 0, true, SweepDirection.Counterclockwise, true));
            pf.Segments.Add(new LineSegment(new Point(rect.Left + 3, rect.Bottom - 6), true));
            pf.Segments.Add(new LineSegment(new Point(rect.Left, rect.Bottom - 6), true));

            PathFigure pf2 = new PathFigure();
            pf2.StartPoint = new Point(rect.Left + 5, rect.Bottom);
            PathGeometry pg = new PathGeometry();
            pg.Figures.Add(pf);
            dc.DrawGeometry(Brushes.Red, new Pen(Brushes.Blue, 0), pg);
        }
    }
}
