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
	public class ObjectInstantFeedbackSource<TProjection, TPrimaryKey> : IInstantFeedbackSource<TProjection>
		where TProjection : class {
		class ObjectList : IList {
			IList<TProjection> list;
			public ObjectList(IList<TProjection> list) {
				this.list = list;
			}
			public object this[int index] {
				get { return list[index]; }
				set { list[index] = (TProjection)value; }
			}
			public int Count {
				get { return list.Count; }
			}
			public bool IsFixedSize {
				get { return true; }
			}
			public bool IsReadOnly {
				get { return false; }
			}
			public bool IsSynchronized {
				get { return true; }
			}
			public object SyncRoot {
				get { return null; }
			}
			public int Add(object value) {
				list.Add((TProjection)value);
				return list.Count;
			}
			public void Clear() {
				list.Clear();
			}
			public bool Contains(object value) {
				return list.Contains((TProjection)value);
			}
			public void CopyTo(Array array, int index) {
				foreach (var obj in array) {
					var typed = (TProjection)obj;
					list.Insert(index, typed);
					index++;
				}
			}
			public IEnumerator GetEnumerator() {
				return list.GetEnumerator();
			}
			public int IndexOf(object value) {
				return list.IndexOf((TProjection)value);
			}
			public void Insert(int index, object value) {
				list.Insert(index, (TProjection)value);
			}
			public void Remove(object value) {
				list.Remove((TProjection)value);
			}
			public void RemoveAt(int index) {
				list.RemoveAt(index);
			}
		}
		IList<TProjection> list;
		ObjectList objectList;
		public ObjectInstantFeedbackSource(IList<TProjection> list) {
			this.list = list;
			this.objectList = new ObjectList(list);
		}
		public bool ContainsListCollection {
			get { return false; }
		}
		public IList GetList() {
			return objectList;
		}
		public TProperty GetPropertyValue<TProperty>(object threadSafeProxy, Expression<Func<TProjection, TProperty>> propertyExpression) {
			return propertyExpression.Compile()((TProjection)threadSafeProxy);
		}
		public bool IsLoadedProxy(object threadSafeProxy) {
			return true;
		}
		public void Refresh() {
		}
	}
}
