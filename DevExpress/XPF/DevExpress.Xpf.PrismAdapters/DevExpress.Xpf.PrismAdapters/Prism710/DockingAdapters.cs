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

using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Docking.Base;
using DevExpress.Xpf.Native.Prism;
using DevExpress.Xpf.Native.PrismWrappers.Prism710;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
namespace DevExpress.Xpf.Native.Prism710 {
	public class LayoutPanelAdapterImpl {
		public LayoutPanelAdapterImpl(int prismVersion) {
		}
		class Adapter : ContentPresenterAdapterBase<LayoutPanel> {
			public Adapter(IRegionRuntimeWrapper region, LayoutPanel target)
				: base(region, target) { }
			protected override object TargetContent { get { return Target.Content; } set { Target.Content = value; } }
		}
		public void Adapt(IRegionRuntimeWrapper region, LayoutPanel regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
	abstract class LayoutGroupAdapterBase<T, TChild> : ItemsContainerAdapterBase<T, TChild>
		where TChild : BaseLayoutItem {
		readonly static MethodInfo findDockLayoutManager;
		readonly static MethodInfo getActualClosingBehavior;
		static LayoutGroupAdapterBase() {
			findDockLayoutManager = typeof(LayoutItemsHelper).GetMethod("FindDockLayoutManager", BindingFlags.Static | BindingFlags.NonPublic);
			getActualClosingBehavior = typeof(DockControllerHelper).GetMethod("GetActualClosingBehavior", BindingFlags.Static | BindingFlags.NonPublic);
		}
		public LayoutGroupAdapterBase(IRegionRuntimeWrapper region, T target) : base(region, target) { }
		protected DockLayoutManager Manager {
			get {
				if(manager == null) InvalidateManager();
				return manager;
			}
			set {
				if(manager == value) return;
				var oldValue = manager;
				manager = value;
				OnManagerChanged(oldValue, manager);
			}
		}
		protected override void SelectChild(TChild child) {
			if(Manager == null || Manager.DockController == null || child == null) return;
			Manager.DockController.Restore(child);
			Manager.DockController.Activate(child);
		}
		protected void ConfigureChild(TChild child, object viewModel, object caption) {
			child.SetCurrentValue(ContentItem.DataContextProperty, viewModel);
			child.SetCurrentValue(ContentItem.ContentProperty, viewModel);
			child.SetCurrentValue(ContentItem.CaptionProperty, caption);
		}
		protected override void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			InvalidateManager();
			base.OnViewsCollectionChanged(sender, e);
		}
		protected virtual void OnManagerChanged(DockLayoutManager oldValue, DockLayoutManager newValue) {
			if(oldValue != null) {
				oldValue.DockItemActivated -= OnManagerDockItemActivated;
				oldValue.DockItemClosing -= OnManagerDockItemClosing;
			}
			if(newValue != null) {
				newValue.DockItemActivated += OnManagerDockItemActivated;
				newValue.DockItemClosing += OnManagerDockItemClosing;
			}
		}
		void OnManagerDockItemActivated(object sender, DockItemActivatedEventArgs e) {
			if(e.Item == null || !(e.Item is TChild)) return;
			var vm = GetViewModel((TChild)e.Item);
			if(vm != null)
				Region.Activate(vm);
			else if(ActiveViews.Any())
				Region.Deactivate(ActiveViews.FirstOrDefault());
		}
		void OnManagerDockItemClosing(object sender, ItemCancelEventArgs e) {
			if(e.Handled || e.Cancel) return;
			if(e.Item == null || !(e.Item is TChild)) return;
			var vm = GetViewModel((TChild)e.Item);
			if(vm == null) return;
			var closingBehavior = (ClosingBehavior)getActualClosingBehavior.Invoke(null, new object[] { Manager, e.Item });
			if(closingBehavior != ClosingBehavior.ImmediatelyRemove) {
				Region.Deactivate(vm);
				return;
			}
			Region.Remove(vm);
			e.Handled = true;
		}
		void InvalidateManager() {
			if(manager == null)
				Manager = (DockLayoutManager)findDockLayoutManager.Invoke(null, new object[] { Target });
		}
		DockLayoutManager manager;
	}
	abstract class LayoutGroupAdapter<T, TChild> : LayoutGroupAdapterBase<T, TChild>
		where T : LayoutGroup
		where TChild : LayoutPanel, new() {
		readonly static MethodInfo prepareContainerMethod;
		static LayoutGroupAdapter() {
			prepareContainerMethod = typeof(T).GetMethod("PrepareContainerForItemCore", BindingFlags.Instance | BindingFlags.NonPublic);
		}
		public LayoutGroupAdapter(IRegionRuntimeWrapper region, T target) : base(region, target) { }
		protected override TChild CreateChild(object viewModel) {
			var res = new TChild();
			ConfigureChild(res, viewModel, GetCaption(viewModel));
			Target.Add(res);
			prepareContainerMethod.Invoke(Target, new object[] { res });
			return res;
		}
		protected override void RemoveChild(TChild child) {
			if(Manager != null) {
				Manager.DockController.RemovePanel(child);
			} else if(Target.Items.Contains(child)) {
				Target.Items.Remove(child);
			}
		}
		protected override void ClearChildren(IEnumerable<TChild> children) {
			foreach(var documentPanel in children)
				RemoveChild(documentPanel);
		}
		object GetCaption(object viewModel) {
			if(viewModel is UIElement)
				return Target.CaptionTemplate != null || Target.CaptionTemplateSelector != null ? viewModel : viewModel.ToString();
			return viewModel;
		}
	}
	public class LayoutGroupAdapterImpl {
		public LayoutGroupAdapterImpl(int prismVersion) {
		}
		class Adapter : LayoutGroupAdapter<LayoutGroup, LayoutPanel> {
			public Adapter(IRegionRuntimeWrapper region, LayoutGroup target) : base(region, target) { }
		}
		public void Adapt(IRegionRuntimeWrapper region, LayoutGroup regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
	public class DocumentGroupAdapterImpl {
		public DocumentGroupAdapterImpl(int prismVersion) {
		}
		class Adapter : LayoutGroupAdapter<DocumentGroup, DocumentPanel> {
			public Adapter(IRegionRuntimeWrapper region, DocumentGroup target) : base(region, target) { }
		}
		public void Adapt(IRegionRuntimeWrapper region, DocumentGroup regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
	public class TabbedGroupAdapterImpl {
		public TabbedGroupAdapterImpl(int prismVersion) {
		}
		class Adapter : LayoutGroupAdapter<TabbedGroup, LayoutPanel> {
			public Adapter(IRegionRuntimeWrapper region, DocumentGroup target) : base(region, target) { }
		}
		public void Adapt(IRegionRuntimeWrapper region, DocumentGroup regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
}
