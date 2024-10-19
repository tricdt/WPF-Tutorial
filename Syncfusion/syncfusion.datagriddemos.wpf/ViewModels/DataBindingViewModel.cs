﻿using Syncfusion.Windows.Shared;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
namespace syncfusion.datagriddemos.wpf
{
    public class DataBindingViewModel : NotificationObject
    {
        public DataBindingViewModel()
        {
            SetShipCity();
            // Binding List
            this.OrdersListDetails = this.PopulateOrders(200);
            this.EmployeeDetails = this.GetEmployeeDetails(50);
        }
        private BindingList<OrderInfo> _ordersListDetails;
        Random r = new Random();
        public Dictionary<string, string[]> ShipCity = new Dictionary<string, string[]>();
        /// <summary>
        /// Gets or sets the orders details.
        /// </summary>
        /// <value>The orders details.</value>
        public BindingList<OrderInfo> OrdersListDetails
        {
            get { return _ordersListDetails; }
            set { _ordersListDetails = value; }
        }
        private DataTable _employeeDetails;

        /// <summary>
        /// Gets or sets the employee details.
        /// </summary>
        /// <value>The employee details.</value>
        public DataTable EmployeeDetails
        {
            get { return _employeeDetails; }
            set { _employeeDetails = value; }
        }
        ObservableCollection<dynamic> _dynamicOrders;

        /// <summary>
        /// Gets or sets the dynamic orders.
        /// </summary>
        /// <value>The dynamic orders.</value>
        public ObservableCollection<dynamic> DynamicOrders
        {
            get
            {
                if (_dynamicOrders == null)
                {
                    _dynamicOrders = this.PopulateData(200);
                }
                return _dynamicOrders;
            }
            set
            {
                _dynamicOrders = value;
                RaisePropertyChanged("DynamicOrders");
            }
        }
        internal ObservableCollection<dynamic> PopulateData(int count)
        {
            var collection = new ObservableCollection<dynamic>();
            for (int i = 10000; i < count + 10000; i++)
            {
                collection.Add(GetOrder(i));
            }
            return collection;
        }
        private dynamic GetOrder(int i)
        {
            var shipcountry = ShipCountry[r.Next(0, 5)];
            var shipcitycoll = ShipCity[shipcountry];
            dynamic dOrder = new ExpandoObject();

            dOrder.OrderID = i;
            dOrder.CustomerID = CustomerID[r.Next(0, 15)];
            dOrder.EmployeeID = r.Next(0, 9);
            dOrder.Freight = Math.Round(r.Next(0, 1000) + r.NextDouble(), 2);
            dOrder.ShipCountry = shipcountry;
            dOrder.ShipCity = shipcitycoll[r.Next(shipcitycoll.Length - 1)];
            return dOrder;
        }
        public DataTable GetEmployeeDetails(int count)
        {

            DataTable orderDetailTable = new DataTable("OrderDetail");

            // Define all the columns once.
            DataColumn[] cols =
                {
                    new DataColumn("EmployeeID",typeof(string)),
                    new DataColumn("EmployeeName",typeof(string)),
                    new DataColumn("Designation",typeof(string)),
                    new DataColumn("ContactNumber",typeof(string)),
                    new DataColumn("City",typeof(string)),
                    new DataColumn("Country",typeof(string)),
                };

            orderDetailTable.Columns.AddRange(cols);

            for (int i = 0; i < count; i++)
            {
                var shipcountry = ShipCountry[r.Next(0, 5)];
                var shipcitycoll = ShipCity[shipcountry];

                DataRow row1 = orderDetailTable.NewRow();
                row1["EmployeeID"] = r.Next(0, 9);
                row1["EmployeeName"] = employeeName[r.Next(0, 9)];
                row1["Designation"] = title[r.Next(0, 9)];
                row1["ContactNumber"] = 9991121234 + i;
                row1["City"] = shipcitycoll[r.Next(shipcitycoll.Length - 1)];
                row1["Country"] = shipcountry;

                orderDetailTable.Rows.Add(row1);
            }

            return orderDetailTable;
        }

