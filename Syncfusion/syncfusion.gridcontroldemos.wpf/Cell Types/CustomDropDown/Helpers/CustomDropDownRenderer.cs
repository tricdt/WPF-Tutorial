using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Scroll;
using System.Windows.Input;

namespace syncfusion.gridcontroldemos.wpf
{
    public class CustomeDropDownCellModel : GridCellDropDownCellModel<CustomDropDownRenderer>
    {
    }

    public class CustomDropDownRenderer : GridCellDropDownCellRenderer<CustomeDropDown>
    {
        private CustomeDropDownCellModel CustomDropDownModel
        {
            get { return this.CellModel as CustomeDropDownCellModel; }
        }
        public override void OnInitializeContent(CustomeDropDown dropDownControl, GridRenderStyleInfo style)
        {
            if (dropDownControl.ListBoxPart != null)
            {
                dropDownControl.ListBoxPart.SelectionChanged -= this.OnComboBoxSelectionChanged;
            }
            base.OnInitializeContent(dropDownControl, style);

        }

        protected override void SetSelectedIndex(int index)
        {
            if (index != this.CurrentCellUIElement.ListBoxPart.SelectedIndex)
            {
                this.CurrentCellUIElement.ListBoxPart.SelectedIndex = index;
            }
        }

        protected override void ArrangeUIElement(ArrangeCellArgs aca, CustomeDropDown uiElement, GridRenderStyleInfo style)
        {
            base.ArrangeUIElement(aca, uiElement, style);
            var dropDownControl = uiElement;
            if (style.ItemsSource != null)
            {
                dropDownControl.ListBoxPart.ItemsSource = this.CustomDropDownModel.GetDataSource(style);
                dropDownControl.ListBoxPart.DisplayMemberPath = style.HasDisplayMember ? style.DisplayMember : string.Empty;
                dropDownControl.ListBoxPart.SelectedValue = this.GetControlValue(style);
                if (style.HasValueMember)
                {
                    dropDownControl.ListBoxPart.SelectedValuePath = style.ValueMember;
                }
            }
            uiElement.ListBoxPart.SelectionChanged += OnComboBoxSelectionChanged;
        }
        private void OnComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0].ToString();
                this.CustomDropDownModel.ListModel.CurrentIndex = this.CustomDropDownModel.FindValue(this.CurrentStyle, item);
                if (!this.AlreadyTextChanged)
                {
                    this.CurrentCellUIElement.TextBoxPart.Text = item;
                }
            }
        }

        public override void RaiseGridCellClick(int rowIndex, int colIndex, MouseControllerEventArgs e)
        {
            base.RaiseGridCellClick(rowIndex, colIndex, e);
            var v = this.CurrentCell.Renderer;
            if (!this.CurrentCell.IsEditing)
                this.CurrentCell.BeginEdit();
            if (v.HasCurrentCellState && v.CurrentCellUIElement != null)
            {
                this.GridControl.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ((CustomDropDownRenderer)v).CurrentCellUIElement.IsDropDownOpen = !((CustomDropDownRenderer)v).CurrentCellUIElement.IsDropDownOpen;
                }));
            }
        }

        protected override bool ShouldGridTryToHandlePreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (this.IsDroppedDown)
                    {
                        if (this.CurrentCellUIElement.ListBoxPart.SelectedIndex > 0)
                        {
                            this.CurrentCellUIElement.ListBoxPart.SelectedIndex = this.CurrentCellUIElement.ListBoxPart.SelectedIndex - 1;
                            this.CurrentCellUIElement.ListBoxPart.ScrollIntoView(this.CurrentCellUIElement.ListBoxPart.Items[this.CurrentCellUIElement.ListBoxPart.SelectedIndex]);
                            e.Handled = true;
                        }
                        return false;
                    }
                    else
                        return true;
                case Key.Down:
                    if (this.IsDroppedDown)
                    {
                        if (this.CurrentCellUIElement.ListBoxPart.SelectedIndex < this.CurrentCellUIElement.ListBoxPart.Items.Count)
                        {
                            this.CurrentCellUIElement.ListBoxPart.SelectedIndex = this.CurrentCellUIElement.ListBoxPart.SelectedIndex + 1;
                            this.CurrentCellUIElement.ListBoxPart.ScrollIntoView(this.CurrentCellUIElement.ListBoxPart.Items[this.CurrentCellUIElement.ListBoxPart.SelectedIndex]);
                            e.Handled = true;

                        }
                        return false;
                    }
                    else
                        return true;


            }
            return base.ShouldGridTryToHandlePreviewKeyDown(e);
        }
    }
}
