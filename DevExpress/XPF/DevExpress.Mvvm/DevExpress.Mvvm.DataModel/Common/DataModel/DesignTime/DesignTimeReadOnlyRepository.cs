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
using System.Collections.ObjectModel;
using DevExpress.Mvvm;
namespace DevExpress.Mvvm.DataModel.DesignTime {
	public class DesignTimeReadOnlyRepository<TEntity> : DesignTimeRepositoryQuery<TEntity>, IReadOnlyRepository<TEntity>
		where TEntity : class {
		static IQueryable<TEntity> CreateSampleQueryable() {
			return DesignTimeHelper.CreateDesignTimeObjects<TEntity>(2).AsQueryable();
		}
		readonly DesignTimeUnitOfWork unitOfWork;
		public DesignTimeReadOnlyRepository(DesignTimeUnitOfWork unitOfWork)
			: base(CreateSampleQueryable()) {
			this.unitOfWork = unitOfWork;
		}
		#region IReadOnlyRepository
		IUnitOfWork IReadOnlyRepository<TEntity>.UnitOfWork {
			get { return unitOfWork; }
		}
		#endregion
	}
	public class DesignTimeRepositoryQuery<TEntity> : RepositoryQueryBase<TEntity>, IRepositoryQuery<TEntity> {
		public DesignTimeRepositoryQuery(IQueryable<TEntity> queryable)
			: base(() => queryable) { }
		IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Include<TProperty>(Expression<Func<TEntity, TProperty>> path) {
			return this;
		}
		IRepositoryQuery<TEntity> IRepositoryQuery<TEntity>.Where(Expression<Func<TEntity, bool>> predicate) {
			return this;
		}
	}
}
