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

using DevExpress.Mvvm.Xpf.Native;
using DevExpress.Xpf.Data;
using System;
using System.Collections.Generic;
namespace DevExpress.Mvvm.Xpf {
	public enum SummaryProcess {
		Start = 0,
		Calculate = 1,
		Finalize = 2
	}
	public enum SummaryMode {
		AllRows = 0,
		Selection = 1,
		Mixed = 2
	}
	public sealed class SummaryArgsItem : SummaryDefinition {
		internal SummaryArgsItem() { }
		public SummaryArgsItem(string propertyName, SummaryType summaryType, object tag = null) {
			SetSummaryDefinition(propertyName, summaryType, tag);
		}
	}
	public abstract class SummaryArgs {
		readonly Func<SummaryArgs, object> getItem;
		public object Item { get { return getItem(this); } }
		public object FieldValue { get; private set; }
		public SummaryArgsItem SummaryItem { get; private set; }
		public SummaryProcess SummaryProcess { get; private set; }
		public object TotalValue { get; set; }
		public bool TotalValueReady { get; set; }
		internal SummaryArgs(Func<SummaryArgs, object> getItem) {
			this.getItem = getItem;
		}
		public SummaryArgs(object item, SummaryArgsItem summaryItem, object fieldValue, object totalValue, SummaryProcess summaryProcess)
			: this(_ => item) {
			SetSummaryArgs(summaryItem, fieldValue, totalValue, summaryProcess);
		}
		internal void SetSummaryArgs(SummaryArgsItem summaryItem, object fieldValue, object totalValue, SummaryProcess summaryProcess) {
			FieldValue = fieldValue;
			SummaryItem = summaryItem;
			TotalValue = totalValue;
			TotalValueReady = false;
			SummaryProcess = summaryProcess;
		}
	}
	public class RowSummaryArgs : SummaryArgs {
		GridLazyValuesContainer itemContainer;
		public int SourceIndex { get { return itemContainer.SourceIndex; } }
		public IReadOnlyList<GroupInfo> GroupPath { get; }
		public bool IsTotalSummary { get { return GroupPath == null || GroupPath.Count == 0; } }
		public bool IsGroupSummary { get { return !IsTotalSummary; } }
		public SummaryMode Mode { get; private set; }
		internal RowSummaryArgs(Func<int, object> getItem, Func<int, int> getSourceIndexByRowHandle, IReadOnlyList<GroupInfo> groupPath)
			: base(_self => ((RowSummaryArgs)_self).itemContainer.Item) {
			itemContainer = new GridLazyValuesContainer(getItem, getSourceIndexByRowHandle, null);
			GroupPath = groupPath;
		}
		public RowSummaryArgs(object item, int sourceIndex, SummaryArgsItem summaryItem, object fieldValue, object totalValue, IReadOnlyList<GroupInfo> groupPath, SummaryProcess summaryProcess, SummaryMode mode)
			: this(_ => item, _ => sourceIndex, groupPath) {
			SetRowSummaryArgs(int.MinValue, summaryItem, fieldValue, totalValue, summaryProcess, mode);
		}
		internal void SetRowSummaryArgs(int rowHandle, SummaryArgsItem summaryItem, object fieldValue, object totalValue, SummaryProcess summaryProcess, SummaryMode mode) {
			SetSummaryArgs(summaryItem, fieldValue, totalValue, summaryProcess);
			SetRowSummaryArgs(rowHandle, mode);
		}
		void SetRowSummaryArgs(int rowHandle, SummaryMode mode) {
			GridLazyValuesContainer.Initialize(ref itemContainer, rowHandle, null);
			Mode = mode;
		}
	}
	public class NodeSummaryArgs : SummaryArgs {
		object item;
		object node;
		IEnumerable<object> path;
		readonly Func<object, IEnumerable<object>> getPath;
		public IEnumerable<object> Path {
			get {
				if(path == null)
					path = getPath(node);
				return path;
			}
		}
		public bool IsTotalSummary { get; private set; }
		public bool IsNodeSummary { get { return !IsTotalSummary; } }
		internal NodeSummaryArgs(Func<object, IEnumerable<object>> getPath)
			: base(_self => ((NodeSummaryArgs)_self).item) {
			this.getPath = getPath;
		}
		public NodeSummaryArgs(object item, IEnumerable<object> path, SummaryArgsItem summaryItem, object fieldValue, bool isTotalSummary, object totalValue, SummaryProcess summaryProcess)
			: this(null) {
			SetNodeSummaryArgs(item, null, summaryItem, fieldValue, isTotalSummary, totalValue, summaryProcess);
			this.path = path;
		}
		internal void SetNodeSummaryArgs(object item, object node, SummaryArgsItem summaryItem, object fieldValue, bool isTotalSummary, object totalValue, SummaryProcess summaryProcess) {
			SetSummaryArgs(summaryItem, fieldValue, totalValue, summaryProcess);
			this.item = item;
			this.node = node;
			path = null;
			IsTotalSummary = isTotalSummary;
		}
	}
	public class RowSummaryExistsArgs {
		public SummaryArgsItem SummaryItem { get; private set; }
		public IReadOnlyList<GroupInfo> GroupPath { get; }
		public bool IsTotalSummary { get { return GroupPath == null || GroupPath.Count == 0; } }
		public bool IsGroupSummary { get { return !IsTotalSummary; } }
		public bool Exists { get; set; }
		internal RowSummaryExistsArgs(IReadOnlyList<GroupInfo> groupPath) {
			GroupPath = groupPath;
		}
		public RowSummaryExistsArgs(SummaryArgsItem summaryItem, IReadOnlyList<GroupInfo> groupPath)
			: this(groupPath) {
			SetRowSummaryExistsArgs(summaryItem);
		}
		internal void SetRowSummaryExistsArgs(SummaryArgsItem summaryItem) {
			SummaryItem = summaryItem;
			Exists = true;
		}
	}
}
