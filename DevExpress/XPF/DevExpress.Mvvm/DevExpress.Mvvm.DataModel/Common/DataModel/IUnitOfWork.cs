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
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
namespace DevExpress.Mvvm.DataModel {
	public interface IUnitOfWork {
		void SaveChanges();
		bool HasChanges();
	}
	public interface IUnitOfWorkFactory<TUnitOfWork> where TUnitOfWork : IUnitOfWork {
		TUnitOfWork CreateUnitOfWork();
		IInstantFeedbackSource<TProjection> CreateInstantFeedbackSource<TEntity, TProjection, TPrimaryKey>(Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection)
			where TEntity : class, new()
			where TProjection : class;
	}
	public interface IInstantFeedbackSource<TEntity> : IListSource
		where TEntity : class {
		void Refresh();
		TProperty GetPropertyValue<TProperty>(object threadSafeProxy, Expression<Func<TEntity, TProperty>> propertyExpression);
		bool IsLoadedProxy(object threadSafeProxy);
	}
	class SharedUnitOfWorkFactory<TUnitOfWork> : IUnitOfWorkFactory<TUnitOfWork> where TUnitOfWork : IUnitOfWork {
		IUnitOfWorkFactory<TUnitOfWork> factory;
		IUnitOfWorkFactory<TUnitOfWork> nonSharedFactory;
		TUnitOfWork uow;
		public SharedUnitOfWorkFactory(IUnitOfWorkFactory<TUnitOfWork> factory, IUnitOfWorkFactory<TUnitOfWork> nonSharedFactory) {
			this.factory = factory;
			this.nonSharedFactory = nonSharedFactory;
			this.uow = factory.CreateUnitOfWork();
		}
		public IInstantFeedbackSource<TProjection> CreateInstantFeedbackSource<TEntity, TProjection, TPrimaryKey>(Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection)
			where TEntity : class, new()
			where TProjection : class {
			return factory.CreateInstantFeedbackSource(getRepositoryFunc, projection);
		}
		public TUnitOfWork CreateUnitOfWork() {
			return uow;
		}
		public IUnitOfWorkFactory<TUnitOfWork> UnderlyingUnitOfWork { get { return factory; } }
		public void Reset() {
			uow = nonSharedFactory.CreateUnitOfWork();
		}
	}
	public static class UnitOfWorkFactoryExtensions {
		public static IUnitOfWorkFactory<TUnitOfWork> MakeShared<TUnitOfWork>(this IUnitOfWorkFactory<TUnitOfWork> factory) 
			where TUnitOfWork : IUnitOfWork  {
			var shared = factory as SharedUnitOfWorkFactory<TUnitOfWork>;
			if (shared != null)
				return new SharedUnitOfWorkFactory<TUnitOfWork>(factory, shared.UnderlyingUnitOfWork);
			return new SharedUnitOfWorkFactory<TUnitOfWork>(factory, factory);
		}
		public static void ResetShared<TUnitOfWork>(this IUnitOfWorkFactory<TUnitOfWork> factory) 
			where TUnitOfWork : IUnitOfWork {
			var shared = factory as SharedUnitOfWorkFactory<TUnitOfWork>;
			if(shared != null) {
				shared.Reset();
			}
		}
	}
}
