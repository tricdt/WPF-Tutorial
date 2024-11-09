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
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.ViewModel;
namespace DevExpress.Mvvm.ViewModel {
	[POCOViewModel]
	public class ReadOnlyCollectionViewModelBase<TEntity, TProjection, TUnitOfWork> : EntitiesViewModelBase<TEntity, TProjection, TUnitOfWork>, ISupportLogicalLayout
		where TEntity : class
		where TProjection : class
		where TUnitOfWork : IUnitOfWork {
		internal protected ReadOnlyCollectionViewModelBase(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection)
			: this(unitOfWorkFactory, getRepositoryFunc, projection, UnitOfWorkPolicy.Individual) { }
		internal protected ReadOnlyCollectionViewModelBase(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection,
			UnitOfWorkPolicy unitOfWorkPolicy
			) : base(unitOfWorkFactory, getRepositoryFunc, projection, unitOfWorkPolicy) {
			Messenger.Default.Register<CloseAllMessage>(this, m => {
				if(m.ShouldProcess(this)) {
					SaveLayout();
				}
			});
		}
		public virtual TProjection SelectedEntity { get; set; }
		public virtual Expression<Func<TEntity, bool>> FilterExpression { get; set; }
		public virtual void Refresh() {
			if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
				for(var i = 0; i < Entities.Count; ++i)
					Entities.Move(i, i);
			} else {
				LoadEntities(false);
			}
		}
		[ServiceProperty(false)]
		protected virtual ILayoutSerializationService LayoutSerializationService { get { return this.GetService<ILayoutSerializationService>(); } }
		protected virtual string ViewName { get { return typeof(TEntity).Name + "ReadonlyCollectionView"; } }
		public bool CanSerialize { get { return true; } }
		public virtual IDocumentManagerService DocumentManagerService { get { return this.GetService<IDocumentManagerService>(); } }
		public IEnumerable<object> LookupViewModels { get { return null; } }
		bool isLoaded = false;
		[Display(AutoGenerateField = false)]
		public virtual void OnLoaded() {
			isLoaded = true;
			PersistentLayoutHelper.TryDeserializeLayout(LayoutSerializationService, ViewName);
		}
		[Display(AutoGenerateField = false)]
		public virtual void OnUnloaded() {
			if(isLoaded) {
				SaveLayout();
			}
		}
		void SaveLayout() {
			PersistentLayoutHelper.TrySerializeLayout(LayoutSerializationService, ViewName);
		}
		protected override void OnClose(CancelEventArgs e) {
			SaveLayout();
			Messenger.Default.Send(new DestroyOrphanedDocumentsMessage(x => x != this));
			base.OnClose(e);
		}
		public bool CanRefresh() {
			return !IsLoading;
		}
		protected override void OnEntitiesAssigned(Func<TProjection> getSelectedEntityCallback) {
			base.OnEntitiesAssigned(getSelectedEntityCallback);
			if(getSelectedEntityCallback != null)
				SelectedEntity = getSelectedEntityCallback();
		}
		protected override Func<TProjection> GetSelectedEntityCallback() {
			if(SelectedEntity == null)
				return null;
			int selectedItemIndex = Entities.IndexOf(SelectedEntity);
			return () => (selectedItemIndex >= 0 && selectedItemIndex < Entities.Count) ? Entities[selectedItemIndex] : null;
		}
		protected override void OnIsLoadingChanged() {
			base.OnIsLoadingChanged();
			this.RaiseCanExecuteChanged(x => x.Refresh());
		}
		protected virtual void OnSelectedEntityChanged() { }
		protected virtual void OnFilterExpressionChanged() {
			if(IsLoaded || IsLoading)
				LoadEntities(true);
		}
		protected override Expression<Func<TEntity, bool>> GetFilterExpression() {
			return FilterExpression;
		}
	}
}
