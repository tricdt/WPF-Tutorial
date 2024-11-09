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
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.DemoData.Core;
using DevExpress.DemoData.Helpers;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoChooser.Internal;
using System.Windows.Threading;
namespace DevExpress.Xpf.DemoChooser {
	public class Program {
#if !NET
		[ServiceContract]
		interface IWpfDemoChooser {
			[OperationContract]
			void ShowMainWindow();
		}
		[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
		class WpfDemoChooser : IWpfDemoChooser {
			readonly string platformName;
			readonly TaskScheduler scheduler;
			public WpfDemoChooser(string platformName, TaskScheduler scheduler) {
				this.platformName = platformName;
				this.scheduler = scheduler;
			}
			void IWpfDemoChooser.ShowMainWindow() {
				default(UnitT).Promise().Execute(() => Show(platformName), scheduler);
			}
		}
#endif
		public static void DemoChooserMain(bool debug, string platform) {
			DevExpress.Data.Utils.ProcessStartPolicy.RegisterTrustedProcess("DemoChooser"); 
			var id = "DevExpressDemoChooser" + AssemblyInfo.VersionShort + platform + Core3 + (EnvironmentHelper.IsClickOnce ? "_co" : "");
			RunOnceHelper.BeginApplicationStart(id, 19, runOnceHelper => {
				var app = new App(debug, platform);
				app.Startup += (_, __) => {
					if(string.Equals(platform, "Wpf", StringComparison.OrdinalIgnoreCase))
						PreloadWpfDB();
					app.MainWindow = Create(platform);
					runOnceHelper.EndApplicationStart(c => {
						if(c == 19)
							Show(platform);
					});
					app.MainWindow.Show();
				};
				app.Run();
			});
		}
		[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
		static void PreloadWpfDB() {
			if(!EnvironmentHelper.IsClickOnce) {
				ApplicationThemeHelper.PreloadAsync(PreloadCategories.Core, PreloadCategories.Accordion, PreloadCategories.Ribbon, PreloadCategories.Docking, PreloadCategories.LayoutControl, PreloadCategories.Grid);
				new TaskFactory()
					.StartNew(() => DevExpress.DemoData.Models.NWindContext.Preload())
					.ContinueWith(t => DevExpress.DemoData.Models.Vehicles.VehiclesContext.Preload(), TaskContinuationOptions.ExecuteSynchronously);
			}
		}
#if NET
		const string Core3 = "_c3";
#else
		const string Core3 = "";
#endif
		static DemoChooserWindow mainWindow;
		static void Show(string platformName) {
			if(mainWindow != null) {
				if(mainWindow.WindowState == WindowState.Minimized)
					mainWindow.WindowState = WindowState.Normal;
				mainWindow.Activate();
			} else {
				Create(platformName).Show();
			}
		}
		static Window Create(string platformName) {
			if(mainWindow != null)
				throw new InvalidOperationException("DemoChooserMainWindow");
			var window = new DemoChooserWindow();
			window.Closed += (_, __) => {
				if(mainWindow == window)
					mainWindow = null;
			};
			mainWindow = window;
			mainWindow.Closing += OnMainWindowClosing;
			window.DataContext = new DemoChooserWindowViewModel(platformName);
			return window;
		}
		static void OnMainWindowClosing(object sender, CancelEventArgs e) {
			MessageBoxResult result = WebDevServerHelper.CheckAgreeCloseWebServers((Window)sender);
			if(result == MessageBoxResult.Cancel)
				e.Cancel = true;
			else if(result == MessageBoxResult.Yes)
				WebDevServerHelper.CloseWebServers();
		}
		class App : Application {
			public App(bool debug, string platform) {
				ApplicationThemeHelper.ApplicationThemeName = DemoBase.DemoBaseControl.ActualDefaultTheme.Name;
				this.debug = debug;
			}
			readonly bool debug;
			public bool IsDebug { get { return debug; } }
		}
	}
}
