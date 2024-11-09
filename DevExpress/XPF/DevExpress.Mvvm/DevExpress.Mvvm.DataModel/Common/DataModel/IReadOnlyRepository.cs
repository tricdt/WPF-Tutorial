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
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
namespace DevExpress.Mvvm.DataModel {
	public interface IReadOnlyRepository<TEntity> : IRepositoryQuery<TEntity> where TEntity : class {
		IUnitOfWork UnitOfWork { get; }
	}
	public interface IRepositoryQuery<T> : IQueryable<T> {
		IRepositoryQuery<T> Include<TProperty>(Expression<Func<T, TProperty>> path);
		IRepositoryQuery<T> Where(Expression<Func<T, bool>> predicate);
	}
	public abstract class RepositoryQueryBase<T> : IQueryable<T> {
		readonly Lazy<IQueryable<T>> queryable;
		protected IQueryable<T> Queryable { get { return queryable.Value; } }
		protected RepositoryQueryBase(Func<IQueryable<T>> getQueryable) {
			this.queryable = new Lazy<IQueryable<T>>(getQueryable);
		}
		Type IQueryable.ElementType { get { return this.Queryable.ElementType; } }
		Expression IQueryable.Expression { get { return this.Queryable.Expression; } }
		IQueryProvider IQueryable.Provider { get { return this.Queryable.Provider; } }
		IEnumerator IEnumerable.GetEnumerator() { return this.Queryable.GetEnumerator(); }
		IEnumerator<T> IEnumerable<T>.GetEnumerator() { return this.Queryable.GetEnumerator(); }
	}
	public static class ReadOnlyRepositoryExtensions {
		public static IQueryable<TProjection> GetFilteredEntities<TEntity, TProjection>(
			this IReadOnlyRepository<TEntity> repository, 
			Expression<Func<TEntity, bool>> predicate, 
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection) where TEntity : class {
			return AppendToProjection(predicate, projection)(repository);
		}
		public static Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> AppendToProjection<TEntity, TProjection>(Expression<Func<TEntity, bool>> predicate, Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection) where TEntity : class {
			if(predicate == null && projection == null)
				return q => (IQueryable<TProjection>)q;
			if(predicate == null)
				return projection;
			if(projection == null)
				return q => (IQueryable<TProjection>)q.Where(predicate);
			return q => projection(q.Where(predicate));
		}
		public static IQueryable<TEntity> GetFilteredEntities<TEntity>(this IReadOnlyRepository<TEntity> repository, Expression<Func<TEntity, bool>> predicate) where TEntity : class {
			return repository.GetFilteredEntities(predicate, x => x);
		}
	}
}
