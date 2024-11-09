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

using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security;
namespace DevExpress.Mvvm.Native {
	[SecuritySafeCritical]
	public static class DataAnnotationsAttributeHelper {
		internal static bool HasRequiredAttribute(MemberInfo member) {
			return MetadataHelper.GetAttribute<RequiredAttribute>(member) != null;
		}
		#region scaffolding
		internal static Type GetScaffoldColumnAttributeType() {
			return typeof(ScaffoldColumnAttribute);
		}
		internal static LambdaExpression GetScaffoldColumnAttributeCreateExpression() {
			Expression<Func<ScaffoldColumnAttribute>> expression = () => new ScaffoldColumnAttribute(default(bool));
			return expression;
		}
		internal static IEnumerable<object> GetScaffoldColumnAttributeConstructorParameters(Attribute attribute) {
			return new object[] { ((ScaffoldColumnAttribute)attribute).Scaffold };
		}
		internal static TBuilder DoNotScaffoldCore<TBuilder>(TBuilder builder) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrReplaceAttribute(new ScaffoldColumnAttribute(false));
		}
#if !FREE
		internal static TBuilder DoNotScaffoldDetailCollectionCore<TBuilder>(TBuilder builder) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrReplaceAttribute(new ScaffoldDetailCollectionAttribute(false));
		}
#endif
		#endregion
		internal static Type GetDisplayAttributeType() {
			return typeof(DisplayAttribute);
		}
		internal static LambdaExpression GetDisplayAttributeCreateExpression() {
			Expression<Func<DisplayAttribute>> expression = () => new DisplayAttribute();
			return expression;
		}
		internal static IEnumerable<Tuple<PropertyInfo, object>> GetDisplayAttributePropertyValuePairs(Attribute attributeBase) {
			DisplayAttribute attribute = (DisplayAttribute)attributeBase;
			List<Tuple<PropertyInfo, object>> result = new List<Tuple<PropertyInfo, object>>();
			if(attribute.GetOrder() != null)
				result.Add(GetPropertyValuePair(attribute, x => x.Order));
			if(attribute.GetAutoGenerateField() != null)
				result.Add(GetPropertyValuePair(attribute, x => x.AutoGenerateField));
			result.Add(GetPropertyValuePair(attribute, x => x.Name));
			result.Add(GetPropertyValuePair(attribute, x => x.ShortName));
			result.Add(GetPropertyValuePair(attribute, x => x.Description));
			return result;
		}
		internal static string GetDisplayName(IEnumerable<Attribute> attributes) {
			var display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
			return display.Return(x => x.GetName() ?? x.GetShortName(), () => GetDisplayNameFromDisplayNameAttribute(attributes));
		}
		static string GetDisplayNameFromDisplayNameAttribute(IEnumerable<Attribute> attributes) {
			return attributes.OfType<DisplayNameAttribute>().FirstOrDefault().With(y => y.DisplayName);
		}
		internal static Tuple<PropertyInfo, object> GetPropertyValuePair<TAttribute, TProperty>(TAttribute attribute, Expression<Func<TAttribute, TProperty>> propertyExpression) {
			PropertyInfo property = ExpressionHelper.GetArgumentPropertyStrict(propertyExpression);
			return new Tuple<PropertyInfo, object>(property, property.GetValue(attribute, null));
		}
		internal static TBuilder DisplayNameCore<TBuilder>(TBuilder builder, string name) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayAttribute>(x => x.Name = name);
		}
		internal static TBuilder DisplayShortNameCore<TBuilder>(TBuilder builder, string shortName) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayAttribute>(x => x.ShortName = shortName);
		}
		internal static TBuilder DescriptionCore<TBuilder>(TBuilder builder, string description) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayAttribute>(x => x.Description = description);
		}
		internal static TBuilder AutoGeneratedCore<TBuilder>(TBuilder builder, bool autoGenerate) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayAttribute>(x => x.AutoGenerateField = autoGenerate);
		}
		public static bool GetAutoGenerateField(FieldInfo field) {
			return GetFieldDisplayAttribute(field).Return(x => x.GetAutoGenerateField().GetValueOrDefault(true), () => true);
		}
		public static string GetFieldDisplayName(FieldInfo field) {
			return GetFieldDisplayAttribute(field).With(x => x.GetName() ?? x.GetShortName());
		}
		public static string GetFieldDescription(FieldInfo field) {
			return GetFieldDisplayAttribute(field).With(x => x.GetDescription());
		}
		public static int? GetFieldOrder(FieldInfo field) {
			return (GetFieldDisplayAttribute(field) == null) ? null : GetFieldDisplayAttribute(field).GetOrder();
		}
		public static bool IsBrowsable(FieldInfo field) {
			return MetadataHelper.GetAttribute<BrowsableAttribute>(field)?.Browsable ?? true;
		}
		static DisplayAttribute GetFieldDisplayAttribute(FieldInfo field) {
			return MetadataHelper.GetAttribute<DisplayAttribute>(field);
		}
