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
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Utils;
using DevExpress.Xpf.DemoBase.ThemeLayoutHelpers;
using DevExpress.Xpf.DemoCenterBase.Helpers;
using static System.Math;
namespace DevExpress.Xpf.DemoChooser.Helpers {
	using System.Windows;
	using DevExpress.DemoData.Model;
	using DevExpress.Xpf.DemoBase.Helpers;
	using DevExpress.Xpf.DemoCenter;
	using DevExpress.Xpf.DemoCenter.Internal;
	using DevExpress.Xpf.DemoChooser.Internal;
	public partial class CrossPlatformDemoCarouselControl : ItemsControl, IColumnControl {
		const Visibility Visibility_Collapsed = Visibility.Collapsed;
		static CrossPlatformDemoCarouselControl() {
			DependencyPropertyRegistrator<CrossPlatformDemoCarouselControl>.New()
				.Register(nameof(FlyoutVisibility), out FlyoutVisibilityProperty, Visibility_Collapsed)
				.RegisterReadOnly(nameof(Panel), out PanelPropertyKey, out PanelProperty, default(CrossPlatformDemoCarouselPanel))
			;
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CrossPlatformDemoCarouselControl), new FrameworkPropertyMetadata(typeof(CrossPlatformDemoCarouselControl)));
		}
		public CrossPlatformDemoCarouselControl() {
			Loaded += OnLoaded;
		}
		void OnLoaded(object sender, RoutedEventArgs e) {
			Panel = this.ByType<CrossPlatformDemoCarouselPanel>();
			UpdateColumns();
		}
		void UpdateColumns() {
			if(ItemsSource == null || Panel == null || Panel.Children.Count == 0) {
				Columns = EmptyArray<double>.Instance;
				return;
			}
			var values = new List<double>();
			double width = 0d;
			double lastWidth = 0d;
			foreach(var item in Panel.Children.Cast<UIElement>()) {
				if(item == null)
					continue;
				var point = item.TranslatePoint(new Point(0, 0), this);
				if(point.X != width) {
					values.Add(point.X - width);
				}
				lastWidth = item.RenderSize.Width;
				width = point.X;
			}
			if(lastWidth != 0)
				values.Add(lastWidth);
			Columns = values;
		}
		protected override Size MeasureOverride(Size constraint) {
			var measureSize = base.MeasureOverride(constraint);
			UpdateColumns();
			return measureSize;
		}
		protected override Size ArrangeOverride(Size arrangeBounds) {
			var arrangeSize = base.ArrangeOverride(arrangeBounds);
			UpdateColumns();
			return arrangeSize;
		}
		IEnumerable<double> columns;
		public IEnumerable<double> Columns {
			get { return columns; }
			set {
				columns = value;
				ColumnsChanged?.Invoke();
			}
		}
		public Rect VisibleArea { get; set; }
		public void OnAnimationStarted() { }
		public event Action ColumnsChanged;
	}
	public partial class CrossPlatformDemoCarouselPanel : Panel {
		static CrossPlatformDemoCarouselPanel() {
			DependencyPropertyRegistrator<CrossPlatformDemoCarouselPanel>.New()
				.Register(nameof(ViewPort), out ViewPortProperty, 0d, FrameworkPropertyMetadataOptions.AffectsMeasure)
				.Register(nameof(MaxItemWidth), out MaxItemWidthProperty, 385d, FrameworkPropertyMetadataOptions.AffectsMeasure)
				.RegisterReadOnly(nameof(ActualViewPort), out ActualViewPortPropertyKey, out ActualViewPortProperty, 0d)
			;
		}
		double minItemWidth = 300;
		protected override Size MeasureOverride(Size availableSize) {
			var children = InternalChildren;
			if(children.Count == 0)
				return new Size();
			var availableWidth = ViewPort;
			var resultWidth = 0d;
			var maxItems = Min((int)Floor(availableWidth / minItemWidth), children.Count);
			var itemWidth = Min(availableWidth / maxItems, MaxItemWidth);
			var lastItemWidth = availableWidth - ((maxItems - 1) * itemWidth);
			var childSize = new Size(itemWidth, Max(0d, availableSize.Height));
			var childrenHeight = 0d;
			for(int i = 0; i < children.Count; ++i) {
				var item = children[i];
				item.Measure(childSize);
				resultWidth += (i + 1) % maxItems == 0 ? lastItemWidth : item.DesiredSize.Width;
				childrenHeight = Max(childrenHeight, item.DesiredSize.Height);
			}
			return new Size(resultWidth, childrenHeight);
		}
		protected override Size ArrangeOverride(Size finalSize) {
			var children = InternalChildren;
			var availableWidth = ViewPort;
			var maxItems = Min((int)Floor(availableWidth / minItemWidth), children.Count);
			var itemWidth = Min(availableWidth / maxItems, MaxItemWidth);
			var lastItemWidth = availableWidth - ((maxItems - 1) * itemWidth);
			ActualViewPort = Min(availableWidth, itemWidth * Min(maxItems, children.Count));
			if(children.Count == 0)
				return finalSize;
			var itemSize = new Size(itemWidth, Max(0d, finalSize.Height));
			var point = new Point();
			for(int i = 0; i < children.Count; ++i) {
				var item = children[i];
				item.Arrange(new Rect(point, itemSize));
				point.X += (i + 1) % maxItems == 0 ? lastItemWidth + 1 : itemWidth;
			}
			return finalSize;
		}
	}
	public class CrossPlatformDemoCarouselItem : ImmutableObject {
		public CrossPlatformDemoCarouselItem(
			string name, string title, 
			string platformLabel, bool isAvailable, bool isUniversal, 
			string description, ICommand onRunCommand, Uri preview,
			IEnumerable<DemoCarouselLink> links, IEnumerable<DemoCarouselLink> demoLinks,
			IEnumerable<DemoCarouselLink> staticLinks, string staticLinksTitle,
			IEnumerable<DemoCarouselLink> tutorialLinks = null) {
			Name = name;
			Title = title;
			PlatformLabel = platformLabel;
			IsAvailable = isAvailable;
			IsUniversal = isUniversal;
			Description = description;
			OnRunCommand = onRunCommand;
			Preview = preview;
			Links = links;
			DemoLinks = demoLinks;
			StaticLinks = staticLinks;
			StaticLinksTitle = staticLinksTitle;
			TutorialLinks = tutorialLinks;
		}
		public CrossPlatformDemoCarouselItem(
			Product product, bool isUniversal,
			string description, ICommand onRunCommand,
			IEnumerable<DemoCarouselLink> links, IEnumerable<DemoCarouselLink> demoLinks,
			IEnumerable<DemoCarouselLink> staticLinks, string staticLinksTitle,
			IEnumerable<DemoCarouselLink> tutorialLinks = null)
			: this(product.Name, product.DisplayName,
				   PlatformTitles.GetTitle(PlatformViewModel.GetModel(product.Platform)),
				   product.IsInstalled, isUniversal, description, onRunCommand, product.Image.Uri,
				   links, demoLinks, staticLinks, staticLinksTitle, tutorialLinks) 
			{
			ProductLink = new ProductLink(product.Platform.Name, product, product.DisplayName, product.SupportsDirectX, null);
		}
		public string Name { get; }
		public string Title { get; }
		public string PlatformLabel { get; }
		public bool IsAvailable { get; }
		public bool IsUniversal { get; }
		public string Description { get; }
		public ICommand OnRunCommand { get; }
		public Uri Preview { get; }
		public IEnumerable<DemoCarouselLink> Links { get; }
		public IEnumerable<DemoCarouselLink> DemoLinks { get; }
		public IEnumerable<DemoCarouselLink> StaticLinks { get; }
		public string StaticLinksTitle { get; }
		public IEnumerable<DemoCarouselLink> TutorialLinks { get; }
		public ProductLink ProductLink { get; }
	}
}
