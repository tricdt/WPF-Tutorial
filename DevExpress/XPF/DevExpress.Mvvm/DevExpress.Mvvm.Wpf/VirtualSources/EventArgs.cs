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

using DevExpress.Xpf.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace DevExpress.Xpf.Data {
	public sealed class FetchRowsResult {
		public static implicit operator FetchRowsResult(object[] rows) {
			return new FetchRowsResult(rows, hasMoreRows: true);
		}
		public object[] Rows { get; private set; }
		public bool HasMoreRows { get; private set; }
		public object NextSkipToken { get; private set; }
		public FetchRowsResult(object[] rows, bool hasMoreRows = true, object nextSkipToken = null) {
			Rows = rows ?? Mvvm.Native.EmptyArray<object>.Instance;
			HasMoreRows = hasMoreRows;
			NextSkipToken = nextSkipToken;
		}
	}
	public sealed class SortDefinition {
		public SortDefinition(string propertyName, ListSortDirection direction) {
			PropertyName = propertyName;
			Direction = direction;
		}
		public string PropertyName { get; private set; }
		public ListSortDirection Direction { get; private set; }
		public override bool Equals(object obj) {
			var definition = obj as SortDefinition;
			return definition != null &&
				   PropertyName == definition.PropertyName &&
				   Direction == definition.Direction;
		}
		public override int GetHashCode() {
			var hashCode = 2132011053;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
			hashCode = hashCode * -1521134295 + Direction.GetHashCode();
			return hashCode;
		}
	}
	public enum SummaryType {
		Sum,
		Avg,
		Min,
		Max,
		Count,
		Custom,
	}
	public class SummaryDefinition {
		public override bool Equals(object obj) {
			var description = obj as SummaryDefinition;
			return description != null &&
				   PropertyName == description.PropertyName &&
				   SummaryType == description.SummaryType &&
				   Equals(Tag, description.Tag);
		}
		public override int GetHashCode() {
			var hashCode = -975136626;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PropertyName);
			hashCode = hashCode * -1521134295 + SummaryType.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Tag);
			return hashCode;
		}
		internal SummaryDefinition() { }
		public SummaryDefinition(string propertyName, SummaryType summaryType, object tag = null) {
			SetSummaryDefinition(summaryType != SummaryType.Count ? propertyName : null, summaryType, tag);
		}
		public string PropertyName { get; private set; }
		public SummaryType SummaryType { get; private set; }
		public object Tag { get; private set; }
		internal void SetSummaryDefinition(string propertyName, SummaryType summaryType, object tag = null) {
			PropertyName = propertyName;
			SummaryType = summaryType;
			Tag = tag;
		}
	}
	public struct ValueAndCount : IEquatable<ValueAndCount> {
		public ValueAndCount(object value, int count) {
			Value = value;
			Count = count;
		}
		public object Value { get; }
		public int Count { get; }
		public override bool Equals(object obj) {
			if(!(obj is ValueAndCount)) {
				return false;
			}
			return Equals((ValueAndCount)obj);
		}
		public bool Equals(ValueAndCount other) {
			return EqualityComparer<object>.Default.Equals(Value, other.Value) &&
				   Count == other.Count;
		}
		public override int GetHashCode() {
			var hashCode = 1852890254;
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(Value);
			hashCode = hashCode * -1521134295 + Count.GetHashCode();
			return hashCode;
		}
		public override string ToString() {
			return $"Value: {Value}, Count: {Count}";
		}
		public static bool operator ==(ValueAndCount left, ValueAndCount right) {
			return left.Equals(right);
		}
		public static bool operator !=(ValueAndCount left, ValueAndCount right) {
			return !(left == right);
		}
	}
}
namespace DevExpress.Mvvm.Xpf {
	public abstract class FetchAsyncArgsBase {
		public FetchAsyncArgsBase(CancellationToken cancellationToken, SortDefinition[] sortOrder, object filter, int skip, object[] keys) {
			CancellationToken = cancellationToken;
			SortOrder = sortOrder;
			Filter = filter;
			Skip = skip;
			Keys = keys;
		}
		public CancellationToken CancellationToken { get; }
		public SortDefinition[] SortOrder { get; }
		public object Filter { get; }
		public int Skip { get; }
		public object[] Keys { get; }
		public Task<FetchRowsResult> Result { get; set; }
	}
	public class FetchRowsAsyncArgs : FetchAsyncArgsBase {
		public FetchRowsAsyncArgs(CancellationToken cancellationToken, SortDefinition[] sortOrder, object filter, int skip, int? take, object[] keys, object skipToken)
			: base(cancellationToken, sortOrder, filter, skip, keys) {
			Take = take;
			AllowRetry = true;
			SkipToken = skipToken;
		}
		public int? Take { get; }
		volatile bool allowRetry;
		public bool AllowRetry {
			get { return allowRetry; }
			set { allowRetry = value; }
		}
		public object SkipToken { get; private set; }
	}
	public class FetchPageAsyncArgs : FetchAsyncArgsBase {
		public FetchPageAsyncArgs(CancellationToken cancellationToken, SortDefinition[] sortOrder, object filter, int skip, int take, object[] keys)
			: base(cancellationToken, sortOrder, filter, skip, keys) {
			Take = take;
		}
		public int Take { get; }
	}
	public class GetSummariesAsyncArgs {
		public GetSummariesAsyncArgs(CancellationToken cancellationToken, SummaryDefinition[] summaries, object filter) {
			CancellationToken = cancellationToken;
			Summaries = summaries;
			Filter = filter;
		}
		public CancellationToken CancellationToken { get; }
		public SummaryDefinition[] Summaries { get; }
		public object Filter { get; }
		public Task<object[]> Result { get; set; }
	}
	public class GetUniqueValuesAsyncArgs {
		public GetUniqueValuesAsyncArgs(CancellationToken cancellationToken, string propertyName, object filter) {
			CancellationToken = cancellationToken;
			PropertyName = propertyName;
			Filter = filter;
		}
		public CancellationToken CancellationToken { get; }
		public string PropertyName { get; }
		public object Filter { get; }
		public Task<object[]> Result { get; set; }
		public Task<ValueAndCount[]> ResultWithCounts { get; set; }
	}
}
