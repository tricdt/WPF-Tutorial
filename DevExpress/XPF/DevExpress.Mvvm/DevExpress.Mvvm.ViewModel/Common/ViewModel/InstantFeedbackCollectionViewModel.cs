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
using System.Collections.ObjectModel;
using DevExpress.Data.Linq;
using System.Collections;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Localization;
using DevExpress.Mvvm.ViewModel;
namespace DevExpress.Mvvm.ViewModel {
	public partial class InstantFeedbackCollectionViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork> : InstantFeedbackCollectionViewModelBase<TEntity, TProjection, TPrimaryKey, TUnitOfWork>
		where TEntity : class, new()
		where TProjection : class
		where TUnitOfWork : IUnitOfWork {
		public static InstantFeedbackCollectionViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork> CreateInstantFeedbackCollectionViewModel(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection,
			Func<bool> canCreateNewEntity = null) {
			return ViewModelSource.Create(() => new InstantFeedbackCollectionViewModel<TEntity, TProjection, TPrimaryKey, TUnitOfWork>(unitOfWorkFactory, getRepositoryFunc, projection, canCreateNewEntity));
		}
		protected InstantFeedbackCollectionViewModel(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection,
			Func<bool> canCreateNewEntity = null)
			: base(unitOfWorkFactory, getRepositoryFunc, projection, canCreateNewEntity) {
		}
	}
	public class InstantFeedbackCollectionViewModelBase<TEntity, TProjection, TPrimaryKey, TUnitOfWork> : IDocumentContent, ISupportLogicalLayout
		where TEntity : class, new()
		where TProjection : class
		where TUnitOfWork : IUnitOfWork {
		#region inner classes
		public class InstantFeedbackSourceViewModel : IListSource {
			public static InstantFeedbackSourceViewModel Create(Func<int> getCount, IInstantFeedbackSource<TProjection> source) {
				return ViewModelSource.Create(() => new InstantFeedbackSourceViewModel(getCount, source));
			}
			readonly Func<int> getCount;
			readonly IInstantFeedbackSource<TProjection> source;
			protected InstantFeedbackSourceViewModel(Func<int> getCount, IInstantFeedbackSource<TProjection> source) {
				this.getCount = getCount;
				this.source = source;
			}
			public int Count { get { return getCount(); } }
			public void Refresh() {
				source.Refresh();
				this.RaisePropertyChanged(x => x.Count);
			}
			bool IListSource.ContainsListCollection { get { return source.ContainsListCollection; } }
			IList IListSource.GetList() {
				return source.GetList();
			}
		}
		#endregion
		protected readonly IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory;
		protected readonly Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc;
		protected Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> Projection { get; private set; }
		Func<bool> canCreateNewEntity;
		readonly IRepository<TEntity, TPrimaryKey> helperRepository;
		readonly IInstantFeedbackSource<TProjection> source;
		internal protected InstantFeedbackCollectionViewModelBase(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection = null,
			Func<bool> canCreateNewEntity = null) {
			this.unitOfWorkFactory = unitOfWorkFactory;
			this.canCreateNewEntity = canCreateNewEntity;
			this.getRepositoryFunc = getRepositoryFunc;
			this.Projection = projection;
			this.helperRepository = CreateRepository();
			RepositoryExtensions.VerifyProjection(helperRepository, projection);
			this.source = unitOfWorkFactory.CreateInstantFeedbackSource(getRepositoryFunc, Projection);
			this.Entities = InstantFeedbackSourceViewModel.Create(helperRepository.Count, source);
			if(!this.IsInDesignMode())
				OnInitializeInRuntime();
			Messenger.Default.Register<CloseAllMessage>(this, m => {
				if (m.ShouldProcess(this)) {
					SaveLayout();
				}
			});
		}
		public InstantFeedbackSourceViewModel Entities { get; private set; }
		public virtual object SelectedEntity { get; set; }
		public virtual void New() {
			if(canCreateNewEntity != null && !canCreateNewEntity())
				return;
			DocumentManagerService.ShowNewEntityDocument<TEntity>(this);
		}
		public virtual void Edit(object threadSafeProxy) {
			if(!source.IsLoadedProxy(threadSafeProxy))
				return;
			TPrimaryKey primaryKey = GetProxyPrimaryKey(threadSafeProxy);
			TEntity entity = helperRepository.Find(primaryKey);
			if(entity == null) {
				DestroyDocument(DocumentManagerService.FindEntityDocument<TEntity, TPrimaryKey>(primaryKey));
				return;
			}
			var parameter = new ViewModelInitInfo {
				PrimaryKey = primaryKey
			};
			DocumentManagerService.ShowExistingEntityDocument<TEntity, TPrimaryKey>(this, primaryKey, parameter);
		}
		public virtual bool CanEdit(object threadSafeProxy) {
			return threadSafeProxy != null;
		}
		public virtual void Delete(object threadSafeProxy) {
			if(!source.IsLoadedProxy(threadSafeProxy))
				return;
			if(MessageBoxService.ShowMessage(string.Format(ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Delete), EntityDisplayNameHelper.GetEntityDisplayName(typeof(TEntity))),
				ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Caption), MessageButton.YesNo) != MessageResult.Yes)
				return;
			try {
				TPrimaryKey primaryKey = GetProxyPrimaryKey(threadSafeProxy);
				TEntity entity = helperRepository.Find(primaryKey);
				if(entity != null) {
					OnBeforeEntityDeleted(primaryKey, entity);
					helperRepository.Remove(entity);
					helperRepository.UnitOfWork.SaveChanges();
					OnEntityDeleted(primaryKey, entity);
				}
			} catch (DbException e) {
				Refresh();
				OnDbException(e);
			}
			Refresh();
		}
		protected virtual void OnDbException(DbException e) {
			MessageBoxService.ShowMessage(e.ErrorMessage, e.ErrorCaption, MessageButton.OK, MessageIcon.Error);
		}
		public virtual bool CanDelete(object threadSafeProxy) {
			return threadSafeProxy != null;
		}
		protected ILayoutSerializationService LayoutSerializationService { get { return this.GetService<ILayoutSerializationService>(); } }
		string ViewName { get { return typeof(TEntity).Name + "InstantFeedbackCollectionView"; } }
		[Display(AutoGenerateField = false)]
		public virtual void OnLoaded() {
			PersistentLayoutHelper.TryDeserializeLayout(LayoutSerializationService, ViewName);
		}
		[Display(AutoGenerateField = false)]
		public virtual void OnUnloaded() {
			SaveLayout();
		}
		void SaveLayout() {
			PersistentLayoutHelper.TrySerializeLayout(LayoutSerializationService, ViewName);
		}
		public virtual void Refresh() {
			Entities.Refresh();
		}
		protected TPrimaryKey GetProxyPrimaryKey(object threadSafeProxy) {
			var expression = RepositoryExtensions.GetSinglePropertyPrimaryKeyProjectionProperty<TEntity, TProjection, TPrimaryKey>(helperRepository);
			return GetProxyPropertyValue(threadSafeProxy, expression);
		}
		protected TProperty GetProxyPropertyValue<TProperty>(object threadSafeProxy, Expression<Func<TProjection, TProperty>> propertyExpression) {
			return source.GetPropertyValue(threadSafeProxy, propertyExpression);
		}
		protected virtual void OnEntityDeleted(TPrimaryKey primaryKey, TEntity entity) {
			Messenger.Default.Send(new EntityMessage<TEntity, TPrimaryKey>(primaryKey, EntityMessageType.Deleted));
		}
		protected virtual IMessageBoxService MessageBoxService { get { return this.GetRequiredService<IMessageBoxService>(); } }
		protected virtual IDocumentManagerService DocumentManagerService { get { return this.GetService<IDocumentManagerService>(); } }
		protected virtual void OnBeforeEntityDeleted(TPrimaryKey primaryKey, TEntity entity) { }
		protected void DestroyDocument(IDocument document) {
			if(document != null)
				document.Close();
		}
		protected IRepository<TEntity, TPrimaryKey> CreateRepository() {
			return getRepositoryFunc(CreateUnitOfWork());
		}
		protected TUnitOfWork CreateUnitOfWork() {
			return unitOfWorkFactory.CreateUnitOfWork();
		}
		protected virtual void OnInitializeInRuntime() {
			Messenger.Default.Register<EntityMessage<TEntity, TPrimaryKey>>(this, x => OnMessage(x));
		}
		protected virtual void OnDestroy() {
			Messenger.Default.Unregister(this);
		}
		void OnMessage(EntityMessage<TEntity, TPrimaryKey> message) {
			Refresh();
		}
		protected IDocumentOwner DocumentOwner { get; private set; }
		#region IDocumentContent
		object IDocumentContent.Title { get { return null; } }
		void IDocumentContent.OnClose(CancelEventArgs e) {
			SaveLayout();
		}
		void IDocumentContent.OnDestroy() {
			OnDestroy();
		}
		IDocumentOwner IDocumentContent.DocumentOwner {
			get { return DocumentOwner; }
			set { DocumentOwner = value; }
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
