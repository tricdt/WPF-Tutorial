using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    public class ComboBoxColumnBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
            this.AssociatedObject.SelectionChanged += OnComboBoxSelectionChanged;
        }

        private void OnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ManipulatorView manipulatorView = ManipulatorView.manipulatorView;
            if (comboBox.SelectedItem.ToString().Contains("GridUnBoundColumn"))
            {
                manipulatorView.Height = 285;
                manipulatorView.err_textblock.Visibility = Visibility.Collapsed;
                manipulatorView.mappingname_cmbbox.SelectedItem = null;
                manipulatorView.unbound_Stackpanel.Visibility = Visibility.Visible;
                manipulatorView.mappingname_cmbbox.Visibility = Visibility.Collapsed;
                manipulatorView.Mappingname_Label.Visibility = Visibility.Collapsed;
            }
            else
            {
                manipulatorView.Height = 230;
                manipulatorView.err_textblock.Visibility = Visibility.Collapsed;
                manipulatorView.unbound_Stackpanel.Visibility = Visibility.Collapsed;
                manipulatorView.mappingname_cmbbox.Visibility = Visibility.Visible;
                manipulatorView.Mappingname_Label.Visibility = Visibility.Visible;

            }
        }
        protected override void OnDetaching()
        {
            this.AssociatedObject.SelectionChanged -= OnComboBoxSelectionChanged;
        }
    }
}
