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
using System.Reflection;
using System.Diagnostics;
namespace DevExpress.Mvvm.ViewModel {
	public class AddRemoveJunctionDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey, TUnitOfWork> : AddRemoveDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TUnitOfWork>
		where TEntity : class
		where TDetailEntity : class
		where TJunction : class, new()
		where TJunctionPrimaryKey : class
		where TUnitOfWork : IUnitOfWork {
		public static AddRemoveJunctionDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey, TUnitOfWork> CreateViewModel(
			IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc,
			Func<TUnitOfWork, IRepository<TJunction, TJunctionPrimaryKey>> getJunctionRepositoryFunc,
			Expression<Func<TJunction, TPrimaryKey>> getEntityForeignKey,
			Expression<Func<TJunction, TDetailPrimaryKey>> getDetailForeignKey,
			TPrimaryKey id) {
			return ViewModelSource.Create(() => new AddRemoveJunctionDetailEntitiesViewModel<TEntity, TPrimaryKey, TDetailEntity, TDetailPrimaryKey, TJunction, TJunctionPrimaryKey, TUnitOfWork>(unitOfWorkFactory, getRepositoryFunc, getDetailsRepositoryFunc, getJunctionRepositoryFunc, getEntityForeignKey, getDetailForeignKey, id));
		}
		readonly Func<TUnitOfWork, IRepository<TJunction, TJunctionPrimaryKey>> getJunctionRepositoryFunc;
		readonly Expression<Func<TJunction, TPrimaryKey>> getEntityForeignKey;
		readonly Expression<Func<TJunction, TDetailPrimaryKey>> getDetailForeignKey;
		IRepository<TDetailEntity, TDetailPrimaryKey> DetailsRepository { get { return getDetailsRepositoryFunc(UnitOfWork); } }
		IRepository<TJunction, TJunctionPrimaryKey> JunctionRepository { get { return getJunctionRepositoryFunc(UnitOfWork); } }
		public override bool IsCreateDetailButtonVisible { get { return false; } }
		IQueryable<TDetailEntity> CreateDetailEntityQuery() {
			var detailProperty = typeof(TJunction).GetProperties().Single(x => x.PropertyType == typeof(TDetailEntity));
			var junctions = JunctionRepository.Where(GetJunctionPredicate(Repository.GetPrimaryKey(Entity)));
			var param = Expression.Parameter(typeof(TJunction));
			var body = Expression.Property(param, detailProperty);
			Expression<Func<TJunction, TDetailEntity>> selector = Expression.Lambda<Func<TJunction, TDetailEntity>>(body, param);
			return junctions.Select(selector);
		}
		public override ICollection<TDetailEntity> DetailEntities {
			get {
				if(Entity == null)
					return Enumerable.Empty<TDetailEntity>().ToArray();
				return CreateDetailEntityQuery().ToArray();
			}
		}
		protected AddRemoveJunctionDetailEntitiesViewModel(
				IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory,
				Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
				Func<TUnitOfWork, IRepository<TDetailEntity, TDetailPrimaryKey>> getDetailsRepositoryFunc,
				Func<TUnitOfWork, IRepository<TJunction, TJunctionPrimaryKey>> getJunctionRepositoryFunc,
				Expression<Func<TJunction, TPrimaryKey>> getEntityForeignKey,
				Expression<Func<TJunction, TDetailPrimaryKey>> getDetailForeignKey,
				TPrimaryKey id)
			: base(unitOfWorkFactory, getRepositoryFunc, getDetailsRepositoryFunc, null, id) {
			this.getJunctionRepositoryFunc = getJunctionRepositoryFunc;
			this.getEntityForeignKey = getEntityForeignKey;
			this.getDetailForeignKey = getDetailForeignKey;
			Action updateDetailEntities = () => this.RaisePropertyChanged(x => x.DetailEntities);
		}
		Expression<Func<TJunction, bool>> GetJunctionPredicate(TPrimaryKey primaryKey) {
			return ExpressionHelper.GetKeyEqualsExpression<TJunction, TJunction, TPrimaryKey>(getEntityForeignKey, primaryKey);
		}
		Expression<Func<TJunction, bool>> GetJunctionPredicate(TPrimaryKey primaryKey, TDetailPrimaryKey detailPrimaryKey) {
			var parameter = Expression.Parameter(typeof(TJunction));
			var entityEquals = ExpressionHelper.GetKeyEqualsExpression<TJunction, TJunction, TPrimaryKey>(getEntityForeignKey, primaryKey, parameter);
			var detailEquals = ExpressionHelper.GetKeyEqualsExpression<TJunction, TJunction, TDetailPrimaryKey>(getDetailForeignKey, detailPrimaryKey, parameter);
			var and = Expression.And(entityEquals.Body, detailEquals.Body);
			return Expression.Lambda<Func<TJunction, bool>>(and, parameter);
		}
		public override void AddDetailEntities() {
			IQueryable<TDetailEntity> details = DetailsRepository;
			if (FilterExpression != null)
				details = details.Where(FilterExpression);
			var availableEntities = details.ToList().Except(DetailEntities).ToArray();
			var selectEntitiesViewModel = new SelectDetailEntitiesViewModel<TDetailEntity>(availableEntities);
			if(DialogService.ShowDialog(MessageButton.OKCancel, ScaffoldingLocalizer.GetString(ScaffoldingStringId.AddRemoveDetailEntities_SelectObjects), selectEntitiesViewModel) == MessageResult.OK && selectEntitiesViewModel.SelectedEntities.Count > 0) {
				foreach(var selectedEntity in selectEntitiesViewModel.SelectedEntities) {
					var junction = new TJunction();
					var entityKey = Repository.GetPrimaryKey(Entity);
					var detailKey = DetailsRepository.GetPrimaryKey(selectedEntity);
					var junctionType = typeof(TJunction);
					ExpressionHelper.GetSetKeyAction(getEntityForeignKey)(junction, entityKey);
					ExpressionHelper.GetSetKeyAction(getDetailForeignKey)(junction, detailKey);
					JunctionRepository.Add(junction);
				}
				SaveChangesAndNotify(selectEntitiesViewModel.SelectedEntities);
			}
		}
		public override void RemoveDetailEntities() {
			if(!SelectedEntities.Any())
				return;
			var entityKey = Repository.GetPrimaryKey(Entity);
			foreach(var selectedEntity in SelectedEntities) {
				var detailKey = DetailsRepository.GetPrimaryKey(selectedEntity);
				var junction = JunctionRepository.First(GetJunctionPredicate(entityKey, detailKey));
				JunctionRepository.Remove(junction);
			}
			SaveChangesAndNotify(SelectedEntities);
			SelectedEntities.Clear();
		}
	}
}
