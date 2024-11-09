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
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.NavBar;
using DevExpress.Xpf.Prism;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
namespace DevExpress.Xpf.Prism {
	public interface IPanelInfo {
		string GetPanelCaption();
	}
	public static class AdapterFactory {
		static ModuleBuilder moduleBuilder;
		private static AssemblyBuilder DefineDynamicAssembly(AssemblyName assemblyName) {
			return AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
		}
		private static AssemblyName GetDynamicAssemblyName() {
			return new AssemblyName(Guid.NewGuid().ToString());
		}
		static AdapterFactory() {
			var assemblyName = GetDynamicAssemblyName();
			var assembly = DefineDynamicAssembly(assemblyName);
			moduleBuilder = assembly.DefineDynamicModule(assemblyName.Name);
		}
		class AdapterInfo {
			public Type AdapterBase;
			public Type Adapter;
		}
		internal static MethodBuilder OverrideMethod(TypeBuilder type, Type baseType, string name) {
			return OverrideMethod(type, baseType, x => x.Name == name);
		}
		internal static MethodBuilder OverrideMethod(TypeBuilder type, Type baseType, Func<MethodInfo, bool> pred) {
			if(baseType.IsInterface) {
				type.AddInterfaceImplementation(baseType);
			} else {
				type.SetParent(baseType);
			}
			var method = baseType.GetMethods().FirstOrDefault(pred) ?? baseType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).First(pred);
			var @override = type.DefineMethod(
				method.Name,
				MethodAttributes.Public | MethodAttributes.Virtual,
				method.ReturnType,
				method.GetParameters().Select(x => x.ParameterType).ToArray());
			type.DefineMethodOverride(@override, method);
			return @override;
		}
		static List<AdapterInfo> infos = new List<AdapterInfo>();
		public class CreateRegionShim {
			public Xpf.Native.PrismWrappers.Prism5.IRegionRuntimeWrapper CreateRegionPrism5() {
				return Xpf.Native.PrismWrappers.Prism5.IRegionRuntimeWrapper.Wrap(
					new Xpf.Native.PrismWrappers.Prism5.SingleActiveRegionRuntimeWrapper());
			}
			public Xpf.Native.PrismWrappers.Prism601.IRegionRuntimeWrapper CreateRegionPrism601() {
				return Xpf.Native.PrismWrappers.Prism601.IRegionRuntimeWrapper.Wrap(
					new Xpf.Native.PrismWrappers.Prism601.SingleActiveRegionRuntimeWrapper());
			}
			public Xpf.Native.PrismWrappers.Prism710.IRegionRuntimeWrapper CreateRegionPrism710() {
				return Xpf.Native.PrismWrappers.Prism710.IRegionRuntimeWrapper.Wrap(
					new Xpf.Native.PrismWrappers.Prism710.SingleActiveRegionRuntimeWrapper());
			}
			public Xpf.Native.PrismWrappers.Prism800.IRegionRuntimeWrapper CreateRegionPrism800() {
				return Xpf.Native.PrismWrappers.Prism800.IRegionRuntimeWrapper.Wrap(
					new Xpf.Native.PrismWrappers.Prism800.SingleActiveRegionRuntimeWrapper());
			}
		}
		static LambdaExpression GetAdaptExprPrism601(Type controlType) {
			if (controlType == typeof(LayoutGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.LayoutGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(DocumentGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.DocumentGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(TabbedGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.TabbedGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(LayoutPanel))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.LayoutPanelAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(DXTabControl))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.DXTabControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(NavBarGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.NavBarGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(NavBarControl))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.NavBarControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (typeof(NavigationFrame).IsAssignableFrom(controlType))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.NavigationFrameAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (typeof(ContentControl).IsAssignableFrom(controlType))
				return (Expression<Action<DevExpress.Xpf.Native.Prism601.ContentControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			throw new NotImplementedException();
		}
		static LambdaExpression GetAdaptExprPrism710(Type controlType) {
			if (controlType == typeof(LayoutGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.LayoutGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(DocumentGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.DocumentGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(TabbedGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.TabbedGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(LayoutPanel))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.LayoutPanelAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(DXTabControl))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.DXTabControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(NavBarGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.NavBarGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (controlType == typeof(NavBarControl))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.NavBarControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (typeof(NavigationFrame).IsAssignableFrom(controlType))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.NavigationFrameAdapterImpl>>)(impl => impl.Adapt(null, null));
			if (typeof(ContentControl).IsAssignableFrom(controlType))
				return (Expression<Action<DevExpress.Xpf.Native.Prism710.ContentControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			throw new NotImplementedException();
		}
		static LambdaExpression GetAdaptExprPrism800(Type controlType) {
			if(controlType == typeof(LayoutGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.LayoutGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(controlType == typeof(DocumentGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.DocumentGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(controlType == typeof(TabbedGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.TabbedGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(controlType == typeof(LayoutPanel))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.LayoutPanelAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(controlType == typeof(DXTabControl))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.DXTabControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(controlType == typeof(NavBarGroup))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.NavBarGroupAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(controlType == typeof(NavBarControl))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.NavBarControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(typeof(NavigationFrame).IsAssignableFrom(controlType))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.NavigationFrameAdapterImpl>>)(impl => impl.Adapt(null, null));
			if(typeof(ContentControl).IsAssignableFrom(controlType))
				return (Expression<Action<DevExpress.Xpf.Native.Prism800.ContentControlAdapterImpl>>)(impl => impl.Adapt(null, null));
			throw new NotImplementedException();
		}
		static AdapterInfo EmitAdapter(Type adapterBase) {
			Type factoryType = null;
			var version = adapterBase.Assembly.GetName().Version;
			if (version.Major == 5) {
				var asm = SafeTypeResolver.GetOrLoadAssembly("Microsoft.Practices.Prism.Composition");
				factoryType = SafeTypeResolver.GetKnownType(asm, "Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory");
			} else if (version.Major == 6) {
				var asm = SafeTypeResolver.GetOrLoadAssembly("Prism.Wpf");
				factoryType = SafeTypeResolver.GetKnownType(asm, "Prism.Regions.IRegionBehaviorFactory");
			} else if (version.Major == 7) {
				var asm = SafeTypeResolver.GetOrLoadAssembly("Prism.Wpf");
				factoryType = SafeTypeResolver.GetKnownType(asm, "Prism.Regions.IRegionBehaviorFactory");
			} else if(version.Major == 8) {
				var asm = SafeTypeResolver.GetOrLoadAssembly("Prism.Wpf");
				factoryType = SafeTypeResolver.GetKnownType(asm, "Prism.Regions.IRegionBehaviorFactory");
			} else {
				throw new NotImplementedException();
			}
			var baseCtor = adapterBase.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).First();
			var controlType = adapterBase.GetGenericArguments().First();
			LambdaExpression adaptExpr = null;
			LambdaExpression createRegionExpr = null;
			LambdaExpression wrapExpr = null;
			LambdaExpression objectExpr = null;
			if (version.Major == 5) {
				DXRegionManager.LastRegisteredAdapterVersion = PrismVersion.Prism5;
				createRegionExpr = (Expression<Func<CreateRegionShim, object>>)(impl => impl.CreateRegionPrism5());
				adaptExpr = GetAdaptExprPrism601(controlType);
			} else if (version.Major == 6) {
				DXRegionManager.LastRegisteredAdapterVersion = PrismVersion.Prism6;
				createRegionExpr = (Expression<Func<CreateRegionShim, object>>)(impl => impl.CreateRegionPrism601());
				adaptExpr = GetAdaptExprPrism601(controlType);
			} else if (version.Major == 7) {
				DXRegionManager.LastRegisteredAdapterVersion = PrismVersion.Prism7;
				createRegionExpr = (Expression<Func<CreateRegionShim, object>>)(impl => impl.CreateRegionPrism710());
				adaptExpr = GetAdaptExprPrism710(controlType);
			} else if(version.Major == 8) {
				DXRegionManager.LastRegisteredAdapterVersion = PrismVersion.Prism8;
				createRegionExpr = (Expression<Func<CreateRegionShim, object>>)(impl => impl.CreateRegionPrism800());
				adaptExpr = GetAdaptExprPrism800(controlType);
			}
			wrapExpr = (Expression<Func<object, Xpf.Native.PrismWrappers.Prism601.IRegionRuntimeWrapper>>)(obj => Xpf.Native.PrismWrappers.Prism601.IRegionRuntimeWrapper.Wrap(obj));
			objectExpr = (Expression<Func<Xpf.Native.PrismWrappers.Prism601.IRegionRuntimeWrapper, object>>)(obj => obj.Object);
			Type implType = ((MethodCallExpression)adaptExpr.Body).Method.DeclaringType;
			var type = moduleBuilder.DefineType(string.Format("DX_{0}_{1}", adapterBase.Name, controlType.Name), TypeAttributes.Public, adapterBase);
			var implField = type.DefineField("impl", implType, FieldAttributes.Private);
			var ctor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new[] { factoryType });
			var il = ctor.GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Call, baseCtor);
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldc_I4, version.Major);
			il.Emit(OpCodes.Newobj, implType.GetConstructor(new[] { typeof(int) }));
			il.Emit(OpCodes.Stfld, implField);
			il.Emit(OpCodes.Ret);
			var createRegionMethod = ((MethodCallExpression)createRegionExpr.Body).Method;
			var objectProperty = (PropertyInfo)((MemberExpression)objectExpr.Body).Member;
			var wrapMethod = ((MethodCallExpression)wrapExpr.Body).Method;
			var adaptMethod = ((MethodCallExpression)adaptExpr.Body).Method;
			il = OverrideMethod(type, adapterBase, "Adapt").GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, implField);
			il.Emit(OpCodes.Ldarg_1);
			il.Emit(OpCodes.Call, wrapMethod);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, adaptMethod);
			il.Emit(OpCodes.Ret);
			il = OverrideMethod(type, adapterBase, "CreateRegion").GetILGenerator();
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, implField);
			il.Emit(OpCodes.Call, createRegionMethod);
			il.Emit(OpCodes.Call, objectProperty.GetGetMethod());
			il.Emit(OpCodes.Ret);
			return new AdapterInfo {
				Adapter = type.CreateType(),
				AdapterBase = adapterBase
			};
		}
		public static AdapterBase Make<AdapterBase>(object factory) {
			var adapterInfo = infos.FirstOrDefault(x => x.AdapterBase == typeof(AdapterBase));
			if (adapterInfo == null) {
				adapterInfo = EmitAdapter(typeof(AdapterBase));
				infos.Add(adapterInfo);
			}
			return (AdapterBase)Activator.CreateInstance(adapterInfo.Adapter, new object[] { factory });
		}
	}
}
