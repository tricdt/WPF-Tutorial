﻿#region Copyright (c) 2000-2024 Developer Express Inc.
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
using System.Collections.ObjectModel;
using DevExpress.Mvvm.DataModel;
using DevExpress.Xpf.Native.OrmMeta.EF7;
namespace DevExpress.Mvvm.DataModel.EFCore {
	public class DbReadOnlyRepository<TEntity, TDbContext> : DbRepositoryQuery<TEntity>, IReadOnlyRepository<TEntity>
		where TEntity : class {
		readonly Func<TDbContext, IQueryable<TEntity>> dbSetAccessor;
		protected readonly DbUnitOfWork<TDbContext> unitOfWork;
		public DbReadOnlyRepository(DbUnitOfWork<TDbContext> unitOfWork, Func<TDbContext, IQueryable<TEntity>> dbSetAccessor)
			: base(() => dbSetAccessor(unitOfWork.Context)) {
			this.dbSetAccessor = dbSetAccessor;
			this.unitOfWork = unitOfWork;
		}
		protected IQueryable<TEntity> DbSet {
			get { return dbSetAccessor(unitOfWork.Context); }
		}
		internal DbSetRuntimeWrapper<TEntity> TypedDbSet {
			get { return DbSetRuntimeWrapper<TEntity>.Wrap(dbSetAccessor(unitOfWork.Context)); }
		}
		protected TDbContext Context {
			get { return unitOfWork.Context; }
		}
		internal DbContextRuntimeWrapper TypedContext {
			get { return DbContextRuntimeWrapper.Wrap(unitOfWork.Context); }
		}
		#region IReadOnlyRepository
		IUnitOfWork IReadOnlyRepository<TEntity>.UnitOfWork {
			get { return unitOfWork; }
		}
		#endregion
	}
	public class DbRepositoryQuery<TEntity> : RepositoryQueryBase<TEntity>, IRepositoryQuery<TEntity> where TEntity : class {
		public DbRepositoryQuery(Func<IQueryable<TEntity>> getQueryable)
			: base(getQueryable) { }
		IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Include<TProperty>(Expression<Func<TEntity, TProperty>> path) {
			return new DbRepositoryQuery<TEntity>(() => (IQueryable<TEntity>)(Queryable.Include(path)).Object);
		}
		IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Where(Expression<Func<TEntity, bool>> predicate) {
			return new DbRepositoryQuery<TEntity>(() => Queryable.Where(predicate));
		}
	}
}
