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

using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
namespace DevExpress.Xpf.Data {
	public static class ExpressionHelper {
		public static Expression<Func<T, bool>> Not<T>(Expression<Func<T, bool>> expression) {
			return Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters);
		}
		public static Expression<Func<T, bool>> And<T>(Expression<Func<T, bool>>[] operands) {
			return Merge(operands, Expression.AndAlso);
		}
		public static Expression<Func<T, bool>> Or<T>(Expression<Func<T, bool>>[] operands) {
			return Merge(operands, Expression.OrElse);
		}
		static Expression<T> Merge<T>(Expression<T>[] operands, Func<Expression, Expression, Expression> merge) {
			return operands.Skip(1).Aggregate(operands[0], (op, agg) => op.Merge(agg, merge));
		}
		static Expression<T> Merge<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge) {
			if(first.Parameters.Count != 1 || second.Parameters.Count != 1)
				throw new InvalidOperationException();
			var parameter = first.Parameters.Single();
			var secondBody = new ParameterReplacer(parameter).Visit(second.Body);
			return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
		}
		public static Expression<Func<T, TResult>> Substitute<T, TProperty, TResult>(Expression<Func<TProperty, TResult>> expression, PropertyInfo[] properties) {
			expression = (Expression<Func<TProperty, TResult>>)new Clojurer().Visit(expression);
			var member = MakeMemberExpression<T, TProperty>(properties);
			var result = SubstituteParameter(expression, member);
			return result;
		}
		public static PropertyInfo[] GetPropertyPath<T>(string propertyPath) {
			var path = propertyPath.Split('.');
			var type = typeof(T);
			return path.Select(propertyName => {
				var property = GetProperty(type, propertyName);
				type = property.PropertyType;
				return property;
			}).ToArray();
		}
		public static PropertyInfo GetProperty(Type type, string propertyName) {
			var property = GetPropertyCore(type, propertyName);
			return property.DeclaringType == property.ReflectedType
				? property
				: GetPropertyCore(property.DeclaringType, propertyName);
		}
		static PropertyInfo GetPropertyCore(Type type, string propertyName) {
			return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
		}
		static Expression<Func<T2, TResult>> SubstituteParameter<T1, T2, TResult>(this Expression<Func<T1, TResult>> expression, Expression<Func<T2, T1>> substitution) {
			var substituted = new ParameterReplacer(substitution.Body).Visit(expression.Body);
			return Expression.Lambda<Func<T2, TResult>>(substituted, substitution.Parameters);
		}
		public static Expression<Func<T, TResult>> MakeMemberExpression<T, TResult>(string propertyName) {
			return MakeMemberExpression<T, TResult>(GetPropertyPath<T>(propertyName));
		}
		public static Expression<Func<T, TResult>> MakeMemberExpression<T, TResult>(PropertyInfo[] properties) {
			var parameter = Expression.Parameter(typeof(T), "x");
			var body = MakeMemberExpressionBody(properties, parameter);
			if(typeof(TResult) != properties.Last().PropertyType) {
				body = Expression.Convert(body, typeof(TResult));
			}
			return Expression.Lambda<Func<T, TResult>>(body, new ParameterExpression[] { parameter });
		}
		public static Expression MakeMemberExpressionBody(PropertyInfo[] properties, ParameterExpression parameter) {
			Expression body = parameter;
			foreach(var property in properties) {
				body = Expression.Property(body, property);
			}
			return body;
		}
		class Clojurer : ExpressionVisitor {
			protected override Expression VisitMember(MemberExpression node) {
				if(node.Member is FieldInfo && node.Expression is ConstantExpression) {
					var value = ((ConstantExpression)node.Expression).Value;
					if(value != null && value.GetType().GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length > 0) {
						var field = ((FieldInfo)node.Member);
						var result = field.GetValue(value);
						return Expression.Constant(result, field.FieldType);
					}
				}
				return base.VisitMember(node);
			}
		}
		class ParameterReplacer : ExpressionVisitor {
			readonly Expression replacement;
			public ParameterReplacer(Expression replacement) {
				this.replacement = replacement;
			}
			protected override Expression VisitParameter(ParameterExpression p) {
				return replacement;
			}
		}
	}
}
