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

#pragma warning disable 108 // Use the new keyword if hiding was intended
using System;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq.Expressions;
using System.Windows;
using DevExpress.Data.Internal;
namespace DevExpress.Xpf.Native.PrismWrappers.Prism710 {
	class RuntimeTypesHelper {
		public static PropertyInfo GetProperty(Type type, string name) {
			return type.GetInterfaces()
				.SelectMany(i => i.GetProperties())
				.Concat(type.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
				.Where(p => p.Name == name)
				.First();
		}
		public static MethodInfo[] GetMethods(Type type, string name, bool generic) {
			return type.GetInterfaces()
				.SelectMany(i => i.GetMethods())
				.Concat(type.GetMethods())
				.Where(m => m.Name == name)
				.Where(m => m.IsGenericMethodDefinition == generic)
				.ToArray();
		}
		public static T UnwrapInvocationException<T>(Func<T> func) {
			try {
				return func();
			} catch (TargetInvocationException e) {
				throw e.InnerException;
			}
		}
		public static void UnwrapInvocationExceptionVoid(Action action) {
			UnwrapInvocationException(() => { action(); return string.Empty; });
		}
		public static Func<string, string> GetAssemblyName = null;
		public static Assembly LoadAssembly(string name) {
			if (GetAssemblyName != null)
				name = GetAssemblyName(name);
			return SafeTypeResolver.GetOrLoadAssembly(name);
		}
	}
	interface IWrapper {
		object Object { get; }
	}
	static class Wrapper {
		public static object Wrap(Type type, object obj) {
			if (type == typeof(NavigationParametersRuntimeWrapper))
				return NavigationParametersRuntimeWrapper.Wrap(obj);
			if (type == typeof(AllActiveRegionRuntimeWrapper))
				return AllActiveRegionRuntimeWrapper.Wrap(obj);
			if (type == typeof(SingleActiveRegionRuntimeWrapper))
				return SingleActiveRegionRuntimeWrapper.Wrap(obj);
			if (type == typeof(IViewsCollectionRuntimeWrapper))
				return IViewsCollectionRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionRuntimeWrapper))
				return IRegionRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionNavigationJournalEntryRuntimeWrapper))
				return IRegionNavigationJournalEntryRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionNavigationJournalRuntimeWrapper))
				return IRegionNavigationJournalRuntimeWrapper.Wrap(obj);
			if (type == typeof(INavigateAsyncRuntimeWrapper))
				return INavigateAsyncRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionNavigationServiceRuntimeWrapper))
				return IRegionNavigationServiceRuntimeWrapper.Wrap(obj);
			if (type == typeof(NavigationContextRuntimeWrapper))
				return NavigationContextRuntimeWrapper.Wrap(obj);
			if (type == typeof(NavigationResultRuntimeWrapper))
				return NavigationResultRuntimeWrapper.Wrap(obj);
			if (type == typeof(RegionBehaviorRuntimeWrapper))
				return RegionBehaviorRuntimeWrapper.Wrap(obj);
			if (type == typeof(IHostAwareRegionBehaviorRuntimeWrapper))
				return IHostAwareRegionBehaviorRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionCollectionRuntimeWrapper))
				return IRegionCollectionRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionManagerRuntimeWrapper))
				return IRegionManagerRuntimeWrapper.Wrap(obj);
			if (type == typeof(RegionRuntimeWrapper))
				return RegionRuntimeWrapper.Wrap(obj);
			if (type == typeof(INavigationAwareRuntimeWrapper))
				return INavigationAwareRuntimeWrapper.Wrap(obj);
			if (type == typeof(IConfirmNavigationRequestRuntimeWrapper))
				return IConfirmNavigationRequestRuntimeWrapper.Wrap(obj);
			if (type == typeof(IRegionAdapterRuntimeWrapper))
				return IRegionAdapterRuntimeWrapper.Wrap(obj);
			if (type == typeof(RegionAdapterMappingsRuntimeWrapper))
				return RegionAdapterMappingsRuntimeWrapper.Wrap(obj);
			if (type == typeof(IServiceLocatorRuntimeWrapper))
				return IServiceLocatorRuntimeWrapper.Wrap(obj);
			if (type == typeof(RegionCreationExceptionRuntimeWrapper))
				return RegionCreationExceptionRuntimeWrapper.Wrap(obj);
			return obj;
		}
	}
	partial class NavigationParametersRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected NavigationParametersRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "NavigationParameters") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "NavigationParameters") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static NavigationParametersRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new NavigationParametersRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(NavigationParametersRuntimeWrapper left, NavigationParametersRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(NavigationParametersRuntimeWrapper left, NavigationParametersRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
	}
	partial class AllActiveRegionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected AllActiveRegionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "AllActiveRegion") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "AllActiveRegion") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static AllActiveRegionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new AllActiveRegionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(AllActiveRegionRuntimeWrapper left, AllActiveRegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(AllActiveRegionRuntimeWrapper left, AllActiveRegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public AllActiveRegionRuntimeWrapper() {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.AllActiveRegion");
			Object = Activator.CreateInstance(__type, EmptyArray<object>.Instance);
		}
	}
	partial class SingleActiveRegionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected SingleActiveRegionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "SingleActiveRegion") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "SingleActiveRegion") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static SingleActiveRegionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new SingleActiveRegionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(SingleActiveRegionRuntimeWrapper left, SingleActiveRegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(SingleActiveRegionRuntimeWrapper left, SingleActiveRegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public SingleActiveRegionRuntimeWrapper() {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.SingleActiveRegion");
			Object = Activator.CreateInstance(__type, EmptyArray<object>.Instance);
		}
	}
	partial class IViewsCollectionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IViewsCollectionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IViewsCollection") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IViewsCollection") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IViewsCollectionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IViewsCollectionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IViewsCollectionRuntimeWrapper left, IViewsCollectionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IViewsCollectionRuntimeWrapper left, IViewsCollectionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
	}
	partial class IRegionRuntimeWrapper : INavigateAsyncRuntimeWrapper, IWrapper {
		partial void Init();
		protected IRegionRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegion") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegion") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static IRegionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionRuntimeWrapper left, IRegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionRuntimeWrapper left, IRegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public IViewsCollectionRuntimeWrapper Views {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IViewsCollectionRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Views").GetValue(Object, null))); }
		}
		public IViewsCollectionRuntimeWrapper ActiveViews {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IViewsCollectionRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "ActiveViews").GetValue(Object, null))); }
		}
		public IRegionNavigationServiceRuntimeWrapper NavigationService {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionNavigationServiceRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "NavigationService").GetValue(Object, null))); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "NavigationService").SetValue(Object, ((IWrapper)value).Object, null)); }
		}
		public void Activate(Object view) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Activate", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)view is IWrapper) ? ((IWrapper)(object)view).Object : view
		 })
			);
		}
		public void Deactivate(Object view) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Deactivate", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)view is IWrapper) ? ((IWrapper)(object)view).Object : view
		 })
			);
		}
		public void Remove(Object view) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Remove", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)view is IWrapper) ? ((IWrapper)(object)view).Object : view
		 })
			);
		}
		public IRegionManagerRuntimeWrapper Add(Object view) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IRegionManagerRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Add", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)view is IWrapper) ? ((IWrapper)(object)view).Object : view
		 })
			)
			);
		}
	}
	partial class IRegionNavigationJournalEntryRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IRegionNavigationJournalEntryRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegionNavigationJournalEntry") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegionNavigationJournalEntry") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IRegionNavigationJournalEntryRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionNavigationJournalEntryRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionNavigationJournalEntryRuntimeWrapper left, IRegionNavigationJournalEntryRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionNavigationJournalEntryRuntimeWrapper left, IRegionNavigationJournalEntryRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public Uri Uri {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Uri)RuntimeTypesHelper.GetProperty(__type, "Uri").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Uri").SetValue(Object, value, null)); }
		}
	}
	partial class IRegionNavigationJournalRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IRegionNavigationJournalRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegionNavigationJournal") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegionNavigationJournal") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IRegionNavigationJournalRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionNavigationJournalRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionNavigationJournalRuntimeWrapper left, IRegionNavigationJournalRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionNavigationJournalRuntimeWrapper left, IRegionNavigationJournalRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public Boolean CanGoBack {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "CanGoBack").GetValue(Object, null)); }
		}
		public Boolean CanGoForward {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "CanGoForward").GetValue(Object, null)); }
		}
		public IRegionNavigationJournalEntryRuntimeWrapper CurrentEntry {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionNavigationJournalEntryRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "CurrentEntry").GetValue(Object, null))); }
		}
		public INavigateAsyncRuntimeWrapper NavigationTarget {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => INavigateAsyncRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "NavigationTarget").GetValue(Object, null))); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "NavigationTarget").SetValue(Object, ((IWrapper)value).Object, null)); }
		}
		public void Clear() {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Clear", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			);
		}
		public void GoBack() {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "GoBack", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			);
		}
		public void GoForward() {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "GoForward", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			);
		}
		public void RecordNavigation(IRegionNavigationJournalEntryRuntimeWrapper entry, Boolean persistInHistory) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "RecordNavigation", false)
			let parameters = new string[] { "IRegionNavigationJournalEntry", "Boolean" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)entry).Object
		, 
			((object)persistInHistory is IWrapper) ? ((IWrapper)(object)persistInHistory).Object : persistInHistory
		 })
			);
		}
	}
	partial class INavigateAsyncRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected INavigateAsyncRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "INavigateAsync") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "INavigateAsync") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static INavigateAsyncRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new INavigateAsyncRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(INavigateAsyncRuntimeWrapper left, INavigateAsyncRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(INavigateAsyncRuntimeWrapper left, INavigateAsyncRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public void RequestNavigate(Uri target, Action<NavigationResultRuntimeWrapper> navigationCallback) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "RequestNavigate", false)
			let parameters = new string[] { "Uri", "Action`1" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)target is IWrapper) ? ((IWrapper)(object)target).Object : target
		, 
			((object)navigationCallback is IWrapper) ? ((IWrapper)(object)navigationCallback).Object : navigationCallback
		 })
			);
		}
	}
	partial class IRegionNavigationServiceRuntimeWrapper : INavigateAsyncRuntimeWrapper, IWrapper {
		partial void Init();
		protected IRegionNavigationServiceRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegionNavigationService") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegionNavigationService") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static IRegionNavigationServiceRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionNavigationServiceRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionNavigationServiceRuntimeWrapper left, IRegionNavigationServiceRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionNavigationServiceRuntimeWrapper left, IRegionNavigationServiceRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public IRegionNavigationJournalRuntimeWrapper Journal {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionNavigationJournalRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Journal").GetValue(Object, null))); }
		}
		public IRegionRuntimeWrapper Region {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Region").GetValue(Object, null))); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Region").SetValue(Object, ((IWrapper)value).Object, null)); }
		}
	}
	partial class NavigationContextRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected NavigationContextRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "NavigationContext") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "NavigationContext") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static NavigationContextRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new NavigationContextRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(NavigationContextRuntimeWrapper left, NavigationContextRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(NavigationContextRuntimeWrapper left, NavigationContextRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public NavigationContextRuntimeWrapper(IRegionNavigationServiceRuntimeWrapper navigationService, Uri uri) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.NavigationContext");
			Object = Activator.CreateInstance(__type, new object[] { 
				((IWrapper)navigationService).Object
		,
			((object)uri is IWrapper) ? ((IWrapper)(object)uri).Object : uri
		 });
		}
		public IRegionNavigationServiceRuntimeWrapper NavigationService {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionNavigationServiceRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "NavigationService").GetValue(Object, null))); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "NavigationService").SetValue(Object, ((IWrapper)value).Object, null)); }
		}
		public NavigationParametersRuntimeWrapper Parameters {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => NavigationParametersRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Parameters").GetValue(Object, null))); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Parameters").SetValue(Object, ((IWrapper)value).Object, null)); }
		}
		public Uri Uri {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Uri)RuntimeTypesHelper.GetProperty(__type, "Uri").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Uri").SetValue(Object, value, null)); }
		}
	}
	partial class NavigationResultRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected NavigationResultRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "NavigationResult") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "NavigationResult") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static NavigationResultRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new NavigationResultRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(NavigationResultRuntimeWrapper left, NavigationResultRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(NavigationResultRuntimeWrapper left, NavigationResultRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public NavigationResultRuntimeWrapper(NavigationContextRuntimeWrapper context, Nullable<Boolean> result) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.NavigationResult");
			Object = Activator.CreateInstance(__type, new object[] { 
				((IWrapper)context).Object
		,
			((object)result is IWrapper) ? ((IWrapper)(object)result).Object : result
		 });
		}
		public NavigationResultRuntimeWrapper(NavigationContextRuntimeWrapper context, Exception error) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.NavigationResult");
			Object = Activator.CreateInstance(__type, new object[] { 
				((IWrapper)context).Object
		,
			((object)error is IWrapper) ? ((IWrapper)(object)error).Object : error
		 });
		}
		public Exception Error {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Exception)RuntimeTypesHelper.GetProperty(__type, "Error").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Error").SetValue(Object, value, null)); }
		}
		public Nullable<Boolean> Result {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Nullable<Boolean>)RuntimeTypesHelper.GetProperty(__type, "Result").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Result").SetValue(Object, value, null)); }
		}
	}
	partial class RegionBehaviorRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected RegionBehaviorRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "RegionBehavior") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "RegionBehavior") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static RegionBehaviorRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new RegionBehaviorRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(RegionBehaviorRuntimeWrapper left, RegionBehaviorRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(RegionBehaviorRuntimeWrapper left, RegionBehaviorRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public IRegionRuntimeWrapper Region {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Region").GetValue(Object, null))); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "Region").SetValue(Object, ((IWrapper)value).Object, null)); }
		}
	}
	partial class IHostAwareRegionBehaviorRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IHostAwareRegionBehaviorRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IHostAwareRegionBehavior") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IHostAwareRegionBehavior") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IHostAwareRegionBehaviorRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IHostAwareRegionBehaviorRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IHostAwareRegionBehaviorRuntimeWrapper left, IHostAwareRegionBehaviorRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IHostAwareRegionBehaviorRuntimeWrapper left, IHostAwareRegionBehaviorRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
	}
	partial class IRegionCollectionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IRegionCollectionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegionCollection") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegionCollection") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IRegionCollectionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionCollectionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionCollectionRuntimeWrapper left, IRegionCollectionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionCollectionRuntimeWrapper left, IRegionCollectionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public void Add(IRegionRuntimeWrapper region) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Add", false)
			let parameters = new string[] { "IRegion" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)region).Object
		 })
			);
		}
	}
	partial class IRegionManagerRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IRegionManagerRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegionManager") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegionManager") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IRegionManagerRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionManagerRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionManagerRuntimeWrapper left, IRegionManagerRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionManagerRuntimeWrapper left, IRegionManagerRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public IRegionCollectionRuntimeWrapper Regions {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IRegionCollectionRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Regions").GetValue(Object, null))); }
		}
	}
	partial class RegionAdapterBaseRuntimeWrapper<T> : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected RegionAdapterBaseRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "RegionAdapterBase`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "RegionAdapterBase`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static RegionAdapterBaseRuntimeWrapper<T> Wrap(object obj) {
			return obj == null ? null : new RegionAdapterBaseRuntimeWrapper<T>(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(RegionAdapterBaseRuntimeWrapper<T> left, RegionAdapterBaseRuntimeWrapper<T> right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(RegionAdapterBaseRuntimeWrapper<T> left, RegionAdapterBaseRuntimeWrapper<T> right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
	}
	partial class RegionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected RegionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "Region") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "Region") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static RegionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new RegionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(RegionRuntimeWrapper left, RegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(RegionRuntimeWrapper left, RegionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public RegionRuntimeWrapper() {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.Region");
			Object = Activator.CreateInstance(__type, EmptyArray<object>.Instance);
		}
	}
	partial class INavigationAwareRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected INavigationAwareRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "INavigationAware") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "INavigationAware") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static INavigationAwareRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new INavigationAwareRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(INavigationAwareRuntimeWrapper left, INavigationAwareRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(INavigationAwareRuntimeWrapper left, INavigationAwareRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public Boolean IsNavigationTarget(NavigationContextRuntimeWrapper navigationContext) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(Boolean)
			(from m in RuntimeTypesHelper.GetMethods(__type, "IsNavigationTarget", false)
			let parameters = new string[] { "NavigationContext" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)navigationContext).Object
		 })
			);
		}
		public void OnNavigatedFrom(NavigationContextRuntimeWrapper navigationContext) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "OnNavigatedFrom", false)
			let parameters = new string[] { "NavigationContext" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)navigationContext).Object
		 })
			);
		}
		public void OnNavigatedTo(NavigationContextRuntimeWrapper navigationContext) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "OnNavigatedTo", false)
			let parameters = new string[] { "NavigationContext" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)navigationContext).Object
		 })
			);
		}
	}
	partial class IConfirmNavigationRequestRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IConfirmNavigationRequestRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IConfirmNavigationRequest") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IConfirmNavigationRequest") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IConfirmNavigationRequestRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IConfirmNavigationRequestRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IConfirmNavigationRequestRuntimeWrapper left, IConfirmNavigationRequestRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IConfirmNavigationRequestRuntimeWrapper left, IConfirmNavigationRequestRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public void ConfirmNavigationRequest(NavigationContextRuntimeWrapper navigationContext, Action<Boolean> continuationCallback) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "ConfirmNavigationRequest", false)
			let parameters = new string[] { "NavigationContext", "Action`1" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)navigationContext).Object
		, 
			((object)continuationCallback is IWrapper) ? ((IWrapper)(object)continuationCallback).Object : continuationCallback
		 })
			);
		}
	}
	partial class IRegionAdapterRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IRegionAdapterRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IRegionAdapter") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IRegionAdapter") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IRegionAdapterRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IRegionAdapterRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IRegionAdapterRuntimeWrapper left, IRegionAdapterRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IRegionAdapterRuntimeWrapper left, IRegionAdapterRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public IRegionRuntimeWrapper Initialize(Object regionTarget, String regionName) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IRegionRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Initialize", false)
			let parameters = new string[] { "Object", "String" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)regionTarget is IWrapper) ? ((IWrapper)(object)regionTarget).Object : regionTarget
		, 
			((object)regionName is IWrapper) ? ((IWrapper)(object)regionName).Object : regionName
		 })
			)
			);
		}
	}
	partial class RegionAdapterMappingsRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected RegionAdapterMappingsRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "RegionAdapterMappings") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "RegionAdapterMappings") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static RegionAdapterMappingsRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new RegionAdapterMappingsRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(RegionAdapterMappingsRuntimeWrapper left, RegionAdapterMappingsRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(RegionAdapterMappingsRuntimeWrapper left, RegionAdapterMappingsRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public RegionAdapterMappingsRuntimeWrapper() {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.RegionAdapterMappings");
			Object = Activator.CreateInstance(__type, EmptyArray<object>.Instance);
		}
		public IRegionAdapterRuntimeWrapper GetMapping(Type controlType) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IRegionAdapterRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetMapping", false)
			let parameters = new string[] { "Type" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)controlType is IWrapper) ? ((IWrapper)(object)controlType).Object : controlType
		 })
			)
			);
		}
	}
	partial class IServiceLocatorRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IServiceLocatorRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IServiceLocator") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IServiceLocator") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IServiceLocatorRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IServiceLocatorRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(IServiceLocatorRuntimeWrapper left, IServiceLocatorRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(IServiceLocatorRuntimeWrapper left, IServiceLocatorRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public TService GetInstance<TService>() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(TService)
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetInstance", true)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
			.MakeGenericMethod(typeof(TService))
		.Invoke(Object, EmptyArray<object>.Instance)
			);
		}
		public Object GetInstance(Type serviceType) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(Object)
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetInstance", false)
			let parameters = new string[] { "Type" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)serviceType is IWrapper) ? ((IWrapper)(object)serviceType).Object : serviceType
		 })
			);
		}
	}
	partial class RegionCreationExceptionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected RegionCreationExceptionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "RegionCreationException") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "RegionCreationException") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static RegionCreationExceptionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new RegionCreationExceptionRuntimeWrapper(obj, null);
		}
		public override bool Equals(object obj) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null) {
				obj = asWrapper.Object;
			}
			if (Object != null)
				return Object.Equals(obj);
			return base.Equals(obj);
		}
		public override int GetHashCode() {
			if (Object != null)
				return Object.GetHashCode();
			return base.GetHashCode();
		}
		public static bool operator==(RegionCreationExceptionRuntimeWrapper left, RegionCreationExceptionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Equality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right == null;
				if ((object)right == null)
					return (object)left == null;
				return ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public static bool operator!=(RegionCreationExceptionRuntimeWrapper left, RegionCreationExceptionRuntimeWrapper right) {
			var comp = left.Object.GetType().GetMethod("op_Inequality", BindingFlags.Static | BindingFlags.Public);
			if (comp == null) {
				if ((object)left == null)
					return (object)right != null;
				if ((object)right == null)
					return (object)left != null;
				return !ReferenceEquals(left.Object, right.Object);
			}
			return (bool)comp.Invoke(null, new [] { (object)left == null ? null : left.Object, (object)right == null ? null : right.Object });
		}
		public RegionCreationExceptionRuntimeWrapper(String message, Exception inner) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			__type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.Behaviors.RegionCreationException");
			Object = Activator.CreateInstance(__type, new object[] { 
			((object)message is IWrapper) ? ((IWrapper)(object)message).Object : message
		,
			((object)inner is IWrapper) ? ((IWrapper)(object)inner).Object : inner
		 });
		}
	}
	static partial class RegionManagerRuntimeWrapper {
		public static String GetRegionName(DependencyObject regionTarget) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			var __type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.RegionManager");
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(String)
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetRegionName", false)
			let parameters = new string[] { "DependencyObject" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(null, new object[] { 
			((object)regionTarget is IWrapper) ? ((IWrapper)(object)regionTarget).Object : regionTarget
		 })
			);
		}
		public static void SetRegionName(DependencyObject regionTarget, String regionName) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Prism.Wpf");
			var __type = SafeTypeResolver.GetKnownType(assembly, "Prism.Regions.RegionManager");
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "SetRegionName", false)
			let parameters = new string[] { "DependencyObject", "String" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(null, new object[] { 
			((object)regionTarget is IWrapper) ? ((IWrapper)(object)regionTarget).Object : regionTarget
		, 
			((object)regionName is IWrapper) ? ((IWrapper)(object)regionName).Object : regionName
		 })
			);
		}
	}
	static partial class ServiceLocatorRuntimeWrapper {
		public static IServiceLocatorRuntimeWrapper Current {
			get {
				var assembly = RuntimeTypesHelper.LoadAssembly("CommonServiceLocator");
				var __type = SafeTypeResolver.GetKnownType(assembly, "CommonServiceLocator.ServiceLocator");
				return RuntimeTypesHelper.UnwrapInvocationException(() => IServiceLocatorRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Current").GetValue(null, null)));
			}
		}
	}
}
