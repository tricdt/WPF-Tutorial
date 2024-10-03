using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Tools.Controls;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    public class CopyPasteBehaviour : Behavior<CutCopyPasteDemo>
    {
        SfDataGrid SfDataGrid;
        ComboBoxAdv CopyCombobox, PasteCombobox;
        TextBox Clipboardtextbox;
        protected override void OnAttached()
        {
            var window = this.AssociatedObject;
            this.SfDataGrid = window.FindName("datagrid") as SfDataGrid;
            this.CopyCombobox = window.FindName("CopyOptionComboBox") as ComboBoxAdv;
            this.PasteCombobox = window.FindName("PasteOptionComboBox") as ComboBoxAdv;
            this.Clipboardtextbox = window.FindName("Clipboardcontent") as TextBox;
            this.SfDataGrid.GridCopyContent += SfDataGrid_GridCopyContent;
            this.CopyCombobox.SelectionChanged += CopyCombobox_SelectionChanged;
            this.PasteCombobox.SelectionChanged += PasteCombobox_SelectionChanged;
        }

        private void PasteCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var data = (sender as ComboBoxAdv);
            if (data.SelectedItems != null)
            {
                var selecteditems = data.SelectedItems.Cast<GridPasteOption>().ToList();
                List<string> selecteddata = new List<string>();
                for (int i = 0; i < selecteditems.Count; i++)
                {
                    if (i == 0)
                        this.SfDataGrid.GridPasteOption = selecteditems[i];
                    else
                        this.SfDataGrid.GridPasteOption = this.SfDataGrid.GridPasteOption | selecteditems[i];
                }
            }
        }

        private void CopyCombobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var data = (sender as ComboBoxAdv);
            if (data.SelectedItems != null)
            {
                var selecteditems = data.SelectedItems.Cast<GridCopyOption>().ToList();
                for (int i = 0; i < selecteditems.Count; i++)
                {
                    if (i == 0)
                        this.SfDataGrid.GridCopyOption = selecteditems[i];
                    else
                        this.SfDataGrid.GridCopyOption = this.SfDataGrid.GridCopyOption | selecteditems[i];
                }
            }
        }

        private void SfDataGrid_GridCopyContent(object? sender, GridCopyPasteEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Clipboardtextbox.Text = Clipboard.GetText();
            }));
        }
    }
}
