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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer.Internal;
using System.Windows;
using DevExpress.Xpf.Utils;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
namespace DevExpress.Xpf.DemoBase.Helpers.Internal {
	[TemplatePart(Name = "TextPresenter", Type = typeof(RichTextPresenter))]
	[TemplatePart(Name = "HighlightedRangesPresenter", Type = typeof(Canvas))]
	[TemplatePart(Name = "ExpandButtonsPresenter", Type = typeof(Canvas))]
	[TemplatePart(Name = "ScrollViewer", Type = typeof(ScrollViewer))]
	public class CodeViewPresenter : Control {
		[IgnoreDependencyPropertiesConsistencyChecker]
		public static readonly DependencyProperty HighlightedRangesProperty =
			DependencyPropertyManager.Register("HighlightedRanges", typeof(ObservableCollection<SLTextRange>), typeof(CodeViewPresenter), new PropertyMetadata(null,
				(d, e) => ((CodeViewPresenter)d).OnHighlightedRangesChanged(e)));
		public static readonly DependencyProperty HighlightedRangeRectangleProperty =
			DependencyPropertyManager.Register("HighlightedRangeRectangle", typeof(DataTemplate), typeof(CodeViewPresenter), new PropertyMetadata(null));
		public static readonly DependencyProperty CodeTextProperty =
			DependencyPropertyManager.Register("CodeText", typeof(CodeLanguageText), typeof(CodeViewPresenter), new PropertyMetadata(null,
				(d, e) => ((CodeViewPresenter)d).OnCodeTextChanged(e)));
		public static readonly DependencyProperty CurrentPositionProperty =
			DependencyPropertyManager.Register("CurrentPosition", typeof(TextPointer), typeof(CodeViewPresenter), new PropertyMetadata(null));
		public static readonly DependencyProperty AutoExpandAllRegionsProperty =
			DependencyPropertyManager.Register("AutoExpandAllRegions", typeof(bool), typeof(CodeViewPresenter), new PropertyMetadata(false));
		static CodeViewPresenter() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeViewPresenter), new FrameworkPropertyMetadata(typeof(CodeViewPresenter)));
		}
		readonly Dictionary<SLTextRange, Rectangle> highlights = new Dictionary<SLTextRange, Rectangle>();
#if DEBUGTEST
		public
