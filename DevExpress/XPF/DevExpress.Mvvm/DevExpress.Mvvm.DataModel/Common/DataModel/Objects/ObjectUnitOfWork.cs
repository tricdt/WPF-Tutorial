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
using DevExpress.Mvvm.DataModel.DesignTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
namespace DevExpress.Internal.Mvvm.DataModel.Objects {
	public class ObjectUnitOfWork<TSource> : UnitOfWorkBase, IUnitOfWork {
		internal TSource source;
		internal class NewEntityEntry {
			public Action add;
			public object entity;
		}
		internal class DeleteEntityEntry {
			public Action delete;
			public object entity;
		}
		internal class UpdateEntityEntry {
			public Action apply;
			public object entity;
		}
		internal List<NewEntityEntry> newEntities = new List<NewEntityEntry>();
		internal List<DeleteEntityEntry> deleteEntities = new List<DeleteEntityEntry>();
		internal List<UpdateEntityEntry> updateEntities = new List<UpdateEntityEntry>();
		public ObjectUnitOfWork(TSource source) {
			this.source = source;
		}
		public bool HasChanges() {
			return newEntities.Count > 0 || deleteEntities.Count > 0 || updateEntities.Count > 0;
		}
		public void SaveChanges() {
			newEntities.ForEach(x => x.add());
			deleteEntities.ForEach(x => x.delete());
			updateEntities.ForEach(x => x.apply());
			newEntities.Clear();
			deleteEntities.Clear();
			updateEntities.Clear();
		}
		TKey CreateNewPrimaryKey<TKey, TEntity>(IList<TEntity> list, ObjectRepository<TSource, TEntity, TKey> repository) where TEntity : class {
			if(typeof(TKey) == typeof(Guid))
				return (TKey)(object)Guid.NewGuid();
			if (typeof(TKey) == typeof(int)) {
				if(!list.Any())
					return (TKey)(object)1;
				var last = list.Max(x => (int)(object)repository.GetPrimaryKey(x));
				return (TKey)(object)(last + 1);
			}
			throw new NotImplementedException();
		}
		internal void RecordNew<TEntity, TKey>(ObjectRepository<TSource, TEntity, TKey> repository, TEntity entity) where TEntity : class {
			newEntities.Add(new NewEntityEntry {
				add = () => {
					var list = repository.getEntityList(source);
					repository.SetPrimaryKey(entity, CreateNewPrimaryKey(list, repository));
					repository.getEntityList(source).Add(entity);
					repository.localClones.Add(entity);
				},
				entity = entity
			});
			deleteEntities = deleteEntities.Where(x => x.entity != entity).ToList();
		}
		internal void RecordDelete<TEntity, TKey>(ObjectRepository<TSource, TEntity, TKey> repository, TEntity entity) where TEntity : class {
			deleteEntities.Add(new DeleteEntityEntry {
				delete = () => {
					var list = repository.getEntityList(source);
					var original = list.First(x => Equals(repository.GetPrimaryKey(x), repository.GetPrimaryKey(entity)));
					repository.getEntityList(source).Remove(original);
					repository.localClones.Remove(entity);
				},
				entity = entity
			});
			newEntities = newEntities.Where(x => x.entity != entity).ToList();
		}
		internal void RecordUpdate<TEntity, TKey>(ObjectRepository<TSource, TEntity, TKey> repository, TEntity entity) where TEntity : class {
			if(entity == null)
				return;
			var newEntity = newEntities.FirstOrDefault(x => Equals(repository.GetPrimaryKey((TEntity)x.entity), repository.GetPrimaryKey(entity)));
			if(newEntity != null)
				return;
			var deletedEntity = deleteEntities.FirstOrDefault(x => Equals(repository.GetPrimaryKey((TEntity)x.entity), repository.GetPrimaryKey(entity)));
			if(deletedEntity != null)
				return;
			updateEntities = updateEntities.Where(x => !Equals(repository.GetPrimaryKey((TEntity)x.entity), repository.GetPrimaryKey(entity))).ToList();
			var original = repository.getEntityList(source).First(x => Equals(repository.GetPrimaryKey(x), repository.GetPrimaryKey(entity)));
			var changes = new List<Tuple<PropertyInfo, object>>();
			foreach (var prop in original.GetType().GetProperties()) {
				var value = prop.GetValue(entity, null);
				changes.Add(Tuple.Create(prop, value));
			}
			updateEntities.Add(new UpdateEntityEntry {
				apply = () => {
					original = repository.getEntityList(source).FirstOrDefault(x => Equals(repository.GetPrimaryKey(x), repository.GetPrimaryKey(entity)));
					if(original == null)
						return;
					foreach (var change in changes) {
						var prop = change.Item1;
						var value = change.Item2;
						prop.SetValue(original, value, null);
					}
				},
				entity = entity
			});
		}
		protected IRepository<TEntity, TPrimaryKey>
			GetRepository<TEntity, TPrimaryKey>(
				Func<TSource, IList<TEntity>> getEntityList, 
				Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression)
			where TEntity : class, new() {
			return GetRepositoryCore<IRepository<TEntity, TPrimaryKey>, TEntity>(
				() => new ObjectRepository<TSource, TEntity, TPrimaryKey>(source, this, getEntityList, getPrimaryKeyExpression));
		}
	}
}
