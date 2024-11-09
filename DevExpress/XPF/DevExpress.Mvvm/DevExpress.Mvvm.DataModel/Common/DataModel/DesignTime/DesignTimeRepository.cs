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
using System.Collections.Generic;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
namespace DevExpress.Mvvm.DataModel.DesignTime {
	public class DesignTimeRepository<TEntity, TPrimaryKey> : DesignTimeReadOnlyRepository<TEntity>, IRepository<TEntity, TPrimaryKey>
		where TEntity : class {
		readonly Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
		readonly EntityTraits<TEntity, TPrimaryKey> entityTraits;
		public DesignTimeRepository(DesignTimeUnitOfWork unitOfWork, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression)
			: base(unitOfWork) {
			this.getPrimaryKeyExpression = getPrimaryKeyExpression;
			this.entityTraits = ExpressionHelper.GetEntityTraits(this, getPrimaryKeyExpression);
		}
		protected virtual TEntity CreateCore() {
			return DesignTimeHelper.CreateDesignTimeObject<TEntity>();
		}
		protected virtual void UpdateCore(TEntity entity) {
		}
		protected virtual EntityState GetStateCore(TEntity entity) {
			return EntityState.Detached;
		}
		protected virtual TEntity FindCore(TPrimaryKey primaryKey) {
			throw new InvalidOperationException();
		}
		protected virtual void RemoveCore(TEntity entity) {
			throw new InvalidOperationException();
		}
		protected virtual TEntity ReloadCore(TEntity entity) {
			throw new InvalidOperationException();
		}
		protected virtual TPrimaryKey GetPrimaryKeyCore(TEntity entity) {
			return entityTraits.GetPrimaryKey(entity);
		}
		protected virtual void SetPrimaryKeyCore(TEntity entity, TPrimaryKey primaryKey) {
			var setPrimaryKeyAction = entityTraits.SetPrimaryKey;
			setPrimaryKeyAction(entity, primaryKey);
		}
		protected virtual void AddCore(TEntity entity) {
			throw new InvalidOperationException();
		}
		#region IRepository
		TEntity IRepository<TEntity, TPrimaryKey>.Find(TPrimaryKey primaryKey) {
			return FindCore(primaryKey);
		}
		void IRepository<TEntity, TPrimaryKey>.Add(TEntity entity) {
			AddCore(entity);
		}
		void IRepository<TEntity, TPrimaryKey>.Remove(TEntity entity) {
			RemoveCore(entity);
		}
		TEntity IRepository<TEntity, TPrimaryKey>.Create(bool add) {
			return CreateCore();
		}
		void IRepository<TEntity, TPrimaryKey>.Update(TEntity entity) {
			UpdateCore(entity);
		}
		EntityState IRepository<TEntity, TPrimaryKey>.GetState(TEntity entity) {
			return GetStateCore(entity);
		}
		TEntity IRepository<TEntity, TPrimaryKey>.Reload(TEntity entity) {
			return ReloadCore(entity);
		}
		Expression<Func<TEntity, TPrimaryKey>> IRepository<TEntity, TPrimaryKey>.GetPrimaryKeyExpression {
			get { return getPrimaryKeyExpression; }
		}
		TPrimaryKey IRepository<TEntity, TPrimaryKey>.GetPrimaryKey(TEntity entity) {
			return GetPrimaryKeyCore(entity);
		}
		bool IRepository<TEntity, TPrimaryKey>.HasPrimaryKey(TEntity entity) {
			return entityTraits.HasPrimaryKey(entity);
		}
		void IRepository<TEntity, TPrimaryKey>.SetPrimaryKey(TEntity entity, TPrimaryKey primaryKey) {
			SetPrimaryKeyCore(entity, primaryKey);
		}
		#endregion
	}
}
