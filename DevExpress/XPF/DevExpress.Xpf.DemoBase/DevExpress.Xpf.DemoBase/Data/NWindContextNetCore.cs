﻿#region Copyright (c) 2000-2024 Developer Express Inc.
/*
{*******************************************************************}
{                                                                   }
{       Developer Express .NET Component Library                    }
{                                                                   }
{                                                                   }
{       Copyright (c) 2000-2024 Developer Express Inc.              }
{       ALL RIGHTS RESERVED                                         }
{                                                                   }
{   The entire contents of this file is protected by U.S. and       }
{   International Copyright Laws. Unauthorized reproduction,        }
{   reverse-engineering, and distribution of all or any portion of  }
{   the code contained in this file is strictly prohibited and may  }
{   result in severe civil and criminal penalties and will be       }
{   prosecuted to the maximum extent possible under the law.        }
{                                                                   }
{   RESTRICTIONS                                                    }
{                                                                   }
{   THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES           }
{   ARE CONFIDENTIAL AND PROPRIETARY TRADE                          }
{   SECRETS OF DEVELOPER EXPRESS INC. THE REGISTERED DEVELOPER IS   }
{   LICENSED TO DISTRIBUTE THE PRODUCT AND ALL ACCOMPANYING .NET    }
{   CONTROLS AS PART OF AN EXECUTABLE PROGRAM ONLY.                 }
{                                                                   }
{   THE SOURCE CODE CONTAINED WITHIN THIS FILE AND ALL RELATED      }
{   FILES OR ANY PORTION OF ITS CONTENTS SHALL AT NO TIME BE        }
{   COPIED, TRANSFERRED, SOLD, DISTRIBUTED, OR OTHERWISE MADE       }
{   AVAILABLE TO OTHER INDIVIDUALS WITHOUT EXPRESS WRITTEN CONSENT  }
{   AND PERMISSION FROM DEVELOPER EXPRESS INC.                      }
{                                                                   }
{   CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON       }
{   ADDITIONAL RESTRICTIONS.                                        }
{                                                                   }
{*******************************************************************}
*/
#endregion Copyright (c) 2000-2024 Developer Express Inc.

