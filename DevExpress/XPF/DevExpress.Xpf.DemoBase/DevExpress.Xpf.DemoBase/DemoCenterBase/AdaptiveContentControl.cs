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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevExpress.Mvvm.Native;
namespace DevExpress.Xpf.DemoCenterBase.Helpers {
	public class AdaptiveContentControl : Panel {
		public static int GetPriority(UIElement obj) {
			return (int)obj.GetValue(PriorityProperty);
		}
		public static void SetPriority(UIElement obj, int value) {
			obj.SetValue(PriorityProperty, value);
		}
		public static readonly DependencyProperty PriorityProperty =
			DependencyProperty.RegisterAttached("Priority", typeof(int), typeof(AdaptiveContentControl), new PropertyMetadata(-1));
		static int GetActualPriority(UIElement element) {
			var presenter = element as ContentPresenter;
			if(presenter != null)
				return VisualTreeHelper.GetChildrenCount(presenter) == 0 ? -1 : GetPriority((UIElement)VisualTreeHelper.GetChild(presenter, 0));
			return GetPriority(element);
		}
		protected override Size MeasureOverride(Size availableSize) {
			var children = Children.Cast<UIElement>().OrderByDescending(GetActualPriority).Where(x => GetActualPriority(x) >= -1).ToList();
			foreach(var child in children) {
				child.Opacity = 1d;
				(child as DemoScreenFooterControl).Do(x => x.IsCompactMode = false);
				child.Measure(availableSize);
			}
			var availableHeight = availableSize.Height;
			while(children.Count != 0) {
				if(children.Sum(x => x.DesiredSize.Height) <= availableHeight)
					break;
				var candidate = children[0];
				if(GetPriority(candidate) < 0)
					break;
				var footer = candidate as DemoScreenFooterControl;
				if(footer != null) {
					if(!footer.IsCompactMode) {
						footer.IsCompactMode = true;
						footer.InvalidateMeasure();
						footer.Measure(availableSize);
					}
					availableHeight -= footer.DesiredSize.Height;
				} else {
					candidate.Opacity = 0d;
				}
				children.Remove(candidate);
			}
			var width = 0d;
			var height = 0d;
			availableHeight = availableSize.Height;
			foreach(var child in Children.Cast<UIElement>().Where(x => x.Opacity > 0d).OrderByDescending(GetActualPriority)) {
				child.Measure(new Size(availableSize.Width, availableHeight));
				availableHeight = Math.Max(0d, availableHeight - child.DesiredSize.Height);
				height += child.DesiredSize.Height;
				width = Math.Max(width, child.DesiredSize.Width);
			}
			return new Size(width, height);
		}
		protected override Size ArrangeOverride(Size finalSize) {
			double y = 0;
			var children = Children.Cast<UIElement>().Where(x => x.Opacity > 0d).ToList();
			foreach(var child in children) {
				if(child == children.Last()) {
					child.Arrange(new Rect(0, finalSize.Height - child.DesiredSize.Height, finalSize.Width, child.DesiredSize.Height));
				} else {
					child.Arrange(new Rect(0, y, finalSize.Width, child.DesiredSize.Height));
					y += child.DesiredSize.Height;
				}
			}
			return finalSize;
		}
	}
}
