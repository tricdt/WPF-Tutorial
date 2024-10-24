﻿using syncfusion.demoscommon.wpf;
using Syncfusion.Windows.Controls.Grid;
using System.IO;
using System.Windows.Media;

namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for Tooltip.xaml
    /// </summary>
    public partial class Tooltip : DemoControl
    {
        public Tooltip()
        {
            InitializeComponent();
            //if (!LayoutControl.IsInDesignMode)
            this.InitGrid();
        }

        public Tooltip(string themename) : base(themename)
        {
            InitializeComponent();
            //if (!LayoutControl.IsInDesignMode)
            this.InitGrid();
        }

        private void InitGrid()
        {
            var model = this.grid.Model;
            model.RowCount = 30;
            model.ColumnCount = 12;
            model.TableStyle.CellType = "Static";

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(List<CustomersInfo>));

            List<CustomersInfo> customers;
            using (Stream reader = new FileStream(@"Data\GridControl\Customers.xml", FileMode.Open))
            {
                customers = (List<CustomersInfo>)xs.Deserialize(reader);
            }

            model[0, 1].CellValue = "CustomerID";
            model[0, 2].CellValue = "ContactName";
            model[0, 3].CellValue = "ContactTitle";
            model[0, 4].CellValue = "CompanyName";
            model[0, 5].CellValue = "City";
            model[0, 6].CellValue = "Country";
            model[0, 7].CellValue = "Address";
            model[0, 8].CellValue = "Phone";
            model[0, 9].CellValue = "ContactName";
            model[0, 10].CellValue = "Orders";
            model[0, 11].CellValue = "Fax";

            for (int i = 1; i < model.RowCount; i++)
            {
                var customer = customers[i - 1];
                model[i, 1].CellValue = customer.CustomerID;
                model[i, 3].CellValue = customer.ContactTitle;
                model[i, 4].CellValue = customer.CompanyName;
                model[i, 5].CellValue = customer.City;
                model[i, 6].CellValue = customer.Country;
                model[i, 7].CellValue = customer.Address;
                model[i, 8].CellValue = customer.Phone;
                model[i, 9].CellValue = customer.ContactName;
                model[i, 10].CellValue = customer.Fax;
                model.RowHeights[i] = 30;

                int j = 2;
                var style = model[i, j];
                style.ShowTooltip = true;
                style.CellValue = customer.ContactName;
                style.Background = new SolidColorBrush(Colors.NavajoWhite);

                style.CellValue2 = customer.Phone.Length > 0 ? customer.Phone : "None";
                style.TooltipTemplateKey = "templateTooltip";
            }

            model.ColumnWidths[0] = 30;
            model.ColumnWidths[2] = 150;
            model.ResizeColumnsToFit(GridRangeInfo.Cols(3, 5), GridResizeToFitOptions.NoShrinkSize);
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
