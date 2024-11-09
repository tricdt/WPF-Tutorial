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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
namespace DevExpress.Mvvm.ViewModel {
	public partial class PeekCollectionViewModel<TNavigationToken, TEntity, TPrimaryKey, TUnitOfWork> : CollectionViewModelBase<TEntity, TEntity, TPrimaryKey, TUnitOfWork>
		where TEntity : class
		where TUnitOfWork : IUnitOfWork {
		public static PeekCollectionViewModel<TNavigationToken, TEntity, TPrimaryKey, TUnitOfWork> Create(
			TNavigationToken navigationToken,
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TEntity>> projection = null) {
			return ViewModelSource.Create(() => new PeekCollectionViewModel<TNavigationToken, TEntity, TPrimaryKey, TUnitOfWork>(navigationToken, unitOfWorkFactory, getRepositoryFunc, projection));
		}
		TNavigationToken navigationToken;
		TEntity pickedEntity;
		protected PeekCollectionViewModel(
			TNavigationToken navigationToken,
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TEntity>> projection = null
			) : base(unitOfWorkFactory, getRepositoryFunc, projection, null, null, true) {
			this.navigationToken = navigationToken;
		}
		[Display(AutoGenerateField = false)]
		public void Navigate(TEntity projectionEntity) {
			pickedEntity = projectionEntity;
			SendSelectEntityMessage();
			Messenger.Default.Send(new NavigateMessage<TNavigationToken>(navigationToken), navigationToken);
		}
		public bool CanNavigate(TEntity projectionEntity) {
			return projectionEntity != null;
		}
		protected override void OnInitializeInRuntime() {
			base.OnInitializeInRuntime();
			Messenger.Default.Register<SelectedEntityRequest>(this, x => SendSelectEntityMessage());
		}
		void SendSelectEntityMessage() {
			if(IsLoaded && pickedEntity != null)
				Messenger.Default.Send(new SelectEntityMessage(CreateRepository().GetProjectionPrimaryKey(pickedEntity)));
		}
		[ServiceProperty(false)]
		protected override ILayoutSerializationService LayoutSerializationService { get { return null; } }
	}
}
