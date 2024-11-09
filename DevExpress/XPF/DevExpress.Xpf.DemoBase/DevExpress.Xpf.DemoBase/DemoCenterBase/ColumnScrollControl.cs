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
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core;
using static System.Math;
namespace DevExpress.Xpf.DemoCenterBase.Helpers {
	using System.Collections.ObjectModel;
	using System.Windows;
	using System.Windows.Controls;
	using DevExpress.Xpf.Core.Native;
	using DevExpress.Xpf.DemoBase.Helpers;
	interface IColumnControl {
		IEnumerable<double> Columns { get; }
		event Action ColumnsChanged;
		Rect VisibleArea { get; set; }
		void OnAnimationStarted();
	}
	public class ColumnScrollDecorator : Decorator {
		internal Action OnMeasure;
		protected override Size MeasureOverride(Size availableSize) {
			Child?.Measure(availableSize);
			OnMeasure?.Invoke();
			return Child == null ? new Size() : Child.DesiredSize;
		}
		protected override Size ArrangeOverride(Size finalSize) {
			Child?.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
			return finalSize;
		}
	}
	public partial class ColumnScrollControl : ContentControl {
		public sealed class PageItem : BindableBase {
			bool isChecked = false;
			ColumnScrollControl carousel;
			public void SilentlyCheck(bool value) {
				SetProperty(ref isChecked, value, () => IsChecked);
			}
			public bool IsChecked {
				get { return isChecked; }
				set {
					SetProperty(ref isChecked, value, () => IsChecked, () => {
						if(value) {
							carousel.NavigateToPage(carousel.PageButtons.IndexOf(this));
						}
					});
				}
			}
			public PageItem(ColumnScrollControl carousel) {
				this.carousel = carousel;
			}
		}
		ColumnScrollDecorator childContainer;
		double hardPadding = 10000d;
		IColumnControl TypedContent { get { return (IColumnControl)Content; } }
		FrameworkElement UIContent { get { return (FrameworkElement)Content; } }
		internal bool AllowDifferentColumnWidth { get; set; }
		static ColumnScrollControl() {
			DependencyPropertyRegistrator<ColumnScrollControl>.New()
				.Register(nameof(CrossPlatform), out CrossPlatformProperty, false)
				.Register(nameof(ShowButtons), out ShowButtonsProperty, true)
				.Register(nameof(AllowScrollWithMouseWheel), out AllowScrollWithMouseWheelProperty, true)
				.RegisterReadOnly(nameof(ItemsCount), out ItemsCountPropertyKey, out ItemsCountProperty, 0)
				.RegisterReadOnly(nameof(ActualCurrentPage), out ActualCurrentPagePropertyKey, out ActualCurrentPageProperty, 0)
				.Register(nameof(CurrentPage), out CurrentPageProperty, 0, (d, _, v) => d.currentPageLocker.DoIfNotLocked(() => d.NavigateToPage(v)))
				.RegisterReadOnly(nameof(PagesCount), out PagesCountPropertyKey, out PagesCountProperty, 0)
				.RegisterReadOnly(nameof(PageButtons), out PageButtonsPropertyKey, out PageButtonsProperty, default(ReadOnlyObservableCollection<PageItem>))
				.RegisterReadOnly(nameof(IsFirstPageSelected), out IsFirstPageSelectedPropertyKey, out IsFirstPageSelectedProperty, true)
				.RegisterReadOnly(nameof(IsLastPageSelected), out IsLastPageSelectedPropertyKey, out IsLastPageSelectedProperty, false)
				.RegisterReadOnly(nameof(IsOpacityMaskVisible), out IsOpacityMaskVisiblePropertyKey, out IsOpacityMaskVisibleProperty, false)
				.RegisterReadOnly(nameof(IsLeftOpacityMaskVisible), out IsLeftOpacityMaskVisiblePropertyKey, out IsLeftOpacityMaskVisibleProperty, false)
				.RegisterReadOnly(nameof(ExtraSpace), out ExtraSpacePropertyKey, out ExtraSpaceProperty, 0d)
				.RegisterReadOnly(nameof(ScrollViewer), out ScrollViewerPropertyKey, out ScrollViewerProperty, default(ScrollViewer))
			;
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnScrollControl), new FrameworkPropertyMetadata(typeof(ColumnScrollControl)));
		}
		protected override void OnContentChanged(object oldContent, object newContent) {
			base.OnContentChanged(oldContent, newContent);
			if(oldContent != null) {
				((FrameworkElement)oldContent).SizeChanged -= ColumnScrollPanel_SizeChanged;
				((IColumnControl)oldContent).ColumnsChanged -= InvalidateChildContainerMeasure;
			}
			if(newContent != null) {
				((FrameworkElement)newContent).SizeChanged += ColumnScrollPanel_SizeChanged;
				((IColumnControl)newContent).ColumnsChanged += InvalidateChildContainerMeasure;
			}
			if(newContent == null)
				return;
			InvalidateChildContainerMeasure();
		}
		void InvalidateChildContainerMeasure() {
			if(ScrollViewer != null)
				UpdateExtraSpace();
			childContainer?.InvalidateMeasure();
		}
		void ColumnScrollPanel_SizeChanged(object sender, SizeChangedEventArgs e) {
			InvalidateChildContainerMeasure();
			UpdateExtraSpace();
		}
		double GetViewportWidth() {
			return Max(childContainer.DesiredSize.Width, 0d);
		}
		double GetViewportHeight() {
			return Max(childContainer.DesiredSize.Height, 0);
		}
		internal void UpdatePageButtons() {
			var content = (IColumnControl)Content;
			if(content == null)
				return;
			ItemsCount = content.Columns.Count();
			if(ItemsCount == 0) {
				PagesCount = 0;
				PageButtons = new ReadOnlyObservableCollection<PageItem>(new ObservableCollection<PageItem>());
				return;
			}
			var columnsPerPage = CalcColumnsPerPage();
			PagesCount = (ItemsCount + columnsPerPage - 1) / columnsPerPage;
			PageButtons = Enumerable.Range(0, PagesCount).Select(_ => new PageItem(this)).ToReadOnlyObservableCollection();
			NavigateToPage(Min(ActualCurrentPage, PagesCount - 1));
			PageButtons.ElementAt(ActualCurrentPage).SilentlyCheck(true);
			UpdateFirstLastPagesAndOpacityMask();
			UpdateVisibleArea();
		}
		private void UpdateFirstLastPagesAndOpacityMask() {
			IsLastPageSelected = ActualCurrentPage == PagesCount - 1;
			IsFirstPageSelected = ActualCurrentPage == 0;
			if(TypedContent != null && TypedContent.Columns != null && TypedContent.Columns.Any()) {
				double columnWidth = TypedContent.Columns.Average();
				if(!AllowDifferentColumnWidth)
					Debug.Assert(Abs(columnWidth - FirstColumnHeight) < 0.01d);
				const double safeMargin = 5d;
				IsOpacityMaskVisible = Abs(ScrollViewer.DesiredSize.Width % columnWidth) > safeMargin;
			}
			if(IsLastPageSelected) {
				IsOpacityMaskVisible = false;
			}
		}
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty ScrollViewerHorizontalOffsetProperty =
			DependencyProperty.Register("ScrollViewerHorizontalOffset", typeof(double), typeof(ColumnScrollControl), new PropertyMetadata(0d, (d, e) => ((ColumnScrollControl)d).ScrollViewer?.ScrollToHorizontalOffset((double)e.NewValue)));
		readonly Locker currentPageLocker = new Locker();
		public void NavigateToPage(int page, bool doNothingOnCurrentPage = true, bool instantly = false) {
			page = Min(Max(page, 0), PagesCount - 1);
			if(doNothingOnCurrentPage && ActualCurrentPage == page)
				return;
			if(page == -1)
				return;
			var scrollViewer = ScrollViewer;
			double newPos = page * (int)(scrollViewer.DesiredSize.Width / FirstColumnHeight) * FirstColumnHeight;
			if(Abs(scrollViewer.HorizontalOffset - newPos) < 0.001)
				return;
			ActualCurrentPage = page;
			currentPageLocker.DoLockedAction(() => SetCurrentValue(CurrentPageProperty, page));
			PageButtons.ToList().ForEach(b => b.SilentlyCheck(false));
			PageButtons[page].SilentlyCheck(true);
			UIContent.IsHitTestVisible = false;
			TypedContent.OnAnimationStarted();
			IsLeftOpacityMaskVisible = true;
			var animation = new DoubleAnimation {
				From = scrollViewer.HorizontalOffset,
				To = CalcPageOffset(page),
				Duration = TimeSpan.FromMilliseconds(1000),
				FillBehavior = FillBehavior.Stop,
				EasingFunction = new QuinticEase { EasingMode = EasingMode.EaseOut }
			};
			IsOpacityMaskVisible = true;
			Action onAnimationCompleted = () => {
				UpdateVisibleArea();
				UpdateFirstLastPagesAndOpacityMask();
				UIContent.IsHitTestVisible = true;
				IsLeftOpacityMaskVisible = false;
			};
			SetValue(ScrollViewerHorizontalOffsetProperty, animation.To);
			if(!instantly) {
				animation.Completed += (s, e) => onAnimationCompleted();
				BeginAnimation(ScrollViewerHorizontalOffsetProperty, animation);
			} else {
				onAnimationCompleted();
			}
			UpdateExtraSpace();
		}
		double FirstColumnHeight {
			get {
				if(!TypedContent.Columns.Any())
					return 1;
				return TypedContent.Columns.First();
			}
		}
		int CalcColumnsPerPage() {
			return Max((int)(Round(ScrollViewer.DesiredSize.Width / FirstColumnHeight, 1)), 1);
		}
		void UpdateExtraSpace() {
			var columns = TypedContent?.Columns;
			if(columns == null) {
				ExtraSpace = 0;
				return;
			}
			var columnsPerPage = CalcColumnsPerPage();
			var actualColumnsWidth = columns
				.Skip(columnsPerPage * ActualCurrentPage)
				.Take(columnsPerPage)
				.Count() * FirstColumnHeight;
			ExtraSpace = Max(0d, DesiredSize.Width - actualColumnsWidth);
		}
		double CalcPageOffset(int page) {
			var columns = TypedContent.Columns.ToList();
			int availableColumnsCount = CalcColumnsPerPage() * page;
			int maxColumnsCount = Math.Min(columns.Count, availableColumnsCount);
			if(AllowDifferentColumnWidth) {
				var offset = 0d;
				for(int i = 0; i < maxColumnsCount; i++) {
					offset += columns[i];
				}
				return offset + hardPadding;
			}
			return maxColumnsCount * FirstColumnHeight + hardPadding;
		}
		void UpdateVisibleArea() {
			TypedContent.VisibleArea = new Rect(
				(double)GetValue(ScrollViewerHorizontalOffsetProperty) - hardPadding,
				0,
				GetViewportWidth(),
				GetViewportHeight());
		}
		Action unsubscribeFromSwipe;
		Action unsubscribeFromPrevButton;
		Action unsubscribeFromNextButton;
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			unsubscribeFromSwipe?.Invoke();
			if(ScrollViewer != null) {
				ScrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
			}
			ScrollViewer = (ScrollViewer)GetTemplateChild("PART_ScrollViewer");
			if(ScrollViewer != null) {
				ScrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
			}
			unsubscribeFromSwipe = ScrollViewer.With(x => MouseTouchAdapter.SubscribeToSwipe(x, x, _ => { }, ProgressSwipe, CompleteSwipe));
			childContainer.Do(x => x.OnMeasure = null);
			childContainer = (ColumnScrollDecorator)GetTemplateChild("PART_ChildContainer");
			childContainer.Do(x => x.OnMeasure = UpdatePageButtons);
			unsubscribeFromPrevButton?.Invoke();
			var prevButton = (Button)GetTemplateChild("PART_PrevButton");
			unsubscribeFromPrevButton = prevButton.With(x => MouseTouchAdapter.SubscribeToPointerUp(x, this, a => NavigateToPage(Max(ActualCurrentPage - 1, 0))));
			unsubscribeFromNextButton?.Invoke();
			var nextButton = (Button)GetTemplateChild("PART_NextButton");
			unsubscribeFromNextButton = nextButton.With(x => MouseTouchAdapter.SubscribeToPointerUp(x, this, a => NavigateToPage(Min(ActualCurrentPage + 1, PagesCount - 1))));
			UpdateExtraSpace();
			UpdateScrollViewer();
		}
		private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e) {
			if(e.HorizontalOffset != (double)GetValue(ScrollViewerHorizontalOffsetProperty))
				UpdateScrollViewer();
		}
		void UpdateScrollViewer() {
			SetValue(ScrollViewerHorizontalOffsetProperty, hardPadding);
			ScrollViewer.ScrollToHorizontalOffset(hardPadding);
			if(ActualCurrentPage != -1 && PagesCount > 0)
				NavigateToPage(ActualCurrentPage, false, true);
		}
		void ProgressSwipe(Point position) {
			double correctedAccumulatedDelta = position.X;
			if(ActualCurrentPage == 0 && position.X > 0) {
				correctedAccumulatedDelta = Pow(position.X, 0.55d);
			}
			ScrollViewer.ScrollToHorizontalOffset(CalcPageOffset(ActualCurrentPage) - correctedAccumulatedDelta);
			UpdateFirstLastPagesAndOpacityMask();
		}
		void CompleteSwipe(Point position) {
			var scrollViewer = ScrollViewer;
			double toPrevPageDelta = scrollViewer.HorizontalOffset - CalcPageOffset(ActualCurrentPage - 1);
			double toNextPageDelta = CalcPageOffset(ActualCurrentPage + 1) - scrollViewer.HorizontalOffset;
			const double swipeSpaceCoeff = 0.08d;
			if(toPrevPageDelta < toNextPageDelta && Abs(position.X) > swipeSpaceCoeff * scrollViewer.DesiredSize.Width) {
				NavigateToPage(ActualCurrentPage - 1, false);
			} else if(Abs(position.X) > swipeSpaceCoeff * scrollViewer.DesiredSize.Width) {
				NavigateToPage(ActualCurrentPage + 1, false);
			} else {
				NavigateToPage(ActualCurrentPage, false);
			}
		}
		public ColumnScrollControl() {
			skipMouseWheelEvents = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 50), DispatcherPriority.Background, (s, e) => ((DispatcherTimer)s).Stop(), Dispatcher);
			this.SizeChanged += (s, e) => UpdateFirstLastPagesAndOpacityMask();
			this.PreviewMouseWheel += ColumnScrollPanel_MouseWheel;
		}
		double accumulatedDelta;
		readonly DispatcherTimer skipMouseWheelEvents;
		void ColumnScrollPanel_MouseWheel(object sender, MouseWheelEventArgs e) {
			if(!AllowScrollWithMouseWheel) return;
			e.Handled = true;
			if(skipMouseWheelEvents.IsEnabled) {
				skipMouseWheelEvents.Stop();
				skipMouseWheelEvents.Start();
				return;
			}
			var ex = e as MouseWheelEventArgsEx;
			var delta = ex == null ? e.Delta : (ex.DeltaY - ex.DeltaX);
			bool sameSign = (delta > 0 && accumulatedDelta >= 0) || (delta < 0 && accumulatedDelta <= 0);
			if(sameSign) {
				accumulatedDelta += delta;
			} else {
				accumulatedDelta = delta;
			}
			if(Abs(accumulatedDelta) > 180d) {
				if(accumulatedDelta > 0) {
					NavigateToPage(Max(ActualCurrentPage - 1, 0));
				} else if(accumulatedDelta < 0) {
					NavigateToPage(Min(ActualCurrentPage + 1, PagesCount - 1));
				}
				accumulatedDelta = 0d;
				skipMouseWheelEvents.Start();
			}
		}
	}
	class NonInteractiveScrollViewer : ScrollViewer {
		public NonInteractiveScrollViewer() {
			ScrollBarExtensions.SetAllowMouseScrolling(this, false);
		}
		protected override void OnMouseWheel(MouseWheelEventArgs e) {
		}
	}
}
