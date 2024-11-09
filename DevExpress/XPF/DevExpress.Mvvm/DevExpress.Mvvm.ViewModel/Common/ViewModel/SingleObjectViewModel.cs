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
using DevExpress.Mvvm.ViewModel;
using System.Reflection;
using System.Diagnostics;
namespace DevExpress.Mvvm.ViewModel {
	public class SingleObjectViewModelState {
		public object[] Key { get; set; }
		public string Title { get; set; }
		public UnitOfWorkPolicy UnitOfWorkPolicy { get; set; }
		public bool AllowSaveReset { get; set; }
		public bool IsSharedTreeRoot { get; set; }
	}
	interface ISupportUnitOfWorkPolicy {
		UnitOfWorkPolicy UnitOfWorkPolicy { get; }
	}
	[POCOViewModel]
	public abstract class SingleObjectViewModelBase<TEntity, TPrimaryKey, TUnitOfWork> 
		: ISingleObjectViewModel<TEntity, TPrimaryKey>, 
		ISupportParameter, 
		IDocumentContent, 
		ISupportLogicalLayout<SingleObjectViewModelState>, 
		ISupportUnitOfWorkPolicy, 
		ISupportParentViewModel
		where TEntity : class
		where TUnitOfWork : IUnitOfWork {
		enum ViewModelEntityState {
			New = 1,
			Changed = 2,
			ExistingUnchanged = 0
		}
		class DependentPropertyInfo {
			public PropertyInfo info;
			public PropertyInfo[] foreignKey;
		}
		class LookupViewModelInfo {
			public PropertyInfo property;
			public IDocumentContent viewModel;
			public List<DependentPropertyInfo> dependentProperties = new List<DependentPropertyInfo>();
		}
		class LookupRegistry {
			readonly List<LookupViewModelInfo> infos = new List<LookupViewModelInfo>();
			public LookupViewModelInfo Get(PropertyInfo collectionProperty) {
				var info = infos.FirstOrDefault(x => x.property == collectionProperty);
				if(info == null) {
					info = new LookupViewModelInfo();
					infos.Add(info);
				}
				return info;
			}
			public IEnumerable<LookupViewModelInfo> GetAll() {
				return infos.ToArray();
			}
		}
		object title;
		protected readonly Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc;
		protected readonly Func<TEntity, object> getEntityDisplayNameFunc;
		Action<TEntity> entityInitializer;
#if DEBUGTEST
		string PrintEntityState() { return entityState.ToString(); }
#endif
		ViewModelEntityState entityState = ViewModelEntityState.ExistingUnchanged;
		readonly LookupRegistry lookups = new LookupRegistry();
		public virtual bool AllowSaveReset { get; protected set; }
		internal virtual bool IsSharedTreeRoot { get; set; }
		protected SingleObjectViewModelBase(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, Func<TEntity, object> getEntityDisplayNameFunc) {
			UnitOfWorkFactory = unitOfWorkFactory;
			this.getRepositoryFunc = getRepositoryFunc;
			this.getEntityDisplayNameFunc = getEntityDisplayNameFunc;
			UpdateUnitOfWork();
			if(this.IsInDesignMode())
				this.Entity = this.Repository.FirstOrDefault();
			else
				OnInitializeInRuntime();
			var inpc = (INotifyPropertyChanged)this;
			inpc.PropertyChanged += OnPropertyChanged;
			UnitOfWorkPolicy = UnitOfWorkPolicy.Individual;
			AllowSaveReset = true;
			IsSharedTreeRoot = false;
			Messenger.Default.Register<SharedUnitOfWorkCollectionChangedMessage>(this, OnChildSharedUnitOfWorkSingleClosed);
		}
		void OnChildSharedUnitOfWorkSingleClosed(SharedUnitOfWorkCollectionChangedMessage message) {
			if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared && lookups.GetAll().Any(x => x.viewModel == message.ViewModel)) {
				entityState |= ViewModelEntityState.Changed;
				UpdateTitle();
			}
		}
		UnitOfWorkPolicy unitOfWorkPolicy;
#if DEBUGTEST
		public
#else
		internal
#endif
		UnitOfWorkPolicy UnitOfWorkPolicy {
			get { return unitOfWorkPolicy; }
			set {
				if(unitOfWorkPolicy != value) {
					unitOfWorkPolicy = value;
					this.RaisePropertyChanged(x => x.UnitOfWorkPolicy);
				}
			}
		}
		UnitOfWorkPolicy ISupportUnitOfWorkPolicy.UnitOfWorkPolicy { get { return UnitOfWorkPolicy; } }
		public object Title { get { return title; } }
		public virtual TEntity Entity { get; protected set; }
		[Display(AutoGenerateField = false)]
		public void Update() {
			UpdateTitle();
			UpdateCommands();
			if (!dontUpdateEntityState && Entity != null) {
				entityState |= ViewModelEntityState.Changed;
				Repository.Update(Entity);
		   }
		}
		public virtual void Save() {
			SaveCore();
		}
		public virtual bool CanSave() {
			return Entity != null && !HasValidationErrors() && NeedSave();
		}
		[Command(CanExecuteMethodName = "CanSave")]
		public void SaveAndClose() {
			if(SaveCore())
				Close();
		}
		[Command(CanExecuteMethodName = "CanSave")]
		public void SaveAndNew() {
			if(SaveCore())
				CreateAndInitializeEntity(this.entityInitializer);
		}
		[Display(Name = "Reset Changes")]
		public virtual void Reset() {
			MessageResult confirmationResult = MessageBoxService.ShowMessage(ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Reset), ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Caption), MessageButton.OKCancel);
			if(confirmationResult == MessageResult.OK) {
				if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
					if (IsNew()) {
						 CreateAndInitializeEntity(this.entityInitializer);
					} else {
						entityState = ViewModelEntityState.ExistingUnchanged;
						UnitOfWorkFactory.ResetShared();
						UnitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
						RefreshLookUpCollections(true);
						UpdateTitle();
					}
				} else {
					Reload();
				}
			}
		}
		public bool CanReset() {
			return NeedReset();
		}
		string ViewName { get { return typeof(TEntity).Name + "View"; } }
		public virtual string EntityDisplayName { get { return EntityDisplayNameHelper.GetEntityDisplayName(typeof(TEntity)); } }
		[DXImage("Save")]
		[Display(Name = "Save Layout")]
		public void SaveLayout() {
			PersistentLayoutHelper.TrySerializeLayout(LayoutSerializationService, ViewName);
			PersistentLayoutHelper.SaveLayout();
		}
		[DXImage("Reset")]
		[Display(Name = "Reset Layout")]
		public void ResetLayout() {
			if(LayoutSerializationService != null)
				LayoutSerializationService.Deserialize(null);
		}
		[Display(AutoGenerateField = false)]
		public virtual void OnLoaded() {
			PersistentLayoutHelper.TryDeserializeLayout(LayoutSerializationService, ViewName);
		}
		public virtual void Delete() {
			if(MessageBoxService.ShowMessage(string.Format(ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Delete), EntityDisplayName), GetConfirmationMessageTitle(), MessageButton.YesNo) != MessageResult.Yes)
				return;
			try {
				OnBeforeEntityDeleted(PrimaryKey, Entity);
				Repository.Remove(Entity);
				UnitOfWork.SaveChanges();
				TPrimaryKey primaryKeyForMessage = PrimaryKey;
				TEntity entityForMessage = Entity;
				Entity = null;
				OnEntityDeleted(primaryKeyForMessage, entityForMessage);
				Close();
			} catch (DbException e) {
				OnDbException(e);
			}
		}
		public virtual bool CanDelete() {
			return Entity != null && !IsNew();
		}
		public void Close() {
			if(!TryClose())
				return;
			if(IsNew()) {
				Entity = null;
			}
			if(DocumentOwner != null)
				DocumentOwner.Close(this);
		}
		public virtual IUnitOfWorkFactory<TUnitOfWork> UnitOfWorkFactory { get; set; }
		protected TUnitOfWork UnitOfWork { get; private set; }
		protected virtual bool SaveCore() {
			try {
				bool isNewEntity = IsNew();
				OnBeforeEntitySaved(PrimaryKey, Entity, isNewEntity);
				UnitOfWork.SaveChanges();
				entityState = ViewModelEntityState.ExistingUnchanged;
				if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
					title = GetTitle(false);
					this.RaisePropertyChanged(x => x.Title);
					foreach(var detail in lookups.GetAll().Select(x => x.viewModel).OfType<ICollectionViewModel>()) {
						detail.SendRefreshCollectionsMessage();
					}
					UpdateCommands();
				} else {
					LoadEntityByKey(Repository.GetPrimaryKey(Entity));
				}
				if (isNewEntity) {
					PrimaryKey = Repository.GetPrimaryKey(Entity);
				}
				OnEntitySaved(PrimaryKey, Entity, isNewEntity);
				this.RaisePropertyChanged(x => x.IsPrimaryKeyReadOnly);
				return true;
			} catch (DbException e) {
				OnDbException(e);
				return false;
			}
		}
		protected virtual void OnDbException(DbException e) {
			MessageBoxService.ShowMessage(e.ErrorMessage, e.ErrorCaption, MessageButton.OK, MessageIcon.Error);
		}
		protected virtual void OnBeforeEntitySaved(TPrimaryKey primaryKey, TEntity entity, bool isNewEntity) { }
		protected virtual void OnEntitySaved(TPrimaryKey primaryKey, TEntity entity, bool isNewEntity) {
			Messenger.Default.Send(new EntityMessage<TEntity, TPrimaryKey>(primaryKey, isNewEntity ? EntityMessageType.Added : EntityMessageType.Changed, this));
		}
		protected virtual void OnBeforeEntityDeleted(TPrimaryKey primaryKey, TEntity entity) { }
		protected virtual void OnEntityDeleted(TPrimaryKey primaryKey, TEntity entity) {
			Messenger.Default.Send(new EntityMessage<TEntity, TPrimaryKey>(primaryKey, EntityMessageType.Deleted));
		}
		protected virtual void OnInitializeInRuntime() {
			Messenger.Default.Register<EntityMessage<TEntity, TPrimaryKey>>(this, x => OnEntityMessage(x));
			Messenger.Default.Register<SaveAllMessage>(this, x => Save());
			Messenger.Default.Register<CloseAllMessage>(this, m => {
				if(m.ShouldProcess(this)) {
					OnClosing(m);
				}
			});
		}
		protected virtual void OnEntityMessage(EntityMessage<TEntity, TPrimaryKey> message) {
			if(Entity == null) return;
			if(message.MessageType == EntityMessageType.Deleted && object.Equals(message.PrimaryKey, PrimaryKey))
				Close();
		}
		protected virtual void OnEntityChanged() {
			if(Entity != null && Repository.HasPrimaryKey(Entity)) {
				PrimaryKey = Repository.GetPrimaryKey(Entity);
				RefreshLookUpCollections(true);
			}
			Update();
		}
		protected IRepository<TEntity, TPrimaryKey> Repository { get { return getRepositoryFunc(UnitOfWork); } }
		protected TPrimaryKey PrimaryKey { get; private set; }
		protected IMessageBoxService MessageBoxService { get { return this.GetRequiredService<IMessageBoxService>(); } }
		protected ILayoutSerializationService LayoutSerializationService { get { return this.GetService<ILayoutSerializationService>(); } }
		ViewModelInitInfo AdaptParameter(object parameter) {
			if(parameter == null)
				return null;
			if(parameter is ViewModelInitInfo)
				return (ViewModelInitInfo)parameter;
			if(parameter is IUnitOfWorkFactory<TUnitOfWork>)
				return new ViewModelInitInfo { UnitOfWorkFactory = parameter };
			if(parameter is Action<TEntity>)
				return new ViewModelInitInfo { NewEntityInitializer = parameter };
			if(parameter is TPrimaryKey)
				return new ViewModelInitInfo { PrimaryKey = parameter };
			return null;
		}
		protected virtual void OnParameterChanged(object parameter) {
			var initInfo = AdaptParameter(parameter);
			if(initInfo == null)
				return;
			UnitOfWorkPolicy = initInfo.UnitOfWorkPolicy;
			var factory = (IUnitOfWorkFactory<TUnitOfWork>)initInfo.UnitOfWorkFactory ?? UnitOfWorkFactory;
			UnitOfWorkFactory = initInfo.UnitOfWorkPolicy == UnitOfWorkPolicy.Shared ? factory.MakeShared() : factory;
			IsSharedTreeRoot = initInfo.IsSharedTreeRoot;
			UpdateUnitOfWork();
			UpdateAllowSaveReset();
			if(initInfo.NewEntity != null) {
				Entity = (TEntity)initInfo.NewEntity;
				entityState = ViewModelEntityState.New;
			} else if(initInfo.PrimaryKey != null) {
				LoadEntityByKey((TPrimaryKey)initInfo.PrimaryKey);
				entityInitializer = (Action<TEntity>)initInfo.NewEntityInitializer;
			} else {
				CreateAndInitializeEntity((Action<TEntity>)initInfo.NewEntityInitializer);
			}
		}
		protected virtual TEntity CreateEntity() {
			return Repository.Create();
		}
		protected void Reload() {
			if(Entity == null || IsNew())
				CreateAndInitializeEntity(this.entityInitializer);
			else
				LoadEntityByKey(PrimaryKey);
		}
		protected void CreateAndInitializeEntity(Action<TEntity> entityInitializer) {
			entityState = ViewModelEntityState.New;
			UpdateUnitOfWork();
			this.entityInitializer = entityInitializer;
			var entity = CreateEntity();
			if(this.entityInitializer != null)
				this.entityInitializer(entity);
			dontUpdateEntityState = true;
			Entity = entity;
			dontUpdateEntityState = false;
		}
		bool dontUpdateEntityState = false;
		protected void LoadEntityByKey(TPrimaryKey primaryKey) {
			entityState = ViewModelEntityState.ExistingUnchanged;
			UpdateUnitOfWork();
			dontUpdateEntityState = true;
			Entity = Repository.Find(primaryKey);
			dontUpdateEntityState = false;
		}
		internal void UpdateUnitOfWork() {
			UnitOfWork = UnitOfWorkFactory.CreateUnitOfWork();
		}
		void UpdateTitle(string nullEntityValue = null) {
			if(Entity == null) {
				title = nullEntityValue;
			} else if(IsNew()) {
				title = GetTitleForNewEntity();
			} else {
				var modified = (entityState & ViewModelEntityState.Changed) == ViewModelEntityState.Changed 
					|| GetState() == EntityState.Modified;
				title = GetTitle(modified);
			}
			this.RaisePropertyChanged(x => x.Title);
		}
		protected virtual void UpdateCommands() {
			this.RaiseCanExecuteChanged(x => x.Save());
			this.RaiseCanExecuteChanged(x => x.SaveAndClose());
			this.RaiseCanExecuteChanged(x => x.SaveAndNew());
			this.RaiseCanExecuteChanged(x => x.Delete());
			this.RaiseCanExecuteChanged(x => x.Reset());
		}
		protected IDocumentOwner DocumentOwner { get; private set; }
		protected virtual void OnDestroy() {
			Messenger.Default.Unregister(this);
			RefreshLookUpCollections(false);
		}
		protected virtual bool TryClose() {
			if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared && !IsSharedTreeRoot) {
				Messenger.Default.Send(
					new SharedUnitOfWorkCollectionChangedMessage {
						Entity = Entity,
						ViewModel = this
					});
				return true;
			}
			if(HasValidationErrors() && (entityState & ViewModelEntityState.Changed) == ViewModelEntityState.Changed) {
				MessageResult warningResult = MessageBoxService.ShowMessage(
					ScaffoldingLocalizer.GetString(ScaffoldingStringId.Warning_SomeFieldsContainInvalidData), 
					ScaffoldingLocalizer.GetString(ScaffoldingStringId.Warning_Caption), 
					MessageButton.OKCancel);
				return warningResult == MessageResult.OK;
			}
			if(!NeedReset()) return true;
			MessageResult result = MessageBoxService.ShowMessage(
				ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Save), 
				GetConfirmationMessageTitle(),
				MessageButton.YesNoCancel);
			if(result == MessageResult.Yes)
				return SaveCore();
			if(result == MessageResult.No)
				Reload();
			return result != MessageResult.Cancel;
		}
		protected virtual void OnClosing(CloseAllMessage message) {
			if(!message.Cancel)
				message.Cancel = !TryClose();
		}
		protected virtual string GetConfirmationMessageTitle() {
			return GetTitle();
		}
		public bool IsNew() {
			return Entity != null && ((entityState & ViewModelEntityState.New) == ViewModelEntityState.New);
		}
		public bool IsPrimaryKeyReadOnly {
			get { return !IsNew(); }
		}
		protected virtual bool NeedSave() {
			if(Entity == null)
				return false;
			if((entityState & ViewModelEntityState.New) == ViewModelEntityState.New)
				return true;
			if((entityState & ViewModelEntityState.Changed) == ViewModelEntityState.Changed)
				return true;
			return GetState() == EntityState.Modified;
		}
		protected virtual bool NeedReset() {
			var isNew = (entityState & ViewModelEntityState.New) == ViewModelEntityState.New;
			var isModified = (entityState & ViewModelEntityState.Changed) == ViewModelEntityState.Changed;
			return NeedSave() && !(isNew && !isModified);
		}
		protected virtual bool HasValidationErrors() {
			IDataErrorInfo dataErrorInfo = Entity as IDataErrorInfo;
			return dataErrorInfo != null && IDataErrorInfoHelper.HasErrors(dataErrorInfo);
		}
		string GetTitle(bool entityModified) {
			return GetTitle() + (entityModified ? ScaffoldingLocalizer.GetString(ScaffoldingStringId.Entity_Changed) : string.Empty);
		}
		protected virtual string GetTitleForNewEntity() {
			return EntityDisplayName + ScaffoldingLocalizer.GetString(ScaffoldingStringId.Entity_New);
		}
		protected virtual string GetTitle() {
			string entityText = Convert.ToString(getEntityDisplayNameFunc != null ? getEntityDisplayNameFunc(Entity) : PrimaryKey);
			return (EntityDisplayName + (string.IsNullOrEmpty(entityText) ? null : " - " + entityText))
			.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
		}
		protected EntityState GetState() {
			return Repository.GetState(Entity);
		}
