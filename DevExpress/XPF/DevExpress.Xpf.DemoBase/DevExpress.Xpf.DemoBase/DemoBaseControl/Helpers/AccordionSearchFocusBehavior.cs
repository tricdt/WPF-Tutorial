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

using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Accordion;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public class AccordionSearchFocusBehavior : Behavior<AccordionControl> {
		public AccordionSearchFocusBehavior() {
			this.NotifyValue(x => x.AssociatedObject)
				.Where(x => x != null)
				.SelectMany(accordion => Observable.Clock<MouseButtonEventArgs>(action => { MouseButtonEventHandler h = (_, e) => action(e); accordion.PreviewMouseLeftButtonDown += h; return () => accordion.PreviewMouseLeftButtonDown -= h; }))
				.Execute(e => {
					var searchControl = LayoutTreeHelper.GetVisualChildren(AssociatedObject).OfType<AccordionSearchControl>().First();
					if(searchControl.IsKeyboardFocusWithin) {
						Keyboard.Focus(AssociatedObject);
						return;
					}
					var position = e.GetPosition(AssociatedObject);
					if(position.Y > 80.0)
						return;
					if(LayoutTreeHelper.GetVisualParents(VisualTreeHelper.HitTest(AssociatedObject, position).VisualHit).Where(x => x is AccordionItem || x is ButtonBase || x is ScrollBar).Any())
						return;
					Keyboard.Focus(searchControl);
					e.Handled = true;
				})
			;
		}
	}
	public class AccordionResetSelectionBehavior : Behavior<AccordionControl> {
		public object ActiveDemoModule {
			get { return (object)GetValue(ActiveDemoModuleProperty); }
			set { SetValue(ActiveDemoModuleProperty, value); }
		}
		public static readonly DependencyProperty ActiveDemoModuleProperty =
			DependencyProperty.Register("ActiveDemoModule", typeof(object), typeof(AccordionResetSelectionBehavior), new PropertyMetadata(0));
		protected override void OnAttached() {
			base.OnAttached();
			AssociatedObject.LostKeyboardFocus += Accordion_LostKeyboardFocus;
		}
		protected override void OnDetaching() {
			base.OnDetaching();
			AssociatedObject.LostKeyboardFocus -= Accordion_LostKeyboardFocus;
		}
		void Accordion_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) {
			if(e.NewFocus is AccordionItem)
				return;
			AssociatedObject.SelectedItem = ActiveDemoModule;
		}
	}
}
