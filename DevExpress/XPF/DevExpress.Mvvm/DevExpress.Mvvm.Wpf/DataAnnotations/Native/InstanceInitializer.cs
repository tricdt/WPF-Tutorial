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

using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
namespace DevExpress.Mvvm.Native {
	public interface IInstanceInitializer {
		IEnumerable<TypeInfo> Types { get; }
		object CreateInstance(TypeInfo type);
	}
	public interface IDictionaryItemInstanceInitializer : IInstanceInitializer {
		KeyValuePair<object, object>? CreateInstance(TypeInfo type, ITypeDescriptorContext context, IEnumerable dictionary);
	}
	public class TypeInfo {
		public Type Type { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }
		public TypeInfo(Type type)
			: this(type, null) {
		}
		public TypeInfo(Type type, string name) {
			if(Object.ReferenceEquals(type, null))
				throw new ArgumentNullException(nameof(type));
			Type = type;
			Name = name;
		}
		public TypeInfo(Type type, string name, string description) : this(type, name){
			Description = description;
		}
		public override string ToString() {
			return Name ?? Type.Name;
		}
		public override int GetHashCode() {
			return Type.GetHashCode() ^ (Name ?? string.Empty).GetHashCode();
		}
		public override bool Equals(object obj) {
			if(obj == null)
				return false;
			var typeInfo = obj as TypeInfo;
			if(typeInfo == null)
				return false;
			return Name == typeInfo.Name && Type == typeInfo.Type && Description == typeInfo.Description;
		}
	}
}