#endif            
		List<string> themePalleteNames = new List<string>();
		public CodeViewPresenter() {
			FocusHelper.SetFocusable(this, false);
			HighlightedRanges = new ObservableCollection<SLTextRange>();
			HeaderContentControls = new List<CodeLineHeaderContentControl>();
			this.AddHandler(RichTextPresenter.KeyDownEvent, (KeyEventHandler)OnKeyDown, true);
			FlowDirection = System.Windows.FlowDirection.LeftToRight;
			Loaded += CodeViewPresenter_Loaded;
			UpdatePalleteNames();
		}
		void CodeViewPresenter_Loaded(object sender, RoutedEventArgs e) {
			UpdateTextPresenter();
		}
		internal ObservableCollection<SLTextRange> HighlightedRanges { get { return (ObservableCollection<SLTextRange>)GetValue(HighlightedRangesProperty); } set { SetValue(HighlightedRangesProperty, value); } }
		public DataTemplate HighlightedRangeRectangle { get { return (DataTemplate)GetValue(HighlightedRangeRectangleProperty); } set { SetValue(HighlightedRangeRectangleProperty, value); } }
		public CodeLanguageText CodeText { get { return (CodeLanguageText)GetValue(CodeTextProperty); } set { SetValue(CodeTextProperty, value); } }
		public TextPointer CurrentPosition { get { return (TextPointer)GetValue(CurrentPositionProperty); } set { SetValue(CurrentPositionProperty, value); } }
		public bool AutoExpandAllRegions { get { return (bool)GetValue(AutoExpandAllRegionsProperty); } set { SetValue(AutoExpandAllRegionsProperty, value);  } }	   
		internal IRichTextPresenter TextPresenter { get; private set; }
		protected Canvas HighlightedRangesPresenter { get; private set; }
		protected Canvas ExpandButtonsPresenter { get; private set; }
		protected ScrollViewer ScrollViewer { get; private set; }
		List<CodeLineHeaderContentControl> HeaderContentControls;
		bool isCodeExampleView;
		internal void ScrollTo(SLTextRange range) {
			if(range == null || ScrollViewer == null) return;
			Rect rect = GetRangeRect(range);
			double horizontalOffsetMax = rect.Left;
			double horizontalOffsetMin = rect.Right - ScrollViewer.ViewportWidth;
			if(horizontalOffsetMin > horizontalOffsetMax)
				horizontalOffsetMin = rect.Left - ScrollViewer.ViewportWidth;
			double verticalOffsetMax = rect.Top;
			double verticalOffsetMin = rect.Bottom - ScrollViewer.ViewportHeight;
			if(verticalOffsetMin > verticalOffsetMax)
				verticalOffsetMin = rect.Top - ScrollViewer.ViewportHeight;
			if(ScrollViewer.HorizontalOffset < horizontalOffsetMin || ScrollViewer.HorizontalOffset > horizontalOffsetMax)
				ScrollViewer.ScrollToHorizontalOffset(horizontalOffsetMin);
			if(ScrollViewer.VerticalOffset < verticalOffsetMin || ScrollViewer.VerticalOffset > verticalOffsetMax)
				ScrollViewer.ScrollToVerticalOffset((verticalOffsetMin + verticalOffsetMax) / 2.0);
		}
		protected virtual void OnHighlightedRangesChanged(DependencyPropertyChangedEventArgs e) {
			ObservableCollection<SLTextRange> oldValue = (ObservableCollection<SLTextRange>)e.OldValue;
			ObservableCollection<SLTextRange> newValue = (ObservableCollection<SLTextRange>)e.NewValue;
			if(oldValue != null)
				oldValue.CollectionChanged -= OnHighlightedRangesCollectionChanged;
			if(newValue != null)
				newValue.CollectionChanged += OnHighlightedRangesCollectionChanged;
			RedrawHighlightedRanges();
		}
		void OnHighlightedRangesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			switch(e.Action) {
			case NotifyCollectionChangedAction.Reset:
				RedrawHighlightedRanges();
				break;
			case NotifyCollectionChangedAction.Add:
				AddHighlightedRanges(e.NewItems);
				break;
			case NotifyCollectionChangedAction.Remove:
				RemoveHighlightedRanges(e.OldItems);
				break;
			case NotifyCollectionChangedAction.Replace:
				RemoveHighlightedRanges(e.OldItems);
				AddHighlightedRanges(e.NewItems);
				break;
			}
		}
		protected virtual void RedrawHighlightedRanges() {
			if(HighlightedRangesPresenter == null) return;
			highlights.Clear();
			HighlightedRangesPresenter.Children.Clear();
			AddHighlightedRanges(HighlightedRanges);
		}
		protected virtual void AddHighlightedRanges(IList list) {
			if(list == null || HighlightedRangesPresenter == null) return;
			foreach(SLTextRange range in list) {
				Rectangle highlight = CreateHighlightRectangle(GetRangeRect(range));
				highlights.Add(range, highlight);
				HighlightedRangesPresenter.Children.Add(highlight);
			}
		}
		protected virtual void RemoveHighlightedRanges(IList list) {
			if(list == null || HighlightedRangesPresenter == null) return;
			foreach(SLTextRange range in list) {
				Rectangle highlight = highlights[range];
				highlights.Remove(range);
				HighlightedRangesPresenter.Children.Remove(highlight);
			}
		}
		protected virtual Rectangle CreateHighlightRectangle(Rect bounds) {
			Rectangle rectangle = HighlightedRangeRectangle == null ? null : HighlightedRangeRectangle.LoadContent() as Rectangle;
			if(rectangle == null)
				rectangle = new Rectangle();
			rectangle.Width = bounds.Width - rectangle.Margin.Left - rectangle.Margin.Right;
			rectangle.Height = bounds.Height - rectangle.Margin.Top - rectangle.Margin.Bottom;
			Canvas.SetLeft(rectangle, bounds.Left);
			Canvas.SetTop(rectangle, bounds.Top);
			return rectangle;
		}
		internal virtual Rect GetRangeRect(SLTextRange range) {
			Rect s = range.Start.GetCharacterRect(LogicalDirection.Forward);
			s.Union(range.End.GetCharacterRect(LogicalDirection.Backward));
			return s;
		}
		RootCodeBlock CodePresenter = null;
		protected virtual void OnCodeTextChanged(DependencyPropertyChangedEventArgs e) {
			UpdateTextPresenter();
		}
		protected internal virtual void UpdateTextPresenter() {
			if(CodeText == null || TextPresenter == null || !IsLoaded) {
				if(!string.IsNullOrEmpty(currentKey)) {
					UpdateTextPresenterFromCache(currentKey);
					return;
				}
				CodePresenter = null;
				return;
			}
			bool isDark = GetIsDark();
			Background = new SolidColorBrush(isDark ? CodeViewControl.DarkBackground : Color.FromArgb(255, 255, 255, 255));
			CodePresenter = RichTextHelper.CreateCodePresenter(CodeText.Language, CodeText.Text == null ? null : CodeText.Text(), isDark);
			CurrentPosition = null;
			HighlightedRanges.Clear();
			double width = RichTextHelper.SetText(TextPresenter, CodePresenter);
			TextPresenter.TextWidthMaxSet(width);
			UpdateRegions();
			if(AutoExpandAllRegions)
				ExpandAllRegions();
		}
		Dictionary<string, Cache> cache = new Dictionary<string, Cache>();
		class Cache {
			public RootCodeBlock Light { get; set; }
			public RootCodeBlock Dark { get; set; }
			public List<ExpandableHierarchyCodeBlock> RegionsLight { get; set; }
			public List<ExpandableHierarchyCodeBlock> RegionsDark { get; set; }
		}
		public int CacheCount { get { return cache.Count; } }
		public void AddCache(string key, CodeLanguageText code) {
		   var light = RichTextHelper.CreateCodePresenter(code.Language, code.Text == null ? null : code.Text(), false);
			var dark = RichTextHelper.CreateCodePresenter(code.Language, code.Text == null ? null : code.Text(), true);
			cache.Add(key, new Cache() { Dark = dark, Light = light, RegionsLight = light.GetRegions(), RegionsDark = dark.GetRegions() }); ;
		}
		string currentKey = null;
		public void UpdateTextPresenterFromCache(string key) {
			Cache codePresenters;
			if(!cache.TryGetValue(key, out codePresenters) || TextPresenter == null || !IsLoaded) {
				CodePresenter = null;
				return;
			}
			currentKey = key;
			bool isDark = GetIsDark();
			Background = new SolidColorBrush(isDark ? CodeViewControl.DarkBackground : Color.FromArgb(255, 255, 255, 255));
			CodePresenter = isDark ? codePresenters.Dark : codePresenters.Light;
			CurrentPosition = null;
			HighlightedRanges.Clear();				 
			double width = RichTextHelper.SetText(TextPresenter, CodePresenter);
			TextPresenter.TextWidthMaxSet(width);
			UpdateRegions(isDark ? codePresenters.RegionsDark : codePresenters.RegionsLight);
			if(AutoExpandAllRegions)
			ExpandAllRegions(isDark ? codePresenters.RegionsDark : codePresenters.RegionsLight);			
		}
		bool GetIsDark() {
			var name = CommonThemeHelper.GetTreeWalkerThemeName(this);
			if(name == null) return false;
			if(name == Theme.Office2013DarkGray.Name || name == Theme.Office2013DarkGrayTouch.Name ||
				name == Theme.Office2007Black.Name)
				return false;
			if(name == Theme.Office2019HighContrast.Name || name == Theme.Office2019HighContrastTouch.Name)
				return true;
			foreach(var palette in themePalleteNames)
				if(name.StartsWith(palette)) {
					name = name.Replace(palette, "");
					break;
				}
			return name.Contains("Dark") || name.Contains("Black");
		}
		void UpdatePalleteNames() {
			var paletteFields = typeof(PredefinedThemePalettes).GetProperties(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
				.Where(x => x.PropertyType == typeof(PredefinedThemePalette));
			themePalleteNames = paletteFields.Select(x => ((PredefinedThemePalette)x.GetValue(null)).Name).ToList();
		}
		internal void UpdateRegions(List<ExpandableHierarchyCodeBlock> regions = null) {
			HeaderContentControls = new List<CodeLineHeaderContentControl>();
			ExpandButtonsPresenter.Children.Clear();
			string allText = this.GetDisplayedText();
			int codeTextStartIndex = 0;
			foreach(var region in regions ?? CodePresenter.GetRegions()) {
				codeTextStartIndex = allText.IndexOf(region.FirstLineText, codeTextStartIndex);
				CreateRegionHeaderBorder(region, codeTextStartIndex);
				PlaceExpandButton(region);
			}
		}
		public void ExpandAllRegions() {
			ExpandAllRegions(null);		   
		}
		void ExpandAllRegions(List<ExpandableHierarchyCodeBlock> regions = null) {
			foreach(var block in HeaderContentControls)
				block.Expand();
			UpdateRegions(regions);
		}
		void CreateRegionHeaderBorder(ExpandableHierarchyCodeBlock region, int codeTextStartIndex) {
			var collapsedTextRange = region.GetCollapsedTextRange();
			if(collapsedTextRange == null)
				return;
			Rect rect = GetRangeRect(collapsedTextRange);
			if(rect.IsEmpty)
				return;
			CreateCollapsedBlockRectangle(rect, region, codeTextStartIndex);
		}
		internal void PlaceExpandButton(ExpandableHierarchyCodeBlock region) {
			Rect bounds = GetRangeRect(new SLTextRange(region.Inline.ContentStart, region.Inline.ContentEnd));
			if(bounds.IsEmpty)
				return;
			CodeLineToggleButton button = new CodeLineToggleButton() { Command = new DelegateCommand(() => { region.Expanded = !region.Expanded; UpdateRegions(); }), IsChecked = region.Expanded };
			Canvas.SetLeft(button, bounds.Left);
			Canvas.SetTop(button, bounds.Top);
			ExpandButtonsPresenter.Children.Add(button);
		}
		internal void CreateCollapsedBlockRectangle(Rect bounds, ExpandableHierarchyCodeBlock currentRegion, int codeTextStartIndex) {
			CodeLineHeaderContentControl regionControl = new CodeLineHeaderContentControl();
			regionControl.SetCodeBlock(this, currentRegion, codeTextStartIndex);
			regionControl.Width = bounds.Width - regionControl.Margin.Left - regionControl.Margin.Right;
			regionControl.Height = bounds.Height - regionControl.Margin.Top - regionControl.Margin.Bottom;
			Canvas.SetLeft(regionControl, bounds.Left + 10);
			Canvas.SetTop(regionControl, bounds.Top);
			HeaderContentControls.Add(regionControl);
			ExpandButtonsPresenter.Children.Add(regionControl);
		}
		void OnKeyDown(object sender, KeyEventArgs e) {
			const double scrollDiff = 14.0; 
			switch(e.Key) {
			case Key.Up:
				ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - scrollDiff);
				break;
			case Key.Down:
				ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + scrollDiff);
				break;
			case Key.Left:
				ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - scrollDiff);
				break;
			case Key.Right:
				ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + scrollDiff);
				break;
			case Key.Home:
				ScrollViewer.ScrollToVerticalOffset(0.0);
				break;
			case Key.End:
				ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
				break;
			case Key.PageUp:
				ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - ScrollViewer.ActualHeight);
				break;
			case Key.PageDown:
				ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + ScrollViewer.ActualHeight);
				break;
			}
		}
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			TextPresenter = (RichTextPresenter)GetTemplateChild("TextPresenter");
			TextPresenter.CopyTextToClipboard += TextPresenter_CopyTextToClipboard;
			HighlightedRangesPresenter = (Canvas)GetTemplateChild("HighlightedRangesPresenter");
			ExpandButtonsPresenter = (Canvas)GetTemplateChild("ExpandButtonsPresenter");
			ScrollViewer = (ScrollViewer)GetTemplateChild("ScrollViewer");
			UpdateTextPresenter();
			isCodeExampleView = Core.Native.LayoutHelper.FindParentObject<ShowcaseBrowserModuleCore>(this) != null;
		}
		void TextPresenter_CopyTextToClipboard(object sender, CopyEventArgs e) {
			string allText = GetDisplayedText();
			int copyTextStartIndex = allText.IndexOf(e.CopyText);
			int copyTextEndIndex = copyTextStartIndex + e.CopyText.Length;
			if(copyTextStartIndex == copyTextEndIndex)
				return;
			string resultCopyText = e.CopyText;
			int inCopyTextIndex = 0;
			foreach(var headerContentControl in HeaderContentControls) {
				if(headerContentControl.CodeStartIndex > copyTextEndIndex || headerContentControl.CodeStartIndex < copyTextStartIndex)
					continue;
				int searchRegionIndex = resultCopyText.IndexOf(headerContentControl.FirstLineText, inCopyTextIndex);
				if(searchRegionIndex == -1)
					continue;
				inCopyTextIndex = searchRegionIndex + headerContentControl.FirstLineText.Length;
				resultCopyText = resultCopyText.Remove(searchRegionIndex, headerContentControl.FirstLineText.Length);
				resultCopyText = resultCopyText.Insert(searchRegionIndex, headerContentControl.InnerBlockText);
			}
			var snippetSource = isCodeExampleView ? "code example" : "source file";
			Logger.Log($"Snippet has been copied from {snippetSource}.");
			e.CopyText = resultCopyText.Replace("\n", Environment.NewLine);
		}
		protected override Size ArrangeOverride(Size size) {
			size = base.ArrangeOverride(size);
			Clip = new RectangleGeometry() { Rect = new Rect(new Point(), size) };
			return size;
		}
		internal string GetDisplayedText() {
			return RichTextHelper.GetText(TextPresenter);
		}
		public void Search(string textLine, string highlightText) {
			if(string.IsNullOrEmpty(textLine)) {
				HighlightedRanges.Clear();
				ScrollViewer?.ScrollToVerticalOffset(0);
				return;
			}
			TextPointer currentPosition = TextPresenter.ContentStart;		
			SLTextRange r = RichTextHelper.FindText(TextPresenter, textLine, new SLTextRange(currentPosition, null), StringComparison.OrdinalIgnoreCase);	  
			if (r != null)
				r = RichTextHelper.FindText(TextPresenter, highlightText, r, StringComparison.OrdinalIgnoreCase);
			if(r == null)
				HighlightedRanges.Clear();
			else if(HighlightedRanges.Count == 0)
				HighlightedRanges.Add(r);
			else
				HighlightedRanges[0] = r;
			CurrentPosition = r == null ? null : r.End;
			ScrollTo(r);			
		}
	}
	public class CodeLineToggleButton : ToggleButton {
		static CodeLineToggleButton() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeLineToggleButton), new FrameworkPropertyMetadata(typeof(CodeLineToggleButton)));
		}
		public CodeLineToggleButton() {
			FocusHelper.SetFocusable(this, false);
		}
	}
	public class CodeLineHeaderContentControl : Control {
		static CodeLineHeaderContentControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(CodeLineHeaderContentControl), new FrameworkPropertyMetadata(typeof(CodeLineHeaderContentControl)));
		}
		ExpandableHierarchyCodeBlock Block;
		CodeViewPresenter Presenter;
		public string FirstLineText { get { return Block.FirstLineText; } }
		public int CodeStartIndex { get; private set; }
		public string InnerBlockText { get { return Block.Text; } }
		public CodeLineHeaderContentControl() {
			FocusHelper.SetFocusable(this, false);
		}
		internal void SetCodeBlock(CodeViewPresenter presenter, ExpandableHierarchyCodeBlock block, int codeStartIndex) {
			Block = block;
			Presenter = presenter;
			CodeStartIndex = codeStartIndex;
			ToolTip = String.Empty;
			MouseDoubleClick += CodeLineHeaderContentControl_MouseDoubleClick;
		}
		protected override void OnToolTipOpening(ToolTipEventArgs e) {
			if(Block != null && ToolTip.ToString() == String.Empty)
				ToolTip = Block.GetHintContent();
			base.OnToolTipOpening(e);
		}
		void CodeLineHeaderContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
			Block.Expanded = !Block.Expanded;
			Presenter.UpdateRegions();
		}
		public void Expand() {
			Block.Expanded = true;
		}
	}
}
