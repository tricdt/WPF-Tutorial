using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LedSign
{
    public class VirtualizedCellModel : GridCellModel<VirtualizedCellRenderer>
    {
    }

    public class VirtualizedCellRenderer : GridVirtualizingCellRenderer<ContentControl>
    {
        public VirtualizedCellRenderer()
        {
            base.SupportsRenderOptimization = true;
            base.AllowRecycle = true;
            base.IsControlTextShown = true;
            base.IsFocusable = true;
            base.AllowKeepAliveOnlyCurrentCell = true;
        }
        public override void OnInitializeContent(ContentControl uiElement, GridRenderStyleInfo style)
        {
            base.OnInitializeContent(uiElement, style);
            OnUnwireUIElement(uiElement);
            bool found = false;

            if (style.CellItemTemplateKey != null)
            {
                DataTemplate dt = (DataTemplate)style.GridControl.TryFindResource(style.CellItemTemplateKey);
                found = dt != null;

                if (found)
                    uiElement.ContentTemplate = dt;
            }

            if (!found)
                uiElement.ContentTemplate = style.CellItemTemplate;

            uiElement.Content = style.CellValue;
            OnWireUIElement(uiElement);
        }
        protected override void OnEditingComplete()
        {
            base.OnEditingComplete();
        }
        public override void CreateRendererElement(ContentControl uiElement, GridRenderStyleInfo style)
        {
            bool found = false;

            if (style.CellItemTemplateKey != null)
            {
                DataTemplate dt = (DataTemplate)style.GridControl.TryFindResource(style.CellItemTemplateKey);
                found = dt != null;
                if (found)
                    uiElement.ContentTemplate = dt;
            }

            if (!found)
                uiElement.ContentTemplate = style.CellItemTemplate;

            uiElement.Content = style.CellValue;
            base.CreateRendererElement(uiElement, style);
        }

        protected override void OnWireUIElement(ContentControl uiElement)
        {
            uiElement.PreviewKeyDown += UiElement_PreviewKeyDown;
            base.OnWireUIElement(uiElement);
        }
        protected override void OnUnwireUIElement(ContentControl uiElement)
        {
            uiElement.PreviewKeyDown -= UiElement_PreviewKeyDown;
            base.OnUnwireUIElement(uiElement);
        }
        private void UiElement_PreviewKeyDown(object sender, KeyEventArgs e)
        {
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
                case Key.Enter:
                    base.CurrentCell.MoveRight();
                    base.CurrentCell.BeginEdit(focusCellUIElement: true);
                    return false;
            }
            return base.ShouldGridTryToHandlePreviewKeyDown(e);
        }
    }
}
