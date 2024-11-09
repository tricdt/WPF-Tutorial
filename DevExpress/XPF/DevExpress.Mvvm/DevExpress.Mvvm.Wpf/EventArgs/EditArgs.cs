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

using System.Collections.Generic;
namespace DevExpress.Mvvm.Xpf {
	public class EditArgs {
		public object Item { get; }
		public EditArgs(object item) {
			Item = item;
		}
	}
	public class RowEditArgs : EditArgs {
		public int SourceIndex { get; }
		public RowEditArgs(object item, int sourceIndex)
			: base(item) {
			SourceIndex = sourceIndex;
		}
	}
	public class RowEditStartedArgs : RowEditArgs {
		public RowEditStartedArgs(object item, int sourceIndex)
			: base(item, sourceIndex) { }
	}
	public class RowEditFinishedArgs : RowEditArgs {
		public bool? Success { get; }
		public RowEditFinishedArgs(object item, int sourceIndex, bool? success) 
			: base(item, sourceIndex) {
			Success = success;
		}
	}
	public class RowEditStartingArgs : RowEditArgs {
		public CellEditorData[] CellEditors { get; }
		public bool Cancel { get; set; }
		public bool IsNewItem { get; }
		public RowEditStartingArgs(object item, int sourceIndex, bool isNewItem, CellEditorData[] cellEditors)
			: base(item, sourceIndex) {
			CellEditors = cellEditors;
			IsNewItem = isNewItem;
		}
	}
	public class NodeEditArgs : EditArgs {
		public IEnumerable<object> Path { get; }
		public NodeEditArgs(object item, IEnumerable<object> path)
			: base(item) {
			Path = path;
		}
	}
	public class NodeEditStartedArgs : NodeEditArgs {
		public NodeEditStartedArgs(object item, IEnumerable<object> path)
			: base(item, path) { }
	}
	public class NodeEditFinishedArgs : NodeEditArgs {
		public bool? Success { get; }
		public NodeEditFinishedArgs(object item, IEnumerable<object> path, bool? success)
			: base(item, path) {
			Success = success;
		}
	}
	public class NodeEditStartingArgs : NodeEditArgs {
		public CellEditorData[] CellEditors { get; }
		public bool Cancel { get; set; }
		public bool IsNewItem { get; }
		public NodeEditStartingArgs(object item, IEnumerable<object> path, bool isNewItem, CellEditorData[] cellEditors)
			: base(item, path) {
			CellEditors = cellEditors;
			IsNewItem = isNewItem;
		}
	}
}
