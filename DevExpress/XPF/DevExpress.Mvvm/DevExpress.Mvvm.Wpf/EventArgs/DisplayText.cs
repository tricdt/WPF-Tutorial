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
using System;
using System.Collections.Generic;
namespace DevExpress.Mvvm.Xpf {
	public class DisplayTextArgs {
		readonly Func<DisplayTextArgs, object> getItem;
		public object Item { get { return getItem(this); } }
		public string FieldName { get; private set; }
		public object Value { get; private set; }
		public string DisplayText { get; set; }
		internal DisplayTextArgs(Func<DisplayTextArgs, object> getItem) {
			this.getItem = getItem;
		}
		public DisplayTextArgs(object item, string fieldName, object value, string displayText)
			: this(_ => item) {
			SetDisplayTextArgs(fieldName, value, displayText);
		}
		internal void SetDisplayTextArgs(string fieldName, object value, string displayText) {
			FieldName = fieldName;
			Value = value;
			DisplayText = displayText;
		}
	}
	public class ColumnDisplayTextArgs : DisplayTextArgs {
		GridLazyValuesContainer lazyValues;
		public int SourceIndex { get { return lazyValues.SourceIndex; } }
		public bool ShowAsNullText { get; set; }
		internal ColumnDisplayTextArgs(
			Func<int, object> getItemByRowHandle,
			Func<int, int> getSourceIndexByRowHandle,
			Func<int, int> getRowHandleBySourceIndex
		) : base(self => ((ColumnDisplayTextArgs)self).lazyValues.Item) {
			lazyValues = new GridLazyValuesContainer(getItemByRowHandle, getSourceIndexByRowHandle, getRowHandleBySourceIndex);
		}
		public ColumnDisplayTextArgs(object item, string fieldName, int sourceIndex, object value, string displayText, bool showAsNullText)
			: this(_ => item, null, _ => int.MinValue) {
			SetColumnDisplayTextArgs(fieldName, null, sourceIndex, value, displayText, showAsNullText);
		}
		internal void SetColumnDisplayTextArgs(string fieldName, int? rowHandle, int? sourceIndex, object value, string displayText, bool showAsNullText) {
			SetDisplayTextArgs(fieldName, value, displayText);
			GridLazyValuesContainer.Initialize(ref lazyValues, rowHandle, sourceIndex);
			ShowAsNullText = showAsNullText;
		}
	}
	public class NodeDisplayTextArgs : DisplayTextArgs {
		object item;
		object node;
		IEnumerable<object> path;
		readonly Func<object, IEnumerable<object>> getPath;
		public bool ShowAsNullText { get; set; }
		public IEnumerable<object> Path {
			get {
				if(path == null)
					path = getPath(node);
				return path;
			}
		}
		internal NodeDisplayTextArgs(Func<object, IEnumerable<object>> getPath)
			: base(self => ((NodeDisplayTextArgs)self).item) {
			this.getPath = getPath;
		}
		public NodeDisplayTextArgs(object item, IEnumerable<object> path, string fieldName, object value, string displayText, bool showAsNullText)
			: this(null) {
			SetNodeDisplayTextArgs(null, fieldName, item, value, displayText, showAsNullText);
			this.path = path;
		}
		internal void SetNodeDisplayTextArgs(object node, string fieldName, object item, object value, string displayText, bool showAsNullText) {
			SetDisplayTextArgs(fieldName, value, displayText);
			this.item = item;
			ShowAsNullText = showAsNullText;
			this.node = node;
			path = null;
		}
	}
	public class GroupDisplayTextArgs : DisplayTextArgs {
		public IReadOnlyList<GroupInfo> GroupPath { get; }
		public GroupDisplayTextArgs(IReadOnlyList<GroupInfo> groupPath, object item, string fieldName, object value, string displayText)
			: base(item, fieldName, value, displayText) {
			GroupPath = groupPath;
		}
	}
}
