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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.DemoData;
using DevExpress.DemoData.Helpers;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Xpf.DemoCenterBase;
namespace DevExpress.Xpf.DemoBase {
	public class DataLoaderActions {
		public Action<BaseReallifeDemo> StartFeaturedDemo;
		public Action<ModuleDescription> OnModuleDescriptionSelected;
		public Action<ProductDescription> OnProductDescriptionSelected;
	}
	public class DataLoader {
		string platformName;
		string mainProductID;
		Platform platform;
		public List<ProductDescription> Products { get; private set; }
		public ProductDescription MainProduct { get; private set; }
		public string Title { get; private set; }
		public DataLoader(string platform, string mainProjectID) {
			this.platformName = platform;
			this.mainProductID = mainProjectID;
		}
		internal static Platform DefaultPlatform = Repository.WpfPlatform;
		public PlatformDescription LoadCurrentPlatform() {
			var platformDescription = new PlatformDescription();
			platformDescription.ID = new PlatformID();
			platform = DefaultPlatform;
			platformDescription.Title = platform.ProductListTitle;
			platformDescription.Subtitle = platform.ProductListSubtitle;
			platformDescription.Path = platform.Name;
			return platformDescription;
		}
		public void LoadProducts() {
			List<ProductDescription> data = new List<ProductDescription>();
			foreach(var demo in platform.ReallifeDemos) {
				if((EnvironmentHelper.IsClickOnce || EnvironmentHelper.IsWinStore) && !demo.IsAvailableInClickonce) continue;
				data.Add(LoadFeaturedDemo(demo, platform));
			}
			List<string> groups = new List<string>();
			Dictionary<string, List<ProductDescription>> pds = new Dictionary<string, List<ProductDescription>>();
			foreach(var product in platform.Products) {
				if (!product.IsInstalled)
					continue;
				if ((EnvironmentHelper.IsClickOnce || EnvironmentHelper.IsWinStore) && !product.IsAvailableOnline)
					continue;
				string groupNL = product.Group;
				List<ProductDescription> groupPds;
				if(!pds.TryGetValue(groupNL, out groupPds)) {
					groupPds = new List<ProductDescription>();
					groups.Add(groupNL);
					pds.Add(groupNL, groupPds);
				}
				ProductDescription pd = LoadProduct(product);
				groupPds.Add(pd);
			}
			foreach(string group in groups) {
				List<ProductDescription> groupPds = pds[group];
				foreach(ProductDescription pd in groupPds) {
					data.Add(pd);
				}
			}
			Products = data;
		}
		ProductDescription LoadProduct(Product product) {
			ProductDescription pd = new ProductDescription(this, product);
			pd.IsNew = product.IsNew;
			pd.IsUpdated = product.IsUpdated;
			pd.DisplayName = product.DisplayName;
			pd.Name = product.Name;
			pd.GroupTitle = product.Group;
			pd.IsFeaturedDemo = false;
			pd.Title = product.DisplayName;
			pd.CSSolutionPath = Path.GetDirectoryName(product.Demos.First().CsSolutionPath);
			pd.VBSolutionPath = Path.GetDirectoryName(product.Demos.First().VbSolutionPath);
			return pd;
		}
		public ReadOnlyCollection<ModuleDescription> LoadModules(ProductDescription pd, WpfDemo demo) {
			List<string> groups = new List<string>();
			Dictionary<string, List<ModuleDescription>> mds = new Dictionary<string, List<ModuleDescription>>();
			foreach(var module in demo.Modules.Cast<WpfModule>()) {
				string groupNL = module.Group;
				List<ModuleDescription> groupMds;
				if(!mds.TryGetValue(groupNL, out groupMds)) {
					groupMds = new List<ModuleDescription>();
					groups.Add(groupNL);
					mds.Add(groupNL, groupMds);
				}
				bool isCurrentDemo = pd.Name == this.mainProductID;
				ModuleDescription md = LoadModule(isCurrentDemo, demo, module);
				md.Product = pd;
				if(pd.Name != "DXEditors" || ShouldShowDXEditorsModule(md)) {
					groupMds.Add(md);
				}
			}
			List<ModuleDescription> data = new List<ModuleDescription>();
			foreach(string group in groups) {
				List<ModuleDescription> groupMds = mds[group];
				foreach(ModuleDescription md in groupMds) {
					data.Add(md);
				}
			}
			return data.AsReadOnly();
		}
		bool ShouldShowDXEditorsModule(ModuleDescription md) {
			return true;
		}
		ModuleDescription LoadModule(bool isCurrentDemo, WpfDemo demo, WpfModule module) {
			ModuleDescription md = new ModuleDescription();
			md.WpfModule = module;
			md.DemoAssemblyName = demo.AssemblyName;
			md.Name = module.Name;
			md.Title = module.DisplayName;
			md.AllowRtl = module.AllowRtl;
			md.GroupName = module.Group;
			md.GroupTitle = module.Group;
			md.IsHighlighted = module.IsFeatured;
			md.IsNew = module.IsNew;
			md.IsUpdated = module.IsUpdated;
			md.Description = module.Description;
			md.ShortDescription = module.ShortDescription;
			md.AllowSwitchingTheme = module.AllowSwitchingThemes;
			md.UpdateModuleType = a => {
				Type moduleType = DevExpress.Data.Internal.SafeTypeResolver.GetKnownType(a, module.TypeName);
				if(moduleType == null)
					throw new Exception(string.Format("DemoModule *{0}* not found", module.TypeName));
				md.ModuleType = moduleType;
			};
			if(isCurrentDemo) {
				try { md.UpdateModuleType(null); } catch { }
			}
			return md;
		}
		ProductDescription LoadFeaturedDemo(BaseReallifeDemo featuredDemo, Platform platform) {
			ProductDescription pd = new ProductDescription(this, null);
			LoadFeatureDemoCommon(featuredDemo, pd, platform);
			return pd;
		}
		private void LoadFeatureDemoCommon(BaseReallifeDemo demo, ProductDescription pd, Platform platform) {
			pd.Name = demo.Name;
			pd.DisplayName = demo.DisplayName;
			pd.GroupTitle = demo.Group ?? "Sample Applications";
			pd.IsFeaturedDemo = true;
			pd.Title = demo.DisplayName;
		}
	}
	public sealed class ProductDescription : BindableBase {
		bool demosLoaded = false;
		object demosLoadedLock = new object();
		DataLoader dataLoader;
		public ProductDescription(DataLoader loader, DevExpress.DemoData.Model.Product realProduct) {
			IsFeaturedDemo = realProduct == null;
			dataLoader = loader;
			DemoDataProduct = realProduct;
		}
		public void OpenCSSolution(string[] filesToOpen) {
			var demo = DemoDataProduct.Demos.First();
			DemoRunner.OpenSolution(demo, x => x.CsSolutionPath, DemoRunner.GetOpenSolutionMessage(demo, CallingSite.WpfDemo, false), true, filesToOpen);
		}
		public void OpenVBSolution(string[] filesToOpen) {
			var demo = DemoDataProduct.Demos.First();
			DemoRunner.OpenSolution(demo, x => x.VbSolutionPath, DemoRunner.GetOpenSolutionMessage(demo, CallingSite.WpfDemo, true), true, filesToOpen);
		}
		public DevExpress.DemoData.Model.Product DemoDataProduct { get; set; }
		public bool IsNew { get; set; }
		public bool IsUpdated { get; set; }
		public string Filter { get; set; }
		public bool IsFeaturedDemo { get; set; }
		public string DisplayName { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string GroupTitle { get; set; }
		public ReadOnlyCollection<ModuleDescription> Modules { get; set; }
		public ReadOnlyCollection<DemoDescription> Demos { get; set; }
		public ICommand OpenCSCommand { get; set; }
		public ICommand OpenVBCommand { get; set; }
		public string OpenCSTitleNL { get; set; }
		public string OpenVBTitleNL { get; set; }
		public string CSSolutionPath { get; set; }
		public string VBSolutionPath { get; set; }
		public event EventHandler<DataLoadEventArgs<ReadOnlyCollection<DemoDescription>>> DemosLoad;
		public ModuleDescription AdvanceModule(ModuleDescription module, int delta) {
			int index = Modules.IndexOf(module);
			if(index < 0) return null;
			index = (index + Modules.Count + delta) % Modules.Count;
			return Modules[index];
		}
		public void LoadModules() {
			if(IsFeaturedDemo)
				return;
			Modules = dataLoader.LoadModules(this, (DevExpress.DemoData.Model.WpfDemo)DemoDataProduct.Demos.First());
		}
		public void LoadDemos(Action<Action> dispatcher) {
			lock(demosLoadedLock) {
				if(demosLoaded) return;
				demosLoaded = true;
			}
			var e = new DataLoadEventArgs<ReadOnlyCollection<DemoDescription>>();
			if(DemosLoad != null)
				DemosLoad(this, e);
			if(e.Data == null)
				e.Data = new List<DemoDescription>().AsReadOnly();
			dispatcher(() => Demos = e.Data);
		}
		public string PlatformLabel { get; set; }
		#region Equality
		public override int GetHashCode() {
			return Name.GetHashCode();
		}
		public static bool operator ==(ProductDescription d1, ProductDescription d2) {
			if(ReferenceEquals(d1, d2))
				return true;
			if(ReferenceEquals(d1, null) || ReferenceEquals(d2, null))
				return false;
			return d1.DemoDataProduct == d2.DemoDataProduct;
		}
		public override bool Equals(object obj) {
			return this == obj as ProductDescription;
		}
		public static bool operator !=(ProductDescription d1, ProductDescription d2) {
			return !(d1 == d2);
		}
		#endregion
	}
	enum ModuleModifier { None, New, Updated, Highlighted, HighlightedNew, HighlightedUpdated }
	public sealed class ModuleDescription : BindableBase {
		public Action<Assembly> UpdateModuleType { get; set; }
		public ProductDescription Product { get; set; }
		public string Tags { get; set; }
		public WpfModule WpfModule { get; set; }
		public string DemoAssemblyName { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public bool AllowRtl { get; set; }
		public string GroupName { get; set; }
		public string GroupTitle { get; set; }
		public string ShortDescription { get; set; }
		public string Description { get; set; }
		public bool IsNew { get; set; }
		public bool IsUpdated { get; set; }
		public bool IsHighlighted { get; set; }
		public bool AllowSwitchingTheme { get; set; }
		internal ModuleModifier Modifier { get; set; }
		public Color Color { get; set; }
		public Type ModuleType { get; set; }
		#region Equality
		public override int GetHashCode() {
			return Name.GetHashCode();
		}
		public static bool operator ==(ModuleDescription d1, ModuleDescription d2) {
			if(ReferenceEquals(d1, d2))
				return true;
			if(ReferenceEquals(d1, null) || ReferenceEquals(d2, null))
				return false;
			return d1.Name == d2.Name && d1.Product == d2.Product;
		}
		public override bool Equals(object obj) {
			return this == obj as ModuleDescription;
		}
		public static bool operator !=(ModuleDescription d1, ModuleDescription d2) {
			return !(d1 == d2);
		}
		#endregion
		public override string ToString() {
			return Name;
		}
	}
}
