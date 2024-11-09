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
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Native.PrismWrappers.Prism800;
namespace DevExpress.Xpf.Native.Prism800 {
	public class DXTabControlAdapterImpl {
		public DXTabControlAdapterImpl(int prismVersion) { }
		class Adapter : AdapterBase<DXTabControl> {
			bool updating = false;
			public Adapter(IRegionRuntimeWrapper region, DXTabControl target)
				: base(region, target) {
				target.SelectionChanged += Target_SelectionChanged;
				target.SelectionChanging += Target_SelectionChanging;
				target.TabRemoved += Target_TabRemoved;
			}
			void Target_TabRemoved(object sender, TabControlTabRemovedEventArgs e) {
				if(updating)
					return;
				Region.Remove(e.Item);
			}
			void Target_SelectionChanging(object sender, TabControlSelectionChangingEventArgs e) {
				if(updating)
					return;
				var vm = TryGetViewModel(e.OldSelectedItem);
				if(vm != null && IConfirmNavigationRequestRuntimeWrapper.IsCompatible(vm.GetType())) {
					var confirmNavigation = IConfirmNavigationRequestRuntimeWrapper.Wrap(vm);
					NavigationContextRuntimeWrapper context = null;
					context = new NavigationContextRuntimeWrapper(Region.NavigationService, null);
					confirmNavigation.ConfirmNavigationRequest(context, confirm => {
						e.Cancel = !confirm;
					});
				}
			}
			void Target_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e) {
				if (e.NewSelectedItem == null)
					return;
				Region.Activate(e.NewSelectedItem);
			}
			object TryGetViewModel(object item) {
				var fe = item as FrameworkElement;
				if(fe != null)
					return fe.DataContext;
				var fce = item as FrameworkContentElement;
				if(fce != null)
					return fce.DataContext;
				return item;
			}
			protected override void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
				if (e.Action == NotifyCollectionChangedAction.Add) {
					var selectedItemWasNull = Target.SelectedItem == null;
					foreach (var item in e.NewItems) {
						Target.Items.Add(item);
					}
					Debug.Assert(Target.SelectedItem != null);
					if (selectedItemWasNull) {
						Region.Activate(Target.SelectedItem);
					}
				} else if (e.Action == NotifyCollectionChangedAction.Remove) {
					foreach (var item in e.OldItems) {
						Target.Items.Remove(item);
					}
				}
			}
			protected override void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
				updating = true;
				try {
					if(e.Action == NotifyCollectionChangedAction.Add) {
						Target.SelectedItem = e.NewItems[0];
					} else if(e.Action == NotifyCollectionChangedAction.Remove) {
						bool removeSelected = false;
						foreach(var item in e.OldItems) {
							if(Target.SelectedItem == item) {
								removeSelected = true;
							}
						}
						if (removeSelected) {
							var toSelect = Target.Items
								.Cast<object>()
								.Zip(Enumerable.Range(0, Target.Items.Count), Tuple.Create)
								.Where(x => !e.OldItems.Contains(x.Item1))
								.OrderBy(x => x.Item2)
								.FirstOrDefault();
							if(toSelect != null) {
								Region.Activate(toSelect.Item1);
							}
						}
					}
				} finally {
					updating = false;
				}
			}
		}
		public void Adapt(IRegionRuntimeWrapper region, DXTabControl regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
}
