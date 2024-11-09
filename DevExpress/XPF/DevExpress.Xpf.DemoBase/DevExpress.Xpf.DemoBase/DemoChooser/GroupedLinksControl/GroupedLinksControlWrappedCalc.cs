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
using System.Linq;
using System.Windows;
namespace DevExpress.Xpf.DemoChooser {
	class GroupedLinksControlWrappedCalc : GroupedLinksControlCalc {
		int maxGroupsPerColumn;
		internal const double vColumnSpacing = 35d;
		public GroupedLinksControlWrappedCalc(int maxItemsPerColumn, double hColumnSpacing, VerticalAlignment verticalAlignment, Action<ReadOnlyCollection<double>> setColumns, Action<int> setVisibleGroupsCount, IList<FilteredUIGroup> filteredUIGroups, IList<UIGroup> uiGroups)
			: base(hColumnSpacing, verticalAlignment, setColumns, setVisibleGroupsCount, filteredUIGroups, uiGroups) {
			this.maxGroupsPerColumn = maxItemsPerColumn;
		}
		ReadOnlyCollection<double> PutGroups(Size constraint, Action<UIElement, double, double, double, double> put, double hColumnSpacing, IEnumerable<FilteredUIGroup> filteredUIGroups) {
			double x = 0, y = 0;
			var asDesired = AsDesired(put, () => x, () => y);
			double columnWidth = 0;
			int groupCount = 0;
			var columns = new List<double>();
			foreach (var group in filteredUIGroups) {
				bool notEnoughHeight = y + group.Header.DesiredSize.Height + group.Links.First().Panel.DesiredSize.Height * 3 > constraint.Height;
				if (groupCount > 0 && (notEnoughHeight || (groupCount >= maxGroupsPerColumn && maxGroupsPerColumn > 0))) {
					groupCount = 0;
					columns.Add(columnWidth);
					y = 0d;
					x += columnWidth + hColumnSpacing;
					columnWidth = 0;
				}
				columnWidth = Math.Max(columnWidth, group.Header.DesiredSize.Width);
				asDesired(group.Header);
				y += group.Header.DesiredSize.Height;
				int linkIndex = 0;
				foreach (var link in group.Links) {
					if (group.Links.Count - linkIndex > 1 && y + link.Panel.DesiredSize.Height * 2 > constraint.Height) {
						columns.Add(columnWidth);
						y = 0d;
						x += columnWidth + hColumnSpacing;
						columnWidth = 0;
					}
					asDesired(link.Panel);
					columnWidth = Math.Max(columnWidth, link.Panel.DesiredSize.Width);
					y += link.Panel.DesiredSize.Height;
					linkIndex++;
				}
				if (y > 0.01d) { 
					y += vColumnSpacing;
				}
				groupCount++;
			}
			if (columnWidth > 0)
				columns.Add(columnWidth);
			return columns.AsReadOnly();
		}
		protected override ReadOnlyCollection<double> PutGroups(Size constraint, Action<UIElement, double, double, double, double> put, IList<FilteredUIGroup> filteredUIGroups, IList<UIGroup> uiGroups) {
			var columns = PutGroups(constraint, (e, a, b, c, d) => { }, 0, filteredUIGroups);
			double hSpacing = Math.Max(10d, Math.Min(50d, (constraint.Width - columns.Sum()) / columns.Count));
			return PutGroups(constraint, put, hSpacing, filteredUIGroups);
		}
	}
}
