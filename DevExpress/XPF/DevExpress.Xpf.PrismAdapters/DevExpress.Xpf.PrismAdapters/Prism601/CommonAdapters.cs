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

using DevExpress.Xpf.Native.PrismWrappers.Prism601;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
namespace DevExpress.Xpf.Native.Prism {
	public class AdapterBase<T> {
		protected IRegionRuntimeWrapper Region { get; private set; }
		protected T Target { get; private set; }
		protected IEnumerable<object> Views { get { return (IEnumerable<object>)Region.Views.Object; } }
		protected IEnumerable<object> ActiveViews { get { return (IEnumerable<object>)Region.ActiveViews.Object; } }
		public AdapterBase(IRegionRuntimeWrapper region, T target) {
			this.Region = region;
			this.Target = target;
			(((INotifyCollectionChanged)ActiveViews)).CollectionChanged += OnActiveViewsCollectionChanged;
			(((INotifyCollectionChanged)Views)).CollectionChanged += OnViewsCollectionChanged;
		}
		protected virtual void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { }
		protected virtual void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) { }
	}
	public abstract class ContentPresenterAdapterBase<T> : AdapterBase<T> {
		public ContentPresenterAdapterBase(IRegionRuntimeWrapper region, T target) : base(region, target) { }
		protected override void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if(e.Action == NotifyCollectionChangedAction.Add && !ActiveViews.Any())
				Region.Activate(e.NewItems[0]);
		}
		protected override void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			TargetContent = ActiveViews.FirstOrDefault();
		}
		protected abstract object TargetContent { get; set; }
	}
	public abstract class ItemsContainerAdapterBase<T, TChild> : AdapterBase<T> {
		public ItemsContainerAdapterBase(IRegionRuntimeWrapper region, T target) : base(region, target) { }
		protected abstract TChild CreateChild(object viewModel);
		protected abstract void RemoveChild(TChild child);
		protected abstract void ClearChildren(IEnumerable<TChild> children);
		protected abstract void SelectChild(TChild child);
		protected TChild GetChild(object viewModel) {
			if(viewModel == null)
				return default(TChild);
			TChild value;
			return children.TryGetValue(viewModel, out value) ? value : default(TChild);
		}
		protected object GetViewModel(TChild child) {
			if(child == null || !children.ContainsValue(child)) return null;
			return children.Where(x => object.Equals(child, x.Value)).First().Key;
		}
		protected override void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if(e.Action == NotifyCollectionChangedAction.Move
				|| e.Action == NotifyCollectionChangedAction.Replace)
				throw new NotImplementedException();
			if(e.Action == NotifyCollectionChangedAction.Reset && !Views.Any()) {
				Clear();
				return;
			}
			if(e.NewItems != null) foreach(var vm in e.NewItems) Add(vm);
			if(e.OldItems != null) foreach(var vm in e.OldItems) Remove(vm);
		}
		protected override void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			var old = selected;
			selected = ActiveViews.FirstOrDefault();
			if(old != selected) SelectChild(GetChild(selected));
		}
		void Add(object viewModel) {
			var res = CreateChild(viewModel);
			children.Add(viewModel, res);
		}
		void Remove(object viewModel) {
			TChild value;
			if(!children.TryGetValue(viewModel, out value))
				return;
			RemoveChild(value);
			children.Remove(viewModel);
		}
		void Clear() {
			ClearChildren(children.Values);
			children.Clear();
		}
		readonly Dictionary<object, TChild> children = new Dictionary<object, TChild>();
		object selected;
	}
}
