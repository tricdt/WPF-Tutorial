﻿using syncfusion.demoscommon.wpf;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for CustomPrintingDemo.xaml
    /// </summary>
    public partial class CustomPrintingDemo : DemoControl
    {
        public CustomPrintingDemo(string themename) : base(themename)
        {
            InitializeComponent();
        }
        protected override void Dispose(bool disposing)
        {
            this.Resources.Clear();

            //Release all managed resources
            if (this.syncgrid != null)
            {
                this.syncgrid.Dispose();
                this.syncgrid = null;
            }

            if (this.DataContext != null)
                this.DataContext = null;

            if (this.AllowPrintByDrawingCkb != null)
                this.AllowPrintByDrawingCkb = null;

            if (this.button != null)
                this.button = null;

            base.Dispose(disposing);
        }
    }
}
