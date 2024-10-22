using Syncfusion.Windows.Controls.Grid;
using System.Windows.Controls;

namespace syncfusion.gridcontroldemos.wpf
{
    public class CustomeDropDown : GridCellDropDownControlBase
    {
        public ImageTextListBox ListBoxPart
        {
            get
            {
                if (this.PopupContent != null)
                {
                    return this.PopupContent.Content as ImageTextListBox;
                }
                return null;
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnContentLoaded(ContentControl popupContent)
        {
            base.OnContentLoaded(popupContent);
            ImageTextListBox l = new ImageTextListBox(this);
            l.Height = 200;
            popupContent.Content = l;
        }
    }
}