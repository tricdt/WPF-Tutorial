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

using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace DevExpress.Internal.Mvvm.DataModel.Objects {
	public class ObjectRepository<TSource, TEntity, TPrimaryKey> : ObjectReadOnlyRepository<TEntity>, IRepository<TEntity, TPrimaryKey>
		where TEntity : class {
		internal Func<TSource, IList<TEntity>> getEntityList;
		internal List<TEntity> localClones;
		private Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression;
		private ObjectUnitOfWork<TSource> unitOfWork;
		public ObjectRepository(
			TSource source,
			ObjectUnitOfWork<TSource> objectUnitOfWork,
			Func<TSource, IList<TEntity>> getEntityList, 
			Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression) 
			: base(objectUnitOfWork)
		{
			this.localClones = getEntityList(source).Select(Clone).ToList();
			this.queryable = localClones.AsQueryable();
			this.unitOfWork = objectUnitOfWork;
			this.getEntityList = getEntityList;
			this.getPrimaryKeyExpression = getPrimaryKeyExpression;
		}
		public Expression<Func<TEntity, TPrimaryKey>> GetPrimaryKeyExpression {
			get { return getPrimaryKeyExpression; }
		}
		public void Add(TEntity entity) {
			unitOfWork.RecordNew(this, entity);
		}
		TEntity Clone(TEntity entity) {
			var clone = (TEntity)Activator.CreateInstance(typeof(TEntity));
			CopyProperties(entity, clone);
			return clone;
		}
		private static void CopyProperties(TEntity from, TEntity to) {
			foreach(var prop in to.GetType().GetProperties()) {
				var value = prop.GetValue(from, null);
				prop.SetValue(to, value, null);
			}
		}
		public TEntity Create(bool add = true) {
			var entity = (TEntity)Activator.CreateInstance(typeof(TEntity));
			if (add) {
				Add(entity);
			}
			return entity;
		}
		public TEntity Find(TPrimaryKey primaryKey) {
			var newEntity = unitOfWork.newEntities.FirstOrDefault(x => Equals(GetPrimaryKey((TEntity)x.entity), primaryKey));
			if(newEntity != null)
				return (TEntity)newEntity.entity;
			return localClones.FirstOrDefault(x => Equals(GetPrimaryKey(x), primaryKey))
				?? getEntityList(unitOfWork.source).FirstOrDefault(x => Equals(GetPrimaryKey(x), primaryKey));
		}
		public TPrimaryKey GetPrimaryKey(TEntity entity) {
			if(entity == null)
				return default(TPrimaryKey);
			return getPrimaryKeyExpression.Compile()(entity);
		}
		public EntityState GetState(TEntity entity) {
			return unitOfWork.newEntities.Any(x => Equals(x.entity, entity)) ? EntityState.Added :
				unitOfWork.deleteEntities.Any(x => Equals(x.entity, entity)) ? EntityState.Deleted :
				unitOfWork.updateEntities.Any(x => Equals(x.entity, entity)) ? EntityState.Modified :
				getEntityList(unitOfWork.source).Any(x => Equals(GetPrimaryKey(x), GetPrimaryKey(entity))) ? EntityState.Unchanged :
				EntityState.Detached;
		}
		public bool HasPrimaryKey(TEntity entity) {
			return entity != null;
		}
		public TEntity Reload(TEntity entity) {
			unitOfWork.updateEntities = unitOfWork.updateEntities.Where(x => x.entity != entity).ToList();
			var original = getEntityList(unitOfWork.source).First(x => Equals(GetPrimaryKey(x), GetPrimaryKey(entity)));
			CopyProperties(original, entity);
			return entity;
		}
		public void Remove(TEntity entity) {
			unitOfWork.RecordDelete(this, entity);
		}
		public void SetPrimaryKey(TEntity entity, TPrimaryKey primaryKey) {
			ExpressionHelper.GetSetKeyAction(getPrimaryKeyExpression)(entity, primaryKey);
		}
		public void Update(TEntity entity) {
			unitOfWork.RecordUpdate(this, entity);
		}
	}
}
