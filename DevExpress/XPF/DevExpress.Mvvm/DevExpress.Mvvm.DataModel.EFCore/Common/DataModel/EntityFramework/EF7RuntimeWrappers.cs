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
namespace DevExpress.Xpf.Native.OrmMeta.EF7 {
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
			if (type == typeof(DbContextRuntimeWrapper))
				return DbContextRuntimeWrapper.Wrap(obj);
			if (type == typeof(ChangeTrackerRuntimeWrapper))
				return ChangeTrackerRuntimeWrapper.Wrap(obj);
			if (type == typeof(EntityEntryRuntimeWrapper))
				return EntityEntryRuntimeWrapper.Wrap(obj);
			if (type == typeof(DbUpdateExceptionRuntimeWrapper))
				return DbUpdateExceptionRuntimeWrapper.Wrap(obj);
			if (type == typeof(IModelRuntimeWrapper))
				return IModelRuntimeWrapper.Wrap(obj);
			if (type == typeof(IEntityTypeRuntimeWrapper))
				return IEntityTypeRuntimeWrapper.Wrap(obj);
			if (type == typeof(INavigationRuntimeWrapper))
				return INavigationRuntimeWrapper.Wrap(obj);
			if (type == typeof(IForeignKeyRuntimeWrapper))
				return IForeignKeyRuntimeWrapper.Wrap(obj);
			if (type == typeof(IKeyRuntimeWrapper))
				return IKeyRuntimeWrapper.Wrap(obj);
			if (type == typeof(IPropertyRuntimeWrapper))
				return IPropertyRuntimeWrapper.Wrap(obj);
			if (type == typeof(IPropertyBaseRuntimeWrapper))
				return IPropertyBaseRuntimeWrapper.Wrap(obj);
			return obj;
		}
	}
	partial class DbContextRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DbContextRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DbContext") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DbContext") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DbContextRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DbContextRuntimeWrapper(obj, null);
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
		public static bool operator==(DbContextRuntimeWrapper left, DbContextRuntimeWrapper right) {
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
		public static bool operator!=(DbContextRuntimeWrapper left, DbContextRuntimeWrapper right) {
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
		public IModelRuntimeWrapper Model {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IModelRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Model").GetValue(Object, null))); }
		}
		public ChangeTrackerRuntimeWrapper ChangeTracker {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => ChangeTrackerRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "ChangeTracker").GetValue(Object, null))); }
		}
		public EntityEntryRuntimeWrapper<TEntity> Entry<TEntity>(TEntity entity) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				EntityEntryRuntimeWrapper<TEntity>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Entry", true)
			let parameters = new string[] { "TEntity" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
			.MakeGenericMethod(typeof(TEntity))
		.Invoke(Object, new object[] { 
			((object)entity is IWrapper) ? ((IWrapper)(object)entity).Object : entity
		 })
			)
			);
		}
		public Int32 SaveChanges() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(Int32)
			(from m in RuntimeTypesHelper.GetMethods(__type, "SaveChanges", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			);
		}
		public DbSetRuntimeWrapper<TEntity> Set<TEntity>() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				DbSetRuntimeWrapper<TEntity>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Set", true)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
			.MakeGenericMethod(typeof(TEntity))
		.Invoke(Object, EmptyArray<object>.Instance)
			)
			);
		}
	}
	partial class DbSetRuntimeWrapper<TEntity> : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DbSetRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DbSet`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DbSet`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DbSetRuntimeWrapper<TEntity> Wrap(object obj) {
			return obj == null ? null : new DbSetRuntimeWrapper<TEntity>(obj, null);
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
		public static bool operator==(DbSetRuntimeWrapper<TEntity> left, DbSetRuntimeWrapper<TEntity> right) {
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
		public static bool operator!=(DbSetRuntimeWrapper<TEntity> left, DbSetRuntimeWrapper<TEntity> right) {
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
		public EntityEntryRuntimeWrapper<TEntity> Add(TEntity entity) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				EntityEntryRuntimeWrapper<TEntity>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Add", false)
			let parameters = new string[] { typeof(TEntity).Name.Replace("RuntimeWrapper", "") }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)entity is IWrapper) ? ((IWrapper)(object)entity).Object : entity
		 })
			)
			);
		}
		public EntityEntryRuntimeWrapper<TEntity> Remove(TEntity entity) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				EntityEntryRuntimeWrapper<TEntity>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Remove", false)
			let parameters = new string[] { typeof(TEntity).Name.Replace("RuntimeWrapper", "") }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)entity is IWrapper) ? ((IWrapper)(object)entity).Object : entity
		 })
			)
			);
		}
		public TEntity Find(Object[] keyValues) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				(TEntity)
			(from m in RuntimeTypesHelper.GetMethods(__type, "Find", false)
			let parameters = new string[] { "Object[]" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
			((object)keyValues is IWrapper) ? ((IWrapper)(object)keyValues).Object : keyValues
		 })
			);
		}
	}
	partial class ChangeTrackerRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected ChangeTrackerRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "ChangeTracker") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "ChangeTracker") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static ChangeTrackerRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new ChangeTrackerRuntimeWrapper(obj, null);
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
		public static bool operator==(ChangeTrackerRuntimeWrapper left, ChangeTrackerRuntimeWrapper right) {
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
		public static bool operator!=(ChangeTrackerRuntimeWrapper left, ChangeTrackerRuntimeWrapper right) {
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
		public IEnumerableRuntimeWrapper<EntityEntryRuntimeWrapper> Entries() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEnumerableRuntimeWrapper<EntityEntryRuntimeWrapper>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Entries", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			)
			);
		}
	}
	partial class EntityEntryRuntimeWrapper<TEntity> : EntityEntryRuntimeWrapper, IWrapper {
		partial void Init();
		protected EntityEntryRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "EntityEntry`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "EntityEntry`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static EntityEntryRuntimeWrapper<TEntity> Wrap(object obj) {
			return obj == null ? null : new EntityEntryRuntimeWrapper<TEntity>(obj, null);
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
		public static bool operator==(EntityEntryRuntimeWrapper<TEntity> left, EntityEntryRuntimeWrapper<TEntity> right) {
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
		public static bool operator!=(EntityEntryRuntimeWrapper<TEntity> left, EntityEntryRuntimeWrapper<TEntity> right) {
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
		public TEntity Entity {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (TEntity)RuntimeTypesHelper.GetProperty(__type, "Entity").GetValue(Object, null)); }
		}
		public IEntityTypeRuntimeWrapper Metadata {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IEntityTypeRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Metadata").GetValue(Object, null))); }
		}
	}
	partial class EntityEntryRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected EntityEntryRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "EntityEntry") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "EntityEntry") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static EntityEntryRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new EntityEntryRuntimeWrapper(obj, null);
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
		public static bool operator==(EntityEntryRuntimeWrapper left, EntityEntryRuntimeWrapper right) {
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
		public static bool operator!=(EntityEntryRuntimeWrapper left, EntityEntryRuntimeWrapper right) {
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
		public Object Entity {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Object)RuntimeTypesHelper.GetProperty(__type, "Entity").GetValue(Object, null)); }
		}
		public DbContextRuntimeWrapper Context {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => DbContextRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Context").GetValue(Object, null))); }
		}
		public EntityStateRuntimeWrapper State {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (EntityStateRuntimeWrapper)RuntimeTypesHelper.GetProperty(__type, "State").GetValue(Object, null)); }
			set { RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() => RuntimeTypesHelper.GetProperty(__type, "State").SetValue(Object, value, null)); }
		}
		public void Reload() {
			RuntimeTypesHelper.UnwrapInvocationExceptionVoid(() =>
			(from m in RuntimeTypesHelper.GetMethods(__type, "Reload", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			);
		}
	}
	partial class DbUpdateExceptionRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected DbUpdateExceptionRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "DbUpdateException") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "DbUpdateException") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static DbUpdateExceptionRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new DbUpdateExceptionRuntimeWrapper(obj, null);
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
		public static bool operator==(DbUpdateExceptionRuntimeWrapper left, DbUpdateExceptionRuntimeWrapper right) {
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
		public static bool operator!=(DbUpdateExceptionRuntimeWrapper left, DbUpdateExceptionRuntimeWrapper right) {
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
	partial class IModelRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IModelRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IModel") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IModel") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IModelRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IModelRuntimeWrapper(obj, null);
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
		public static bool operator==(IModelRuntimeWrapper left, IModelRuntimeWrapper right) {
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
		public static bool operator!=(IModelRuntimeWrapper left, IModelRuntimeWrapper right) {
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
		public IEnumerableRuntimeWrapper<IEntityTypeRuntimeWrapper> GetEntityTypes() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEnumerableRuntimeWrapper<IEntityTypeRuntimeWrapper>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetEntityTypes", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			)
			);
		}
	}
	partial class IEntityTypeRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IEntityTypeRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IEntityType") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IEntityType") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IEntityTypeRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IEntityTypeRuntimeWrapper(obj, null);
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
		public static bool operator==(IEntityTypeRuntimeWrapper left, IEntityTypeRuntimeWrapper right) {
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
		public static bool operator!=(IEntityTypeRuntimeWrapper left, IEntityTypeRuntimeWrapper right) {
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
		public IEntityTypeRuntimeWrapper BaseType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IEntityTypeRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "BaseType").GetValue(Object, null))); }
		}
		public IModelRuntimeWrapper Model {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IModelRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "Model").GetValue(Object, null))); }
		}
		public String Name {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (String)RuntimeTypesHelper.GetProperty(__type, "Name").GetValue(Object, null)); }
		}
		public Type ClrType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Type)RuntimeTypesHelper.GetProperty(__type, "ClrType").GetValue(Object, null)); }
		}
		public IKeyRuntimeWrapper FindPrimaryKey() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IKeyRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "FindPrimaryKey", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			)
			);
		}
		public IEnumerableRuntimeWrapper<IForeignKeyRuntimeWrapper> GetForeignKeys() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEnumerableRuntimeWrapper<IForeignKeyRuntimeWrapper>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetForeignKeys", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			)
			);
		}
		public IEnumerableRuntimeWrapper<IPropertyRuntimeWrapper> GetProperties() {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEnumerableRuntimeWrapper<IPropertyRuntimeWrapper>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetProperties", false)
			let parameters = EmptyArray<string>.Instance
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, EmptyArray<object>.Instance)
			)
			);
		}
		public IEnumerableRuntimeWrapper<INavigationRuntimeWrapper> GetNavigations(IEntityTypeRuntimeWrapper entityType) {
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEnumerableRuntimeWrapper<INavigationRuntimeWrapper>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetNavigations", false)
			let parameters = new string[] { "IEntityType" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(Object, new object[] { 
				((IWrapper)entityType).Object
		 })
			)
			);
		}
	}
	partial class INavigationRuntimeWrapper : IPropertyBaseRuntimeWrapper, IWrapper {
		partial void Init();
		protected INavigationRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "INavigation") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "INavigation") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static INavigationRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new INavigationRuntimeWrapper(obj, null);
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
		public static bool operator==(INavigationRuntimeWrapper left, INavigationRuntimeWrapper right) {
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
		public static bool operator!=(INavigationRuntimeWrapper left, INavigationRuntimeWrapper right) {
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
		public IForeignKeyRuntimeWrapper ForeignKey {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IForeignKeyRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "ForeignKey").GetValue(Object, null))); }
		}
	}
	partial class IForeignKeyRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IForeignKeyRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IForeignKey") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IForeignKey") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IForeignKeyRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IForeignKeyRuntimeWrapper(obj, null);
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
		public static bool operator==(IForeignKeyRuntimeWrapper left, IForeignKeyRuntimeWrapper right) {
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
		public static bool operator!=(IForeignKeyRuntimeWrapper left, IForeignKeyRuntimeWrapper right) {
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
		public IEntityTypeRuntimeWrapper DeclaringEntityType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IEntityTypeRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "DeclaringEntityType").GetValue(Object, null))); }
		}
		public INavigationRuntimeWrapper DependentToPrincipal {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => INavigationRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "DependentToPrincipal").GetValue(Object, null))); }
		}
		public Boolean IsRequired {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "IsRequired").GetValue(Object, null)); }
		}
		public Boolean IsUnique {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "IsUnique").GetValue(Object, null)); }
		}
		public IEntityTypeRuntimeWrapper PrincipalEntityType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IEntityTypeRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "PrincipalEntityType").GetValue(Object, null))); }
		}
		public IKeyRuntimeWrapper PrincipalKey {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IKeyRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "PrincipalKey").GetValue(Object, null))); }
		}
		public INavigationRuntimeWrapper PrincipalToDependent {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => INavigationRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "PrincipalToDependent").GetValue(Object, null))); }
		}
		public IReadOnlyListRuntimeWrapper<IPropertyRuntimeWrapper> Properties {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IReadOnlyListRuntimeWrapper<IPropertyRuntimeWrapper>.Wrap(RuntimeTypesHelper.GetProperty(__type, "Properties").GetValue(Object, null))); }
		}
	}
	partial class IKeyRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IKeyRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IKey") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IKey") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IKeyRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IKeyRuntimeWrapper(obj, null);
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
		public static bool operator==(IKeyRuntimeWrapper left, IKeyRuntimeWrapper right) {
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
		public static bool operator!=(IKeyRuntimeWrapper left, IKeyRuntimeWrapper right) {
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
		public IEntityTypeRuntimeWrapper DeclaringEntityType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IEntityTypeRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "DeclaringEntityType").GetValue(Object, null))); }
		}
		public IReadOnlyListRuntimeWrapper<IPropertyRuntimeWrapper> Properties {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IReadOnlyListRuntimeWrapper<IPropertyRuntimeWrapper>.Wrap(RuntimeTypesHelper.GetProperty(__type, "Properties").GetValue(Object, null))); }
		}
	}
	partial class IPropertyRuntimeWrapper : IPropertyBaseRuntimeWrapper, IWrapper {
		partial void Init();
		protected IPropertyRuntimeWrapper(object obj, Token token) : base(obj, token) { }
		public new static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IProperty") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IProperty") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public new static IPropertyRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IPropertyRuntimeWrapper(obj, null);
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
		public static bool operator==(IPropertyRuntimeWrapper left, IPropertyRuntimeWrapper right) {
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
		public static bool operator!=(IPropertyRuntimeWrapper left, IPropertyRuntimeWrapper right) {
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
		public Type ClrType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Type)RuntimeTypesHelper.GetProperty(__type, "ClrType").GetValue(Object, null)); }
		}
		public Boolean IsNullable {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "IsNullable").GetValue(Object, null)); }
		}
		public Boolean IsReadOnlyAfterSave {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "IsReadOnlyAfterSave").GetValue(Object, null)); }
		}
		public Boolean IsReadOnlyBeforeSave {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "IsReadOnlyBeforeSave").GetValue(Object, null)); }
		}
		public Boolean RequiresValueGenerator {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "RequiresValueGenerator").GetValue(Object, null)); }
		}
		public Boolean IsShadowProperty {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (Boolean)RuntimeTypesHelper.GetProperty(__type, "IsShadowProperty").GetValue(Object, null)); }
		}
	}
	partial class IPropertyBaseRuntimeWrapper : IWrapper {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IPropertyBaseRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IPropertyBase") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IPropertyBase") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IPropertyBaseRuntimeWrapper Wrap(object obj) {
			return obj == null ? null : new IPropertyBaseRuntimeWrapper(obj, null);
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
		public static bool operator==(IPropertyBaseRuntimeWrapper left, IPropertyBaseRuntimeWrapper right) {
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
		public static bool operator!=(IPropertyBaseRuntimeWrapper left, IPropertyBaseRuntimeWrapper right) {
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
		public IEntityTypeRuntimeWrapper DeclaringEntityType {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => IEntityTypeRuntimeWrapper.Wrap(RuntimeTypesHelper.GetProperty(__type, "DeclaringEntityType").GetValue(Object, null))); }
		}
		public String Name {
			get { return RuntimeTypesHelper.UnwrapInvocationException(() => (String)RuntimeTypesHelper.GetProperty(__type, "Name").GetValue(Object, null)); }
		}
	}
	partial class IEnumerableRuntimeWrapper<T> : IWrapper, IEnumerable<T> {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IEnumerableRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IEnumerable`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IEnumerable`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IEnumerableRuntimeWrapper<T> Wrap(object obj) {
			return obj == null ? null : new IEnumerableRuntimeWrapper<T>(obj, null);
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
		public static bool operator==(IEnumerableRuntimeWrapper<T> left, IEnumerableRuntimeWrapper<T> right) {
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
		public static bool operator!=(IEnumerableRuntimeWrapper<T> left, IEnumerableRuntimeWrapper<T> right) {
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
	partial class IReadOnlyListRuntimeWrapper<T> : IWrapper, IEnumerable<T> {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IReadOnlyListRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IReadOnlyList`1") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IReadOnlyList`1") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IReadOnlyListRuntimeWrapper<T> Wrap(object obj) {
			return obj == null ? null : new IReadOnlyListRuntimeWrapper<T>(obj, null);
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
		public static bool operator==(IReadOnlyListRuntimeWrapper<T> left, IReadOnlyListRuntimeWrapper<T> right) {
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
		public static bool operator!=(IReadOnlyListRuntimeWrapper<T> left, IReadOnlyListRuntimeWrapper<T> right) {
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
	partial class IIncludableQueryableRuntimeWrapper<TEntity, TProperty> : IWrapper, IEnumerable<TEntity> {
		partial void Init();
		protected Type __type;
		public object Object { get; protected set; }
		protected class Token { }
		protected IIncludableQueryableRuntimeWrapper(object obj, Token token) {
			var asWrapper = obj as IWrapper;
			if (asWrapper != null)
				obj = asWrapper.Object;
			Object = obj;
			__type = obj.GetType();
		}
		public static bool IsCompatible(Type type) {
			foreach(var i in type.GetInterfaces()) {
				if (i.Name == "IIncludableQueryable`2") {
					return true;
				}
			}
			while (type != null) {
				if (type.Name == "IIncludableQueryable`2") {
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
		public static IIncludableQueryableRuntimeWrapper<TEntity, TProperty> Wrap(object obj) {
			return obj == null ? null : new IIncludableQueryableRuntimeWrapper<TEntity, TProperty>(obj, null);
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
		public static bool operator==(IIncludableQueryableRuntimeWrapper<TEntity, TProperty> left, IIncludableQueryableRuntimeWrapper<TEntity, TProperty> right) {
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
		public static bool operator!=(IIncludableQueryableRuntimeWrapper<TEntity, TProperty> left, IIncludableQueryableRuntimeWrapper<TEntity, TProperty> right) {
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
		class E : IEnumerator<TEntity> {
			IEnumerator e;
			public E(IEnumerator e) { this.e = e; }
			public TEntity Current {
				get { return (TEntity)(typeof(TEntity).GetInterface("IWrapper") != null ? Wrapper.Wrap(typeof(TEntity), e.Current) : e.Current); }
			}
			object IEnumerator.Current { get { return Current; } }
			public void Dispose() { }
			public bool MoveNext() { return e.MoveNext(); }
			public void Reset() { e.Reset(); }
		}
		public IEnumerator<TEntity> GetEnumerator() {
			return new E(((IEnumerable)Object).GetEnumerator());
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
	static partial class EntityTypeExtensionsRuntimeWrapper {
		public static IEnumerableRuntimeWrapper<INavigationRuntimeWrapper> GetNavigations(this IEntityTypeRuntimeWrapper entityType) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Microsoft.EntityFrameworkCore");
			var __type = SafeTypeResolver.GetKnownType(assembly, "Microsoft.EntityFrameworkCore.EntityTypeExtensions");
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEnumerableRuntimeWrapper<INavigationRuntimeWrapper>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "GetNavigations", false)
			let parameters = new string[] { "IEntityType" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(null, new object[] { 
				((IWrapper)entityType).Object
		 })
			)
			);
		}
	}
	static partial class ModelExtensionsRuntimeWrapper {
		public static IEntityTypeRuntimeWrapper FindEntityType(this IModelRuntimeWrapper model, Type type) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Microsoft.EntityFrameworkCore");
			var __type = SafeTypeResolver.GetKnownType(assembly, "Microsoft.EntityFrameworkCore.ModelExtensions");
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IEntityTypeRuntimeWrapper.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "FindEntityType", false)
			let parameters = new string[] { "IModel", "Type" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
		.Invoke(null, new object[] { 
				((IWrapper)model).Object
		, 
			((object)type is IWrapper) ? ((IWrapper)(object)type).Object : type
		 })
			)
			);
		}
	}
	static partial class EntityFrameworkQueryableExtensionsRuntimeWrapper {
		public static IIncludableQueryableRuntimeWrapper<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath) {
			var assembly = RuntimeTypesHelper.LoadAssembly("Microsoft.EntityFrameworkCore");
			var __type = SafeTypeResolver.GetKnownType(assembly, "Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions");
			return RuntimeTypesHelper.UnwrapInvocationException(() =>
				IIncludableQueryableRuntimeWrapper<TEntity, TProperty>.Wrap(
			(from m in RuntimeTypesHelper.GetMethods(__type, "Include", true)
			let parameters = new string[] { "IQueryable`1", "Expression`1" }
			where m.GetParameters().Select(x => x.ParameterType.Name).SequenceEqual(parameters)
			select m).First()
			.MakeGenericMethod(typeof(TEntity), typeof(TProperty))
		.Invoke(null, new object[] { 
			((object)source is IWrapper) ? ((IWrapper)(object)source).Object : source
		, 
			((object)navigationPropertyPath is IWrapper) ? ((IWrapper)(object)navigationPropertyPath).Object : navigationPropertyPath
		 })
			)
			);
		}
	}
	enum EntityStateRuntimeWrapper {
		Detached = 0,
		Unchanged = 1,
		Deleted = 2,
		Modified = 3,
		Added = 4
	}
}
