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
using DevExpress.Mvvm.Native;
using static System.Math;
namespace DevExpress.Xpf.DemoChooser {
	class GroupedLinksControlAlignedGroupsCalc : GroupedLinksControlCalc {
		internal const double vColumnSpacing = 15d;
		int maxGroupsPerColumn;
		public GroupedLinksControlAlignedGroupsCalc(int maxItemsPerColumn, double hColumnSpacing, VerticalAlignment verticalAlignment, Action<ReadOnlyCollection<double>> setColumns, Action<int> setVisibleGroupsCount, IList<FilteredUIGroup> filteredUIGroups, IList<UIGroup> uiGroups)
			: base(hColumnSpacing, verticalAlignment, setColumns, setVisibleGroupsCount, filteredUIGroups, uiGroups) {
			this.maxGroupsPerColumn = maxItemsPerColumn;
		}
		protected override ReadOnlyCollection<double> PutGroups(Size constraint, Action<UIElement, double, double, double, double> put, IList<FilteredUIGroup> filteredUIGroups, IList<UIGroup> uiGroups) {
			double x = 0, y = 0;
			var asDesired = AsDesired(put, () => x, () => y);
			int columns = Max(1, (filteredUIGroups.Count + 1) / maxGroupsPerColumn);
			int rows = (filteredUIGroups.Count + columns - 1) / columns;
			for (int row = 0; row < rows; row++) {
				double rowHeight = 0;
				double rowY = y;
				for (int column = 0; column < columns; column++) {
					var index = row * columns + column;
					if (index >= filteredUIGroups.Count)
						break;
					var group = filteredUIGroups[index];
					asDesired(group.Header);
					y += group.Header.DesiredSize.Height;
					foreach (var link in group.Links) {
						asDesired(link.Panel);
						y += link.Panel.DesiredSize.Height;
					}
					rowHeight = Math.Max(rowHeight, y);
					y = rowY;
					x += hColumnSpacing;
				}
				y += rowHeight + 15;
				x = 0;
			}
			return Enumerable.Range(0, columns).Select(_ => 0.0).ToReadOnlyCollection();
		}
	}
}
