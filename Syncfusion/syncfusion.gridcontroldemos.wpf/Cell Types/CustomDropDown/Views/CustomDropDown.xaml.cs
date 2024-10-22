using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Cells;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for CustomDropDown.xaml
    /// </summary>
    public partial class CustomDropDown : DemoControl
    {
        public CustomDropDown()
        {
            InitializeComponent();
            GridSettings();
        }

        public CustomDropDown(string themename) : base(themename)
        {
            InitializeComponent();
            GridSettings();
        }

        private void GridSettings()
        {
            this.grid.Model.RowCount = 35;
            this.grid.Model.ColumnCount = 25;

            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(1, 1, 3, 4));

            var cell = this.grid.Model[1, 1];
            cell.CellValue = "Custom Drop-down";
            cell.Foreground = Brushes.Black;
            cell.Background = Brushes.LightBlue;
            cell.Font.FontSize = 18;
            cell.HorizontalAlignment = HorizontalAlignment.Center;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Font.FontWeight = FontWeights.Bold;

            this.grid.Model.CoveredCells.Add(new CoveredCellInfo(4, 1, 5, 8));

            cell = this.grid.Model[4, 1];
            cell.CellValue = "This sample showcases how we can create custom drop downs. This sample creates a image and text display inside the drop down";
            cell.Foreground = Brushes.Black;
            cell.HorizontalAlignment = HorizontalAlignment.Center;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.Font.FontWeight = FontWeights.Bold;

            this.grid.Model.CellModels.Add("CustomDropdown", new CustomeDropDownCellModel());
            var dropdown1 = this.grid.Model[7, 2];
            dropdown1.CellType = "CustomDropdown";

            dropdown1.ItemsSource = new ListBoxContent().GenerateListBoxContent();
            dropdown1.DisplayMember = "Text";
            dropdown1.DropDownStyle = GridDropDownStyle.Exclusive;
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
