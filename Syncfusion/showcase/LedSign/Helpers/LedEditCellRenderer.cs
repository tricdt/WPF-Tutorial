using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;

namespace LedSign
{
    public class LedEditCellModel : GridCellModel<LedEditCellRenderer>
    {
    }

    public class LedEditCellRenderer : GridCellIntegerEditCellRenderer
    {
        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        {
            Int16 pwm16 = Convert.ToInt16(style.CellValue.ToString());
            Byte pwm = Convert.ToByte(pwm16 * 16);
            Byte R = (style.Tag as LedDot).LedColor.Color.R;
            Byte G = (style.Tag as LedDot).LedColor.Color.G;
            Byte B = (style.Tag as LedDot).LedColor.Color.B;
            string themename = SfSkinManager.GetTheme(base.GridControl).ThemeName;
            GridRangeInfo range = GridControl.Model.SelectedCells;
            if (themename == "Windows11Light")
            {
                switch ((style.Tag as LedDot).LedShape)
                {
                    case LEDSHAPE.Rectangle:
                        if ((rca.ColumnIndex < range.Left || rca.ColumnIndex > range.Right || rca.RowIndex < range.Top || rca.RowIndex > range.Bottom))
                            dc.DrawRoundedRectangle(new SolidColorBrush(Color.FromRgb(255, Convert.ToByte(255 - pwm), Convert.ToByte(255 - pwm))), new Pen(), rca.CellRect, 5, 5);
                        //else { }
                        //dc.DrawRoundedRectangle(base.GridControl.Model.Options.HighlightSelectionAlphaBlend, new Pen(), rca.CellRect, 5, 5);
                        break;
                    case LEDSHAPE.Circle:
                        break;
                    case LEDSHAPE.Polygon:
                        break;
                }
            }
            if (range.Bottom == rca.RowIndex && range.Right == rca.ColumnIndex)
            {
                Rect r = rca.CellRect;
                r.X = r.Right - 2;
                r.Width = 4;
                r.Y = r.Bottom - 2;
                r.Height = 4;
                dc.DrawRectangle(Brushes.Black, null, r);
            }
            base.OnRender(dc, rca, style);
        }
        //protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        //{
        //    if (rca.CellUIElements != null)
        //    {
        //        return;
        //    }
        //    Thickness defaultMargin = style.TextMargins.ToThickness();
        //    defaultMargin = ((!style.HasImageIndex) ? style.ErrorInfo.AdjustErrorInfoMargin(defaultMargin, rca.CellRect.Size) : style.AdjustImageWidthAndHeightToMargin(defaultMargin, rca.CellRect.Size));
        //    Rect rect = rca.SubtractBorderMargins(rca.CellRect, defaultMargin);
        //    defaultMargin.Left = Math.Max(defaultMargin.Left, 2.0);
        //    defaultMargin.Right = Math.Max(defaultMargin.Right, 2.0);
        //    rect = rca.SubtractBorderMargins(rca.CellRect, defaultMargin);
        //    var themename = SfSkinManager.GetTheme(base.GridControl).ThemeName;
        //    if (!rect.IsEmpty)
        //    {
        //        string empty = string.Empty;
        //        empty = ((!IsCurrentCell(style) || !base.HasControlText) ? GetControlText(style) : ControlText);
        //        if (IsNegativeValue(style.CellValue))
        //        {
        //            style.Foreground = (style.HasNegativeForeground ? style.NegativeForeground : style.Foreground);
        //        }
        //        Int16 pwm16 = Convert.ToInt16(style.CellValue.ToString());
        //        Byte pwm = Convert.ToByte(pwm16 * 16);
        //        Byte R = (style.Tag as LedDot).LedColor.Color.R;
        //        Byte G = (style.Tag as LedDot).LedColor.Color.G;
        //        Byte B = (style.Tag as LedDot).LedColor.Color.B;