        internal BindingList<OrderInfo> PopulateOrders(int count)
        {
            BindingList<OrderInfo> orderCollection = new BindingList<OrderInfo>();

            for (int i = 0; i < count; i++)
            {
                var shipcountry = ShipCountry[r.Next(0, 5)];
                var shipcitycoll = ShipCity[shipcountry];

                OrderInfo orderInfo = new OrderInfo();
                orderInfo.OrderID = 10000 + i;
                orderInfo.CustomerID = CustomerID[r.Next(0, 14)];
                orderInfo.ShipCountry = shipcountry;
                orderInfo.ShipCity = shipcitycoll[r.Next(shipcitycoll.Length - 1)];
                orderInfo.Freight = Math.Round(r.Next(0, 1000) + r.NextDouble(), 2);

                orderCollection.Add(orderInfo);
            }


            return orderCollection;
        }
        string[] ShipCountry = new string[]
        {
            "Argentina",
            "Austria",
            "Belgium",
            "Brazil",
            "Canada",
            "Denmark",
            "Finland",
            "France",
            "Germany",
            "Ireland",
            "Italy",
            "Mexico",
            "Norway",
            "Poland",
            "Portugal",
            "Spain",
            "Sweden",
            "Switzerland",
            "UK",
            "USA",
            "Venezuela"
        };


        /// <summary>
        /// Sets the ship city.
        /// </summary>
        private void SetShipCity()
        {
            string[] argentina = new string[] { "Buenos Aires" };

            string[] austria = new string[] { "Graz", "Salzburg" };

            string[] belgium = new string[] { "Bruxelles", "Charleroi" };

            string[] brazil = new string[] { "Campinas", "Resende", "Rio de Janeiro", "São Paulo" };

            string[] canada = new string[] { "Montréal", "Tsawassen", "Vancouver" };

            string[] denmark = new string[] { "Århus", "København" };

            string[] finland = new string[] { "Helsinki", "Oulu" };

            string[] france = new string[] { "Lille", "Lyon", "Marseille", "Nantes", "Paris", "Reims", "Strasbourg", "Toulouse", "Versailles" };

            string[] germany = new string[] { "Aachen", "Berlin", "Brandenburg", "Cunewalde", "Frankfurt a.M.", "Köln", "Leipzig", "Mannheim", "München", "Münster", "Stuttgart" };

            string[] ireland = new string[] { "Cork" };

            string[] italy = new string[] { "Bergamo", "Reggio Emilia", "Torino" };

            string[] mexico = new string[] { "México D.F." };

            string[] norway = new string[] { "Stavern" };

            string[] poland = new string[] { "Warszawa" };

            string[] portugal = new string[] { "Lisboa" };

            string[] spain = new string[] { "Barcelona", "Madrid", "Sevilla" };

            string[] sweden = new string[] { "Bräcke", "Luleå" };

            string[] switzerland = new string[] { "Bern", "Genève" };

            string[] uk = new string[] { "Colchester", "Hedge End", "London" };

            string[] usa = new string[] { "Albuquerque", "Anchorage", "Boise", "Butte", "Elgin", "Eugene", "Kirkland", "Lander", "Portland", "San Francisco", "Seattle", "Walla Walla" };

            string[] venezuela = new string[] { "Barquisimeto", "Caracas", "I. de Margarita", "San Cristóbal" };

            ShipCity.Add("Argentina", argentina);
            ShipCity.Add("Austria", austria);
            ShipCity.Add("Belgium", belgium);
            ShipCity.Add("Brazil", brazil);
            ShipCity.Add("Canada", canada);
            ShipCity.Add("Denmark", denmark);
            ShipCity.Add("Finland", finland);
            ShipCity.Add("France", france);
            ShipCity.Add("Germany", germany);
            ShipCity.Add("Ireland", ireland);
            ShipCity.Add("Italy", italy);
            ShipCity.Add("Mexico", mexico);
            ShipCity.Add("Norway", norway);
            ShipCity.Add("Poland", poland);
            ShipCity.Add("Portugal", portugal);
            ShipCity.Add("Spain", spain);
            ShipCity.Add("Sweden", sweden);
            ShipCity.Add("Switzerland", switzerland);
            ShipCity.Add("UK", uk);
            ShipCity.Add("USA", usa);
            ShipCity.Add("Venezuela", venezuela);

        }

