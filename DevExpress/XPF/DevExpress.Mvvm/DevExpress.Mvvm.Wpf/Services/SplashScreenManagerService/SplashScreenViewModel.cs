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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
namespace DevExpress.Mvvm {
	public class DXSplashScreenViewModel : BindableBase {
		static DXSplashScreenViewModel designTimeData = null;
		public static DXSplashScreenViewModel DesignTimeData {
			get { return designTimeData ?? (designTimeData = new DXSplashScreenViewModel()); }
		}
		static DXSplashScreenViewModel() {
			var ensurePackSchemeIsKnown = System.IO.Packaging.PackUriHelper.UriSchemePack;
		}
		public double Progress {
			get { return progress; }
			set { SetValue(ref progress, value); }
		}
		public bool IsIndeterminate {
			get { return isIndeterminate; }
			set { SetValue(ref isIndeterminate, value); }
		}
		public object Tag {
			get { return tag; }
			set { SetValue(ref tag, value); }
		}
		public string Title {
			get { return title; }
			set { SetValue(ref title, value); }
		}
		public string Subtitle {
			get { return subtitle; }
			set { SetValue(ref subtitle, value); }
		}
		public string Status {
			get { return status; }
			set { SetValue(ref status, value); }
		}
		public string Copyright {
			get { return copyright; }
			set { SetValue(ref copyright, value); }
		}
		public Uri Logo {
			get { return logo; }
			set { SetValue(ref logo, value); }
		}
		string title;
		string subtitle;
		string status;
		string copyright;
		double progress = 0d;
		bool isIndeterminate = true;
		object tag;
		Uri logo;
		public DXSplashScreenViewModel() : this(true) { }
		protected DXSplashScreenViewModel(bool initDefaultValues) {
			if(initDefaultValues)
				InitProperties();
		}
		void InitProperties() {
			isIndeterminate = true;
			progress = 0d;
			logo = new Uri($"pack://application:,,,/{AssemblyInfo.SRAssemblyXpfCore};component/Core/Images/Logo.svg");
			var names = GetApplicationName();
			title = names.Item1;
			subtitle = names.Item2;
			status = "Loading...";
			copyright = $"Copyright © {DateTime.Today.Year} Company Name.\nAll rights reserved.";
		}
		static Tuple<string, string> GetApplicationName() {
			string name = "";
			string version = "";
			try {
				if(Application.Current != null) {
					var entryType = Application.Current.MainWindow != null ? Application.Current.MainWindow.GetType() : Application.Current.GetType();
					var customAttributes = entryType.Assembly.GetCustomAttributes(true);
					var productName = customAttributes.OfType<AssemblyProductAttribute>().FirstOrDefault();
					var productVersion = customAttributes.OfType<AssemblyVersionAttribute>().FirstOrDefault();
					if(productName != null)
						name = productName.Product;
					if(productVersion != null)
						version = productVersion.Version;
				}
			} catch { }
			if(name.Length == 0 || version.Length == 0)
				try {
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if(entryAssembly != null) {
						var assemblyName = entryAssembly.GetName();
						if(name.Length == 0)
							name = assemblyName.Name;
						if(version.Length == 0)
							version = assemblyName.Version.ToString();
					}
				} catch { }
			return new Tuple<string, string>(name, version);
		}
	}
}
