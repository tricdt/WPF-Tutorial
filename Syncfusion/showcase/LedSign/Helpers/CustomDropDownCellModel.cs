using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using Syncfusion.Windows.Shared;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LedSign
{
    //public class CustomDropDownCellModel : GridCellDropDownCellModel<CustomDropDownRenderer>
    //{
    //}

    //public class CustomDropDownRenderer : GridCellDropDownCellRenderer<CustomDropDown>
    //{
    //}

    //public class CustomDropDown : GridCellDropDownControlBase
    //{
    //}


    public class CustomIntegerEditCellModel : GridCellNumericEditCellModel<CustomIntegerEditCellRenderer>
    {
        protected string GetNumber(string text, NumberFormatInfo numberFormat)
        {
            string text2 = string.Empty;
            int num = text.Length;
            if (text.Contains("."))
            {
                num = text.IndexOf(".");
            }

            for (int i = 0; i < num; i++)
            {
                char c = text[i];
                if (char.IsDigit(c) || numberFormat.NegativeSign.Contains(c.ToString()))
                {
                    text2 += c;
                }
            }

            return text2;
        }

        //
        // Summary:
        //     Return formatted text for the specified value.
        //
        // Parameters:
        //   style:
        //     The Syncfusion.Windows.Controls.Grid.GridStyleInfo object that holds cell information.
        //
        //
        //   value:
        //     The value to format.
        //
        //   textInfo:
        //     TextInfo is a hint of who is calling, default is GridCellBaseTextInfo.DisplayText.
        //
        //
        // Returns:
        //     The formatted text for the given value.
        //protected override string GetFormattedText(string text, NumberFormatInfo numberFormatInfo)
        //{
        //    NumberFormatInfo numberFormatInfo2 = numberFormatInfo.Clone() as NumberFormatInfo;
        //    numberFormatInfo2.NumberDecimalDigits = 0;
        //    text = GetNumber(text, numberFormatInfo2);
        //    _ = string.Empty;
        //    if (long.TryParse(text, out var result))
        //    {
        //        return result.ToString("N", numberFormatInfo2);
        //    }

        //    return "0";
        //}

        //public override string GetFormattedText(GridStyleInfo style, object value, int textInfo)
        //{
        //    string text = GetText(style, value);
        //    NumberFormatInfo numberFormatInfo = (style.HasNumberFormat ? style.NumberFormat : style.GetCulture(useCurrentCultureIfNull: false).NumberFormat);
        //    numberFormatInfo = ((style.NumberFormat == null || style.NumberFormat == style.GetCulture(useCurrentCultureIfNull: false).NumberFormat) ? style.GetCulture(useCurrentCultureIfNull: false).NumberFormat : style.NumberFormat);
        //    NumberFormatInfo numberFormatInfo2 = numberFormatInfo.Clone() as NumberFormatInfo;
        //    numberFormatInfo2.NumberDecimalDigits = 0;
        //    if (style.IntegerEdit != null)
        //    {
        //        numberFormatInfo2.NumberGroupSeparator = ((!style.IntegerEdit.HasGroupSeperatorEnabled) ? numberFormatInfo.NumberGroupSeparator : (style.IntegerEdit.GroupSeperatorEnabled ? numberFormatInfo.NumberGroupSeparator : string.Empty));
        //    }

        //    text = GetNumber(text, numberFormatInfo2);
        //    string empty = string.Empty;
        //    if (long.TryParse(text, out var result))
        //    {
        //        return empty = result.ToString("N", numberFormatInfo2);
        //    }

        //    if (style.IntegerEdit.UseNullOption)
        //    {
        //        return empty;
        //    }

        //    return "0";
        //}

        //
        // Summary:
        //     Parses the display text and converts it into a cell value to be stored in the
        //     style object.
        //
        // Parameters:
        //   style:
        //     Style information for the cell.
        //
        //   text:
        //     The input text to be parsed.
        //
        // Returns:
        //     True if value was parsed correctly and saved in style object as Syncfusion.Windows.Controls.Grid.GridStyleInfo.CellValue;
        //     False otherwise.
        //protected override object ApplyFormattedValue(GridStyleInfo style, string text)
        //{
        //    if (style.NumberFormat != null)
        //    {
        //        style.NumberFormat.NumberDecimalDigits = 0;
        //    }

        //    text = GetNumber(text, style.NumberFormat);
        //    long result = long.MinValue;
        //    if (long.TryParse(text, out result))
        //    {
        //        return result;
        //    }

        //    return base.ApplyFormattedValue(style, text);
        //}
    }


    public class CustomIntegerEditCellRenderer : GridVirtualizingCellRenderer<CustomIntegerTextBox>
    {
        private int ccSelectionStart;

        private int ccSelectionLength;

        private int textSelectionStart = -1;

        private int textSelectionLength;

        public CustomIntegerEditCellRenderer()
        {
            base.SupportsRenderOptimization = true;
            base.AllowRecycle = true;
            base.IsControlTextShown = true;
            base.IsFocusable = true;
            base.AllowKeepAliveOnlyCurrentCell = true;
        }

        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo style)
        {
            if (rca.CellUIElements != null)
            {
                return;
            }
            dc.DrawLine(new Pen(Brushes.Red, 10), new Point(0, 0), new Point(10, 10));
            Thickness defaultMargin = style.TextMargins.ToThickness();
            defaultMargin = ((!style.HasImageIndex) ? style.ErrorInfo.AdjustErrorInfoMargin(defaultMargin, rca.CellRect.Size) : style.AdjustImageWidthAndHeightToMargin(defaultMargin, rca.CellRect.Size));
            Rect rect = rca.SubtractBorderMargins(rca.CellRect, defaultMargin);
            defaultMargin.Left = Math.Max(defaultMargin.Left, 2.0);
            defaultMargin.Right = Math.Max(defaultMargin.Right, 2.0);
            rect = rca.SubtractBorderMargins(rca.CellRect, defaultMargin);
            if (!rect.IsEmpty)
            {
                string empty = string.Empty;
                empty = ((!IsCurrentCell(style) || !base.HasControlText) ? GetControlText(style) : ControlText);
                if (IsNegativeValue(style.CellValue))
                {
                    style.Foreground = (style.HasNegativeForeground ? style.NegativeForeground : style.Foreground);
                }
                //dc.DrawRectangle(Brushes.Red, new Pen(), rect);
                //dc.DrawLine(new Pen(Brushes.Red, 20), new Point(0, 0), new Point(100, 100));
                //GridTextBoxPaint.DrawText(dc, rect, empty, style);
                Int16 pwm16 = Convert.ToInt16(style.CellValue.ToString());
                Byte pwm = Convert.ToByte(pwm16 * 16);
                dc.DrawEllipse(new SolidColorBrush(Color.FromArgb(pwm, 255, 0, 0)), new Pen(), new Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2), rect.Width / 2, rect.Width / 2);
                GridTextBoxPaint.DrawText(dc, rect, empty, style);
            }
        }

        protected virtual bool IsNegativeValue(object cellValue)
        {
            long result = long.MinValue;
            if (cellValue != null && long.TryParse(cellValue.ToString(), out result) && result < 0)
            {
                return true;
            }

            return false;
        }

        public override void OnInitializeContent(CustomIntegerTextBox uiElement, GridRenderStyleInfo style)
        {
            base.OnInitializeContent(uiElement, style);
            OnUnwireUIElement(uiElement);
            Thickness defaultMargin = style.TextMargins.ToThickness();
            defaultMargin.Left = Math.Max(0.0, defaultMargin.Left - 2.0);
            defaultMargin.Right = Math.Max(0.0, defaultMargin.Right - 2.0);
            defaultMargin = ((!style.HasImageIndex) ? style.ErrorInfo.AdjustErrorInfoMargin(defaultMargin, style.GridControl, style.CellRowColumnIndex) : style.AdjustImageWidthAndHeightToMargin(defaultMargin, style.GridControl));
            uiElement.Padding = defaultMargin;
            uiElement.BorderThickness = new Thickness(0.0);
            GridIntegerEditStyleInfo integerEdit = style.IntegerEdit;
            uiElement.IsScrollingOnCircle = (integerEdit.HasIsScrollingOnCircle ? integerEdit.IsScrollingOnCircle : uiElement.IsScrollingOnCircle);
            NumberFormatInfo numberFormatInfo = (style.HasNumberFormat ? style.NumberFormat : style.GetCulture(useCurrentCultureIfNull: false).NumberFormat);
            uiElement.GroupSeperatorEnabled = (integerEdit.HasGroupSeperatorEnabled ? integerEdit.GroupSeperatorEnabled : uiElement.GroupSeperatorEnabled);
            uiElement.NumberGroupSeparator = ((!integerEdit.HasGroupSeperatorEnabled) ? numberFormatInfo.NumberGroupSeparator : (integerEdit.GroupSeperatorEnabled ? numberFormatInfo.NumberGroupSeparator : string.Empty));
            uiElement.NumberGroupSizes = new Int32Collection(numberFormatInfo.NumberGroupSizes.ToList());
            uiElement.Background = Brushes.Red;
            uiElement.Foreground = Brushes.WhiteSmoke;
            uiElement.PositiveForeground = Brushes.Black;
            uiElement.MaxLength = style.MaxLength;
            GridIntegerEditStyleInfo integerEdit2 = style.IntegerEdit;
            uiElement.IsScrollingOnCircle = (integerEdit2.HasIsScrollingOnCircle ? integerEdit2.IsScrollingOnCircle : uiElement.IsScrollingOnCircle);
            uiElement.EnableFocusColors = false;
            uiElement.MinValidation = (integerEdit2.HasMinValidation ? integerEdit2.MinValidation : uiElement.MinValidation);
            uiElement.MaxValidation = (integerEdit2.HasMaxValidation ? integerEdit2.MaxValidation : uiElement.MaxValidation);
            uiElement.UseNullOption = integerEdit2.UseNullOption;
            uiElement.NullValue = integerEdit2.NullValue;
            uiElement.MinValue = (integerEdit2.HasMinValue ? integerEdit2.MinValue : uiElement.MinValue);
            uiElement.MaxValue = (integerEdit2.HasMaxValue ? integerEdit2.MaxValue : uiElement.MaxValue);
            long integerValue = GetIntegerValue(style);
            if (integerValue != long.MinValue)
            {
                uiElement.Value = integerValue;
            }
            else
            {
                uiElement.UseNullOption = true;
                uiElement.Value = null;
                uiElement.Text = string.Empty;
            }

            if (style.FlowDirection == FlowDirection.RightToLeft)
            {
                uiElement.FlowDirection = style.FlowDirection;
                uiElement.TextWrapping = TextWrapping.NoWrap;
                double m = -1.0;
                double m2 = 1.0;
                double width = uiElement.Width;
                double offsetY = 0.0;
                uiElement.LayoutTransform = new MatrixTransform(m, 0.0, 0.0, m2, width, offsetY);
            }
            else
            {
                uiElement.LayoutTransform = Transform.Identity;
            }

            OnWireUIElement(uiElement);
        }

        protected override void ArrangeUIElement(ArrangeCellArgs aca, CustomIntegerTextBox uiElement, GridRenderStyleInfo style)
        {
            Thickness defaultMargin = style.TextMargins.ToThickness();
            defaultMargin = style.ErrorInfo.AdjustErrorInfoMarginOnEditing(defaultMargin, style.GridControl, style.CellRowColumnIndex);
            uiElement.Padding = defaultMargin;
            base.ArrangeUIElement(aca, uiElement, style);
            GridIntegerEditStyleInfo integerEdit = style.IntegerEdit;
            NumberFormatInfo numberFormatInfo = (style.HasNumberFormat ? style.NumberFormat : style.GetCulture(useCurrentCultureIfNull: false).NumberFormat);
            uiElement.NumberGroupSeparator = ((!integerEdit.HasGroupSeperatorEnabled) ? numberFormatInfo.NumberGroupSeparator : (integerEdit.GroupSeperatorEnabled ? numberFormatInfo.NumberGroupSeparator : string.Empty));
            uiElement.NumberGroupSizes = new Int32Collection(numberFormatInfo.NumberGroupSizes.ToList());
            uiElement.EnableFocusColors = false;
            uiElement.IsScrollingOnCircle = (integerEdit.HasIsScrollingOnCircle ? integerEdit.IsScrollingOnCircle : uiElement.IsScrollingOnCircle);
            uiElement.CornerRadius = new CornerRadius() { BottomLeft = 10, BottomRight = 10, TopLeft = 10, TopRight = 10 };
        }


        protected override void OnInitialize()
        {
            base.OnInitialize();
            long integerValue = GetIntegerValue(base.CurrentStyle);
            if (integerValue != long.MinValue)
            {
                ControlValue = integerValue;
                return;
            }

            object cellValue = base.CurrentStyle.CellValue;
            ControlValue = cellValue;
        }

        protected override void OnEditingComplete()
        {
            base.GridControl.InvalidateCell(base.CellRowColumnIndex);
            if (base.CurrentCellUIElement != null)
            {
                base.CurrentCellUIElement.CaretIndex = 0;
            }
        }

        protected override void OnEnteredEditMode()
        {
            ApplyIntegerTextBoxProperties();
        }

        protected override void OnActivated()
        {
            ApplyIntegerTextBoxProperties();
        }

        protected virtual void ApplyIntegerTextBoxProperties()
        {
            GridRenderStyleInfo gridRenderStyleInfo = base.CurrentStyle;
            if (gridRenderStyleInfo != null)
            {
                long integerValue = GetIntegerValue(gridRenderStyleInfo);
                if (integerValue != long.MinValue && base.CurrentCellUIElement != null)
                {
                    base.CurrentCellUIElement.Value = integerValue;
                }
            }

            if (base.CurrentCellUIElement != null && base.GridControl.Model.Options.AllowTextSelectionOnReadOnly)
            {
                base.CurrentCellUIElement.IsReadOnly = gridRenderStyleInfo.ReadOnly;
            }

            if (base.CurrentCellUIElement == null)
            {
                base.GridControl.InvalidateCell(base.CellRowColumnIndex);
            }
        }

        //protected override void OnDeactivated()
        //{
        //    base.GridControl.InvalidateCell(base.CellRowColumnIndex);
        //}

        private long GetIntegerValue(GridStyleInfo style)
        {
            long result = long.MinValue;
            object obj = GetControlValue(base.CurrentStyle);
            if (obj != null && obj.ToString() != string.Empty)
            {
                long.TryParse(obj.ToString(), out result);
            }

            return result;
        }

        //protected override string GetControlTextFromEditorCore(CustomIntegerTextBox uiElement)
        //{
        //    return uiElement.Value.ToString();
        //}

        public override void CreateRendererElement(CustomIntegerTextBox uiElement, GridRenderStyleInfo style)
        {
            base.CreateRendererElement(uiElement, style);
            uiElement.Background = Brushes.Red;
        }
        //
        // Summary:
        //     Refreshes the current cell content.
        //public override void RefreshContent()
        //{
        //    base.RefreshContent();
        //    if (textSelectionStart != -1 && base.CurrentCellUIElement != null)
        //    {
        //        base.CurrentCellUIElement.SelectionStart = textSelectionStart;
        //        base.CurrentCellUIElement.SelectionLength = textSelectionLength;
        //    }
        //}

        protected override void OnWireUIElement(CustomIntegerTextBox uiElement)
        {
            base.OnWireUIElement(uiElement);
            uiElement.ValueChanged += uiElement_ValueChanged;
            uiElement.Loaded += uiElement_Loaded;
            //uiElement.PreviewKeyDown += new KeyEventHandler(OnPreviewKeyDown);
            //uiElement.KeyDown += new KeyEventHandler(OnKeyDown);
            //uiElement.MouseRightButtonUp += new MouseButtonEventHandler(uiElement_MouseRightButtonUp);
            uiElement.AddHandler(UIElement.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown), handledEventsToo: true);
            uiElement.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDown), handledEventsToo: true);
            uiElement.AddHandler(UIElement.MouseRightButtonUpEvent, new MouseButtonEventHandler(uiElement_MouseRightButtonUp), handledEventsToo: true);
        }



        private void uiElement_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.CurrentCellUIElement != null && !base.CurrentCell.IsModified)
            {
                if ((base.GridControl.Model.Options.ActivateCurrentCellBehavior & GridCellActivateAction.SelectAll) != 0)
                {
                    base.CurrentCellUIElement.SelectAll();
                    return;
                }

                base.CurrentCellUIElement.Select(base.CurrentCellUIElement.Text.Length, 0);
                base.CurrentCellUIElement.Focus();
            }
        }

        private void uiElement_ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomIntegerTextBox integerTextBox = (CustomIntegerTextBox)d;
            integerTextBox.CornerRadius = new CornerRadius() { BottomLeft = 10, BottomRight = 10 };

            integerTextBox.Background = Brushes.Red;
            if (!base.IsInArrange && IsCurrentCell(integerTextBox) && !base.CurrentCell.IsInEndEdit)
            {
                if (!integerTextBox.Value.HasValue && integerTextBox.UseNullOption)
                {
                    integerTextBox.Value = integerTextBox.NullValue;
                }
                else if (!integerTextBox.Value.HasValue)
                {
                    integerTextBox.Value = 0L;
                }

                if (!SetControlValue(integerTextBox.Value))
                {
                    RefreshContent();
                }
            }
        }

        protected override void OnUnwireUIElement(CustomIntegerTextBox uiElement)
        {
            base.OnUnwireUIElement(uiElement);
            uiElement.ValueChanged -= uiElement_ValueChanged;
            uiElement.Loaded -= uiElement_Loaded;
            //uiElement.PreviewKeyDown -= new KeyEventHandler(OnPreviewKeyDown);
            //uiElement.KeyDown -= new KeyEventHandler(OnKeyDown);
            //uiElement.MouseRightButtonUp -= new MouseButtonEventHandler(uiElement_MouseRightButtonUp);

            uiElement.RemoveHandler(UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDown));
            uiElement.RemoveHandler(UIElement.PreviewKeyDownEvent, new KeyEventHandler(OnPreviewKeyDown));
            uiElement.RemoveHandler(UIElement.MouseRightButtonUpEvent, new MouseButtonEventHandler(uiElement_MouseRightButtonUp));
        }

        private void uiElement_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (base.GridControl.Model.DisableEditorsContextMenu)
            {
                e.Handled = true;
            }
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs args)
        {
            IntegerTextBox integerTextBox = (IntegerTextBox)sender;
            Key key = args.Key;
            if ((uint)(key - 23) <= 3u)
            {
                ccSelectionLength = integerTextBox.SelectionLength;
                ccSelectionStart = integerTextBox.SelectionStart;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs args)
        {
            CustomIntegerTextBox integerTextBox = (CustomIntegerTextBox)sender;
            bool flag = (args.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0;
            Key key = args.Key;
            if ((uint)(key - 23) <= 3u && !flag && ccSelectionStart == integerTextBox.SelectionStart && ccSelectionLength == integerTextBox.SelectionLength)
            {
                base.GridControl.MoveCurrentCellWithArrowKey(args);
            }
        }

        protected override void OnGridPreviewTextInput(TextCompositionEventArgs e)
        {
            if (!base.CurrentCell.IsEditing)
            {
                base.CurrentCell.ScrollInView();
                base.CurrentCell.BeginEdit(focusCellUIElement: true);
                if (base.CurrentCellUIElement != null)
                {
                    base.CurrentCellUIElement.Text = string.Empty;
                }
            }
        }

        protected override void OnSetFocus()
        {
            if ((base.GridControl.Model.Options.ActivateCurrentCellBehavior & GridCellActivateAction.SelectAll) != 0)
            {
                base.CurrentCellUIElement.SelectAll();
                return;
            }

            base.CurrentCellUIElement.Select(base.CurrentCellUIElement.Text.Length, 0);
            base.CurrentCellUIElement.Focus();
        }

        protected override bool ShouldGridTryToHandlePreviewKeyDown(KeyEventArgs e)
        {
            bool flag = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0;
            bool flag2 = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != 0;
            if (flag && !base.CurrentCell.IsEditing)
            {
                return true;
            }

            switch (e.Key)
            {
                case Key.Tab:
                    return true;
                case Key.Left:
                    if (base.CurrentCell.IsEditing)
                    {
                        if (flag2)
                        {
                            return false;
                        }

                        if (base.CurrentCellUIElement != null)
                        {
                            if (flag)
                            {
                                base.CurrentCellUIElement.CaretIndex = 0;
                                e.Handled = true;
                                return false;
                            }

                            if (base.CurrentCellUIElement.CaretIndex == 0 && base.CurrentCellUIElement.SelectionLength == 0)
                            {
                                e.Handled = true;
                                return true;
                            }

                            if (base.CurrentCellUIElement.SelectionLength == base.CurrentCellUIElement.Text.Length)
                            {
                                base.CurrentCellUIElement.CaretIndex = 0;
                                e.Handled = true;
                                return false;
                            }

                            return false;
                        }
                    }

                    return true;
                case Key.Right:
                    if (base.CurrentCell.IsEditing)
                    {
                        if (flag2)
                        {
                            return false;
                        }

                        if (base.CurrentCellUIElement != null)
                        {
                            if (flag)
                            {
                                base.CurrentCellUIElement.CaretIndex = base.CurrentCellUIElement.Text.Length;
                                e.Handled = true;
                                return false;
                            }

                            if (base.CurrentCellUIElement.CaretIndex == 0 && base.CurrentCellUIElement.SelectionLength == 0)
                            {
                                return false;
                            }

                            if (base.CurrentCellUIElement.SelectionLength == base.CurrentCellUIElement.Text.Length)
                            {
                                base.CurrentCellUIElement.CaretIndex = base.CurrentCellUIElement.Text.Length;
                                e.Handled = true;
                                return false;
                            }

                            if (base.CurrentCellUIElement.CaretIndex == base.CurrentCellUIElement.Text.Length)
                            {
                                return true;
                            }

                            return false;
                        }
                    }

                    return true;
                case Key.End:
                case Key.Home:
                    if (base.CurrentCell.IsEditing)
                    {
                        return false;
                    }

                    return true;
                case Key.Down:
                    if (base.CurrentCell.IsEditing && base.CurrentCellUIElement != null)
                    {
                        if (flag2)
                        {
                            base.CurrentCellUIElement.Select(base.CurrentCellUIElement.CaretIndex, base.CurrentCellUIElement.Text.Length - base.CurrentCellUIElement.CaretIndex);
                            e.Handled = true;
                            return false;
                        }

                        if (flag)
                        {
                            base.CurrentCellUIElement.CaretIndex = base.CurrentCellUIElement.Text.Length;
                            e.Handled = true;
                            return false;
                        }

                        if (base.CurrentCellUIElement.IsScrollingOnCircle)
                        {
                            return false;
                        }

                        return true;
                    }

                    return true;
                case Key.Up:
                    if (base.CurrentCell.IsEditing && base.CurrentCellUIElement != null)
                    {
                        if (flag2)
                        {
                            e.Handled = true;
                            return false;
                        }

                        if (flag)
                        {
                            e.Handled = true;
                            return false;
                        }

                        if (base.CurrentCellUIElement.IsScrollingOnCircle)
                        {
                            return false;
                        }

                        return true;
                    }

                    return true;
                case Key.Delete:
                    base.CurrentCell.BeginEdit(focusCellUIElement: true);
                    return false;
                case Key.Back:
                    base.CurrentCell.BeginEdit(focusCellUIElement: true);
                    return false;
                case Key.Enter:
                    if (flag2)
                    {
                        break;
                    }

                    if (base.CurrentStyle != null)
                    {
                        IGridCellRenderer renderer = base.CurrentCell.Renderer;
                        if (renderer != null)
                        {
                            GridDataStyleInfo gridDataStyleInfo = renderer.CurrentStyle.ModelStyle as GridDataStyleInfo;
                            if (gridDataStyleInfo != null && gridDataStyleInfo.CellIdentity.TableCellType != GridDataTableCellType.AddNewRecordCell)
                            {
                                e.Handled = true;
                            }
                            else if (gridDataStyleInfo != null && gridDataStyleInfo.CellIdentity.TableCellType == GridDataTableCellType.AddNewRecordCell && !base.CurrentCell.IsEditing)
                            {
                                e.Handled = true;
                            }
                        }
                    }

                    base.CurrentCell.MoveRight();
                    return true;
            }

            return base.ShouldGridTryToHandlePreviewKeyDown(e);
        }

        //
        // Summary:
        //     Raises the GridCellClick event for the cell.
        //
        // Parameters:
        //   rowIndex:
        //     Cell row index.
        //
        //   colIndex:
        //     Cell column index.
        //
        //   e:
        //     A reference to Syncfusion.Windows.Controls.Scroll.MouseControllerEventArgs.
        public override void RaiseGridCellClick(int rowIndex, int colIndex, MouseControllerEventArgs e)
        {
            if (base.CurrentCell.HasCurrentCellAt(rowIndex, colIndex) && !base.CurrentCell.IsEditing && (base.GridControl.Model.Options.ActivateCurrentCellBehavior & GridCellActivateAction.DblClickOnCell) == 0 && base.GridControl.Model.Options.ActivateCurrentCellBehavior != 0)
            {
                base.CurrentCell.BeginEdit(focusCellUIElement: true);
            }

            base.RaiseGridCellClick(rowIndex, colIndex, e);
        }

        internal void UnRegisterUIElement(IntegerTextBox uiElement)
        {
            uiElement.ClearValue(IntegerTextBox.MinValueProperty);
            uiElement.ClearValue(IntegerTextBox.MaxValueProperty);
        }

    }

    public class CustomIntegerTextBox : IntegerTextBox
    {
        public CustomIntegerTextBox() : base()
        {
        }
    }
}
