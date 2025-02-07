using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfVisualCustomization.Templating.CustomControlLib
{
    public class CustomContentControl : ContentControl
    {
        public CustomContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomContentControl), new FrameworkPropertyMetadata(typeof(CustomContentControl)));
        }
    }
}
