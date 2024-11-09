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
	public enum ExceptionMode {
		DisplayError,
		ThrowException,
		NoAction,
		Ignore,
	};
	public class InvalidItemExceptionArgs {
		public object Item { get; }
		public bool IsNewItem { get; }
		public Exception Exception { get; }
		public string ErrorText { get; set; }
		public string WindowCaption { get; set; }
		public ExceptionMode ExceptionMode { get; set; }
		public InvalidItemExceptionArgs(object item, bool isNewItem, Exception exception, string errorText, string windowCaption, ExceptionMode exceptionMode) {
			Item = item;
			IsNewItem = isNewItem;
			Exception = exception;
			ErrorText = errorText;
			WindowCaption = windowCaption;
			ExceptionMode = exceptionMode;
		}
	}
	public class InvalidRowExceptionArgs : InvalidItemExceptionArgs{
		public int SourceIndex { get; }
		public InvalidRowExceptionArgs(object item, bool isNewItem, int sourceIndex, Exception exception, string errorText, string windowCaption, ExceptionMode exceptionMode)
			: base(item, isNewItem, exception, errorText, windowCaption, exceptionMode) {
			SourceIndex = sourceIndex;
		}
	}
	public class InvalidNodeExceptionArgs : InvalidItemExceptionArgs {
		public IEnumerable<object> Path { get; }
		public InvalidNodeExceptionArgs(object item, bool isNewItem, IEnumerable<object> path, Exception exception, string errorText, string windowCaption, ExceptionMode exceptionMode)
			: base(item, isNewItem, exception, errorText, windowCaption, exceptionMode) {
			Path = path;
		}
	}
}
