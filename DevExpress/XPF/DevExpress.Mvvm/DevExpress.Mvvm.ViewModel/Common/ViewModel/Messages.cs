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
using System.Linq;
using System.ComponentModel;
namespace DevExpress.Mvvm.ViewModel {
	public enum EntityMessageType {
		Added,
		Deleted,
		Changed
	}
	public class EntityMessage<TEntity, TPrimaryKey> {
		public EntityMessage(TPrimaryKey primaryKey, EntityMessageType messageType, object sender = null) {
			this.PrimaryKey = primaryKey;
			this.MessageType = messageType;
			this.Sender = sender;
		}
		public TPrimaryKey PrimaryKey { get; private set; }
		public EntityMessageType MessageType { get; private set; }
		public object Sender { get; private set; }
	}
	public class SaveAllMessage {
	}
	public class CloseAllMessage {
		readonly CancelEventArgs cancelEventArgs;
		Func<object, bool> viewModelPredicate;
		public CloseAllMessage(CancelEventArgs cancelEventArgs, Func<object, bool> viewModelPredicate) {
			this.cancelEventArgs = cancelEventArgs;
			this.viewModelPredicate = viewModelPredicate;
		}
		public bool ShouldProcess(object viewModel) {
			return viewModelPredicate(viewModel);
		}
		public bool Cancel {
			get { return cancelEventArgs.Cancel; }
			set { cancelEventArgs.Cancel = value; }
		}
	}
	public class DestroyOrphanedDocumentsMessage {
		readonly Func<object, bool> viewModelPredicate;
		public DestroyOrphanedDocumentsMessage(Func<object, bool> viewModelPredicate = null) {
			this.viewModelPredicate = viewModelPredicate;
		}
		public bool ShouldProcess(object viewModel) {
			return viewModelPredicate?.Invoke(viewModel) ?? true;
		}
	}
	public class NavigateMessage<TNavigationToken> {
		public NavigateMessage(TNavigationToken token) {
			Token = token;
		}
		public TNavigationToken Token { get; private set; }
	}
	public class SharedUnitOfWorkCollectionChangedMessage {
		public object Entity { get; set; }
		public object ViewModel { get; set; }
	};
	public class RefreshCollectionMessage<TEntity> { }
}
