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

namespace DevExpress.Xpf.DemoBase.Helpers {
using System.Windows;
using DevExpress.Xpf.Core.Native;
	partial class SidebarWindow {
		public static readonly DependencyProperty SidebarProperty;
		public UIElement Sidebar {
			get { return (UIElement)GetValue(SidebarProperty); }
			set { SetValue(SidebarProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase {
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
using DevExpress.Xpf.DemoBase.ThemeLayoutHelpers;
using DevExpress.Xpf.DemoCenterBase;
	partial class DemoBaseControl {
		public static readonly DependencyProperty IsNavigationVisibleProperty;
		static readonly DependencyPropertyKey IsNavigationVisiblePropertyKey;
		public bool IsNavigationVisible {
			get { return (bool)GetValue(IsNavigationVisibleProperty); }
			private set { SetValue(IsNavigationVisiblePropertyKey, value); }
		}
		public static readonly DependencyProperty DemoProperty;
		static readonly DependencyPropertyKey DemoPropertyKey;
		public DemoDescription Demo {
			get { return (DemoDescription)GetValue(DemoProperty); }
			private set { SetValue(DemoPropertyKey, value); }
		}
		public static readonly DependencyProperty ModulesProperty;
		static readonly DependencyPropertyKey ModulesPropertyKey;
		public ReadOnlyObservableCollection<DemoModuleDescription> Modules {
			get { return (ReadOnlyObservableCollection<DemoModuleDescription>)GetValue(ModulesProperty); }
			private set { SetValue(ModulesPropertyKey, value); }
		}
		public static readonly DependencyProperty DemoBaseProperty;
		static readonly DependencyPropertyKey DemoBasePropertyKey;
		public static DemoBaseControl GetDemoBase(DependencyObject d) {
			return (DemoBaseControl)d.GetValue(DemoBaseProperty);
		}
		static void SetDemoBase(DependencyObject d, DemoBaseControl value) {
			d.SetValue(DemoBasePropertyKey, value);
		}
		public static readonly DependencyProperty SelectedModuleProperty;
		public DemoModuleDescription SelectedModule {
			get { return (DemoModuleDescription)GetValue(SelectedModuleProperty); }
			set { SetValue(SelectedModuleProperty, value); }
		}
		public static readonly DependencyProperty HideThemeSelectorProperty;
		public bool HideThemeSelector {
			get { return (bool)GetValue(HideThemeSelectorProperty); }
			set { SetValue(HideThemeSelectorProperty, value); }
		}
		public static readonly DependencyProperty FixtureTypeForClickOnceTestingProperty;
		public Type FixtureTypeForClickOnceTesting {
			get { return (Type)GetValue(FixtureTypeForClickOnceTestingProperty); }
			set { SetValue(FixtureTypeForClickOnceTestingProperty, value); }
		}
		public static readonly DependencyProperty LoadingInProgressProperty;
		static readonly DependencyPropertyKey LoadingInProgressPropertyKey;
		public bool LoadingInProgress {
			get { return (bool)GetValue(LoadingInProgressProperty); }
			private set { SetValue(LoadingInProgressPropertyKey, value); }
		}
		public static readonly DependencyProperty CurrentDemoModuleExceptionProperty;
		static readonly DependencyPropertyKey CurrentDemoModuleExceptionPropertyKey;
		public Exception CurrentDemoModuleException {
			get { return (Exception)GetValue(CurrentDemoModuleExceptionProperty); }
			private set { SetValue(CurrentDemoModuleExceptionPropertyKey, value); }
		}
		public static readonly DependencyProperty CurrentDemoModuleProperty;
		static readonly DependencyPropertyKey CurrentDemoModulePropertyKey;
		public DemoModule CurrentDemoModule {
			get { return (DemoModule)GetValue(CurrentDemoModuleProperty); }
			private set { SetValue(CurrentDemoModulePropertyKey, value); }
		}
		public static readonly DependencyProperty ShowLoadingIndicatorProperty;
		static readonly DependencyPropertyKey ShowLoadingIndicatorPropertyKey;
		public bool ShowLoadingIndicator {
			get { return (bool)GetValue(ShowLoadingIndicatorProperty); }
			private set { SetValue(ShowLoadingIndicatorPropertyKey, value); }
		}
		public static readonly DependencyProperty FullWindowModeProperty;
		public bool FullWindowMode {
			get { return (bool)GetValue(FullWindowModeProperty); }
			set { SetValue(FullWindowModeProperty, value); }
		}
		public static readonly DependencyProperty StartupModuleNameProperty;
		public static string GetStartupModuleName(DependencyObject d) {
			return (string)d.GetValue(StartupModuleNameProperty);
		}
		public static void SetStartupModuleName(DependencyObject d, string value) {
			d.SetValue(StartupModuleNameProperty, value);
		}
		public static readonly DependencyProperty StartupArgumentsProperty;
		public static string[] GetStartupArguments(DependencyObject d) {
			return (string[])d.GetValue(StartupArgumentsProperty);
		}
		public static void SetStartupArguments(DependencyObject d, string[] value) {
			d.SetValue(StartupArgumentsProperty, value);
		}
		public static readonly DependencyProperty StartupMethodProperty;
		public static DemoBaseControlStartup GetStartupMethod(DependencyObject d) {
			return (DemoBaseControlStartup)d.GetValue(StartupMethodProperty);
		}
		public static void SetStartupMethod(DependencyObject d, DemoBaseControlStartup value) {
			d.SetValue(StartupMethodProperty, value);
		}
		public static readonly DependencyProperty SearchTextProperty;
		public string SearchText {
			get { return (string)GetValue(SearchTextProperty); }
			set { SetValue(SearchTextProperty, value); }
		}
		public static readonly DependencyProperty ActiveDemoModuleProperty;
		public DemoModuleDescription ActiveDemoModule {
			get { return (DemoModuleDescription)GetValue(ActiveDemoModuleProperty); }
			set { SetValue(ActiveDemoModuleProperty, value); }
		}
		public static readonly DependencyProperty AllowUseNewFilteringUIProperty;
		public bool AllowUseNewFilteringUI {
			get { return (bool)GetValue(AllowUseNewFilteringUIProperty); }
			set { SetValue(AllowUseNewFilteringUIProperty, value); }
		}
		public static readonly DependencyProperty UseNewFilteringUIProperty;
		public bool? UseNewFilteringUI {
			get { return (bool?)GetValue(UseNewFilteringUIProperty); }
			set { SetValue(UseNewFilteringUIProperty, value); }
		}
		public static readonly DependencyProperty ShowModuleInfoInTooltipProperty;
		public bool ShowModuleInfoInTooltip {
			get { return (bool)GetValue(ShowModuleInfoInTooltipProperty); }
			set { SetValue(ShowModuleInfoInTooltipProperty, value); }
		}
		public static readonly DependencyProperty DebugTestProperty;
		public bool DebugTest {
			get { return (bool)GetValue(DebugTestProperty); }
			set { SetValue(DebugTestProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase {
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
using DevExpress.Xpf.DemoBase.ThemeLayoutHelpers;
using DevExpress.Xpf.DemoCenterBase;
	partial class SplashScreeenManagerServiceEx {
		public static readonly DependencyProperty ShowLoadingIndicatorProperty;
		public bool ShowLoadingIndicator {
			get { return (bool)GetValue(ShowLoadingIndicatorProperty); }
			set { SetValue(ShowLoadingIndicatorProperty, value); }
		}
		public static readonly DependencyProperty IsCompletelyLoadedProperty;
		public bool IsCompletelyLoaded {
			get { return (bool)GetValue(IsCompletelyLoadedProperty); }
			set { SetValue(IsCompletelyLoadedProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase {
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
using DevExpress.Xpf.DemoBase.ThemeLayoutHelpers;
using DevExpress.Xpf.DemoCenterBase;
	partial class ShowCodeVisibilityBehavior {
		public static readonly DependencyProperty IsVisibleProperty;
		public bool IsVisible {
			get { return (bool)GetValue(IsVisibleProperty); }
			set { SetValue(IsVisibleProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Helpers {
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Threading;
using DevExpress.Mvvm.Native;
	partial class WindowTitleService {
		public static readonly DependencyProperty WindowTitleProperty;
		public string WindowTitle {
			get { return (string)GetValue(WindowTitleProperty); }
			set { SetValue(WindowTitleProperty, value); }
		}
		public static readonly DependencyProperty WindowIconProperty;
		public ImageSource WindowIcon {
			get { return (ImageSource)GetValue(WindowIconProperty); }
			set { SetValue(WindowIconProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase {
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using DevExpress.Mvvm;
	partial class DemoModule {
		public static readonly DependencyProperty IsPopupContentInvisibleProperty;
		static readonly DependencyPropertyKey IsPopupContentInvisiblePropertyKey;
		public bool IsPopupContentInvisible {
			get { return (bool)GetValue(IsPopupContentInvisibleProperty); }
			private set { SetValue(IsPopupContentInvisiblePropertyKey, value); }
		}
		public static readonly DependencyProperty OwnerProperty;
		static readonly DependencyPropertyKey OwnerPropertyKey;
		public object Owner {
			get { return (object)GetValue(OwnerProperty); }
			private set { SetValue(OwnerPropertyKey, value); }
		}
		public static readonly DependencyProperty OptionsProperty;
		static readonly DependencyPropertyKey OptionsPropertyKey;
		public FrameworkElement Options {
			get { return (FrameworkElement)GetValue(OptionsProperty); }
			private set { SetValue(OptionsPropertyKey, value); }
		}
		public static readonly DependencyProperty OptionsDataContextProperty;
		public object OptionsDataContext {
			get { return (object)GetValue(OptionsDataContextProperty); }
			set { SetValue(OptionsDataContextProperty, value); }
		}
		public static readonly DependencyProperty RibbonStyleProperty;
		public Style RibbonStyle {
			get { return (Style)GetValue(RibbonStyleProperty); }
			set { SetValue(RibbonStyleProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Helpers {
using System.Windows;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Core;
	partial class TouchUICoerceBehavior {
		public static readonly DependencyProperty AllowTouchUIProperty;
		public bool AllowTouchUI {
			get { return (bool)GetValue(AllowTouchUIProperty); }
			set { SetValue(AllowTouchUIProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Helpers {
using System;
using System.Windows;
using DevExpress.Mvvm.Native;
	partial class SvgImageBehavior {
		public static readonly DependencyProperty UriProperty;
		public Uri Uri {
			get { return (Uri)GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}
		public static readonly DependencyProperty UriStringProperty;
		public string UriString {
			get { return (string)GetValue(UriStringProperty); }
			set { SetValue(UriStringProperty, value); }
		}
		public static readonly DependencyProperty AutoSizeProperty;
		public bool AutoSize {
			get { return (bool)GetValue(AutoSizeProperty); }
			set { SetValue(AutoSizeProperty, value); }
		}
		public static readonly DependencyProperty SizeProperty;
		public Size? Size {
			get { return (Size?)GetValue(SizeProperty); }
			set { SetValue(SizeProperty, value); }
		}
		public static readonly DependencyProperty TargetProperty;
		public DependencyProperty Target {
			get { return (DependencyProperty)GetValue(TargetProperty); }
			set { SetValue(TargetProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoChooser {
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Utils;
using DevExpress.Xpf.DemoBase.Helpers;
	partial class GroupedLinksControl {
		public static readonly DependencyProperty GroupHeaderTemplateProperty;
		public DataTemplate GroupHeaderTemplate {
			get { return (DataTemplate)GetValue(GroupHeaderTemplateProperty); }
			set { SetValue(GroupHeaderTemplateProperty, value); }
		}
		public static readonly DependencyProperty ShowAllTemplateProperty;
		public DataTemplate ShowAllTemplate {
			get { return (DataTemplate)GetValue(ShowAllTemplateProperty); }
			set { SetValue(ShowAllTemplateProperty, value); }
		}
		public static readonly DependencyProperty LayoutTypeProperty;
		public GroupedLinksControlLayoutType LayoutType {
			get { return (GroupedLinksControlLayoutType)GetValue(LayoutTypeProperty); }
			set { SetValue(LayoutTypeProperty, value); }
		}
		public static readonly DependencyProperty LinkTemplateProperty;
		public DataTemplate LinkTemplate {
			get { return (DataTemplate)GetValue(LinkTemplateProperty); }
			set { SetValue(LinkTemplateProperty, value); }
		}
		public static readonly DependencyProperty LinkLabelTemplateProperty;
		public DataTemplate LinkLabelTemplate {
			get { return (DataTemplate)GetValue(LinkLabelTemplateProperty); }
			set { SetValue(LinkLabelTemplateProperty, value); }
		}
		public static readonly DependencyProperty FilterStringProperty;
		public string FilterString {
			get { return (string)GetValue(FilterStringProperty); }
			set { SetValue(FilterStringProperty, value); }
		}
		public static readonly DependencyProperty VisibleGroupsCountProperty;
		public int VisibleGroupsCount {
			get { return (int)GetValue(VisibleGroupsCountProperty); }
			set { SetValue(VisibleGroupsCountProperty, value); }
		}
		public static readonly DependencyProperty MaxGroupsPerColumnProperty;
		public int MaxGroupsPerColumn {
			get { return (int)GetValue(MaxGroupsPerColumnProperty); }
			set { SetValue(MaxGroupsPerColumnProperty, value); }
		}
		public static readonly DependencyProperty HColumnSpacingProperty;
		public double HColumnSpacing {
			get { return (double)GetValue(HColumnSpacingProperty); }
			set { SetValue(HColumnSpacingProperty, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Helpers {
using System.Windows;
using DevExpress.Xpf.Ribbon;
	partial class RibbonDynamicCategoryBehavior {
		public static readonly DependencyProperty IsVisibleProperty;
		public static bool? GetIsVisible(RibbonPage d) {
			return (bool?)d.GetValue(IsVisibleProperty);
		}
		public static void SetIsVisible(RibbonPage d, bool? value) {
			d.SetValue(IsVisibleProperty, value);
		}
	}
}
namespace DevExpress.Xpf.DemoBase.Helpers {
using System.Windows;
	partial class DocumentPresenterContentAdapter {
		public static readonly DependencyProperty DocumentMaxHeightProperty;
		public double DocumentMaxHeight {
			get { return (double)GetValue(DocumentMaxHeightProperty); }
			set { SetValue(DocumentMaxHeightProperty, value); }
		}
		public static readonly DependencyProperty AlwaysShowFullDescriptionProperty;
		public bool AlwaysShowFullDescription {
			get { return (bool)GetValue(AlwaysShowFullDescriptionProperty); }
			set { SetValue(AlwaysShowFullDescriptionProperty, value); }
		}
		public static readonly DependencyProperty DocumentProperty;
		public string Document {
			get { return (string)GetValue(DocumentProperty); }
			set { SetValue(DocumentProperty, value); }
		}
		public static readonly DependencyProperty UseShortDocumentProperty;
		static readonly DependencyPropertyKey UseShortDocumentPropertyKey;
		public bool UseShortDocument {
			get { return (bool)GetValue(UseShortDocumentProperty); }
			private set { SetValue(UseShortDocumentPropertyKey, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoChooser.Helpers {
using System.Windows;
using DevExpress.DemoData.Model;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoCenter;
using DevExpress.Xpf.DemoCenter.Internal;
using DevExpress.Xpf.DemoChooser.Internal;
	partial class CrossPlatformDemoCarouselControl {
		public static readonly DependencyProperty FlyoutVisibilityProperty;
		public Visibility FlyoutVisibility {
			get { return (Visibility)GetValue(FlyoutVisibilityProperty); }
			set { SetValue(FlyoutVisibilityProperty, value); }
		}
		public static readonly DependencyProperty PanelProperty;
		static readonly DependencyPropertyKey PanelPropertyKey;
		public CrossPlatformDemoCarouselPanel Panel {
			get { return (CrossPlatformDemoCarouselPanel)GetValue(PanelProperty); }
			private set { SetValue(PanelPropertyKey, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoChooser.Helpers {
using System.Windows;
using DevExpress.DemoData.Model;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoCenter;
using DevExpress.Xpf.DemoCenter.Internal;
using DevExpress.Xpf.DemoChooser.Internal;
	partial class CrossPlatformDemoCarouselPanel {
		public static readonly DependencyProperty ViewPortProperty;
		public double ViewPort {
			get { return (double)GetValue(ViewPortProperty); }
			set { SetValue(ViewPortProperty, value); }
		}
		public static readonly DependencyProperty MaxItemWidthProperty;
		public double MaxItemWidth {
			get { return (double)GetValue(MaxItemWidthProperty); }
			set { SetValue(MaxItemWidthProperty, value); }
		}
		public static readonly DependencyProperty ActualViewPortProperty;
		static readonly DependencyPropertyKey ActualViewPortPropertyKey;
		public double ActualViewPort {
			get { return (double)GetValue(ActualViewPortProperty); }
			private set { SetValue(ActualViewPortPropertyKey, value); }
		}
	}
}
namespace DevExpress.Xpf.DemoCenterBase.Helpers {
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoBase.Helpers;
	partial class ColumnScrollControl {
		public static readonly DependencyProperty CrossPlatformProperty;
		public bool CrossPlatform {
			get { return (bool)GetValue(CrossPlatformProperty); }
			set { SetValue(CrossPlatformProperty, value); }
		}
		public static readonly DependencyProperty ShowButtonsProperty;
		public bool ShowButtons {
			get { return (bool)GetValue(ShowButtonsProperty); }
			set { SetValue(ShowButtonsProperty, value); }
		}
		public static readonly DependencyProperty AllowScrollWithMouseWheelProperty;
		public bool AllowScrollWithMouseWheel {
			get { return (bool)GetValue(AllowScrollWithMouseWheelProperty); }
			set { SetValue(AllowScrollWithMouseWheelProperty, value); }
		}
		public static readonly DependencyProperty ItemsCountProperty;
		static readonly DependencyPropertyKey ItemsCountPropertyKey;
		public int ItemsCount {
			get { return (int)GetValue(ItemsCountProperty); }
			private set { SetValue(ItemsCountPropertyKey, value); }
		}
		public static readonly DependencyProperty ActualCurrentPageProperty;
		static readonly DependencyPropertyKey ActualCurrentPagePropertyKey;
		public int ActualCurrentPage {
			get { return (int)GetValue(ActualCurrentPageProperty); }
			private set { SetValue(ActualCurrentPagePropertyKey, value); }
		}
		public static readonly DependencyProperty CurrentPageProperty;
		public int CurrentPage {
			get { return (int)GetValue(CurrentPageProperty); }
			set { SetValue(CurrentPageProperty, value); }
		}
		public static readonly DependencyProperty PagesCountProperty;
		static readonly DependencyPropertyKey PagesCountPropertyKey;
		public int PagesCount {
			get { return (int)GetValue(PagesCountProperty); }
			private set { SetValue(PagesCountPropertyKey, value); }
		}
		public static readonly DependencyProperty PageButtonsProperty;
		static readonly DependencyPropertyKey PageButtonsPropertyKey;
		public ReadOnlyObservableCollection<PageItem> PageButtons {
			get { return (ReadOnlyObservableCollection<PageItem>)GetValue(PageButtonsProperty); }
			private set { SetValue(PageButtonsPropertyKey, value); }
		}
		public static readonly DependencyProperty IsFirstPageSelectedProperty;
		static readonly DependencyPropertyKey IsFirstPageSelectedPropertyKey;
		public bool IsFirstPageSelected {
			get { return (bool)GetValue(IsFirstPageSelectedProperty); }
			private set { SetValue(IsFirstPageSelectedPropertyKey, value); }
		}
		public static readonly DependencyProperty IsLastPageSelectedProperty;
		static readonly DependencyPropertyKey IsLastPageSelectedPropertyKey;
		public bool IsLastPageSelected {
			get { return (bool)GetValue(IsLastPageSelectedProperty); }
			private set { SetValue(IsLastPageSelectedPropertyKey, value); }
		}
		public static readonly DependencyProperty IsOpacityMaskVisibleProperty;
		static readonly DependencyPropertyKey IsOpacityMaskVisiblePropertyKey;
		public bool IsOpacityMaskVisible {
			get { return (bool)GetValue(IsOpacityMaskVisibleProperty); }
			private set { SetValue(IsOpacityMaskVisiblePropertyKey, value); }
		}
		public static readonly DependencyProperty IsLeftOpacityMaskVisibleProperty;
		static readonly DependencyPropertyKey IsLeftOpacityMaskVisiblePropertyKey;
		public bool IsLeftOpacityMaskVisible {
			get { return (bool)GetValue(IsLeftOpacityMaskVisibleProperty); }
			private set { SetValue(IsLeftOpacityMaskVisiblePropertyKey, value); }
		}
		public static readonly DependencyProperty ExtraSpaceProperty;
		static readonly DependencyPropertyKey ExtraSpacePropertyKey;
		public double ExtraSpace {
			get { return (double)GetValue(ExtraSpaceProperty); }
			private set { SetValue(ExtraSpacePropertyKey, value); }
		}
		public static readonly DependencyProperty ScrollViewerProperty;
		static readonly DependencyPropertyKey ScrollViewerPropertyKey;
		public ScrollViewer ScrollViewer {
			get { return (ScrollViewer)GetValue(ScrollViewerProperty); }
			private set { SetValue(ScrollViewerPropertyKey, value); }
		}
	}
}
