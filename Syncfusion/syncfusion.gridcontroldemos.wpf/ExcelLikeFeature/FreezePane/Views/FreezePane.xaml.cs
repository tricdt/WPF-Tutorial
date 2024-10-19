using syncfusion.demoscommon.wpf;
using System.Windows;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for FreezePane.xaml
    /// </summary>
    public partial class FreezePane : DemoControl, IFreezePanel
    {
        public FreezePane()
        {
            InitializeComponent();
            this.DataContext = new FreezePanelViewModel(this);
        }

        public FreezePane(string themename) : base(themename)
        {
            InitializeComponent();
            this.DataContext = new FreezePanelViewModel(this);
        }

        public void Initialize()
        {
            this.grid.Model.Options.ExcelLikeFreezePane = true;
            this.grid.AllowDragColumns = true;
            this.grid.Model.RowCount = 50;
            this.grid.Model.ColumnCount = 50;
            for (int i = 1; i < 50; i++)
            {
                for (int j = 1; j < 50; j++)
                {
                    this.grid.Model[i, j].CellValue = string.Format("Row{0} Col{1}", i, j);
                }
            }
        }
        bool frozen = false;
        public void SetFreezePane()
        {
            if (this.grid.CurrentCell.RowIndex == -1)
            {
                MessageBox.Show("Select any Cell");
                return;
            }

            frozen = !frozen;

            if (frozen)
            {
                this.grid.Model.FrozenRows = this.grid.CurrentCell.RowIndex;
                this.grid.Model.FrozenColumns = this.grid.CurrentCell.ColumnIndex;
                this.button1.Content = this.button1.Content.ToString().Replace("Freeze", "Unfreeze");
            }
            else
            {
                this.grid.Model.FrozenRows = 1;
                this.grid.Model.FrozenColumns = 1;
                this.button1.Content = this.button1.Content.ToString().Replace("Unfreeze", "Freeze");
            }
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
