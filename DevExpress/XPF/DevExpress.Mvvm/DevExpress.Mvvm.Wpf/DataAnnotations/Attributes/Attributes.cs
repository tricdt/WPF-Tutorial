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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
namespace DevExpress.Mvvm.DataAnnotations {
	public abstract class OrderAttribute : Attribute {
		int? order;
		public int Order {
			get {
				if(order == null)
					throw new InvalidOperationException();
				return order.Value;
			}
			set { 
				order = value; 
			}
		}
		public int? GetOrder() {
			return order;
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
	public class ToolBarItemAttribute : OrderAttribute {
		public string Page { get; set; }
		public string PageGroup { get; set; }
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
	public class ContextMenuItemAttribute : OrderAttribute {
		public string Group { get; set; }
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false)]
	public class CommandParameterAttribute : Attribute {
		public string CommandParameter { get; private set; }
		public CommandParameterAttribute(string commandParameter) {
			this.CommandParameter = commandParameter;
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class HiddenAttribute : Attribute {
		public HiddenAttribute()
			: this(true) {
		}
		public HiddenAttribute(bool hidden) {
			Hidden = hidden;
		}
		public bool Hidden { get; set; }
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public abstract class InstanceInitializerAttributeBase : Attribute {
		readonly Func<object> createInstanceCallback;
		protected InstanceInitializerAttributeBase(Type type) 
			: this(type, type.Name, null, null) {
		}
		protected InstanceInitializerAttributeBase(Type type, string name, string description, Func<object> createInstanceCallback) {
			if(Object.ReferenceEquals(type, null))
				throw new ArgumentNullException(nameof(type));
			if(string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));
			this.Type = type;
			this.Name = name;
			this.Description = description;
			this.createInstanceCallback = createInstanceCallback;
		}
		public string Name { get; private set; }
		public Type Type { get; private set; }
		public string Description { get; private set; }
		public virtual object CreateInstance() {
			return createInstanceCallback != null ? createInstanceCallback() : Activator.CreateInstance(Type);
		}
		public override bool Equals(object obj) {
			InstanceInitializerAttributeBase @base = obj as InstanceInitializerAttributeBase;
			return @base != null &&
				   base.Equals(obj) &&
				   EqualityComparer<Func<object>>.Default.Equals(createInstanceCallback, @base.createInstanceCallback) &&
				   Name == @base.Name &&
				   EqualityComparer<Type>.Default.Equals(Type, @base.Type) &&
				   Description == @base.Description;
		}
		public override int GetHashCode() {
			int hashCode = -2079329921;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Func<object>>.Default.GetHashCode(createInstanceCallback);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
			hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(Type);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
			return hashCode;
		}
#if !WINUI
		public override object TypeId { get { return Name; } }
#endif
	}
	public class InstanceInitializerAttribute : InstanceInitializerAttributeBase {
		public InstanceInitializerAttribute(Type type)
			: base(type) {
		}
		public InstanceInitializerAttribute(Type type, string name)
			: base(type, name, null, null) {
		}
		public InstanceInitializerAttribute(Type type, string name, string description)
			: base(type, name, description, null) {
		}
		internal InstanceInitializerAttribute(Type type, string name, string description, Func<object> createInstanceCallback)
			: base(type, name, description, createInstanceCallback) {
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ItemDisplayMemberAttribute : Attribute {
		public string DisplayMember { get; set; }
		public string DescriptionMember { get; set; }
		public ItemDisplayMemberAttribute(string displayMember) {
			DisplayMember = displayMember;
		}
		public ItemDisplayMemberAttribute() {
		}
	}
	public class NewItemInstanceInitializerAttribute : InstanceInitializerAttributeBase {
		Func<ITypeDescriptorContext, IEnumerable, KeyValuePair<object, object>?> createDictionaryInstanceCallback;
		public NewItemInstanceInitializerAttribute(Type type)
			: base(type) {
		}
		public NewItemInstanceInitializerAttribute(Type type, string name)
			: base(type, name, null, null) {
		}
		public NewItemInstanceInitializerAttribute(Type type, string name, string description)
			: base(type, name, description, null) {
		}
		internal NewItemInstanceInitializerAttribute(Type type, string name,  string description, Func<object> createInstanceCallback)
			: base(type, name, description, createInstanceCallback) {
		}
		internal NewItemInstanceInitializerAttribute(Type type, string name, string description, Func<ITypeDescriptorContext, IEnumerable, KeyValuePair<object, object>?> createDictionaryInstanceCallback)
			: base(type, name, description, null) {
			this.createDictionaryInstanceCallback = createDictionaryInstanceCallback;
		}
		public virtual KeyValuePair<object, object>? CreateInstance(ITypeDescriptorContext context, IEnumerable dictionary) {
			return createDictionaryInstanceCallback.Return(x => x(context, dictionary), () => null);
		}
		public override bool Equals(object obj) {
			NewItemInstanceInitializerAttribute attribute = obj as NewItemInstanceInitializerAttribute;
			return attribute != null &&
				   base.Equals(obj) &&
				   Equals(createDictionaryInstanceCallback, attribute.createDictionaryInstanceCallback);
		}
		public override int GetHashCode() {
			int hashCode = -1836809165;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Func<ITypeDescriptorContext, IEnumerable, KeyValuePair<object, object>?>>.Default.GetHashCode(createDictionaryInstanceCallback);
			return hashCode;
		}
	}
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
	public class ScaffoldDetailCollectionAttribute : Attribute {
		public const bool DefaultScaffold = true;
		public ScaffoldDetailCollectionAttribute()
			: this(DefaultScaffold) {
		}
		public ScaffoldDetailCollectionAttribute(bool scaffold) {
			this.Scaffold = scaffold;
		}
		public bool Scaffold { get; private set; }
	}
}
