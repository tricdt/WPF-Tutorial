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
using System.ComponentModel;
using System.Linq.Expressions;
namespace DevExpress.Mvvm.DataAnnotations {
	public abstract class PropertyMetadataBuilderGeneric<T, TProperty, TBuilder> : 
		PropertyMetadataBuilderBase<T, TProperty, TBuilder>
		where TBuilder : PropertyMetadataBuilderGeneric<T, TProperty, TBuilder> {
		internal PropertyMetadataBuilderGeneric(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent)
			: base(storage, parent) {
		}
		public MetadataBuilder<T> EndProperty() { return (MetadataBuilder<T>)parent; }
		public TBuilder Required(bool allowEmptyStrings = false, Func<string> errorMessageAccessor = null) { return RequiredCore(allowEmptyStrings, errorMessageAccessor); }
		public TBuilder Required(Func<string> errorMessageAccessor) { return RequiredCore(errorMessageAccessor); }
		public TBuilder MaxLength(int maxLength, Func<string> errorMessageAccessor = null) { return MaxLengthCore(maxLength, GetErrorMessageAccessor(errorMessageAccessor)); }
		public TBuilder MaxLength(int maxLength, Func<TProperty, string> errorMessageAccessor) { return MaxLengthCore(maxLength, errorMessageAccessor); }
		public TBuilder MinLength(int minLength, Func<string> errorMessageAccessor = null) { return MinLengthCore(minLength, GetErrorMessageAccessor(errorMessageAccessor)); }
		public TBuilder MinLength(int minLength, Func<TProperty, string> errorMessageAccessor) { return MinLengthCore(minLength, errorMessageAccessor); }
		public TBuilder MatchesRegularExpression(string pattern, Func<string> errorMessageAccessor = null) { return MatchesRegularExpressionCore(pattern, GetErrorMessageAccessor(errorMessageAccessor)); }
		public TBuilder MatchesRegularExpression(string pattern, Func<TProperty, string> errorMessageAccessor) { return MatchesRegularExpressionCore(pattern, errorMessageAccessor); }
		public TBuilder MatchesRule(Func<TProperty, bool> isValidFunction, Func<string> errorMessageAccessor = null) { return MatchesRuleCore(isValidFunction, GetErrorMessageAccessor(errorMessageAccessor)); }
		public TBuilder MatchesRule(Func<TProperty, bool> isValidFunction, Func<TProperty, string> errorMessageAccessor) { return MatchesRuleCore(isValidFunction, errorMessageAccessor); }
		public TBuilder MatchesInstanceRule(Func<TProperty, T, bool> isValidFunction, Func<string> errorMessageAccessor = null) { return MatchesInstanceRuleCore(isValidFunction, errorMessageAccessor == null ? null : new Func<TProperty, T, string>((x,y) => errorMessageAccessor())); }
		public TBuilder MatchesInstanceRule(Func<TProperty, T, bool> isValidFunction, Func<TProperty, T, string> errorMessageAccessor) { return MatchesInstanceRuleCore(isValidFunction, errorMessageAccessor); }
		[Obsolete("Use the MatchesInstanceRule(Func<TProperty, T, bool> isValidFunction, Func<string> errorMessageAccessor = null) method instead.")]
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public TBuilder MatchesInstanceRule(Func<T, bool> isValidFunction, Func<string> errorMessageAccessor = null) { return MatchesInstanceRuleCore(isValidFunction, errorMessageAccessor); }
#if !FREE
		public TBuilder DisplayName(string name) { return DisplayNameCore(name); }
		public TBuilder DisplayShortName(string shortName) { return DisplayShortNameCore(shortName); }
		public TBuilder Description(string description) { return DescriptionCore(description); }
		public TBuilder NotAutoGenerated() { return NotAutoGeneratedCore(); }
		public TBuilder AutoGenerated() { return AutoGeneratedCore(); }
		public TBuilder DoNotScaffold() { return DoNotScaffoldCore(); }
		public TBuilder DoNotScaffoldDetailCollection() { return DoNotScaffoldDetailCollectionCore(); }
		public TBuilder LocatedAt(int position, PropertyLocation propertyLocation = PropertyLocation.BeforePropertiesWithoutSpecifiedLocation) { return LocatedAtCore(position, propertyLocation); }
		public TBuilder DefaultEditor(object templateKey) { return DefaultEditorCore(templateKey); }
		public TBuilder GridEditor(object templateKey) { return GridEditorCore(templateKey); }
		public TBuilder LayoutControlEditor(object templateKey) { return LayoutControlEditorCore(templateKey); }
		public TBuilder PropertyGridEditor(object templateKey) { return PropertyGridEditorCore(templateKey); }
		public TBuilder Hidden(bool hidden = true) { return HiddenCore(hidden); }
		public TBuilder ReadOnly() { return ReadOnlyCore(); }
		public TBuilder NotEditable() { return NotEditableCore(); }
		public TBuilder Initializer<TValue>(Func<TValue> createDelegate, string name = null, string description = null) { return InitializerCore<TValue>(createDelegate, name, description); }
		public TypeConverterBuilder<T, TProperty, TBuilder> TypeConverter() { return TypeConverterCore(); }
		public TBuilder TypeConverter<TConverter>() where TConverter : TypeConverter, new() { return TypeConverterCore<TConverter>(); }
		public TBuilder DoNotConvertEmptyStringToNull() { return DoNotConvertEmptyStringToNullCore(); }
		public TBuilder NullDisplayText(string nullDisplayText) { return NullDisplayTextCore(nullDisplayText); }
		public TBuilder DisplayFormatString(string dataFormatString, bool applyDisplayFormatInEditMode = false) { return DisplayFormatStringCore(dataFormatString, applyDisplayFormatInEditMode); }
		public TBuilder ConvertTo(Type type) {
			return AddOrReplaceAttribute(new ConvertToAttribute(type));
		}
#endif
		#region POCO
		public TBuilder DoNotMakeBindable() {
			return AddOrReplaceAttribute(new BindablePropertyAttribute(false));
		}
		public TBuilder MakeBindable() {
			return AddOrReplaceAttribute(new BindablePropertyAttribute());
		}
		public TBuilder OnPropertyChangedCall(Expression<Action<T>> onPropertyChangedExpression) {
			return AddOrModifyAttribute<BindablePropertyAttribute>(x => x.OnPropertyChangedMethod = ExpressionHelper.GetArgumentMethodStrict(onPropertyChangedExpression));
		}
		public TBuilder OnPropertyChangingCall(Expression<Action<T>> onPropertyChangingExpression) {
			return AddOrModifyAttribute<BindablePropertyAttribute>(x => x.OnPropertyChangingMethod = ExpressionHelper.GetArgumentMethodStrict(onPropertyChangingExpression));
		}
		public TBuilder ReturnsService(ServiceSearchMode searchMode = default(ServiceSearchMode)) {
			return ReturnsService(null, searchMode);
		}
		public TBuilder ReturnsService(string key, ServiceSearchMode searchMode = default(ServiceSearchMode)) {
			return AddOrReplaceAttribute(new ServicePropertyAttribute() { SearchMode = searchMode, Key = key });
		}
		public TBuilder DoesNotReturnService() {
			return AddOrReplaceAttribute(new ServicePropertyAttribute(false));
		}
		public TBuilder DependsOn(params Expression<Func<T, object>>[] propertyExpression) {
			foreach(var exp in propertyExpression) {
				var propName = ExpressionHelper.GetArgumentPropertyStrict(exp).Name;
				AddAttribute(new DependsOnPropertiesAttribute(propName));
			}
			return (TBuilder)this;
		}
		#endregion
	}
	public class PropertyMetadataBuilder<T, TProperty> :
		PropertyMetadataBuilderGeneric<T, TProperty, PropertyMetadataBuilder<T, TProperty>> {
		internal PropertyMetadataBuilder(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent)
			: base(storage, parent) { }
	}
}
