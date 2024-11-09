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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DevExpress.Data.Helpers;
using DevExpress.Utils;
using DevExpress.Xpf.Editors.Filtering;
using DevExpress.Xpf.Editors.Settings;
namespace DevExpress.Xpf.DemoBase {
	public class FilterFieldInfo : ImmutableObject {
		public string Caption { get; set; }
		public BaseEditSettings EditSettings { get; set; }
		public string FieldName { get; set; }
		public FilterFieldInfo(string caption, BaseEditSettings editSettings, string fieldName) {
			Caption = caption;
			EditSettings = editSettings;
			FieldName = fieldName;
		}
	}
	public static class FilterFieldsHelper {
		public static IEnumerable<FilterFieldInfo> GetFields(FrameworkElement owner, Type objectType, IEnumerable<string> hiddenPropertyNames, PropertyInfoCollection additionalProperties) {
			var columns = FilteredComponentHelper.GetColumnsByType(owner, objectType, false);
			var properties = TypeDescriptor.GetProperties(objectType)
			.Cast<PropertyDescriptor>()
			.Where(x => {
				if(!IsBrowsableProperty(x) || hiddenPropertyNames.Contains(x.Name))
					return false;
				if(x.PropertyType == typeof(string))
					return true;
				if(typeof(IEnumerable).IsAssignableFrom(x.PropertyType))
					return false;
				return true;
			})
			.Select(x => {
				var column = columns.First(y => y.FieldName == x.Name);
				var caption = column.ColumnCaption ?? column.FieldName;
				return CreateFilterFieldInfo(caption.ToString(), column.EditSettings, column.FieldName);
			});
			properties = properties.Concat(additionalProperties
				.Select(
				x => {
					var editSettings = x.Type != null ? FilteredComponentHelper.GetColumnsByType(owner, x.Type, false).First().EditSettings
					: new TextEditSettings();
					return CreateFilterFieldInfo(x.Caption ?? x.Name, editSettings, x.Name);
				}));
			return properties.OrderBy(x => x.FieldName);
		}
		static bool IsBrowsableProperty(PropertyDescriptor property) {
			foreach(var item in property.Attributes) {
				var attribute = item as EditorBrowsableAttribute;
				if(attribute != null && attribute.State == EditorBrowsableState.Never)
					return false;
			}
			return true;
		}
		static FilterFieldInfo CreateFilterFieldInfo(string caption, BaseEditSettings editSettings, string fieldName) {
			return new FilterFieldInfo(SplitStringHelper.SplitPascalCaseString(caption), editSettings, fieldName);
		}
	}
}
