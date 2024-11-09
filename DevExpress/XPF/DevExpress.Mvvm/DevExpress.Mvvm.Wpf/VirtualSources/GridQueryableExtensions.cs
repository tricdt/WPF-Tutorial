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
using DevExpress.Xpf.Data.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
namespace DevExpress.Xpf.Data {
	public static class GridQueryableExtensions {
		class QueryableToArrayHandler : IQueryableHandler<object[]> {
			object[] IQueryableHandler<object[]>.Handle<T>(IQueryable<T> queryable, Func<object[], object[]> transform) {
				var result = ((IEnumerable<T>)queryable).Cast<object>().ToArray();
				return transform(result);
			}
		}
		#region sort
		public static IQueryable<T> SortBy<T>(this IQueryable<T> source, SortDefinition[] sortOrder, string defaultUniqueSortPropertyName = null) {
			return source.SortBy(sortOrder, SortDefinitionHelper.MakeDefaultUniqueSort(sortOrder, defaultUniqueSortPropertyName));
		}
		public static IQueryable<T> SortBy<T>(this IQueryable<T> source, SortDefinition[] sortOrder, SortDefinition defaultUniqueSort) {
			sortOrder = SortDefinitionHelper.ConcatWithDefaultSortOrder(sortOrder, defaultUniqueSort);
			if(sortOrder.Length == 0)
				return source;
			var ordered = InvokeGenericMethod(source, sortOrder.First(), "OrderBy");
			foreach(var sortDescription in sortOrder.Skip(1)) {
				ordered = InvokeGenericMethod(ordered, sortDescription, "ThenBy");
			}
			return ordered;
		}
		static IOrderedQueryable<T> OrderBy<T, TProperty>(IQueryable<T> query, SortDefinition sortDescription) {
			var member = ExpressionHelper.MakeMemberExpression<T, TProperty>(sortDescription.PropertyName);
			return sortDescription.Direction == ListSortDirection.Ascending 
				? query.OrderBy(member) 
				: query.OrderByDescending(member);
		}
		static IOrderedQueryable<T> ThenBy<T, TProperty>(IOrderedQueryable<T> query, SortDefinition sortDescription) {
			var member = ExpressionHelper.MakeMemberExpression<T, TProperty>(sortDescription.PropertyName);
			return sortDescription.Direction == ListSortDirection.Ascending
				? query.ThenBy(member)
				: query.ThenByDescending(member);
		}
		static IQueryable<T> InvokeGenericMethod<T>(IQueryable<T> source, SortDefinition sortDescription, string methodName) {
			var properties = ExpressionHelper.GetPropertyPath<T>(sortDescription.PropertyName);
			var getDistinctValuesMethod = MakeGenericMethod<T>(typeof(GridQueryableExtensions), properties.Last(), methodName);
			return (IQueryable<T>)getDistinctValuesMethod.Invoke(null, new object[] { source, sortDescription });
		}
		#endregion
		#region distinct
		public static object[] Distinct<T>(this IQueryable<T> queryable, string propertyName, DistinctOptions options = null) {
			return queryable.Distinct(propertyName, new QueryableToArrayHandler(), options);
		}
		public static TResult Distinct<T, TResult>(this IQueryable<T> queryable, string propertyName, IQueryableHandler<TResult> handler, DistinctOptions options = null) {
			var properties = ExpressionHelper.GetPropertyPath<T>(propertyName);
			var getDistinctValuesMethod = MakeGenericMethod<T>(typeof(GridQueryableExtensions), properties.Last(), nameof(GetDistinctQueryResult), new[] { typeof(TResult) });
			return (TResult)getDistinctValuesMethod.Invoke(null, new object[] { queryable, propertyName, handler, options });
		}
		static TResult GetDistinctQueryResult<T, TProperty, TResult>(IQueryable<T> query, string propertyName, IQueryableHandler<TResult> handler, DistinctOptions options) {
			options = options ?? new DistinctOptions();
			var member = ExpressionHelper.MakeMemberExpression<T, TProperty>(propertyName);
			var distinctQuery = query
				.Select(member)
				.Distinct();
			if(ShouldApplyNotNull<TProperty>(options.IncludeNulls)) {
				var parameter = Expression.Parameter(typeof(TProperty), "x");
				var notNullExpression = Expression.Lambda<Func<TProperty, bool>>(Expression.NotEqual(parameter, Expression.Constant(null)), new[] { parameter });
				distinctQuery = distinctQuery.Where(notNullExpression);
			}
			if(ShouldApplyNotEmptyString<TProperty>(options.IncludeEmptyStrings)) {
				var stringQuery = (IQueryable<string>)distinctQuery;
				distinctQuery = (IQueryable<TProperty>)stringQuery.Where(x => x != string.Empty);
			}
			distinctQuery = ApplyOrderAndMaxCount(distinctQuery, options.SortDirection, options.MaxCount, x => x);
			var result = handler.Handle(distinctQuery, values => values);
			return result;
		}
		public static ValueAndCount[] DistinctWithCounts<T>(this IQueryable<T> queryable, string propertyName, DistinctOptions options = null) {
			var properties = ExpressionHelper.GetPropertyPath<T>(propertyName);
			var getDistinctValuesMethod = MakeGenericMethod<T>(typeof(GridQueryableExtensions), properties.Last(), nameof(GetDistinctWithCountsQueryResult), new[] { typeof(ValueAndCount[]) });
			return (ValueAndCount[])getDistinctValuesMethod.Invoke(null, new object[] { queryable, propertyName, options });
		}
		static ValueAndCount[] GetDistinctWithCountsQueryResult<T, TProperty, TResult>(IQueryable<T> query, string propertyName, DistinctOptions options) {
			options = options ?? new DistinctOptions();
			var member = ExpressionHelper.MakeMemberExpression<T, TProperty>(propertyName);
			Action<object> applyNotEqual = value => {
				var memberExpression = (MemberExpression)member.Body;
				var notEqualExpression = Expression.NotEqual(memberExpression, Expression.Constant(value));
				var lambda = Expression.Lambda<Func<T, bool>>(notEqualExpression, member.Parameters.Single());
				query = query.Where(lambda);
			};
			if(ShouldApplyNotNull<TProperty>(options.IncludeNulls)) {
				applyNotEqual(null);
			}
			if(ShouldApplyNotEmptyString<TProperty>(options.IncludeEmptyStrings)) {
				applyNotEqual(string.Empty);
			}
			var distinctQuery = query
				.GroupBy(member)
				.Select(x => new { Value = x.Key, Count = x.Count() });
			distinctQuery = ApplyOrderAndMaxCount(distinctQuery, options.SortDirection, options.MaxCount, x => x.Value);
			var result = distinctQuery
				.ToList()
				.Select(x => new ValueAndCount(x.Value, x.Count))
				.ToArray();
			return result;
		}
		static bool ShouldApplyNotEmptyString<TProperty>(bool includeEmptyStrings) {
			return !includeEmptyStrings && typeof(TProperty) == typeof(string);
		}
		static bool ShouldApplyNotNull<TProperty>(bool includeNulls) {
			return !includeNulls && (Nullable.GetUnderlyingType(typeof(TProperty)) != null || typeof(TProperty).IsClass);
		}
		static IQueryable<T> ApplyOrderAndMaxCount<T, TKey>(IQueryable<T> distinctQuery, ListSortDirection direction, int? maxCount, Expression<Func<T, TKey>> expression) { 
			distinctQuery = direction == ListSortDirection.Ascending
				? distinctQuery.OrderBy(expression)
				: distinctQuery.OrderByDescending(expression);
			if(maxCount != null)
				distinctQuery = distinctQuery.Take(maxCount.Value);
			return distinctQuery;
		}
		#endregion
		#region summaries
		public static object GetSingleSummary<T>(this IQueryable<T> queryable, SummaryDefinition summary, SingleSummaryOptions options = null) {
			options = options ?? new SingleSummaryOptions();
			if(options.UseSortForMinMax && (summary.SummaryType == SummaryType.Min || summary.SummaryType == SummaryType.Max)) {
				var sort = new SortDefinition(summary.PropertyName, summary.SummaryType == SummaryType.Min ? ListSortDirection.Ascending : ListSortDirection.Descending);
				var first = queryable.SortBy(sort.YieldToArray()).FirstOrDefault();
				if(first == null)
					return null;
				var properties = ExpressionHelper.GetPropertyPath<T>(summary.PropertyName);
				object value = first;
				foreach(var property in properties) {
					value = property.GetValue(value);
				}
				return value;
			}
			var source = Expression.Parameter(typeof(IQueryable<T>), "x");
			var aggregate = GetAggregate<T>(typeof(Queryable), summary, source);
			var arguments = queryable.Yield<object>();
			if(aggregate.Arguments.Count == 2) {
				var lambda = (LambdaExpression)((UnaryExpression)aggregate.Arguments.Last()).Operand;
				var expression = typeof(GridQueryableExtensions)
					.GetMethod("MakeLambdaExpression", BindingFlags.Static | BindingFlags.NonPublic)
					.MakeGenericMethod(typeof(T), lambda.ReturnType)
					.Invoke(null, new object[] { lambda });
				arguments = arguments.Concat(expression.Yield());
			}
			var result = aggregate.Method.Invoke(null, arguments.ToArray());
			return result;
		}
		static Expression<Func<T, TProperty>> MakeLambdaExpression<T, TProperty>(LambdaExpression lambda) {
			return Expression.Lambda<Func<T, TProperty>>(lambda.Body, lambda.Parameters);
		}
		public static object[] GetSummaries<T>(this IQueryable<T> queryable, SummaryDefinition[] summaries) {
			return GetSummaries<T, object[]>(queryable, summaries, new QueryableToArrayHandler());
		}
		public static TResult GetSummaries<T, TResult>(this IQueryable<T> queryable, SummaryDefinition[] summaries, IQueryableHandler<TResult> handler) {
			var groupParameter = Expression.Parameter(typeof(IGrouping<int, T>), "x");
			var aggregates = summaries
				.Select(x => {
					return GetAggregate<T>(typeof(Enumerable), x, groupParameter);
				})
				.ToArray();
			var containerType = SummariesContainer.ContainerTypes.Value[aggregates.Length - 1].MakeGenericType(aggregates.Select(x => x.Type).ToArray());
			var assignments = aggregates
				.Select((x, i) => Expression.Bind(containerType.GetProperty("P" + i, BindingFlags.Instance | BindingFlags.Public), x))
				.ToArray();
			var initExpression = Expression.MemberInit(Expression.New(containerType), assignments);
			var result = typeof(GridQueryableExtensions)
				.GetMethod("GetSummariesContainerValue", BindingFlags.Static | BindingFlags.NonPublic)
				.MakeGenericMethod(typeof(T), containerType, typeof(TResult))
				.Invoke(null, new object[] {
					queryable,
					groupParameter,
					initExpression,
					handler,
					summaries.Select(x => x.SummaryType).ToArray(),
				});
			return (TResult)result;
		}
		static MethodCallExpression GetAggregate<T>(Type rootType, SummaryDefinition summary, ParameterExpression source) {
			var lambdaParameter = Expression.Parameter(typeof(T), "o");
			var properties = summary.SummaryType != SummaryType.Count
				? ExpressionHelper.GetPropertyPath<T>(summary.PropertyName)
				: null;
			var propertyLambda = properties.With(p => Expression.Lambda(ExpressionHelper.MakeMemberExpressionBody(p, lambdaParameter), lambdaParameter));
			var aggregate = GetAggregateCall<T>(rootType, source, propertyLambda, summary.SummaryType);
			return aggregate;
		}
		static MethodCallExpression GetAggregateCall<T>(Type rootType, Expression source, LambdaExpression lambda, SummaryType summary) {
			switch(summary) {
			case SummaryType.Sum:
				return Expression.Call(rootType, "Sum", new Type[] { typeof(T) }, source, lambda);
			case SummaryType.Avg:
				return Expression.Call(rootType, "Average", new Type[] { typeof(T) }, source, lambda);
			case SummaryType.Min:
			case SummaryType.Max:
				return Expression.Call(rootType, summary.ToString(), new Type[] { typeof(T), lambda.Body.Type }, source, lambda);
			case SummaryType.Count:
				return Expression.Call(rootType, "Count", new Type[] { typeof(T) }, source);
			default:
				throw new InvalidOperationException();
			}
		}
		static TResult GetSummariesContainerValue<T, TContainer, TResult>(IQueryable<T> queryable, ParameterExpression groupParameter, MemberInitExpression initExpression, IQueryableHandler<TResult> handler, SummaryType[] summaryTypes)
			where TContainer: class {
			var lambda = Expression.Lambda<Func<IGrouping<int, T>, TContainer>>(initExpression, groupParameter);
			var summaries = queryable.GroupBy(x => 0).Select(lambda);
			var result = handler.Handle<TContainer>(summaries, values => {
				if(values.Length == 0) {
					return summaryTypes.Select(x => {
						switch(x) {
						case SummaryType.Sum:
						case SummaryType.Count:
							return (object)0;
						default:
							return null;
						}
					}).ToArray();
				}
				return TypeDescriptor.GetProperties(typeof(TContainer))
					.Cast<PropertyDescriptor>()
					.OrderBy(x => x.Name)
					.Select(x => x.GetValue(values.Single()))
					.ToArray();
			});
			return result;
		}
		#endregion
		static MethodInfo MakeGenericMethod<T>(Type type, PropertyInfo property, string methodName, Type[] additionalTypes = null) {
			Type[] typeArguments = new[] { typeof(T), property.PropertyType };
			if(additionalTypes != null)
				typeArguments = typeArguments.Concat(additionalTypes).ToArray();
			return type
				.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static)
				.MakeGenericMethod(typeArguments);
		}
	}
	public interface IQueryableHandler<TResult> {
		TResult Handle<T>(IQueryable<T> queryable, Func<object[], object[]> transform);
	}
	public class SingleSummaryOptions {
		public SingleSummaryOptions(bool useSortForMinMax = false) {
			UseSortForMinMax = useSortForMinMax;
		}
		public bool UseSortForMinMax { get; private set; }
	}
	public class DistinctOptions {
		public DistinctOptions(ListSortDirection sortDirection = ListSortDirection.Ascending, int? maxCount = null, bool includeNulls = false, bool includeEmptyStrings = false) {
			SortDirection = sortDirection;
			MaxCount = maxCount;
			IncludeNulls = includeNulls;
			IncludeEmptyStrings = includeEmptyStrings;
		}
		public ListSortDirection SortDirection { get; private set; }
		public int? MaxCount { get; private set; }
		public bool IncludeNulls { get; private set; }
		public bool IncludeEmptyStrings { get; private set; }
	}
}
