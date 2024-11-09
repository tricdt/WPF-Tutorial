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
using System.Linq;
using DevExpress.DemoData.Model;
using DevExpress.Xpf.DemoCenter.Internal;
namespace DevExpress.Xpf.DemoCenter {
	public static class PlatformTitles {
		public static string GetTitle(PlatformViewModel platformViewModel) {
			if(!string.IsNullOrEmpty(platformViewModel.DisplayName))
				return platformViewModel.DisplayName;
			var platform = platformViewModel.Platform;
			if(platform == Repository.WinPlatform) return "WinForms";
			if(platform == Repository.WpfPlatform) return "WPF";
			if(platform == Repository.AspPlatform) return "ASP.NET Web Forms";
			if(platform == Repository.MvcPlatform) return "ASP.NET MVC";
			if(platform == Repository.AspNetCorePlatform) return "ASP.NET Core";
			if(platform == Repository.BlazorPlatform) return "Blazor";
			if(platform == Repository.DevExtremePlatform) return "DevExtreme Complete";
			if(platform == Repository.BootstrapPlatform) return "Bootstrap";
			if(platform == Repository.ReportingPlatform) return "Reporting";
			if(platform == Repository.DashboardsPlatform) return "BI Dashboard";
			if(platform == Repository.DocsPlatform) return "Office File API";
			if(platform == Repository.FrameworksPlatform) return "Cross-Platform .NET App UI";
			throw new InvalidOperationException();
		}
		public static string GetLabel(Platform platform) {
			if(platform == Repository.WinPlatform) return "WIN";
			if(platform == Repository.WpfPlatform) return "WPF";
			if(platform == Repository.AspPlatform) return "ASP";
			if(platform == Repository.MvcPlatform) return "MVC";
			if(platform == Repository.BlazorPlatform) return "Blazor";
			if(platform == Repository.BootstrapPlatform) return "Bootstrap";
			throw new InvalidOperationException();
		}
	}
}
