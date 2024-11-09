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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DevExpress.Internal.Mvvm.DataModel.Objects {
	public class ObjectUnitOfWorkFactory<TUnitOfWork, TSource> : IUnitOfWorkFactory<TUnitOfWork> where TUnitOfWork : IUnitOfWork {
		Func<TUnitOfWork> create;
		public ObjectUnitOfWorkFactory(Func<TUnitOfWork> create) {
			this.create = create;
		}
		public IInstantFeedbackSource<TProjection> CreateInstantFeedbackSource<TEntity, TProjection, TPrimaryKey>(
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc, 
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection)
			where TEntity : class, new()
			where TProjection : class {
			var untypedUow = create();
			var uow = (ObjectUnitOfWork<TSource>)(object)untypedUow;
			var repository = (ObjectRepository<TSource, TProjection, TPrimaryKey>)getRepositoryFunc(untypedUow);
			var list = repository.getEntityList(uow.source);
			return new ObjectInstantFeedbackSource<TProjection, TPrimaryKey>(list);
		}
		public TUnitOfWork CreateSharedUnitOfWork(bool reset) {
			return create();
		}
		public TUnitOfWork CreateUnitOfWork() {
			return create();
		}
	}
}
