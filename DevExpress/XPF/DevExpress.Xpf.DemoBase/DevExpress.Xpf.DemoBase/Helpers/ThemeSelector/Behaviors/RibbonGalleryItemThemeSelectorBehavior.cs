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
using System.Reflection;
using System.Windows.Data;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Utils.Themes;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public class BarSplitButtonItemDemoThemeSelectorThemeKeyExtension : DemoBaseIndependentThemeKeyExtensionBase<BarSplitButtonItemDemoThemeSelectorThemeKeys> { }
	public enum BarSplitButtonItemDemoThemeSelectorThemeKeys {
		Style
	}
	public enum RibbonGalleryItemDemoThemeSelectorThemeKeys {
		Style
	}
	public class RibbonGalleryItemDemoThemeSelectorThemeKeyExtension : DemoBaseIndependentThemeKeyExtensionBase<RibbonGalleryItemDemoThemeSelectorThemeKeys> { }
	public class RibbonGalleryItemDemoThemeSelectorBehavior : GalleryBarItemDemoThemeSelectorBehavior<RibbonGalleryBarItem> {
		protected GalleryDemoThemeSelectorBehavior DropDownGalleryBehavior {
			get {
				if(dropDownGalleryBehavior == null)
					dropDownGalleryBehavior = new GalleryDemoThemeSelectorBehavior();
				return dropDownGalleryBehavior;
			}
		}
		protected Gallery DropDownGallery {
			get { return dropDownGallery; }
			set {
				if(dropDownGallery != value) {
					var oldValue = dropDownGallery;
					dropDownGallery = value;
					OnDropDownGalleryChanged(oldValue);
				}
			}
		}
		public object DropDownGalleryStyleKey {
			get { return dropDownGalleryStyleKey; }
			set {
				if(dropDownGalleryStyleKey != value) {
					dropDownGalleryStyleKey = value;
					OnDropDownGalleryStyleKeyChanged();
				}
			}
		}
		public RibbonGalleryItemDemoThemeSelectorBehavior() {
			DropDownGalleryStyleKey = CreateDropDownGalleryStyleKey();
		}
		bool isGalleryAssigned = false;
		protected override void Initialize(RibbonGalleryBarItem item) {
			base.Initialize(item);
			Gallery = item.Gallery ?? (item.Gallery = new Gallery());
			Binding binding = new Binding("Gallery");
			binding.Source = item;
			BindingOperations.SetBinding(this, RibbonGalleryItemDemoThemeSelectorBehavior.GalleryProperty, binding);
			item.DropDownGalleryInit += OnDropDownGalleryInit;
			item.DropDownGalleryClosed += OnDropDownGalleryClosed;
			if(item.DropDownGallery == null) {
				item.DropDownGallery = new Gallery();
				isGalleryAssigned = true;
			}
		}
		protected override void Clear(RibbonGalleryBarItem item) {
			base.Clear(item);
			item.DropDownGalleryInit -= OnDropDownGalleryInit;
			item.DropDownGalleryClosed -= OnDropDownGalleryClosed;
			DropDownGallery = null;
			Gallery = null;
			if(isGalleryAssigned) {
				item.ClearValue(RibbonGalleryBarItem.DropDownGalleryProperty);
				isGalleryAssigned = false;
			}
		}
		protected override void OnThemesCollectionChanged(System.ComponentModel.ICollectionView oldValue) {
			base.OnThemesCollectionChanged(oldValue);
			DropDownGalleryBehavior.ThemesCollection = ThemesCollection;
		}
		void OnDropDownGalleryInit(object sender, DropDownGalleryEventArgs e) {
			DropDownGallery = e.DropDownGallery.Gallery;
		}
		void OnDropDownGalleryClosed(object sender, DropDownGalleryEventArgs e) {
			DropDownGallery = null;
		}
		protected virtual void OnDropDownGalleryChanged(Gallery oldValue) {
			if(oldValue != null)
				UninitializeGallery(oldValue, DropDownGalleryBehavior);
			if(DropDownGallery != null)
				InitializeGallery(DropDownGallery, DropDownGalleryBehavior);
		}
		protected virtual void OnDropDownGalleryStyleKeyChanged() {
			DropDownGalleryBehavior.StyleKey = DropDownGalleryStyleKey;
		}
		protected override object CreateStyleKey() {
			return new RibbonGalleryItemDemoThemeSelectorThemeKeyExtension() { ResourceKey = RibbonGalleryItemDemoThemeSelectorThemeKeys.Style, IsThemeIndependent = true };
		}
		protected override object CreateGalleryStyleKey() {
			return new GalleryDemoThemeSelectorThemeKeyExtension() { ResourceKey = GalleryDemoThemeSelectorThemeKeys.InRibbonGalleryStyle, IsThemeIndependent = true };
		}
		protected virtual object CreateDropDownGalleryStyleKey() {
			return new GalleryDemoThemeSelectorThemeKeyExtension() { ResourceKey = GalleryDemoThemeSelectorThemeKeys.InRibbonDropDownGalleryStyle, IsThemeIndependent = true };
		}
		Gallery dropDownGallery;
		GalleryDemoThemeSelectorBehavior dropDownGalleryBehavior;
		object dropDownGalleryStyleKey;
	}
}
