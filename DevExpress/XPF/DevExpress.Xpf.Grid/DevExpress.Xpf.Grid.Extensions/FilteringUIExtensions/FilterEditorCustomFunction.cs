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
using DevExpress.Data.Filtering;
using DevExpress.Data.Helpers;
namespace DevExpress.Xpf.Core.FilteringUI {
	public static class CustomFunctionFactory {
		public static ICustomFunctionOperatorBrowsable Create(string name, Func<bool> evaluate) =>
			new FilterEditorCustomFunction(name, 0, _ => evaluate(), EmptyArray<Type>.Instance);
		public static ICustomFunctionOperatorBrowsable Create<T>(string name, Func<T, bool> evaluate) =>
			new FilterEditorCustomFunction(name, 1, op => evaluate(CorrectOp<T>(op, 0)), new[] { typeof(T) });
		public static ICustomFunctionOperatorBrowsable Create<T1, T2>(string name, Func<T1, T2, bool> evaluate) =>
			new FilterEditorCustomFunction(name, 2, op => evaluate(CorrectOp<T1>(op, 0), CorrectOp<T2>(op, 1)), new[] { typeof(T1), typeof(T2) });
		public static ICustomFunctionOperatorBrowsable Create<T1, T2, T3>(string name, Func<T1, T2, T3, bool> evaluate) =>
			new FilterEditorCustomFunction(name, 3, op => evaluate(CorrectOp<T1>(op, 0), CorrectOp<T2>(op, 1), CorrectOp<T3>(op, 2)), new[] { typeof(T1), typeof(T2), typeof(T3) });
		public static ICustomFunctionOperatorBrowsable Create<T1, T2, T3, T4>(string name, Func<T1, T2, T3, T4, bool> evaluate) =>
			new FilterEditorCustomFunction(name, 4, op => evaluate(CorrectOp<T1>(op, 0), CorrectOp<T2>(op, 1), CorrectOp<T3>(op, 2), CorrectOp<T4>(op, 3)), new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) });
		static T CorrectOp<T>(object[] op, int index) {
			return (T)FilterHelperBase.CorrectFilterValueType(typeof(T), op[index]);
		}
		class FilterEditorCustomFunction : ICustomFunctionOperatorBrowsable {
			readonly string name;
			readonly int operandCount;
			readonly Type[] operandTypes;
			readonly Func<object[], bool> evaluate;
			public FilterEditorCustomFunction(string name, int operandCount, Func<object[], bool> evaluate, Type[] operandTypes) {
				this.name = name;
				this.operandCount = operandCount;
				this.evaluate = evaluate;
				this.operandTypes = operandTypes;
			}
			#region ICustomFunctionOperatorBrowsable
			int ICustomFunctionOperatorBrowsable.MinOperandCount => operandCount;
			int ICustomFunctionOperatorBrowsable.MaxOperandCount => operandCount;
			string ICustomFunctionOperator.Name => name;
			object ICustomFunctionOperator.Evaluate(params object[] operands) {
				return evaluate(operands);
			}
			bool ICustomFunctionOperatorBrowsable.IsValidOperandCount(int count) {
				return count == operandCount;
			}
			bool ICustomFunctionOperatorBrowsable.IsValidOperandType(int operandIndex, int operandCount, Type type) {
				return operandIndex >= 0 && operandIndex < operandTypes.Length && operandTypes[operandIndex].IsAssignableFrom(type);
			}
			Type ICustomFunctionOperator.ResultType(params Type[] operands) {
				return typeof(bool);
			}
			string ICustomFunctionOperatorBrowsable.Description => null;
			FunctionCategory ICustomFunctionOperatorBrowsable.Category => FunctionCategory.Logical;
			#endregion
		}
	}
}
