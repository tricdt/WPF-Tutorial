using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfVisualCustomization.CustomControlLib
{
    public class AnotherCustomStyledButton : Button
    {
        static AnotherCustomStyledButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnotherCustomStyledButton), new FrameworkPropertyMetadata(typeof(AnotherCustomStyledButton)));
        }
    }
}
