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
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using DevExpress.DemoData;
using DevExpress.DemoData.Helpers;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Utils.Text;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
using DevExpress.Utils.Text.Internal;
using System.Windows;
namespace DevExpress.Xpf.DemoChooser.Internal {
	public class PlatformPageViewModel : ViewModelBase {
		object selectedItem;
		public readonly Action<Product> SelectProduct;
		public PlatformPageViewModel(Platform platform, Action<Product> selectProduct) {
			SelectProduct = selectProduct;
			Platform = platform;
			if(platform == Repository.AspPlatform) {
				AspDemosLoader.LoadASPDemos();
			} else if(platform == Repository.MvcPlatform) {
				AspDemosLoader.LoadMVCDemos();
			} else if(platform == Repository.AspNetCorePlatform) {
				AspDemosLoader.LoadAspNetCoreDemos();
			} else if(platform == Repository.BootstrapPlatform) {
				AspDemosLoader.LoadBootstrapDemos();
			}
			UseLegacyThemesVisibility = platform == Repository.WpfPlatform && !EnvironmentHelper.IsClickOnce ? Visibility.Visible : Visibility.Collapsed;
			useLegacyThemes = GetUseLegacyThemes(platform.Name);
			UpdateProducts();
		}
		public bool IsWinPage { get { return Platform == Repository.WinPlatform; } }
		public bool IsAspNetCorePage { get { return Platform == Repository.AspNetCorePlatform; } }
		public bool IsBootstrapPage { get { return Platform == Repository.BootstrapPlatform; } }
		public bool IsBlazorPage { get { return Platform == Repository.BlazorPlatform; } }
		IEnumerable<object> dashboardFinancialProducts;
		public IEnumerable<object> DashboardFinancialProducts {
			get { return dashboardFinancialProducts; }
			set { SetProperty(ref dashboardFinancialProducts, value, () => DashboardFinancialProducts); }
		}
		public Platform Platform { get; }
		public object SelectedItem {
			get { return selectedItem; }
			set { SetProperty(ref selectedItem, value, nameof(SelectedItem)); }
		}
		public string DemosTitle { get { return Platform.DisplayName + " Product Demos"; } }
		public Visibility UseLegacyThemesVisibility {
			get { return GetProperty(() => UseLegacyThemesVisibility); }
			set { SetProperty(() => UseLegacyThemesVisibility, value); }
		}
		bool useLegacyThemes;
		public bool UseLegacyThemes {
			get { return useLegacyThemes; }
			set { SetProperty(ref useLegacyThemes, value, () => UseLegacyThemes, () => SetUseLegacyThemes(Platform.Name, value)); }
		}
		static readonly Dictionary<string, bool> optionStorage = new Dictionary<string, bool>();
		static void SetUseLegacyThemes(string platformName, bool value) {
			if(optionStorage.ContainsKey(platformName))
				optionStorage[platformName] = value;
			else
				optionStorage.Add(platformName, value);
		}
		public static bool GetUseLegacyThemes(string platformName) {
			return DictionaryExtensions.GetValueOrDefault(optionStorage, platformName, false);
		}
		void UpdateProducts() {
			UpdateProductsCollection(Platform.Products.Where(x => x.ShowInDemoChooser).ToList());
		}
		public IEnumerable<GroupedLinks> Groups {
			get { return GetProperty(() => Groups); }
			set { SetProperty(() => Groups, value); }
		}
		public IEnumerable<GroupedLinks> EmptyGroups {
			get { return GetProperty(() => EmptyGroups); }
			set { SetProperty(() => EmptyGroups, value); }
		}
		public IEnumerable<DemoCarouselItem> FeaturedDemos {
			get { return GetProperty(() => FeaturedDemos); }
			set { SetProperty(() => FeaturedDemos, value); }
		}
		public string FeaturedDemosTitle { get { return Platform == Repository.BlazorPlatform ? "Featured Demos" : "Sample Applications"; } }
		public int ProductsViewMaxGroupsPerColumn { get { return Platform == Repository.AspNetCorePlatform ? 1 : 2; } }
		static DemoCarouselItem CarouselItem(BaseReallifeDemo demo) {
			IEnumerable<DemoCarouselLink> links = EnvironmentHelper.IsClickOnce
				? new ReadOnlyObservableCollection<DemoCarouselLink>(new ObservableCollection<DemoCarouselLink>())
				: demo
					.CreateOpenSolutionMenu(CallingSite.DemoChooser)
					.Select(i => new DemoCarouselLink { Title = DemoRunner.FormatOpenSolution(i.IsVB, i.IsNetCore), SelectedCommand = i.OpenCommand })
					.ToReadOnlyObservableCollection()
			;
			return new DemoCarouselItem(
				platformLabel: demo.ShowTryRtlTag ? "Try RTL Mode" : string.Empty,
				name: demo.Name,
				title: demo.DisplayName,
				preview: demo.MediumImage.Uri,
				isAvailable: string.IsNullOrEmpty(demo.ComponentName) || Linker.IsProductInstalled(demo.ComponentName),
				isUniversal: demo.IsUniversal,
				onRunCommand: new DelegateCommand(() => {
					demo.Match(
						rlDemo => DemoRunner.TryStartReallifeDemoAndShowErrorMessage(rlDemo, CallingSite.DemoChooser),
						wpfDemo => DemoRunner.LoadAndRunWpfDemo(wpfDemo.AssemblyName, false.AsLeft()),
						link => DemoRunner.StartWebLinkApp(link)
					);
				}),
				links: links
			);
		}
		void UpdateProductsCollection(List<Product> products) {
			Groups = null;
			EmptyGroups = null;
			if(products.SelectMany(p => p.Demos.SelectMany(d => d.Match(x => x.Modules, x => x.Modules, _ => EmptyArray<Module>.Instance, x => x.Modules, _ => Enumerable.Empty<Module>()))).Any()) {
				Groups = products.Select(product => {
					var demos = product.Demos
						.Where(d => !d.Match(x => x.Modules, x => x.Modules, _ => Enumerable.Empty<Module>(), x => x.Modules, _ => Enumerable.Empty<Module>()).Any())
						.Select(d => new GroupedLink(d.DisplayName, d.DisplayName, d.DisplayName.Yield().Concat(d.Tags).ToArray(), null, () => DemoRunner.TryStartDemoAndShowErrorMessage(d, CallingSite.DemoChooser, false)))
					;
					var modules = product.Demos.SelectMany(d => d.Match(x => x.Modules.Where(m => !m.IsOutdated && m.Name != "About"), x => x.Modules, _ => Enumerable.Empty<Module>(), x => x.Modules, _ => Enumerable.Empty<Module>()))
						.Where(m => m.ShowInDemoChooser)
						.OrderByDescending(m => m.Match(x => x.IsFeatured, _ => false, _ => false))
						.Select(m => new GroupedLink(m.DisplayName, m.DisplayName, m.DisplayName.Yield().Concat(m.Tags).ToArray(), m.Match(x => DocumentPresenterHelper.FormatText(x.Description), x => DocumentPresenterHelper.FormatText(x.ShortDescription), _ => null), () => DemoRunner.TryStartDemoModuleAndShowErrorMessage(m, CallingSite.DemoChooser)))
					;
					return new GroupedLinks(
						header: product.DisplayName.Replace("\r\n", " "),
						contextMenu: new ReadOnlyObservableCollection<GroupedLinkContextMenuItem>(new ObservableCollection<GroupedLinkContextMenuItem>(
							product
								.Demos.First().CreateOpenSolutionMenu(CallingSite.DemoChooser)
								.Select(i => new GroupedLinkContextMenuItem { Title = DemoRunner.FormatOpenSolution(i.IsVB, i.IsNetCore), SelectedCommand = i.OpenCommand })
							)),
						links: demos.Concat(modules).ToReadOnlyCollection(),
						showAllCommand: new DelegateCommand(() => SelectProduct(product))
					);
				}).ToReadOnlyObservableCollection();
			}
			EmptyGroups = (from p in products
						   group p by p.Group into g
						   let productWithGroupOrder = g.FirstOrDefault(x => x.GroupOrder != -1) ?? products.First()
						   orderby productWithGroupOrder.GroupOrder
						   select new GroupedLinks(
							   header: g.Key,
							   links: g.Select(product => {
								   var productLink = new ProductLink(
									   platformName: Platform.Name,
									   product: product,
									   supportsDirectX: product.SupportsDirectX,
									   title: product.DisplayName.Replace("\r\n", " "),
									   contextMenu: product.Demos.Count == 0 ? null : new ReadOnlyObservableCollection<GroupedLinkContextMenuItem>(new ObservableCollection<GroupedLinkContextMenuItem>(
									   product
										   .Demos.First().CreateOpenSolutionMenu(CallingSite.DemoChooser)
										   .Select(i => new GroupedLinkContextMenuItem { Title = DemoRunner.FormatOpenSolution(i.IsVB, i.IsNetCore), SelectedCommand = i.OpenCommand })
									   ))
								   );
								   return new GroupedLink(productLink, productLink.Title, productLink.Title.YieldToArray(), null, () => SelectProduct(product));
							   }).ToReadOnlyCollection(),
							   contextMenu: null, showAllCommand: null
						   )).ToReadOnlyObservableCollection();
			var reallifeDemos = products.First().Platform.ReallifeDemos.AsEnumerable();
			if(EnvironmentHelper.IsClickOnce)
				reallifeDemos = reallifeDemos.Where(x => x.IsAvailableInClickonce);
			FeaturedDemos = reallifeDemos.Select(CarouselItem).ToReadOnlyObservableCollection();
		}
	}
	public class ProductLink : BindableBase {
		public ProductLink(string platformName, Product product, string title, bool supportsDirectX, ReadOnlyObservableCollection<GroupedLinkContextMenuItem> contextMenu) {
			PlatformName = platformName;
			Product = product;
			this.SupportsDirectX = supportsDirectX;
			this.Title = title;
			this.ContextMenu = contextMenu;
			this.useGdi = PreferGdi(Product.Name);
		}
		public readonly Product Product;
		public bool IsAvailable { get { return Product.IsInstalled; } }
		public string PlatformName { get; }
		public bool SupportsDirectX { get; }
		public string Title { get; }
		public ReadOnlyObservableCollection<GroupedLinkContextMenuItem> ContextMenu { get; }
		bool useGdi;
		public bool UseGdi {
			get { return useGdi; }
			set { SetProperty(ref useGdi, value, () => UseGdi, () => SetPreferGdi(Product.Name, value)); }
		}
		static readonly Dictionary<string, bool> preferGdi = new Dictionary<string, bool>();
		static void SetPreferGdi(string productName, bool useGdi) {
			if(preferGdi.ContainsKey(productName))
				preferGdi[productName] = useGdi;
			else
				preferGdi.Add(productName, useGdi);
		}
		public static bool PreferGdi(string productName) {
			return DictionaryExtensions.GetValueOrDefault(preferGdi, productName, false);
		}
	}
	public static class DocumentPresenterHelper {
		public static string FormatText(string text) {
			var parseInfo = StringParser.Parse(10, text, true);
			return string.Concat(ProcessStringBlocks(parseInfo));
		}
		static IEnumerable<string> ProcessStringBlocks(List<StringBlock> blocks) {
			return blocks.Select(x => x.Match(
					link: () => $"<Paragraph><Hyperlink NavigateUri=\"{x.Link}\" xml:space=\"preserve\">{x.Text}</Hyperlink></Paragraph>",
					fallback: () => $"<Paragraph>{x.Text}</Paragraph>"
				)
			);
		}
		static string Match(this StringBlock block, Func<string> link = null, Func<string> text = null, Func<string> fallback = null) {
			if(link != null && block.Type == StringBlockType.Link)
				return link();
			if(text != null && block.Type == StringBlockType.Text)
				return text();
			return fallback();
		}
	}
}
