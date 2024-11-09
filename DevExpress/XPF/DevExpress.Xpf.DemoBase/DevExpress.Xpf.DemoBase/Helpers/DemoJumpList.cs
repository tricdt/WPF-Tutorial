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

#if !NET
using System;
using System.Linq;
using System.Windows.Media.Imaging;
using DevExpress.DemoData;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public static class DemoJumpList {
		public static void Apply(string platformName) {
			try {
				var service = new ApplicationJumpListService();
				string uri = string.Format("pack://application:,,,/DevExpress.Xpf.DemoBase.{0};component/DemoChooser/", AssemblyInfo.VSuffixWithoutSeparator);
				service.AddOrReplace(new Mvvm.ApplicationJumpTaskInfo {
					Title = "Explore DevExpress Universal",
					CustomCategory = "Become a UI Superhero",
					Icon = BitmapFrame.Create(new Uri(uri + "Icons/Universal.ico", UriKind.Absolute)),
					Action = () => DocumentPresenter.OpenLink(LinksHelper.UniversalSubscriptionLink),
					ApplicationPath = LinksHelper.UniversalSubscriptionLink
				});
				service.AddOrReplace(new Mvvm.ApplicationJumpTaskInfo {
					Title = "Online Tutorials",
					CustomCategory = "Become a UI Superhero",
					Action = () => DocumentPresenter.OpenLink(LinksHelper.GetStarted(platformName)),
					ApplicationPath = LinksHelper.GetStarted(platformName)
				});
				service.AddOrReplace(new Mvvm.ApplicationJumpTaskInfo {
					Title = "Buy Now",
					CustomCategory = "Become a UI Superhero",
					Icon = BitmapFrame.Create(new Uri(uri + "Icons/DevExpress.ico", UriKind.Absolute)),
					Action = () => DocumentPresenter.OpenLink(LinksHelper.BuyNow(platformName)),
					ApplicationPath = LinksHelper.BuyNow(platformName)
				});
				service.Apply();
			} catch { }
		}
	}
}
#endif
