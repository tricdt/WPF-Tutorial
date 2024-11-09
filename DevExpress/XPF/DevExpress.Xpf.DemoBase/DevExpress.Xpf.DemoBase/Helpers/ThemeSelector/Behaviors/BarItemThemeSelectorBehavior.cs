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
using System.Reflection;
using System.Windows;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Utils.Themes;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public abstract class DemoBaseIndependentThemeKeyExtensionBase<T> : IndependentThemeKeyExtensionBase<T> {
		protected override Assembly GetAssembly() {
			return typeof(DemoBaseIndependentThemeKeyExtensionBase<>).Assembly;
		}
	}
	public enum GalleryDemoThemeSelectorThemeKeys {
		Style,
		GroupStyle,
		ItemStyle,
		GroupTemplate,
		ItemTemplate,
		InSplitButtonGalleryStyle,
		InSplitButtonGroupStyle,
		InSplitButtonItemStyle,
		InRibbonGalleryStyle,
		InRibbonGalleryGroupStyle,
		InRibbonGalleryItemStyle,
		InRibbonDropDownGalleryStyle,
		InRibbonDropDownGalleryGroupStyle,
		InRibbonDropDownGalleryItemStyle,
	}
	public class GalleryDemoThemeSelectorThemeKeyExtension : DemoBaseIndependentThemeKeyExtensionBase<GalleryDemoThemeSelectorThemeKeys> { }
	[Browsable(false)]
	public abstract class BarItemDemoThemeSelectorBehavior<T> : DemoThemeSelectorBehavior<T> where T : BarItem {
		protected override DependencyProperty StyleProperty { get { return BarItem.StyleProperty; } }
		protected override void Initialize(T item) {
			item.DataContext = ThemesCollection;
			UpdateAssociatedObjectResourceReference();
		}
		protected override void Clear(T item) {
			base.Clear(item);
			item.DataContext = null;
		}
		protected override void OnThemesCollectionChanged(ICollectionView oldValue) {
			if(AssociatedObject != null)
				AssociatedObject.DataContext = ThemesCollection;
		}
		protected void UpdateAssociatedObjectResourceReference() {
			if(AssociatedObject != null) {
				AssociatedObject.SetResourceReference(StyleProperty, StyleKey);
			}
		}
	}
	[Browsable(false)]
	public abstract class GalleryBarItemDemoThemeSelectorBehavior<T> : BarItemDemoThemeSelectorBehavior<T> where T : BarItem {
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty GalleryProperty;
		static GalleryBarItemDemoThemeSelectorBehavior() {
			GalleryProperty = DependencyProperty.Register("Gallery", typeof(Gallery), typeof(GalleryBarItemDemoThemeSelectorBehavior<T>), new PropertyMetadata(null, OnGalleryPropertyChanged));
		}
		static void OnGalleryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((GalleryBarItemDemoThemeSelectorBehavior<T>)d).OnGalleryChanged(e.OldValue as Gallery);
		}
		public Gallery Gallery {
			get { return (Gallery)GetValue(GalleryProperty); }
			set { SetValue(GalleryProperty, value); }
		}
		protected GalleryDemoThemeSelectorBehavior GalleryBehavior {
			get {
				if(galleryBehavior == null)
					galleryBehavior = new GalleryDemoThemeSelectorBehavior();
				return galleryBehavior;
			}
		}
		public object GalleryStyleKey {
			get { return galleryStyleKey; }
			set {
				if(galleryStyleKey != value) {
					galleryStyleKey = value;
					OnGalleryStyleKeyChanged();
				}
			}
		}
		public GalleryBarItemDemoThemeSelectorBehavior() {
			GalleryStyleKey = CreateGalleryStyleKey();
		}
		protected override void Clear(T item) {
			base.Clear(item);
			Gallery = null;
		}
		protected override void OnThemesCollectionChanged(ICollectionView oldValue) {
			base.OnThemesCollectionChanged(oldValue);
			GalleryBehavior.ThemesCollection = ThemesCollection;
		}
		protected virtual object CreateGalleryStyleKey() {
			return new GalleryDemoThemeSelectorThemeKeyExtension() { ResourceKey = GalleryDemoThemeSelectorThemeKeys.Style, IsThemeIndependent = true };
		}
		protected virtual void InitializeGallery(Gallery gallery, GalleryDemoThemeSelectorBehavior behavior) {
			Interaction.GetBehaviors(gallery).Add(behavior);
		}
		protected virtual void UninitializeGallery(Gallery gallery, GalleryDemoThemeSelectorBehavior behavior) {
			Interaction.GetBehaviors(gallery).Remove(behavior);
		}
		protected virtual void OnGalleryChanged(Gallery oldValue) {
			if(oldValue != null)
				UninitializeGallery(oldValue, GalleryBehavior);
			if(Gallery != null)
				InitializeGallery(Gallery, GalleryBehavior);
		}
		protected virtual void OnGalleryStyleKeyChanged() {
			GalleryBehavior.StyleKey = GalleryStyleKey;
		}
		GalleryDemoThemeSelectorBehavior galleryBehavior;
		object galleryStyleKey;
	}
	public class DemoThemeViewModel : BindableBase {
		Action<Theme> selectTheme;
		Action<bool> setShowAllCategories;
		public DemoThemeViewModel(Theme theme) {
			GuardHelper.ArgumentNotNull(theme, "theme");
			Theme = theme;
		}
		internal bool Init(Action<Theme> selectTheme, Action<bool> setShowAllCategories) {
			if(this.selectTheme != null) return false;
			GuardHelper.ArgumentNotNull(selectTheme, "selectTheme");
			this.selectTheme = selectTheme;
			this.setShowAllCategories = setShowAllCategories;
			return true;
		}
		internal void SetIsSelected(bool isSelected) {
			isSelectedUpdateLocker.DoLockedAction(() => IsSelected = isSelected);
		}
		public Theme Theme { get; private set; }
		readonly Locker isSelectedUpdateLocker = new Locker();
		public bool IsSelected {
			get { return GetProperty(() => IsSelected); }
			set { SetProperty(() => IsSelected, value, OnIsSelectedChanged); }
		}
		public string DisplayName { get { return ThemeProperties.DisplayName(Theme); } }
		public string Category { get { return ThemeProperties.Category(Theme.Name); } }
		public object Owner {
			get { return GetProperty(() => Owner); }
			set { SetProperty(() => Owner, value); }
		}
		public bool ShowAllCategories {
			get { return GetProperty(() => ShowAllCategories); }
			set { SetProperty(() => ShowAllCategories, value, () => setShowAllCategories.Do(x => x(ShowAllCategories))); }
		}
		public bool UseSvgGlyphs {
			get { return useSvgGlyphs; }
			set { SetProperty(ref useSvgGlyphs, value, () => UseSvgGlyphs); }
		}
		void OnIsSelectedChanged() {
			isSelectedUpdateLocker.DoIfNotLocked(() => {
				if(IsSelected)
					selectTheme(Theme);
			});
		}
		bool useSvgGlyphs;
	}
}