        string[] CustomerID = new string[]
        {
            "ALFKI",
            "FRANS",
            "MEREP",
            "FOLKO",
            "SIMOB",
            "WARTH",
            "VAFFE",
            "FURIB",
            "SEVES",
            "LINOD",
            "RISCU",
            "PICCO",
            "BLONP",
            "WELLI",
            "FOLIG"
        };
        internal string[] employeeName = new string[]
        {
            "Sean Jacobson",
            "Phyllis Allen",
            "Marvin Allen",
            "Michael Allen",
            "Cecil Allison",
            "Oscar Alpuerto",
            "Sandra Altamirano",
            "Selena Alvarad",
            "Emilio Alvaro",
            "Maxwell Amland",
            "Mae Anderson",
            "Ramona Antrim",
            "Sabria Appelbaum",
            "Hannah Arakawa",
            "Kyley Arbelaez",
            "Tom Johnston",
            "Thomas Armstrong",
            "John Arthur",
            "Chris Ashton",
            "Teresa Atkinson",
            "John Ault",
            "Robert Avalos",
            "Stephen Ayers",
            "Phillip Bacalzo",
            "Gustavo Achong",
            "Catherine Abel",
            "Kim Abercrombie",
            "Humberto Acevedo",
            "Pilar Ackerman",
            "Frances Adams",
            "Margar Smith",
            "Carla Adams",
            "Jay Adams",
            "Ronald Adina",
            "Samuel Agcaoili",
            "James Aguilar",
            "Robert Ahlering",
            "Francois Ferrier",
            "Kim Akers",
            "Lili Alameda",
            "Amy Alberts",
            "Anna Albright",
            "Milton Albury",
            "Paul Alcorn",
            "Gregory Alderson",
            "J. Phillip Alexander",
            "Michelle Alexander",
            "Daniel Blanco",
            "Cory Booth",
            "James Bailey"
        };
        internal string[] title = new string[]
        {
            "Marketing Assistant",
            "Engineering Manager",
            "Senior Tool Designer",
            "Tool Designer",
            "Marketing Manager",
            "Production Supervisor - WC60",
            "Production Technician - WC10",
            "Design Engineer",
            "Production Technician - WC10",
            "Design Engineer",
            "Vice President of Engineering",
            "Production Technician - WC10",
            "Production Supervisor - WC50",
            "Production Technician - WC10",
            "Production Supervisor - WC60",
            "Production Technician - WC10",
            "Production Supervisor - WC60",
            "Production Technician - WC10",
            "Production Technician - WC30",
            "Production Control Manager",
            "Production Technician - WC45",
            "Production Technician - WC45",
            "Production Technician - WC30",
            "Production Supervisor - WC10",
            "Production Technician - WC20",
            "Production Technician - WC40",
            "Network Administrator",
            "Production Technician - WC50",
            "Human Resources Manager",
            "Production Technician - WC40",
            "Production Technician - WC30",
            "Production Technician - WC30",
            "Stocker",
            "Shipping and Receiving Clerk",
            "Production Technician - WC50",
            "Production Technician - WC60",
            "Production Supervisor - WC50",
            "Production Technician - WC20",
            "Production Technician - WC45",
            "Quality Assurance Supervisor",
            "Information Services Manager",
            "Production Technician - WC50",
            "Master Scheduler",
            "Production Technician - WC40",
            "Marketing Specialist",
            "Recruiter",
            "Production Technician - WC50",
            "Maintenance Supervisor",
            "Production Technician - WC30",
        };
    }
}
