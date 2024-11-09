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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Utils;
namespace DevExpress.Xpf.DemoCenterBase.Helpers {
	public class DemoCarouselLink {
		public string Title { get; set; }
		public ICommand SelectedCommand { get; set; }
	}
	public sealed class DemoCarouselItem : ImmutableObject {
		public DemoCarouselItem(string name, string title, string platformLabel, Uri preview, bool isAvailable, bool isUniversal, ICommand onRunCommand, IEnumerable<DemoCarouselLink> links) {
			this.Name = name;
			this.Title = title;
			this.PlatformLabel = platformLabel;
			this.Preview = preview;
			this.IsAvailable = isAvailable;
			this.IsUniversal = isUniversal;
			this.OnRunCommand = onRunCommand;
			this.Links = links;
		}
		public string Name { get; }
		public string Title { get; }
		public string PlatformLabel { get; }
		public Uri Preview { get; }
		public bool IsAvailable { get; }
		public bool IsUniversal { get; }
		public ICommand OnRunCommand { get; }
		public IEnumerable<DemoCarouselLink> Links { get; }
	}
	public class DemoCarouselControl : ItemsControl, IColumnControl {
		double itemWidth = 290;
		public int ItemsCount {
			get { return (int)GetValue(ItemsCountProperty); }
			set { SetValue(ItemsCountProperty, value); }
		}
		public static readonly DependencyProperty ItemsCountProperty =
			DependencyProperty.Register("ItemsCount", typeof(int), typeof(DemoCarouselControl), new PropertyMetadata(0));
		public Visibility FlyoutVisibility {
			get { return (Visibility)GetValue(FlyoutVisibilityProperty); }
			set { SetValue(FlyoutVisibilityProperty, value); }
		}
		public static readonly DependencyProperty FlyoutVisibilityProperty =
			DependencyProperty.Register("FlyoutVisibility", typeof(Visibility), typeof(DemoCarouselControl), new PropertyMetadata(Visibility.Collapsed));
		protected override Size MeasureOverride(Size constraint) {
			if(ItemsSource != null) {
				Columns = Enumerable.Range(0, ItemsSource.Cast<object>().Count()).Select(_ => itemWidth).ToList();
			} else {
				Columns = EmptyArray<double>.Instance;
			}
			return base.MeasureOverride(constraint);
		}
		static DemoCarouselControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(DemoCarouselControl), new FrameworkPropertyMetadata(typeof(DemoCarouselControl)));
		}
		public IEnumerable<double> Columns { get; private set; }
		public Rect VisibleArea { get; set; }
		public void OnAnimationStarted() { }
		public event Action ColumnsChanged { add { } remove { } }
	}
}
