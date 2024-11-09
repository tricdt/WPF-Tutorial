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
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Bars;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public class BarSplitButtonItemBehavior : Behavior<BarSplitButtonItem> {
		public BarSplitButtonItemBehavior() {
			var itemsVar = this
				.NotifyValue(x => x.AssociatedObject)
				.Where(x => x != null)
				.SelectMany(a => a.DependencyValue(x => x.PopupControl))
				.Select(p => (p as PopupMenu).With(x => x.Items))
				.Where(x => x != null);
			var firstItemVar = itemsVar.SelectMany(items => items.OnChanged().Select(_ => (BarItem)items.FirstOrDefault()).Memoize()).Where(x => x != null);
			var selectedItemVar = itemsVar.SelectMany(items =>
				Observable.Trigger(items, (IBarItem item) => Observable.Clock(x => {
					ItemClickEventHandler h = (_, __) => x();
					((BarItem)item).ItemClick += h;
					return () => ((BarItem)item).ItemClick -= h;
				}).Select(() => (BarItem)item))
			);
			var selectedOrFirstItemVar = firstItemVar.SelectMany(x => selectedItemVar.InitialValue(x));
			Action<DependencyProperty> updateOnSelectedItemChanged = property => selectedOrFirstItemVar
				.SelectMany(d => d.DependencyValue(property))
				.Where(x => x != null)
				.Execute(v => AssociatedObject.Do(x => x.SetValue(property, v)));
			updateOnSelectedItemChanged(BarItem.GlyphProperty);
			updateOnSelectedItemChanged(BarItem.LargeGlyphProperty);
			BarItem selectedItem = null;
			selectedOrFirstItemVar.Execute(x => selectedItem = x);
			this
				.NotifyValue(x => x.AssociatedObject)
				.Where(x => x != null)
				.SelectMany(a => Observable.Clock(x => {
					ItemClickEventHandler h = (_, __) => x();
					a.ItemClick += h;
					return () => a.ItemClick -= h;
				}))
				.Execute(() => selectedItem.Do(x => x.PerformClick()));
		}
	}
}
