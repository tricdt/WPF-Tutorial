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
using DevExpress.Xpf.Native.OrmMeta.EF7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace DevExpress.Mvvm.DataModel.EFCore {
	public abstract class DbUnitOfWork<TContext> : UnitOfWorkBase, IUnitOfWork {
		readonly Lazy<TContext> context;
		readonly Lazy<DbContextRuntimeWrapper> typedContext;
		public DbUnitOfWork(Func<TContext> contextFactory) {
			context = new Lazy<TContext>(contextFactory);
			typedContext = new Lazy<DbContextRuntimeWrapper>(() => DbContextRuntimeWrapper.Wrap(context.Value));
		}
		public TContext Context { get { return context.Value; } }
		internal DbContextRuntimeWrapper TypedContext { get { return typedContext.Value; } }
		void IUnitOfWork.SaveChanges() {
			try {
				TypedContext.SaveChanges();
			} catch (Exception ex) {
				if (DbUpdateExceptionRuntimeWrapper.IsCompatible(ex.GetType())) {
					throw DbExceptionsConverter.Convert(ex);
				}
				throw ex;
			}
		}
		bool IUnitOfWork.HasChanges() {
			return typedContext.Value.ChangeTracker.Entries().Any(x => x.State == EntityStateRuntimeWrapper.Modified);
		}
		protected IRepository<TEntity, TPrimaryKey>
			GetRepository<TEntity, TPrimaryKey>(Func<TContext, IQueryable<TEntity>> dbSetAccessor, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression)
			where TEntity : class, new() {
			return GetRepositoryCore<IRepository<TEntity, TPrimaryKey>, TEntity>(() => new DbRepository<TEntity, TPrimaryKey, TContext>(this, dbSetAccessor, getPrimaryKeyExpression));
		}
		protected IReadOnlyRepository<TEntity>
			GetReadOnlyRepository<TEntity>(Func<TContext, IQueryable<TEntity>> dbSetAccessor)
			where TEntity : class {
			return GetRepositoryCore<IReadOnlyRepository<TEntity>, TEntity>(() => new DbReadOnlyRepository<TEntity, TContext>(this, dbSetAccessor));
		}
	}
}
