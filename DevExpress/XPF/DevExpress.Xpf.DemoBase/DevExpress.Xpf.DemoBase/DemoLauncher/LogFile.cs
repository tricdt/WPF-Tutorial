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

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace DevExpress.Xpf.DemoLauncher {
	public class LogFile {
		const string LogFileName = "DemoLauncherLoading.log";
		public string silverlight = string.Empty;
		static LogFile current;
		public static LogFile Current {
			get {
				if(current == null)
					current = new LogFile();
				return current;
			}
		}
		private LogFile() {
			lock(this) {
				try {
					if(File.Exists(LogFilePath))
						File.Delete(LogFilePath);
				} catch { }
			}
		}
		public void WriteLine(string format, params object[] args) {
			WriteLine(string.Format(format, args));
		}
		public void WriteLine(string message) {
			message += $" ({Environment.CurrentManagedThreadId})";
			Debug.WriteLine(message);
			lock(this) {
				try {
					using(FileStream logFile = new FileStream(LogFilePath, FileMode.Append, FileAccess.Write)) {
						using(StreamWriter writer = new StreamWriter(logFile)) {
							writer.WriteLine(message);
						}
					}
				} catch { }
			}
		}
		string LogFilePath { 
			get {
				return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFileName); 
			} 
		}
	}
}
