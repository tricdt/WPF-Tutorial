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

using DevExpress.Data.Helpers;
using DevExpress.Data.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
namespace DevExpress.Data {
	public class VirtualSource : IListSource, IDXCloneable, IDisposable {
		readonly int Count;
		VirtualSourceCore _List = null;
		protected VirtualSourceCore List {
			get {
				if(_List == null)
					CreateInnerList();
				return _List;
			}
		}
		public VirtualSource(int count) {
			Count = count;
		}
		public IList GetList() { return List; }
		void CreateInnerList() {
			_List = new VirtualSourceCore(new VirtualSourceReceiver(this), Count);
		}
		void List_PropertyNeeded(object sender, VirtualSourcePropertyNeededEventArgs e) {
			if(PropertyNeeded != null)
				PropertyNeeded(this, e);
		}
		void List_ValuePushed(object sender, VirtualSourceValuePushedEventArgs e) {
			if(ValuePushed != null)
				ValuePushed(this, e);
		}
		void List_ValueNeeded(object sender, VirtualSourceValueNeededEventArgs e) {
			if(ValueNeeded != null)
				ValueNeeded(this, e);
		}
		public bool ContainsListCollection { get { return true; } }
		public void GenerateProperties(int count) {
			if(PropertyNeeded == null)
				return;
			List.GenerateProperties(count);
		}
		#region IDXClone
		public virtual object DXClone() {
			return new VirtualSource(Count);
		}
		#endregion
		#region IDisposable
		public void Dispose() {
			if(_List == null)
				return;
			_List.ClearReceiver();
		}
		#endregion
		internal VirtualPropertyDescriptor[] GetProperties(int count) {
			if(PropertyNeeded == null)
				return EmptyArray<VirtualPropertyDescriptor>.Instance;
			VirtualPropertyDescriptor[] descriptors = new VirtualPropertyDescriptor[count];
			VirtualSourcePropertyNeededEventArgs args = new VirtualSourcePropertyNeededEventArgs();
			for(int i = 0; i < count; i++) {
				args.Index = i;
				args.PropertyType = typeof(string);
				args.ProperyName = "Property" + i.ToString();
				args.IsReadOnly = false;
				args.IsBrowsable = true;
				args.Description = null;
				args.Category = null;
				args.DisplayName = null;
				PropertyNeeded(this, args);
				descriptors[i] = new VirtualPropertyDescriptor(List, i, args.ProperyName, args.DisplayName, args.Description, args.Category, args.PropertyType, args.IsReadOnly);
			}
			return descriptors;
		}
		internal object GetPropertyValue(int rowIndex, int columnIndex) {
			if(ValueNeeded == null)
				return null;
			var args = new VirtualSourceValueNeededEventArgs(rowIndex, columnIndex);
			ValueNeeded(this, args);
			return args.Value;
		}
		internal void SetPropertyValue(int rowIndex, int columnIndex, object value) {
			if(ValuePushed != null)
				ValuePushed(this, new VirtualSourceValuePushedEventArgs(rowIndex, columnIndex, value));
		}
		public event EventHandler<VirtualSourceValueNeededEventArgs> ValueNeeded;
		public event EventHandler<VirtualSourceValuePushedEventArgs> ValuePushed;
		public event EventHandler<VirtualSourcePropertyNeededEventArgs> PropertyNeeded;
	}
	public class VirtualSourceValuePushedEventArgs : EventArgs {
		public VirtualSourceValuePushedEventArgs(int rowIndex, int columnIndex, object value) {
			RowIndex = rowIndex;
			ColumnIndex = columnIndex;
			Value = value;
		}
		public int RowIndex { get; private set; }
		public int ColumnIndex { get; private set; }
		public object Value { get; private set; }
	}
	public class VirtualSourceValueNeededEventArgs : EventArgs {
		public VirtualSourceValueNeededEventArgs(int rowIndex, int columnIndex) {
			RowIndex = rowIndex;
			ColumnIndex = columnIndex;
		}
		public int RowIndex { get; private set; }
		public int ColumnIndex { get; private set; }
		public object Value { get; set; }
	}
	public class VirtualSourcePropertyNeededEventArgs : EventArgs {
		public int Index { get; internal set; }
		public Type PropertyType { get; set; }
		public string ProperyName { get; set; }
		public bool IsReadOnly { get; set; }
		public bool IsBrowsable { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		public string DisplayName { get; set; }
	}
}
