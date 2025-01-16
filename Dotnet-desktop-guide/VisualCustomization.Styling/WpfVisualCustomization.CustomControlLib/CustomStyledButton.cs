using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfVisualCustomization.CustomControlLib
{
    public class CustomStyledButton : Button
    {
        static CustomStyledButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomStyledButton), new FrameworkPropertyMetadata(typeof(CustomStyledButton)));
        }
    }
}
