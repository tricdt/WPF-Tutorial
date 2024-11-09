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
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Bars.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.Core.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	[Browsable(false)]
	public abstract class DemoThemeSelectorBehavior<T> : Behavior<T>, IThemeSelectorBehavior where T : DependencyObject {
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty ThemesCollectionProperty;
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty ShowTouchThemesProperty;
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty ShowAllCategoriesProperty;
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty UseSvgGlyphsProperty;
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty SelectedThemeProperty;
		public ICollectionView ThemesCollection {
			get { return (ICollectionView)GetValue(ThemesCollectionProperty); }
			set { SetValue(ThemesCollectionProperty, value); }
		}
		public bool ShowTouchThemes {
			get { return (bool)GetValue(ShowTouchThemesProperty); }
			set { SetValue(ShowTouchThemesProperty, value); }
		}
		protected virtual void OnShowTouchThemesChanged() {
			UpdateThemesCollection();
		}
		public bool ShowAllCategories {
			get { return (bool)GetValue(ShowAllCategoriesProperty); }
			set { SetValue(ShowAllCategoriesProperty, value); }
		}
		public DemoThemeViewModel SelectedTheme {
			get { return (DemoThemeViewModel)GetValue(SelectedThemeProperty); }
			set { SetValue(SelectedThemeProperty, value); }
		}
		protected override bool AllowAttachInDesignMode { get { return true; } }
		public object StyleKey { get; set; }
		protected virtual DependencyProperty StyleProperty { get { return FrameworkElement.StyleProperty; } }
		static readonly Action<string, bool> defaultThemeSelector = (t, _) => {
			ApplicationThemeHelper.ApplicationThemeName = t;
		};
		Action<string, bool> selectTheme = defaultThemeSelector;
		Action<string, bool> IThemeSelectorBehavior.SelectTheme { get { return selectTheme; } set { selectTheme = value; } }
		static DemoThemeSelectorBehavior() {
			DependencyPropertyRegistrator<DemoThemeSelectorBehavior<T>>.New()
				.Register(nameof(ShowAllCategories), out ShowAllCategoriesProperty, true, (s, e) => s.UpdateThemeViewModels())
				.Register(nameof(ShowTouchThemes), out ShowTouchThemesProperty, true, (s, e) => s.OnShowTouchThemesChanged())
				.Register(nameof(ThemesCollection), out ThemesCollectionProperty, (ICollectionView)null, (s, e) => s.RaiseThemesCollectionChanged((ICollectionView)e.OldValue))
				.Register(nameof(UseSvgGlyphs), out UseSvgGlyphsProperty, true, s => s.OnUseSvgIconsChanged(), (s, v) => s.CoerceUseSvgIcons(v))
				.Register(nameof(SelectedTheme), out SelectedThemeProperty, (DemoThemeViewModel)null)
				;
		}
		public bool UseSvgGlyphs {
			get { return (bool)GetValue(UseSvgGlyphsProperty); }
			set { SetValue(UseSvgGlyphsProperty, value); }
		}
		public DemoThemeSelectorBehavior() {
			UpdateThemesCollection();
			StyleKey = CreateStyleKey();
			CommonThemeHelper.ApplicationThemeChangedWeakEvent += (s, e) => UpdateThemeViewModels();
		}
		void UpdateThemeViewModels() {
			if(foreignThemesCollection) return;
			var currentThemeName = ShowTouchThemes ? ApplicationThemeHelper.ApplicationThemeName : ThemeProperties.GetFromAlias(ApplicationThemeHelper.ApplicationThemeName, false);
			var applicationTheme = Theme.FindTheme(ApplicationThemeHelper.ApplicationThemeName);
			foreach(var themeViewModel in GetThemes()) {
				themeViewModel.UseSvgGlyphs = UseSvgGlyphs;
				themeViewModel.ShowAllCategories = ShowAllCategories;
				themeViewModel.SetIsSelected(IsThemeSelected(currentThemeName, themeViewModel, applicationTheme));
			}
			SelectedTheme = GetThemes().FirstOrDefault(x => x.IsSelected);
		}
		bool IsThemeSelected(string currentThemeName, DemoThemeViewModel viewModel, Theme applicationTheme) {
			return viewModel.Theme.Name == currentThemeName
				|| IsWin10SystemTheme(viewModel.Theme, applicationTheme)
				|| IsPaletteTheme(viewModel, applicationTheme);
		}
		static bool IsWin10SystemTheme(Theme viewModelTheme, Theme appTheme) => ThemePaletteHelper.IsWin10SystemTheme(appTheme) && ThemePaletteHelper.IsWin10SystemTheme(viewModelTheme);
		static bool IsPaletteTheme(DemoThemeViewModel viewModel, Theme appTheme) {
			if(viewModel == null || appTheme == null)
				return false;
			return !ThemePaletteHelper.IsWin10SystemTheme(appTheme) && viewModel.Theme == ThemePaletteHelper.GetBaseTheme(appTheme);
		} 
		IEnumerable<DemoThemeViewModel> GetThemes() {
			return ThemesCollection.SourceCollection.With(x => x.OfType<DemoThemeViewModel>()) ?? EmptyArray<DemoThemeViewModel>.Instance;
		}
		void UpdateThemesCollection() {
			ThemesCollection = CreateCollectionView();
		}
		protected abstract object CreateStyleKey();
		protected override void OnAttached() {
			base.OnAttached();
			if(AssociatedObject != null)
				Initialize(AssociatedObject);
		}
		protected override void OnDetaching() {
			base.OnDetaching();
			if(AssociatedObject != null)
				Clear(AssociatedObject);
		}
		protected virtual void Clear(T item) {
			item.ClearValue(StyleProperty);
		}
		protected abstract void Initialize(T item);
		bool foreignThemesCollection;
		void RaiseThemesCollectionChanged(ICollectionView oldValue) {
			var selfRef = new WeakReference(this);
			Action<Theme> selectTheme = x => {
				var self = (DemoThemeSelectorBehavior<T>)selfRef.Target;
				if(self == null) return;
				var themeName = !ThemePaletteHelper.IsWin10SystemTheme(x) ? x.Name : ThemePaletteHelper.GetWin10ActualThemeName(x);
				self.selectTheme(themeName, self.ShowTouchThemes);
			};
			Action<bool> setShowAllCategories = x => {
				var self = (DemoThemeSelectorBehavior<T>)selfRef.Target;
				if(self == null) return;
				self.ShowAllCategories = x;
			};
			foreignThemesCollection = GetThemes().Select(x => x.Init(selectTheme, setShowAllCategories)).Any(x => !x);
			UpdateThemeViewModels();
			OnThemesCollectionChanged(oldValue);
		}
		protected abstract void OnThemesCollectionChanged(ICollectionView oldValue);
		protected virtual ICollectionView CreateCollectionView() {
			ICollectionView view = CollectionViewSource.GetDefaultView(ThemeProperties.GetThemesCollection(ShowTouchThemes, useLegacyCategories: false)
				.Select(x => new DemoThemeViewModel(x)).ToArray());
			view.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
			return view;
		}
		protected virtual void OnUseSvgIconsChanged() {
			UpdateThemeViewModels();
		}
		bool CoerceUseSvgIcons(bool useSvgIcons) {
			return useSvgIcons && ApplicationThemeHelper.UseDefaultSvgImages;
		}
	}
}
