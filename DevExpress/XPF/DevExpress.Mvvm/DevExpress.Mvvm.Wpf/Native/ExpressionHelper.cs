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
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
#if WINUI
using DevExpress.Mvvm.Native;
#endif
#if MVVM
namespace DevExpress.Mvvm.Native {
#else
namespace DevExpress.Internal {
#endif
#if MVVM
	public
#endif
	static class ExpressionHelper {
		internal static MethodInfo GetArgumentMethodStrict<T>(Expression<Action<T>> expression) {
			return GetArgumentMethodStrictCore(expression);
		}
		internal static MethodInfo GetArgumentFunctionStrict<T, TResult>(Expression<Func<T, TResult>> expression) {
			return GetArgumentMethodStrictCore(expression);
		}
		static MethodInfo GetArgumentMethodStrictCore(LambdaExpression expression) {
			MethodCallExpression memberExpression = GetMethodCallExpression(expression);
			CheckParameterExpression(memberExpression.Object);
			return memberExpression.Method;
		}
		internal static PropertyInfo GetArgumentPropertyStrict<T, TResult>(Expression<Func<T, TResult>> expression) {
			MemberExpression memberExpression = null;
			if(expression.Body is MemberExpression) {
				memberExpression = (MemberExpression)expression.Body;
			}
			else if(expression.Body is UnaryExpression) {
				UnaryExpression uExp = (UnaryExpression)expression.Body;
				if(uExp.NodeType == ExpressionType.Convert)
					memberExpression = (MemberExpression)uExp.Operand;
			}
			if(memberExpression == null)
				throw new ArgumentException(null, nameof(expression));
			CheckParameterExpression(memberExpression.Expression);
			return (PropertyInfo)memberExpression.Member;
		}
		static void CheckParameterExpression(Expression expression) {
			if(expression.NodeType == ExpressionType.Parameter)
				return;
			if(expression.NodeType == ExpressionType.Convert) {
				if(((UnaryExpression)expression).Operand.NodeType == ExpressionType.Parameter)
					return;
			}
			throw new ArgumentException(null, nameof(expression));
		}
		internal static ConstructorInfo GetConstructor<T>(Expression<Func<T>> commandMethodExpression) {
			return GetConstructorCore(commandMethodExpression);
		}
		internal static ConstructorInfo GetConstructorCore(LambdaExpression commandMethodExpression) {
			NewExpression newExpression = commandMethodExpression.Body as NewExpression;
			if(newExpression == null) {
				throw new ArgumentException(null, nameof(commandMethodExpression));
			}
			return newExpression.Constructor;
		}
		public static string GetMethodName(Expression<Action> expression) {
			return GetMethod(expression).Name;
		}
		internal static MethodInfo GetMethod(LambdaExpression expression) {
			MethodCallExpression memberExpression = GetMethodCallExpression(expression);
			return memberExpression.Method;
		}
		private static MethodCallExpression GetMethodCallExpression(LambdaExpression expression) {
			if(expression.Body is InvocationExpression) {
				expression = (LambdaExpression)((InvocationExpression)expression.Body).Expression;
			}
			return (MethodCallExpression)expression.Body;
		}
		public static string GetPropertyName<T>(Expression<Func<T>> expression) {
			return GetPropertyNameCore(expression);
		}
		public static string GetPropertyName<T, TProperty>(Expression<Func<T, TProperty>> expression) {
			return GetPropertyNameCore(expression);
		}
#if !WINUI
		public static PropertyDescriptor GetProperty<T, TProperty>(Expression<Func<T, TProperty>> expression) {
			return TypeDescriptor.GetProperties(typeof(T))[GetPropertyName(expression)];
		}
#endif
		static string GetPropertyNameCore(LambdaExpression expression) {
			MemberExpression memberExpression = GetMemberExpression(expression);
			MemberExpression nextMemberExpression = memberExpression.Expression as MemberExpression;
			if(IsPropertyExpression(nextMemberExpression)) {
				throw new ArgumentException(null, nameof(expression));
			}
			return memberExpression.Member.Name;
		}
		static bool IsPropertyExpression(MemberExpression expression) {
			return expression != null &&
#if !WINUI
				expression.Member.MemberType == MemberTypes.Property;
#else
				expression.Member is PropertyInfo;
#endif
		}
		static MemberExpression GetMemberExpression(LambdaExpression expression) {
			if(expression == null)
				throw new ArgumentNullException(nameof(expression));
			Expression body = expression.Body;
			if(body is UnaryExpression) {
				body = ((UnaryExpression)body).Operand;
			}
			MemberExpression memberExpression = body as MemberExpression;
			if(memberExpression == null) {
				throw new ArgumentException(null, nameof(expression));
			}
			return memberExpression;
		}
		public static bool PropertyHasImplicitImplementation<TInterface, TPropertyType>(TInterface _interface, Expression<Func<TInterface, TPropertyType>> property, bool tryInvoke = true)
			where TInterface : class {
			if(_interface == null)
				throw new ArgumentNullException(nameof(_interface));
			string propertyName = GetArgumentPropertyStrict(property).Name;
			string getMethodName = "get_" + propertyName;
			MethodInfo getMethod = GetGetMethod(_interface, getMethodName);
			if(!getMethod.IsPublic || !string.Equals(getMethod.Name, getMethodName)) return false;
			try {
				if(tryInvoke) {
					getMethod.Invoke(_interface, null);
				}
			}
			catch(Exception e) {
#if !WINUI
				if(e is TargetException) return false;
#endif
				if(e is ArgumentException) return false;
				if(e is TargetParameterCountException) return false;
				if(e is MethodAccessException) return false;
				if(e is InvalidOperationException) return false;
				throw;
			}
			return true;
		}
		static MethodInfo GetGetMethod<TInterface>(TInterface _interface, string getMethodName) {
#if !WINUI
			InterfaceMapping map = _interface.GetType().GetInterfaceMap(typeof(TInterface));
			MethodInfo getMethod = map.TargetMethods[map.InterfaceMethods
				.Select((m, i) => new { name = m.Name, index = i })
				.Where(m => string.Equals(m.name, getMethodName, StringComparison.Ordinal))
				.Select(m => m.index)
				.First()];
#else
			var expliciteGetMethodName = typeof(TInterface).FullName.Replace("+", ".") + "." + getMethodName;
			var expliciteMethod = _interface.GetType().GetRuntimeMethods().FirstOrDefault(x => string.Equals(x.Name, expliciteGetMethodName, StringComparison.Ordinal));
			var method = _interface.GetType().GetRuntimeMethods().FirstOrDefault(x => string.Equals(x.Name, getMethodName, StringComparison.Ordinal));
			MethodInfo getMethod = expliciteMethod != null ? expliciteMethod : method;
#endif
			return getMethod;
		}
	}
}
