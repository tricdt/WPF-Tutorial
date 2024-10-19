using Syncfusion.Windows.Shared;

namespace syncfusion.datagriddemos.wpf
{
    /// <summary>
    /// Interaction logic for ColumnChooserDemo.xaml
    /// </summary>
    public partial class ColumnChooserDemo : ChromelessWindow
    {
        public ColumnChooserDemo()
        {
            InitializeComponent();
            this.Closed += ColumnChooserDemo_Closed;
        }

        private void ColumnChooserDemo_Closed(object? sender, EventArgs e)
        {
            if (this.dataGrid != null)
            {
                this.dataGrid.Dispose();
                this.dataGrid = null;
            }

            this.Closed -= ColumnChooserDemo_Closed;
        }
    }
}
