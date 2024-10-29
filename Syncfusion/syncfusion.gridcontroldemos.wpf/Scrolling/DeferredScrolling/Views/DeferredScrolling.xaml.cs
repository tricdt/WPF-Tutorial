using syncfusion.demoscommon.wpf;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for DeferredScrolling.xaml
    /// </summary>
    public partial class DeferredScrolling : DemoControl
    {
        public DeferredScrolling()
        {
            InitializeComponent();
            GridSettings();
        }
        public DeferredScrolling(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }
        void GridSettings()
        {
            gridControl1.Model.ColumnCount = 400;
            gridControl1.Model.RowCount = 400;
            this.gridControl1.Model.Options.ExcelLikeCurrentCell = true;
            this.gridControl1.Model.Options.ExcelLikeSelectionFrame = true;

            Random r = new Random();

            for (int row = 1; row < 400; row++)
            {
                for (int col = 1; col < 400; col++)
                {
                    if (r.Next(100) > 60)
                        gridControl1.Model[row, col].Text = r.Next(5000, 10000).ToString();
                }
            }
        }

        private void Deferered_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (ck.IsChecked == true)
            {
                this.scrollViewer.IsDeferredScrollingEnabled = true;
            }
            else
            {
                this.scrollViewer.IsDeferredScrollingEnabled = false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.gridControl1 != null)
            {
                this.gridControl1.Dispose();
                this.gridControl1 = null;
            }
            base.Dispose(disposing);
        }
    }
}
