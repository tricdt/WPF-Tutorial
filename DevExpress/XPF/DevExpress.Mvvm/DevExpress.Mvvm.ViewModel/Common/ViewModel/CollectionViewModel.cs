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
using DevExpress.Mvvm.Localization;
using System.Diagnostics;
using DevExpress.Mvvm.Native;
namespace DevExpress.Mvvm.ViewModel {
	public class ViewModelInitInfo {
		public UnitOfWorkPolicy UnitOfWorkPolicy;
		public object UnitOfWorkFactory;
		public object PrimaryKey;
		public object NewEntityInitializer;
		public object NewEntity;
		public bool IsSharedTreeRoot;
	}
	public enum UnitOfWorkPolicy {
		Individual,
		Shared,
		PassSharedToChildren,
	}
	public class CollectionViewModelState {
		public UnitOfWorkPolicy UnitOfWorkPolicy { get; set; }
	}
	public interface ICollectionViewModel {
		void SendRefreshCollectionsMessage();
	}
	public class CollectionViewModelBase<TEntity, TProjection, TPrimaryKey, TUnitOfWork> : ReadOnlyCollectionViewModelBase<TEntity, TProjection, TUnitOfWork>, ISupportLogicalLayout<CollectionViewModelState>, ICollectionViewModel
		where TEntity : class
		where TProjection : class
		where TUnitOfWork : IUnitOfWork {
		EntitiesChangeTracker<TPrimaryKey> ChangeTrackerWithKey { get { return (EntitiesChangeTracker<TPrimaryKey>)ChangeTracker; } }
		readonly Action<TEntity> newEntityInitializer;
		readonly Func<bool> canCreateNewEntity;
		protected IRepository<TEntity, TPrimaryKey> Repository { get { return (IRepository<TEntity, TPrimaryKey>)ReadOnlyRepository; } }
		IDocument openedSharedUOWDetail = null;
		bool sharedChildChanged = false;
		public CollectionViewModelBase(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection = null,
			Action<TEntity> newEntityInitializer = null,
			Func<bool> canCreateNewEntity = null,
			bool ignoreSelectEntityMessage = false,
			UnitOfWorkPolicy unitOfWorkPolicy = UnitOfWorkPolicy.Individual) 
			: base(unitOfWorkFactory, getRepositoryFunc, projection, unitOfWorkPolicy)
		{
			RepositoryExtensions.VerifyProjection(CreateRepository(), projection);
			this.newEntityInitializer = newEntityInitializer;
			this.canCreateNewEntity = canCreateNewEntity;
			this.ignoreSelectEntityMessage = ignoreSelectEntityMessage;
			if(!this.IsInDesignMode())
				RegisterSelectEntityMessage();
			Messenger.Default.Register<SharedUnitOfWorkCollectionChangedMessage>(this, OnChildSharedUnitOfWorkSingleClosed);
			Messenger.Default.Register<RefreshCollectionMessage<TEntity>>(this, OnRefreshCollection);
		}
		void OnRefreshCollection(RefreshCollectionMessage<TEntity> message) {
			Refresh();
		}
		public virtual void New() {
			if(canCreateNewEntity != null && !canCreateNewEntity())
				return;
			var parameter = new ViewModelInitInfo {
				UnitOfWorkPolicy = UnitOfWorkPolicy != UnitOfWorkPolicy.Individual ? UnitOfWorkPolicy.Shared : UnitOfWorkPolicy.Individual,
				NewEntityInitializer = newEntityInitializer,
				PrimaryKey = null,
				UnitOfWorkFactory = UnitOfWorkFactory,
				IsSharedTreeRoot = UnitOfWorkPolicy == UnitOfWorkPolicy.PassSharedToChildren
			};
			var service = UnitOfWorkPolicy == UnitOfWorkPolicy.Shared ? WindowedDocumentManagerService : DocumentManagerService;
			service = service ?? DocumentManagerService;
			openedSharedUOWDetail = service.With(x => x.CreateDocument<TEntity>(parameter, this));
			openedSharedUOWDetail.Do(x => {
				x.Show();
				x.DestroyOnClose = UnitOfWorkPolicy == UnitOfWorkPolicy.Shared;
			});
		}
		void OnChildSharedUnitOfWorkSingleClosed(SharedUnitOfWorkCollectionChangedMessage message) {
			if(openedSharedUOWDetail == null || openedSharedUOWDetail.Content != message.ViewModel || message.Entity == null)
				return;
			openedSharedUOWDetail = null;
			sharedChildChanged = true;
			var entity = (TProjection)message.Entity;
			if(!Entities.Contains(entity)) {
				Entities.Add(entity);
			}
			Refresh();
			Messenger.Default.Send(new SharedUnitOfWorkCollectionChangedMessage {
				Entity = null,
				ViewModel = this
			});
		}
		public virtual void Edit(TProjection projectionEntity) {
			if(Repository.IsDetached(projectionEntity))
				return;
			TPrimaryKey primaryKey = Repository.GetProjectionPrimaryKey(projectionEntity);
			int index = Entities.IndexOf(projectionEntity);
			if(UnitOfWorkPolicy != UnitOfWorkPolicy.Shared) {
				projectionEntity = ChangeTrackerWithKey.FindActualProjectionByKey(primaryKey);
				if(index >= 0) {
					if(projectionEntity == null)
						Entities.RemoveAt(index);
					else
						Entities[index] = projectionEntity;
				}
			}
			if(projectionEntity == null) {
				DestroyDocument(DocumentManagerService.FindEntityDocument<TEntity, TPrimaryKey>(primaryKey));
				return;
			}
			var parameter = new ViewModelInitInfo {
				UnitOfWorkPolicy = UnitOfWorkPolicy != UnitOfWorkPolicy.Individual ? UnitOfWorkPolicy.Shared : UnitOfWorkPolicy.Individual,
				NewEntityInitializer = newEntityInitializer,
				PrimaryKey = primaryKey,
				UnitOfWorkFactory = UnitOfWorkFactory,
				IsSharedTreeRoot = UnitOfWorkPolicy == UnitOfWorkPolicy.PassSharedToChildren
			};
			if (UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
				var isNewEntity = Repository.GetState((TEntity)(object)Entities[index]) == EntityState.Added;
				if (isNewEntity) {
					parameter.NewEntity = Entities[index];
				}
				openedSharedUOWDetail = WindowedDocumentManagerService.CreateDocument<TEntity>(parameter, this);
				openedSharedUOWDetail.DestroyOnClose = true;
				openedSharedUOWDetail.Show();
			} else {
				DocumentManagerService.ShowExistingEntityDocument<TEntity, TPrimaryKey>(this, primaryKey, parameter);
			}
		}
		public virtual bool CanEdit(TProjection projectionEntity) {
			return projectionEntity != null && !IsLoading;
		}
		public virtual void Delete(TProjection projectionEntity) {
			if(MessageBoxService.ShowMessage(string.Format(ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Delete), EntityDisplayName),
				ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Caption), MessageButton.YesNo) != MessageResult.Yes)
				return;
			try {
				Entities.Remove(projectionEntity);
				if (UnitOfWorkPolicy == UnitOfWorkPolicy.Shared && Repository.GetState((TEntity)(object)projectionEntity) == EntityState.Added) {
					Repository.Remove((TEntity)(object)projectionEntity);
					return;
				}
				TPrimaryKey primaryKey = Repository.GetProjectionPrimaryKey(projectionEntity);
				TEntity entity = Repository.Find(primaryKey);
				if(entity != null) {
					OnBeforeEntityDeleted(primaryKey, entity);
					Repository.Remove(entity);
					if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
						Messenger.Default.Send(new SharedUnitOfWorkCollectionChangedMessage {
							Entity = null,
							ViewModel = this
						});
						sharedChildChanged = true;
					} else {
						Repository.UnitOfWork.SaveChanges();
						OnEntityDeleted(primaryKey, entity);
					}
				}
			} catch (DbException e) {
				Refresh();
				OnDbException(e);
			}
		}
		protected virtual void OnDbException(DbException e) {
			MessageBoxService.ShowMessage(e.ErrorMessage, e.ErrorCaption, MessageButton.OK, MessageIcon.Error);
		}
		public virtual bool CanDelete(TProjection projectionEntity) {
			return projectionEntity != null && !IsLoading;
		}
		public virtual string EntityDisplayName { get { return EntityDisplayNameHelper.GetEntityDisplayName(typeof(TEntity)); } }
		[Display(AutoGenerateField = false)]
		public virtual void Save(TProjection projectionEntity) {
			if(projectionEntity == null) {
				if(UnitOfWorkPolicy != UnitOfWorkPolicy.Shared)
					throw new InvalidOperationException("Saving without providing an entity is only supported with UnitOfWorkPolicy.Shared");
				Repository.UnitOfWork.SaveChanges();
				sharedChildChanged = false;
				Refresh();
				return;
			}
			var entity = Repository.FindExistingOrAddNewEntity(projectionEntity, ApplyProjectionPropertiesToEntity);
			try {
				OnBeforeEntitySaved(entity);
				Repository.UnitOfWork.SaveChanges();
				var primaryKey = Repository.GetPrimaryKey(entity);
				Repository.SetProjectionPrimaryKey(projectionEntity, primaryKey);
				OnEntitySaved(primaryKey, entity);
			} catch (DbException e) {
				OnDbException(e);
			}
		}
		public virtual bool CanSave(TProjection projectionEntity) {
			if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared)
				return sharedChildChanged;
			return projectionEntity != null && !IsLoading;
		}
		[Display(AutoGenerateField = false)]
		public virtual void Reset() {
			if(UnitOfWorkPolicy != UnitOfWorkPolicy.Shared)
				throw new InvalidOperationException("Reset is only supported with UnitOfWorkPolicy.Shared");
			MessageResult confirmationResult = MessageBoxService.ShowMessage(
				ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Reset), 
				ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Caption),
				MessageButton.OKCancel);
			if(confirmationResult == MessageResult.OK) {
				UnitOfWorkFactory.ResetShared();
				LoadEntities(false);
				sharedChildChanged = false;
			}
		}
		public virtual bool CanReset() {
			return UnitOfWorkPolicy == UnitOfWorkPolicy.Shared && sharedChildChanged;
		}
		[Display(AutoGenerateField = false)]
		public virtual void UpdateSelectedEntity() {
			this.RaisePropertyChanged(x => x.SelectedEntity);
		}
		[Display(AutoGenerateField = false)]
		public void Close() {
			if(DocumentOwner != null)
				DocumentOwner.Close(this);
		}
		protected override string ViewName { get { return typeof(TEntity).Name + "CollectionView"; } }
		protected IMessageBoxService MessageBoxService { get { return this.GetRequiredService<IMessageBoxService>(); } }
		protected IDocumentManagerService WindowedDocumentManagerService {
			get { return this.GetService<IDocumentManagerService>("WindowedDocumentUIService"); }
		}
		protected virtual void OnBeforeEntityDeleted(TPrimaryKey primaryKey, TEntity entity) { }
		protected virtual void OnEntityDeleted(TPrimaryKey primaryKey, TEntity entity) {
			Messenger.Default.Send(new EntityMessage<TEntity, TPrimaryKey>(primaryKey, EntityMessageType.Deleted));
		}
		protected override Func<TProjection> GetSelectedEntityCallback() {
			if(SelectedEntity == null)
				return null;
			var entity = SelectedEntity;
			return () => FindLocalProjectionWithSameKey(entity);
		}
		TProjection FindLocalProjectionWithSameKey(TProjection projectionEntity) {
			bool primaryKeyAvailable = projectionEntity != null && Repository.ProjectionHasPrimaryKey(projectionEntity);
			return primaryKeyAvailable ? ChangeTrackerWithKey.FindLocalProjectionByKey(Repository.GetProjectionPrimaryKey(projectionEntity)) : null;
		}
		protected virtual void OnBeforeEntitySaved(TEntity entity) { }
		protected virtual void OnEntitySaved(TPrimaryKey primaryKey, TEntity entity) {
			Messenger.Default.Send(new EntityMessage<TEntity, TPrimaryKey>(primaryKey, EntityMessageType.Changed));
		}
		protected virtual void ApplyProjectionPropertiesToEntity(TProjection projectionEntity, TEntity entity) {
			throw new NotImplementedException("Override this method in the collection view model class and apply projection properties to the entity so that it can be correctly saved by unit of work.");
		}
		protected override void OnSelectedEntityChanged() {
			base.OnSelectedEntityChanged();
			UpdateCommands();
		}
		protected override void RestoreSelectedEntity(TProjection existingProjectionEntity, TProjection newProjectionEntity) {
			base.RestoreSelectedEntity(existingProjectionEntity, newProjectionEntity);
			if(ReferenceEquals(SelectedEntity, existingProjectionEntity))
				SelectedEntity = newProjectionEntity;
		}
		protected override void OnIsLoadingChanged() {
			base.OnIsLoadingChanged();
			UpdateCommands();
			if(!IsLoading)
				RequestSelectedEntity();
		}
		void UpdateCommands() {
			TProjection projectionEntity = null;
			this.RaiseCanExecuteChanged(x => x.Edit(projectionEntity));
			this.RaiseCanExecuteChanged(x => x.Delete(projectionEntity));
			this.RaiseCanExecuteChanged(x => x.Save(projectionEntity));
		}
		protected void DestroyDocument(IDocument document) {
			if(document != null)
				document.Close();
		}
		protected IRepository<TEntity, TPrimaryKey> CreateRepository() {
			return (IRepository<TEntity, TPrimaryKey>)CreateReadOnlyRepository();
		}
		protected override IEntitiesChangeTracker CreateEntitiesChangeTracker() {
			return new EntitiesChangeTracker<TPrimaryKey>(this);
		}
		#region SelectEntityMessage
		protected class SelectEntityMessage {
			public SelectEntityMessage(TPrimaryKey primaryKey) {
				PrimaryKey = primaryKey;
			}
			public TPrimaryKey PrimaryKey { get; private set; }
		}
		protected class SelectedEntityRequest { }
		readonly bool ignoreSelectEntityMessage;
		void RegisterSelectEntityMessage() {
			if(!ignoreSelectEntityMessage)
				Messenger.Default.Register<SelectEntityMessage>(this, x => OnSelectEntityMessage(x));
		}
		void RequestSelectedEntity() {
			if(!ignoreSelectEntityMessage)
				Messenger.Default.Send(new SelectedEntityRequest());
		}
		void OnSelectEntityMessage(SelectEntityMessage message) {
			if(!IsLoaded)
				return;
			var projectionEntity = ChangeTrackerWithKey.FindActualProjectionByKey(message.PrimaryKey);
			if(projectionEntity == null) {
				FilterExpression = null;
				projectionEntity = ChangeTrackerWithKey.FindActualProjectionByKey(message.PrimaryKey);
			}
			SelectedEntity = projectionEntity;
		}
		public CollectionViewModelState SaveState() {
			return new CollectionViewModelState { UnitOfWorkPolicy = UnitOfWorkPolicy };
		}
		public void RestoreState(CollectionViewModelState state) {
			UnitOfWorkPolicy = state.UnitOfWorkPolicy;
		}
		public void SendRefreshCollectionsMessage() {
			Messenger.Default.Send(new RefreshCollectionMessage<TEntity>());
		}
		#endregion
		#region ISupportLogicalLayout
		bool ISupportLogicalLayout.CanSerialize {
			get { return true; }
		}
		IDocumentManagerService ISupportLogicalLayout.DocumentManagerService {
			get { return DocumentManagerService; }
		}
		IEnumerable<object> ISupportLogicalLayout.LookupViewModels {
			get { return null; }
		}
		#endregion
	}
}
