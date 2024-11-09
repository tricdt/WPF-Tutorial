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
using DevExpress.Data.Filtering.Helpers;
using System;
using System.Linq;
namespace DevExpress.Xpf.Data {
	public delegate T BinaryOperatorMatcher<T>(string propertyName, object value, BinaryOperatorType operatorType);
	public delegate T FunctionOperatorMatcher<T>(string propertyName, object[] values, FunctionOperatorType operatorType);
	public delegate T InOperatorMatcher<T>(string propertyName, object[] values);
	public delegate T GroupOperatorMatcher<T>(T[] operands);
	public delegate T NotOperatorMatcher<T>(T operand);
	public static class FilterCriteriaMatchHelper {
		class MatchVisitor<T> : ICriteriaVisitor<T> {
			readonly BinaryOperatorMatcher<T> binary;
			readonly InOperatorMatcher<T> @in;
			readonly FunctionOperatorMatcher<T> function;
			readonly GroupOperatorMatcher<T> and;
			readonly GroupOperatorMatcher<T> or;
			readonly NotOperatorMatcher<T> not;
			readonly TodayHelper today;
			public MatchVisitor(
				BinaryOperatorMatcher<T> binary,
				InOperatorMatcher<T> @in,
				FunctionOperatorMatcher<T> function,
				GroupOperatorMatcher<T> and,
				GroupOperatorMatcher<T> or,
				NotOperatorMatcher<T> not,
				TodayHelper today) {
				this.binary = binary;
				this.@in = @in;
				this.function = function;
				this.and = and;
				this.or = or;
				this.not = not;
				this.today = today;
			}
			T ICriteriaVisitor<T>.Visit(BinaryOperator theOperator) {
				if(binary == null)
					throw new CriteriaHandlerNotProvidedException("Binary operator handler not provided");
				if(!(theOperator.LeftOperand is OperandProperty) && (theOperator.RightOperand is OperandProperty)) {
					var invertedType = Invert(theOperator.OperatorType);
					if(invertedType != null) {
						theOperator = new BinaryOperator(theOperator.RightOperand, theOperator.LeftOperand, invertedType.Value);
					}
				}
				return binary(
					theOperator.LeftOperand.ToPropertyName(), 
					ToValue(theOperator.RightOperand), 
					theOperator.OperatorType
				);
			}
			static BinaryOperatorType? Invert(BinaryOperatorType type) {
				switch(type) {
				case BinaryOperatorType.Equal:
				case BinaryOperatorType.NotEqual:
					return type;
				case BinaryOperatorType.Greater:
					return BinaryOperatorType.Less;
				case BinaryOperatorType.Less:
					return BinaryOperatorType.Greater;
				case BinaryOperatorType.LessOrEqual:
					return BinaryOperatorType.GreaterOrEqual;
				case BinaryOperatorType.GreaterOrEqual:
					return BinaryOperatorType.LessOrEqual;
				}
				return null;
			}
			T ICriteriaVisitor<T>.Visit(UnaryOperator theOperator) {
				if(theOperator.OperatorType == UnaryOperatorType.Not) {
					if((theOperator.Operand is OperandProperty)) {
						return VisitBinaryEquals(theOperator.Operand, false);
					} else {
						if(not == null)
							throw new CriteriaHandlerNotProvidedException("Not operator handler not provided");
						return not(theOperator.Operand.Accept(this));
					}
				}
				if(theOperator.OperatorType == UnaryOperatorType.IsNull) {
					return VisitBinaryEquals(theOperator.Operand, null);
				}
				throw new InvalidOperationException("UnaryOperator operator not expected");
			}
			T VisitBinaryEquals(CriteriaOperator operand, object value) {
				return ((ICriteriaVisitor<T>)this).Visit(new BinaryOperator(operand, new OperandValue(value), BinaryOperatorType.Equal));
			}
			T ICriteriaVisitor<T>.Visit(InOperator theOperator) {
				if(@in == null)
					throw new CriteriaHandlerNotProvidedException("In operator handler not provided");
				return @in(
					theOperator.LeftOperand.ToPropertyName(),
					theOperator.Operands.Select(ToValue).ToArray()
				);
			}
			T ICriteriaVisitor<T>.Visit(GroupOperator theOperator) {
				Func<T[]> getValues = () => theOperator.Operands.Select(x => x.Accept(this)).ToArray();
				if(theOperator.OperatorType == GroupOperatorType.And) {
					if(and == null)
						throw new CriteriaHandlerNotProvidedException("Group \"and\" operator handler not provided");
					return and(getValues());
				} else {
					if(or == null)
						throw new CriteriaHandlerNotProvidedException("Group \"or\" operator handler not provided");
					return or(getValues());
				}
			}
			T ICriteriaVisitor<T>.Visit(BetweenOperator theOperator) {
				var rangeOperator = new GroupOperator(
					GroupOperatorType.And,
					new BinaryOperator(theOperator.TestExpression, theOperator.BeginExpression, BinaryOperatorType.GreaterOrEqual),
					new BinaryOperator(theOperator.TestExpression, theOperator.EndExpression, BinaryOperatorType.LessOrEqual)
					);
				return ((ICriteriaVisitor<T>)this).Visit(rangeOperator);
			}
			T ICriteriaVisitor<T>.Visit(OperandValue theOperand) {
				throw new InvalidOperationException("OperandValue operator not expected");
			}
			T ICriteriaVisitor<T>.Visit(FunctionOperator theOperator) {
				if(function == null)
					throw new CriteriaHandlerNotProvidedException("Function operator handler not provided");
				if(theOperator.OperatorType == FunctionOperatorType.Custom) {
					var values = theOperator.Operands.Take(1).Concat(theOperator.Operands.Skip(2));
					return function(
						theOperator.Operands[1].ToPropertyName(),
						values.Select(ToValue).ToArray(),
						theOperator.OperatorType
					);
				}
				return function(
					theOperator.Operands[0].ToPropertyName(),
					theOperator.Operands.Skip(1).Select(ToValue).ToArray(),
					theOperator.OperatorType
				);
			}
			object ToValue(CriteriaOperator operandValue) {
				if(operandValue is FunctionOperator) {
					var localDateTime = TryConvertLocalDateTimeFunction(((FunctionOperator)operandValue).OperatorType);
					if(localDateTime != null)
						operandValue = new OperandValue(localDateTime.Value);
				}
				if(!(operandValue is OperandValue))
					throw new OperandValueExpectedException(operandValue);
				return ((OperandValue)operandValue).Value;
			}
			DateTime? TryConvertLocalDateTimeFunction(FunctionOperatorType type) {
				switch(type) {
				case FunctionOperatorType.LocalDateTimeThisYear:
					return today.YearStart;
				case FunctionOperatorType.LocalDateTimeThisMonth:
					return today.MonthStart;
				case FunctionOperatorType.LocalDateTimeLastWeek:
					return today.PrevWeekStart;
				case FunctionOperatorType.LocalDateTimeThisWeek:
					return today.WeekStart;
				case FunctionOperatorType.LocalDateTimeYesterday:
					return today.Yesterday;
				case FunctionOperatorType.LocalDateTimeToday:
					return today.Today;
				case FunctionOperatorType.LocalDateTimeTomorrow:
					return today.Tomorrow;
				case FunctionOperatorType.LocalDateTimeDayAfterTomorrow:
					return today.AfterTomorrow;
				case FunctionOperatorType.LocalDateTimeNextWeek:
					return today.NextWeekStart;
				case FunctionOperatorType.LocalDateTimeTwoWeeksAway:
					return today.NextNextWeekStart;
				case FunctionOperatorType.LocalDateTimeNextMonth:
					return today.NextMonthStart;
				case FunctionOperatorType.LocalDateTimeNextYear:
					return today.NextYearStart;
				case FunctionOperatorType.LocalDateTimeTwoMonthsAway:
					return today.NextNextMonthStart;
				case FunctionOperatorType.LocalDateTimeLastMonth:
					return today.PrevMonthStart;
				case FunctionOperatorType.LocalDateTimeLastYear:
					return today.PrevYearStart;
				case FunctionOperatorType.LocalDateTimeYearBeforeToday:
					return today.YearBeforeToday;
				case FunctionOperatorType.LocalDateTimeNow:
					return today.Now;
				default:
					return null;
				}
			}
		}
		public static T Match<T>(this CriteriaOperator criteria,
			BinaryOperatorMatcher<T> binary = null,
			InOperatorMatcher<T> @in = null,
			FunctionOperatorMatcher<T> function = null,
			GroupOperatorMatcher<T> and = null,
			GroupOperatorMatcher<T> or = null,
			NotOperatorMatcher<T> not = null,
			T @null = default(T),
			DateTime? now = null, 
			DayOfWeek? firstDayOfWeek = null
			) {
			if(Equals(criteria, null))
				return @null;
			var visitor = new MatchVisitor<T>(
				binary: binary,
				@in: @in,
				function: function,
				and: and,
				or: or,
				not: not,
				today: new TodayHelper(now, firstDayOfWeek));
			return criteria.Accept(visitor);
		}
		static string ToPropertyName(this CriteriaOperator operandProperty) {
			if(!(operandProperty is OperandProperty))
				throw new OperandPropertyExpectedException(operandProperty);
			return ((OperandProperty)operandProperty).PropertyName;
		}
	}
	public class CriteriaHandlerNotProvidedException : Exception {
		public CriteriaHandlerNotProvidedException(string message) : base(message) {
		}
	}
	public class OperandPropertyExpectedException : Exception {
		public OperandPropertyExpectedException(CriteriaOperator theOperator) : base("OperandProperty expected, but was: " + theOperator.ToString()) {
		}
	}
	public class OperandValueExpectedException : Exception {
		public OperandValueExpectedException(CriteriaOperator theOperator) : base("OperandValue expected, but was: " + theOperator.ToString()) {
		}
	}
}