#region look up and detail view models
		protected virtual void RefreshLookUpCollections(bool raisePropertyChanged) {
			if(this.IsInDesignMode()) return;
			foreach(var item in lookups.GetAll()) {
				if (item.viewModel != null) {
					var inpt = item.viewModel as INotifyPropertyChanged;
					if (inpt != null) {
						inpt.PropertyChanged -= OnLookupCollectionPropertyChanged;
					}
					item.viewModel.OnDestroy();
				}
				item.viewModel = null;
				if(raisePropertyChanged)
					((IPOCOViewModel)this).RaisePropertyChanged(item.property.Name);
			}
			foreach(var lookup in lookups.GetAll()) {
				foreach(var dep in lookup.dependentProperties) {
					var depPropertyValue = dep.info.GetValue(this, null);
					OnLookupCollectionPropertyChanged(depPropertyValue, new PropertyChangedEventArgs("Entities"));
				}
			}
		}
		protected AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>
			CreateAddRemoveDetailEntitiesViewModel<TDetailEntity, TDetailPrimaryKey>(Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc, Func<TEntity, ICollection<TDetailEntity>> getDetailsFunc)
			where TDetailEntity : class {
			var viewModel = CreateAddRemoveDetailEntitiesViewModelCore(getDetailsRepositoryFunc, getDetailsFunc);
			viewModel.SetParentViewModel(this);
			return viewModel;
		}
		protected virtual AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> CreateAddRemoveDetailEntitiesViewModelCore<TDetailEntity, TDetailPrimaryKey>(Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc, Func<TEntity, ICollection<TDetailEntity>> getDetailsFunc) where TDetailEntity : class {
			return AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>.Create(UnitOfWorkFactory, this.getRepositoryFunc, getDetailsRepositoryFunc, getDetailsFunc, PrimaryKey);
		}
		protected AddRemoveJunctionDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey, TUnitOfWork>
			CreateAddRemoveJunctionDetailEntitiesViewModel<TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey>(
				Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc,
				Func<TUnitOfWork, IRepository<TJunction, TJunctionPrimaryKey>> getJunctionRepositoryFunc,
				Expression<Func<TJunction, TPrimaryKey>> getEntityForeignKey,
				Expression<Func<TJunction, TDetailPrimaryKey>> getDetailForeignKey)
			where TDetailEntity : class
			where TJunction : class, new()
			where TJunctionPrimaryKey : class {
			var viewModel = CreateAddRemoveJunctionDetailEntitiesViewModelCore(getDetailsRepositoryFunc, getJunctionRepositoryFunc, getEntityForeignKey, getDetailForeignKey);
			viewModel.SetParentViewModel(this);
			return viewModel;
		}
		protected virtual AddRemoveJunctionDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey, TUnitOfWork>
			CreateAddRemoveJunctionDetailEntitiesViewModelCore<TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey>(
				Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc,
				Func<TUnitOfWork, IRepository<TJunction, TJunctionPrimaryKey>> getJunctionRepositoryFunc,
				Expression<Func<TJunction, TPrimaryKey>> getEntityForeignKey,
				Expression<Func<TJunction, TDetailPrimaryKey>> getDetailForeignKey)
			where TDetailEntity : class
			where TJunction : class, new()
			where TJunctionPrimaryKey : class {
			return AddRemoveJunctionDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey, TUnitOfWork>
							.CreateViewModel(UnitOfWorkFactory, this.getRepositoryFunc, getDetailsRepositoryFunc, getJunctionRepositoryFunc, getEntityForeignKey, getDetailForeignKey, PrimaryKey);
		}
		protected virtual CollectionViewModelBase<TDetailEntity, TDetailProjection, TDetailPrimaryKey, TUnitOfWork>
			CreateDetailsCollectionViewModel<TDetailEntity, TDetailProjection, TDetailPrimaryKey>(
				Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
				Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailProjection>> predicate,
				Action<TDetailEntity> newEntityInitializer,
				Func<bool> canCreateNewEntity,
				bool ignoreSelectEntityMessage,
				UnitOfWorkPolicy unitOfWorkPolicy)
			where TDetailEntity : class
			where TDetailProjection : class {
			return ViewModelSource.Create(() => new CollectionViewModelBase<TDetailEntity, TDetailProjection, TDetailPrimaryKey, TUnitOfWork>(
				unitOfWorkPolicy == UnitOfWorkPolicy.Shared ? UnitOfWorkFactory.MakeShared() : UnitOfWorkFactory, 
				getRepositoryFunc, 
				predicate, 
				newEntityInitializer, 
				() => UnitOfWorkPolicy == UnitOfWorkPolicy.Shared || canCreateNewEntity(), 
				true, 
				unitOfWorkPolicy == UnitOfWorkPolicy.Shared ? UnitOfWorkPolicy.Shared : UnitOfWorkPolicy));
		}
		protected CollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> GetDetailsCollectionViewModel<TViewModel, TDetailEntity, TDetailPrimaryKey, TForeignKey>(
			Expression<Func<TViewModel, CollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>>> propertyExpression,
			Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Action<TDetailEntity, TPrimaryKey> setMasterEntityKeyAction,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailEntity>> projection = null) where TDetailEntity : class {
			return GetCollectionViewModelCore<CollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>, TDetailEntity, TDetailEntity, TForeignKey>(propertyExpression,
				() => CreateDetailsCollectionViewModel<TDetailEntity, TDetailEntity, TDetailPrimaryKey>(
					getRepositoryFunc,
					AppendForeignKeyPredicate(foreignKeyExpression, projection),
					CreateForeignKeyPropertyInitializer(setMasterEntityKeyAction, () => PrimaryKey),
					CanCreateNewEntity,
					true,
					UnitOfWorkPolicy));
		}
		protected CollectionViewModelBase<TDetailEntity, TDetailProjection, TDetailPrimaryKey, TUnitOfWork> GetDetailProjectionsCollectionViewModel<TViewModel, TDetailEntity, TDetailProjection, TDetailPrimaryKey, TForeignKey>(
			Expression<Func<TViewModel, CollectionViewModelBase<TDetailEntity, TDetailProjection, TDetailPrimaryKey, TUnitOfWork>>> propertyExpression,
			Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Action<TDetailEntity, TPrimaryKey> setMasterEntityKeyAction,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailProjection>> projection = null) where TDetailEntity : class where TDetailProjection : class {
			return GetCollectionViewModelCore<CollectionViewModelBase<TDetailEntity, TDetailProjection, TDetailPrimaryKey, TUnitOfWork>, TDetailEntity, TDetailProjection, TForeignKey>(propertyExpression,
				() => CreateDetailsCollectionViewModel<TDetailEntity, TDetailProjection, TDetailPrimaryKey>(
					getRepositoryFunc, 
					AppendForeignKeyPredicate(foreignKeyExpression, projection), 
					CreateForeignKeyPropertyInitializer(setMasterEntityKeyAction, () => PrimaryKey), 
					CanCreateNewEntity,
					true,
					UnitOfWorkPolicy));
		}
		protected CollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> GetDetailsCollectionViewModel<TViewModel, TDetailEntity, TDetailPrimaryKey, TForeignKey>(
				Expression<Func<TViewModel, CollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>>> propertyExpression,
				Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
				Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
				Expression<Func<TDetailEntity, TEntity>> navigationExpression,
				Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailEntity>> projection = null,
				UnitOfWorkPolicy unitOfWorkPolicy = UnitOfWorkPolicy.Individual) where TDetailEntity : class {
			var setKey = ExpressionHelper.GetSetKeyUntypedAction(foreignKeyExpression);
			var setNavigation = ExpressionHelper.GetSetKeyUntypedAction(navigationExpression);
			return GetCollectionViewModelCore<CollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>, TDetailEntity, TDetailEntity, TForeignKey>(propertyExpression,
				() => CreateDetailsCollectionViewModel<TDetailEntity, TDetailEntity, TDetailPrimaryKey>(
					getRepositoryFunc,
					AppendForeignKeyPredicate(foreignKeyExpression, projection),
					detail => {
						setKey(detail, (TForeignKey)(object)PrimaryKey);
						if(UnitOfWorkPolicy == UnitOfWorkPolicy.Shared) {
							if(setNavigation == null)
								throw new InvalidOperationException("setMasterEntityKeyAction can not be null with UnitOfWorkPolicy.Shared");
							setNavigation(detail, Entity);
						}
					},
					CanCreateNewEntity,
					true,
					unitOfWorkPolicy));
		}
		protected virtual InstantFeedbackCollectionViewModel<TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey, TUnitOfWork>
			CreateDetailsInstantFeedbackCollectionViewModel<TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey>(
				Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
				Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailEntityProjection>> predicate)
			where TDetailEntity : class, new()
			where TDetailEntityProjection : class, new() {
			return InstantFeedbackCollectionViewModel<TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey, TUnitOfWork>
				.CreateInstantFeedbackCollectionViewModel(
					UnitOfWorkFactory,
					getRepositoryFunc,
					predicate,
					CanCreateNewEntity);
		}
		protected InstantFeedbackCollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> GetDetailsInstantFeedbackCollectionViewModel<
			TViewModel, TDetailEntity, TDetailPrimaryKey, TForeignKey>(
			Expression<Func<TViewModel, InstantFeedbackCollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>>> propertyExpression,
			Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Action<TDetailEntity, TPrimaryKey> setMasterEntityKeyAction,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailEntity>> projection = null)
			where TDetailEntity : class, new() {
			var predicate = AppendForeignKeyPredicate(foreignKeyExpression, projection);
			return GetCollectionViewModelCore<
				InstantFeedbackCollectionViewModelBase<TDetailEntity, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>, 
				TDetailEntity, 
				TDetailEntity, 
				TForeignKey>(propertyExpression, () => CreateDetailsInstantFeedbackCollectionViewModel(getRepositoryFunc, predicate));
		}
		protected InstantFeedbackCollectionViewModel<TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey, TUnitOfWork> GetDetailsInstantFeedbackCollectionViewModel<
			TViewModel, TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey, TForeignKey>(
			Expression<Func<TViewModel, InstantFeedbackCollectionViewModel<TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey, TUnitOfWork>>> propertyExpression,
			Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getRepositoryFunc,
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Action<TDetailEntity, TPrimaryKey> setMasterEntityKeyAction,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailEntityProjection>> projection = null)
			where TDetailEntity : class, new()
			where TDetailEntityProjection : class, new() {
			var predicate = AppendForeignKeyPredicate(foreignKeyExpression, projection);
			return GetCollectionViewModelCore<
				InstantFeedbackCollectionViewModel<TDetailEntity, TDetailEntityProjection, TDetailPrimaryKey, TUnitOfWork>,
				TDetailEntity,
				TDetailEntityProjection,
				TForeignKey>(propertyExpression, () => CreateDetailsInstantFeedbackCollectionViewModel(getRepositoryFunc, predicate));
		}
		protected virtual ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailProjection, TUnitOfWork> CreateDetailsReadOnlyCollectionViewModel<TDetailEntity, TDetailProjection>(
			Func<TUnitOfWork, IReadOnlyRepository<TDetailEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailProjection>> predicate)
			where TDetailEntity : class
			where TDetailProjection : class {
			return ViewModelSource.Create(() => new ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailProjection, TUnitOfWork>(
				UnitOfWorkFactory, getRepositoryFunc, predicate, UnitOfWorkPolicy));
		}
		protected ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailEntity, TUnitOfWork> GetReadOnlyDetailsCollectionViewModel<TViewModel, TDetailEntity, TForeignKey>(
			Expression<Func<TViewModel, ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailEntity, TUnitOfWork>>> propertyExpression,
			Func<TUnitOfWork, IReadOnlyRepository<TDetailEntity>> getRepositoryFunc,
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailEntity>> projection = null) where TDetailEntity : class {
			var predicate = AppendForeignKeyPredicate(foreignKeyExpression, projection);
			return GetCollectionViewModelCore<ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailEntity, TUnitOfWork>, TDetailEntity, TDetailEntity, TForeignKey>(
				propertyExpression,
				() => CreateDetailsReadOnlyCollectionViewModel(getRepositoryFunc, predicate));
		}
		protected ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailProjection, TUnitOfWork> GetReadOnlyDetailProjectionsCollectionViewModel<
			TViewModel, TDetailEntity, TDetailProjection, TForeignKey>(
			Expression<Func<TViewModel, ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailProjection, TUnitOfWork>>> propertyExpression,
			Func<TUnitOfWork, IReadOnlyRepository<TDetailEntity>> getRepositoryFunc,
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailProjection>> projection) where TDetailEntity : class where TDetailProjection : class {
			var predicate = AppendForeignKeyPredicate(foreignKeyExpression, projection);
			return GetCollectionViewModelCore<ReadOnlyCollectionViewModelBase<TDetailEntity, TDetailProjection, TUnitOfWork>, TDetailEntity, TDetailProjection, TForeignKey>(
				propertyExpression,
				() => CreateDetailsReadOnlyCollectionViewModel(getRepositoryFunc, predicate));
		}
		Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailProjection>> AppendForeignKeyPredicate<TDetailEntity, TDetailProjection, TForeignKey>(
			Expression<Func<TDetailEntity, TForeignKey>> foreignKeyExpression,
			Func<IRepositoryQuery<TDetailEntity>, IQueryable<TDetailProjection>> projection)
			where TDetailEntity : class
			where TDetailProjection : class {
			var predicate = ExpressionHelper.GetKeyEqualsExpression<TDetailEntity, TDetailEntity, TForeignKey>(foreignKeyExpression, PrimaryKey);
			return ReadOnlyRepositoryExtensions.AppendToProjection<TDetailEntity, TDetailProjection>(predicate, projection);
		}
		protected IEntitiesViewModel<TLookUpEntity> GetLookUpEntitiesViewModel<TViewModel, TLookUpEntity, TLookUpEntityKey>(
			Expression<Func<TViewModel, IEntitiesViewModel<TLookUpEntity>>> propertyExpression,
			Func<TUnitOfWork, IRepository<TLookUpEntity, TLookUpEntityKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TLookUpEntity>, IQueryable<TLookUpEntity>> projection = null) where TLookUpEntity : class {
			return GetLookUpProjectionsViewModel<TViewModel, TLookUpEntity, TLookUpEntity, TLookUpEntityKey>(propertyExpression, getRepositoryFunc, projection);
		}
		protected IEntitiesViewModel<TLookUpEntity> GetLookUpEntitiesViewModel<TViewModel, TLookUpEntity, TLookUpEntityKey>(
			Expression<Func<TViewModel, IEntitiesViewModel<TLookUpEntity>>> propertyExpression,
			Func<TUnitOfWork, IReadOnlyRepository<TLookUpEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TLookUpEntity>, IQueryable<TLookUpEntity>> projection = null) where TLookUpEntity : class {
			return GetLookUpProjectionsViewModel<TViewModel, TLookUpEntity, TLookUpEntity, TLookUpEntityKey>(propertyExpression, getRepositoryFunc, projection);
		}
		protected IEntitiesViewModel<TLookUpProjection> GetLookUpProjectionsViewModel<TViewModel, TLookUpEntity, TLookUpProjection, TLookUpEntityKey>(
			Expression<Func<TViewModel, IEntitiesViewModel<TLookUpProjection>>> propertyExpression, 
			Func<TUnitOfWork, IRepository<TLookUpEntity, TLookUpEntityKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TLookUpEntity>, IQueryable<TLookUpProjection>> projection) where TLookUpEntity : 
			class where TLookUpProjection : class {
			return GetEntitiesViewModelCore<IEntitiesViewModel<TLookUpProjection>, TLookUpProjection>(
				propertyExpression,
				() => CreateLookUpEntitiesViewModel<TLookUpEntity, TLookUpProjection, TLookUpEntityKey>(getRepositoryFunc, projection));
		}
		protected IEntitiesViewModel<TLookUpProjection> GetLookUpProjectionsViewModel<TViewModel, TLookUpEntity, TLookUpProjection, TLookUpEntityKey>(
			Expression<Func<TViewModel, IEntitiesViewModel<TLookUpProjection>>> propertyExpression,
			Func<TUnitOfWork, IReadOnlyRepository<TLookUpEntity>> getRepositoryFunc,
			Func<IRepositoryQuery<TLookUpEntity>, IQueryable<TLookUpProjection>> projection) where TLookUpEntity :
			class where TLookUpProjection : class {
			return GetEntitiesViewModelCore<IEntitiesViewModel<TLookUpProjection>, TLookUpProjection>(
				propertyExpression,
				() => CreateLookUpEntitiesViewModel<TLookUpEntity, TLookUpProjection, TLookUpEntityKey>(getRepositoryFunc, projection));
		}
		protected virtual LookUpEntitiesViewModel<TLookUpEntity, TLookUpProjection, TLookUpEntityKey, TUnitOfWork>
			CreateLookUpEntitiesViewModel<TLookUpEntity, TLookUpProjection, TLookUpEntityKey>(
			Func<TUnitOfWork, IReadOnlyRepository<TLookUpEntity>> getRepositoryFunc, 
			Func<IRepositoryQuery<TLookUpEntity>, 
			IQueryable<TLookUpProjection>> projection)
			where TLookUpEntity : class
			where TLookUpProjection : class {
			return LookUpEntitiesViewModel<TLookUpEntity, TLookUpProjection, TLookUpEntityKey, TUnitOfWork>
				.Create(UnitOfWorkFactory, getRepositoryFunc, projection);
		}
		Action<TDetailEntity> CreateForeignKeyPropertyInitializer<TDetailEntity, TForeignKey>(
			Action<TDetailEntity, TPrimaryKey> setMasterEntityKeyAction,
			Func<TForeignKey> getMasterEntityKey) where TDetailEntity : class {
			return x => {
				setMasterEntityKeyAction(x, (TPrimaryKey)(object)getMasterEntityKey());
			};
		}
		protected virtual bool CanCreateNewEntity() {
			if(Entity == null)
				return false;
			if(!IsNew())
				return true;
			string message = string.Format(ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_SaveParent), EntityDisplayName);
			var result = MessageBoxService.ShowMessage(message, ScaffoldingLocalizer.GetString(ScaffoldingStringId.Confirmation_Caption), MessageButton.YesNo);
			return result == MessageResult.Yes && SaveCore();
		}
		TViewModel GetCollectionViewModelCore<TViewModel, TDetailEntity, TDetailProjection, TForeignKey>(
			LambdaExpression propertyExpression,
			Func<TViewModel> createViewModelCallback)
			where TViewModel : IDocumentContent
			where TDetailEntity : class
			where TDetailProjection : class {
			return GetEntitiesViewModelCore<TViewModel, TDetailProjection>(propertyExpression, () =>
			{
				var viewModel = createViewModelCallback();
				viewModel.SetParentViewModel(this);
				return viewModel;
			});
		}
		static bool IsDerivedType(Type type, Type baseType) {
			while(type != null) {
				if(type == baseType)
					return true;
				type = type.BaseType;
			}
			return false;
		}
		object GetLookupPrimaryKey(object lookupEntity, Type lookupType) {
			var lookupRepositoryProperty = typeof(TUnitOfWork).GetProperties()
				.FirstOrDefault(x => IsDerivedType(x.PropertyType.GetGenericTypeDefinition(), typeof(IRepository<,>))
								  && x.PropertyType.GetGenericArguments().First() == lookupType);
			if(lookupRepositoryProperty == null)
				return null;
			var lookupRepository = lookupRepositoryProperty.GetValue(UnitOfWork, null);
			return lookupRepositoryProperty.PropertyType.GetMethod("GetPrimaryKey").Invoke(lookupRepository, new[] { lookupEntity });
		}
		void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if(Entity == null)
				return;
			var lookupProperty = (from lookup in lookups.GetAll()
								  from dep in lookup.dependentProperties
								  where dep.info.Name == e.PropertyName
								  select new { lookup, dep }).FirstOrDefault();
			if(lookupProperty == null)
				return;
			var value = lookupProperty.dep.info.GetValue(this, null);
			if(value == null)
				return;
			var primaryKey = GetLookupPrimaryKey(value, lookupProperty.dep.info.PropertyType);
			var pairs = ExpressionHelper.GetKeyPropertyValues(primaryKey).Zip(lookupProperty.dep.foreignKey, (pk, fk) => new { pk, fk });
			foreach(var p in pairs) {
				p.fk.SetValue(Entity, p.pk, null);
			}
		}
		static Expression<Func<T, bool>> MakeWhereClause<T>(PropertyInfo property, object[] pk, PropertyInfo[] fk) {
			var param = Expression.Parameter(typeof(T), "x");
			var andExpr = fk.Zip(pk, (f, p) => new { f, p }).Aggregate((Expression)Expression.Constant(true), (expr, pair) => {
				var propertyExpr = Expression.Property(param, pair.f);
				var equals = Expression.Equal(propertyExpr, Expression.Constant(pair.p));
				return Expression.AndAlso(expr, equals);
			});
			return null;
		}
		protected void AddLookUpProperty<TLookup, TForeignKey>(
			Expression<Func<IEntitiesViewModel<TLookup>>> lookupCollection,
			Expression<Func<TLookup>> lookup,
			Expression<Func<TForeignKey>> foreignKey) where TLookup : class
		{
			var collectionProperty = (PropertyInfo)((MemberExpression)lookupCollection.Body).Member;
			var info = lookups.Get(collectionProperty);
			var dependentProperty = new DependentPropertyInfo();
			info.dependentProperties.Add(dependentProperty);
			info.property = collectionProperty;
			dependentProperty.info = (PropertyInfo)((MemberExpression)lookup.Body).Member;
			var args = ((NewExpression)foreignKey.Body).Arguments;
			var fk = args.Select(x => {
				var propExpr = (MemberExpression)x;
				return (PropertyInfo)propExpr.Member;
			});
			dependentProperty.foreignKey = fk.ToArray();
		}
		TViewModel GetEntitiesViewModelCore<TViewModel, TDetailEntity>(
			LambdaExpression propertyExpression,
			Func<TViewModel> createViewModelCallback)
			where TViewModel : IDocumentContent
			where TDetailEntity : class
		{
			var property = (PropertyInfo)((MemberExpression)propertyExpression.Body).Member;
			var info = lookups.Get(property);
			info.property = property;
			if(info.viewModel == null) {
				info.viewModel = createViewModelCallback();
				var inpt = info.viewModel as INotifyPropertyChanged;
				if(inpt != null) {
					inpt.PropertyChanged += OnLookupCollectionPropertyChanged;
				}
			}
			return (TViewModel)info.viewModel;
		}
		Type StripPocoType(Type type) {
			if(type.Assembly.GetName().Name.StartsWith("EntityFrameworkDynamicProxies")) {
				return type.BaseType;
			}
			return type;
		}
		Type GetEntityCollectionViewModelEntityType(Type type) {
			var alltypes = new List<Type>();
			while(type != null) {
				alltypes.Add(type);
				alltypes.AddRange(type.GetInterfaces());
				type = type.BaseType;
			}
			var interfaceType = alltypes.FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntitiesViewModel<>));
			if(interfaceType == null)
				return null;
			return interfaceType.GetGenericArguments().FirstOrDefault();
		}
		void OnLookupCollectionPropertyChanged(object sender, PropertyChangedEventArgs e) {
			if(e.PropertyName != "Entities" || sender == null)
				return;
			var lookup = lookups.GetAll().FirstOrDefault(x => GetEntityCollectionViewModelEntityType(x.property.PropertyType)
														   == GetEntityCollectionViewModelEntityType(sender.GetType()));
			if(lookup == null)
				return;
			foreach(var dep in lookup.dependentProperties) {
				var entities = (IEnumerable<object>)sender.GetType().GetProperty("Entities").GetValue(sender, null);
				var newValue = entities.FirstOrDefault(x => {
					var pk = GetLookupPrimaryKey(x, StripPocoType(x.GetType()));
					if (pk == null)
						return false;
					var pkValues = ExpressionHelper.GetKeyPropertyValues(pk);
					var pairs = pkValues.Zip(dep.foreignKey, (p, f) => new { p, f });
					foreach(var pair in pairs) {
						if(!Equals(pair.f.GetValue(Entity, null), pair.p))
							return false;
					}
					return true;
				});
				dep.info.SetValue(this, newValue, null);
			}
		}
