﻿using System.Windows;
using System.Windows.Controls;
namespace CustomControl.Controls.Clock
{
    public class AnalogClock : Control
    {
        static AnalogClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));
        }
    }
}
