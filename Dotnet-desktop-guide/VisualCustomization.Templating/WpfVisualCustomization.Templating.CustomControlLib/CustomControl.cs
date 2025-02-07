using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfVisualCustomization.Templating.CustomControlLib
{
    public class CustomControl : TextBox
    {
        public CustomControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl), new FrameworkPropertyMetadata(typeof(CustomControl)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Message from Button_Click hoping for the best",
                "Message",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
