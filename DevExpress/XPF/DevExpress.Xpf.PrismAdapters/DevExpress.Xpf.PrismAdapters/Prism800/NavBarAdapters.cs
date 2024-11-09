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
using DevExpress.Xpf.Native.PrismWrappers.Prism800;
using DevExpress.Xpf.NavBar;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections;
using DevExpress.Xpf.Native.Prism;
namespace DevExpress.Xpf.Native.Prism800 {
	abstract class NavBarAdapterBase<TControl, TItem> : AdapterBase<TControl>
		where TItem : class
		where TControl : DependencyObject {
		bool updating = false;
		public NavBarAdapterBase(IRegionRuntimeWrapper region, TControl target) : base(region, target) {
			((INotifyCollectionChanged)GetItems(target)).CollectionChanged += Items_CollectionChanged;
			BindingOperations.SetBinding(target, GetSelectedItemProperty(),
				new Binding("SelectedItem") { Source = this, Mode = BindingMode.OneWayToSource });
		}
		void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if(updating)
				return;
			if(e.Action == NotifyCollectionChangedAction.Add) {
				foreach(var item in e.NewItems) {
					Region.Add(item);
				}
			} else if(e.Action == NotifyCollectionChangedAction.Remove) {
			}
		}
		TItem selectedItem;
		public TItem SelectedItem {
			get { return selectedItem; }
			set {
				if(selectedItem != value) {
					selectedItem = value;
					if(value != null && !updating) {
						Region.Activate(FindRegionItem(value));
					}
				}
			}
		}
		protected override void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			updating = true;
			if(e.Action == NotifyCollectionChangedAction.Add) {
				foreach(var obj in e.NewItems) {
					GetItems(Target).Add(CreateItem(obj));
				}
				if(TargetSelectedItem == null) {
					Region.Activate(e.NewItems[0]);
				}
			} else if(e.Action == NotifyCollectionChangedAction.Remove) {
				foreach(var obj in e.OldItems) {
					GetItems(Target).Remove(FindItem(obj));
				}
			}
			updating = false;
		}
		protected override void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			updating = true;
			if(e.Action == NotifyCollectionChangedAction.Add) {
				TargetSelectedItem = FindItem(e.NewItems[0]);
			}
			updating = false;
		}
		TItem FindItem(object obj) {
			return GetItems(Target).OfType<TItem>().FirstOrDefault(x =>
				ReferenceEquals(obj, x) ||
				ReferenceEquals(obj, GetContent(x)) ||
				ReferenceEquals(obj, GetDataContext(x)
			));
		}
		object FindRegionItem(TItem item) {
			return ((IEnumerable<object>)Region.Views.Object).FirstOrDefault(x =>
				ReferenceEquals(x, item) ||
				ReferenceEquals(x, GetContent(item)) ||
				ReferenceEquals(x, GetDataContext(item)
			));
		}
		protected abstract object GetDataContext(TItem item);
		protected abstract object GetContent(TItem item);
		protected abstract IList GetItems(TControl control);
		protected abstract object TargetSelectedItem { get; set; }
		protected abstract TItem CreateItem(object obj);
		protected abstract DependencyProperty GetSelectedItemProperty();
	}
	public class NavBarGroupAdapterImpl {
		class Adapter : NavBarAdapterBase<NavBarGroup, NavBarItem> {
			public Adapter(IRegionRuntimeWrapper region, NavBarGroup target) : base(region, target) { }
			protected override object TargetSelectedItem {
				get { return Target.SelectedItem; }
				set { Target.SelectedItem = (NavBarItem)value; }
			}
			protected override NavBarItem CreateItem(object obj) {
				if(obj is NavBarItem)
					return (NavBarItem)obj;
				if(obj is UIElement)
					return new NavBarItem { Content = obj };
				return new NavBarItem { DataContext = obj };
			}
			protected override object GetContent(NavBarItem item) {
				return item.Content;
			}
			protected override object GetDataContext(NavBarItem item) {
				return item.DataContext;
			}
			protected override IList GetItems(NavBarGroup control) {
				return control.Items;
			}
			protected override DependencyProperty GetSelectedItemProperty() {
				return NavBarGroup.SelectedItemProperty;
			}
		}
		public NavBarGroupAdapterImpl(int prismVersion) {
		}
		public void Adapt(IRegionRuntimeWrapper region, NavBarGroup regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
	public class NavBarControlAdapterImpl {
		class Adapter : NavBarAdapterBase<NavBarControl, NavBarGroup> {
			public Adapter(IRegionRuntimeWrapper region, NavBarControl target) : base(region, target) { }
			protected override object TargetSelectedItem {
				get { return Target.SelectedGroup; }
				set { Target.SelectedGroup = value; }
			}
			protected override NavBarGroup CreateItem(object obj) {
				if(obj is NavBarGroup)
					return (NavBarGroup)obj;
				if(obj is UIElement)
					return new NavBarGroup { Content = obj };
				return new NavBarGroup { DataContext = obj };
			}
			protected override object GetContent(NavBarGroup item) {
				return item.Content;
			}
			protected override object GetDataContext(NavBarGroup item) {
				return item.DataContext;
			}
			protected override IList GetItems(NavBarControl control) {
				return control.Groups;
			}
			protected override DependencyProperty GetSelectedItemProperty() {
				return NavBarControl.SelectedGroupProperty;
			}
		}
		public NavBarControlAdapterImpl(int prismVersion) { }
		public void Adapt(IRegionRuntimeWrapper region, NavBarControl regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
}
