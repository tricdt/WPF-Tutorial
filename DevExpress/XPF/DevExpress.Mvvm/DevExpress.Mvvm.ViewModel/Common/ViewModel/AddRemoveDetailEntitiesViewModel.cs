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
using System.Linq.Expressions;
using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DevExpress.Mvvm.POCO;
using System.Linq;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Localization;
namespace DevExpress.Mvvm.ViewModel {
	public class AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> : SingleObjectViewModelBase<TEntity, TPrimaryKey, TUnitOfWork>
		where TEntity : class
		where TDetailEntity : class
		where TUnitOfWork : IUnitOfWork {
		public static AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork> Create(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc, Func<TEntity, ICollection<TDetailEntity>> getDetailsFunc, TPrimaryKey id) {
			return ViewModelSource.Create(() => new AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>(unitOfWorkFactory, getRepositoryFunc, getDetailsRepositoryFunc, getDetailsFunc, id));
		}
		protected readonly Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc;
		readonly Func<TEntity, ICollection<TDetailEntity>> getDetailsFunc;
		protected IDialogService DialogService { get { return this.GetRequiredService<IDialogService>(); } }
		protected IDocumentManagerService DocumentManagerService { get { return this.GetRequiredService<IDocumentManagerService>(); } }
		IRepository<TDetailEntity, TDetailPrimaryKey> DetailsRepository { get { return getDetailsRepositoryFunc(UnitOfWork); } }
		public virtual ICollection<TDetailEntity> DetailEntities { get { return Entity != null ? getDetailsFunc(Entity) : Enumerable.Empty<TDetailEntity>().ToArray(); } }
		public ObservableCollection<TDetailEntity> SelectedEntities { get; set; }
		public virtual bool IsCreateDetailButtonVisible { get { return true; } }
		public virtual Expression<Func<TDetailEntity, bool>> FilterExpression { get; set; }
		protected AddRemoveDetailEntitiesViewModel(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory, Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc, Func<TEntity, ICollection<TDetailEntity>> getDetailsFunc, TPrimaryKey id)
			: base(unitOfWorkFactory, getRepositoryFunc, null) {
			this.getDetailsRepositoryFunc = getDetailsRepositoryFunc;
			this.getDetailsFunc = getDetailsFunc;
			SelectedEntities = new ObservableCollection<TDetailEntity>();
			if(this.IsInDesignMode())
				return;
			LoadEntityByKey(id);
			Messenger.Default.Register(this, (EntityMessage<TDetailEntity, TDetailPrimaryKey> m) =>
			{
				if(m.MessageType != EntityMessageType.Added)
					return;
				var withParent = m.Sender as ISupportParentViewModel;
				if(withParent == null || withParent.ParentViewModel != this)
					return;
				var withEntity = m.Sender as SingleObjectViewModelBase<TDetailEntity, TDetailPrimaryKey, TUnitOfWork>;
				var detailEntity = DetailsRepository.Find(DetailsRepository.GetPrimaryKey(withEntity.Entity));
				if(detailEntity == null)
					return;
				DetailEntities.Add(detailEntity);
				SaveChangesAndNotify(new TDetailEntity[] { detailEntity });
			});
		}
		protected void OnSelectedEntitiesChanged() {
			NotifyCanModifyEntitiesChanged();
		}
		public virtual void CreateDetailEntity() {
			DocumentManagerService.ShowNewEntityDocument<TDetailEntity>(this);
		}
		public virtual void EditDetailEntity() {
			if(SelectedEntities.Any()) {
				var detailKey = DetailsRepository.GetPrimaryKey(SelectedEntities.First());
				var parameter = new ViewModelInitInfo {
					UnitOfWorkPolicy = UnitOfWorkPolicy,
					PrimaryKey = detailKey,
					UnitOfWorkFactory = UnitOfWorkFactory
				};
				DocumentManagerService.ShowExistingEntityDocument<TDetailEntity, TDetailPrimaryKey>(this, detailKey, parameter);
			}
		}
		public bool CanEditDetailEntity() {
			return CanModifyDetailEntities();
		}
		protected override void OnInitializeInRuntime() {
			base.OnInitializeInRuntime();
			Messenger.Default.Register<EntityMessage<TEntity, TPrimaryKey>>(this, m => OnMessage(m));
		}
		public virtual void AddDetailEntities() {
			IQueryable<TDetailEntity> details = DetailsRepository;
			if (FilterExpression != null)
				details = details.Where(FilterExpression);
			var availalbleEntities = details.ToList().Except(DetailEntities).ToArray();
			var selectEntitiesViewModel = new SelectDetailEntitiesViewModel<TDetailEntity>(availalbleEntities);
			if(DialogService.ShowDialog(MessageButton.OKCancel, ScaffoldingLocalizer.GetString(ScaffoldingStringId.AddRemoveDetailEntities_SelectObjects), selectEntitiesViewModel) == MessageResult.OK && selectEntitiesViewModel.SelectedEntities.Count > 0) {
				foreach(var selectedEntity in selectEntitiesViewModel.SelectedEntities) {
					DetailEntities.Add(selectedEntity);
				}
				SaveChangesAndNotify(selectEntitiesViewModel.SelectedEntities);
			}
		}
		public bool CanAddDetailEntities() {
			return Entity != null;
		}
		public virtual void RemoveDetailEntities() {
			if(SelectedEntities.Count == 0)
				return;
			var entities = SelectedEntities.ToList();
			foreach(var e in entities) {
				SelectedEntities.Remove(e);
				DetailEntities.Remove(e);
			}
			SaveChangesAndNotify(entities);
			NotifyCanModifyEntitiesChanged();
		}
		public bool CanRemoveDetailEntities() {
			return CanModifyDetailEntities();
		}
		bool CanModifyDetailEntities() {
			return Entity != null && SelectedEntities.Count > 0;
		}
		void NotifyCanModifyEntitiesChanged() {
			this.RaiseCanExecuteChanged(x => x.RemoveDetailEntities());
			this.RaiseCanExecuteChanged(x => x.EditDetailEntity());
		}
		protected void SaveChangesAndNotify(IEnumerable<TDetailEntity> detailEntities) {
			try {
				UnitOfWork.SaveChanges();
				foreach(var detailEntity in detailEntities) {
					Messenger.Default.Send(new EntityMessage<TDetailEntity, TDetailPrimaryKey>(DetailsRepository.GetPrimaryKey(detailEntity), EntityMessageType.Changed, this));
				}
				Reload();
			} catch (DbException e) {
				OnDbException(e);
			}
		}
		void OnMessage(EntityMessage<TEntity, TPrimaryKey> message) {
			if(message.MessageType == EntityMessageType.Changed && Entity != null && object.Equals(PrimaryKey, message.PrimaryKey))
				Reload();
		}
		protected override void OnEntityChanged() {
			base.OnEntityChanged();
			this.RaisePropertyChanged(x => x.DetailEntities);
		}
	}
	public class SelectDetailEntitiesViewModel<TEntity> where TEntity : class {
		public SelectDetailEntitiesViewModel(TEntity[] detailEntities) {
			AvailableEntities = detailEntities;
			SelectedEntities = new List<TEntity>();
		}
		public TEntity[] AvailableEntities { get; private set; }
		public List<TEntity> SelectedEntities { get; set; }
	}
}
