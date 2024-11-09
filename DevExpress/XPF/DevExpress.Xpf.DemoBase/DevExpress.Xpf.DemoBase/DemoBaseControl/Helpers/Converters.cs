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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public partial class ObjectToItemsSourceConverter : MarkupExtension, IValueConverter, IMultiValueConverter {
		public int ItemsCount { get; set; }
		public override object ProvideValue(IServiceProvider serviceProvider) { return this; }
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return new ObservableCollection<object>(Enumerable.Range(0, ItemsCount).Select(x => new object[] { x, value }));
		}
		object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
			return new ObservableCollection<object>(Enumerable.Range(0, ItemsCount).Select(x => new object[] { x, values }));
		}
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
		object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}
	public class RoundCornersToMarginConverter : MarkupExtension, IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return (bool)value ? new Thickness(5, 0, 0, 0) : new Thickness();
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
		public override object ProvideValue(IServiceProvider serviceProvider) {
			return this;
		}
	}
	public class ThemeDependencyConverter : IValueConverter {
		ThemeDependencyConverter(){}
		public static ThemeDependencyConverter Instance { get; } = new ThemeDependencyConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
#pragma warning disable CS0612
			var themeName = (value as ThemeTreeWalker)?.ThemeName ?? CommonThemeHelper.ActualApplicationThemeName;
			var isWin11 = themeName?.StartsWith("Win11") ?? false;
			switch(parameter?.ToString()) {
				case "DescriptionBorderThickness": return isWin11 ? new Thickness() : new Thickness(0, 1, 0, 0);
				case "AccordionBorderThickness": return isWin11 ? new Thickness() : new Thickness(0, 0, 1, 0);
				case "ModuleBorderThickness": return isWin11 ? new Thickness(1) : new Thickness();
				case "ModuleCornerRadius": return isWin11 ? new CornerRadius(7) : new CornerRadius();
				case "ModuleMargin": return isWin11 ? new Thickness(6,0,6,0) : new Thickness();
				case "OptionsBorderThickness": return isWin11 ? new Thickness() : new Thickness(1,1,0,0);
				case "OptionsInnerBorderThickness": return isWin11 ? new Thickness(1,0,0,0) : new Thickness();
				case "OptionsMargin":
					if(isWin11)
						return new Thickness(0);
					return themeName == "DeepBlue" ? new Thickness(0, -2, 0, 0) : new Thickness(0, -1, 0, 0);
			}
			return DependencyProperty.UnsetValue;
#pragma warning restore CS0612
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
	}
	public class ClipGeometryConverter : IMultiValueConverter {
		ClipGeometryConverter(){}
		public static ClipGeometryConverter Instance { get; } = new ClipGeometryConverter();
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
#pragma warning disable CS0612
			if(values.Length == 4) {
				var width = ((double)values[0]);
				var height = ((double)values[1]);
				var radius = ((CornerRadius)values[2]).TopLeft;
				var themeName = (values[3] as ThemeTreeWalker)?.ThemeName ?? CommonThemeHelper.ActualApplicationThemeName;
				if(themeName?.StartsWith("Win11") ?? false) {
					var geometry = new RectangleGeometry(new Rect(0, 0, width, height), radius, radius);
					geometry.Freeze();
					return geometry;
				}
			}
			return DependencyProperty.UnsetValue;
#pragma warning restore CS0612
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
