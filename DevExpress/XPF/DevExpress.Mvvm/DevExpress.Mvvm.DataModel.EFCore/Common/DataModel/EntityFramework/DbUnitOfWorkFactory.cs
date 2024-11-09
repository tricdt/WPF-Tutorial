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
using DevExpress.Mvvm;
using System.Collections;
using System.ComponentModel;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.Async.Helpers;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.Utils;
namespace DevExpress.Mvvm.DataModel.EFCore {
	public class DbUnitOfWorkFactory<TUnitOfWork> : IUnitOfWorkFactory<TUnitOfWork> where TUnitOfWork : IUnitOfWork {
		Func<TUnitOfWork> createUnitOfWork;
		public DbUnitOfWorkFactory(Func<TUnitOfWork> createUnitOfWork) {
			this.createUnitOfWork = createUnitOfWork;
		}
		TUnitOfWork IUnitOfWorkFactory<TUnitOfWork>.CreateUnitOfWork() {
			return createUnitOfWork();
		}
		IInstantFeedbackSource<TProjection> IUnitOfWorkFactory<TUnitOfWork>.CreateInstantFeedbackSource<TEntity, TProjection, TPrimaryKey>(
			Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc,
			Func<IRepositoryQuery<TEntity>, IQueryable<TProjection>> projection) {
			var threadSafeProperties = new TypeInfoProxied(TypeDescriptor.GetProperties(typeof(TProjection)), null).UIDescriptors;
			if(projection == null) {
				projection = x => x as IQueryable<TProjection>;
			}
			var keyProperties = ExpressionHelper.GetKeyProperties(getRepositoryFunc(createUnitOfWork()).GetPrimaryKeyExpression);
			var keyExpression = string.Join(";", keyProperties.Select(p => p.Name));
			var source = new EntityInstantFeedbackSource((DevExpress.Data.Linq.GetQueryableEventArgs e) => e.QueryableSource = projection(getRepositoryFunc(createUnitOfWork())))
			{
				KeyExpression = keyExpression
			};
			return new InstantFeedbackSource<TProjection>(source, threadSafeProperties);
		}
	}
}
