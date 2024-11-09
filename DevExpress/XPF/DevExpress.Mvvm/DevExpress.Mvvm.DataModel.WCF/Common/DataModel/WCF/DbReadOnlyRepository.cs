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
using DevExpress.Xpf.Native.OrmMeta.Wcf;
namespace DevExpress.Mvvm.DataModel.WCF {
	internal class DbReadOnlyRepository<TEntity, TDbContext> : DbRepositoryQuery<TEntity>, IReadOnlyRepository<TEntity>
		where TEntity : class
		where TDbContext : class {
		readonly Func<TDbContext, IQueryable<TEntity>> dbSetAccessor;
		readonly DbUnitOfWork<TDbContext> unitOfWork;
		readonly Lazy<DataServiceCollectionRuntimeWrapper<TEntity>> localCollection;
		protected DbUnitOfWork<TDbContext> UnitOfWork {
			get { return unitOfWork; }
		}
		protected IQueryable<TEntity> DbSet { get { return dbSetAccessor((TDbContext)unitOfWork.ContextWrapper.Object); } }
		public DbReadOnlyRepository(DbUnitOfWork<TDbContext> unitOfWork, Func<TDbContext, IQueryable<TEntity>> dbSetAccessor, bool useExtendedDataQuery)
			: base(() => dbSetAccessor((TDbContext)unitOfWork.ContextWrapper.Object), null, useExtendedDataQuery) {
			this.dbSetAccessor = dbSetAccessor;
			this.unitOfWork = unitOfWork;
			this.localCollection = new Lazy<DataServiceCollectionRuntimeWrapper<TEntity>>(
				() => new DataServiceCollectionRuntimeWrapper<TEntity>(DbSet.Cast<TEntity>().Where(x => false)));
		}
		IUnitOfWork IReadOnlyRepository<TEntity>.UnitOfWork {
			get { return unitOfWork; }
		}
		internal protected DataServiceCollectionRuntimeWrapper<TEntity> LocalCollection {
			get {
				return localCollection.Value;
			}
		}
		protected override void LoadItem(TEntity item) {
			if(item != null)
				LocalCollection.Load(item);
		}
	}
	internal class DbRepositoryQuery<TEntity> : QueryableWrapper<TEntity, TEntity>, IRepositoryQuery<TEntity> where TEntity : class {
		readonly Lazy<IQueryable<TEntity>> query;
		DataServiceQueryRuntimeWrapper QueryWrapper { get { return DataServiceQueryRuntimeWrapper.Wrap(Query); } }
		internal IQueryable<TEntity> Query { get { return query.Value; } }
		public bool UseExtendedDataQuery { get; private set; }
		public DbRepositoryQuery(Func<IQueryable<TEntity>> getQuery, Action<TEntity> loadItemCallback, bool useExtendedDataQuery)
			: base(getQuery, loadItemCallback) {
			this.query = new Lazy<IQueryable<TEntity>>(getQuery);
			UseExtendedDataQuery = useExtendedDataQuery;
		}
		IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Include<TProperty>(Expression<Func<TEntity, TProperty>> path) {
			return new DbRepositoryQuery<TEntity>(() => QueryWrapper.Expand<TEntity, TProperty>(path), LoadItemCallback, UseExtendedDataQuery);
		}
		IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Where(Expression<Func<TEntity, bool>> predicate) {
			return new DbRepositoryQuery<TEntity>(() => Query.Where(predicate), LoadItemCallback, UseExtendedDataQuery);
		}
	}
	static class DataServiceQueryRuntimeWrapperExtensions {
		public static IQueryable<TEntity> Expand<TEntity, TProperty>(this DataServiceQueryRuntimeWrapper wrapper, object arg) {
			var obj = wrapper.Object;
			var definition = obj.GetType().GetMethods()
				.First(m => m.Name == "Expand" && m.GetParameters().Single().ParameterType != typeof(string));
			var method = definition.MakeGenericMethod(typeof(TProperty));
			return (IQueryable<TEntity>)method.Invoke(obj, new[] { arg });
		}
	}
}
