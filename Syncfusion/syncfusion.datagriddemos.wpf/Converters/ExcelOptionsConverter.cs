﻿using System.Windows;

namespace syncfusion.datagriddemos.wpf
{
    public class ExcelExportingOptionsWrapper : DependencyObject
    {

        public bool CanCustomizeStyle
        {
            get { return (bool)GetValue(CanCustomizeStyleProperty); }
            set { SetValue(CanCustomizeStyleProperty, value); }
        }

        public static readonly DependencyProperty CanCustomizeStyleProperty =
            DependencyProperty.Register("CanCustomizeStyle", typeof(bool), typeof(ExcelExportingOptionsWrapper), new PropertyMetadata(false));


        public bool AllowOutlining
        {
            get { return (bool)GetValue(AllowOutliningProperty); }
            set { SetValue(AllowOutliningProperty, value); }
        }

        public static readonly DependencyProperty AllowOutliningProperty =
            DependencyProperty.Register("AllowOutlining", typeof(bool), typeof(ExcelExportingOptionsWrapper), new PropertyMetadata(true));



    }
}
