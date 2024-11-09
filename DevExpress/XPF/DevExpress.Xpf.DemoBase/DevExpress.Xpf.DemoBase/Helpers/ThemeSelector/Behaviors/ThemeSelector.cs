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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Data.Utils;
using DevExpress.DemoData.Helpers;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoLauncher;
namespace DevExpress.Xpf.DemoBase.Helpers {
	interface IThemeSelectorBehavior {
		Action<string, bool> SelectTheme { get; set; }
	}
	public class ThemeSelector : Behavior<DependencyObject> {
		Action<string, bool> defaultSelector;
		protected override void OnAttached() {
			base.OnAttached();
			var selectorBehavior = AssociatedObject as IThemeSelectorBehavior;
			if(selectorBehavior != null) {
				defaultSelector = selectorBehavior.SelectTheme;
				selectorBehavior.SelectTheme = SelectTheme;
			}
		}
		protected override void OnDetaching() {
			base.OnDetaching();
			var selectorBehavior = AssociatedObject as IThemeSelectorBehavior;
			if(selectorBehavior != null) {
				selectorBehavior.SelectTheme = defaultSelector;
				defaultSelector = null;
			}
		}
		readonly Locker selectedThemeLocker = new Locker();
		public static readonly DependencyProperty SelectedThemeProperty =
			DependencyProperty.Register("SelectedTheme", typeof(string), typeof(ThemeSelector), new FrameworkPropertyMetadata(null,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				(d, e) => ((ThemeSelector)d).OnSelectedThemeChanged()));
		public string SelectedTheme {
			get { return (string)GetValue(SelectedThemeProperty); }
			set { SetValue(SelectedThemeProperty, value); }
		}
		void OnSelectedThemeChanged() {
			selectedThemeLocker.DoIfNotLocked(() => SelectTheme(SelectedTheme, false));
		}
		readonly Locker allowTouchUIPropertyLocker = new Locker();
		public static readonly DependencyProperty AllowTouchUIProperty =
			DependencyProperty.Register("AllowTouchUI", typeof(bool), typeof(ThemeSelector), new PropertyMetadata(false,
				(d, e) => ((ThemeSelector)d).OnAllowTouchUIChanged((bool)e.OldValue)));
		public bool AllowTouchUI {
			get { return (bool)GetValue(AllowTouchUIProperty); }
			[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
			set { SetValue(AllowTouchUIProperty, value); }
		}
		void OnAllowTouchUIChanged(bool oldValue) {
			allowTouchUIPropertyLocker.DoLockedActionIfNotLocked(() => SetCurrentValue(AllowTouchUIProperty, oldValue));
		}
		public static readonly DependencyProperty TouchUIProperty =
			DependencyProperty.Register("TouchUI", typeof(bool), typeof(ThemeSelector), new FrameworkPropertyMetadata(false,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				(d, e) => ((ThemeSelector)d).OnTouchUIChanged()));
		public bool TouchUI {
			get { return (bool)GetValue(TouchUIProperty); }
			set { SetValue(TouchUIProperty, value); }
		}
		readonly Locker touchUIUpdateLocker = new Locker();
		void OnTouchUIChanged() {
			touchUIUpdateLocker.DoIfNotLocked(UpdateApplicationTheme);
		}
		void SetTouchUI(bool t) {
			if(!AllowTouchUI) return;
			touchUIUpdateLocker.DoLockedAction(() => SetCurrentValue(TouchUIProperty, t));
		}
		void SetSelectedTheme(string theme) {
			selectedThemeLocker.DoLockedAction(() => SelectedTheme = theme);
			if(string.IsNullOrEmpty(theme)) return;
			var touchThemeName = ThemeProperties.Combine(theme, true);
			allowTouchUIPropertyLocker.DoLockedAction(() => SetCurrentValue(AllowTouchUIProperty, Theme.Themes.Any(x => x.Name == touchThemeName)));
		}
		void UpdateApplicationTheme() {
			if(string.IsNullOrEmpty(SelectedTheme))
				return;
			SetApplicationThemeName(ThemeProperties.Combine(SelectedTheme, AllowTouchUI && TouchUI));
			ClosePopup(AssociatedObject);
		}
		void ClosePopup(DependencyObject associatedObject) {
			var visualElement = (associatedObject as Behavior)?.AssociatedObject as DependencyObject;
			var buttonItem = LayoutHelper.FindAmongParents<BarSplitButtonItem>(visualElement, null);
			buttonItem?.PopupControl.ClosePopup();
		}
		public static void SetApplicationThemeName(string themeName) {
			ApplicationThemeHelper.ApplicationThemeName = themeName;
		}
		void UpdateSelectedTheme(string themeName, bool forceSetTouchUI) {
			SetSelectedTheme(ThemeProperties.GetFromAlias(themeName, false));
			if(forceSetTouchUI)
				SetTouchUI(ThemeProperties.IsTouch(themeName) || ThemeProperties.IsTouchlineTheme(themeName));
		}
		public void SelectTheme(string themeName, bool forceSetTouchUI) {
			UpdateSelectedTheme(themeName, forceSetTouchUI: forceSetTouchUI);
			UpdateApplicationTheme();
		}
		public ThemeSelector() {
			CommonThemeHelper.ApplicationThemeChangedWeakEvent += (s, e) => UpdateSelectedTheme(CommonThemeHelper.ActualApplicationThemeName, forceSetTouchUI: true);
			UpdateSelectedTheme(ApplicationThemeHelper.ApplicationThemeName, forceSetTouchUI: true);
		}
	}
}
