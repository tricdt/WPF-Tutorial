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
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.DataModel.DesignTime;
using DevExpress.Mvvm;
using System.Collections;
using System.ComponentModel;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.Async.Helpers;
namespace DevExpress.Mvvm.DataModel.EF6 {
	class InstantFeedbackSource<TEntity> : IInstantFeedbackSource<TEntity>
		where TEntity : class {
		readonly EntityInstantFeedbackSource source;
		readonly PropertyDescriptorCollection threadSafeProperties;
		public InstantFeedbackSource(EntityInstantFeedbackSource source, PropertyDescriptorCollection threadSafeProperties) {
			this.source = source;
			this.threadSafeProperties = threadSafeProperties;
		}
		bool IListSource.ContainsListCollection { get { return ((IListSource)source).ContainsListCollection; } }
		IList IListSource.GetList() {
			return ((IListSource)source).GetList();
		}
		TProperty IInstantFeedbackSource<TEntity>.GetPropertyValue<TProperty>(object threadSafeProxy, Expression<Func<TEntity, TProperty>> propertyExpression) {
			var propertyName = ExpressionHelper.GetPropertyName(propertyExpression);
			var threadSafeProperty = threadSafeProperties[propertyName];
			return (TProperty)threadSafeProperty.GetValue(threadSafeProxy);
		}
		bool IInstantFeedbackSource<TEntity>.IsLoadedProxy(object threadSafeProxy) {
			return threadSafeProxy is ReadonlyThreadSafeProxyForObjectFromAnotherThread;
		}
		void IInstantFeedbackSource<TEntity>.Refresh() {
			source.Refresh();
		}
	}
}
