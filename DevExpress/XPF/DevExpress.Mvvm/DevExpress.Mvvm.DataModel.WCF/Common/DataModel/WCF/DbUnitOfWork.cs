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
using DevExpress.Mvvm.DataModel;
using DevExpress.Xpf.Native.OrmMeta.Wcf;
namespace DevExpress.Mvvm.DataModel.WCF {
	internal interface IProvideContext {
		DataServiceContextRuntimeWrapper Context { get; }
	}
	public abstract class DbUnitOfWork<TContext> : UnitOfWorkBase, IProvideContext, IUnitOfWork where TContext : class {
		readonly Lazy<DataServiceContextRuntimeWrapper> context;
		public TContext Context { get { return (TContext)context.Value.Object; } }
		internal DataServiceContextRuntimeWrapper ContextWrapper { get { return context.Value; } }
		DataServiceContextRuntimeWrapper IProvideContext.Context { get { return context.Value; } }
		public DbUnitOfWork(Func<TContext> contextFactory) {
			this.context = new Lazy<DataServiceContextRuntimeWrapper>(() => DataServiceContextRuntimeWrapper.Wrap(contextFactory()));
		}
		void IUnitOfWork.SaveChanges() {
			try {
				ContextWrapper.SaveChanges();
			} catch (Exception ex) {
				throw DbExceptionsConverter.Convert(ex);
			}
		}
		bool IUnitOfWork.HasChanges() {
			return ContextWrapper.Entities.Any(x => x.State != EntityStatesRuntimeWrapper.Unchanged) 
				|| ContextWrapper.Links.Any(x => x.State != EntityStatesRuntimeWrapper.Unchanged);
		}
		internal protected IRepository<TEntity, TPrimaryKey>
			GetRepository<TEntity, TPrimaryKey>(Expression<Func<TContext, IQueryable<TEntity>>> dbSetAccessor, Expression<Func<TEntity, TPrimaryKey>> getPrimaryKeyExpression, bool useExtendedDataQuery = false)
			where TEntity : class, new() {
			return GetRepositoryCore<IRepository<TEntity, TPrimaryKey>, TEntity>(() => new DbRepository<TEntity, TPrimaryKey, TContext>(this, dbSetAccessor, getPrimaryKeyExpression, useExtendedDataQuery));
		}
		internal protected IReadOnlyRepository<TEntity>
			GetReadOnlyRepository<TEntity>(Func<TContext, IQueryable<TEntity>> dbSetAccessor, bool useExtendedDataQuery = false)
			where TEntity : class {
			return GetRepositoryCore<IReadOnlyRepository<TEntity>, TEntity>(() => new DbReadOnlyRepository<TEntity, TContext>(this, dbSetAccessor, useExtendedDataQuery));
		}
	}
}
