using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for CustomDataTemplateCell.xaml
    /// </summary>
    public partial class CustomDataTemplateCell : DemoControl
    {
        EmployeesSource employeesSource = new EmployeesSource();
        public CustomDataTemplateCell()
        {
            InitializeComponent();

            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;
            grid.Model.CellModels.Add("DataTemplate", new DataTemplateCellModel());
            grid.Model.QueryCellInfo += new GridQueryCellInfoEventHandler(Model_QueryCellInfo);
            grid.Model.ColumnWidths[2] = 250;
        }



        public CustomDataTemplateCell(string themename) : base(themename)
        {
            InitializeComponent();

            grid.Model.RowCount = 35;
            grid.Model.ColumnCount = 25;
            grid.Model.CellModels.Add("DataTemplate", new DataTemplateCellModel());
            grid.Model.QueryCellInfo += new GridQueryCellInfoEventHandler(Model_QueryCellInfo);
            grid.Model.ColumnWidths[2] = 250;
        }

        private void Model_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Cell.RowIndex > 1 && e.Cell.ColumnIndex == 2)
            {
                e.Style.CellType = "DataTemplate";
                e.Style.CellItemTemplateKey = "integerEdit";
                e.Style.Background = Brushes.Linen;

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
    public class Led
    {
        public int pwm { get; set; }
    }
}
