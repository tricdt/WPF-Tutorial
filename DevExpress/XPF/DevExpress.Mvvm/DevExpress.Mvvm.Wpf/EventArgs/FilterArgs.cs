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
	public class FilterArgs {
		object item;
		bool isItemCreated;
		readonly Func<FilterArgs, object> getItemBySourceIndex;
		bool? defaultVisibility;
		readonly Func<FilterArgs, bool> calcVisibility;
		public object Item {
			get {
				if(!isItemCreated) {
					item = getItemBySourceIndex(this);
					isItemCreated = true;
				}
				return item;
			}
		}
		public bool DefaultVisibility {
			get {
				if(!defaultVisibility.HasValue)
					defaultVisibility = calcVisibility(this);
				return defaultVisibility.Value;
			}
		}
		public bool? Visible { get; set; }
		internal FilterArgs(Func<FilterArgs, object> getItemBySourceIndex, Func<FilterArgs, bool> calcVisibility) {
			this.getItemBySourceIndex = getItemBySourceIndex;
			this.calcVisibility = calcVisibility;
		}
		public FilterArgs(object item, bool fit) {
			this.item = item;
			isItemCreated = true;
			defaultVisibility = fit;
			Visible = null;
		}
		internal void ClearFilterArgs() {
			isItemCreated = false;
			defaultVisibility = null;
			Visible = null;
		}
	}
	public class RowFilterArgs : FilterArgs {
		bool fit;
		public int SourceIndex { get; private set; }
		internal RowFilterArgs(Func<int, object> getItemBySourceIndex)
			: base(self => getItemBySourceIndex(((RowFilterArgs)self).SourceIndex), self => ((RowFilterArgs)self).fit) { }
		public RowFilterArgs(object item, int sourceIndex, bool fit)
			: base(item, fit) {
			SourceIndex = sourceIndex;
		}
		internal void SetRowFilterArgs(int sourceIndex, bool fit) {
			ClearFilterArgs();
			SourceIndex = sourceIndex;
			this.fit = fit;
		}
	}
	public class NodeFilterArgs : FilterArgs {
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
		internal NodeFilterArgs(Func<object, IEnumerable<object>> getPath, Func<object, bool> calcVisibility)
			: base(self => ((NodeFilterArgs)self).item, self => calcVisibility(((NodeFilterArgs)self).node)) {
			this.getPath = getPath;
		}
		public NodeFilterArgs(object item, IEnumerable<object> path, bool fit)
			: base(item, fit) {
			this.path = path;
		}
		internal void SetNodeFilterArgs(object node, object item) {
			ClearFilterArgs();
			this.node = node;
			this.item = item;
			path = null;
		}
	}
}
