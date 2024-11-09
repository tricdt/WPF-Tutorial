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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
namespace DevExpress.Xpf.Grid {
	public class CountToVisibilityConverter : IValueConverter {
		public bool Invert { get; set; }
		#region IValueConverter Members
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			return (int)value == 1 ^ Invert ? Visibility.Collapsed : Visibility.Visible;
		}
		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}
		#endregion
	}
	public class DragDropTextConverter : IMultiValueConverter {
		#region IMultiValueConverter Members
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
			if(values == null || values.Length != 2)
				return null;
			if(!(values[0] is DropTargetType))
				return null;
			DropTargetType? type = (DropTargetType)values[0];
			DataViewBase view = values[1] as DataViewBase;
			GridControlStringId id;
			switch(type.Value) {
				case DropTargetType.None:
					id = GridControlStringId.DDExtensionsCannotDropHere;
					break;
				case DropTargetType.InsertRowsAfter:
					id = GridControlStringId.DDExtensionsInsertAfter;
					break;
				case DropTargetType.InsertRowsBefore:
					id = GridControlStringId.DDExtensionsInsertBefore;
					break;
				case DropTargetType.InsertRowsIntoGroup:
					id = GridControlStringId.DDExtensionsMoveToGroup;
					break;
				case DropTargetType.InsertRowsIntoNode:
					id = GridControlStringId.DDExtensionsMoveToChildrenCollection;
					break;
				case DropTargetType.DataArea:
					id = GridControlStringId.DDExtensionsAddRows;
					break;
				default:
					return null;
			}
			if(view != null) {
				string stringId = Enum.GetName(typeof(GridControlStringId), id);
				return view.LocalizationDescriptor.GetValue(stringId);
			} else
				return GridControlLocalizer.GetString(id);
		}
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
			throw new NotImplementedException();
		}
		#endregion
	}
}
