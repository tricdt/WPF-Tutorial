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
namespace DevExpress.Mvvm.Xpf.Native {
	struct GridLazyValuesContainer {
		readonly Func<int, object> getItemByRowHandle;
		readonly Func<int, int> getSourceIndexByRowHandle;
		readonly Func<int, int> getRowHandleBySourceIndex;
		public GridLazyValuesContainer(
			Func<int, object> getItemByRowHandle,
			Func<int, int> getSourceIndexByRowHandle,
			Func<int, int> getRowHandleBySourceIndex
		) {
			rowHandle = null;
			sourceIndex = null;
			itemInitialized = false;
			item = null;
			this.getItemByRowHandle = getItemByRowHandle;
			this.getSourceIndexByRowHandle = getSourceIndexByRowHandle;
			this.getRowHandleBySourceIndex = getRowHandleBySourceIndex;
		}
		int? rowHandle;
		public int RowHandle {
			get {
				if(rowHandle == null)
					rowHandle = getRowHandleBySourceIndex(SourceIndex);
				return rowHandle.Value;
			}
		}
		int? sourceIndex;
		public int SourceIndex {
			get {
				if(sourceIndex == null)
					sourceIndex = getSourceIndexByRowHandle(RowHandle);
				return sourceIndex.Value;
			}
		}
		object item;
		bool itemInitialized;
		public object Item {
			get {
				if(!itemInitialized) {
					itemInitialized = true;
					item = getItemByRowHandle(RowHandle);
				}
				return item;
			}
		}
		public static void Initialize(ref GridLazyValuesContainer self, int? rowHandle, int? sourceIndex) {
			if((rowHandle.HasValue && sourceIndex.HasValue) || (!rowHandle.HasValue && !sourceIndex.HasValue))
				throw new ArgumentException("rowHandle, sourceIndex");
			self.rowHandle = rowHandle;
			self.sourceIndex = sourceIndex;
			self.itemInitialized = false;
			self.item = null;
		}
	}
}
