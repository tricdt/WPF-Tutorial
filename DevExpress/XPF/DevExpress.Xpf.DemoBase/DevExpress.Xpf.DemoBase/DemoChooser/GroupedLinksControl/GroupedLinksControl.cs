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
using System.Linq;
using System.Windows.Controls;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoCenterBase.Helpers;
namespace DevExpress.Xpf.DemoChooser {
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Input;
	using System.Windows.Media;
	using DevExpress.Utils;
	using DevExpress.Xpf.DemoBase.Helpers;
	public partial class GroupedLinksControl : ItemsControl, IColumnControl {
		static GroupedLinksControl() {
			DependencyPropertyRegistrator<GroupedLinksControl>.New()
				.Register(nameof(GroupHeaderTemplate), out GroupHeaderTemplateProperty, default(DataTemplate))
				.Register(nameof(ShowAllTemplate), out ShowAllTemplateProperty, default(DataTemplate))
				.Register(nameof(LayoutType), out LayoutTypeProperty, GroupedLinksControlLayoutType.OneGroupPerColumn, d => d.UpdatePanelCalc())
				.Register(nameof(LinkTemplate), out LinkTemplateProperty, default(DataTemplate))
				.Register(nameof(LinkLabelTemplate), out LinkLabelTemplateProperty, default(DataTemplate))
				.Register(nameof(FilterString), out FilterStringProperty, string.Empty, d => d.OnFilterStringChanged())
				.Register(nameof(VisibleGroupsCount), out VisibleGroupsCountProperty, 1)
				.Register(nameof(MaxGroupsPerColumn), out MaxGroupsPerColumnProperty, 1, d => d.UpdatePanelCalc(), (d, v) => Math.Max(1, v))
				.Register(nameof(HColumnSpacing), out HColumnSpacingProperty, 10d, d => d.UpdatePanelCalc())
				.OverrideMetadata(VerticalAlignmentProperty, d => d.UpdatePanelCalc())
				.OverrideMetadata(FocusableProperty, false)
			;
			DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupedLinksControl), new FrameworkPropertyMetadata(typeof(GroupedLinksControl)));
		}
		readonly List<UIGroup> uiGroups = new List<UIGroup>();
		FrameworkElement flyout;
		bool update = false;
		GroupedLinksControlPanel panel;
		void UpdatePanelCalc() {
			if(panel == null) return;
			var filterString = FilterString;
			var filteredUIGroups = uiGroups.Select(g => new FilteredUIGroup(
				TagsHelper.Contains(g.Tag, filterString) ? g.Links : g.Links.Where(l => TagsHelper.Contains(l.Tags, filterString)).ToReadOnlyCollection(),
				g.Header,
				g.ShowAllDemos
			)).Where(g => g.Links.Any()).ToReadOnlyCollection();
			GroupedLinksControlCalc calc;
			switch(LayoutType) {
			case GroupedLinksControlLayoutType.OneGroupPerColumn:
				calc = new GroupedLinksControlOneGroupPerColumnCalc(HColumnSpacing, VerticalAlignment, x => Columns = x, x => VisibleGroupsCount = x, filteredUIGroups, uiGroups.AsReadOnly());
				break;
			case GroupedLinksControlLayoutType.AlignedColumns:
				calc = new GroupedLinksControlAlignedGroupsCalc(MaxGroupsPerColumn, HColumnSpacing, VerticalAlignment, x => Columns = x, x => VisibleGroupsCount = x, filteredUIGroups, uiGroups.AsReadOnly());
				break;
			case GroupedLinksControlLayoutType.Wrapped:
				calc = new GroupedLinksControlWrappedCalc(MaxGroupsPerColumn, HColumnSpacing, VerticalAlignment, x => Columns = x, x => VisibleGroupsCount = x, filteredUIGroups, uiGroups.AsReadOnly());
				break;
			default: throw new InvalidOperationException();
			}
			panel.UpdateCalc(calc);
		}
		IEnumerable<GroupedLinks> Groups { get { return (IEnumerable<GroupedLinks>)ItemsSource; } }
		public Visibility LeftFlyoutIndicatorVisibility {
			get { return (Visibility)GetValue(LeftFlyoutIndicatorVisibilityProperty); }
			set { SetValue(LeftFlyoutIndicatorVisibilityProperty, value); }
		}
		public static readonly DependencyProperty LeftFlyoutIndicatorVisibilityProperty =
			DependencyProperty.Register("LeftFlyoutIndicatorVisibility", typeof(Visibility), typeof(GroupedLinksControl), new PropertyMetadata(Visibility.Collapsed));
		public Visibility RightFlyoutIndicatorVisibility {
			get { return (Visibility)GetValue(RightFlyoutIndicatorVisibilityProperty); }
			set { SetValue(RightFlyoutIndicatorVisibilityProperty, value); }
		}
		public static readonly DependencyProperty RightFlyoutIndicatorVisibilityProperty =
			DependencyProperty.Register("RightFlyoutIndicatorVisibility", typeof(Visibility), typeof(GroupedLinksControl), new PropertyMetadata(Visibility.Collapsed));
		public event EventHandler FilterStringChanged;
		void OnFilterStringChanged() {
			if(FilterStringChanged != null)
				FilterStringChanged(this, EventArgs.Empty);
			Logger.LogSearch("Platform", FilterString);
			UpdatePanelCalc();
		}
		IEnumerable<double> columns;
		public IEnumerable<double> Columns {
			get { return columns; }
			set {
				columns = value;
				if(ColumnsChanged != null)
					ColumnsChanged();
			}
		}
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue) {
			update = true;
			CreateControls();
			base.OnItemsSourceChanged(oldValue, newValue);
		}
		public static void Animate(FrameworkElement child, Point currentPosition, Point newPosition) {
			Point delta = new Point(newPosition.X - currentPosition.X, newPosition.Y - currentPosition.Y);
			var transform = new TranslateTransform();
			child.RenderTransform = transform;
			transform.X = newPosition.X;
			transform.Y = newPosition.Y;
		}
		void CreateControls() {
			if(panel == null) return;
			if(update) {
				update = false;
				uiGroups.Clear();
				panel.Children.Clear();
				foreach(var group in Groups ?? EmptyArray<GroupedLinks>.Instance) {
					var uiLinks = new List<UILink>();
					foreach(var link in group.Links) {
						var linkPanel = new DockPanel();
						var linkControl = new ContentControl { Content = link.Content, ContentTemplate = LinkTemplate, Focusable = false };
						var linkLabelControl = new ContentControl { Content = link.Content, ContentTemplate = LinkLabelTemplate, Focusable = false };
						DockPanel.SetDock(linkLabelControl, Dock.Right);
						linkPanel.Children.Add(linkControl);
						linkPanel.Children.Add(linkLabelControl);
						var uiLink = new UILink(control: linkControl, title: link.Title, tags: link.Tags, description: link.Description, panel: linkPanel);
						MouseTouchAdapter.SubscribeToPointerUp(linkControl, this, _ => link.Execute?.Invoke());
						uiLinks.Add(uiLink);
						panel.Children.Add(linkPanel);
					}
					var uiGroup = new UIGroup(
						uiLinks.AsReadOnly(),
						group.Header,
						new ContentControl { Content = group, ContentTemplate = GroupHeaderTemplate, Focusable = false },
						new ContentControl { Content = group, ContentTemplate = ShowAllTemplate, Focusable = false }
					);
					var groupLocal = group;
					MouseTouchAdapter.SubscribeToPointerUp(uiGroup.Header, this, p => groupLocal.ShowAllCommand.Do(c => c.Execute(null)));
					MouseTouchAdapter.SubscribeToPointerUp(uiGroup.ShowAllDemos, this, p => groupLocal.ShowAllCommand.Do(c => c.Execute(null)));
					panel.Children.Add(uiGroup.Header);
					panel.Children.Add(uiGroup.ShowAllDemos);
					uiGroups.Add(uiGroup);
				}
			}
			UpdatePanelCalc();
		}
		Point GetPosition(Visual visual) {
			return visual.TransformToAncestor(this).Transform(new Point());
		}
		FrameworkElement panelContainer;
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			panelContainer = (FrameworkElement)GetTemplateChild("PART_PanelContainer");
			flyout = (FrameworkElement)GetTemplateChild("PART_Flyout");
			panel?.UpdateCalc(null);
			panel = (GroupedLinksControlPanel)GetTemplateChild("PART_Panel");
			CreateControls();
		}
		Point UpdateFlyoutPosition(UIElement anchor) {
			var anchorPos = GetPosition(anchor);
			var flyoutSize = flyout.DesiredSize;
			double visibleAreaRight = VisibleArea.IsEmpty ? ActualWidth : VisibleArea.Right;
			bool onRight = flyoutSize.Width + anchor.DesiredSize.Width + anchorPos.X < visibleAreaRight;
			LeftFlyoutIndicatorVisibility = onRight ? Visibility.Visible : Visibility.Hidden;
			RightFlyoutIndicatorVisibility = onRight ? Visibility.Hidden : Visibility.Visible;
			double left = Math.Round(anchorPos.X + (onRight ? anchor.DesiredSize.Width : -flyoutSize.Width));
			double top = Math.Round(anchorPos.Y - flyoutSize.Height / 2 + anchor.DesiredSize.Height / 2 + 3);
			if(top < 0) {
				LeftFlyoutIndicatorVisibility = Visibility.Hidden;
				RightFlyoutIndicatorVisibility = Visibility.Hidden;
				top = 0;
			} else if(top + flyoutSize.Height > ActualHeight) {
				LeftFlyoutIndicatorVisibility = Visibility.Hidden;
				RightFlyoutIndicatorVisibility = Visibility.Hidden;
				top = ActualHeight - flyout.DesiredSize.Height;
			}
			return new Point(left, top);
		}
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e) {
			HideFlyout();
			base.OnPreviewMouseDown(e);
		}
		protected override void OnPreviewMouseMove(MouseEventArgs e) {
			var pos = e.GetPosition(this);
			var hitTestResult = VisualTreeHelper.HitTest(panelContainer, pos);
			if(hitTestResult == null)
				return;
			var visualHit = hitTestResult.VisualHit;
			if(uiGroups == null)
				return;
			var uiLink = uiGroups.SelectMany(g => g.Links).FirstOrDefault(l => l.Control.VisualChildren().Any(c => c == visualHit));
			if(uiLink != null && uiLink.Description != null) {
				ShowFlyout(uiLink);
			} else {
				HideFlyout();
			}
			base.OnPreviewMouseMove(e);
		}
		internal void ShowFlyout(UILink uiLink) {
			ShowFlyout();
			flyout.DataContext = new FlyoutViewModel(uiLink.Title, uiLink.Description);
			flyout.UpdateLayout();
			Animate(flyout, new Point(), UpdateFlyoutPosition(uiLink.Control));
		}
		public GroupedLinksControl() {
			VisibleArea = Rect.Empty;
			Columns = EmptyArray<double>.Instance;
			panel = new GroupedLinksControlPanel();
			UpdatePanelCalc();
		}
		public Rect VisibleArea { get; set; }
		public void OnAnimationStarted() {
			HideFlyout();
		}
		void ShowFlyout() {
			flyout.Opacity = 1;
			flyout.Visibility = Visibility.Visible;
		}
		void HideFlyout() {
			flyout.Opacity = 0;
			flyout.Visibility = Visibility.Collapsed;
		}
		public event Action ColumnsChanged;
	}
	public sealed class GroupedLink {
		public GroupedLink(object content, string title, string[] tags, string description, Action execute) {
			Content = content;
			Title = title;
			Tags = tags;
			Description = description;
			Execute = execute;
		}
		public readonly object Content;
		public readonly string Title;
		public readonly string[] Tags;
		public readonly string Description;
		public readonly Action Execute;
	}
	public class GroupedLinkContextMenuItem {
		public ICommand SelectedCommand { get; set; }
		public string Title { get; set; }
	}
	public sealed class GroupedLinks : ImmutableObject {
		public GroupedLinks(string header, ReadOnlyCollection<GroupedLink> links, ReadOnlyObservableCollection<GroupedLinkContextMenuItem> contextMenu, ICommand showAllCommand) {
			Header = header;
			Links = links;
			ContextMenu = contextMenu;
			ShowAllCommand = showAllCommand;
		}
		public string Header { get; }
		public readonly ReadOnlyCollection<GroupedLink> Links;
		public ReadOnlyObservableCollection<GroupedLinkContextMenuItem> ContextMenu { get; }
		public readonly ICommand ShowAllCommand;
	}
	public enum GroupedLinksControlLayoutType {
		OneGroupPerColumn, Wrapped, AlignedColumns
	}
	public sealed class UILink {
		public UILink(string title, string[] tags, string description, ContentControl control, DockPanel panel) {
			Title = title;
			Tags = tags;
			Description = description;
			Control = control;
			Panel = panel;
		}
		public readonly string Title;
		public readonly string[] Tags;
		public readonly string Description;
		public readonly ContentControl Control;
		public readonly DockPanel Panel;
	}
	public sealed class FlyoutViewModel : ImmutableObject {
		public FlyoutViewModel(string title, string description) {
			Title = title;
			Description = description;
		}
		public string Title { get; }
		public string Description { get; }
	}
	public sealed class UIGroup {
		public UIGroup(IList<UILink> links, string tag, ContentControl header, ContentControl showAllDemos) {
			Links = links;
			Tag = tag;
			Header = header;
			ShowAllDemos = showAllDemos;
		}
		public readonly IList<UILink> Links;
		public readonly string Tag;
		public readonly ContentControl Header;
		public readonly ContentControl ShowAllDemos;
	}
	public sealed class FilteredUIGroup {
		public FilteredUIGroup(IList<UILink> links, ContentControl header, ContentControl showAllDemos) {
			Links = links;
			Header = header;
			ShowAllDemos = showAllDemos;
		}
		public readonly IList<UILink> Links;
		public readonly ContentControl Header;
		public readonly ContentControl ShowAllDemos;
	}
	static class UIGroupExtensions {
		public static IEnumerable<UIElement> GetAllControls(this UIGroup g) => new UIElement[] { g.Header, g.ShowAllDemos }.Concat(g.Links.Select(l => l.Panel));
		public static IEnumerable<UIElement> GetAllControls(this FilteredUIGroup g) => new UIElement[] { g.Header, g.ShowAllDemos }.Concat(g.Links.Select(l => l.Panel));
	}
}
