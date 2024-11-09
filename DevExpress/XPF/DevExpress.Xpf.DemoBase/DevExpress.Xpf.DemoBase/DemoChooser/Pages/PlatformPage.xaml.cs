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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoCenterBase;
namespace DevExpress.Xpf.DemoChooser.Internal {
	public partial class PlatformPage : UserControl {
		public PlatformPage() {
			InitializeComponent();
			Loaded += (_, __) => DemoWindow = this.VisualParents().OfType<DemoChooserWindow>().FirstOrDefault();
			Unloaded += (_, __) => DemoWindow = null;
		}
		protected override Size MeasureOverride(Size constraint) {
			constraint = base.MeasureOverride(constraint);
			ClearAutomationEventsHelper.ClearAutomationEvents();
			return constraint;
		}
		DemoChooserWindow demoWindow;
		Action unsubscribeFromCtrlF;
		DemoChooserWindow DemoWindow {
			get { return demoWindow; }
			set {
				if(demoWindow != null) {
					unsubscribeFromCtrlF();
					unsubscribeFromCtrlF = null;
					demoWindow.PreviewTextInput -= OnDemoWindowPreviewTextInput;
				}
				demoWindow = value;
				if(demoWindow != null) {
					demoWindow.PreviewTextInput += OnDemoWindowPreviewTextInput;
					unsubscribeFromCtrlF = SubscribeToCtrlF(demoWindow, (_, __) => filter.FocusEditor());
				}
			}
		}
		static Action SubscribeToCtrlF(Window window, ExecutedRoutedEventHandler handler) {
			var command = new RoutedCommand();
			command.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
			var commandBinding = new CommandBinding(command, handler);
			window.CommandBindings.Add(commandBinding);
			return () => window.CommandBindings.Remove(commandBinding);
		}
		void OnDemoWindowPreviewTextInput(object sender, TextCompositionEventArgs e) {
			if(!string.IsNullOrEmpty(e.Text))
				filter.FocusEditor();
		}
		void OnClearFilterFocus(object sender, EventArgs e) {
			if(DemoWindow != null) {
				try {
					FocusManager.SetFocusedElement(DemoWindow, DemoWindow);
				} catch(NullReferenceException) { }
			}
		}
	}
}
