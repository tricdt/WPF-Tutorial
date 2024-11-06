﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Scroll;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for NestedGrid.xaml
    /// </summary>
    public partial class NestedGrid : DemoControl
    {
        public NestedGrid()
        {
            InitializeComponent();
        }

        public NestedGrid(string themename) : base(themename)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.grid.MouseControllerDispatcher.TrackMouse = this.grid.GetClipRect(ScrollAxisRegion.Body, ScrollAxisRegion.Body);
        }
        protected override void Dispose(bool disposing)
        {
            if (this.grid.Model != null)
            {
                this.grid.Model.Dispose();
                this.grid.Model = null;
            }
            if (this.grid != null)
            {
                this.grid.Dispose();
                this.grid = null;
            }
            base.Dispose(disposing);
        }
    }
}
