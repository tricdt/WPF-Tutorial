using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for Zooming.xaml
    /// </summary>
    public partial class Zooming : DemoControl
    {
        public Zooming()
        {
            InitializeComponent();
            InitializeGrid();
        }
        public Zooming(string themename) : base(themename)
        {
            InitializeComponent();
            InitializeGrid();
        }
        private void InitializeGrid()
        {
            this.DataContext = Orders1.LoadOrders();
            grid.Model.RowCount = (this.DataContext as IEnumerable<Order>).Count() + 1;
            grid.Model.ColumnCount = 14;
            grid.Model.Options.ActivateCurrentCellBehavior = GridCellActivateAction.DblClickOnCell;
        }

        protected override void Dispose(bool disposing)
        {
            if (this.grid != null)
            {
                this.grid.Dispose();
                this.grid = null;
            }
            base.Dispose(disposing);
        }
    }
}
