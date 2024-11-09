

using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Syncfusion.Windows.Controls.Grid;

//
// Summary:
//     Draws a text box control in the grid cells.
public class MyGridTextBoxPaint
{
    //
    // Summary:
    //     Gets or sets a value indicating whether [allow round draw text origin].
    //
    // Value:
    //     true if [allow round draw text origin]; otherwise, false.
    public static bool AllowRoundDrawTextOrigin { get; set; }

    //
    // Summary:
    //     Initializes the Syncfusion.Windows.Controls.Grid.GridTextBoxPaint class.
    static MyGridTextBoxPaint()
    {
        AllowRoundDrawTextOrigin = false;
    }

    //
    // Summary:
    //     Draws the text.
    //
    // Parameters:
    //   drawingContext:
    //     The drawing context.
    //
    //   textRectangle:
    //     The text rectangle.
    //
    //   displayText:
    //     The display text.
    //
    //   style:
    //     The cell style.
    public static void DrawText(DrawingContext drawingContext, Rect textRectangle, string displayText, GridStyleInfo style)
    {
        if (string.IsNullOrEmpty(displayText))
        {
            return;
        }

        double num = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        CultureInfo culture = style.GetCulture(useCurrentCultureIfNull: true);
        FlowDirection flowDirection = style.FlowDirection;
        Typeface typeface = style.Typeface;
        double fontSize = style.ReadOnlyFont.FontSize;
        double lineHeightValue = style.ReadOnlyFont.GetLineHeightValue();
        Brush foreground = style.Foreground;
        double num5 = style.Font.Orientation % 360;
        bool flag = num5 == 90.0 || num5 == 270.0;
        FormattedText formattedText = new FormattedText(displayText, culture, flowDirection, typeface, fontSize, foreground);
        formattedText.Trimming = style.TextTrimming;
        if (flag)
        {
            formattedText.TextAlignment = HorizontalAlignmentToTextAlignment((HorizontalAlignment)style.VerticalAlignment, num5);
        }
        else
        {
            formattedText.TextAlignment = HorizontalAlignmentToTextAlignment(style.HorizontalAlignment, num5);
        }

        if (style.Font != null && style.Font.TextDecorations != null)
        {
            formattedText.SetTextDecorations(style.Font.TextDecorations);
        }

        Rect rect = textRectangle;
        Rect rect2 = textRectangle;
        double val = 4.0;
        if (num5 < 0.0)
        {
            num5 += 360.0;
        }

        RotateTransform rotateTransform = null;
        if (num5 != 0.0)
        {
            rotateTransform = new RotateTransform(num5, textRectangle.X + textRectangle.Width / 2.0, textRectangle.Y + textRectangle.Height / 2.0);
            rect2 = rotateTransform.TransformBounds(textRectangle);
            num = textRectangle.Width;
            num2 = textRectangle.Height;
            num3 = textRectangle.X;
            num4 = textRectangle.Y;
        }

        textRectangle = rect2;
        bool flag2 = false;
        bool flag3 = style.TextWrapping != TextWrapping.NoWrap;
        if (flag3)
        {
            if (textRectangle.Height >= lineHeightValue * 2.0)
            {
                formattedText.MaxLineCount = (int)(textRectangle.Height / lineHeightValue) + 1;
            }
            else
            {
                flag3 = false;
            }
        }

        if (!flag3)
        {
            formattedText.MaxLineCount = 1;
        }

        if (flag)
        {
            formattedText.MaxTextWidth = Math.Max(val, rect.Height);
            formattedText.MaxTextHeight = Math.Max(val, rect.Width);
        }
        else if (num5 == 180.0)
        {
            formattedText.MaxTextWidth = Math.Max(val, rect.Width);
            formattedText.MaxTextHeight = Math.Max(val, rect.Height);
        }
        else if (num5 != 0.0)
        {
            formattedText.MaxTextWidth = Math.Max(val, Math.Min(rect.Height, rect.Width));
            formattedText.MaxTextHeight = Math.Max(val, formattedText.MaxTextWidth);
        }
        else if (style.TextWrapping != TextWrapping.NoWrap || formattedText.Trimming != 0)
        {
            formattedText.MaxTextWidth = Math.Max(val, textRectangle.Width);
        }
        else
        {
            formattedText.MaxTextWidth = textRectangle.Width;
        }

        if (num5 != 0.0 && num5 != 270.0 && num5 != 90.0 && num5 != 180.0)
        {
            double width = formattedText.Width;
            if (textRectangle.Width - width < 0.0)
            {
                flag2 = true;
            }

            if (width < textRectangle.Width)
            {
                textRectangle.Width = width;
            }
            else
            {
                flag2 = true;
            }

            switch (style.HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    rotateTransform = ((!(num5 > 180.0)) ? new RotateTransform(num5, num3 + num / 3.0, num4) : new RotateTransform(num5, num3 + num / 3.0, num4 - 10.0 + num2));
                    break;
                case HorizontalAlignment.Left:
                    rotateTransform = ((!(num5 > 180.0)) ? new RotateTransform(num5, num3 + 10.0, num4) : new RotateTransform(num5, num3 + 5.0, num4 - 10.0 + num2));
                    break;
                case HorizontalAlignment.Right:
                    rotateTransform = ((!(num5 > 180.0)) ? new RotateTransform(num5, num3 + num / 2.0, num4) : new RotateTransform(num5, num3 + num / 2.0, num4 - 10.0 + num2));
                    break;
            }
        }

        double height = formattedText.Height;
        double num6 = textRectangle.Height - height;
        if (num6 < 0.0)
        {
            flag2 = true;
            num6 = 0.0;
        }

        if (height < textRectangle.Height)
        {
            textRectangle.Height = height;
        }
        else
        {
            flag2 = true;
        }

        VerticalAlignment verticalAlignment = style.VerticalAlignment;
        if (flag)
        {
            verticalAlignment = (VerticalAlignment)style.HorizontalAlignment;
        }

        double top = style.BorderMargins.Top;
        double bottom = style.BorderMargins.Bottom;
        switch (verticalAlignment)
        {
            case VerticalAlignment.Center:
                textRectangle.Y += num6 / 2.0 - 2.0 * (top + bottom);
                break;
            case VerticalAlignment.Top:
                textRectangle.Y -= 2.0 * (top + bottom);
                break;
            case VerticalAlignment.Bottom:
                textRectangle.Y += num6 - 2.0 * (top + bottom);
                break;
        }

        if (flag2 || formattedText.Width > ((num5 != 0.0 && num5 != 180.0) ? rect.Height : rect.Width) || formattedText.Height > ((num5 != 0.0 && num5 != 180.0) ? rect.Width : rect.Height) || ((num5 == 0.0 || num5 == 180.0) && (textRectangle.X < rect.X || textRectangle.Y < rect.Y || textRectangle.Right > rect.Right || textRectangle.Bottom > rect.Bottom)))
        {
            Rect rect3 = rect;
            if (num5 == 90.0 || num5 == 180.0 || num5 == 270.0)
            {
                rect3.Intersect(rect);
                rect3 = rotateTransform.TransformBounds(rect3);
            }

            RectangleGeometry rectangleGeometry = new RectangleGeometry(rect3);
            rectangleGeometry.Freeze();
            drawingContext.PushClip(rectangleGeometry);
            if (rotateTransform != null)
            {
                drawingContext.PushTransform(rotateTransform);
            }

            Point origin = ((!AllowRoundDrawTextOrigin) ? new Point(textRectangle.Location.X, textRectangle.Location.Y + 0.5) : new Point(Math.Ceiling(textRectangle.Location.X), Math.Ceiling(textRectangle.Location.Y)));
            if (flowDirection == FlowDirection.RightToLeft)
            {
                double m = -1.0;
                double m2 = 1.0;
                double offsetX = formattedText.Width + rect.Width;
                double offsetY = 0.0;
                drawingContext.PushTransform(new MatrixTransform(m, 0.0, 0.0, m2, offsetX, offsetY));
                origin.X = formattedText.Width - origin.X;
            }

            drawingContext.DrawText(formattedText, origin);
            if (flowDirection == FlowDirection.RightToLeft)
            {
                drawingContext.Pop();
            }

            if (rotateTransform != null)
            {
                drawingContext.Pop();
            }

            drawingContext.Pop();
        }
        else
        {
            if (rotateTransform != null)
            {
                drawingContext.PushTransform(rotateTransform);
            }

            Point origin2 = ((!AllowRoundDrawTextOrigin) ? new Point(textRectangle.Location.X, textRectangle.Location.Y + 0.5) : new Point(Math.Ceiling(textRectangle.Location.X), Math.Ceiling(textRectangle.Location.Y)));
            if (flowDirection == FlowDirection.RightToLeft)
            {
                double m3 = -1.0;
                double m4 = 1.0;
                double offsetX2 = formattedText.Width + rect.Width;
                double offsetY2 = 0.0;
                drawingContext.PushTransform(new MatrixTransform(m3, 0.0, 0.0, m4, offsetX2, offsetY2));
                origin2.X = formattedText.Width - origin2.X;
            }

            if (num5 != 0.0 && num5 != 270.0 && num5 != 90.0 && num5 != 180.0)
            {
                origin2.X = rotateTransform.CenterX;
                origin2.Y = rotateTransform.CenterY;
            }

            drawingContext.DrawText(formattedText, origin2);
            if (flowDirection == FlowDirection.RightToLeft)
            {
                drawingContext.Pop();
            }

            if (rotateTransform != null)
            {
                drawingContext.Pop();
            }
        }
    }

