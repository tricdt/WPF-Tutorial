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
using System;
using System.ComponentModel;
using System.Linq;
using DevExpress.Xpf.Data.Native;
namespace DevExpress.Xpf.Data {
	public static class SkipTokenHelper {
		public static CriteriaOperator MakeFilterSkipToken<T>(SortDefinition[] sortOrder, string defaultUniqueSortPropertyName, T lastFetchedRow) {
			return MakeFilterSkipToken(sortOrder, SortDefinitionHelper.MakeDefaultUniqueSort(sortOrder, defaultUniqueSortPropertyName), lastFetchedRow);
		}
		public static CriteriaOperator MakeFilterSkipToken<T>(SortDefinition[] sortOrder, SortDefinition defaultUniqueSort, T lastFetchedRow) {
			if(defaultUniqueSort == null && sortOrder.Length == 0)
				throw new InvalidOperationException("At least 'defaultUniqueSort' argument should be specified to make a filter skip token");
			if((object)lastFetchedRow == null)
				return null;
			sortOrder = SortDefinitionHelper.ConcatWithDefaultSortOrder(sortOrder, defaultUniqueSort);
			var allProperties = TypeDescriptor.GetProperties(typeof(T));
			var properties = sortOrder.Select(x => allProperties[x.PropertyName]).ToArray();
			var values = properties.Select(x => x.GetValue(lastFetchedRow)).ToArray();
			if(values.Last() == null)
				throw new InvalidOperationException("Last sort value can't be null");
			var conditions = Enumerable.Range(0, sortOrder.Length)
				.Select(i => {
					var equalConditions = Enumerable.Range(0, i)
						.Select(j => new BinaryOperator(sortOrder[j].PropertyName, values[j], BinaryOperatorType.Equal));
					var orderCondition = MakeOrderComparison(sortOrder[i], values[i], properties[i].PropertyType);
					return CriteriaOperator.And(equalConditions.Concat(orderCondition.YieldIfNotNull()).ToArray());
				});
			return CriteriaOperator.Or(conditions);
		}
		static CriteriaOperator MakeOrderComparison(SortDefinition sort, object value, Type propertyType) {
			bool canBeNull = propertyType.IsClass || Nullable.GetUnderlyingType(propertyType) != null;
			Func<object, BinaryOperatorType, BinaryOperator> binary = (val, type) => new BinaryOperator(sort.PropertyName, val, type);
			if(sort.Direction == ListSortDirection.Ascending) {
				if(value == null)
					return binary(null, BinaryOperatorType.NotEqual);
				return binary(value, BinaryOperatorType.Greater);
			} else {
				if(value == null)
					return null;
				var orderCondition = binary(value, BinaryOperatorType.Less);
				if(canBeNull) {
					return CriteriaOperator.Or(
						orderCondition,
						binary(null, BinaryOperatorType.Equal)
					);
				}
				return orderCondition;
			}
		}
	}
}
