using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace syncfusion.gridcontroldemos.wpf
{
    public class DataTemplateCellModel : GridCellModel<DataTemplateCellRenderer>
    {
    }

    public class DataTemplateCellRenderer : GridVirtualizingCellRenderer<ContentControl>
    {
        public DataTemplateCellRenderer()
        {
            IsFocusable = true;
            AllowRecycle = true;
        }

        public override void OnInitializeContent(ContentControl uiElement, GridRenderStyleInfo style)
        {
            base.OnInitializeContent(uiElement, style);
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
        }

        public override void CreateRendererElement(ContentControl uiElement, GridRenderStyleInfo style)
        {
            base.CreateRendererElement(uiElement, style);
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
        }

        protected override bool ShouldGridTryToHandlePreviewKeyDown(KeyEventArgs e)
        {
            // return false to indicate the CurrentCellUIElement should handle the key
            // and the grid should ignore it.
            bool isControlKey = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None;
            if (isControlKey)
                return true;

            switch (e.Key)
            {


                case Key.Right:
                case Key.Left:
                case Key.Down:
                case Key.Up:
                    {
                        return !CurrentCell.IsEditing;
                    }

                case Key.End:
                case Key.Home:
                    {
                        CurrentCell.BeginEdit(true);
                        return false;
                    }

                case Key.Delete:
                    {
                        CurrentCell.BeginEdit(true);
                        return false;
                    }
                case Key.Enter:
                    {
                        if (this.CurrentCell.IsEditing)
                            CurrentCell.EndEdit();
                        CurrentCell.MoveRight();
                        return true;
                    }

                case Key.Back:
                    {
                        CurrentCell.BeginEdit(true);
                        return false;
                    }
            }

            return base.ShouldGridTryToHandlePreviewKeyDown(e);
        }

        protected override string GetControlTextFromEditorCore(ContentControl uiElement)
        {
            return uiElement.Content.ToString();
        }
    }
}
