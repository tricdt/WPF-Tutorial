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

using DevExpress.Data.Filtering;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Data.Native;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
namespace DevExpress.Xpf.Data {
	public sealed class GridFilterCriteriaToExpressionConverter<T> {
		readonly TodayHelper today;
		public GridFilterCriteriaToExpressionConverter(DateTime? today = null, DayOfWeek? firstDayOfWeek = null) {
			this.today = new TodayHelper(today, firstDayOfWeek);
			RegisterDefaultFactories();
		}
		#region factories
		readonly Dictionary<Tuple<BinaryOperatorType, Type>, Delegate> BinaryFactories
			= new Dictionary<Tuple<BinaryOperatorType, Type>, Delegate>();
		readonly Dictionary<Tuple<FunctionOperatorType, Type>, Delegate> FunctionFactories
			= new Dictionary<Tuple<FunctionOperatorType, Type>, Delegate>();
		void RegisterDefaultFactories() {
			RegisterBinaryExpressionFactory<string>(BinaryOperatorType.Greater, val => x => string.Compare(x, val) > 0);
			RegisterBinaryExpressionFactory<string>(BinaryOperatorType.GreaterOrEqual, val => x => string.Compare(x, val) >= 0);
			RegisterBinaryExpressionFactory<string>(BinaryOperatorType.Less, val => x => string.Compare(x, val) < 0);
			RegisterBinaryExpressionFactory<string>(BinaryOperatorType.LessOrEqual, val => x => string.Compare(x, val) <= 0);
			RegisterFunctionExpressionFactory<string>(FunctionOperatorType.Contains, val => x => x.Contains(val));
			RegisterFunctionExpressionFactory<string>(FunctionOperatorType.StartsWith, val => x => x.StartsWith(val));
			RegisterFunctionExpressionFactory<string>(FunctionOperatorType.EndsWith, val => x => x.EndsWith(val));
			RegisterFunctionExpressionFactory<string>(FunctionOperatorType.IsNullOrEmpty, () => x => (x == null || x.Length == 0));
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalBeyondThisYear, today.NextYearStart, null);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalLaterThisYear, today.NextMonthStart, today.NextYearStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalLaterThisMonth, today.NextNextWeekStart, today.NextMonthStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalNextWeek, today.NextWeekStart, today.NextNextWeekStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalLaterThisWeek, today.AfterTomorrow, today.NextWeekStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalTomorrow, today.Tomorrow, today.AfterTomorrow);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalToday, today.Today, today.Tomorrow);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalYesterday, today.Yesterday, today.Today);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalEarlierThisWeek, today.WeekStart, today.Yesterday);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalLastWeek, today.PrevWeekStart, today.WeekStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalEarlierThisMonth, today.MonthStart, today.PrevWeekStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalEarlierThisYear, today.YearStart, today.MonthStart);
			RegisterOutlookIntervalExpressionFactory(
				FunctionOperatorType.IsOutlookIntervalPriorThisYear, null, today.YearStart);
		}
		#region DateTime
		void RegisterOutlookIntervalExpressionFactory(FunctionOperatorType type, DateTime? from, DateTime? to) {
			RegisterFunctionExpressionFactory<DateTime>(type, () => GetDateRangeExpression<DateTime>(from, to));
			RegisterFunctionExpressionFactory<DateTime?>(type, () => GetDateRangeExpression<DateTime?>(from, to));
		}
		Expression<Func<TResult, bool>> GetDateRangeExpression<TResult>(DateTime? from, DateTime? to) {
			Expression fromBody = null;
			Expression toBody = null;
			var parameter = Expression.Parameter(typeof(TResult), "x");
			if(from != null) {
				var fromValue = (TResult)(object)from;
				fromBody = Expression.GreaterThanOrEqual(parameter, Expression.Constant(fromValue, typeof(TResult)));
			}
			if(to != null) {
				var toValue = (TResult)(object)to;
				toBody = Expression.LessThan(parameter, Expression.Constant(toValue, typeof(TResult)));
			}
			if(fromBody != null && toBody != null) {
				fromBody = Expression.AndAlso(fromBody, toBody);
			}
			return Expression.Lambda<Func<TResult, bool>>(fromBody ?? toBody, new[] { parameter });
		}
		#endregion
		public void RegisterBinaryExpressionFactory<TValue>(BinaryOperatorType operatorType, Func<TValue, Expression<Func<TValue, bool>>> factory) {
			RegisterFactoryCore<BinaryOperatorType, TValue>(BinaryFactories, operatorType, factory);
		}
		Func<TValue, Expression<Func<TValue, bool>>> GetBinaryFactory<TValue>(BinaryOperatorType type) {
			return (Func<TValue, Expression<Func<TValue, bool>>>)DictionaryExtensions.GetValueOrDefault(BinaryFactories, Tuple.Create(type, typeof(TValue)));
		}
		public void RegisterFunctionExpressionFactory<TValue>(FunctionOperatorType operatorType, Func<TValue, Expression<Func<TValue, bool>>> factory) {
			RegisterFactoryCore<FunctionOperatorType, TValue>(FunctionFactories, operatorType, factory);
		}
		public void RegisterFunctionExpressionFactory<TValue>(FunctionOperatorType operatorType, Func<Expression<Func<TValue, bool>>> factory) {
			RegisterFactoryCore<FunctionOperatorType, TValue>(FunctionFactories, operatorType, factory);
		}
		Delegate GetFunctionFactory(FunctionOperatorType functionType, Type propertyType) {
			return DictionaryExtensions.GetValueOrDefault(FunctionFactories, Tuple.Create(functionType, propertyType));
		}
		static void RegisterFactoryCore<TType, TValue>(Dictionary<Tuple<TType, Type>, Delegate> dictionary, TType type, Delegate factory) {
			dictionary[Tuple.Create(type, typeof(TValue))] = factory;
		}
		#endregion
		#region convert
		public Expression<Func<T, bool>> Convert(CriteriaOperator filter) {
			return filter.Match<Expression<Func<T, bool>>>(
				binary: ConvertBinary,
				function: ConvertFunction,
				@in: ConvertIn,
				and: ConvertAnd,
				or: ConvertOr,
				not: ConvertNot,
				@null: ConvertNull,
				now: today.Today,
				firstDayOfWeek: today.FirstDayOfWeek
			);
		}
		public Expression<Func<T, bool>> ConvertBinary(string propertyName, object value, BinaryOperatorType type) {
			var properties = ExpressionHelper.GetPropertyPath<T>(propertyName);
			var propertyType = properties.Last().PropertyType;
			var valueType = value.With(x => x.GetType()) ?? propertyType;
			var commonType = CastHelper.GetCast(valueType, propertyType);
			return MakeMethodAndInvoke("MakeBinaryExpression", properties, new[] { value, type }, commonType);
		}
		public Expression<Func<T, bool>> ConvertFunction(string propertyName, object[] values, FunctionOperatorType type) {
			var properties = ExpressionHelper.GetPropertyPath<T>(propertyName);
			if(values.Length == 1) {
				return MakeMethodAndInvoke(nameof(MakeFunction2Expression), properties, new[] { values.Single(), type });
			}
			if(values.Length == 0) {
				var propertyType = properties.Last().PropertyType;
				if(GetFunctionFactory(type, propertyType) == null) {
					var underlyingStructType = Nullable.GetUnderlyingType(propertyType);
					if((type == FunctionOperatorType.IsNullOrEmpty || type == FunctionOperatorType.IsNull) && underlyingStructType != null) {
						return MakeMethodAndInvoke(nameof(MakeIsNullExpression), properties, EmptyArray<object>.Instance, propertyType: underlyingStructType);
					}
				}
				return MakeMethodAndInvoke(nameof(MakeFunction1Expression), properties, new object[] { type });
			}
			throw new InvalidOperationException(string.Format("Not supported"));
		}
		public Expression<Func<T, bool>> ConvertIn(string propertyName, object[] values) {
			var properties = ExpressionHelper.GetPropertyPath<T>(propertyName);
			return MakeMethodAndInvoke("MakeInExpression", properties, new[] { values });
		}
		public Expression<Func<T, bool>> ConvertAnd(Expression<Func<T, bool>>[] expressions) {
			return ExpressionHelper.And(expressions);
		}
		public Expression<Func<T, bool>> ConvertOr(Expression<Func<T, bool>>[] expressions) {
			return ExpressionHelper.Or(expressions);
		}
		public Expression<Func<T, bool>> ConvertNot(Expression<Func<T, bool>> expression) {
			return ExpressionHelper.Not(expression);
		}
		public Expression<Func<T, bool>> ConvertNull { get { return x => true; } }
		#endregion
		Expression<Func<T, bool>> MakeMethodAndInvoke(string methodName, PropertyInfo[] properties, object[] additionalParams, Type propertyType = null) {
			var makeExpressionMethod = typeof(GridFilterCriteriaToExpressionConverter<T>)
				.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
				.MakeGenericMethod(new[] { propertyType ?? properties.Last().PropertyType });
			return (Expression<Func<T, bool>>)makeExpressionMethod.Invoke(this, properties.Yield().Concat(additionalParams).ToArray());
		}
		Expression<Func<T, bool>> MakeBinaryExpression<TProperty>(PropertyInfo[] properties, object val, BinaryOperatorType type) {
			var propertyType = properties.Last().PropertyType;
			if(val == null && !propertyType.IsClass && Nullable.GetUnderlyingType(propertyType) == null) {
				if(type == BinaryOperatorType.Equal)
					return x => false;
				if(type == BinaryOperatorType.NotEqual)
					return x => true;
			}
			var typedVal = GetTypedVal<TProperty>(val);
			var factory = GetBinaryFactory<TProperty>(type);
			if(factory != null) {
				return ExpressionHelper.Substitute<T, TProperty, bool>(factory(typedVal), properties);
			}
			var createBodyMethod = BinaryOperatorTypeToCreateBodyMethod(Nullable.GetUnderlyingType(typeof(TProperty)) ?? typeof(TProperty), type);
			if(createBodyMethod != null) {
				return MakeBinaryMemberExpression(properties, typedVal, createBodyMethod);
			}
			throw new InvalidOperationException("Not supported");
		}
		Expression<Func<T, bool>> MakeFunction2Expression<TProperty>(PropertyInfo[] properties, object val, FunctionOperatorType type) {
			var factory = (Func<TProperty, Expression<Func<TProperty, bool>>>)GetFunctionFactory(type, typeof(TProperty));
			if(factory == null)
				throw new InvalidOperationException("Not supported");
			return ExpressionHelper.Substitute<T, TProperty, bool>(factory((TProperty)val), properties);
		}
		Expression<Func<T, bool>> MakeFunction1Expression<TProperty>(PropertyInfo[] properties, FunctionOperatorType type) {
			var factory = (Func<Expression<Func<TProperty, bool>>>)GetFunctionFactory(type, typeof(TProperty));
			if(factory == null)
				throw new InvalidOperationException("Not supported");
			return ExpressionHelper.Substitute<T, TProperty, bool>(factory(), properties);
		}
		Expression<Func<T, bool>> MakeIsNullExpression<TProperty>(PropertyInfo[] properties) where TProperty : struct {
			Expression<Func<TProperty?, bool>> expression = x => x == null;
			return ExpressionHelper.Substitute<T, TProperty?, bool>(expression, properties);
		}
		Expression<Func<T, bool>> MakeInExpression<TProperty>(PropertyInfo[] properties, object[] vals) {
			var typedVals = vals.Select(x => GetTypedVal<TProperty>(x)).ToArray();
			Func<TProperty[], Expression<Func<TProperty, bool>>> factory = values => x => values.Contains(x);
			return ExpressionHelper.Substitute<T, TProperty, bool>(factory(typedVals), properties);
		}
		static Expression<Func<T, bool>> MakeBinaryMemberExpression<TProperty>(PropertyInfo[] properties, TProperty typedVal, Func<Expression, Expression, BinaryExpression> createBodyMethod) {
			var member = ExpressionHelper.MakeMemberExpression<T, TProperty>(properties);
			var body = createBodyMethod(member.Body, Expression.Constant(typedVal, typeof(TProperty)));
			return Expression.Lambda<Func<T, bool>>(body, member.Parameters.ToArray());
		}
		static TProperty GetTypedVal<TProperty>(object val) {
			return (TProperty)((val is TProperty) 
				? val 
				: (val != null 
					? System.Convert.ChangeType(val, typeof(TProperty))
					: default(TProperty)));
		}
		static Func<Expression, Expression, BinaryExpression> BinaryOperatorTypeToCreateBodyMethod(Type propertyType, BinaryOperatorType type) {
			var createExpression = ComparisonBinaryOperatorTypeToCreateExpressionMethod(type);
			if(createExpression != null) {
				if(IsBuiltComparableType(propertyType)) {
					return createExpression;
				}
				var compareToMethod = GetCompareToMethod(propertyType);
				if(compareToMethod != null) {
					return (left, right) => {
						var compareCall = Expression.Call(left, compareToMethod, right);
						return createExpression(compareCall, Expression.Constant(0));
					};
				}
			}
			switch(type) {
			case BinaryOperatorType.Equal:
				return Expression.Equal;
			case BinaryOperatorType.NotEqual:
				return Expression.NotEqual;
			}
			return null;
		}
		static Func<Expression, Expression, BinaryExpression> ComparisonBinaryOperatorTypeToCreateExpressionMethod(BinaryOperatorType type) {
			switch(type) {
			case BinaryOperatorType.Greater:
				return Expression.GreaterThan;
			case BinaryOperatorType.GreaterOrEqual:
				return Expression.GreaterThanOrEqual;
			case BinaryOperatorType.Less:
				return Expression.LessThan;
			case BinaryOperatorType.LessOrEqual:
				return Expression.LessThanOrEqual;
			}
			return null;
		}
		static bool IsBuiltComparableType(Type type) {
			switch(Type.GetTypeCode(type)) {
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
				return true;
			}
			return false;
		}
		static MethodInfo GetCompareToMethod(Type type) {
			var comparableType = typeof(IComparable<>).MakeGenericType(type);
			if(!comparableType.IsAssignableFrom(type))
				return null;
			var compareToMethod = type.GetMethod("CompareTo", BindingFlags.Public | BindingFlags.Instance, null, new[] { type }, null);
			return compareToMethod;
		}
	}
	class TodayHelper {
		public TodayHelper(DateTime? now = null, DayOfWeek? firstDayOfWeek = null) {
			Now = now ?? DateTime.Now;
			FirstDayOfWeek = firstDayOfWeek ?? CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
		}
		public DateTime Now { get; private set; }
		public DateTime Today => Now.Date;
		public DateTime YearBeforeToday => Today.AddYears(-1);
		public DateTime Yesterday => Today.AddDays(-1);
		public DateTime Tomorrow => Today.AddDays(1);
		public DateTime AfterTomorrow => Today.AddDays(2);
		public DayOfWeek FirstDayOfWeek { get; private set; }
		public DateTime WeekStart {
			get {
				var date = Today;
				while(date.DayOfWeek != FirstDayOfWeek) {
					date = date.AddDays(-1);
				}
				return date;
			}
		}
		public DateTime PrevWeekStart => WeekStart.AddDays(-7);
		public DateTime NextWeekStart => WeekStart.AddDays(7);
		public DateTime NextNextWeekStart => WeekStart.AddDays(14);
		public DateTime YearStart => new DateTime(Today.Year, 1, 1);
		public DateTime NextYearStart => YearStart.AddYears(1);
		public DateTime PrevYearStart => YearStart.AddYears(-1);
		public DateTime MonthStart => new DateTime(Today.Year, Today.Month, 1);
		public DateTime NextMonthStart => MonthStart.AddMonths(1);
		public DateTime PrevMonthStart => MonthStart.AddMonths(-1);
		public DateTime NextNextMonthStart => MonthStart.AddMonths(2);
	}
	public class ExpressionFilterConverter<T> : MarkupExtension, IValueConverter {
		object IValueConverter.Convert(object filter, Type targetType, object parameter, CultureInfo culture) {
			if(filter != null && !(filter is CriteriaOperator))
				throw new InvalidOperationException();
			var converter = new GridFilterCriteriaToExpressionConverter<T>();
			SetUpConverter(converter);
			return converter.Convert((CriteriaOperator)filter);
		}
		protected virtual void SetUpConverter(GridFilterCriteriaToExpressionConverter<T> converter) {
		}
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
		public override object ProvideValue(IServiceProvider serviceProvider) => this;
	}
}
