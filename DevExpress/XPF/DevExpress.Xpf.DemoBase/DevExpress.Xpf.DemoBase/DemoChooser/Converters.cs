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
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using DevExpress.Xpf.Core;
namespace DevExpress.Xpf.DemoChooser.Helpers {
	public class StringIsNullToVisibilityConverter : System.Windows.Markup.MarkupExtension, IValueConverter {
		public override object ProvideValue(IServiceProvider serviceProvider) {
			return this;
		}
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var stringVlaue = value as string;
			return string.IsNullOrEmpty(stringVlaue) ? Visibility.Collapsed : Visibility.Visible;
		}
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}
	public class ObjectToVisibilityConverter : MarkupExtension, IValueConverter {
		public override object ProvideValue(IServiceProvider serviceProvider) {
			return this;
		}
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			var boolValue = (bool)new ObjectToBooleanConverter().Convert(value, targetType, parameter, culture);
			return boolValue ? Visibility.Visible : Visibility.Collapsed;
		}
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}
	public class ListToVisibilityConverter : MarkupExtension, IValueConverter {
		public override object ProvideValue(IServiceProvider serviceProvider) {
			return this;
		}
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return (Visibility)new Core.ObjectToVisibilityConverter().Convert(value, targetType, parameter, culture);
		}
		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException();
		}
	}
}
