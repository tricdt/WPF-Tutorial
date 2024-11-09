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

using DevExpress.Mvvm;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
namespace DevExpress.Xpf.Prism {
	public enum PrismVersion {
		Unknown, Prism5, Prism6, Prism7, Prism8
	}
	public class DXRegionManager {
		public static string GetRegionName(DependencyObject obj) {
			return (string)obj.GetValue(RegionNameProperty);
		}
		public static void SetRegionName(DependencyObject obj, string value) {
			obj.SetValue(RegionNameProperty, value);
		}
		public static readonly DependencyProperty RegionNameProperty =
			DependencyProperty.RegisterAttached("RegionName", typeof(string), typeof(DXRegionManager), new PropertyMetadata(null, OnRegionNameChanged));
		private static void OnRegionNameChanged(DependencyObject element, DependencyPropertyChangedEventArgs args) {
			if(DesignerProperties.GetIsInDesignMode(element))
				return;
			var prismVersion = GetPrismVersion();
			switch(prismVersion) {
				case PrismVersion.Prism8:
					Native.PrismWrappers.Prism800.RegionManagerRuntimeWrapper.SetRegionName(element, args.NewValue as string);
					break;
				case PrismVersion.Prism7:
					Native.PrismWrappers.Prism710.RegionManagerRuntimeWrapper.SetRegionName(element, args.NewValue as string);
					break;
				case PrismVersion.Prism6:
					CreateRegionPrism601(element);
					break;
				case PrismVersion.Prism5:
					CreateRegionPrism5(element);
					break;
			}
		}
		static void CreateRegionPrism5(DependencyObject element) {
			var locator = Native.PrismWrappers.Prism5.ServiceLocatorRuntimeWrapper.Current;
			var mappingsType = new DevExpress.Xpf.Native.PrismWrappers.Prism5.RegionAdapterMappingsRuntimeWrapper().Object.GetType();
			var mappings = DevExpress.Xpf.Native.PrismWrappers.Prism601.RegionAdapterMappingsRuntimeWrapper.Wrap(locator.GetInstance(mappingsType));
			var regionCreationBehavior = new DXDelayedRegionCreationBehavior(mappings, PrismVersion.Prism5);
			regionCreationBehavior.TargetElement = element;
			regionCreationBehavior.Attach();
		}
		static void CreateRegionPrism601(DependencyObject element) {
			var locator = Native.PrismWrappers.Prism601.ServiceLocatorRuntimeWrapper.Current;
			Type mappingsType = null;
			mappingsType = new DevExpress.Xpf.Native.PrismWrappers.Prism601.RegionAdapterMappingsRuntimeWrapper().Object.GetType();
			var mappings = DevExpress.Xpf.Native.PrismWrappers.Prism601.RegionAdapterMappingsRuntimeWrapper.Wrap(locator.GetInstance(mappingsType));
			var regionCreationBehavior = new DXDelayedRegionCreationBehavior(mappings, PrismVersion.Prism6);
			regionCreationBehavior.TargetElement = element;
			regionCreationBehavior.Attach();
		}
		static PrismVersion GetPrismVersion() {
			if(PrismVersion != PrismVersion.Unknown)
				return PrismVersion;
			if(LastRegisteredAdapterVersion != PrismVersion.Unknown)
				return LastRegisteredAdapterVersion;
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var prismAssemblies = assemblies.Where(x => x.GetName().Name == "Prism").ToList();
			if(prismAssemblies.Any(x => x.GetName().Version.Major == 8))
				return PrismVersion.Prism8;
			if (prismAssemblies.Any(x => x.GetName().Version.Major == 7))
				return PrismVersion.Prism7;
			if (prismAssemblies.Any(x => x.GetName().Version.Major == 6))
				return PrismVersion.Prism6;
			if (assemblies.Any(x => x.GetName().Name == "Microsoft.Practices.Prism.Composition" && x.GetName().Version.Major == 5))
				return PrismVersion.Prism5;
			throw new Exception(string.Format("Couldn't deduce the Prism version. Set the {0}.{1} property.",
				typeof(DXRegionManager).FullName, BindableBase.GetPropertyName(() => PrismVersion)));
		}
		internal static PrismVersion LastRegisteredAdapterVersion { get; set; }
		public static PrismVersion PrismVersion { get; set; }
	}
}