        //        if (themename == "Windows11Light")
        //        {
        //            switch ((style.Tag as LedDot).LedShape)
        //            {
        //                case LEDSHAPE.Rectangle:
        //                    //dc.DrawRoundedRectangle(new SolidColorBrush(Color.FromRgb(255, Convert.ToByte(255 - pwm), Convert.ToByte(255 - pwm))), new Pen(), rca.CellRect, 5, 5);
        //                    break;
        //                case LEDSHAPE.Circle:
        //                    break;
        //                case LEDSHAPE.Polygon:
        //                    break;
        //            }
        //        }
        //        //if (themename == "Windows11Light")
        //        //{
        //        //    style.Foreground = Brushes.Black;
        //        //    switch ((style.Tag as Led).LedShape)
        //        //    {
        //        //        case LedShape.Rectangle:
        //        //            dc.DrawRoundedRectangle(new SolidColorBrush(Color.FromArgb(255, 255, Convert.ToByte(255 - pwm), Convert.ToByte(255 - pwm))), new Pen(), rca.CellRect, 5, 5);
        //        //            break;
        //        //        case LedShape.Circle:
        //        //            dc.DrawEllipse(new SolidColorBrush(Color.FromArgb(255, 255, Convert.ToByte(255 - pwm), Convert.ToByte(255 - pwm))), new Pen(), new Point((rca.CellRect.Left + rca.CellRect.Right) / 2, (rca.CellRect.Top + rca.CellRect.Bottom) / 2), rca.CellRect.Width / 2, rca.CellRect.Width / 2);
        //        //            break;
        //        //        case LedShape.TriAngle:
        //        //            MyDrawingVisual.DrawTriangle(dc, rca.CellRect, empty, style);
        //        //            break;
        //        //        case LedShape.Polygon:
        //        //            MyDrawingVisual.DrawPolygon(dc, rca.CellRect, style);
        //        //            break;
        //        //        default:
        //        //            break;
        //        //    }

        //        //}
        //        //else if (themename == "Windows11Dark")
        //        //{
        //        //    style.Foreground = Brushes.White;
        //        //    switch ((style.Tag as Led).LedShape)
        //        //    {
        //        //        case LedShape.Rectangle:
        //        //            dc.DrawRoundedRectangle(new SolidColorBrush(Color.FromArgb(255, pwm, 0, 0)), new Pen(), rca.CellRect, 5, 5);
        //        //            break;
        //        //        case LedShape.Circle:
        //        //            dc.DrawEllipse(new SolidColorBrush(Color.FromRgb((style.Tag as Led).LdColor.Color.R, (style.Tag as Led).LdColor.Color.G, (style.Tag as Led).LdColor.Color.B)), new Pen(), new Point((rca.CellRect.Left + rca.CellRect.Right) / 2, (rca.CellRect.Top + rca.CellRect.Bottom) / 2), rca.CellRect.Width / 2, rca.CellRect.Width / 2);
        //        //            break;
        //        //        case LedShape.TriAngle:
        //        //            MyDrawingVisual.DrawTriangle(dc, rca.CellRect, empty, style);
        //        //            break;
        //        //        case LedShape.Polygon:
        //        //            MyDrawingVisual.DrawPolygon(dc, rca.CellRect, style);
        //        //            break;
        //        //        default:
        //        //            break;
        //        //    }
        //        //}
        //        GridRangeInfo range = GridControl.Model.SelectedCells;
        //        if (range.Bottom == rca.RowIndex && range.Right == rca.ColumnIndex)
        //        {
        //            Rect r = rca.CellRect;
        //            //r.Width = range.Width * r.Width;
        //            r.X = r.Right - 2;
        //            r.Width = range.Width;
        //            r.Y = r.Bottom - 2;
        //            r.Height = range.Height;
        //            //dc.DrawRectangle(Brushes.Black, new Pen(), r);
        //            dc.DrawRectangle(Brushes.Yellow, null, r);
        //        }
        //        GridTextBoxPaint.DrawText(dc, rect, empty, style);
        //    }

        //    //if (range.Bottom == rca.RowIndex && range.Right == rca.ColumnIndex)
        //    //{
        //    //    Rect r = rca.CellRect;
        //    //    dc.DrawRectangle(Brushes.Black, new Pen(), r);
        //    //}
        //    base.OnRender(dc, rca, style);

        //}

    }
}
