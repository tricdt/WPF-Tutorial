#region Copyright (c) 2000-2024 Developer Express Inc.
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

using DevExpress.Internal;
using System;
using System.Data.Common;
using System.Windows;
using DevExpress.Xpf.DemoCenterBase;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
#if NET
using Microsoft.EntityFrameworkCore;
using DbModelBuilder = Microsoft.EntityFrameworkCore.ModelBuilder;
#else
using System.Data.SQLite;
using System.Data.Entity;
#endif
using System.IO;
namespace DevExpress.DemoData.Models.Vehicles {
	public class Model {
		public long ID { get; set; }
		public long TrademarkID { get; set; }
		public string Name { get; set; }
		public string Modification { get; set; }
		public long CategoryID { get; set; }
		public decimal Price { get; set; }
		public int? MPGCity { get; set; }
		public int? MPGHighway { get; set; }
		public int Doors { get; set; }
		public long BodyStyleID { get; set; }
		public int Cylinders { get; set; }
		public string Horsepower { get; set; }
		public string Torque { get; set; }
		public string TransmissionSpeeds { get; set; }
		public long TransmissionTypeID { get; set; }
		public string Description { get; set; }
		public byte[] Image { get; set; }
		public byte[] Photo { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public bool InStock { get; set; }
		public string TrademarkName { get { return Trademark.Name; } }
		public string CategoryName { get { return Category.Name; } }
		public string BodyStyleName { get { return BodyStyle.Name; } }
		public string TransmissionTypeName { get { return TransmissionType.Name; } }
		public virtual Trademark Trademark { get; set; }
		public virtual Category Category { get; set; }
		public virtual BodyStyle BodyStyle { get; set; }
		public virtual TransmissionType TransmissionType { get; set; }
	}
	public class BodyStyle {
		public long ID { get; set; }
		public string Name { get; set; }
	}
	public class Category {
		public long ID { get; set; }
		public string Name { get; set; }
		public byte[] Picture { get; set; }
	}
	public class Trademark {
		public long ID { get; set; }
		public string Name { get; set; }
		public string Site { get; set; }
		public byte[] Logo { get; set; }
		public string Description { get; set; }
	}
	public class TransmissionType {
		public long ID { get; set; }
		public string Name { get; set; }
	}
	public partial class VehiclesContext : DbContext {
#if NET
		public VehiclesContext(): base() {
			SetFilePath();
			connectionString = string.Format("Data Source={0}", filePath);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseLazyLoadingProxies().UseSqlite(connectionString);
		}
		string connectionString;
#else
		public VehiclesContext() : base(CreateConnection(), true) { }
		public VehiclesContext(string connectionString) : base(connectionString) { }
		public VehiclesContext(DbConnection connection) : base(connection, true) { }
		static DbConnection CreateConnection() {
			SetFilePath();
			var connection = DbProviderFactories.GetFactory("System.Data.SQLite.EF6").CreateConnection();
			if(filePath != DemoRunner.DBFileFailedString)
				connection.ConnectionString = new SQLiteConnectionStringBuilder { DataSource = filePath }.ConnectionString;
			return connection;
		}
		static VehiclesContext() {
			Database.SetInitializer<VehiclesContext>(null);
		}
#endif
		public static System.Threading.Tasks.Task Preload() {
			return DbContextPreloader<VehiclesContext>.Preload();
		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Entity<Model>()
				.ToTable("Model");
			modelBuilder.Entity<BodyStyle>()
				.ToTable("BodyStyle");
			modelBuilder.Entity<Category>()
				.ToTable("Category");
			modelBuilder.Entity<Trademark>()
				.ToTable("Trademark");
			modelBuilder.Entity<TransmissionType>()
				.ToTable("TransmissionType");
			modelBuilder.Entity<Model>()
				.Property(x => x.MPGCity)
				.HasColumnName("MPG City");
			modelBuilder.Entity<Model>()
				.Property(x => x.MPGHighway)
				.HasColumnName("MPG Highway");
			modelBuilder.Entity<Model>()
				.Property(x => x.TransmissionSpeeds)
				.HasColumnName("Transmission Speeds");
			modelBuilder.Entity<Model>()
				.Property(x => x.TransmissionTypeID)
				.HasColumnName("Transmission Type");
			modelBuilder.Entity<Model>()
				.Property(x => x.DeliveryDate)
				.HasColumnName("Delivery Date");
		}
		static string filePath;
		static void SetFilePath() {
			if(filePath == null)
				filePath = DemoRunner.GetDBFileSafe("vehicles.db");
			try {
				var attributes = File.GetAttributes(filePath);
				if(attributes.HasFlag(FileAttributes.ReadOnly)) {
					File.SetAttributes(filePath, attributes & ~FileAttributes.ReadOnly);
				}
			} catch { }
		}
		public DbSet<Model> Models { get; set; }
		public DbSet<BodyStyle> BodyStyles { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Trademark> Trademarks { get; set; }
		public DbSet<TransmissionType> TransmissionTypes { get; set; }
	}
}
