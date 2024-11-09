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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace DevExpress.Mvvm.Utils {
	public class ExpressionHelper {
		class ValueHolder {
			public readonly object value;
			public ValueHolder(object value) {
				this.value = value;
			}
		}
		static readonly ConcurrentDictionary<Type, object> TraitsCache = new ConcurrentDictionary<Type, object>();
		static Expression GetConstExpression(Type type, object value) {
			return Expression.Convert(Expression.Field(Expression.Constant(new ValueHolder(value)), "value"), type);
		}
		public static bool IsTuple(Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition().Name.StartsWith("Tuple`");
		}
		public static object[] GetKeyPropertyValues(object value) {
			if(value != null && IsTuple(value.GetType())) {
				return value.GetType().GetProperties().Where(p => p.Name.StartsWith("Item")).Select(p => p.GetValue(value, null)).ToArray();
			}
			return new object[] { value };
		}
		public static Expression<Func<TPropertyOwner, bool>> GetKeyEqualsExpression<TGetKeyExpressionOwner, TPropertyOwner, TPrimaryKey>(Expression<Func<TGetKeyExpressionOwner, TPrimaryKey>> getKeyExpression, object key, ParameterExpression parameter = null) {
			if(key == null)
				return k => false;
			var entityParam = parameter ?? Expression.Parameter(typeof(TPropertyOwner));
			var keyProperties = GetKeyProperties(getKeyExpression);
			var keyValues = GetKeyPropertyValues(key);
			if(keyProperties.Count() != keyValues.Count())
				throw new InvalidOperationException();
			var propertyEqualExprs = keyProperties.Zip(keyValues, (p, v) =>
			{
				var constExpr = GetConstExpression(p.PropertyType, v);
				var propertyExpr = Expression.Property(entityParam, typeof(TPropertyOwner).GetProperty(p.Name));
				return Expression.Equal(propertyExpr, constExpr);
			});
			var andExpr = propertyEqualExprs.Aggregate(Expression.And);
			return Expression.Lambda<Func<TPropertyOwner, bool>>(andExpr, entityParam);
		}
		public static EntityTraits<TPropertyOwner, TProperty> GetEntityTraits<TOwner, TPropertyOwner, TProperty>(TOwner owner, Expression<Func<TPropertyOwner, TProperty>> getPropertyExpression) {
			object traits = TraitsCache.GetOrAdd(
				owner.GetType(), 
				_ => new EntityTraits<TPropertyOwner, TProperty>(getPropertyExpression.Compile(), GetSetKeyAction(getPropertyExpression), GetHasKeyFunction(getPropertyExpression)));
			return (EntityTraits<TPropertyOwner, TProperty>)traits;
		}
		public static bool IsFitEntity<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> predicate) where TEntity : class {
			return predicate == null || (new TEntity[] { entity }.AsQueryable().Any(predicate));
		}
		public static TupleType MakeTuple<TupleType>(object[] items) {
			var args = typeof(TupleType).GetGenericArguments();
			if(args.Count() != items.Count())
				throw new Exception();
			var create = typeof(Tuple).GetMethods(BindingFlags.Static | BindingFlags.Public)
				.First(m => m.Name == "Create" && m.GetGenericArguments().Count() == args.Count());
			return (TupleType)create.MakeGenericMethod(args).Invoke(null, items);
		}
		public static Expression<Func<TOwner, TProperty>> GetPropertyExpression<TOwner, TProperty>(string propertyName) {
			var parameter = Expression.Parameter(typeof(TOwner));
			return Expression.Lambda<Func<TOwner, TProperty>>(Expression.Property(parameter, propertyName), parameter);
		}
		public static string GetPropertyName(LambdaExpression expression) {
			Expression body = expression.Body;
			if(body is UnaryExpression) {
				body = ((UnaryExpression)body).Operand;
			}
			var memberExpression = UnpackNullableMemberExpression((MemberExpression)body);
			return memberExpression.Member.Name;
		}
		static MemberExpression UnpackNullableMemberExpression(MemberExpression memberExpression) {
			if(memberExpression != null && IsNullableValueExpression(memberExpression))
				memberExpression = (MemberExpression)memberExpression.Expression;
			return memberExpression;
		}
		static bool IsNullableValueExpression(MemberExpression memberExpression) {
			var propertyInfo = (PropertyInfo)memberExpression.Member;
			return Nullable.GetUnderlyingType(propertyInfo.ReflectedType) != null;
		}
		public static PropertyInfo[] GetKeyProperties<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getPropertyExpression) {
			var memberExpr = UnpackNullableMemberExpression(getPropertyExpression.Body as MemberExpression);
			var methodCallExpr = getPropertyExpression.Body as MethodCallExpression;
			IEnumerable<string> propertyNames;
			if(memberExpr != null) {
				propertyNames = new string[] { memberExpr.Member.Name };
			} else if(methodCallExpr != null) {
				if(methodCallExpr.Method.DeclaringType != typeof(Tuple) || methodCallExpr.Method.Name != "Create") {
					throw new Exception();
				}
				var args = methodCallExpr.Arguments.Cast<MemberExpression>();
				propertyNames = args.Select(a => a.Member.Name);
			} else {
				propertyNames = Enumerable.Empty<string>();
			}
			return propertyNames.Select(p => typeof(TPropertyOwner).GetProperty(p)).ToArray();
		}
		public static Action<TPropertyOwner, TProperty> GetSetKeyAction<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getKeyExpression) {
			return (x, p) => GetSetKeyUntypedAction(getKeyExpression)(x, p);
		}
		public static Action<TPropertyOwner, object> GetSetKeyUntypedAction<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getKeyExpression) {
			var properties = GetKeyProperties(getKeyExpression);
			return (o, val) => {
				var values = GetKeyPropertyValues(val);
				values.Zip(properties, (v, p) => {
					p.SetValue(o, v, null);
					return "";
				}).ToArray();
			};
		}
		static bool IsNullable(Type type) {
			return Nullable.GetUnderlyingType(type) != null;
		}
		static Func<TPropertyOwner, bool> GetHasKeyFunction<TPropertyOwner, TProperty>(Expression<Func<TPropertyOwner, TProperty>> getKeyExpression) {
			var properties = GetKeyProperties(getKeyExpression);
			return o => properties.All(p => !IsNullable(p.PropertyType) || p.GetValue(o, null) != null);
		}
	}
	public class EntityTraits<TEntity, TPrimaryKey> {
		public EntityTraits(Func<TEntity, TPrimaryKey> getPrimaryKeyFunction, Action<TEntity, TPrimaryKey> setPrimaryKeyAction, Func<TEntity, bool> hasPrimaryKeyFunction) {
			this.GetPrimaryKey = getPrimaryKeyFunction;
			this.SetPrimaryKey = setPrimaryKeyAction;
			this.HasPrimaryKey = hasPrimaryKeyFunction;
		}
		public Func<TEntity, TPrimaryKey> GetPrimaryKey { get; private set; }
		public Action<TEntity, TPrimaryKey> SetPrimaryKey { get; private set; }
		public Func<TEntity, bool> HasPrimaryKey { get; private set; }
	}
}
