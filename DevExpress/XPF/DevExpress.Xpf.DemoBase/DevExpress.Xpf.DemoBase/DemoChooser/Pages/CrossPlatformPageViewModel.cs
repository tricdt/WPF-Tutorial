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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Data.Extensions;
using DevExpress.DemoData;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoCenter;
using DevExpress.Xpf.DemoCenter.Internal;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
using DevExpress.Xpf.DemoChooser.Helpers;
namespace DevExpress.Xpf.DemoChooser.Internal {
	public class CrossPlatformPageViewModel : ViewModelBase {
		protected static IEnumerable<DemoCarouselLink> GetFrameworkDemoLinks(FrameworkDemo demo) {
			if(!string.IsNullOrEmpty(demo.WinPath))
				yield return new DemoCarouselLink() { Title = "WinForms Demo", SelectedCommand = CreateFrameworkDemoCommand(demo, demo.WinPath) };
			if(!string.IsNullOrEmpty(demo.WebPath))
				yield return new DemoCarouselLink() { Title = "ASP.NET Demo", SelectedCommand = CreateFrameworkDemoCommand(demo, demo.WebPath) };
			if(!string.IsNullOrEmpty(demo.MobilePath))
				yield return new DemoCarouselLink() { Title = "Blazor Demo", SelectedCommand = CreateFrameworkDemoCommand(demo, demo.MobilePath) };
		}
		protected static ICommand CreateFrameworkDemoCommand(FrameworkDemo demo, string path = null) {
			var launchPath = path ?? demo.BuildItPath;
			var arguments = demo.GetArguments(launchPath);
			return new DelegateCommand(() => {
				DemoRunner.TryStartAndShowErrorMessage(
					demo.Requirements,
					launchPath,
					arguments,
					demo.Product.Platform,
					string.Format("StartDemo:{0}", demo.Name), CallingSite.DemoChooser, true);
			});
		}
		static CrossPlatformDemoCarouselItem CarouselItem(Product product) {
			Debug.Assert(product.Demos.Count == 1);
			var demo = product.Demos.Single();
			var runCommand = new DelegateCommand(() => {
				DemoRunner.TryStartDemoAndShowErrorMessage(product.Demos.First(), CallingSite.DemoChooser, false);
			});
			return new CrossPlatformDemoCarouselItem(
				product: product,
				isUniversal: false,
				description: demo.Description,
				onRunCommand: runCommand,
				links: demo.CreateOpenSolutionMenu(CallingSite.DemoChooser).Select(x => x.CarouselLink()).Concat(demo.AdditionalLinks?.Select(x => x.CarouselLink(product.Platform)) ?? EmptyArray<DemoCarouselLink>.Instance).ToReadOnlyObservableCollection(),
				demoLinks: new DemoCarouselLink() { Title = "Demo", SelectedCommand = runCommand }.YieldToArray(),
				staticLinks: demo.StaticLinks?.Select(x => x.CarouselLink(product.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection(),
				staticLinksTitle: demo.StaticLinksTitle
			);
		}
		static CrossPlatformDemoCarouselItem CarouselItem(BaseReallifeDemo demo) {
			var runCommand = new DelegateCommand(() => {
				demo.Match(
					rlDemo => DemoRunner.TryStartReallifeDemoAndShowErrorMessage(rlDemo, CallingSite.DemoChooser),
					wpfDemo => DemoRunner.LoadAndRunWpfDemo(wpfDemo.AssemblyName, false.AsLeft()),
					link => DemoRunner.StartWebLinkApp(link)
				);
			});
			return new CrossPlatformDemoCarouselItem(
				demo.Name,
				demo.DisplayName,
				PlatformTitles.GetTitle(PlatformViewModel.GetModel(demo.Platform)),
				string.IsNullOrEmpty(demo.ComponentName) || Linker.IsProductInstalled(demo.ComponentName),
				demo.IsUniversal,
				null,
				runCommand,
				demo.MediumImage.Uri,
				demo.CreateOpenSolutionMenu(CallingSite.DemoChooser).Select(x => x.CarouselLink()).Concat(demo.AdditionalLinks?.Select(x => x.CarouselLink(demo.Platform)) ?? EmptyArray<DemoCarouselLink>.Instance).ToReadOnlyObservableCollection(),
				new DemoCarouselLink() { Title = "Demo", SelectedCommand = runCommand }.YieldToArray(),
				demo.StaticLinks?.Select(x => x.CarouselLink(demo.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection(),
				null
				);
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
				GetFrameworkDemoLinks(demo),
				demo.StaticLinks?.Select(x => x.CarouselLink(demo.Product.Platform)).NullIfEmpty()?.ToReadOnlyObservableCollection(),
				demo.StaticLinksTitle
			);
		}
		public readonly Platform Platform;
		public ImageSource ProductIcon { get { return Platform.Icon.GetImageSource(); } }
		public string ProductTitle { get { return Platform.ProductListTitle; } }
		public string PlatformDescription { get { return Platform.Description; } }
		bool IsDocsPage { get { return Platform == Repository.DocsPlatform; } }
		bool IsReportsPage { get { return Platform == Repository.ReportingPlatform; } }
		bool IsDashboardsPage { get { return Platform == Repository.DashboardsPlatform; } }
		bool IsFrameworksPage { get { return Platform == Repository.FrameworksPlatform; } }
		public IEnumerable<CrossPlatformDemoCarouselItem> FeaturedDemos { get { return GetValue<IEnumerable<CrossPlatformDemoCarouselItem>>(); } protected set { SetValue(value); } }
		public CrossPlatformPageViewModel(Platform platform) {
			Platform = platform;
			UpdateProducts();
		}
		protected virtual void UpdateProducts() {
			if(IsFrameworksPage) {
				FeaturedDemos = Platform.Products.First().Demos.Cast<FrameworkDemo>().Select(CarouselItem).ToReadOnlyObservableCollection();
				return;
			}
			var visibleProducts = (Platform.Products.Where(x => x.ShowInDemoChooser).ToList());
			var demos = visibleProducts.Select(CarouselItem);
#if NET
			if(IsReportsPage) {
				var reallifeDemos = Platform.ReallifeDemos.Select(CarouselItem);
				demos = demos.Concat(reallifeDemos);
			}
#endif
			if(IsDashboardsPage) {
				var reallifeDemos = visibleProducts.First().Platform.ReallifeDemos.Select(CarouselItem);
				demos = demos.Concat(reallifeDemos);
				demos = demos.OrderBy(d => {
					return new[] {
							"WebDashboard",
							"DashboardForWin",
							"BlazorDashboard",
							"WPFDashboardDemo",
							"WinFormsDesigner",
							"FinanceTablet",
						}.FindIndex(n => n == d.Name);
				});
			}
			FeaturedDemos = demos.ToReadOnlyObservableCollection();
		}
	}
}