    //
    // Summary:
    //     Returns the size of the text.
    //
    // Parameters:
    //   textRectangle:
    //     Text rectangle.
    //
    //   displayText:
    //     The display text.
    //
    //   style:
    //     Cell style.
    //
    //   queryBounds:
    //     Cell bounds.
    //
    // Returns:
    //     Size of the text.
    public static Size MeasureText(Size textRectangle, string displayText, GridStyleInfo style, GridQueryBounds queryBounds)
    {
        CultureInfo culture = style.GetCulture(useCurrentCultureIfNull: true);
        FlowDirection flowDirection = style.FlowDirection;
        Typeface typeface = style.Typeface;
        double fontSize = style.ReadOnlyFont.FontSize;
        Brush foreground = style.Foreground;
        FormattedText formattedText = new FormattedText(displayText, culture, flowDirection, typeface, fontSize, foreground);
        if (queryBounds == GridQueryBounds.Height)
        {
            formattedText.MaxTextWidth = textRectangle.Width;
        }

        formattedText.Trimming = style.TextTrimming;
        return new Size(formattedText.WidthIncludingTrailingWhitespace, formattedText.Height);
    }

    private static TextAlignment HorizontalAlignmentToTextAlignment(HorizontalAlignment horizontalAlignment, double orientation)
    {
        TextAlignment result;
        switch (horizontalAlignment)
        {
            default:
                result = TextAlignment.Left;
                if (orientation == 90.0 || orientation == 270.0)
                {
                    result = TextAlignment.Right;
                }

                break;
            case HorizontalAlignment.Right:
                result = TextAlignment.Right;
                if (orientation == 90.0 || orientation == 270.0)
                {
                    result = TextAlignment.Left;
                }

                break;
            case HorizontalAlignment.Center:
                result = TextAlignment.Center;
                break;
            case HorizontalAlignment.Stretch:
                result = TextAlignment.Justify;
                break;
        }

        return result;
    }
}
#if false // Decompilation log
'233' items in cache
------------------
Resolve: 'System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'PresentationFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'PresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\PresentationFramework.dll'
------------------
Resolve: 'System.Xaml, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Xaml.dll'
------------------
Resolve: 'PresentationCore, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'PresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\PresentationCore.dll'
------------------
Resolve: 'WindowsBase, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'WindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\WindowsBase.dll'
------------------
Resolve: 'ReachFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'ReachFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\ReachFramework.dll'
------------------
Resolve: 'UIAutomationClient, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'UIAutomationClient, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\UIAutomationClient.dll'
------------------
Resolve: 'UIAutomationTypes, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'UIAutomationTypes, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\UIAutomationTypes.dll'
------------------
Resolve: 'System.Linq.Expressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Expressions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Linq.Expressions.dll'
------------------
Resolve: 'Syncfusion.Linq.Base, Version=27.1.48.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89'
Found single assembly: 'Syncfusion.Linq.Base, Version=27.1.48.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89'
Load from: 'C:\Program Files (x86)\Syncfusion\Essential Studio\WPF\27.1.48\precompiledassemblies\net8.0\Syncfusion.Linq.Base.dll'
------------------
Resolve: 'System.Collections, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Collections.dll'
------------------
Resolve: 'Syncfusion.Shared.WPF, Version=27.1.48.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89'
Found single assembly: 'Syncfusion.Shared.WPF, Version=27.1.48.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89'
Load from: 'C:\Program Files (x86)\Syncfusion\Essential Studio\WPF\27.1.48\precompiledassemblies\net8.0\Syncfusion.Shared.WPF.dll'
------------------
Resolve: 'System.ComponentModel.TypeConverter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.TypeConverter, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ComponentModel.TypeConverter.dll'
------------------
Resolve: 'System.ComponentModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ComponentModel.dll'
------------------
Resolve: 'System.ObjectModel, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ObjectModel.dll'
------------------
Resolve: 'UIAutomationProvider, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'UIAutomationProvider, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\UIAutomationProvider.dll'
------------------
Resolve: 'Syncfusion.GridCommon.WPF, Version=27.1.48.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89'
Found single assembly: 'Syncfusion.GridCommon.WPF, Version=27.1.48.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89'
Load from: 'C:\Program Files (x86)\Syncfusion\Essential Studio\WPF\27.1.48\precompiledassemblies\net8.0\Syncfusion.GridCommon.WPF.dll'
------------------
Resolve: 'System.Xml.ReaderWriter, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.ReaderWriter, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Xml.ReaderWriter.dll'
------------------
Resolve: 'System.ComponentModel.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ComponentModel.Primitives.dll'
------------------
Resolve: 'System.Threading.Thread, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading.Thread, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Threading.Thread.dll'
------------------
Resolve: 'System.Collections.NonGeneric, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.NonGeneric, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Collections.NonGeneric.dll'
------------------
Resolve: 'System.Text.RegularExpressions, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Text.RegularExpressions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Text.RegularExpressions.dll'
------------------
Resolve: 'System.Data.Common, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Data.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Data.Common.dll'
------------------
Resolve: 'System.Xml.XmlSerializer, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.XmlSerializer, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Xml.XmlSerializer.dll'
------------------
Resolve: 'System.Collections.Specialized, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Collections.Specialized, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Collections.Specialized.dll'
------------------
Resolve: 'System.ComponentModel.Annotations, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Annotations, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ComponentModel.Annotations.dll'
------------------
Resolve: 'System.Net.WebClient, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.WebClient, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Net.WebClient.dll'
------------------
Resolve: 'System.Net.Primitives, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Net.Primitives.dll'
------------------
Resolve: 'System.Configuration.ConfigurationManager, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Configuration.ConfigurationManager, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Configuration.ConfigurationManager.dll'
------------------
Resolve: 'System.Printing, Version=6.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'System.Printing, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Printing.dll'
------------------
Resolve: 'System.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Linq.dll'
------------------
Resolve: 'System.Threading, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Threading, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Threading.dll'
------------------
Resolve: 'System.Diagnostics.Process, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.Process, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Diagnostics.Process.dll'
------------------
Resolve: 'System.Linq.Queryable, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Linq.Queryable, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Linq.Queryable.dll'
------------------
Resolve: 'System.Console, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Console, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Console.dll'
------------------
Resolve: 'System.Xml.XPath, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.XPath, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Xml.XPath.dll'
------------------
Resolve: 'System.Net.WebHeaderCollection, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Net.WebHeaderCollection, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Net.WebHeaderCollection.dll'
------------------
Resolve: 'System.Net.WebProxy, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Net.WebProxy, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Net.WebProxy.dll'
------------------
Resolve: 'System.Diagnostics.TraceSource, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Diagnostics.TraceSource, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Diagnostics.TraceSource.dll'
------------------
Resolve: 'System.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Xaml.dll'
------------------
Resolve: 'WindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'WindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\WindowsBase.dll'
------------------
Resolve: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ObjectModel, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ObjectModel.dll'
------------------
Resolve: 'System.Windows.Extensions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Windows.Extensions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Windows.Extensions.dll'
------------------
Resolve: 'System.Security.Permissions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Security.Permissions, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Security.Permissions.dll'
------------------
Resolve: 'System.IO.Packaging, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.IO.Packaging, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.IO.Packaging.dll'
------------------
Resolve: 'System.ComponentModel.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.ComponentModel.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.ComponentModel.Primitives.dll'
------------------
Resolve: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Runtime, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Runtime.dll'
------------------
Resolve: 'UIAutomationTypes, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Found single assembly: 'UIAutomationTypes, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\UIAutomationTypes.dll'
------------------
Resolve: 'System.Xml.ReaderWriter, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Xml.ReaderWriter, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Xml.ReaderWriter.dll'
------------------
Resolve: 'System.Drawing.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Found single assembly: 'System.Drawing.Common, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.WindowsDesktop.App.Ref\8.0.8\ref\net8.0\System.Drawing.Common.dll'
------------------
Resolve: 'System.Security.AccessControl, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Security.AccessControl, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Security.AccessControl.dll'
------------------
Resolve: 'System.Drawing.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Found single assembly: 'System.Drawing.Primitives, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Drawing.Primitives.dll'
------------------
Resolve: 'System.Runtime.InteropServices, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.InteropServices, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Runtime.InteropServices.dll'
------------------
Resolve: 'System.Runtime.CompilerServices.Unsafe, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null'
Found single assembly: 'System.Runtime.CompilerServices.Unsafe, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'
WARN: Version mismatch. Expected: '6.0.0.0', Got: '8.0.0.0'
Load from: 'C:\Program Files\dotnet\packs\Microsoft.NETCore.App.Ref\8.0.8\ref\net8.0\System.Runtime.CompilerServices.Unsafe.dll'
#endif
