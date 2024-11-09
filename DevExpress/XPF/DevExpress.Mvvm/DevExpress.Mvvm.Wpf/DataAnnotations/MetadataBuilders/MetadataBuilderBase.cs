﻿#region Copyright (c) 2000-2024 Developer Express Inc.
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
using System.Linq.Expressions;
using System.Reflection;
namespace DevExpress.Mvvm.DataAnnotations {
	public abstract class MetadataBuilderBase<T, TMetadataBuilder> : 
		IAttributesProvider,
		IAttributeBuilderInternal,
		IAttributeBuilderInternal<TMetadataBuilder>
		where TMetadataBuilder : MetadataBuilderBase<T, TMetadataBuilder> {
		Dictionary<string, MemberMetadataStorage> storages = new Dictionary<string, MemberMetadataStorage>();
		IEnumerable<Attribute> IAttributesProvider.GetAttributes(string propertyName) {
			MemberMetadataStorage storage;
			storages.TryGetValue(propertyName ?? string.Empty, out storage);
			return storage != null ? storage.GetAttributes() : null;
		}
		internal TBuilder GetBuilder<TBuilder>(string memberName, Func<MemberMetadataStorage, TBuilder> createBuilderCallBack) where TBuilder : IPropertyMetadataBuilder {
			MemberMetadataStorage storage = storages.GetOrAdd(memberName ?? string.Empty, () => new MemberMetadataStorage());
			return (TBuilder)createBuilderCallBack(storage);
		}
		protected internal TMetadataBuilder AddOrReplaceAttribute<TAttribute>(TAttribute attribute) where TAttribute : Attribute {
			GetBuilder<IPropertyMetadataBuilder>(null, x => {
				x.AddOrReplaceAttribute(attribute);
				return null;
			});
			return (TMetadataBuilder)this;
		}
		protected internal TMetadataBuilder AddOrModifyAttribute<TAttribute>(Action<TAttribute> setAttributeValue) where TAttribute : Attribute, new() {
			GetBuilder<IPropertyMetadataBuilder>(null, x => {
				x.AddOrModifyAttribute(setAttributeValue);
				return null;
			});
			return (TMetadataBuilder)this;
		}
		TMetadataBuilder IAttributeBuilderInternal<TMetadataBuilder>.AddOrReplaceAttribute<TAttribute>(TAttribute attribute) {
			return AddOrReplaceAttribute(attribute);
		}
		TMetadataBuilder IAttributeBuilderInternal<TMetadataBuilder>.AddOrModifyAttribute<TAttribute>(Action<TAttribute> setAttributeValue) {
			return AddOrModifyAttribute(setAttributeValue);
		}
		void IAttributeBuilderInternal.AddOrReplaceAttribute<TAttribute>(TAttribute attribute) {
			AddOrReplaceAttribute(attribute);
		}
		void IAttributeBuilderInternal.AddOrModifyAttribute<TAttribute>(Action<TAttribute> setAttributeValue) {
			AddOrModifyAttribute(setAttributeValue);
		}
		internal static string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression) {
			return ExpressionHelper.GetArgumentPropertyStrict(expression).Name;
		}
#if !FREE
		public ClassTypeConverterBuilder<T, TMetadataBuilder> TypeConverter() {
			return new ClassTypeConverterBuilder<T, TMetadataBuilder>((TMetadataBuilder)this);
		}
		public TMetadataBuilder TypeConverter<TConverter>() where TConverter : TypeConverter, new() {
			return AddOrModifyAttribute<TypeConverterWrapperAttribute>(x => x.BaseConverterType = typeof(TConverter));
		}
		public TMetadataBuilder DefaultEditor(object templateKey) {
			IAttributeBuilderInternal<TMetadataBuilder> builder = this;
			return builder.AddOrModifyAttribute((DefaultEditorAttribute a) => a.TemplateKey = templateKey);
		}
		public TMetadataBuilder GridEditor(object templateKey) {
			IAttributeBuilderInternal<TMetadataBuilder> builder = this;
			return builder.AddOrModifyAttribute((GridEditorAttribute a) => a.TemplateKey = templateKey);
		}
		public TMetadataBuilder LayoutControlEditor(object templateKey) {
			IAttributeBuilderInternal<TMetadataBuilder> builder = this;
			return builder.AddOrModifyAttribute((LayoutControlEditorAttribute a) => a.TemplateKey = templateKey);
		}
		public TMetadataBuilder PropertyGridEditor(object templateKey) {
			IAttributeBuilderInternal<TMetadataBuilder> builder = this;
			return builder.AddOrModifyAttribute((PropertyGridEditorAttribute a) => a.TemplateKey = templateKey);
		}
#endif
	}
}
