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
using System.Windows;
using System.Windows.Threading;
using DevExpress.Mvvm.UI.Interactivity;
namespace DevExpress.Xpf.DemoBase {
	public class DispatchFocusBehavior : Behavior<DemoModule> {
		public FrameworkElement Element {
			get { return (FrameworkElement)GetValue(ElementProperty); }
			set { SetValue(ElementProperty, value); }
		}
		public static readonly DependencyProperty ElementProperty =
			DependencyProperty.Register("Element", typeof(FrameworkElement), typeof(DispatchFocusBehavior), new FrameworkPropertyMetadata(null, ElementChangedCallback));
		private static void ElementChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			((DispatchFocusBehavior)d).FocusElement((FrameworkElement)e.NewValue);
		}
		void FocusElement(FrameworkElement element) {
			if(element != null) {
				if(element.IsLoaded) {
					DispatchFocus(element);
				} else {
					element.Loaded += element_Loaded;
				}
			}
		}
		void element_Loaded(object sender, RoutedEventArgs e) {
			var senderElement = (FrameworkElement)sender;
			senderElement.Loaded -= element_Loaded;
			DispatchFocus(senderElement);
		}
		void DispatchFocus(FrameworkElement element) {
			element.Dispatcher.BeginInvoke(
				new System.Action(() => element.Focus()),
				DispatcherPriority.ApplicationIdle);
		}
		protected override void OnAttached() {
			base.OnAttached();
			AssociatedObject.ModuleLoaded += OnAssociatedObjectModuleAppear;
		}
		protected override void OnDetaching() {
			AssociatedObject.ModuleLoaded -= OnAssociatedObjectModuleAppear;
			base.OnDetaching();
		}
		void OnAssociatedObjectModuleAppear(object sender, RoutedEventArgs e) {
			FocusElement(Element);
		}
	}
}