using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DevExpress.DemoData.Models.Mapping;
using DevExpress.Internal;
using DevExpress.Mvvm;
using DbModelBuilder = Microsoft.EntityFrameworkCore.ModelBuilder;
using System.Threading.Tasks;
using DevExpress.Xpf.DemoCenterBase;
namespace DevExpress.DemoData {
	using DevExpress.Mvvm.Native;
	using Models;
	using System.Linq;
	public abstract class DataLoaderBase {
		protected void LoadIfNeed<T>(ref bool checkFlag, IQueryable<T> target) {
			if(!checkFlag) {
				target.Load();
				checkFlag = true;
			}
		}
	}
	public class NWindDataLoader : DataLoaderBase {
		NWindContext context;
		public NWindDataLoader() {
			if(!ViewModelBase.IsInDesignMode)
				context = NWindContext.Create();
		}
		public object Invoices {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Invoice>();
				return context.Invoices.OrderBy(i => i.OrderID).ToList();
			}
		}
		public object ObservableInvoices {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Invoice>();
				return new System.Collections.ObjectModel.ObservableCollection<Invoice>(context.Invoices.OrderBy(i => i.OrderID));
			}
		}
		public object ObservableInvoices200 {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Invoice>();
				return new System.Collections.ObjectModel.ObservableCollection<Invoice>(context.Invoices.Take(200).OrderBy(i => i.OrderID));
			}
		}
		public object Customers {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Customer>();
				context.Customers.Load();
				return context.Customers.Local.ToList();
			}
		}
		public object Employees {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Employee>();
				context.Employees.Load();
				var employees = context.Employees.Local.ToList();
				FillCustomers(employees);
				return employees;
			}
		}
		public object SalesPersons {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<SalesPerson>();
				context.SalesPersons.Load();
				return context.SalesPersons.Local.ToList();
			}
		}
		public object EmployeesWithOrdersAndOrderDetails {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Employee>();
				context.Employees.Include(x => x.Orders.Select(y => y.OrderDetails)).Load();
				return context.Employees.Local.ToList();
			}
		}
		public object Products {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Product>();
				context.Products.Load();
				return context.Products.Local.ToList();
			}
		}
		public object Categories {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Category>();
				context.Categories.Load();
				return context.Categories.Local.ToList();
			}
		}
		public object OrderDetails {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<OrderDetail>();
				context.OrderDetails.Load();
				return context.OrderDetails.Local.ToList();
			}
		}
		public object OrderDetailsExtended {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<OrderDetailsExtended>();
				context.OrderDetailsExtended.Load();
				return context.OrderDetailsExtended.Local.ToList();
			}
		}
		public object Orders {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<Order>();
				context.Orders.Load();
				return context.Orders.Local.ToList();
			}
		}
		public static List<string> Titles {
			get {
				if(ViewModelBase.IsInDesignMode)
					return new List<string>();
				return NWindContext.Create().Employees.Select(e => e.Title).Distinct().ToList();
			}
		}
		public static string[] Countries {
			get {
				if(ViewModelBase.IsInDesignMode)
					return EmptyArray<string>.Instance;
				return NWindContext.Create().CountriesArray;
			}
		}
		public object ProductReports {
			get {
				if(ViewModelBase.IsInDesignMode)
					return EmptyArray<ProductReport>.Instance;
				context.ProductReports.Load();
				return context.ProductReports.Local.ToList();
			}
		}
		public object OrderReports {
			get {
				if(ViewModelBase.IsInDesignMode)
					return EmptyArray<OrderReport>.Instance;
				context.OrderReports.Load();
				return context.OrderReports.Local.ToList();
			}
		}
		public object CustomerReports {
			get {
				if(ViewModelBase.IsInDesignMode)
					return EmptyArray<CustomerReport>.Instance;
				context.CustomerReports.Load();
				return context.CustomerReports.Local.ToList();
			}
		}
		void FillCustomers(IList<Employee> employees) {
			var customers = (List<Customer>)Customers;
			int customersCount = customers.Count / employees.Count;
			foreach (var employee in employees) {
				var index = employees.IndexOf(employee);
				employee.Customers = customers.GetRange(index * customersCount, customersCount);
			}
		}
	}
	public static class NWindDataProvider {
		public static IList<Employee> Employees {
			get { return (IList<Employee>)new NWindDataLoader().Employees; }
		}
		public static IList<Customer> Customers {
			get { return (IList<Customer>)new NWindDataLoader().Customers; }
		}
		public static IList<Invoice> Invoices {
			get { return (IList<Invoice>)new NWindDataLoader().Invoices; }
		}
		public static IList<Invoice> InvoicesUpToDate {
			get {
				var invoices = Invoices;
				var correction = (DateTime.Today - invoices.Select(x => x.OrderDate).Max()).Value.Days;
				foreach(var invoice in invoices) {
					invoice.OrderDate = invoice.OrderDate.Value.AddDays(correction);
				}
				return invoices;
			}
		}
		public static IList<Product> Products {
			get { return (IList<Product>)new NWindDataLoader().Products; }
		}
		public static IList<SalesPerson> SalesPersons {
			get { return (IList<SalesPerson>)new NWindDataLoader().SalesPersons; }
		}
		public static IList<ProductReport> ProductReports {
			get { return (IList<ProductReport>)new NWindDataLoader().ProductReports; }
		}
		public static IList<OrderReport> OrderReports {
			get { return (IList<OrderReport>)new NWindDataLoader().OrderReports; }
		}
		public static IList<CustomerReport> CustomerReports {
			get { return (IList<CustomerReport>)new NWindDataLoader().CustomerReports; }
		}
		public static System.Collections.ObjectModel.ObservableCollection<Invoice> ObservableInvoices200 {
			get { return (System.Collections.ObjectModel.ObservableCollection<Invoice>)new NWindDataLoader().ObservableInvoices200; }
		}
	}
}
namespace DevExpress.DemoData.Models {
	static class DbContextPreloader<T> where T : DbContext, new() {
		static System.Threading.Tasks.Task task;
		static DbContextPreloader() {
			Action action = null;
			if(ViewModelBase.IsInDesignMode) {
				action = () => { };
			} else {
				action = () => {
					var context = new T();
					var prop = typeof(T).GetProperties()
						.Where(p => p.PropertyType.IsGenericType &&
									p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
						.FirstOrDefault();
					if(prop == null)
						return;
					var query = (IQueryable<object>)prop.GetValue(context, null);
					query.Count();
				};
			}
			task = new TaskFactory().StartNew(action);
		}
		public static Task Preload() {
			return task;
		}
	}
	public partial class NWindContext : DbContext {
		public NWindContext() : base() {
			SetFilePath();
			if(filePath != DemoRunner.DBFileFailedString)
				connectionString = string.Format("Data Source={0}", filePath);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseLazyLoadingProxies().UseSqlite(connectionString);
		}
		string connectionString = "";
		public override int SaveChanges() {
			throw new Exception("Readonly context");
		}
		public static Task Preload() {
			return DbContextPreloader<NWindContext>.Preload();
		}
		static NWindContext Context { get; set; }
		public static NWindContext Create() {
			return Context ?? (Context = new NWindContext());
		}
		static string filePath;
		static void SetFilePath() {
			if(filePath == null)
				filePath = DemoRunner.GetDBFileSafe("nwind.db");
			try {
				var attributes = File.GetAttributes(filePath);
				if(attributes.HasFlag(FileAttributes.ReadOnly)) {
					File.SetAttributes(filePath, attributes & ~FileAttributes.ReadOnly);
				}
			} catch { }
		}
		public DbSet<Category> Categories { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<EmployeeTerritory> EmployeeTerritories { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Region> Regions { get; set; }
		public DbSet<Shipper> Shippers { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<Territory> Territories { get; set; }
		public DbSet<AlphabeticalListOfProduct> AlphabeticalListOfProducts { get; set; }
		public DbSet<CategoryProduct> CategoryProducts { get; set; }
		public DbSet<CurrentProductList> CurrentProductLists { get; set; }
		public DbSet<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get; set; }
		public DbSet<CustomerReport> CustomerReports { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<OrderDetailsExtended> OrderDetailsExtended { get; set; }
		public DbSet<OrderReport> OrderReports { get; set; }
		public DbSet<OrdersQry> OrdersQries { get; set; }
		public DbSet<OrderSubtotal> OrderSubtotals { get; set; }
		public DbSet<ProductReport> ProductReports { get; set; }
		public DbSet<ProductsAboveAveragePrice> ProductsAboveAveragePrices { get; set; }
		public DbSet<ProductsByCategory> ProductsByCategories { get; set; }
		public DbSet<SalesByCategory> SalesByCategories { get; set; }
		public DbSet<SalesPerson> SalesPersons { get; set; }
		public DbSet<SalesTotalsByAmount> SalesTotalsByAmounts { get; set; }
		public DbSet<SummaryOfSalesByQuarter> SummaryOfSalesByQuarters { get; set; }
		public DbSet<SummaryOfSalesByYear> SummaryOfSalesByYears { get; set; }
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.ApplyConfiguration(new AlphabeticalListOfProductMap());
			modelBuilder.ApplyConfiguration(new CategoryMap());
			modelBuilder.ApplyConfiguration(new CustomerMap());
			modelBuilder.ApplyConfiguration(new EmployeeMap());
			modelBuilder.ApplyConfiguration(new EmployeeTerritoryMap());
			modelBuilder.ApplyConfiguration(new OrderDetailMap());
			modelBuilder.ApplyConfiguration(new OrderMap());
			modelBuilder.ApplyConfiguration(new ProductMap());
			modelBuilder.ApplyConfiguration(new RegionMap());
			modelBuilder.ApplyConfiguration(new ShipperMap());
			modelBuilder.ApplyConfiguration(new SupplierMap());
			modelBuilder.ApplyConfiguration(new TerritoryMap());
			modelBuilder.ApplyConfiguration(new AlphabeticalListOfProductMap());
			modelBuilder.ApplyConfiguration(new CategoryProductMap());
			modelBuilder.ApplyConfiguration(new CurrentProductListMap());
			modelBuilder.ApplyConfiguration(new CustomerAndSuppliersByCityMap());
			modelBuilder.ApplyConfiguration(new CustomerReportMap());
			modelBuilder.ApplyConfiguration(new InvoiceMap());
			modelBuilder.ApplyConfiguration(new OrderDetailsExtendedMap());
			modelBuilder.ApplyConfiguration(new OrderReportMap());
			modelBuilder.ApplyConfiguration(new OrdersQryMap());
			modelBuilder.ApplyConfiguration(new OrderSubtotalMap());
			modelBuilder.ApplyConfiguration(new ProductReportMap());
			modelBuilder.ApplyConfiguration(new ProductsAboveAveragePriceMap());
			modelBuilder.ApplyConfiguration(new ProductsByCategoryMap());
			modelBuilder.ApplyConfiguration(new SalesByCategoryMap());
			modelBuilder.ApplyConfiguration(new SalesPersonMap());
			modelBuilder.ApplyConfiguration(new SalesTotalsByAmountMap());
			modelBuilder.ApplyConfiguration(new SummaryOfSalesByQuarterMap());
			modelBuilder.ApplyConfiguration(new SummaryOfSalesByYearMap());
		}
		public string[] CountriesArray = new[] { "United States", "Afghanistan", "Albania", "Algeria", "Andorra", "Angola",
			"Anguilla", "Antarctica", "Antigua & Barbuda", "Argentina", "Armenia", "Aruba (neth.)", "Australia", "Austria", "Azerbaijan", "Azores (port.)", "Bahamas",
			"Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia And Herzegovina", "Botswana", "Brazil",
			"British Virgin Islands", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic",
			"Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Cook Islands", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic",
			"Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands", "Fiji", "Finland", "Fmr Yug Rep Macedonia", "France", "French Guiana", "French Polynesia",
			"Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea Bissau", "Guyana", "Haiti", "Honduras", "Hong Kong",
			"Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Iraq-Saudi Arabia Neutral Zone", "Ireland", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati",
			"Korea Dem.People's Rep.", "Korea, Republic Of", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya Arab Jamahiriy", "Liechtenstein", "Lithuania", "Luxembourg", "Madagascar",
			"Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mexico", "Micronesia, Fed Stat", "Moldova, Republic Of", "Monaco", "Mongolia", "Morocco",
			"Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau Islands",
			"Panama", "Panama Canal Zone", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Lucia", "San Marino",
			"Sao Tome & Principe", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "Spain", "Sri Lanka", "St.Kitts & Nevis",
			"St.Vinct & Grenadine", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Rep.", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia",
			"Turkey", "Turkmenistan", "Turks And Caicos Islands", "Tuvalu", "U.S. Virgin Islands", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City (Holy See)",
			"Venezuela", "Vietnam", "Western Sahara", "Western Samoa", "Yemen", "Yugoslavia", "Zaire", "Zambia", "Zimbabwe" };
	}
}
namespace DevExpress.DemoData.Models {
	public partial class AlphabeticalListOfProduct {
		public long ProductID { get; set; }
		public string ProductName { get; set; }
		public Nullable<long> SupplierID { get; set; }
		public Nullable<long> CategoryID { get; set; }
		public string QuantityPerUnit { get; set; }
		public Nullable<decimal> UnitPrice { get; set; }
		public Nullable<short> UnitsInStock { get; set; }
		public Nullable<short> UnitsOnOrder { get; set; }
		public Nullable<short> ReorderLevel { get; set; }
		public bool Discontinued { get; set; }
		public string EAN13 { get; set; }
		public string CategoryName { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Category {
		public long CategoryID { get; set; }
		public string CategoryName { get; set; }
		public string Description { get; set; }
		public byte[] Picture { get; set; }
		public byte[] Icon25 { get; set; }
		public byte[] Icon17 { get; set; }
		public virtual ICollection<Product> Products { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class CategoryProduct {
		public long ProductID { get; set; }
		public Nullable<long> SupplierID { get; set; }
		public string ProductName { get; set; }
		public string CategoryName { get; set; }
		public byte[] Picture { get; set; }
		public string Description { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class CurrentProductList {
		public long ProductID { get; set; }
		public string ProductName { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Customer {
		public string CustomerID { get; set; }
		public string CompanyName { get; set; }
		public string ContactName { get; set; }
		public string ContactTitle { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public virtual ICollection<Employee> Employees { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class CustomerAndSuppliersByCity {
		public string City { get; set; }
		public string CompanyName { get; set; }
		public string ContactName { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class CustomerReport {
		public string ProductName { get; set; }
		public string CompanyName { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }
		public decimal ProductAmount { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "DXCA003", Justification = "VB Convertion")]
	public partial class Employee : BindableBase {
		long _EmployeeID;
		public long EmployeeID { get { return _EmployeeID; } set { SetProperty(ref _EmployeeID, value, nameof(EmployeeID)); } }
		string _LastName;
		public string LastName { get { return _LastName; } set { SetProperty(ref _LastName, value, nameof(LastName)); } }
		string _FirstName;
		public string FirstName { get { return _FirstName; } set { SetProperty(ref _FirstName, value, nameof(FirstName)); } }
		string _Title;
		public string Title { get { return _Title; } set { SetProperty(ref _Title, value, nameof(Title)); } }
		string _TitleOfCourtesy;
		public string TitleOfCourtesy { get { return _TitleOfCourtesy; } set { SetProperty(ref _TitleOfCourtesy, value, nameof(TitleOfCourtesy)); } }
		Nullable<System.DateTime> _BirthDate;
		public Nullable<System.DateTime> BirthDate { get { return _BirthDate; } set { SetProperty(ref _BirthDate, value, nameof(BirthDate)); } }
		Nullable<System.DateTime> _HireDate;
		public Nullable<System.DateTime> HireDate { get { return _HireDate; } set { SetProperty(ref _HireDate, value, nameof(HireDate)); } }
		string _Address;
		public string Address { get { return _Address; } set { SetProperty(ref _Address, value, nameof(Address)); } }
		string _City;
		public string City { get { return _City; } set { SetProperty(ref _City, value, nameof(City)); } }
		string _Region;
		public string Region { get { return _Region; } set { SetProperty(ref _Region, value, nameof(Region)); } }
		string _PostalCode;
		public string PostalCode { get { return _PostalCode; } set { SetProperty(ref _PostalCode, value, nameof(PostalCode)); } }
		string _Country;
		public string Country { get { return _Country; } set { SetProperty(ref _Country, value, nameof(Country)); } }
		string _HomePhone;
		public string HomePhone { get { return _HomePhone; } set { SetProperty(ref _HomePhone, value, nameof(HomePhone)); } }
		string _Extension;
		public string Extension { get { return _Extension; } set { SetProperty(ref _Extension, value, nameof(Extension)); } }
		byte[] _Photo;
		public byte[] Photo { get { return _Photo; } set { SetProperty(ref _Photo, value, nameof(Photo)); } }
		string _Notes;
		public string Notes { get { return _Notes; } set { SetProperty(ref _Notes, value, nameof(Notes)); } }
		Nullable<long> _ReportsTo;
		public Nullable<long> ReportsTo { get { return _ReportsTo; } set { SetProperty(ref _ReportsTo, value, nameof(ReportsTo)); } }
		string _Email;
		public string Email { get { return _Email; } set { SetProperty(ref _Email, value, nameof(Email)); } }
		string _GroupName;
		public string GroupName { get { return _GroupName; } set { SetProperty(ref _GroupName, value, nameof(GroupName)); } }
		string _FullName = null;
		public string FullName {
			get {
				if(_FullName == null)
					_FullName = String.Format("{0} {1}", FirstName, LastName);
				return _FullName;
			}
		}
		public virtual ICollection<Customer> Customers { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
		public virtual ICollection<Employee> Employees { get; set; }
		public virtual Employee SubEmployee { get; set; }
		public string PageHeader { get { return (FirstName + " " + LastName).ToUpper(); } }
		public string PageContent {
			get {
				return FirstName + " " + LastName + " was born on " + DateToString(BirthDate) + ". Now lives in " +
					City + ", " + Country + ". " + TitleOfCourtesy + " " + LastName + " holds a position of " + Title + " our " +
					Region + " deparment, (" + City + " " + Country + "). Joined our company on " + DateToString(HireDate) + ".";
			}
		}
		string DateToString(DateTime? date) {
			if(date == null)
				return null;
			string[] Months = { "January", "February", "Marth", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
			return date.Value.Day + "th of " + Months[date.Value.Month - 1] + " in " + date.Value.Year;
		}
		IEnumerable<ChartPoint> _ChartSource = null;
		public IEnumerable<ChartPoint> ChartSource {
			get {
				if(_ChartSource == null)
					CreateChartSource();
				return _ChartSource;
			}
		}
		void CreateChartSource() {
			IEnumerable<ChartPoint> list = (from o in Orders
											group o by o.OrderDate into cp
											select new ChartPoint() {
												ArgumentMember = cp.Key,
												Orders = cp.ToList()
											}).ToList();
			foreach(ChartPoint cp in list) {
				decimal value = 0;
				foreach(Order order in cp.Orders)
					foreach(OrderDetailsExtended inv in order.OrderDetails)
						value += inv.Quantity * inv.UnitPrice;
				cp.ValueMember = (int)value;
			}
			_ChartSource = list;
		}
	}
	[NotMapped]
	public class ChartPoint {
		public DateTime? ArgumentMember { get; internal set; }
		public int ValueMember { get; set; }
		internal IList<Order> Orders { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class EmployeeTerritory {
		public long EmployeeID { get; set; }
		public string TerritoryID { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class InternationalOrder {
		public long OrderID { get; set; }
		public string CustomsDescription { get; set; }
		public decimal ExciseTax { get; set; }
		public virtual Order Order { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Invoice {
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipCity { get; set; }
		public string ShipRegion { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipCountry { get; set; }
		public string CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public long OrderID { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }
		public Nullable<System.DateTime> RequiredDate { get; set; }
		public Nullable<System.DateTime> ShippedDate { get; set; }
		public string ShipperName { get; set; }
		public long ProductID { get; set; }
		public string ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public double Discount { get; set; }
		public Nullable<decimal> Freight { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Order {
		public long OrderID { get; set; }
		public string CustomerID { get; set; }
		public Nullable<long> EmployeeID { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }
		public Nullable<System.DateTime> RequiredDate { get; set; }
		public Nullable<System.DateTime> ShippedDate { get; set; }
		public Nullable<long> ShipVia { get; set; }
		public Nullable<decimal> Freight { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipCity { get; set; }
		public string ShipRegion { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipCountry { get; set; }
		public virtual Employee Employee { get; set; }
		public virtual Customer Customer { get; set; }
		public virtual ICollection<OrderDetailsExtended> OrderDetails { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class OrderDetail {
		public long OrderID { get; set; }
		public long ProductID { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public double Discount { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class OrderDetailsExtended {
		public long OrderID { get; set; }
		public long ProductID { get; set; }
		public string ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public double Discount { get; set; }
		public decimal ExtendedPrice { get; set; }
		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class OrderReport {
		public long OrderID { get; set; }
		public long ProductID { get; set; }
		public string ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public double Discount { get; set; }
		public decimal ExtendedPrice { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class OrdersQry {
		public long OrderID { get; set; }
		public string CustomerID { get; set; }
		public Nullable<long> EmployeeID { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }
		public Nullable<System.DateTime> RequiredDate { get; set; }
		public Nullable<System.DateTime> ShippedDate { get; set; }
		public Nullable<long> ShipVia { get; set; }
		public Nullable<decimal> Freight { get; set; }
		public string ShipName { get; set; }
		public string ShipAddress { get; set; }
		public string ShipCity { get; set; }
		public string ShipRegion { get; set; }
		public string ShipPostalCode { get; set; }
		public string ShipCountry { get; set; }
		public string CompanyName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class OrderSubtotal {
		public long OrderID { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class PreviousEmployee {
		public long EmployeeID { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string Title { get; set; }
		public string TitleOfCourtesy { get; set; }
		public Nullable<System.DateTime> BirthDate { get; set; }
		public Nullable<System.DateTime> HireDate { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string HomePhone { get; set; }
		public string Extension { get; set; }
		public byte[] Photo { get; set; }
		public string Notes { get; set; }
		public string PhotoPath { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Product {
		public long ProductID { get; set; }
		public string ProductName { get; set; }
		public Nullable<long> SupplierID { get; set; }
		public Nullable<long> CategoryID { get; set; }
		public string QuantityPerUnit { get; set; }
		public Nullable<decimal> UnitPrice { get; set; }
		public Nullable<short> UnitsInStock { get; set; }
		public Nullable<short> UnitsOnOrder { get; set; }
		public Nullable<short> ReorderLevel { get; set; }
		public bool Discontinued { get; set; }
		public string EAN13 { get; set; }
		public virtual Category Category { get; set; }
		public virtual ICollection<OrderDetailsExtended> OrderDetails { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class ProductReport {
		public string CategoryName { get; set; }
		public string ProductName { get; set; }
		public decimal ProductSales { get; set; }
		public Nullable<System.DateTime> ShippedDate { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class ProductsAboveAveragePrice {
		public string ProductName { get; set; }
		public Nullable<decimal> UnitPrice { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class ProductsByCategory {
		public string CategoryName { get; set; }
		public string ProductName { get; set; }
		public string QuantityPerUnit { get; set; }
		public Nullable<short> UnitsInStock { get; set; }
		public bool Discontinued { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Region {
		public long RegionID { get; set; }
		public string RegionDescription { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class SalesByCategory {
		public long CategoryID { get; set; }
		public string CategoryName { get; set; }
		public string ProductName { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class SalesPerson {
		public long OrderID { get; set; }
		public string Country { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string ProductName { get; set; }
		public string CategoryName { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public double Discount { get; set; }
		public decimal ExtendedPrice { get; set; }
		public string FullName { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class SalesTotalsByAmount {
		public long OrderID { get; set; }
		public string CompanyName { get; set; }
		public Nullable<System.DateTime> ShippedDate { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Shipper {
		public long ShipperID { get; set; }
		public string CompanyName { get; set; }
		public string Phone { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class SummaryOfSalesByQuarter {
		public Nullable<System.DateTime> ShippedDate { get; set; }
		public long OrderID { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class SummaryOfSalesByYear {
		public Nullable<System.DateTime> ShippedDate { get; set; }
		public long OrderID { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Supplier {
		public long SupplierID { get; set; }
		public string CompanyName { get; set; }
		public string ContactName { get; set; }
		public string ContactTitle { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string HomePage { get; set; }
	}
}
namespace DevExpress.DemoData.Models {
	public partial class Territory {
		public string TerritoryID { get; set; }
		public string TerritoryDescription { get; set; }
		public long RegionID { get; set; }
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class AlphabeticalListOfProductMap : IEntityTypeConfiguration<AlphabeticalListOfProduct> {
		public void Configure(EntityTypeBuilder<AlphabeticalListOfProduct> builder) {
			builder.HasKey(t => t.ProductID);
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.QuantityPerUnit)
				.HasMaxLength(20);
			builder.Property(t => t.EAN13)
				.HasMaxLength(2147483647);
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.ToTable("AlphabeticalListOfProducts");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.SupplierID).HasColumnName("SupplierID");
			builder.Property(t => t.CategoryID).HasColumnName("CategoryID");
			builder.Property(t => t.QuantityPerUnit).HasColumnName("QuantityPerUnit");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double?>();
			builder.Property(t => t.UnitsInStock).HasColumnName("UnitsInStock");
			builder.Property(t => t.UnitsOnOrder).HasColumnName("UnitsOnOrder");
			builder.Property(t => t.ReorderLevel).HasColumnName("ReorderLevel");
			builder.Property(t => t.Discontinued).HasColumnName("Discontinued");
			builder.Property(t => t.EAN13).HasColumnName("EAN13");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class CategoryMap : IEntityTypeConfiguration<Category> {
		public void Configure(EntityTypeBuilder<Category> builder) {
			builder.HasKey(t => t.CategoryID);
			builder.Property(t => t.CategoryID)
				.ValueGeneratedNever();
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.Property(t => t.Description)
				.HasMaxLength(1073741823);
			builder.Property(t => t.Picture)
				.HasMaxLength(2147483647);
			builder.ToTable("Categories");
			builder.Property(t => t.CategoryID).HasColumnName("CategoryID");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
			builder.Property(t => t.Description).HasColumnName("Description");
			builder.Property(t => t.Picture).HasColumnName("Picture");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class CategoryProductMap : IEntityTypeConfiguration<CategoryProduct> {
		public void Configure(EntityTypeBuilder<CategoryProduct> builder) {
			builder.HasKey(t => t.ProductID);
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.Property(t => t.Picture)
				.HasMaxLength(2147483647);
			builder.Property(t => t.Description)
				.HasMaxLength(1073741823);
			builder.ToTable("CategoryProducts");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.SupplierID).HasColumnName("SupplierID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
			builder.Property(t => t.Picture).HasColumnName("Picture");
			builder.Property(t => t.Description).HasColumnName("Description");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class CurrentProductListMap : IEntityTypeConfiguration<CurrentProductList> {
		public void Configure(EntityTypeBuilder<CurrentProductList> builder) {
			builder.HasKey(t => t.ProductID);
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.ToTable("CurrentProductList");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class CustomerAndSuppliersByCityMap : IEntityTypeConfiguration<CustomerAndSuppliersByCity> {
		public void Configure(EntityTypeBuilder<CustomerAndSuppliersByCity> builder) {
			builder.HasKey(t => t.CompanyName);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.ContactName)
				.HasMaxLength(30);
			builder.ToTable("CustomerAndSuppliersByCity");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.ContactName).HasColumnName("ContactName");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class CustomerMap : IEntityTypeConfiguration<Customer> {
		public void Configure(EntityTypeBuilder<Customer> builder) {
			builder.HasKey(t => t.CustomerID);
			builder.Property(t => t.CustomerID)
				.IsRequired()
				.IsFixedLength()
				.HasMaxLength(5);
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.ContactName)
				.HasMaxLength(30);
			builder.Property(t => t.ContactTitle)
				.HasMaxLength(30);
			builder.Property(t => t.Address)
				.HasMaxLength(60);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.Region)
				.HasMaxLength(15);
			builder.Property(t => t.PostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.Property(t => t.Phone)
				.HasMaxLength(24);
			builder.Property(t => t.Fax)
				.HasMaxLength(24);
			builder.ToTable("Customers");
			builder.Property(t => t.CustomerID).HasColumnName("CustomerID");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.ContactName).HasColumnName("ContactName");
			builder.Property(t => t.ContactTitle).HasColumnName("ContactTitle");
			builder.Property(t => t.Address).HasColumnName("Address");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.Region).HasColumnName("Region");
			builder.Property(t => t.PostalCode).HasColumnName("PostalCode");
			builder.Property(t => t.Country).HasColumnName("Country");
			builder.Property(t => t.Phone).HasColumnName("Phone");
			builder.Property(t => t.Fax).HasColumnName("Fax");
			builder.HasMany(x => x.Orders)
				.WithOne(x => x.Customer)
				.HasForeignKey(x => x.CustomerID);
			builder.Ignore(t => t.Employees);
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class CustomerReportMap : IEntityTypeConfiguration<CustomerReport> {
		public void Configure(EntityTypeBuilder<CustomerReport> builder) {
			builder.HasKey(t => new { t.ProductName, t.CompanyName });
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.ToTable("CustomerReports");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.OrderDate).HasColumnName("OrderDate");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class EmployeeMap : IEntityTypeConfiguration<Employee> {
		public void Configure(EntityTypeBuilder<Employee> builder) {
			builder.HasKey(t => t.EmployeeID);
			builder.Property(t => t.EmployeeID)
				.ValueGeneratedNever();
			builder.Property(t => t.LastName)
				.IsRequired()
				.HasMaxLength(20);
			builder.Property(t => t.FirstName)
				.IsRequired()
				.HasMaxLength(10);
			builder.Property(t => t.Title)
				.HasMaxLength(30);
			builder.Property(t => t.TitleOfCourtesy)
				.HasMaxLength(25);
			builder.Property(t => t.Address)
				.HasMaxLength(60);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.Region)
				.HasMaxLength(15);
			builder.Property(t => t.PostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.Property(t => t.HomePhone)
				.HasMaxLength(24);
			builder.Property(t => t.Extension)
				.HasMaxLength(4);
			builder.Property(t => t.Photo)
				.HasMaxLength(2147483647);
			builder.Property(t => t.Notes)
				.HasMaxLength(1073741823);
			builder.ToTable("Employees");
			builder.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
			builder.Property(t => t.LastName).HasColumnName("LastName");
			builder.Property(t => t.FirstName).HasColumnName("FirstName");
			builder.Property(t => t.Title).HasColumnName("Title");
			builder.Property(t => t.TitleOfCourtesy).HasColumnName("TitleOfCourtesy");
			builder.Property(t => t.BirthDate).HasColumnName("BirthDate");
			builder.Property(t => t.HireDate).HasColumnName("HireDate");
			builder.Property(t => t.Address).HasColumnName("Address");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.Region).HasColumnName("Region");
			builder.Property(t => t.PostalCode).HasColumnName("PostalCode");
			builder.Property(t => t.Country).HasColumnName("Country");
			builder.Property(t => t.HomePhone).HasColumnName("HomePhone");
			builder.Property(t => t.Extension).HasColumnName("Extension");
			builder.Property(t => t.Photo).HasColumnName("Photo");
			builder.Property(t => t.Notes).HasColumnName("Notes");
			builder.Property(t => t.ReportsTo).HasColumnName("ReportsTo");
			builder.Property(t => t.GroupName).HasColumnName("GroupName");
			builder.Ignore(t => t.Customers);
			builder.HasMany(x => x.Employees)
				.WithOne(x => x.SubEmployee)
				.HasForeignKey(x => x.ReportsTo);
			builder.HasMany(x => x.Orders)
				.WithOne(x => x.Employee)
				.HasForeignKey(x => x.EmployeeID);
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class EmployeeTerritoryMap : IEntityTypeConfiguration<EmployeeTerritory> {
		public void Configure(EntityTypeBuilder<EmployeeTerritory> builder) {
			builder.HasKey(t => new { t.EmployeeID, t.TerritoryID });
			builder.Property(t => t.EmployeeID)
				.ValueGeneratedNever();
			builder.Property(t => t.TerritoryID)
				.IsRequired()
				.HasMaxLength(20);
			builder.ToTable("EmployeeTerritories");
			builder.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
			builder.Property(t => t.TerritoryID).HasColumnName("TerritoryID");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class InvoiceMap : IEntityTypeConfiguration<Invoice> {
		public void Configure(EntityTypeBuilder<Invoice> builder) {
			builder.HasKey(t => new { t.CustomerName, t.OrderID, t.ShipperName, t.ProductID, t.ProductName, t.UnitPrice, t.Quantity, t.Discount });
			builder.Property(t => t.ShipName)
				.HasMaxLength(40);
			builder.Property(t => t.ShipAddress)
				.HasMaxLength(60);
			builder.Property(t => t.ShipCity)
				.HasMaxLength(15);
			builder.Property(t => t.ShipRegion)
				.HasMaxLength(15);
			builder.Property(t => t.ShipPostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.ShipCountry)
				.HasMaxLength(15);
			builder.Property(t => t.CustomerID)
				.IsFixedLength()
				.HasMaxLength(5);
			builder.Property(t => t.CustomerName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.Address)
				.HasMaxLength(60);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.Region)
				.HasMaxLength(15);
			builder.Property(t => t.PostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.ShipperName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.UnitPrice)
				.ValueGeneratedNever();
			builder.Property(t => t.Quantity)
				.ValueGeneratedNever();
			builder.ToTable("Invoices");
			builder.Property(t => t.ShipName).HasColumnName("ShipName");
			builder.Property(t => t.ShipAddress).HasColumnName("ShipAddress");
			builder.Property(t => t.ShipCity).HasColumnName("ShipCity");
			builder.Property(t => t.ShipRegion).HasColumnName("ShipRegion");
			builder.Property(t => t.ShipPostalCode).HasColumnName("ShipPostalCode");
			builder.Property(t => t.ShipCountry).HasColumnName("ShipCountry");
			builder.Property(t => t.CustomerID).HasColumnName("CustomerID");
			builder.Property(t => t.CustomerName).HasColumnName("CustomerName");
			builder.Property(t => t.Address).HasColumnName("Address");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.Region).HasColumnName("Region");
			builder.Property(t => t.PostalCode).HasColumnName("PostalCode");
			builder.Property(t => t.Country).HasColumnName("Country");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.OrderDate).HasColumnName("OrderDate");
			builder.Property(t => t.RequiredDate).HasColumnName("RequiredDate");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
			builder.Property(t => t.ShipperName).HasColumnName("ShipperName");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double>();
			builder.Property(t => t.Quantity).HasColumnName("Quantity");
			builder.Property(t => t.Discount).HasColumnName("Discount");
			builder.Property(t => t.Freight).HasColumnName("Freight").HasConversion<double?>();
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class OrderDetailMap : IEntityTypeConfiguration<OrderDetail> {
		public void Configure(EntityTypeBuilder<OrderDetail> builder) {
			builder.HasKey(t => new { t.OrderID, t.ProductID, t.UnitPrice, t.Quantity, t.Discount });
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.UnitPrice)
				.ValueGeneratedNever();
			builder.Property(t => t.Quantity)
				.ValueGeneratedNever();
			builder.ToTable("OrderDetails");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double>();
			builder.Property(t => t.Quantity).HasColumnName("Quantity");
			builder.Property(t => t.Discount).HasColumnName("Discount");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class OrderDetailsExtendedMap : IEntityTypeConfiguration<OrderDetailsExtended> {
		public void Configure(EntityTypeBuilder<OrderDetailsExtended> builder) {
			builder.HasKey(t => new { t.OrderID, t.ProductID, t.ProductName, t.UnitPrice, t.Quantity, t.Discount });
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.UnitPrice)
				.ValueGeneratedNever();
			builder.Property(t => t.Quantity)
				.ValueGeneratedNever();
			builder.ToTable("OrderDetailsExtended");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double>();
			builder.Property(t => t.Quantity).HasColumnName("Quantity");
			builder.Property(t => t.Discount).HasColumnName("Discount");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class OrderMap : IEntityTypeConfiguration<Order> {
		public void Configure(EntityTypeBuilder<Order> builder) {
			builder.HasKey(t => t.OrderID);
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.CustomerID)
				.IsFixedLength()
				.HasMaxLength(5);
			builder.Property(t => t.ShipName)
				.HasMaxLength(40);
			builder.Property(t => t.ShipAddress)
				.HasMaxLength(60);
			builder.Property(t => t.ShipCity)
				.HasMaxLength(15);
			builder.Property(t => t.ShipRegion)
				.HasMaxLength(15);
			builder.Property(t => t.ShipPostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.ShipCountry)
				.HasMaxLength(15);
			builder.ToTable("Orders");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.CustomerID).HasColumnName("CustomerID");
			builder.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
			builder.Property(t => t.OrderDate).HasColumnName("OrderDate");
			builder.Property(t => t.RequiredDate).HasColumnName("RequiredDate");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
			builder.Property(t => t.ShipVia).HasColumnName("ShipVia");
			builder.Property(t => t.Freight).HasColumnName("Freight").HasConversion<double?>();
			builder.Property(t => t.ShipName).HasColumnName("ShipName");
			builder.Property(t => t.ShipAddress).HasColumnName("ShipAddress");
			builder.Property(t => t.ShipCity).HasColumnName("ShipCity");
			builder.Property(t => t.ShipRegion).HasColumnName("ShipRegion");
			builder.Property(t => t.ShipPostalCode).HasColumnName("ShipPostalCode");
			builder.Property(t => t.ShipCountry).HasColumnName("ShipCountry");
			builder.HasMany(x => x.OrderDetails)
				.WithOne(x => x.Order).IsRequired()
				.HasForeignKey(x => x.OrderID);
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class OrderReportMap : IEntityTypeConfiguration<OrderReport> {
		public void Configure(EntityTypeBuilder<OrderReport> builder) {
			builder.HasKey(t => new { t.OrderID, t.ProductID, t.ProductName, t.UnitPrice, t.Quantity, t.Discount });
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.UnitPrice)
				.ValueGeneratedNever();
			builder.Property(t => t.Quantity)
				.ValueGeneratedNever();
			builder.ToTable("OrderReports");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double>();
			builder.Property(t => t.Quantity).HasColumnName("Quantity");
			builder.Property(t => t.Discount).HasColumnName("Discount");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class OrdersQryMap : IEntityTypeConfiguration<OrdersQry> {
		public void Configure(EntityTypeBuilder<OrdersQry> builder) {
			builder.HasKey(t => new { t.OrderID, t.CompanyName });
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.CustomerID)
				.IsFixedLength()
				.HasMaxLength(5);
			builder.Property(t => t.ShipName)
				.HasMaxLength(40);
			builder.Property(t => t.ShipAddress)
				.HasMaxLength(60);
			builder.Property(t => t.ShipCity)
				.HasMaxLength(15);
			builder.Property(t => t.ShipRegion)
				.HasMaxLength(15);
			builder.Property(t => t.ShipPostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.ShipCountry)
				.HasMaxLength(15);
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.Address)
				.HasMaxLength(60);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.Region)
				.HasMaxLength(15);
			builder.Property(t => t.PostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.ToTable("OrdersQry");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.CustomerID).HasColumnName("CustomerID");
			builder.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
			builder.Property(t => t.OrderDate).HasColumnName("OrderDate");
			builder.Property(t => t.RequiredDate).HasColumnName("RequiredDate");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
			builder.Property(t => t.ShipVia).HasColumnName("ShipVia");
			builder.Property(t => t.Freight).HasColumnName("Freight").HasConversion<double?>();
			builder.Property(t => t.ShipName).HasColumnName("ShipName");
			builder.Property(t => t.ShipAddress).HasColumnName("ShipAddress");
			builder.Property(t => t.ShipCity).HasColumnName("ShipCity");
			builder.Property(t => t.ShipRegion).HasColumnName("ShipRegion");
			builder.Property(t => t.ShipPostalCode).HasColumnName("ShipPostalCode");
			builder.Property(t => t.ShipCountry).HasColumnName("ShipCountry");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.Address).HasColumnName("Address");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.Region).HasColumnName("Region");
			builder.Property(t => t.PostalCode).HasColumnName("PostalCode");
			builder.Property(t => t.Country).HasColumnName("Country");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class OrderSubtotalMap : IEntityTypeConfiguration<OrderSubtotal> {
		public void Configure(EntityTypeBuilder<OrderSubtotal> builder) {
			builder.HasKey(t => t.OrderID);
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.ToTable("OrderSubtotals");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class PreviousEmployeeMap : IEntityTypeConfiguration<PreviousEmployee> {
		public void Configure(EntityTypeBuilder<PreviousEmployee> builder) {
			builder.HasKey(t => t.EmployeeID);
			builder.Property(t => t.EmployeeID)
				.ValueGeneratedNever();
			builder.Property(t => t.LastName)
				.IsRequired()
				.HasMaxLength(20);
			builder.Property(t => t.FirstName)
				.IsRequired()
				.HasMaxLength(10);
			builder.Property(t => t.Title)
				.HasMaxLength(30);
			builder.Property(t => t.TitleOfCourtesy)
				.HasMaxLength(25);
			builder.Property(t => t.Address)
				.HasMaxLength(60);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.Region)
				.HasMaxLength(15);
			builder.Property(t => t.PostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.Property(t => t.HomePhone)
				.HasMaxLength(24);
			builder.Property(t => t.Extension)
				.HasMaxLength(4);
			builder.Property(t => t.Photo)
				.HasMaxLength(2147483647);
			builder.Property(t => t.Notes)
				.HasMaxLength(2147483647);
			builder.Property(t => t.PhotoPath)
				.HasMaxLength(255);
			builder.ToTable("PreviousEmployees");
			builder.Property(t => t.EmployeeID).HasColumnName("EmployeeID");
			builder.Property(t => t.LastName).HasColumnName("LastName");
			builder.Property(t => t.FirstName).HasColumnName("FirstName");
			builder.Property(t => t.Title).HasColumnName("Title");
			builder.Property(t => t.TitleOfCourtesy).HasColumnName("TitleOfCourtesy");
			builder.Property(t => t.BirthDate).HasColumnName("BirthDate");
			builder.Property(t => t.HireDate).HasColumnName("HireDate");
			builder.Property(t => t.Address).HasColumnName("Address");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.Region).HasColumnName("Region");
			builder.Property(t => t.PostalCode).HasColumnName("PostalCode");
			builder.Property(t => t.Country).HasColumnName("Country");
			builder.Property(t => t.HomePhone).HasColumnName("HomePhone");
			builder.Property(t => t.Extension).HasColumnName("Extension");
			builder.Property(t => t.Photo).HasColumnName("Photo");
			builder.Property(t => t.Notes).HasColumnName("Notes");
			builder.Property(t => t.PhotoPath).HasColumnName("PhotoPath");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class ProductMap : IEntityTypeConfiguration<Product> {
		public void Configure(EntityTypeBuilder<Product> builder) {
			builder.HasKey(t => t.ProductID);
			builder.Property(t => t.ProductID)
				.ValueGeneratedNever();
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.QuantityPerUnit)
				.HasMaxLength(20);
			builder.Property(t => t.EAN13)
				.HasMaxLength(2147483647);
			builder.ToTable("Products");
			builder.Property(t => t.ProductID).HasColumnName("ProductID");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.SupplierID).HasColumnName("SupplierID");
			builder.Property(t => t.CategoryID).HasColumnName("CategoryID");
			builder.Property(t => t.QuantityPerUnit).HasColumnName("QuantityPerUnit");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double?>();
			builder.Property(t => t.UnitsInStock).HasColumnName("UnitsInStock");
			builder.Property(t => t.UnitsOnOrder).HasColumnName("UnitsOnOrder");
			builder.Property(t => t.ReorderLevel).HasColumnName("ReorderLevel");
			builder.Property(t => t.Discontinued).HasColumnName("Discontinued");
			builder.Property(t => t.EAN13).HasColumnName("EAN13");
			builder.HasOne(p => p.Category)
				.WithMany(c => c.Products)
				.HasForeignKey(p => p.CategoryID);
			builder.HasMany(x => x.OrderDetails)
				.WithOne(x => x.Product).IsRequired()
				.HasForeignKey(x => x.ProductID);
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class ProductReportMap : IEntityTypeConfiguration<ProductReport> {
		public void Configure(EntityTypeBuilder<ProductReport> builder) {
			builder.HasKey(t => new { t.CategoryName, t.ProductName, t.ShippedDate });
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.ToTable("ProductReports");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.ProductSales).HasColumnName("ProductSales");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class ProductsAboveAveragePriceMap : IEntityTypeConfiguration<ProductsAboveAveragePrice> {
		public void Configure(EntityTypeBuilder<ProductsAboveAveragePrice> builder) {
			builder.HasKey(t => t.ProductName);
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.ToTable("ProductsAboveAveragePrice");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double?>();
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class ProductsByCategoryMap : IEntityTypeConfiguration<ProductsByCategory> {
		public void Configure(EntityTypeBuilder<ProductsByCategory> builder) {
			builder.HasKey(t => new { t.CategoryName, t.ProductName, t.Discontinued });
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.QuantityPerUnit)
				.HasMaxLength(20);
			builder.ToTable("ProductsByCategory");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.QuantityPerUnit).HasColumnName("QuantityPerUnit");
			builder.Property(t => t.UnitsInStock).HasColumnName("UnitsInStock");
			builder.Property(t => t.Discontinued).HasColumnName("Discontinued");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class RegionMap : IEntityTypeConfiguration<Region> {
		public void Configure(EntityTypeBuilder<Region> builder) {
			builder.HasKey(t => t.RegionID);
			builder.Property(t => t.RegionID)
				.ValueGeneratedNever();
			builder.Property(t => t.RegionDescription)
				.IsRequired()
				.IsFixedLength()
				.HasMaxLength(50);
			builder.ToTable("Region");
			builder.Property(t => t.RegionID).HasColumnName("RegionID");
			builder.Property(t => t.RegionDescription).HasColumnName("RegionDescription");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class SalesByCategoryMap : IEntityTypeConfiguration<SalesByCategory> {
		public void Configure(EntityTypeBuilder<SalesByCategory> builder) {
			builder.HasKey(t => t.CategoryID);
			builder.Property(t => t.CategoryID)
				.ValueGeneratedNever();
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.ToTable("SalesByCategory");
			builder.Property(t => t.CategoryID).HasColumnName("CategoryID");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class SalesPersonMap : IEntityTypeConfiguration<SalesPerson> {
		public void Configure(EntityTypeBuilder<SalesPerson> builder) {
			builder.HasKey(t => new { t.OrderID, t.FirstName, t.LastName, t.ProductName, t.CategoryName, t.UnitPrice, t.Quantity, t.Discount });
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.Property(t => t.FirstName)
				.IsRequired()
				.HasMaxLength(10);
			builder.Property(t => t.LastName)
				.IsRequired()
				.HasMaxLength(20);
			builder.Property(t => t.ProductName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.CategoryName)
				.IsRequired()
				.HasMaxLength(15);
			builder.Property(t => t.UnitPrice)
				.ValueGeneratedNever();
			builder.Property(t => t.Quantity)
				.ValueGeneratedNever();
			builder.ToTable("SalesPerson");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.Country).HasColumnName("Country");
			builder.Property(t => t.FirstName).HasColumnName("FirstName");
			builder.Property(t => t.LastName).HasColumnName("LastName");
			builder.Property(t => t.ProductName).HasColumnName("ProductName");
			builder.Property(t => t.CategoryName).HasColumnName("CategoryName");
			builder.Property(t => t.OrderDate).HasColumnName("OrderDate");
			builder.Property(t => t.UnitPrice).HasColumnName("UnitPrice").HasConversion<double>();
			builder.Property(t => t.Quantity).HasColumnName("Quantity");
			builder.Property(t => t.Discount).HasColumnName("Discount");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class SalesTotalsByAmountMap : IEntityTypeConfiguration<SalesTotalsByAmount> {
		public void Configure(EntityTypeBuilder<SalesTotalsByAmount> builder) {
			builder.HasKey(t => new { t.OrderID, t.CompanyName });
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.ToTable("SalesTotalsByAmount");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class ShipperMap : IEntityTypeConfiguration<Shipper> {
		public void Configure(EntityTypeBuilder<Shipper> builder) {
			builder.HasKey(t => new { t.ShipperID, t.CompanyName });
			builder.Property(t => t.ShipperID)
				.ValueGeneratedNever();
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.Phone)
				.HasMaxLength(24);
			builder.ToTable("Shippers");
			builder.Property(t => t.ShipperID).HasColumnName("ShipperID");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.Phone).HasColumnName("Phone");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class SummaryOfSalesByQuarterMap : IEntityTypeConfiguration<SummaryOfSalesByQuarter> {
		public void Configure(EntityTypeBuilder<SummaryOfSalesByQuarter> builder) {
			builder.HasKey(t => t.OrderID);
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.ToTable("SummaryOfSalesByQuarter");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class SummaryOfSalesByYearMap : IEntityTypeConfiguration<SummaryOfSalesByYear> {
		public void Configure(EntityTypeBuilder<SummaryOfSalesByYear> builder) {
			builder.HasKey(t => t.OrderID);
			builder.Property(t => t.OrderID)
				.ValueGeneratedNever();
			builder.ToTable("SummaryOfSalesByYear");
			builder.Property(t => t.ShippedDate).HasColumnName("ShippedDate");
			builder.Property(t => t.OrderID).HasColumnName("OrderID");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class SupplierMap : IEntityTypeConfiguration<Supplier> {
		public void Configure(EntityTypeBuilder<Supplier> builder) {
			builder.HasKey(t => t.SupplierID);
			builder.Property(t => t.SupplierID)
				.ValueGeneratedNever();
			builder.Property(t => t.CompanyName)
				.IsRequired()
				.HasMaxLength(40);
			builder.Property(t => t.ContactName)
				.HasMaxLength(30);
			builder.Property(t => t.ContactTitle)
				.HasMaxLength(30);
			builder.Property(t => t.Address)
				.HasMaxLength(60);
			builder.Property(t => t.City)
				.HasMaxLength(15);
			builder.Property(t => t.Region)
				.HasMaxLength(15);
			builder.Property(t => t.PostalCode)
				.HasMaxLength(10);
			builder.Property(t => t.Country)
				.HasMaxLength(15);
			builder.Property(t => t.Phone)
				.HasMaxLength(24);
			builder.Property(t => t.Fax)
				.HasMaxLength(24);
			builder.Property(t => t.HomePage)
				.HasMaxLength(1073741823);
			builder.ToTable("Suppliers");
			builder.Property(t => t.SupplierID).HasColumnName("SupplierID");
			builder.Property(t => t.CompanyName).HasColumnName("CompanyName");
			builder.Property(t => t.ContactName).HasColumnName("ContactName");
			builder.Property(t => t.ContactTitle).HasColumnName("ContactTitle");
			builder.Property(t => t.Address).HasColumnName("Address");
			builder.Property(t => t.City).HasColumnName("City");
			builder.Property(t => t.Region).HasColumnName("Region");
			builder.Property(t => t.PostalCode).HasColumnName("PostalCode");
			builder.Property(t => t.Country).HasColumnName("Country");
			builder.Property(t => t.Phone).HasColumnName("Phone");
			builder.Property(t => t.Fax).HasColumnName("Fax");
			builder.Property(t => t.HomePage).HasColumnName("HomePage");
		}
	}
}
namespace DevExpress.DemoData.Models.Mapping {
	public class TerritoryMap : IEntityTypeConfiguration<Territory> {
		public void Configure(EntityTypeBuilder<Territory> builder) {
			builder.HasKey(t => t.TerritoryID);
			builder.Property(t => t.TerritoryID)
				.IsRequired()
				.HasMaxLength(20);
			builder.Property(t => t.TerritoryDescription)
				.IsRequired()
				.IsFixedLength()
				.HasMaxLength(50);
			builder.Property(t => t.RegionID)
				.ValueGeneratedNever();
			builder.ToTable("Territories");
			builder.Property(t => t.TerritoryID).HasColumnName("TerritoryID");
			builder.Property(t => t.TerritoryDescription).HasColumnName("TerritoryDescription");
			builder.Property(t => t.RegionID).HasColumnName("RegionID");
		}
	}
}
