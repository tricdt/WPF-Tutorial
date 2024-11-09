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
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
namespace DevExpress.Mvvm.ViewModel {
	[POCOViewModel]
	public abstract class EntitiesViewModelBase<TEntity, TProjection, TUnitOfWork> : IEntitiesViewModel<TProjection>, ISupportUnitOfWorkPolicy, ISupportParentViewModel
		where TEntity : class
		where TProjection : class
		where TUnitOfWork : IUnitOfWork {
		#region inner classes
		protected interface IEntitiesChangeTracker {
			void RegisterMessageHandler();
			void UnregisterMessageHandler();
		}
		protected class EntitiesChangeTracker<TPrimaryKey> : IEntitiesChangeTracker {
			readonly EntitiesViewModelBase<TEntity, TProjection, TUnitOfWork> owner;
			ObservableCollection<TProjection> Entities { get { return owner.Entities; } }
			IRepository<TEntity, TPrimaryKey> Repository { get { return (IRepository<TEntity, TPrimaryKey>)owner.ReadOnlyRepository; } }
			public EntitiesChangeTracker(EntitiesViewModelBase<TEntity, TProjection, TUnitOfWork> owner) {
				this.owner = owner;
			}
			void IEntitiesChangeTracker.RegisterMessageHandler() {
				Messenger.Default.Register<EntityMessage<TEntity, TPrimaryKey>>(this, x => OnMessage(x));
			}
			void IEntitiesChangeTracker.UnregisterMessageHandler() {
				Messenger.Default.Unregister(this);
			}
			public TProjection FindLocalProjectionByKey(TPrimaryKey primaryKey) {
				var primaryKeyEqualsExpression = RepositoryExtensions.GetProjectionPrimaryKeyEqualsExpression<TEntity, TProjection, TPrimaryKey>(Repository, primaryKey);
				return Entities.AsQueryable().FirstOrDefault(primaryKeyEqualsExpression);
			}
			public TProjection FindActualProjectionByKey(TPrimaryKey primaryKey) {
				var projectionEntity = Repository.FindActualProjectionByKey(owner.Projection, primaryKey);
				if(projectionEntity != null && ExpressionHelper.IsFitEntity(Repository.Find(primaryKey), owner.GetFilterExpression())) {
					owner.OnEntitiesLoaded(GetUnitOfWork(Repository), new TProjection[] { projectionEntity });
					return projectionEntity;
				}
				return null;
			}
			void OnMessage(EntityMessage<TEntity, TPrimaryKey> message) {
				if(!owner.IsLoaded)
					return;
				switch(message.MessageType) {
					case EntityMessageType.Added:
						OnEntityAdded(message.PrimaryKey);
						break;
					case EntityMessageType.Changed:
						OnEntityChanged(message.PrimaryKey);
						break;
					case EntityMessageType.Deleted:
						OnEntityDeleted(message.PrimaryKey);
						break;
				}
			}
			void OnEntityAdded(TPrimaryKey primaryKey) {
				var projectionEntity = FindActualProjectionByKey(primaryKey);
				if(projectionEntity != null)
					Entities.Add(projectionEntity);
			}
			void OnEntityChanged(TPrimaryKey primaryKey) {
				var existingProjectionEntity = FindLocalProjectionByKey(primaryKey);
				var projectionEntity = FindActualProjectionByKey(primaryKey);
				if(projectionEntity == null) {
					Entities.Remove(existingProjectionEntity);
					return;
				}
				if(existingProjectionEntity != null) {
					Entities[Entities.IndexOf(existingProjectionEntity)] = projectionEntity;
					owner.RestoreSelectedEntity(existingProjectionEntity, projectionEntity);
					return;
				}
				OnEntityAdded(primaryKey);
			}
			void OnEntityDeleted(TPrimaryKey primaryKey) {
				Entities.Remove(FindLocalProjectionByKey(primaryKey));
			}
		}
		#endregion
		ObservableCollection<TProjection> entities = new ObservableCollection<TProjection>();
		CancellationTokenSource loadCancellationTokenSource;
		public IUnitOfWorkFactory<TUnitOfWork> UnitOfWorkFactory { get; set; }
		protected readonly Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc;
		protected Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> Projection { get; private set; }
		protected EntitiesViewModelBase(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection)
			: this(unitOfWorkFactory, getRepositoryFunc, projection, UnitOfWorkPolicy.Individual) { }
		protected EntitiesViewModelBase(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IReadOnlyRepository<TEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection,
			UnitOfWorkPolicy unitOfWorkPolicy)
		{
			this.getRepositoryFunc = getRepositoryFunc;
			this.Projection = projection;
			this.ChangeTracker = CreateEntitiesChangeTracker();
			UnitOfWorkPolicy = unitOfWorkPolicy;
			UnitOfWorkFactory = UnitOfWorkPolicy == UnitOfWorkPolicy.Shared ?
				unitOfWorkFactory.MakeShared() : unitOfWorkFactory;
			AllowSaveReset = UnitOfWorkPolicy == UnitOfWorkPolicy.Shared;
			if(!this.IsInDesignMode())
				OnInitializeInRuntime();
		}
		public virtual bool IsLoading { get; protected set; }
		public ObservableCollection<TProjection> Entities {
			get {
				if(!IsLoaded)
					LoadEntities(false);
				return entities;
			}
		}
		protected IEntitiesChangeTracker ChangeTracker { get; private set; }
		protected IReadOnlyRepository<TEntity> ReadOnlyRepository { get; private set; }
		protected bool IsLoaded { get { return ReadOnlyRepository != null; } }
		public bool AllowSaveReset { get; protected set; }
#if DEBUGTEST
		public bool AllowLoadEntitiesForTests = true;
#endif
		protected void LoadEntities(bool forceLoad) {
#if DEBUGTEST
			if(!AllowLoadEntitiesForTests)
				throw new InvalidOperationException("LoadEntities");
#endif
			if(forceLoad) {
				if(loadCancellationTokenSource != null)
					loadCancellationTokenSource.Cancel();
			} else if(IsLoading) {
				return;
			}
			loadCancellationTokenSource = LoadCore();
		}
		void CancelLoading() {
			if(loadCancellationTokenSource != null)
				loadCancellationTokenSource.Cancel();
			IsLoading = false;
		}
		CancellationTokenSource LoadCore() {
			IsLoading = true;
			var cancellationTokenSource = new CancellationTokenSource();
			var selectedEntityCallback = GetSelectedEntityCallback();
			var synchronizationContext = SynchronizationContext.Current != null ? null : DevExpress.Data.Helpers.SyncHelper.CaptureSynchronizationContextOrFail();
			var task = System.Threading.Tasks.Task.Factory.StartNew(() => {
				var repository = CreateReadOnlyRepository();
				var entities = new ObservableCollection<TProjection>(repository.GetFilteredEntities(GetFilterExpression(), Projection));
				OnEntitiesLoaded(GetUnitOfWork(repository), entities);
				return new Tuple<IReadOnlyRepository<TEntity>, ObservableCollection<TProjection>>(repository, entities);
			});
			Action<Task<Tuple<IReadOnlyRepository<TEntity>, ObservableCollection<TProjection>>>> continuation = t => {
				if(!t.IsFaulted) {
					ReadOnlyRepository = t.Result.Item1;
					entities = t.Result.Item2;
					this.RaisePropertyChanged(y => y.Entities);
					OnEntitiesAssigned(selectedEntityCallback);
				}
				IsLoading = false;
			};
			if(synchronizationContext == null) {
				task.ContinueWith(continuation, cancellationTokenSource.Token, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
			} else {
				task.ContinueWith(
					t => synchronizationContext.Post(x => continuation((Task<Tuple<IReadOnlyRepository<TEntity>, ObservableCollection<TProjection>>>)x), t),
					cancellationTokenSource.Token
				);
			}
			return cancellationTokenSource;
		}
		static TUnitOfWork GetUnitOfWork(IReadOnlyRepository<TEntity> repository) {
			return (TUnitOfWork)repository.UnitOfWork;
		}
		protected virtual void OnEntitiesLoaded(TUnitOfWork unitOfWork, IEnumerable<TProjection> entities) {
		}
		protected virtual void OnEntitiesAssigned(Func<TProjection> getSelectedEntityCallback) {
		}
		protected virtual Func<TProjection> GetSelectedEntityCallback() {
			return null;
		}
		protected virtual void RestoreSelectedEntity(TProjection existingProjectionEntity, TProjection projectionEntity) {
		}
		protected virtual Expression<Func<TEntity, bool>> GetFilterExpression() {
			return null;
		}
		protected virtual void OnInitializeInRuntime() {
			if(ChangeTracker != null)
				ChangeTracker.RegisterMessageHandler();
		}
		protected virtual void OnDestroy() {
			CancelLoading();
			if(ChangeTracker != null)
				ChangeTracker.UnregisterMessageHandler();
		}
		protected virtual void OnIsLoadingChanged() {
		}
		public UnitOfWorkPolicy UnitOfWorkPolicy { get; internal set; }
		protected IReadOnlyRepository<TEntity> CreateReadOnlyRepository() {
			return getRepositoryFunc(CreateUnitOfWork());
		}
		protected TUnitOfWork CreateUnitOfWork() {
			if (UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
				return UnitOfWorkFactory.CreateUnitOfWork();
			}
			return UnitOfWorkFactory.CreateUnitOfWork();
		}
		protected virtual IEntitiesChangeTracker CreateEntitiesChangeTracker() {
			return null;
		}
		protected IDocumentOwner DocumentOwner { get; private set; }
		#region IDocumentContent
		object IDocumentContent.Title { get { return null; } }
		protected virtual void OnClose(CancelEventArgs e) { }
		void IDocumentContent.OnClose(CancelEventArgs e) {
			OnClose(e);
		}
		void IDocumentContent.OnDestroy() {
			OnDestroy();
		}
		IDocumentOwner IDocumentContent.DocumentOwner {
			get { return DocumentOwner; }
			set { DocumentOwner = value; }
		}
		#endregion
		#region IEntitiesViewModel
		ObservableCollection<TProjection> IEntitiesViewModel<TProjection>.Entities { get { return Entities; } }
		bool IEntitiesViewModel<TProjection>.IsLoading { get { return IsLoading; } }
		#endregion
		public virtual object ParentViewModel { get; set; }
		protected void OnParentViewModelChanged() {
			var parent = ParentViewModel as ISupportUnitOfWorkPolicy;
			AllowSaveReset = (UnitOfWorkPolicy == UnitOfWorkPolicy.Shared &&
				(parent == null || parent.UnitOfWorkPolicy != UnitOfWorkPolicy.Shared));
		}
	}
	public interface IEntitiesViewModel<TEntity> : IDocumentContent where TEntity : class {
		ObservableCollection<TEntity> Entities { get; }
		bool IsLoading { get; }
	}
}
