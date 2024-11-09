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
namespace DevExpress.Mvvm.Xpf {
	public class CellMergeArgs {
		object firstItem;
		object secondItem;
		bool isFirstItemCreated;
		bool isSecondItemCreated;
		int firstRowHandle;
		int secondRowHandle;
		int firstSourceIndex;
		int secondSourceIndex;
		readonly Func<int, object> getItemByRowHandle;
		readonly Func<int, int> getSourceIndexByRowHandle;
		public string FieldName { get; private set; }
		public object FirstItem {
			get {
				if(!isFirstItemCreated) {
					firstItem = getItemByRowHandle(firstRowHandle);
					isFirstItemCreated = true;
				}
				return firstItem;
			}
		}
		public object SecondItem {
			get {
				if(!isSecondItemCreated) {
					secondItem = getItemByRowHandle(secondRowHandle);
					isSecondItemCreated = true;
				}
				return secondItem;
			}
		}
		public object FirstCellValue { get; private set; }
		public object SecondCellValue { get; private set; }
		public int FirstSourceIndex {
			get {
				if(firstSourceIndex < 0)
					firstSourceIndex = getSourceIndexByRowHandle(firstRowHandle);
				return firstSourceIndex;
			}
		}
		public int SecondSourceIndex {
			get {
				if(secondSourceIndex < 0)
					secondSourceIndex = getSourceIndexByRowHandle(secondRowHandle);
				return secondSourceIndex;
			}
		}
		public bool? Merge { get; set; }
		internal CellMergeArgs(Func<int, object> getItemByRowHandle, Func<int, int> getSourceIndexByRowHandle) {
			this.getItemByRowHandle = getItemByRowHandle;
			this.getSourceIndexByRowHandle = getSourceIndexByRowHandle;
		}
		public CellMergeArgs(string fieldName, object firstItem, object secondItem, object firstCellValue, object secondCellValue, int firstSourceIndex, int secondSourceIndex)
			: this (null, null) {
			SetCellMergeArgs(fieldName, firstCellValue, secondCellValue, -1, -1);
			this.firstItem = firstItem;
			isFirstItemCreated = true;
			this.secondItem = secondItem;
			isSecondItemCreated = true;
			this.firstSourceIndex = firstSourceIndex;
			this.secondSourceIndex = secondSourceIndex;
		}
		internal void SetCellMergeArgs(string fieldName, object firstCellValue, object secondCellValue, int firstRowHandle, int secondRowHandle) {
			Merge = null;
			FieldName = fieldName;
			FirstCellValue = firstCellValue;
			SecondCellValue = secondCellValue;
			this.firstRowHandle = firstRowHandle;
			this.secondRowHandle = secondRowHandle;
			isFirstItemCreated = false;
			isSecondItemCreated = false;
			firstSourceIndex = -1;
			secondSourceIndex = -1;
		}
	}
}
