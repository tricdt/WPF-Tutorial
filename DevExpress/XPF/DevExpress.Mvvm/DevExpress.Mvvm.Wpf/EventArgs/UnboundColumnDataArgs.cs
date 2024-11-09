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
	public class UnboundColumnDataArgs {
		public object Item { get; }
		public string FieldName { get; }
		public bool IsGetData { get; }
		public bool IsSetData { get { return !IsGetData; } }
		public object Value { get; set; }
		public UnboundColumnDataArgs(object item, string fieldName, bool isGetData, object value) {
			Item = item;
			FieldName = fieldName;
			IsGetData = isGetData;
			Value = value;
		}
	}
	public class UnboundColumnNodeArgs : UnboundColumnDataArgs {
		public IEnumerable<object> Path { get; }
		public UnboundColumnNodeArgs(object item, IEnumerable<object> path, string fieldName, bool isGetData, object value)
			: base(item, fieldName, isGetData, value) {
			Path = path;
		}
	}
	public class UnboundColumnRowArgs : UnboundColumnDataArgs {
		public int SourceIndex { get; }
		public UnboundColumnRowArgs(object item, int sourceIndex, string fieldName, bool isGetData, object value)
			: base(item, fieldName, isGetData, value) {
			SourceIndex = sourceIndex;
		}
	}
}
