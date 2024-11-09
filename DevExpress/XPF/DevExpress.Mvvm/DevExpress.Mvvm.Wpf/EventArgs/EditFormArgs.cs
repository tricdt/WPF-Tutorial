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
using System.Threading;
using System.Threading.Tasks;
namespace DevExpress.Mvvm.Xpf {
	public class CreateEditItemViewModelArgs {
		public CreateEditItemViewModelArgs(object key) {
			Key = key;
		}
		public object Key { get; }
		public bool IsNewItem => Key == null;
		public IEditItemViewModel ViewModel { get; set; }
		public Task<IEditItemViewModel> GetViewModelAsync { get; set; }
	}
	public interface IEditItemViewModel : IDisposable {
		object Item { get; }
		object EditOperationContext { get; }
		string Title { get; }
	}
	public class EditFormRowValidationArgs {
		public EditFormRowValidationArgs(object item, bool isNewItem, object editOperationContext, CancellationToken? cancellationToken = null) {
			Item = item;
			IsNewItem = isNewItem;
			EditOperationContext = editOperationContext;
			if(cancellationToken.HasValue)
				CancellationToken = cancellationToken.Value;
		}
		public object Item { get; }
		public bool IsNewItem { get; }
		public object EditOperationContext { get; }
		public Task ValidateAsync { get; set; }
		public CancellationToken CancellationToken { get; }
	}
	public class EditFormValidateRowDeletionArgs {
		public EditFormValidateRowDeletionArgs(object[] keys) {
			Keys = keys;
		}
		public object[] Keys { get; }
		public Task ValidateAsync { get; set; }
	}
	public class EditItemViewModel : ViewModelBase, IEditItemViewModel {
		bool disposedValue;
		readonly Action dispose;
		public EditItemViewModel(object item, object editOperationContext, Action dispose = null, string title = null) {
			this.item = item;
			this.editOperationContext = editOperationContext;
			this.dispose = dispose;
			this.title = title ?? Item?.GetType().Name;
		}
		object item;
		public object Item {
			get { return item; }
			protected set { SetValue(ref item, value); }
		}
		string title;
		public string Title {
			get { return title; }
			protected set { SetValue(ref title, value); }
		}
		object editOperationContext;
		public object EditOperationContext {
			get { return editOperationContext; }
			protected set { SetValue(ref editOperationContext, value); }
		}
		protected virtual void Dispose(bool disposing) {
			if(!disposedValue) {
				if(disposing) {
					dispose?.Invoke();
				}
				disposedValue = true;
			}
		}
		public void Dispose() {
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
