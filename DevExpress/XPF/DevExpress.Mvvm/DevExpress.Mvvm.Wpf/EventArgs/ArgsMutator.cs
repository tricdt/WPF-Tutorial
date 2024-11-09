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
namespace DevExpress.Mvvm.Xpf.Native {
	public static class ArgsMutator {
		public static ColumnDisplayTextArgs CreateColumnDisplayTextArgs(Func<int, object> getItemByRowHandle, Func<int, int> getSourceIndexByRowHandle, Func<int, int> getRowHandleBySourceIndex) {
			return new ColumnDisplayTextArgs(getItemByRowHandle, getSourceIndexByRowHandle, getRowHandleBySourceIndex);
		}
		public static void SetColumnDisplayTextArgs(ColumnDisplayTextArgs args, string fieldName, int? rowHandle, int? sourceIndex, object value, string displayText, bool showAsNullText) {
			args.SetColumnDisplayTextArgs(fieldName, rowHandle, sourceIndex, value, displayText, showAsNullText);
		}
		public static void ClearColumnDisplayTextArgs(ColumnDisplayTextArgs args) {
			args.SetColumnDisplayTextArgs(null, null, -1, null, args.DisplayText, args.ShowAsNullText);
		}
		public static NodeDisplayTextArgs CreateNodeDisplayTextArgs(Func<object, IEnumerable<object>> getPath) {
			return new NodeDisplayTextArgs(getPath);
		}
		public static void SetNodeDisplayTextArgs(NodeDisplayTextArgs args, object node, string fieldName, object item, object value, string displayText, bool showAsNullText) {
			args.SetNodeDisplayTextArgs(node, fieldName, item, value, displayText, showAsNullText);
		}
		public static void ClearNodeDisplayTextArgs(NodeDisplayTextArgs args) {
			args.SetNodeDisplayTextArgs(null, null, null, null, args.DisplayText, args.ShowAsNullText);
		}
		public static RowFilterArgs CreateCustomRowFilterArgs(Func<int, object> getItemBySourceIndex) {
			return new RowFilterArgs(getItemBySourceIndex);
		}
		public static void SetRowFilterArgs(RowFilterArgs args, int sourceIndex, bool fit) {
			args.SetRowFilterArgs(sourceIndex, fit);
		}
		public static void ClearRowFilterArgs(RowFilterArgs args) {
			args.SetRowFilterArgs(-1, false);
		}
		public static RowSortArgs CreateRowSortArgs(Func<int, object> getItemBySourceIndex) {
			return new RowSortArgs(getItemBySourceIndex);
		}
		public static void SetRowSortArgs(RowSortArgs args, string fieldName, int firstSourceIndex, int secondSourceIndex, object firstValue, object secondValue, SortOrder sortOrder) {
			args.SetRowSortArgs(fieldName, firstSourceIndex, secondSourceIndex, firstValue, secondValue, sortOrder);
		}
		public static void ClearRowSortArgs(RowSortArgs args) {
			args.SetRowSortArgs(null, -1, -1, null, null, SortOrder.None);
		}
		public static NodeSortArgs CreateNodeSortArgs(Func<object, IEnumerable<object>> getPath) {
			return new NodeSortArgs(getPath);
		}
		public static void SetNodeSortArgs(NodeSortArgs args, string fieldName, object firstItem, object secondItem, object firstNode, object secondNode, object firstValue, object secondValue, SortOrder sortOrder) {
			args.SetNodeSortArgs(fieldName, firstItem, secondItem, firstNode, secondNode, firstValue, secondValue, sortOrder);
		}
		public static void ClearNodeSortArgs(NodeSortArgs args) {
			args.SetNodeSortArgs(null, null, null, null, null, null, null, SortOrder.None);
		}
		public static SummaryArgsItem CreateSummaryItem() {
			return new SummaryArgsItem();
		}
		public static void SetSummaryItem(SummaryArgsItem item, string propertyName, SummaryType summaryType, object tag) {
			item.SetSummaryDefinition(propertyName, summaryType, tag);
		}
		public static void ClearSummaryItem(SummaryArgsItem item) {
			item.SetSummaryDefinition(null, default(SummaryType), null);
		}
		public static RowSummaryArgs CreateRowSummaryArgs(Func<int, object> getItem, Func<int, int> getSourceIndexByRowHandle, IReadOnlyList<GroupInfo> groupPath) {
			return new RowSummaryArgs(getItem, getSourceIndexByRowHandle, groupPath);
		}
		public static void SetRowSummaryArgs(RowSummaryArgs args, int rowHandle, SummaryArgsItem summaryItem, object fieldValue, object totalValue, SummaryProcess summaryProcess, SummaryMode mode) {
			args.SetRowSummaryArgs(rowHandle, summaryItem, fieldValue, totalValue, summaryProcess, mode);
		}
		public static void ClearRowSummaryArgs(RowSummaryArgs args) {
			args.SetRowSummaryArgs(int.MinValue, null, null, null, default(SummaryProcess), default(SummaryMode));
		}
		public static RowSummaryExistsArgs CreateRowSummaryExistsArgs(IReadOnlyList<GroupInfo> groupPath) {
			return new RowSummaryExistsArgs(groupPath);
		}
		public static void SetRowSummaryExistsArgs(RowSummaryExistsArgs args, SummaryArgsItem summaryItem) {
			args.SetRowSummaryExistsArgs(summaryItem);
		}
		public static void ClearRowSummaryExistsArgs(RowSummaryExistsArgs args) {
			args.SetRowSummaryExistsArgs(null);
		}
		public static NodeSummaryArgs CreateNodeSummaryArgs(Func<object, IEnumerable<object>> getPath) {
			return new NodeSummaryArgs(getPath);
		}
		public static void SetNodeSummaryArgs(NodeSummaryArgs args, object item, object node, SummaryArgsItem summaryItem, object fieldValue, bool isTotalSummary, object totalValue, SummaryProcess summaryProcess) {
			args.SetNodeSummaryArgs(item, node, summaryItem, fieldValue, isTotalSummary, totalValue, summaryProcess);
		}
		public static void ClearNodeSummaryArgs(NodeSummaryArgs args) {
			args.SetNodeSummaryArgs(null, null, null, null, false, null, default(SummaryProcess));
		}
		public static NodeFilterArgs CreateNodeFilterArgs(Func<object, IEnumerable<object>> getPath, Func<object, bool> calcVisibility) {
			return new NodeFilterArgs(getPath, calcVisibility);
		}
		public static void SetNodeFilterArgs(NodeFilterArgs args, object node, object item) {
			args.SetNodeFilterArgs(node, item);
		}
		public static void ClearNodeFilterArgs(NodeFilterArgs args) {
			args.SetNodeFilterArgs(null, null);
		}
		public static CellMergeArgs CreateCellMergeArgs(Func<int, object> getItemByRowHandle, Func<int, int> getSourceIndexByRowHandle) {
			return new CellMergeArgs(getItemByRowHandle, getSourceIndexByRowHandle);
		}
		public static void SetCellMergeArgs(CellMergeArgs args, string fieldName, object firstCellValue, object secondCellValue, int firstRowHandle, int secondRowHandle) {
			args.SetCellMergeArgs(fieldName, firstCellValue, secondCellValue, firstRowHandle, secondRowHandle);
		}
		public static void ClearCellMergeArgs(CellMergeArgs args) {
			args.SetCellMergeArgs(null, null, null, -1, -1);
		}
	}
}
