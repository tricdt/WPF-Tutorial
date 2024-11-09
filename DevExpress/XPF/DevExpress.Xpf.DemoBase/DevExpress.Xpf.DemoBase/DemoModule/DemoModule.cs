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
using System.Windows.Media;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Bars;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoBase {
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Documents;
	using DevExpress.Mvvm;
	public partial class DemoModule : UserControl {
		static DemoModule() {
			DependencyPropertyRegistrator<DemoModule>.New()
				.RegisterReadOnly(nameof(IsPopupContentInvisible), out IsPopupContentInvisiblePropertyKey, out IsPopupContentInvisibleProperty, false, d => d.OnIsPopupContentInvisibleChanged())
				.RegisterReadOnly(nameof(Owner), out OwnerPropertyKey, out OwnerProperty, default(object))
				.RegisterReadOnly(nameof(Options), out OptionsPropertyKey, out OptionsProperty, default(FrameworkElement))
				.Register(nameof(OptionsDataContext), out OptionsDataContextProperty, default(object), d => d.UpdateOptionsDataContext())
				.Register(nameof(RibbonStyle), out RibbonStyleProperty, default(Style))
				.OverrideMetadata(BarNameScope.IsScopeOwnerProperty, true)
				.OverrideMetadata(BackgroundProperty, ViewModelBase.IsInDesignMode ? new SolidColorBrush(Colors.White) : null)
			;
		}
		public static readonly RoutedEvent ModuleLoadedEvent =
			EventManager.RegisterRoutedEvent("ModuleLoaded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DemoModule));
		public event RoutedEventHandler ModuleLoaded { add { this.AddHandler(ModuleLoadedEvent, value); } remove { this.RemoveHandler(ModuleLoadedEvent, value); } }
		public static readonly RoutedEvent ModuleUnloadedEvent =
			EventManager.RegisterRoutedEvent("ModuleUnloaded", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DemoModule));
		public event RoutedEventHandler ModuleUnloaded { add { this.AddHandler(ModuleUnloadedEvent, value); } remove { this.RemoveHandler(ModuleUnloadedEvent, value); } }
		void OnIsPopupContentInvisibleChanged() {
			if(!IsPopupContentInvisible) {
				ShowPopupContent();
				BarManagerHelper.ShowFloatingBars(this, true);
			} else {
				HidePopupContent();
				BarManagerHelper.HideFloatingBars(this, true);
			}
		}
		void UpdateOptionsDataContext() {
			Options.Do(x => {
				if(OptionsDataContext == null)
					x.ClearValue(FrameworkElement.DataContextProperty);
				else
					x.DataContext = OptionsDataContext;
			});
		}
		internal void Prepare() {
			var options = (FrameworkElement)FindName("PART_Options");
			if(options == null) return;
			var optionsPanel = (DockPanel)options.Parent;
			var optionsIndex = optionsPanel.Children.IndexOf(options);
			var decorator = new NonVisualDecorator();
			DockPanel.SetDock(decorator, Dock.Right);
			optionsPanel.Children.Insert(optionsIndex, decorator);
			options.SetBinding(TextElement.ForegroundProperty, new Binding("Foreground") { RelativeSource = new RelativeSource { AncestorType = typeof(Window) }  });
			DangerousHelper.MoveLogicalChild(options, decorator, () => optionsPanel.Children.RemoveAt(optionsIndex + 1), () => decorator.Child = options);
			Options = options;
			UpdateOptionsDataContext();
		}
		public void UpdateOwner(object owner) { Owner = owner; }
		internal void UpdatePopupContent(bool hide) { IsPopupContentInvisible = hide; }
		protected internal virtual void Hide() {
			(Content as UIElement ?? this).RaiseEvent(new RoutedEventArgs() { RoutedEvent = ModuleUnloadedEvent });
		}
		protected internal virtual void Show() {
			(Content as UIElement ?? this).RaiseEvent(new RoutedEventArgs() { RoutedEvent = ModuleLoadedEvent });
		}
		protected virtual void ShowPopupContent() { }
		protected virtual void HidePopupContent() { }
		protected internal virtual void Clear() { }
		protected override Size MeasureOverride(Size constraint) {
			constraint = base.MeasureOverride(constraint);
			ClearAutomationEventsHelper.ClearAutomationEvents();
			return constraint;
		}
		public virtual void ProcessArguments(string[] arguments) { }
		public virtual string GetArgument() { return string.Empty; }
		protected internal virtual bool ShowApplicationButton() { return false; }
	}
}
