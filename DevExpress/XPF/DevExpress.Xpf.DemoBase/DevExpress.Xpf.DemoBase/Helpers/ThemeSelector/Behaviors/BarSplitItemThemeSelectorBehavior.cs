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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Ribbon;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public class BarSplitItemDemoThemeSelectorBehavior : GalleryBarItemDemoThemeSelectorBehavior<BarSplitButtonItem> {
		protected override void Clear(BarSplitButtonItem item) {
			var popupContainer = item.PopupControl as PopupControlContainer;
			item.PopupControl = null;
			Gallery = null;
		}
		public UIElement Footer { get; set; }
		protected override void OnAttached() {
			base.OnAttached();
			if (AssociatedObject != null) {
				Binding binding = new Binding("SelectedTheme.Theme.SvgGlyph");
				binding.Source = this;
				binding.Converter = new SvgUriToImageSourceConverter();
				BindingOperations.SetBinding(AssociatedObject, BarSplitButtonItem.LargeGlyphProperty, binding);
				BindingOperations.SetBinding(AssociatedObject, BarSplitButtonItem.MediumGlyphProperty, binding);
				binding = new Binding("SelectedTheme.Theme.FullName");
				binding.Source = this;
				BindingOperations.SetBinding(AssociatedObject, BarSplitButtonItem.ContentProperty, binding);
			}
		}
		protected override void Initialize(BarSplitButtonItem item) {
			base.Initialize(item);
			PopupControlContainer containerControl = item.PopupControl as PopupControlContainer ?? new PopupControlContainer();
			item.PopupControl = containerControl;
			var container = containerControl.Content as DockPanel ?? new DockPanel() { LastChildFill = true };
			var galleryControl = LayoutHelper.FindElementByType<GalleryControl>(container) as GalleryControl ?? new GalleryControl() { Background = Brushes.Transparent };
			if(galleryControl.Gallery == null)
				galleryControl.Gallery = new Gallery() { AllowFilter = false };
			container.Children.Clear();
			if (Footer != null) {
				DockPanel.SetDock(Footer, Dock.Bottom);
				container.Children.Add(Footer);
			}				
			container.Children.Add(galleryControl);
			containerControl.Content = container;
			Binding binding = new Binding("Gallery");
			binding.Source = galleryControl;
			BindingOperations.SetBinding(this, BarSplitItemDemoThemeSelectorBehavior.GalleryProperty, binding);
		}
		protected override object CreateStyleKey() {
			return new BarSplitButtonItemDemoThemeSelectorThemeKeyExtension() { ResourceKey = BarSplitButtonItemDemoThemeSelectorThemeKeys.Style, IsThemeIndependent = true };
		}
		protected override object CreateGalleryStyleKey() {
			return new GalleryDemoThemeSelectorThemeKeyExtension() { ResourceKey = GalleryDemoThemeSelectorThemeKeys.InRibbonDropDownGalleryStyle, IsThemeIndependent = true };
		}
	}
}
