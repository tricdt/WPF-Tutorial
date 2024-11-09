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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace DevExpress.Internal.Mvvm.DataModel.Objects {
	public class ObjectReadOnlyRepository<TEntity> : IQueryable<TEntity>, IReadOnlyRepository<TEntity>
		where TEntity : class {
		protected IQueryable<TEntity> queryable;
		public ObjectReadOnlyRepository(IUnitOfWork unitOfWork) {
			this.UnitOfWork = unitOfWork;
		}
		public Type ElementType {
			get { return queryable.ElementType; }
		}
		public Expression Expression {
			get { return queryable.Expression; }
		}
		public IQueryProvider Provider {
			get { return queryable.Provider; }
		}
		public IUnitOfWork UnitOfWork { get; private set; }
		public IEnumerator<TEntity> GetEnumerator() {
			return queryable.GetEnumerator();
		}
		public IRepositoryQuery<TEntity> Include<TProperty>(Expression<Func<TEntity, TProperty>> path) {
			return this;
		}
		public IRepositoryQuery<TEntity> Where(Expression<Func<TEntity, bool>> predicate) {
			return new ObjectReadOnlyRepository<TEntity>(UnitOfWork) { queryable = queryable.Where(predicate) };
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return queryable.GetEnumerator();
		}
	}
}
