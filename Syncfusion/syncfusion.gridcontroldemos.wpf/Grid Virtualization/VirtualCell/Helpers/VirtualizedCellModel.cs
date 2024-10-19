using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    public class VirtualizedCellModel : GridCellModel<VirtualizedCellRenderer>
    {
    }

    public class VirtualizedCellRenderer : GridVirtualizingCellRenderer<TextBox>
    {
        public VirtualizedCellRenderer()
        {
            SupportsRenderOptimization = true;
            AllowRecycle = true;
            IsControlTextShown = true;
            IsFocusable = true;
        }
        protected override void OnRender(DrawingContext dc, RenderCellArgs rca, GridRenderStyleInfo cellInfo)
        {
            if (rca.CellUIElements != null)
                return;

            // Will only get hit if SupportsRenderOptimization is true, otherwise rca.CellVisuals is never null.
            string s = String.Format("Render{0}/{1}", rca.RowIndex, rca.ColumnIndex);
            GridTextBoxPaint.DrawText(dc, rca.CellRect, s, cellInfo);
        }

        public override void OnInitializeContent(TextBox uiElement, GridRenderStyleInfo style)
        {
            base.OnInitializeContent(uiElement, style);
            this.InitDefaultProperties(uiElement, style);
        }

        private void InitDefaultProperties(TextBox textBox, GridRenderStyleInfo style)
        {
            Thickness margins = style.TextMargins.ToThickness();

            // TextBoxView always has a minimum margin of 2 for left and right.
            // Margin is hardcoded below so that TextBox behavior is properly emulated.
            margins.Left = Math.Max(0, margins.Left - 2);
            margins.Right = Math.Max(0, margins.Right - 2);

            textBox.Padding = margins;
            textBox.BorderThickness = new Thickness(0);
            VirtualizingCellsControl.SetWantsMouseInput(textBox, true);

            textBox.Text = GetControlText(style);
        }

        public override void CreateRendererElement(TextBox uiElement, GridRenderStyleInfo style)
        {
            base.CreateRendererElement(uiElement, style);
            this.InitDefaultProperties(uiElement, style);
        }

        protected override string GetControlTextFromEditorCore(TextBox uiElement)
        {
            return uiElement.Text;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            ControlText = GetControlText(CurrentStyle);
        }
        protected override void OnWireUIElement(TextBox uiElement)
        {
            base.OnWireUIElement(uiElement);
            uiElement.TextChanged += new TextChangedEventHandler(UiElement_TextChanged);
        }
        protected override void OnUnwireUIElement(TextBox uiElement)
        {
            base.OnUnwireUIElement(uiElement);
            uiElement.TextChanged -= new TextChangedEventHandler(UiElement_TextChanged);
        }
        private void UiElement_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (!this.IsInArrange && IsCurrentCell(textBox))
            {
                TraceUtil.TraceCurrentMethodInfo(textBox.Text);
                if (!SetControlText(textBox.Text))
                    RefreshContent(); // reverses change.
            }
        }

        protected override void OnGridPreviewTextInput(TextCompositionEventArgs e)
        {
            CurrentCell.ScrollInView();
            CurrentCell.BeginEdit(true);
        }

        protected override bool ShouldGridTryToHandlePreviewKeyDown(KeyEventArgs e)
        {
            if (CurrentCellUIElement != null && CurrentCellUIElement.IsFocused && e.Key != Key.Escape)
                return false;

            return true;
        }
    }
}
