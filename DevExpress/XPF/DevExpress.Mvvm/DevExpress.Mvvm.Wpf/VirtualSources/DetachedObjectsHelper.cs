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

using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
namespace DevExpress.Xpf.Data {
	public class DetachedObjectsHelper<T> {
		public static DetachedObjectsHelper<T> Create(string keyProperty, string[] properties) {
			return new DetachedObjectsHelper<T>(keyProperty, properties);
		}
		readonly string keyProperty;
		public PropertyDescriptorCollection Properties { get; }
		DetachedObjectsHelper(string keyProperty, string[] properties) {
			this.keyProperty = keyProperty;
			var originalProperties = TypeDescriptor.GetProperties(typeof(T));
			Properties = new PropertyDescriptorCollection(properties.Concat(keyProperty.Yield()).Distinct()
				.Select(x => new DetachedPropertyDescriptor(originalProperties[x]))
				.ToArray()
			);
		}
		public object GetKey(object detachedObject) {
			return Properties[keyProperty].GetValue(detachedObject);
		}
		public void SetKey(object detachedObject, object key) {
			Properties[keyProperty].SetValue(detachedObject, key);
		}
		public object[] ConvertToDetachedObjects(IEnumerable<T> source) {
			return source.Select(item => {
				var values = Properties
					.Cast<DetachedPropertyDescriptor>()
					.ToDictionary(property => property.Name, property => property.SourceProperty.GetValue(item));
				return new DetachedOject(values);
			}).ToArray<object>();
		}
		public void ApplyProperties(T item, object detachedObject) {
			foreach(DetachedPropertyDescriptor property in Properties) {
				if(!property.SourceProperty.IsReadOnly)
					property.SourceProperty.SetValue(item, property.GetValue(detachedObject));
			}
		}
		class DetachedOject {
			public readonly Dictionary<string, object> Values;
			public DetachedOject(Dictionary<string, object> values) {
				Values = values;
			}
			public DetachedOject()
				: this(new Dictionary<string, object>()) {
			}
		}
		class DetachedPropertyDescriptor : PropertyDescriptor {
			static object GetDefaultValue(Type type) {
				return type.IsValueType ? Activator.CreateInstance(type) : null;
			}
			internal readonly PropertyDescriptor SourceProperty;
			public DetachedPropertyDescriptor(PropertyDescriptor property)
				: base(property.Name, property.Attributes.Cast<Attribute>().ToArray()) {
				SourceProperty = property;
			}
			public override object GetValue(object component) {
				return DictionaryExtensions.GetValueOrDefault(((DetachedOject)component).Values, Name, GetDefaultValue(PropertyType));
			}
			public override void SetValue(object component, object value) {
				((DetachedOject)component).Values[Name] = value;
			}
			public override Type ComponentType => typeof(DetachedOject);
			public override bool IsReadOnly => SourceProperty.IsReadOnly;
			public override Type PropertyType => SourceProperty.PropertyType;
			public override bool CanResetValue(object component) => false;
			public override void ResetValue(object component) {
				throw new NotSupportedException();
			}
			public override bool ShouldSerializeValue(object component) => false;
		}
	}
}
