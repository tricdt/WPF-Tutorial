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
using System.Collections;
using System.ComponentModel;
namespace DevExpress.Data.Native {
	public class VirtualSourceCore : IList, ITypedList {
		VirtualSourceReceiver Receiver;
		PropertyDescriptorCollection Properties;
		public VirtualSourceCore(VirtualSourceReceiver receiver, int count) {
			Receiver = receiver;
			Count = count;
		}
		#region ITypedList Interface
		public string GetListName(PropertyDescriptor[] listAccessors) { return String.Empty; }
		public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors) { return Properties; }
		#endregion
		#region IList Interface
		public virtual int Count {
			get;
			private set;
		}
		public virtual bool IsSynchronized {
			get { return true; }
		}
		public virtual object SyncRoot {
			get { return true; }
		}
		public virtual bool IsReadOnly {
			get { return false; }
		}
		public virtual bool IsFixedSize {
			get { return true; }
		}
		public virtual IEnumerator GetEnumerator() { return null; }
		public virtual void CopyTo(System.Array array, int fIndex) { }
		public virtual int Add(object val) {
			throw new NotImplementedException();
		}
		public virtual void Clear() {
			throw new NotImplementedException();
		}
		public virtual bool Contains(object val) {
			throw new NotImplementedException();
		}
		public virtual int IndexOf(object val) {
			throw new NotImplementedException();
		}
		public virtual void Insert(int fIndex, object val) {
			throw new NotImplementedException();
		}
		public virtual void Remove(object val) {
			throw new NotImplementedException();
		}
		public virtual void RemoveAt(int fIndex) {
			throw new NotImplementedException();
		}
		object IList.this[int fIndex] {
			get { return fIndex; }
			set { }
		}
		#endregion
		public void ClearReceiver() {
			Receiver = null;
		}
		protected virtual void SetProperties(VirtualPropertyDescriptor[] properties) {
			Properties = new PropertyDescriptorCollection(properties);
		}
		public virtual void GenerateProperties(int count) {
			if(Receiver == null)
				SetProperties(EmptyArray<VirtualPropertyDescriptor>.Instance);
			else
				SetProperties(Receiver.GetDescriptors(count));
		}
		protected internal void SetValue(int rowIndex, int columnIndex, object value) {
			if(Receiver == null)
				return;
			Receiver.SetValue(rowIndex, columnIndex, value);
		}
		protected internal object GetValue(int rowIndex, int columnIndex) {
			if(Receiver == null)
				return null;
			return Receiver.GetValue(rowIndex, columnIndex);
		}
	}
	public class VirtualSourceReceiver {
		readonly VirtualSource VirtualSource;
		public VirtualSourceReceiver(VirtualSource virtualSource) {
			VirtualSource = virtualSource;
		}
		public VirtualPropertyDescriptor[] GetDescriptors(int count) {
			return VirtualSource.GetProperties(count);
		}
		public object GetValue(int rowIndex, int columnIndex) {
			return VirtualSource.GetPropertyValue(rowIndex, columnIndex);
		}
		public void SetValue(int rowIndex, int columnIndex, object value) {
			VirtualSource.SetPropertyValue(rowIndex, columnIndex, value);
		}
	}
	public class VirtualPropertyDescriptor : PropertyDescriptor {
		readonly Type propertyType;
		readonly bool isReadOnly;
		readonly VirtualSourceCore list;
		readonly int index;
		readonly string description;
		readonly string category;
		readonly string displayName;
		public VirtualPropertyDescriptor(VirtualSourceCore list, int index, string name, string displayName, string description, string category, Type propertyType, bool isReadOnly)
			: base(name, null) {
			this.index = index;
			this.displayName = displayName;
			this.description = description;
			this.category = category;
			this.propertyType = propertyType;
			this.isReadOnly = isReadOnly;
			this.list = list;
		}
		public override bool CanResetValue(object component) {
			return false;
		}
		public override object GetValue(object component) {
			return list.GetValue((int)component, index);
		}
		public override void SetValue(object component, object val) {
			list.SetValue((int)component, index, val);
		}
		public override bool IsReadOnly { get { return isReadOnly; } }
		public override Type ComponentType { get { return typeof(VirtualSourceCore); } }
		public override Type PropertyType { get { return propertyType; } }
		public override string Description { get { return description; } }
		public override string Category { get { return category; } }
		public override string DisplayName { get { return displayName; } }
		public override void ResetValue(object component) { }
		public override bool ShouldSerializeValue(object component) { return true; }
		public VirtualPropertyDescriptor Clone(VirtualSourceCore virtualList) {
			return new VirtualPropertyDescriptor(virtualList, index, Name, DisplayName, Description, Category, PropertyType, IsReadOnly);
		}
	}
}
