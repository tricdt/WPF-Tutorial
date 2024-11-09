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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DevExpress.DemoData.Helpers;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf.DemoCenterBase;
namespace DevExpress.Xpf.DemoCenter.Internal {
	public sealed class PlatformViewModel : ImmutableObject {
		static readonly Dictionary<Platform, PlatformViewModel> platformsDictionary;
		public static readonly PlatformViewModel Win = new PlatformViewModel(Repository.WinPlatform);
		public static readonly PlatformViewModel Wpf = new PlatformViewModel(Repository.WpfPlatform);
		public static readonly PlatformViewModel Frameworks = new PlatformViewModel(Repository.FrameworksPlatform);
		public static readonly PlatformViewModel XAFSecurity = new PlatformViewModel(
			platform: Repository.FrameworksPlatform,
			displayName: ".NET App Security & Web API Service",
			imageName: "XAFSecurity",
			url: @"OpenLink:https://www.devexpress.com/go/XAF_Security_NonXAF_Series.aspx");
		public static readonly PlatformViewModel Asp = new PlatformViewModel(Repository.AspPlatform);
		public static readonly PlatformViewModel Mvc = new PlatformViewModel(Repository.MvcPlatform);
		public static readonly PlatformViewModel Blazor = new PlatformViewModel(Repository.BlazorPlatform);
		public static readonly PlatformViewModel DevExtreme = new PlatformViewModel(Repository.DevExtremePlatform);
		public static readonly PlatformViewModel Bootstrap = new PlatformViewModel(Repository.BootstrapPlatform);
		public static readonly PlatformViewModel AspNetCore = new PlatformViewModel(Repository.AspNetCorePlatform);
		public static readonly PlatformViewModel Reporting = new PlatformViewModel(Repository.ReportingPlatform);
		public static readonly PlatformViewModel Dashboards = new PlatformViewModel(Repository.DashboardsPlatform);
		public static readonly PlatformViewModel Docs = new PlatformViewModel(Repository.DocsPlatform);
		public static bool IsNetCore3 { get; } =
#if NET
			true
#else
			false
#endif
			;
		static PlatformViewModel() {
			platformsDictionary = new[] {
				Win, Wpf, Frameworks, Asp, Mvc, Blazor, DevExtreme, Bootstrap, AspNetCore, Reporting, Dashboards, Docs
			}.ToDictionary(x => x.Platform, x => x);
		}
		string imageName;
		PlatformViewModel(Platform platform) {
			Platform = platform;
			StartDemoLauncherCommand = new DelegateCommand(() => DemoRunner.TryStartDemoChooserAndShowErrorMessage(Platform, EmptyArray<string>.Instance));
			IsInstalled =
				platform == Repository.DevExtremePlatform
				? File.Exists(StartApplicationHelper.GetDemoFullPath(platform.DemoLauncherPath))
				: platform.IsInstalled
			;
			Name = platform.Name;
		}
		PlatformViewModel(Platform platform, string displayName, string imageName, string url) : this(platform) {
			DisplayName = displayName;
			this.imageName = imageName;
			StartDemoLauncherCommand = DemoRunner.CreateUrlCommand(url, Platform, CallingSite.DemoCenter);
		}
		public readonly Platform Platform;
		public bool IsInstalled { get; }
		public string Name { get; }
		public string ImageName { get { return imageName ?? Name; } }
		public string DisplayName { get; }
		public ICommand StartDemoLauncherCommand { get; }
		public static PlatformViewModel GetModel(Platform platform) {
			PlatformViewModel model;
			return platformsDictionary.TryGetValue(platform, out model) ? model : null;
		}
	}
}
