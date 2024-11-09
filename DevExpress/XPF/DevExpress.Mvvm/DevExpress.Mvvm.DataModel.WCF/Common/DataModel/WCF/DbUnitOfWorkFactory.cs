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
using DevExpress.Mvvm;
using System.Collections;
using System.ComponentModel;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.Async.Helpers;
using DevExpress.Xpf.Core.ServerMode;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Utils;
using DevExpress.Xpf.Native.OrmMeta.Wcf;
namespace DevExpress.Mvvm.DataModel.WCF {
	public class DbUnitOfWorkFactory<TUnitOfWork, TContext> : IUnitOfWorkFactory<TUnitOfWork>
		where TUnitOfWork : IUnitOfWork
		where TContext : class {
		Func<TUnitOfWork> getUnitOfWork;
		Func<TUnitOfWork> getNoTrackingUnitOfWork;
		Func<TContext> getContext;
		public DbUnitOfWorkFactory(Func<TUnitOfWork> getUnitOfWork, Func<TContext> getContext) {
			this.getUnitOfWork = getUnitOfWork;
			this.getNoTrackingUnitOfWork = () => {
				var uow = getUnitOfWork();
				((IProvideContext)uow).Context.MergeOption = MergeOptionRuntimeWrapper.NoTracking;
				return uow;
			};
			this.getContext = () => {
				var context = getContext();
				var wrapper = DataServiceContextRuntimeWrapper.Wrap(context);
				wrapper.MergeOption = MergeOptionRuntimeWrapper.NoTracking;
				return context;
			};
		}
		TUnitOfWork IUnitOfWorkFactory<TUnitOfWork>.CreateUnitOfWork() {
			return getUnitOfWork();
		}
		IInstantFeedbackSource<TProjection> IUnitOfWorkFactory<TUnitOfWork>.CreateInstantFeedbackSource<TEntity, TProjection, TPrimaryKey>(
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection) {
			var threadSafeProperties = new TypeInfoProxied(TypeDescriptor.GetProperties(typeof(TProjection)), null).UIDescriptors;
			if(projection == null) {
				projection = x => x as IQueryable<TProjection>;
			}
			var source = new WcfInstantFeedbackDataSource
			{
				UseExtendedDataQuery = GetDbRepositoryQuery(getRepositoryFunc, projection).UseExtendedDataQuery,
				DataServiceContext = getContext()
			};
			var keyProperties = ExpressionHelper.GetKeyProperties(getRepositoryFunc(getNoTrackingUnitOfWork()).GetPrimaryKeyExpression);
			var keyExpression = string.Join(";", keyProperties.Select(p => p.Name));
			source.GetSource += (s, e) =>
			{
				e.Query = GetDbRepositoryQuery(getRepositoryFunc, projection).Query;
				e.KeyExpression = keyExpression;
				e.Handled = true;
			};
			return new InstantFeedbackSource<TProjection>(source, threadSafeProperties);
		}
		DbRepositoryQuery<TProjection> GetDbRepositoryQuery<TEntity, TProjection, TPrimaryKey>(Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection)
			where TEntity : class, new()
			where TProjection : class {
			var dbRepositoryQuery = projection(getRepositoryFunc(getNoTrackingUnitOfWork())) as DbRepositoryQuery<TProjection>;
			if(dbRepositoryQuery == null)
				throw new InvalidOperationException("WCF projections in the Instant Feedback mode only support the Include and Where operations");
			return dbRepositoryQuery;
		}
	}
}
