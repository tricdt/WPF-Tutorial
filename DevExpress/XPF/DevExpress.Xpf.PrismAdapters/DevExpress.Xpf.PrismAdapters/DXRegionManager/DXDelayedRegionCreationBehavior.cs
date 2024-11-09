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

using DevExpress.Data.Internal;
using DevExpress.Xpf.Native.PrismWrappers.Prism601;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
namespace DevExpress.Xpf.Prism {
	public class DXDelayedRegionCreationBehavior {
		private readonly RegionAdapterMappingsRuntimeWrapper regionAdapterMappings;
		private WeakReference elementWeakReference;
		private bool regionCreated;
		PrismVersion prismVersion;
		internal DXDelayedRegionCreationBehavior(RegionAdapterMappingsRuntimeWrapper regionAdapterMappings, PrismVersion prismVersion) {
			this.regionAdapterMappings = regionAdapterMappings;
			this.prismVersion = prismVersion;
		}
		public DependencyObject TargetElement {
			get { return this.elementWeakReference != null ? this.elementWeakReference.Target as DependencyObject : null; }
			set { this.elementWeakReference = new WeakReference(value); }
		}
		EventInfo GetUpdatingRegionsEvent() {
			var assembly = SafeTypeResolver.GetOrLoadAssembly(prismVersion == PrismVersion.Prism6 ? "Prism.Wpf" : "Microsoft.Practices.Prism.Composition");
			var type = SafeTypeResolver.GetKnownType(assembly, prismVersion == PrismVersion.Prism6 ? "Prism.Regions.RegionManager" : "Microsoft.Practices.Prism.Regions.RegionManager");
			var ev = type.GetEvent("UpdatingRegions", BindingFlags.Static | BindingFlags.Public);
			return ev;
		}
		public void Attach() {
			var ev = GetUpdatingRegionsEvent();
			var handler = typeof(DXDelayedRegionCreationBehavior).GetMethod("OnUpdatingRegions", BindingFlags.Public | BindingFlags.Instance);
			var del = Delegate.CreateDelegate(ev.EventHandlerType, this, handler);
			ev.GetAddMethod().Invoke(null, new object[] { del });
			this.WireUpTargetElement();
		}
		public void Detach() {
			var ev = GetUpdatingRegionsEvent();
			var handler = typeof(DXDelayedRegionCreationBehavior).GetMethod("OnUpdatingRegions", BindingFlags.Public | BindingFlags.Instance);
			var del = Delegate.CreateDelegate(ev.EventHandlerType, this, handler);
			ev.GetRemoveMethod().Invoke(null, new object[] { del });
			this.UnWireTargetElement();
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2109:ReviewVisibleEventHandlers")]
		public void OnUpdatingRegions(object sender, EventArgs e) {
			this.TryCreateRegion();
		}
		private void TryCreateRegion() {
			DependencyObject targetElement = this.TargetElement;
			if(targetElement == null) {
				this.Detach();
				return;
			}
			if(targetElement.CheckAccess()) {
				this.Detach();
				if(!this.regionCreated) {
					string regionName = DXRegionManager.GetRegionName(targetElement);
					CreateRegion(targetElement, regionName);
					this.regionCreated = true;
				}
			}
		}
		protected virtual object CreateRegion(DependencyObject targetElement, string regionName) {
			if(targetElement == null)
				throw new ArgumentNullException(nameof(targetElement));
			try {
				var regionAdapter = this.regionAdapterMappings.GetMapping(targetElement.GetType());
				return regionAdapter.Initialize(targetElement, regionName);
			} catch(Exception ex) {
				throw (Exception)new RegionCreationExceptionRuntimeWrapper(
					string.Format(CultureInfo.CurrentCulture, "RegionCreationException", regionName, ex), ex).Object;
			}
		}
		private void ElementLoaded(object sender, RoutedEventArgs e) {
			this.UnWireTargetElement();
			this.TryCreateRegion();
		}
		private void WireUpTargetElement() {
			var fe = this.TargetElement as FrameworkElement;
			if(fe != null) {
				fe.Loaded += this.ElementLoaded;
			}
			var fce = this.TargetElement as FrameworkContentElement;
			if(fce != null) {
				fce.Loaded += this.ElementLoaded;
			}
		}
		private void UnWireTargetElement() {
			var fe = this.TargetElement as FrameworkElement;
			if(fe != null) {
				fe.Loaded -= this.ElementLoaded;
			}
			var fce = this.TargetElement as FrameworkContentElement;
			if(fce != null) {
				fce.Loaded -= this.ElementLoaded;
			}
		}
	}
}
