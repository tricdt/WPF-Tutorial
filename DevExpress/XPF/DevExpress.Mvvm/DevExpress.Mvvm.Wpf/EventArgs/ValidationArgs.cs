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
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
namespace DevExpress.Mvvm.Xpf {
	public enum ValidationErrorType {
		Default = 1,
		Information = 2,
		Warning = 3,
		Critical = 4,
	}
	public class ValidationErrorInfo {
		public string ErrorContent { get; }
		public ValidationErrorType ErrorType { get; }
		public ValidationErrorInfo(string errorContent, ValidationErrorType errorType = ValidationErrorType.Default) {
			ErrorContent = errorContent;
			ErrorType = errorType;
		}
		public static implicit operator ValidationErrorInfo(string errorContent) => new ValidationErrorInfo(errorContent);
		public override bool Equals(object obj) {
			ValidationErrorInfo error = obj as ValidationErrorInfo;
			return error != null &&
				   EqualityComparer<object>.Default.Equals(ErrorContent, error.ErrorContent) &&
				   ErrorType == error.ErrorType;
		}
		public override int GetHashCode() {
			int hashCode = 733508479;
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(ErrorContent);
			hashCode = hashCode * -1521134295 + ErrorType.GetHashCode();
			return hashCode;
		}
	}
	public abstract class ValidationArgs {
		readonly bool allowAsyncResult;
		Task<ValidationErrorInfo> resultAsync;
		public object Item { get; }
		public bool IsNewItem { get; }
		public ValidationErrorInfo Result { get; set; }
		public Task<ValidationErrorInfo> ResultAsync {
			get { return resultAsync; }
			set {
				if(!allowAsyncResult)
					throw new Exception("Specify the TableView.ShowUpdateRowButtons / TreeListView.ShowUpdateRowButtons property to allow the async data update.");
				resultAsync = value;
			}
		}
		public CancellationToken CancellationToken { get; }
		public bool UseCancellationToken { get; set; }
		public ValidationArgs(object item, bool isNewItem, CancellationToken cancellationToken, bool allowAsyncResult) {
			Item = item;
			IsNewItem = isNewItem;
			CancellationToken = cancellationToken;
			this.allowAsyncResult = allowAsyncResult;
		}
	}
	public class RowValidationArgs : ValidationArgs {
		public int SourceIndex { get; }
		public RowValidationArgs(object item, int sourceIndex, bool isNewItem, CancellationToken cancellationToken, bool allowAsyncResult)
			: base(item, isNewItem, cancellationToken, allowAsyncResult) {
			SourceIndex = sourceIndex;
		}
	}
	public class NodeValidationArgs : ValidationArgs {
		public IEnumerable<object> Path { get; }
		public NodeValidationArgs(object item, IEnumerable<object> path, bool isNewItem, CancellationToken cancellationToken, bool allowAsyncResult)
			: base(item, isNewItem, cancellationToken, allowAsyncResult) {
			Path = path;
		}
	}
	public enum DisplayDeleteOperationError {
		ShowMessageBox, Disabled
	}
	public abstract class DeleteValidationArgs {
		public object[] Items { get; }
		public ValidationErrorInfo Result { get; set; }
		public DisplayDeleteOperationError DisplayErrorMode { get; set; }
		public Task<ValidationErrorInfo> ResultAsync { get; set; }
		public DeleteValidationArgs(object[] items) {
			Items = items;
		}
	}
	public class ValidateRowDeletionArgs : DeleteValidationArgs {
		public int[] SourceIndexes { get; }
		public ValidateRowDeletionArgs(object[] items, int[] sourceIndexes) : base(items) {
			SourceIndexes = sourceIndexes;
		}
	}
	public class ValidateNodeDeletionArgs : DeleteValidationArgs {
		public IEnumerable<object>[] Paths { get; }
		public ValidateNodeDeletionArgs(object[] items, IEnumerable<object>[] paths) : base(items) {
			Paths = paths;
		}
	}
	public class RowCellValidationArgs : RowValidationArgs {
		public string FieldName { get; set; }
		public object OldValue { get; }
		public object NewValue { get; }
		public RowCellValidationArgs(string fieldName, object oldValue, object newValue, object item, int sourceIndex, bool isNewItem, CancellationToken cancellationToken, bool allowAsyncResult)
			: base(item, sourceIndex, isNewItem, cancellationToken, allowAsyncResult) {
			FieldName = fieldName;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
	public class NodeCellValidationArgs : NodeValidationArgs {
		public string FieldName { get; set; }
		public object OldValue { get; }
		public object NewValue { get; }
		public NodeCellValidationArgs(string fieldName, object oldValue, object newValue, object item, IEnumerable<object> path, bool isNewItem, CancellationToken cancellationToken, bool allowAsyncResult)
			: base(item, path, isNewItem, cancellationToken, allowAsyncResult) {
			FieldName = fieldName;
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
