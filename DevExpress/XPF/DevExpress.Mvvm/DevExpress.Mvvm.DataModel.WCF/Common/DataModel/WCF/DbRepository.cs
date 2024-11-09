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
using System.Collections.ObjectModel;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Utils;
using DevExpress.Xpf.Native.OrmMeta.Wcf;
namespace DevExpress.Mvvm.DataModel.WCF {
	internal class DbRepository<TEntity, TPrimaryKey, TDbContext> : DbReadOnlyRepository<TEntity, TDbContext>, IRepository<TEntity, TPrimaryKey>
		where TEntity : class, new()
		where TDbContext : class {
		readonly string dbSetName;
		readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
		readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;
		public DbRepository(DbUnitOfWork<TDbContext> unitOfWork, Expression<Func<TDbContext, IQueryable<TEntity>>> dbSetAccessorExpression, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression, bool useExtendedDataQuery)
			: base(unitOfWork, dbSetAccessorExpression.Compile(), useExtendedDataQuery) {
			Expression body = dbSetAccessorExpression.Body;
			while(body is MethodCallExpression) {
				body = ((MethodCallExpression)body).Object;
			}
			var member = body as MemberExpression;
			if (member == null) {
				var unary = body as UnaryExpression;
				if(unary != null) {
					member = unary.Operand as MemberExpression;
				}
			}
			this.dbSetName = member.Member.Name;
			this.getPrimaryKeyExpression = getPrimaryKeyExpression;
			this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression);
		}
		TEntity IRepository<TEntity, TPrimaryKey>.Find(TPrimaryKey primaryKey) {
			try {
				var entity = LocalCollection.SingleOrDefault(x => object.Equals(GetPrimaryKeyCore(x), primaryKey));
				if(entity != null)
					return entity;
				entity = FindCore(primaryKey);
				if(entity != null)
					LocalCollection.Load(entity);
				return entity;
			} catch (Exception e) {
				if(DataServiceQueryExceptionRuntimeWrapper.IsCompatible(e.GetType()))
					return null;
				throw e;
			}
		}
		void IRepository<TEntity, TPrimaryKey>.Add(TEntity entity) {
			AddCore(entity);
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
		EntityState IRepository<TEntity, TPrimaryKey>.GetState(TEntity entity) {
			return GetStateCore(entity);
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
		protected virtual void AddCore(TEntity entity) {
			LocalCollection.Add(entity);
		}
		protected virtual TEntity CreateCore(bool add) {
			TEntity newEntity = new TEntity();
			if(add) {
				AddCore(newEntity);
			}
			return newEntity;
		}
		protected virtual void UpdateCore(TEntity entity) {
			UnitOfWork.ContextWrapper.UpdateObject(entity);
		}
		protected virtual EntityState GetStateCore(TEntity entity) {
			var descriptor = UnitOfWork.ContextWrapper.GetEntityDescriptor(entity);
			return !ReferenceEquals(descriptor, null) ? GetEntityState(descriptor.State) : EntityState.Detached;
		}
		static EntityState GetEntityState(EntityStatesRuntimeWrapper entityStates) {
			switch(entityStates) {
				case EntityStatesRuntimeWrapper.Added:
					return EntityState.Added;
				case EntityStatesRuntimeWrapper.Deleted:
					return EntityState.Deleted;
				case EntityStatesRuntimeWrapper.Detached:
					return EntityState.Detached;
				case EntityStatesRuntimeWrapper.Modified:
					return EntityState.Modified;
				case EntityStatesRuntimeWrapper.Unchanged:
					return EntityState.Unchanged;
				default:
					throw new NotImplementedException();
			}
		}
		protected virtual TEntity FindCore(TPrimaryKey primaryKey) {
			return DbSet.Where(ExpressionHelper.GetKeyEqualsExpression<TEntity, TEntity, TPrimaryKey>(getPrimaryKeyExpression, primaryKey)).Take(1).ToArray().FirstOrDefault();
		}
		protected virtual void RemoveCore(TEntity entity) {
			try {
				LocalCollection.Remove(entity);
			} catch (Exception ex) {
				throw DbExceptionsConverter.Convert(ex);
			}
		}
		protected virtual TPrimaryKey GetPrimaryKeyCore(TEntity entity) {
			return entityTraits.GetPrimaryKey(entity);
		}
		protected virtual void SetPrimaryKeyCore(TEntity entity, TPrimaryKey primaryKey) {
			var setPrimaryKeyAction = entityTraits.SetPrimaryKey;
			setPrimaryKeyAction(entity, primaryKey);
		}
		TEntity IRepository<TEntity, TPrimaryKey>.Reload(TEntity entity) {
			int index = this.LocalCollection.IndexOf(entity);
			UnitOfWork.ContextWrapper.Detach(entity);
			TEntity newEntity = FindCore(GetPrimaryKeyCore(entity));
			if(newEntity == null)
				LocalCollection.RemoveAt(index);
			else if(index >= 0)
				((ObservableCollection<TEntity>)LocalCollection.Object)[index] = newEntity;
			return newEntity;
		}
	}
}
