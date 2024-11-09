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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoChooser {
	abstract class GroupedLinksControlCalc {
		internal readonly double hColumnSpacing;
		readonly VerticalAlignment verticalAlignment;
		readonly Action<ReadOnlyCollection<double>> setColumns;
		readonly Action<int> setVisibleGroupsCount;
		readonly IList<UIGroup> uiGroups;
		readonly IList<FilteredUIGroup> filteredUIGroups;
		protected GroupedLinksControlCalc(double hColumnSpacing, VerticalAlignment verticalAlignment, Action<ReadOnlyCollection<double>> setColumns, Action<int> setVisibleGroupsCount, IList<FilteredUIGroup> filteredUIGroups, IList<UIGroup> uiGroups) {
			this.hColumnSpacing = hColumnSpacing;
			this.verticalAlignment = verticalAlignment;
			this.setColumns = setColumns;
			this.setVisibleGroupsCount = setVisibleGroupsCount;
			this.uiGroups = uiGroups;
			this.filteredUIGroups = filteredUIGroups;
		}
		protected abstract ReadOnlyCollection<double> PutGroups(Size constraint, Action<UIElement, double, double, double, double> put, IList<FilteredUIGroup> filteredUIGroups, IList<UIGroup> uiGroups);
		protected static Action<UIElement> AsDesired(Action<UIElement, double, double, double, double> put, Func<double> x, Func<double> y) {
			return e => put(e, x(), y(), e.DesiredSize.Width, e.DesiredSize.Height);
		}
		public Size Measure(Size constraint) {
			Debug.Assert(!double.IsNaN(constraint.Height));
			Debug.Assert(!double.IsNaN(constraint.Width));
			var inf = new Size(double.PositiveInfinity, double.PositiveInfinity);
			setVisibleGroupsCount(filteredUIGroups.Count);
			filteredUIGroups.SelectMany(x => x.GetAllControls()).ForEach(c => c.Measure(inf));
			var rects = new List<Rect>();
			Action<UIElement, double, double, double, double> put = (e, x, y, w, h) => rects.Add(new Rect(x, y, w, h));
			setColumns(PutGroups(constraint, put, filteredUIGroups, uiGroups));
			double width = 1 + (rects.Count > 0 ? rects.Max(r => r.X + r.Width) : 0);
			double height = 1 + (rects.Count > 0 ? rects.Max(r => r.Y + r.Height) : 0);
			width = Math.Max(width, 0d);
			return new Size(width, verticalAlignment == VerticalAlignment.Stretch ? Math.Min(constraint.Height, 5000) : height);
		}
		public Size Arrange(Size arrangeBounds) {
			var visible = new HashSet<UIElement>();
			Action<UIElement, double, double, double, double> put = (e, x, y, w, h) => {
				e.Arrange(new Rect(x, y, w, h));
				visible.Add(e);
			};
			PutGroups(arrangeBounds, put, filteredUIGroups, uiGroups);
			var hidden = new HashSet<UIElement>(uiGroups.SelectMany(x => x.GetAllControls()));
			hidden.ExceptWith(visible);
			foreach(var elem in hidden) {
				elem.Arrange(new Rect());
			}
			return arrangeBounds;
		}
	}
}
