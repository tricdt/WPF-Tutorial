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
#if !FREE
	public enum PropertyLocation {
		BeforePropertiesWithoutSpecifiedLocation,
		AfterPropertiesWithoutSpecifiedLocation
	}
#endif
	public abstract class PropertyMetadataBuilderBase<T, TProperty, TBuilder> :
		MemberMetadataBuilderBase<T, TBuilder, ClassMetadataBuilder<T>>
		where TBuilder : PropertyMetadataBuilderBase<T, TProperty, TBuilder> {
		internal PropertyMetadataBuilderBase(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent)
			: base(storage, parent) {
		}
#if !FREE
		internal TBuilder GroupName(string groupName, LayoutType layoutType) {
			if(layoutType == LayoutType.DataForm)
				return AddOrReplaceAttribute(new DataFormGroupAttribute(groupName, parent.CurrentDataFormLayoutOrder++));
			if(layoutType == LayoutType.Table)
				return AddOrReplaceAttribute(new TableGroupAttribute(groupName, parent.CurrentTableLayoutOrder++));
			throw new NotSupportedException();
		}
		protected TBuilder DefaultEditorCore(object templateKey) {
			return AddOrModifyAttribute<DefaultEditorAttribute>(x => x.TemplateKey = templateKey);
		}
		protected TBuilder GridEditorCore(object templateKey) {
			return AddOrModifyAttribute<GridEditorAttribute>(x => x.TemplateKey = templateKey);
		}
		protected TBuilder LayoutControlEditorCore(object templateKey) {
			return AddOrModifyAttribute<LayoutControlEditorAttribute>(x => x.TemplateKey = templateKey);
		}
		protected TBuilder PropertyGridEditorCore(object templateKey) {
			return AddOrModifyAttribute<PropertyGridEditorAttribute>(x => x.TemplateKey = templateKey);
		}
		protected TBuilder HiddenCore(bool hidden = true) {
			return AddOrModifyAttribute<HiddenAttribute>(x => x.Hidden = hidden);
		}
		protected TBuilder ReadOnlyCore() {
			return DataAnnotationsAttributeHelper.SetReadonly((TBuilder)this);
		}
		protected TBuilder NotEditableCore() {
			return DataAnnotationsAttributeHelper.SetNotEditable((TBuilder)this);
		}
		protected TBuilder InitializerCore<TValue>(Func<TValue> createDelegate, string name, string description) {
			return InitializerCore(createDelegate, name, description, (t, n, d, c) => new InstanceInitializerAttribute(t, n, d, c));
		}
		internal TBuilder InitializerCore<TValue, TInstanceInitializerAttribute>(Func<TValue> createDelegate, string name, string description, Func<Type, string, string, Func<object>, TInstanceInitializerAttribute> attributeFactory)
			where TInstanceInitializerAttribute : InstanceInitializerAttributeBase {
			return AddAttribute(attributeFactory(typeof(TValue), name ?? typeof(TValue).Name, description, () => createDelegate()));
		}
		internal TBuilder InitializerCore<TKey, TValue, TNewItem, TInstanceInitializerAttribute>(Func<ITypeDescriptorContext, IDictionary<TKey, TValue>, KeyValuePair<TKey, TNewItem>> createDelegate, string name, string description, Func<Type, string, string, Func<ITypeDescriptorContext, IDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>>, TInstanceInitializerAttribute> attributeFactory) where TNewItem : TValue
			where TInstanceInitializerAttribute : InstanceInitializerAttributeBase {
			return AddAttribute(attributeFactory(typeof(TNewItem), name ?? typeof(TNewItem).Name, description, (c, d) => {
				var x = createDelegate(c, d);
				return new KeyValuePair<TKey, TValue>(x.Key, x.Value);
			}));
		}
		internal TBuilder InitializerCore<TNewItem, TInstanceInitializerAttribute>(Func<ITypeDescriptorContext, IDictionary, KeyValuePair<object, TNewItem>> createDelegate, string name, Func<Type, string, Func<ITypeDescriptorContext, IDictionary, KeyValuePair<object, object>>, TInstanceInitializerAttribute> attributeFactory)
			where TInstanceInitializerAttribute : InstanceInitializerAttributeBase {
			return AddAttribute(attributeFactory(typeof(TNewItem), name ?? typeof(TNewItem).Name, (c, d) => {
				var x = createDelegate(c, d);
				return new KeyValuePair<object, object>(x.Key, x.Value);
			}));
		}
		protected TypeConverterBuilder<T, TProperty, TBuilder> TypeConverterCore() {
			return new TypeConverterBuilder<T, TProperty, TBuilder>((TBuilder)this);
		}
		protected TBuilder TypeConverterCore<TConverter>() where TConverter : TypeConverter, new() {
			return AddOrModifyAttribute<TypeConverterWrapperAttribute>(x => x.BaseConverterType = typeof(TConverter));
		}
		protected TBuilder DoNotConvertEmptyStringToNullCore() {
			return DataAnnotationsAttributeHelper.SetConvertEmptyStringToNull((TBuilder)this, false);
		}
		protected TBuilder NullDisplayTextCore(string nullDisplayText) {
			return DataAnnotationsAttributeHelper.SetNullDisplayText((TBuilder)this, nullDisplayText);
		}
		protected TBuilder DisplayFormatStringCore(string dataFormatString, bool applyDisplayFormatInEditMode = false) {
			return DataAnnotationsAttributeHelper.SetDataFormatString((TBuilder)this, dataFormatString, applyDisplayFormatInEditMode);
		}
#endif
		protected TBuilder RequiredCore(bool allowEmptyStrings = false, Func<string> errorMessageAccessor = null) {
			return AddOrReplaceAttribute(new DXRequiredAttribute(allowEmptyStrings, errorMessageAccessor));
		}
		protected TBuilder RequiredCore(Func<string> errorMessageAccessor) {
			return RequiredCore(false, errorMessageAccessor);
		}
		protected TBuilder MaxLengthCore(int maxLength, Func<TProperty, string> errorMessageAccessor) {
			var _errorMessageAccessor = DXValidationAttribute.ErrorMessageAccessor(errorMessageAccessor);
			return AddOrReplaceAttribute(new DXMaxLengthAttribute(maxLength, _errorMessageAccessor));
		}
		protected TBuilder MinLengthCore(int minLength, Func<TProperty, string> errorMessageAccessor) {
			var _errorMessageAccessor = DXValidationAttribute.ErrorMessageAccessor(errorMessageAccessor);
			return AddOrReplaceAttribute(new DXMinLengthAttribute(minLength, _errorMessageAccessor));
		}
		protected TBuilder MatchesRegularExpressionCore(string pattern, Func<TProperty, string> errorMessageAccessor) {
			var _errorMessageAccessor = DXValidationAttribute.ErrorMessageAccessor(errorMessageAccessor);
			return AddOrReplaceAttribute(new RegularExpressionAttribute(pattern, _errorMessageAccessor));
		}
		protected TBuilder MatchesRuleCore(Func<TProperty, bool> isValidFunction, Func<TProperty, string> errorMessageAccessor) {
			var _errorMessageAccessor = DXValidationAttribute.ErrorMessageAccessor(errorMessageAccessor);
			return AddAttribute(new CustomValidationAttribute(typeof(TProperty), x => isValidFunction((TProperty)x), _errorMessageAccessor));
		}
		protected TBuilder MatchesInstanceRuleCore(Func<TProperty, T, bool> isValidFunction, Func<TProperty, T, string> errorMessageAccessor) {
			var _errorMessageAccessor = DXValidationAttribute.ErrorMessageAccessor(errorMessageAccessor);
			return AddAttribute(new CustomInstanceValidationAttribute(typeof(TProperty), (value, instance) => isValidFunction((TProperty)value, (T)instance), _errorMessageAccessor));
		}
		[Obsolete("Use the MatchesInstanceRule(Func<TProperty, T, bool> isValidFunction, Func<string> errorMessageAccessor = null) method instead.")]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		protected TBuilder MatchesInstanceRuleCore(Func<T, bool> isValidFunction, Func<string> errorMessageAccessor = null) {
			return AddOrReplaceAttribute(new CustomInstanceValidationAttribute(typeof(T), (value, instance) => isValidFunction((T)instance), 
				errorMessageAccessor == null ? null : new DXValidationAttribute.ErrorMessageAccessorDelegate((x, y) => errorMessageAccessor()) ));
		}
		protected static Func<TProperty, string> GetErrorMessageAccessor(Func<string> errorMessageAccessor) {
			if(errorMessageAccessor == null) return null;
			return x => errorMessageAccessor();
		}
	}
}