#if !FREE
		#region DisplayAttribute
		internal static TBuilder SetOrderCore<TBuilder>(TBuilder builder, int order) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayAttribute>(x => x.Order = order);
		}
		#endregion
		#region Display Format
		internal static TBuilder SetConvertEmptyStringToNull<TBuilder>(TBuilder builder, bool convert) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayFormatAttribute>(x => x.ConvertEmptyStringToNull = convert);
		}
		internal static TBuilder SetDataFormatString<TBuilder>(TBuilder builder, string dataFormatString, bool applyDisplayFormatInEditMode) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayFormatAttribute>(x => { x.DataFormatString = dataFormatString; x.ApplyFormatInEditMode = applyDisplayFormatInEditMode; });
		}
		internal static TBuilder SetNullDisplayText<TBuilder>(TBuilder builder, string nullDisplayText) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrModifyAttribute<DisplayFormatAttribute>(x => x.NullDisplayText = nullDisplayText);
		}
		#endregion
		internal static TBuilder SetReadonly<TBuilder>(TBuilder builder) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrReplaceAttribute(new ReadOnlyAttribute(true));
		}
		internal static TBuilder SetNotEditable<TBuilder>(TBuilder builder) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrReplaceAttribute(new EditableAttribute(false));
		}
#endif
		internal static TBuilder SetDataTypeCore<TBuilder>(TBuilder builder, PropertyDataType dataType) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrReplaceAttribute(new DataTypeAttribute(ToDataType(dataType)));
		}
		internal static TBuilder SetEnumDataTypeCore<TBuilder>(TBuilder builder, Type enumDataType) where TBuilder : IAttributeBuilderInternal<TBuilder> {
			return builder.AddOrReplaceAttribute(new EnumDataTypeAttribute(enumDataType));
		}
		#region data type conversion
#if !FREE
		public static PropertyDataType FromDataType(DataType dataType) {
			switch(dataType) {
				case DataType.Currency:
					return PropertyDataType.Currency;
				case DataType.Password:
					return PropertyDataType.Password;
				case DataType.MultilineText:
					return PropertyDataType.MultilineText;
				case DataType.PhoneNumber:
					return PropertyDataType.PhoneNumber;
				case DataType.Url:
					return PropertyDataType.Url;
				case DataType.ImageUrl:
					return PropertyDataType.ImageUrl;
				case DataType.Time:
					return PropertyDataType.Time;
				case DataType.DateTime:
					return PropertyDataType.DateTime;
				case DataType.Date:
					return PropertyDataType.Date;
				default:
					return PropertyDataType.Custom;
			}
		}
#endif
		static DataType ToDataType(PropertyDataType dataType) {
			switch(dataType) {
				case PropertyDataType.Currency:
					return DataType.Currency;
				case PropertyDataType.Password:
					return DataType.Password;
				case PropertyDataType.MultilineText:
					return DataType.MultilineText;
				case PropertyDataType.PhoneNumber:
					return DataType.PhoneNumber;
				case PropertyDataType.ImageUrl:
					return DataType.ImageUrl;
				case PropertyDataType.Time:
					return DataType.Time;
				case PropertyDataType.DateTime:
					return DataType.DateTime;
				case PropertyDataType.Date:
					return DataType.Date;
				default:
					return DataType.Custom;
			}
		}
		#endregion
	}
}