#endregion
#region ISupportParameter
		object ISupportParameter.Parameter {
			get { return null; }
			set { OnParameterChanged(value); }
		}
#endregion
#region IDocumentContent
		object IDocumentContent.Title { get { return Title; } }
		void IDocumentContent.OnClose(CancelEventArgs e) {
			e.Cancel = !TryClose();
			if (!e.Cancel && IsNew()) {
				Entity = null;
			}
			Messenger.Default.Send(new DestroyOrphanedDocumentsMessage(x => x != this));
		}
		void IDocumentContent.OnDestroy() {
			OnDestroy();
		}
		IDocumentOwner IDocumentContent.DocumentOwner {
			get { return DocumentOwner; }
			set { DocumentOwner = value; }
		}
#endregion
#region ISingleObjectViewModel
		TEntity ISingleObjectViewModel<TEntity, TPrimaryKey>.Entity { get { return Entity; } }
		TPrimaryKey ISingleObjectViewModel<TEntity, TPrimaryKey>.PrimaryKey { get { return PrimaryKey; } }
#endregion
#region ISupportLogicalLayout
		bool ISupportLogicalLayout.CanSerialize {
			get { return Entity != null && !IsNew(); }
		}
		SingleObjectViewModelState ISupportLogicalLayout<SingleObjectViewModelState>.SaveState() {
			return new SingleObjectViewModelState
			{
				Key = ExpressionHelper.GetKeyPropertyValues(PrimaryKey),
				Title = GetTitle(false),
				UnitOfWorkPolicy = UnitOfWorkPolicy,
				AllowSaveReset = AllowSaveReset,
				IsSharedTreeRoot = IsSharedTreeRoot
			};
		}
		void ISupportLogicalLayout<SingleObjectViewModelState>.RestoreState(SingleObjectViewModelState state) {
			var key = ExpressionHelper.IsTuple(typeof(TPrimaryKey))
					? ExpressionHelper.MakeTuple<TPrimaryKey>(state.Key)
					: (TPrimaryKey)state.Key.First();
			UnitOfWorkPolicy = state.UnitOfWorkPolicy;
			AllowSaveReset = state.AllowSaveReset;
			IsSharedTreeRoot = state.IsSharedTreeRoot;
			LoadEntityByKey(key);
			if(Entity == null)
				UpdateTitle(state.Title + ScaffoldingLocalizer.GetString(ScaffoldingStringId.Entity_Deleted));
		}
		IDocumentManagerService ISupportLogicalLayout.DocumentManagerService {
			get { return this.GetService<IDocumentManagerService>(); }
		}
		IEnumerable<object> ISupportLogicalLayout.LookupViewModels {
			get { return lookups.GetAll().Select(x => x.viewModel).ToArray(); }
		}
#endregion
		public virtual object ParentViewModel { get; set; }
		protected void OnParentViewModelChanged() {
			UpdateAllowSaveReset();
		}
		void UpdateAllowSaveReset() {
			var parent = ParentViewModel as ISupportUnitOfWorkPolicy;
			if(parent == null)
				return;
			bool isSharedRoot = UnitOfWorkPolicy == UnitOfWorkPolicy.Shared &&
				parent.UnitOfWorkPolicy != UnitOfWorkPolicy.Shared;
			bool isIndividualTree = UnitOfWorkPolicy == UnitOfWorkPolicy.Individual &&
				parent.UnitOfWorkPolicy == UnitOfWorkPolicy.Individual;
			AllowSaveReset = isSharedRoot || isIndividualTree;
		}
	}
}
