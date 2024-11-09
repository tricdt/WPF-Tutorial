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

using System.Windows.Controls;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase.Helpers.Internal;
using DevExpress.Xpf.Utils.Themes;
namespace DevExpress.Xpf.DemoBase.Helpers {
	using System.Windows;
	using DevExpress.Xpf.Core.Native;
	public partial class SidebarWindow : ThemedWindow {
		static SidebarWindow() {
			DependencyPropertyRegistrator<SidebarWindow>.New()
				.Register(nameof(Sidebar), out SidebarProperty, default(UIElement), (d, o, n) => d.OnSidebarChanged(o, n))
				.OverrideMetadata(ShowIconProperty, false)
			;
			CommonThemeHelper.TreeWalkerOverrideProperty<SidebarWindow>((x, newValue) => x.OnThemeChanged());
		}
		public SidebarWindow() {
			DpiAwareSizeCorrector.Attach(this);
		}
		void OnThemeChanged() {
			var themeName = CommonThemeHelper.GetTreeWalkerThemeName(this);
			switch(themeName) {
				case Theme.LightGrayName:
				case Theme.DeepBlueName:
				case Theme.Office2007BlueName:
				case Theme.Office2007BlackName:
				case Theme.Office2007SilverName:
				case Theme.SevenName:
				case Theme.VS2010Name:
				case Theme.DXStyleName:
				case Theme.Office2010BlackName:
				case Theme.Office2010BlueName:
				case Theme.Office2010SilverName:
				case Theme.HybridAppName:
				case Theme.BaseName:
				case Theme.TouchlineDarkName:
					var themeExtension = new ThemedWindowThemeKeyExtension() { ThemeName = themeName, ResourceKey = ThemedWindowThemeKeys.WindowNormalPadding };
					Padding = (Thickness)TryFindResource(themeExtension);
					break;
				default:
					Padding = new Thickness(0);
					break;
			}
		}
		void OnSidebarChanged(UIElement oldValue, UIElement newValue) {
			if(root == null) return;
			if(oldValue != null)
				root.Children.Remove(oldValue);
			if(newValue != null) {
				DockPanel.SetDock(newValue, Dock.Left);
				root.Children.Insert(0, newValue);
			}
		}
		DockPanel root;
		public override void OnApplyTemplate() {
			if(root != null && Sidebar != null)
				root.Children.Remove(Sidebar);
			base.OnApplyTemplate();
			root = (DockPanel)GetTemplateChild("PART_WindowHeaderContentAndStatusPanel");
			if(root == null) return;
			if(Sidebar != null) {
				DockPanel.SetDock(Sidebar, Dock.Left);
				root.Children.Insert(0, Sidebar);
			}
		}
	}
	public class WindowSidebar : NonVisualDecorator {
		public WindowSidebar() {
			Loaded += (s, e) => UpdateWindow();
			Unloaded += (s, e) => UpdateWindow();
		}
		SidebarWindow window;
		SidebarWindow Window {
			get { return window; }
			set {
				if(value == window) return;
				if(window != null)
					window.Sidebar = null;
				window = value;
				if(window != null)
					window.Sidebar = new NonLogicalDecorator() { Child = ActualChild };
			}
		}
		protected override void OnActualChildChanged(DependencyPropertyChangedEventArgs e) {
			base.OnActualChildChanged(e);
			if(Window != null)
				Window.Sidebar = new NonLogicalDecorator() { Child = (UIElement)e.NewValue };
		}
		void UpdateWindow() {
			Window = System.Windows.Window.GetWindow(this) as SidebarWindow;
		}
	}
}
