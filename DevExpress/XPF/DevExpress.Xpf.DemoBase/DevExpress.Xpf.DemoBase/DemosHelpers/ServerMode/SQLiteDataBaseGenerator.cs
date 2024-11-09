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

using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace DevExpress.Xpf.DemoBase {
	public class ProgressInfo {
		public ProgressInfo(int value, int maxValue) {
			Value = value;
			MaxValue = maxValue;
		}
		public int Value { get; private set; }
		public int MaxValue { get; private set; }
	}
	public class DatabaseInfo {
		public DatabaseInfo(string fileName, string tableName, Type type, Func<IDictionary<string, object>> getNewRecordValues, Func<string, DbConnection, DbCommand> createCommand) {
			FileName = fileName;
			TableName = tableName;
			var properties = TypeDescriptor.GetProperties(type)
				.Cast<PropertyDescriptor>()
				.Where(x => x.Name != "Id")
				.OrderBy(x => x.Name);
			Fields = properties.Select(x => x.Name).ToArray();
			Types = properties.Select(x => x.PropertyType).ToArray();
			GetNewRecordValues = getNewRecordValues;
			CreateCommand = createCommand;
		}
		public string FileName { get; private set; }
		public string TableName { get; private set; }
		public string[] Fields { get; private set; }
		public Type[] Types { get; private set; }
		public Func<IDictionary<string, object>> GetNewRecordValues { get; private set; }
		public Func<string, DbConnection, DbCommand> CreateCommand { get; private set; }
	}
	public static class SQLiteDataBaseGenerator {
		const string PreparingStatus = "Preparing...";
		const string GeneratingDataStatus = "Filling database with records...";
		const string CreatingIndicesStatus = "Creating database indices...";
		public static DbConnection CreateConnection(DatabaseInfo info) {
#if !NET
			var connection = DbProviderFactories.GetFactory("System.Data.SQLite.EF6").CreateConnection();
#else
			var connection = DbProviderFactories.GetFactory("Microsoft.Data.Sqlite").CreateConnection();
#endif
			connection.ConnectionString = "data source = " + info.FileName;
			return connection;
		}
		public static System.Threading.Tasks.Task GenerateData(DatabaseInfo info, Action<ProgressInfo, string> onProgress, Func<bool> stopAndShow) {
			return ExecuteWithConnection(info, connection => {
				CreateTable(info, connection);
				GenerateData(info, 300000, onProgress, stopAndShow, connection);
			});
		}
		public static System.Threading.Tasks.Task AddData(DatabaseInfo info, int addRecordCount, bool clearTable, Action<ProgressInfo, string> onProgress, Func<bool> stopAndShow) {
			return ExecuteWithConnection(info, connection => {
				onProgress(null, PreparingStatus);
				if(clearTable) {
					info.CreateCommand(@"DROP TABLE " + info.TableName, connection).ExecuteNonQuery();
					CreateTable(info, connection);
				} else {
					try {
						info.CreateCommand(GetFieldsList(info, "DROP INDEX {0}_idx;\r\n", null), connection).ExecuteNonQuery();
					} catch(Exception) { }
				}
				GenerateData(info, addRecordCount, onProgress, stopAndShow, connection);
			});
		}
		static void GenerateData(DatabaseInfo info, int totalCount, Action<ProgressInfo, string> onProgress, Func<bool> stopAndShow, DbConnection connection) {
			string sql = @"INSERT INTO '" + info.TableName + "' (" + GetFieldsList(info, "'{0}'", ",") + ") VALUES";
			var sb = new StringBuilder();
			const int count = 1000;
			int maxValue = totalCount / count;
			for(int j = 0; j < maxValue && !stopAndShow(); j++) {
				onProgress(new ProgressInfo(j, maxValue), GeneratingDataStatus);
				sb.AppendLine(sql);
				for(int i = 0; i < count; i++) {
					sb.Append("(");
					sb.Append(GetValues(info).ConcatStringsWithDelimiter(", "));
					sb.Append(")");
					if(i < count - 1)
						sb.AppendLine(",");
					else
						sb.AppendLine(";");
				}
				var command = info.CreateCommand(sb.ToString(), connection);
				command.ExecuteNonQuery();
				sb.Clear();
			}
			onProgress(null, CreatingIndicesStatus);
			info.CreateCommand(GetFieldsList(info, "CREATE INDEX {0}_idx ON {2}([{0}]);\r\n", null), connection).ExecuteNonQuery();
		}
		static IEnumerable<string> GetValues(DatabaseInfo info) {
			var values = info.GetNewRecordValues();
			foreach(var value in values.OrderBy(x => x.Key).Select(x => x.Value)) {
				if(value is string)
					yield return "'" + ((string)value).Replace("'", "''") + "'";
				else if(value is bool)
					yield return "'" + ((bool)value ? 1 : 0) + "'";
				else if(value is int || value is decimal || value is long)
					yield return "'" + value + "'";
				else if(value is DateTime)
					yield return "date('" + ((DateTime)value).ToString("yyyy-MM-dd") + "')";
				else
					throw new NotSupportedException();
			}
		}
		static Task ExecuteWithConnection(DatabaseInfo info, Action<DbConnection> action) {
			var dispatcher = Dispatcher.CurrentDispatcher;
			return new TaskFactory().StartNew(() => {
				try {
					var connection = CreateConnection(info);
					connection.Open();
					try {
						action(connection);
					} finally {
						connection.Close();
					}
				} catch(Exception e) {
					dispatcher.BeginInvoke(new Action(() => CompatibilityMessageBox.Show(e.ToString(), "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error)));
				}
			}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
		}
		static void CreateTable(DatabaseInfo info, DbConnection connection) {
			string sql = string.Format(@"
                    CREATE TABLE {0}(
                    `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                    " + GetFieldsList(info, "`{0}` {1}\r\n", ",") + ")", info.TableName);
			var command = info.CreateCommand(sql, connection);
			command.ExecuteNonQuery();
		}
		static string GetFieldsList(DatabaseInfo info, string format, string delimeter) {
			return GetFieldsListCore(info, format).ConcatStringsWithDelimiter(delimeter);
		}
		static IEnumerable<string> GetFieldsListCore(DatabaseInfo info, string format) {
			var stringBuilder = new StringBuilder();
			for(int i = 0; i < info.Fields.Length; i++) {
				yield return string.Format(format, info.Fields[i], GetSQLiteTypeName(info.Types[i]), info.TableName);
			}
		}
		static string GetSQLiteTypeName(Type type) {
			var code = Type.GetTypeCode(type);
			switch(code) {
			case TypeCode.Boolean:
			case TypeCode.Int32:
				return "INTEGER";
			case TypeCode.Int64:
					return "BIGINT";
			case TypeCode.DateTime:
			case TypeCode.Decimal:
				return "REAL";
			case TypeCode.String:
				return "TEXT";
			default:
				throw new NotImplementedException();
			}
		}
	}
}
