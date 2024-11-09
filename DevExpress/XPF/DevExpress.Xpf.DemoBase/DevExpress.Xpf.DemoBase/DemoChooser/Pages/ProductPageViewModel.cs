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
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoCenter;
using DevExpress.Xpf.DemoCenter.Internal;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
using DevExpress.Xpf.DemoChooser.Helpers;
namespace DevExpress.Xpf.DemoChooser.Internal {
	public class ProductPageViewModel : ViewModelBase {
		object selectedItem;
		static CrossPlatformDemoCarouselItem CarouselItem(Demo demo) {
			var runCommand = new DelegateCommand(() => DemoRunner.TryStartDemoAndShowErrorMessage(demo, CallingSite.DemoChooser, false));
			return new CrossPlatformDemoCarouselItem(
				demo.Name,
				demo.DisplayName,
				PlatformTitles.GetTitle(PlatformViewModel.GetModel(demo.Product.Platform)),
				true,
				false,
				demo.Description,
				runCommand,
				new DemoImage(string.Format("{0}/{1}/{2}.Medium.png", demo.Product.Platform.Name, demo.Product.Name, demo.Name)).Uri,
				demo.CreateOpenSolutionMenu(CallingSite.DemoChooser).Select(x => x.CarouselLink()).Concat(demo.AdditionalLinks?.Select(x => x.CarouselLink(demo.Product.Platform)) ?? EmptyArray<DemoCarouselLink>.Instance).ToReadOnlyObservableCollection(),
				new DemoCarouselLink() { Title = "Demo", SelectedCommand = runCommand }.YieldToArray(),
				demo.StaticLinks?.Select(x => x.CarouselLink(demo.Product.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection(),
				demo.StaticLinksTitle
			);
		}
		readonly Product product;
		public ProductPageViewModel(Product product) {
			this.product = product;
			IsXtraBarsPage = product.Name == "XtraBars";
			ProductTitle = product.DisplayName.Replace("\r", "").Replace("\n", " ");
		}
		public bool IsXtraBarsPage { get; }
		public string ProductTitle { get; }
		public string ProductDescription { get { return product.Description; } }
		public object SelectedItem {
			get { return selectedItem; }
			set { SetProperty(ref selectedItem, value, nameof(SelectedItem)); }
		}
		public IEnumerable<CrossPlatformDemoCarouselItem> FeaturedDemos {
			get {
				return product.Demos.Select(CarouselItem).ToReadOnlyObservableCollection();
			}
		}
	}
}
