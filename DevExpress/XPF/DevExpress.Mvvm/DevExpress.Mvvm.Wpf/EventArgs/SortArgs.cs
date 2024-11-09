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
using System.Collections.Generic;
namespace DevExpress.Mvvm.Xpf {
	public enum SortOrder {
		None,
		Ascending,
		Descending,
	}
	public abstract class SortArgs {
		readonly Func<SortArgs, object> getFirstItem;
		readonly Func<SortArgs, object> getSecondItem;
		public object FirstItem { get { return getFirstItem(this); } }
		public object SecondItem { get { return getSecondItem(this); } }
		public string FieldName { get; private set; }
		public object FirstValue { get; private set; }
		public object SecondValue { get; private set; }
		public SortOrder SortOrder { get; private set; }
		public int? Result { get; set; }
		internal SortArgs(Func<SortArgs, object> getFirstItem, Func<SortArgs, object> getSecondItem) {
			this.getFirstItem = getFirstItem;
			this.getSecondItem = getSecondItem;
		}
		public SortArgs(object firstItem, object secondItem, string fieldName, object firstValue, object secondValue, SortOrder sortOrder)
			: this(_ => firstItem, _ => secondItem) {
			SetSortArgs(fieldName, firstValue, secondValue, sortOrder);
		}
		internal void SetSortArgs(string fieldName, object firstValue, object secondValue, SortOrder sortOrder) {
			FieldName = fieldName;
			FirstValue = firstValue;
			SecondValue = secondValue;
			SortOrder = sortOrder;
			Result = null;
		}
	}
	public class RowSortArgs : SortArgs {
		object firstItem;
		bool isFirstItemCreated;
		static Func<SortArgs, object> GetFirstItem(Func<int, object> getItemBySourceIndex) {
			return args => {
				var self = (RowSortArgs)args;
				if(!self.isFirstItemCreated) {
					self.firstItem = getItemBySourceIndex(self.FirstSourceIndex);
					self.isFirstItemCreated = true;
				}
				return self.firstItem;
			};
		}
		object secondItem;
		bool isSecondItemCreated;
		static Func<SortArgs, object> GetSecondItem(Func<int, object> getItemBySourceIndex) {
			return args => {
				var self = (RowSortArgs)args;
				if(!self.isSecondItemCreated) {
					self.secondItem = getItemBySourceIndex(self.SecondSourceIndex);
					self.isSecondItemCreated = true;
				}
				return self.secondItem;
			};
		}
		public int FirstSourceIndex { get; private set; } = -1;
		public int SecondSourceIndex { get; private set; } = -1;
		internal RowSortArgs(Func<int, object> getItemBySourceIndex)
			: base(GetFirstItem(getItemBySourceIndex), GetSecondItem(getItemBySourceIndex)) {
		}
		public RowSortArgs(object firstItem, object secondItem, string fieldName, int firstSourceIndex, int secondSourceIndex, object firstValue, object secondValue, SortOrder sortOrder)
			: base(_ => firstItem, _ => secondItem) {
			SetRowSortArgs(fieldName, firstSourceIndex, secondSourceIndex, firstValue, secondValue, sortOrder);
		}
		internal void SetRowSortArgs(string fieldName, int firstSourceIndex, int secondSourceIndex, object firstValue, object secondValue, SortOrder sortOrder) {
			SetSortArgs(fieldName, firstValue, secondValue, sortOrder);
			isFirstItemCreated = false;
			firstItem = null;
			isSecondItemCreated = false;
			secondItem = null;
			FirstSourceIndex = firstSourceIndex;
			SecondSourceIndex = secondSourceIndex;
		}
	}
	public class NodeSortArgs : SortArgs {
		object firstItem;
		object secondItem;
		object firstNode;
		object secondNode;
		IEnumerable<object> firstPath;
		IEnumerable<object> secondPath;
		readonly Func<object, IEnumerable<object>> getPath;
		public IEnumerable<object> FirstPath {
			get {
				if(firstPath == null)
					firstPath = getPath(firstNode);
				return firstPath;
			}
		}
		public IEnumerable<object> SecondPath {
			get {
				if(secondPath == null)
					secondPath = getPath(secondNode);
				return secondPath;
			}
		}
		internal NodeSortArgs(Func<object, IEnumerable<object>> getPath)
			: base(self => ((NodeSortArgs)self).firstItem, self => ((NodeSortArgs)self).secondItem) {
			this.getPath = getPath;
		}
		public NodeSortArgs(object firstItem, object secondItem, string fieldName, IEnumerable<object> firstPath, IEnumerable<object> secondPath, object firstValue, object secondValue, SortOrder sortOrder)
			: this(null) {
			SetNodeSortArgs(fieldName, firstItem, secondItem, null, null, firstValue, secondValue, sortOrder);
			this.firstPath = firstPath;
			this.secondPath = secondPath;
		}
		internal void SetNodeSortArgs(string fieldName, object firstItem, object secondItem, object firstNode, object secondNode, object firstValue, object secondValue, SortOrder sortOrder) {
			SetSortArgs(fieldName, firstValue, secondValue, sortOrder);
			this.firstItem = firstItem;
			this.secondItem = secondItem;
			this.firstNode = firstNode;
			firstPath = null;
			this.secondNode = secondNode;
			secondPath = null;
		}
	}
}
