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
using System.Data;
using System.Linq.Expressions;
using System.Collections.Generic;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Utils;
using DevExpress.Xpf.Native.OrmMeta.EF7;
namespace DevExpress.Mvvm.DataModel.EFCore {
	public class DbRepository<TEntity, TPrimaryKey, TDbContext> : DbReadOnlyRepository<TEntity, TDbContext>, IRepository<TEntity, TPrimaryKey>
		where TEntity : class, new() {
		readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
		readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;
		public DbRepository(DbUnitOfWork<TDbContext> unitOfWork, Func<TDbContext, IQueryable<TEntity>> dbSetAccessor, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression)
			: base(unitOfWork, dbSetAccessor) {
			this.getPrimaryKeyExpression = getPrimaryKeyExpression;
			this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression);
		}
		protected virtual TEntity CreateCore(bool add = true) {
			var entity = new TEntity();
			if(add) {
				TypedDbSet.Add(entity);
			}
			return entity;
		}
		protected virtual void UpdateCore(TEntity entity) {
			var entry = TypedContext.Entry(entity);
			if(entry.State == EntityStateRuntimeWrapper.Added)
				return;
			TypedContext.Entry(entity).State = EntityStateRuntimeWrapper.Modified;
		}
		protected virtual DevExpress.Mvvm.DataModel.EntityState GetStateCore(TEntity entity) {
			return GetEntityState(TypedContext.Entry(entity).State);
		}
		static DevExpress.Mvvm.DataModel.EntityState GetEntityState(EntityStateRuntimeWrapper entityStates) {
			switch(entityStates) {
				case EntityStateRuntimeWrapper.Added:
					return DevExpress.Mvvm.DataModel.EntityState.Added;
				case EntityStateRuntimeWrapper.Deleted:
					return DevExpress.Mvvm.DataModel.EntityState.Deleted;
				case EntityStateRuntimeWrapper.Detached:
					return DevExpress.Mvvm.DataModel.EntityState.Detached;
				case EntityStateRuntimeWrapper.Modified:
					return DevExpress.Mvvm.DataModel.EntityState.Modified;
				case EntityStateRuntimeWrapper.Unchanged:
					return DevExpress.Mvvm.DataModel.EntityState.Unchanged;
				default:
					throw new NotImplementedException();
			}
		}
		protected virtual TEntity FindCore(TPrimaryKey primaryKey) {
			object[] values;
			if(ExpressionHelper.IsTuple(typeof(TPrimaryKey))) {
				values = ExpressionHelper.GetKeyPropertyValues(primaryKey);
			} else {
				values = new object[] { primaryKey };
			}
			return TypedDbSet.Find(values);
		}
		protected virtual void RemoveCore(TEntity entity) {
			try {
				TypedDbSet.Remove(entity);
			} catch (Exception ex) {
				if(DbUpdateExceptionRuntimeWrapper.IsCompatible(ex.GetType())) {
					throw DbExceptionsConverter.Convert(ex);
				}
				throw ex;
			}
		}
		protected virtual TEntity ReloadCore(TEntity entity) {
			TypedContext.Entry(entity).Reload();
			return FindCore(GetPrimaryKeyCore(entity));
		}
		protected virtual TPrimaryKey GetPrimaryKeyCore(TEntity entity) {
			return entityTraits.GetPrimaryKey(entity);
		}
		protected virtual void SetPrimaryKeyCore(TEntity entity, TPrimaryKey primaryKey) {
			var setPrimaryKeyAction = entityTraits.SetPrimaryKey;
			setPrimaryKeyAction(entity, primaryKey);
		}
		#region IRepository
		TEntity IRepository<TEntity, TPrimaryKey>.Find(TPrimaryKey primaryKey) {
			return FindCore(primaryKey);
		}
		void IRepository<TEntity, TPrimaryKey>.Add(TEntity entity) {
			TypedDbSet.Add(entity);
		}
		void IRepository<TEntity, TPrimaryKey>.Remove(TEntity entity) {
			RemoveCore(entity);
		}
		TEntity IRepository<TEntity, TPrimaryKey>.Create(bool add) {
			return CreateCore(add);
		}
		void IRepository<TEntity, TPrimaryKey>.Update(TEntity entity) {
			UpdateCore(entity);
		}
		DevExpress.Mvvm.DataModel.EntityState IRepository<TEntity, TPrimaryKey>.GetState(TEntity entity) {
			return GetStateCore(entity);
		}
		TEntity IRepository<TEntity, TPrimaryKey>.Reload(TEntity entity) {
			return ReloadCore(entity);
		}
		Expression<Func<TEntity, TPrimaryKey>> IRepository<TEntity, TPrimaryKey>.GetPrimaryKeyExpression {
			get { return this.getPrimaryKeyExpression; }
		}
		void IRepository<TEntity, TPrimaryKey>.SetPrimaryKey(TEntity entity, TPrimaryKey primaryKey) {
			SetPrimaryKeyCore(entity, primaryKey);
		}
		TPrimaryKey IRepository<TEntity, TPrimaryKey>.GetPrimaryKey(TEntity entity) {
			return GetPrimaryKeyCore(entity);
		}
		bool IRepository<TEntity, TPrimaryKey>.HasPrimaryKey(TEntity entity) {
			return entityTraits.HasPrimaryKey(entity);
		}
		#endregion
	}
}
