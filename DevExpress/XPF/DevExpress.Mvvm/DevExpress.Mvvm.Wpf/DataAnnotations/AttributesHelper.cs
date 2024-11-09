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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
namespace DevExpress.Mvvm.DataAnnotations {
	public static class AttributesHelper {
		public static AttributeCollection GetAttributes(Type type) {
			return GetAttributes(null, type);
		}
		public static AttributeCollection GetAttributes<T>(Expression<Func<T, object>> property) {
			return GetAttributes(typeof(T), ExpressionHelper.GetPropertyName(property));
		}
		public static AttributeCollection GetAttributes(Type type, string property) {
			return GetAttributes(TypeDescriptor.GetProperties(type)[property], type);
		}
		public static AttributeCollection GetAttributes(PropertyDescriptor property, Type ownerType = null, IAttributesProvider attributesProvider = null) {
			IEnumerable<Attribute> externalAndFluentAPIAttrbutes;
			if (property != null && property.Attributes.OfType<Attribute>().Any(x => x.GetType().FullName == FilterAttributeProxy.FilterAttributeName))
				externalAndFluentAPIAttrbutes = MetadataHelper.GetExternalAndFluentAPIFilteringAttributes(ownerType, property != null ? property.Name : null);
			else externalAndFluentAPIAttrbutes = MetadataHelper.GetExternalAndFluentAPIAttributes(ownerType, property != null ? property.Name : null);
			IEnumerable<Attribute> attributes = property != null
				? property.Attributes.Cast<Attribute>()
				: TypeDescriptor.GetAttributes(ownerType).Cast<Attribute>();
			IEnumerable<Attribute> attributesFromProvider =
				attributesProvider.With(x => x.GetAttributes(property != null ? property.Name : null)) ?? Enumerable.Empty<Attribute>();
			return new AttributeCollection(UnionAttributes(new[] { attributes, externalAndFluentAPIAttrbutes, attributesFromProvider }).ToArray());
		}
		static IEnumerable<Attribute> UnionAttributes(params IEnumerable<Attribute>[] attributesInPriorityOrder) {
			List<Attribute> res = new List<Attribute>();
			if(attributesInPriorityOrder.Length == 0) return res;
			res.AddRange(attributesInPriorityOrder.Take(1).SelectMany(x => x));
			foreach(var attributes in attributesInPriorityOrder.Skip(1)) {
				List<Attribute> currentRes = new List<Attribute>();
				foreach(var attribute in attributes) {
					if(attribute is DisplayAttribute) {
						DisplayAttribute resAttribute = (DisplayAttribute)attribute;
						var lowPriority = res.OfType<DisplayAttribute>().FirstOrDefault();
						if(lowPriority != null) {
							res.Remove(lowPriority);
							resAttribute = UnionAttributes(lowPriority, resAttribute);
						}
						lowPriority = currentRes.OfType<DisplayAttribute>().FirstOrDefault();
						if(lowPriority != null) {
							currentRes.Remove(lowPriority);
							resAttribute = UnionAttributes(lowPriority, resAttribute);
						}
						currentRes.Add(resAttribute);
					} else currentRes.Add(attribute);
				}
				res.InsertRange(0, currentRes);
			}
			return res;
		}
		static DisplayAttribute UnionAttributes(DisplayAttribute lowPriority, DisplayAttribute highPriority) {
			DisplayAttribute res = new DisplayAttribute();
			UnionAttributeValue(lowPriority, highPriority, x => x.GetAutoGenerateField(), x => res.AutoGenerateField = x.Value);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetAutoGenerateFilter(), x => res.AutoGenerateFilter = x.Value);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetDescription(), x => res.Description = x);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetGroupName(), x => res.GroupName = x);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetName(), x => res.Name = x);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetOrder(), x => res.Order = x.Value);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetPrompt(), x => res.Prompt = x);
			UnionAttributeValue(lowPriority, highPriority, x => x.GetShortName(), x => res.ShortName = x);
			return res;
		}
		static void UnionAttributeValue<TAttribute, TValue>(TAttribute lowPriority, TAttribute highPriority, Func<TAttribute, TValue> getValue, Action<TValue> result) where TAttribute : Attribute {
			var high = getValue(highPriority);
			if(high != null) {
				result(high);
				return;
			}
			var low = getValue(lowPriority);
			if(low != null) {
				result(low);
				return;
			}
		}
	}
}
