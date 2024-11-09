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
using DevExpress.Data.Internal;
namespace DevExpress.Xpf.Native.OrmMeta.Wcf {
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
		public static Func<string, string> GetAssemblyName;
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
			if (type == typeof(DataServiceResponseRuntimeWrapper))
				return DataServiceResponseRuntimeWrapper.Wrap(obj);
			if (type == typeof(DataServiceQueryExceptionRuntimeWrapper))
				return DataServiceQueryExceptionRuntimeWrapper.Wrap(obj);
			if (type == typeof(DataServiceRequestExceptionRuntimeWrapper))
				return DataServiceRequestExceptionRuntimeWrapper.Wrap(obj);
			if (type == typeof(OperationResponseRuntimeWrapper))
				return OperationResponseRuntimeWrapper.Wrap(obj);
			if (type == typeof(DataServiceContextRuntimeWrapper))
				return DataServiceContextRuntimeWrapper.Wrap(obj);
			if (type == typeof(DescriptorRuntimeWrapper))
				return DescriptorRuntimeWrapper.Wrap(obj);
			if (type == typeof(EntityDescriptorRuntimeWrapper))
				return EntityDescriptorRuntimeWrapper.Wrap(obj);
			if (type == typeof(LinkDescriptorRuntimeWrapper))
				return LinkDescriptorRuntimeWrapper.Wrap(obj);
			if (type == typeof(DataServiceQueryRuntimeWrapper))
				return DataServiceQueryRuntimeWrapper.Wrap(obj);
			return obj;
		}
	}
	partial class DataServiceResponseRuntimeWrapper : IWrapper, IEnumerable<OperationResponseRuntimeWrapper> {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DataServiceResponseRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DataServiceResponse") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DataServiceResponse") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DataServiceResponseRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DataServiceResponseRuntimeWrapper(obj, null);
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
		public static bool operator==(DataServiceResponseRuntimeWrapper left, DataServiceResponseRuntimeWrapper right) {
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
		public static bool operator!=(DataServiceResponseRuntimeWrapper left, DataServiceResponseRuntimeWrapper right) {
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
		class E : IEnumerator<OperationResponseRuntimeWrapper> {
			IEnumerator e;
			public E(IEnumerator e) { this.e = e; }
			public OperationResponseRuntimeWrapper Current {
				get { return OperationResponseRuntimeWrapper.Wrap(e.Current); }
			}
			object IEnumerator.Current { get { return Current; } }
			public void Dispose() { }
			public bool MoveNext() { return e.MoveNext(); }
			public void Reset() { e.Reset(); }
		}
		public IEnumerator<OperationResponseRuntimeWrapper> GetEnumerator() {
			return new E(((IEnumerable)Object).GetEnumerator());
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
	partial class DataServiceQueryExceptionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DataServiceQueryExceptionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DataServiceQueryException") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DataServiceQueryException") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DataServiceQueryExceptionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DataServiceQueryExceptionRuntimeWrapper(obj, null);
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
		public static bool operator==(DataServiceQueryExceptionRuntimeWrapper left, DataServiceQueryExceptionRuntimeWrapper right) {
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
		public static bool operator!=(DataServiceQueryExceptionRuntimeWrapper left, DataServiceQueryExceptionRuntimeWrapper right) {
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
		public String Message {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (String)RuntimeTypesHelper.GetProperty(__type, "Message").GetValue(Object, null)); }
		}
	}
	partial class DataServiceRequestExceptionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DataServiceRequestExceptionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DataServiceRequestException") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DataServiceRequestException") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DataServiceRequestExceptionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DataServiceRequestExceptionRuntimeWrapper(obj, null);
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
		public static bool operator==(DataServiceRequestExceptionRuntimeWrapper left, DataServiceRequestExceptionRuntimeWrapper right) {
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
		public static bool operator!=(DataServiceRequestExceptionRuntimeWrapper left, DataServiceRequestExceptionRuntimeWrapper right) {
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
		public String Message {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (String)RuntimeTypesHelper.GetProperty(__type, "Message").GetValue(Object, null)); }
		}
	}
	partial class OperationResponseRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected OperationResponseRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "OperationResponse") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "OperationResponse") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static OperationResponseRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new OperationResponseRuntimeWrapper(obj, null);
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
		public static bool operator==(OperationResponseRuntimeWrapper left, OperationResponseRuntimeWrapper right) {
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
		public static bool operator!=(OperationResponseRuntimeWrapper left, OperationResponseRuntimeWrapper right) {
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
	partial class DataServiceContextRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DataServiceContextRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DataServiceContext") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DataServiceContext") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DataServiceContextRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DataServiceContextRuntimeWrapper(obj, null);
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
		public static bool operator==(DataServiceContextRuntimeWrapper left, DataServiceContextRuntimeWrapper right) {
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
		public static bool operator!=(DataServiceContextRuntimeWrapper left, DataServiceContextRuntimeWrapper right) {
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
		public ReadOnlyCollectionRuntimeWrapper<EntityDescriptorRuntimeWrapper> Entities {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => ReadOnlyCollectionRuntimeWrapper<EntityDescriptorRuntimeWrapper>.Wrap(RuntimeTypesHelper.GetProperty(__type, "Entities").GetValue(Object, null))); }
		}
		public ReadOnlyCollectionRuntimeWrapper<LinkDescriptorRuntimeWrapper> Links {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => ReadOnlyCollectionRuntimeWrapper<LinkDescriptorRuntimeWrapper>.Wrap(RuntimeTypesHelper.GetProperty(__type, "Links").GetValue(Object, null))); }
		}
		public MergeOptionRuntimeWrapper MergeOption {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (MergeOptionRuntimeWrapper)RuntimeTypesHelper.GetProperty(__type, "MergeOption").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "MergeOption").SetValue(Object, value, null)); }
		}
		public DataServiceResponseRuntimeWrapper SaveChanges() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				DataServiceResponseRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "SaveChanges", false)
			let parameters = new string[] {  }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] {  })
			)
			);
		}
		public void UpdateObject(Object entity) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "UpdateObject", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)entity is IWrapper) ? ((IWrapper)(object)entity).Object : entity
		 })
			);
		}
		public EntityDescriptorRuntimeWrapper GetEntityDescriptor(Object entity) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				EntityDescriptorRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetEntityDescriptor", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)entity is IWrapper) ? ((IWrapper)(object)entity).Object : entity
		 })
			)
			);
		}
		public Boolean Detach(Object entity) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(Boolean)
			(from m in RuntimeTypesHelper.GetMethods(__type, "Detach", false)
			let parameters = new string[] { "Object" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)entity is IWrapper) ? ((IWrapper)(object)entity).Object : entity
		 })
			);
		}
	}
	partial class DataServiceCollectionRuntimeWrapper<T> : IWrapper, IEnumerable<T> {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DataServiceCollectionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DataServiceCollection`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DataServiceCollection`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DataServiceCollectionRuntimeWrapper<T> Wrap(object obj) {
			return obj == null ? null : new DataServiceCollectionRuntimeWrapper<T>(obj, null);
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
		public static bool operator==(DataServiceCollectionRuntimeWrapper<T> left, DataServiceCollectionRuntimeWrapper<T> right) {
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
		public static bool operator!=(DataServiceCollectionRuntimeWrapper<T> left, DataServiceCollectionRuntimeWrapper<T> right) {
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
		public DataServiceCollectionRuntimeWrapper(IEnumerable<T> items) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Microsoft.Data.Services.Client");
			__type = SafeTypeResolver.GetKnownType(assembly, "System.Data.Services.Client.DataServiceCollection`1");
			__type = __type.MakeGenericType(typeof(T));
			Object = Activator.CreateInstance(__type, new object[] { 
			((object)items is IWrapper) ? ((IWrapper)(object)items).Object : items
		 });
		}
		public void Load(T item) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Load", false)
			let parameters = new string[] { typeof(T).Name.Replace("RuntimeWrapper", "") }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)item is IWrapper) ? ((IWrapper)(object)item).Object : item
		 })
			);
		}
		public void Add(T item) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Add", false)
			let parameters = new string[] { typeof(T).Name.Replace("RuntimeWrapper", "") }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)item is IWrapper) ? ((IWrapper)(object)item).Object : item
		 })
			);
		}
		public Boolean Remove(T item) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(Boolean)
			(from m in RuntimeTypesHelper.GetMethods(__type, "Remove", false)
			let parameters = new string[] { typeof(T).Name.Replace("RuntimeWrapper", "") }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)item is IWrapper) ? ((IWrapper)(object)item).Object : item
		 })
			);
		}
		public Int32 IndexOf(T item) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(Int32)
			(from m in RuntimeTypesHelper.GetMethods(__type, "IndexOf", false)
			let parameters = new string[] { typeof(T).Name.Replace("RuntimeWrapper", "") }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)item is IWrapper) ? ((IWrapper)(object)item).Object : item
		 })
			);
		}
		public void RemoveAt(Int32 index) {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "RemoveAt", false)
			let parameters = new string[] { "Int32" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)index is IWrapper) ? ((IWrapper)(object)index).Object : index
		 })
			);
		}
		class E : IEnumerator<T> {
			IEnumerator e;
			public E(IEnumerator e) { this.e = e; }
			public T Current {
				get { return (T)(typeof(T).GetInterface("IWrapper") != null ? Wrapper.Wrap(typeof(T), e.Current) : e.Current); }
			}
			object IEnumerator.Current { get { return Current; } }
			public void Dispose() { }
			public bool MoveNext() { return e.MoveNext(); }
			public void Reset() { e.Reset(); }
		}
		public IEnumerator<T> GetEnumerator() {
			return new E(((IEnumerable)Object).GetEnumerator());
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
	partial class DescriptorRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DescriptorRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "Descriptor") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "Descriptor") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DescriptorRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DescriptorRuntimeWrapper(obj, null);
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
		public static bool operator==(DescriptorRuntimeWrapper left, DescriptorRuntimeWrapper right) {
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
		public static bool operator!=(DescriptorRuntimeWrapper left, DescriptorRuntimeWrapper right) {
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
		public EntityStatesRuntimeWrapper State {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (EntityStatesRuntimeWrapper)RuntimeTypesHelper.GetProperty(__type, "State").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "State").SetValue(Object, value, null)); }
		}
	}
	partial class EntityDescriptorRuntimeWrapper : DescriptorRuntimeWrapper, IWrapper {
		partial void Init();
		protected EntityDescriptorRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "EntityDescriptor") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "EntityDescriptor") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static EntityDescriptorRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new EntityDescriptorRuntimeWrapper(obj, null);
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
		public static bool operator==(EntityDescriptorRuntimeWrapper left, EntityDescriptorRuntimeWrapper right) {
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
		public static bool operator!=(EntityDescriptorRuntimeWrapper left, EntityDescriptorRuntimeWrapper right) {
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
	partial class LinkDescriptorRuntimeWrapper : DescriptorRuntimeWrapper, IWrapper {
		partial void Init();
		protected LinkDescriptorRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "LinkDescriptor") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "LinkDescriptor") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static LinkDescriptorRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new LinkDescriptorRuntimeWrapper(obj, null);
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
		public static bool operator==(LinkDescriptorRuntimeWrapper left, LinkDescriptorRuntimeWrapper right) {
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
		public static bool operator!=(LinkDescriptorRuntimeWrapper left, LinkDescriptorRuntimeWrapper right) {
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
	partial class DataServiceQueryRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DataServiceQueryRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DataServiceQuery") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DataServiceQuery") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DataServiceQueryRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DataServiceQueryRuntimeWrapper(obj, null);
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
		public static bool operator==(DataServiceQueryRuntimeWrapper left, DataServiceQueryRuntimeWrapper right) {
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
		public static bool operator!=(DataServiceQueryRuntimeWrapper left, DataServiceQueryRuntimeWrapper right) {
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
	partial class ReadOnlyCollectionRuntimeWrapper<T> : IWrapper, IEnumerable<T> {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected ReadOnlyCollectionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "ReadOnlyCollection`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "ReadOnlyCollection`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static ReadOnlyCollectionRuntimeWrapper<T> Wrap(object obj) {
			return obj == null ? null : new ReadOnlyCollectionRuntimeWrapper<T>(obj, null);
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
		public static bool operator==(ReadOnlyCollectionRuntimeWrapper<T> left, ReadOnlyCollectionRuntimeWrapper<T> right) {
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
		public static bool operator!=(ReadOnlyCollectionRuntimeWrapper<T> left, ReadOnlyCollectionRuntimeWrapper<T> right) {
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
		class E : IEnumerator<T> {
			IEnumerator e;
			public E(IEnumerator e) { this.e = e; }
			public T Current {
				get { return (T)(typeof(T).GetInterface("IWrapper") != null ? Wrapper.Wrap(typeof(T), e.Current) : e.Current); }
			}
			object IEnumerator.Current { get { return Current; } }
			public void Dispose() { }
			public bool MoveNext() { return e.MoveNext(); }
			public void Reset() { e.Reset(); }
		}
		public IEnumerator<T> GetEnumerator() {
			return new E(((IEnumerable)Object).GetEnumerator());
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
	enum MergeOptionRuntimeWrapper {
		AppendOnly = 0,
		OverwriteChanges = 1,
		PreserveChanges = 2,
		NoTracking = 3
	}
	enum EntityStatesRuntimeWrapper {
		Detached = 1,
		Unchanged = 2,
		Added = 4,
		Deleted = 8,
		Modified = 16
	}
}
