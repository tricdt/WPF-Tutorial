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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoCenter;
using DevExpress.Xpf.DemoCenter.Internal;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
using DevExpress.Xpf.DemoChooser.Helpers;
namespace DevExpress.Xpf.DemoChooser.Internal {
	public class FrameworkPageViewModel : CrossPlatformPageViewModel {
		public FrameworkPageViewModel(Platform platform) : base(platform) { }
		protected static IEnumerable<DemoCarouselLink> GetXAFDemoLinks(FrameworkDemo demo) {
			if(!string.IsNullOrEmpty(demo.WinPath))
				yield return new DemoCarouselLink() { Title = "WinForms Demo", SelectedCommand = CreateFrameworkDemoCommand(demo, demo.WinPath) };
			if(!string.IsNullOrEmpty(demo.MobilePath))
				yield return new DemoCarouselLink() { Title = "ASP.NET Core Blazor Demo", SelectedCommand = CreateFrameworkDemoCommand(demo, demo.MobilePath) };
			if(!string.IsNullOrEmpty(demo.WebPath))
				yield return new DemoCarouselLink() { Title = "ASP.NET WebForms Demo", SelectedCommand = CreateFrameworkDemoCommand(demo, demo.WebPath) };
		}
		static CrossPlatformDemoCarouselItem CarouselItem(FrameworkDemo demo) {
			return new CrossPlatformDemoCarouselItem(
				demo.Name,
				demo.DisplayName,
				PlatformTitles.GetTitle(PlatformViewModel.GetModel(demo.Product.Platform)),
				demo.Product.IsInstalled,
				false,
				demo.Description,
				CreateFrameworkDemoCommand(demo),
				demo.MediumImage.Uri,
				demo.CreateOpenSolutionMenu(CallingSite.DemoChooser).Select(x => x.CarouselLink()).Concat(demo.AdditionalLinks?.Select(x => x.CarouselLink(demo.Product.Platform)) ?? EmptyArray<DemoCarouselLink>.Instance).ToReadOnlyObservableCollection(),
				demo.DemoLinks?.Select(x => x.CarouselLink(demo.Product.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection() ?? GetXAFDemoLinks(demo),
				demo.StaticLinks?.Select(x => x.CarouselLink(demo.Product.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection(),
				demo.StaticLinksTitle,
				demo.Tutorials?.Select(x => x.CarouselLink(demo.Product.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection()
			);
		}
		protected override void UpdateProducts() {
			FeaturedDemos = Platform.Products.First().Demos.Cast<FrameworkDemo>().Where(t => t.XafDemoContainerType != XafDemoContainerType.Links).Select(CarouselItem).ToReadOnlyObservableCollection();
			FeaturedLinks = Platform.Products.First().Demos.Cast<FrameworkDemo>().Where(t => t.XafDemoContainerType != XafDemoContainerType.Demo).Select(CarouselItem).ToReadOnlyObservableCollection();
			return;
		}
		public IEnumerable<CrossPlatformDemoCarouselItem> FeaturedLinks { get { return GetValue<IEnumerable<CrossPlatformDemoCarouselItem>>(); } protected set { SetValue(value); } }
	}
}
