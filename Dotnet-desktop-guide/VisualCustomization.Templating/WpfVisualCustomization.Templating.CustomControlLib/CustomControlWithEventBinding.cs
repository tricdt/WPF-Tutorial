using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfVisualCustomization.Templating.CustomControlLib
{
    public class CustomControlWithEventBinding : TextBox
    {
        public CustomControlWithEventBinding()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControlWithEventBinding), new FrameworkPropertyMetadata(typeof(CustomControlWithEventBinding)));
        }

        public override void OnApplyTemplate()
        {
            DependencyObject d = GetTemplateChild("PART_Button");
            if (d != null)
            {
                (d as Button).Click += new RoutedEventHandler(Button_Click);
            }

            base.OnApplyTemplate();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Message from Button_Click using PARTS",
                "Message",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
