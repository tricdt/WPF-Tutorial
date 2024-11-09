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

using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Threading;
using DevExpress.Data.Utils;
using DevExpress.DemoData.Helpers;
using DevExpress.DemoData.Model;
using DevExpress.Images;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoBase.DemoTesting;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoLauncher;
using DevExpress.Xpf.Ribbon;
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
	[TemplatePart(Name = "PART_DemoModule1Presenter", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_DemoModule2Presenter", Type = typeof(ContentControl))]
	[TemplatePart(Name = "PART_DemoTransfer", Type = typeof(DemoTransferDecorator))]
	[TemplatePart(Name = "PART_Ribbon", Type = typeof(ILogicalChildrenContainer))]
	public partial class DemoBaseControl : Control {
		public static Theme DefaultTheme { get; set; }
		static void SetupCulture() {
			var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
			culture.NumberFormat.CurrencyDecimalDigits = 2;
			culture.NumberFormat.CurrencyDecimalSeparator = ".";
			culture.NumberFormat.CurrencyGroupSeparator = ",";
			culture.NumberFormat.CurrencyGroupSizes = new int[] { 3 };
			culture.NumberFormat.CurrencyNegativePattern = 0;
			culture.NumberFormat.CurrencyPositivePattern = 0;
			culture.NumberFormat.CurrencySymbol = "$";
			Thread.CurrentThread.CurrentCulture = culture;
		}
		public static Theme ActualDefaultTheme { get { return DefaultTheme ?? Theme.Office2019Colorful; } }
		public static void SetApplicationTheme(bool callFromProductDemos = true) {
			Theme.CachePaletteThemes = true;
			ApplicationThemeHelper.ApplicationThemeName = ActualDefaultTheme.Name;
		}
#if DEBUGTEST
		public static Type DefaultStartModule { get; set; } = null;
		public static Stopwatch StartupStopwatch;
#endif
		static DemoBaseControl() {
			ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			SetupCulture();
			Theme.RegisterPredefinedPaletteThemes();
			AssemblyResolver.Subscribe();
			ImagesAssemblyLoader.Load();
			DefaultStyleKeyProperty.OverrideMetadata(typeof(DemoBaseControl), new FrameworkPropertyMetadata(typeof(DemoBaseControl)));
			DependencyPropertyRegistrator<DemoBaseControl>.New()
				.RegisterReadOnly(nameof(IsNavigationVisible), out IsNavigationVisiblePropertyKey, out IsNavigationVisibleProperty, false)
				.RegisterReadOnly(nameof(Demo), out DemoPropertyKey, out DemoProperty, default(DemoDescription), d => (d as DemoBaseControl).Do(x => x.OnDemoChanged()))
				.RegisterReadOnly(nameof(Modules), out ModulesPropertyKey, out ModulesProperty, default(ReadOnlyObservableCollection<DemoModuleDescription>), d => (d as DemoBaseControl).Do(x => x.OnModulesChanged()))
				.RegisterAttachedReadOnly<DependencyObject, DemoBaseControl>(nameof(GetDemoBase), out DemoBasePropertyKey, out DemoBaseProperty, default(DemoBaseControl), FrameworkPropertyMetadataOptions.Inherits)
				.Register(nameof(SelectedModule), out SelectedModuleProperty, default(DemoModuleDescription))
				.Register(nameof(HideThemeSelector), out HideThemeSelectorProperty, false)
				.Register(nameof(FixtureTypeForClickOnceTesting), out FixtureTypeForClickOnceTestingProperty, default(Type))
				.RegisterReadOnly(nameof(LoadingInProgress), out LoadingInProgressPropertyKey, out LoadingInProgressProperty, false)
				.RegisterReadOnly(nameof(CurrentDemoModuleException), out CurrentDemoModuleExceptionPropertyKey, out CurrentDemoModuleExceptionProperty, default(Exception))
				.RegisterReadOnly(nameof(CurrentDemoModule), out CurrentDemoModulePropertyKey, out CurrentDemoModuleProperty, default(DemoModule))
				.RegisterReadOnly(nameof(ShowLoadingIndicator), out ShowLoadingIndicatorPropertyKey, out ShowLoadingIndicatorProperty, false)
				.Register(nameof(FullWindowMode), out FullWindowModeProperty, false)
				.RegisterAttached<DependencyObject, string>(nameof(GetStartupModuleName), out StartupModuleNameProperty, string.Empty, d => (d as DemoBaseControl).Do(x => x.SwitchToStartupModule()), FrameworkPropertyMetadataOptions.Inherits)
				.RegisterAttached<DependencyObject, string[]>(nameof(GetStartupArguments), out StartupArgumentsProperty, default(string[]), FrameworkPropertyMetadataOptions.Inherits)
				.RegisterAttached<DependencyObject, DemoBaseControlStartup>(nameof(GetStartupMethod), out StartupMethodProperty, default(DemoBaseControlStartup), FrameworkPropertyMetadataOptions.Inherits)
				.Register(nameof(SearchText), out SearchTextProperty, string.Empty, d => (d as DemoBaseControl).Do(x => x.OnSearchTextChanged()))
				.Register(nameof(ActiveDemoModule), out ActiveDemoModuleProperty, default(DemoModuleDescription))
				.Register(nameof(AllowUseNewFilteringUI), out AllowUseNewFilteringUIProperty, false)
				.Register(nameof(UseNewFilteringUI), out UseNewFilteringUIProperty, (bool?)null, d => d.ApplyGridDemoModuleProperties())
				.Register(nameof(ShowModuleInfoInTooltip), out ShowModuleInfoInTooltipProperty, false)
#if DEBUGTEST
				.Register(nameof(DebugTest), out DebugTestProperty, true)
#else
				.Register(nameof(DebugTest), out DebugTestProperty, false)
#endif
				.OverrideMetadata(FocusableProperty, false)
			;
			CommonThemeHelper.TreeWalkerOverrideProperty<DemoBaseControl>((x, newValue) => x.OnThemeChanged());
		}
		readonly Platform platform;
		public DemoBaseControl() : this(null) { }
		public DemoBaseControl(Platform platform) {
#if DEBUGTEST
			StartupStopwatch = Stopwatch.StartNew();
#endif
			SetDemoBase(this, this);
			this.platform = platform ?? Repository.WpfPlatform;
			Loaded += OnDemoBaseControlLoaded;
			Unloaded += DemoBaseControl_Unloaded;
			ClearPaletteCache = new DelegateCommand(Theme.ClearPaletteThemeCache, true);
			BenchmarkHelper.DispatchStartup(this);
		}
		public ICommand ClearPaletteCache { get; }
		void OnDemoBaseControlLoaded(object sender, RoutedEventArgs e) {
			var themeName = CommonThemeHelper.GetTreeWalkerThemeName(this);
			if(themeName == Theme.MetropolisLightName || themeName == Theme.MetropolisDarkName) {
				var border = ribbon.ByType<RibbonSelectedPageContentPresenter>("PART_SelectedPageControlContainer").
					ByType<RibbonSelectedPageControl>().ByType<DXBorder>("border");
				border.Do(x => x.BorderThickness = new Thickness(0, 1, 0, 1));
				var checkedControl = ribbon.ByType<RibbonPageCategoryControl>().ByType<RibbonPageHeaderControl>().
					ByType<RibbonPageCaptionControl>().ByType<RibbonCheckedBorderControl>();
				ZeroingBorderByLeftSide(checkedControl, "PART_NormalUnchecked");
				ZeroingBorderByLeftSide(checkedControl, "PART_NormalChecked");
				ZeroingBorderByLeftSide(checkedControl, "PART_HoverUnchecked");
				ZeroingBorderByLeftSide(checkedControl, "PART_HoverChecked");
			}
			Window.GetWindow(this).Do(x => x.KeyDown += DemoBaseControl_KeyDown);
		}
		void DemoBaseControl_Unloaded(object sender, RoutedEventArgs e) {
			var window = Window.GetWindow(this);
			if(window == null)
				return;
			window.KeyDown -= DemoBaseControl_KeyDown;
		}
		static void ZeroingBorderByLeftSide(RibbonCheckedBorderControl checkedControl, string name) {
			var border = checkedControl.ByType<Grid>(name).ByType<DXBorder>();
			if(border == null)
				return;
			var thickness = border.BorderThickness;
			border.BorderThickness = new Thickness(0, thickness.Top, thickness.Right, thickness.Bottom);
		}
		TaskLinq<UnitT> Init() {
			if(Demo != null) return default(UnitT).Promise();
			var startupMethod = GetStartupMethod(this) ?? DemoBaseControlStartup.DemoExe;
			var startupAssemblyOrName = startupMethod.DemoAssembly.Match(assemblyOrName => assemblyOrName.Match(x => Either<string, Assembly>.Left(string.IsNullOrEmpty(x) ? "GridDemo" : x), x => x.AsRight()), Window.GetWindow(this).GetType().Assembly.AsRight());
			var startupAssemblyName = startupAssemblyOrName.Match(x => x, x => AssemblyHelper.GetPartialName(x));
			Demo = new DemoDescription(platform.Products.Select(x => (WpfDemo)x.Demos[0]).First(p => p.AssemblyName == startupAssemblyName));
			ApplyGridDemoProperties();
			var modules = LoadModules(Demo.Demo, startupAssemblyOrName.Match(x => null, x => x));
			Dispatcher.CurrentDispatcher.SetAsRethrowAsyncExceptionsContext();
			var demoLoaded = startupAssemblyOrName.Match(
				name => DemoActions.LoadDemoAssemblyAsync(name).Select(startupAssemblyOrError => startupAssemblyOrError.Match(
					e => {
						new DefaultDemoRunnerMessageBox().Show($"<Paragraph>{e.Message}</Paragraph>", true, false);
						throw new AggregateException(new[] { e });
					},
					x => x
				)),
				x => x.Promise()
			);
			return demoLoaded.Select(demoAssembly => {
				try {
					modules.ForEach(x => x.ResolveModuleType(demoAssembly));
					PrepareDemoTesting(demoAssembly, !startupMethod.IsDemoExe);
					Modules = modules;
					var startupModuleName = GetActualStartupModuleName(Environment.GetCommandLineArgs().Skip(1));
					var startupModule = string.IsNullOrEmpty(startupModuleName)
						? null :
						modules.FirstOrDefault(x => string.Equals(x.WpfModule.Name, startupModuleName, StringComparison.OrdinalIgnoreCase));
#if DEBUGTEST
					if(DefaultStartModule != null)
						startupModule = modules.First(x => string.Equals(x.WpfModule.Type, DefaultStartModule.FullName, StringComparison.OrdinalIgnoreCase));
#endif
					SelectedModule = startupModule;
				} catch(Exception e) {
					LogFile.Current.WriteLine("DBC {0}", ToStringEx(e));
				} finally {
					LogFile.Current.WriteLine("DBC OK");
				}
			});
		}
		string GetActualStartupModuleName(IEnumerable<string> args) {
			string argModuleName = null;
			if(args.Any(x => {
					if(x.IndexOf(DemoRunner.StartModuleParameter) == 0) {
						argModuleName = x.Substring(DemoRunner.StartModuleParameter.Length);
						return true;
					}
					return false;
				}))
				return argModuleName;
			return GetStartupModuleName(this);
		}
		void OnThemeChanged() {
			if(moduleBorder == null)
				return;
			var themeName = CommonThemeHelper.GetTreeWalkerThemeName(this);
			if(themeName == Theme.MetropolisLightName || themeName == Theme.MetropolisDarkName) {
				moduleBorder.BorderThickness = new Thickness(0, 1, 0, 0);
				moduleBorder.BorderBrush = new SolidColorBrush(Color.FromArgb(170, 128, 128, 128));
			} else {
				moduleBorder.BorderThickness = new Thickness(0);
			}
		}
		void ApplyGridDemoProperties() {
#if DEBUGTEST
			AllowUseNewFilteringUI = Demo.Demo.AssemblyName == "GridDemo" || Demo.Demo.AssemblyName == "TreeListDemo";
#endif
		}
		void ApplyGridDemoModuleProperties() {
#if DEBUGTEST
			if(!AllowUseNewFilteringUI || !UseNewFilteringUI.HasValue) return;
			PropertyInfo dataControlProperty = null;
			if(Demo.Demo.AssemblyName == "GridDemo")
				dataControlProperty = CurrentDemoModule?.GetType().GetProperty("GridControl");
			if(Demo.Demo.AssemblyName == "TreeListDemo")
				dataControlProperty = CurrentDemoModule?.GetType().GetProperty("TreeListControl");
			var dataControl = dataControlProperty?.GetValue(CurrentDemoModule);
			var viewProperty = dataControl?.GetType().GetProperty("View");
			var view = viewProperty?.GetValue(dataControl);
			if(view == null) return;
			var filterPopupModeProperty = view.GetType().GetProperty("ColumnFilterPopupMode");
			if(filterPopupModeProperty != null) {
				var enumValues = Enum.GetValues(filterPopupModeProperty.PropertyType).OfType<object>();
				var defaultMode = enumValues.FirstOrDefault(x => x.ToString() == "Default");
				var excelSmartMode = enumValues.FirstOrDefault(x => x.ToString() == "ExcelSmart");
				filterPopupModeProperty.SetValue(view, UseNewFilteringUI.Value ? excelSmartMode : defaultMode);
			}
			var useLegacyFilterEditorProperty = view.GetType().GetProperty("UseLegacyFilterEditor");
			useLegacyFilterEditorProperty.SetValue(view, !UseNewFilteringUI.Value);
#endif
		}
		const string Tail = "===================================================";
		static string ToStringEx(Exception e) {
			string inner = e.InnerException == null ? string.Empty : ToStringEx(e.InnerException);
			string message = string.Empty;
			message += string.Format("Message:\n{0}\n{1}\n", e.Message, Tail);
			message += string.Format("Inner Exception:\n{0}\n{1}\n", inner, Tail);
			message += string.Format("Stack Trace:\n{0}{1}\n", e.StackTrace, Tail);
			return message;
		}
		static ReadOnlyObservableCollection<DemoModuleDescription> LoadModules(WpfDemo demo, Assembly mainProductAssembly) {
			var mainProductAssemblyName = mainProductAssembly.With(x => AssemblyHelper.GetPartialName(x));
			return new ReadOnlyObservableCollection<DemoModuleDescription>(new ObservableCollection<DemoModuleDescription>(
				demo.Modules.GroupBy(x => x.Group).SelectMany(x => x).Select(module => {
					bool isCurrentDemo = demo.AssemblyName == mainProductAssemblyName;
					var currentDemoAssembly = isCurrentDemo ? mainProductAssembly : null;
					var md = new DemoModuleDescription(module, demo);
					if(currentDemoAssembly != null)
						md.ResolveModuleType(currentDemoAssembly);
					return md;
				})
			));
		}
		void PrepareDemoTesting(Assembly demoAssembly, bool isDemoLauncher) {
			Logger.Log("Startup:" + (demoAssembly == null ? "NULL" : demoAssembly.GetName().Name));
			var debug = DemoHelper.IsDebug();
			if(EnvironmentHelper.IsClickOnce) {
				var fixtureTypeForClickOnceTesting = FixtureTypeForClickOnceTesting;
				if(fixtureTypeForClickOnceTesting != null || !debug) {
					if(fixtureTypeForClickOnceTesting == null && isDemoLauncher) {
						DemoTestingHelper.PrepareClickOnceTesting(ServiceHelper.SecureServiceUri);
					} else {
						DemoTestingHelper.PrepareClickOnceTesting(fixtureTypeForClickOnceTesting);
					}
				}
			} else {
				if(!isDemoLauncher)
					DemoTestingHelper.ProcessArgs(Environment.GetCommandLineArgs().Skip(1).ToArray());
#if NET
				if (GetStartupModuleName(this) == "DemoTester")
					DemoTestingHelper.ProcessArgs(GetStartupArguments(this));
#endif
			}
		}
		public virtual void LoadModule(DemoModuleDescription moduleDescription) {
			BenchmarkHelper.DispatchShutdown();
			Dispatcher.Queue(DispatcherPriority.Background).SelectMany(() => TaskLinq.WithDefaultScheduler(() => {
				SelectedModule = moduleDescription;
				if(moduleDescription != null)
					LoadSelectedModule();
				return default(UnitT).Promise();
			})).Schedule().Finish();
		}
		DemoTransferDecorator demoTransfer;
		internal RibbonControl ribbon;
		protected ContentControl CurrentDemoModulePresenter { get; private set; }
		protected ContentControl NextDemoModulePresenter { get; private set; }
		Window window;
		Border moduleBorder;
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			if(window != null)
				window.Closed -= OnWindowClosed;
			window = Window.GetWindow(this);
			if(window != null)
				window.Closed += OnWindowClosed;
			ribbon = (RibbonControl)GetTemplateChild("PART_Ribbon");
			if(ribbon != null)
				ApplyDefaultRibbonProperties(ribbon);
			CurrentDemoModulePresenter = NextDemoModulePresenter = null;
			this.demoTransfer = null;
			moduleBorder = (Border)GetTemplateChild("PART_ModuleBorder");
			var presenter1 = (ContentControl)GetTemplateChild("PART_DemoModule1Presenter");
			var presenter2 = (ContentControl)GetTemplateChild("PART_DemoModule2Presenter");
			var demoTransfer = (DemoTransferDecorator)GetTemplateChild("PART_DemoTransfer");
			if(presenter1 == null || presenter2 == null || demoTransfer == null) return;
			this.demoTransfer = demoTransfer;
			this.CurrentDemoModulePresenter = presenter1;
			this.NextDemoModulePresenter = presenter2;
			LoadModule(CurrentDemoModulePresenter);
			ShowModule(CurrentDemoModulePresenter);
			HideModule(NextDemoModulePresenter);
			UnloadModule(NextDemoModulePresenter);
			if(this.IsInDesignTool()) return;
			Init().Execute(LoadFirstModule);
			Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() => DemoRunner.CloseApplicationSplashScreen()));
		}
		void ApplyDefaultRibbonProperties(RibbonControl ribbonControl) {
			ribbonControl.SetCurrentValue(RibbonControl.StyleProperty, null);
			ribbonControl.SetCurrentValue(RibbonControl.RibbonStyleProperty, RibbonStyle.Office2019);
			ribbonControl.SetCurrentValue(RibbonControl.HideEmptyGroupsProperty, true);
			ribbonControl.SetCurrentValue(RibbonControl.AllowCustomizationProperty, false);
			ribbonControl.SetCurrentValue(RibbonControl.ToolbarShowModeProperty, RibbonQuickAccessToolbarShowMode.ShowAbove);
			ribbonControl.SetCurrentValue(RibbonControl.PageCategoryAlignmentProperty, RibbonPageCategoryCaptionAlignment.Right);
		}
		void OnWindowClosed(object sender, EventArgs e) {
			CurrentDemoModule.Do(x => {
				x.Hide();
				x.UpdatePopupContent(hide: true);
				x.UpdateOwner(null);
				x.Clear();
			});
		}
		static void ShowModule(ContentControl presenter) {
			presenter.Opacity = 1.0;
		}
		static void HideModule(ContentControl presenter) {
			presenter.Opacity = 0.0;
		}
		static void LoadModule(ContentControl presenter) {
			presenter.Visibility = Visibility.Visible;
		}
		static void UnloadModule(ContentControl presenter) {
			presenter.Visibility = Visibility.Collapsed;
		}
		protected void SetDemoModule(ContentControl presenter, Option<Either<Exception, DemoModule>> module) {
			var oldModule = presenter.Content as DemoModuleOwner;
			module.Match(e => e.Match(x => presenter.Content = ExceptionHelper.GetMessage(x), x => {
				presenter.Content = x.Owner;
			}), () => presenter.Content = null);
			oldModule.Do(x => {
				oldModule.CleanReferences();
			});
		}
		protected virtual TaskLinq<Option<Either<Exception, DemoModule>>> BeginLoadNextDemoModule(DemoModuleDescription moduleDescription, Func<DemoModule, object> owner) {
			return TaskLinq.WithDefaultScheduler(() => {
				Logger.Log(moduleDescription == null ? "NULL" : moduleDescription.ModuleType.FullName);
				LoadModule(NextDemoModulePresenter);
				var result = BeginAppearDemoModule(moduleDescription.ModuleType, !moduleDescription.WpfModule.AllowRtl, owner);
				SetDemoModule(NextDemoModulePresenter, result);
				return result.Match(d => d.Match(
					e => Either<Exception, DemoModule>.Left(e).AsOption().Promise(),
					m => TaskLinq.On(x => { RoutedEventHandler h = (_, __) => x(); m.Loaded += h; return () => m.Loaded -= h; }).Select(() => Either<Exception, DemoModule>.Right(m).AsOption())
					), () => Option<Either<Exception, DemoModule>>.Empty.Promise()
				);
			});
		}
		protected Option<Either<Exception, DemoModule>> BeginAppearDemoModule(Type demoModuleType, bool disableRtl, Func<DemoModule, object> owner) {
			if(demoModuleType == null)
				return Option<Either<Exception, DemoModule>>.Empty;
			try {
				DemoModule demoModule = CreateDemoModule(demoModuleType);
				if(disableRtl)
					demoModule.FlowDirection = FlowDirection.LeftToRight;
				demoModule.UpdatePopupContent(hide: true);
				demoModule.UpdateOwner(owner(demoModule));
				demoModule.ProcessArguments(GetStartupArguments(this));
				return Either<Exception, DemoModule>.Right(demoModule).AsOption();
			} catch(Exception e) {
				return Either<Exception, DemoModule>.Left(e).AsOption();
			}
		}
		static DemoModule CreateDemoModule(Type demoModuleType) {
			DemoModule demoModule = (DemoModule)Activator.CreateInstance(demoModuleType);
			demoModule.Prepare();
			demoModule.Width = demoModule.Height = double.NaN;
			demoModule.MaxWidth = demoModule.MaxHeight = double.PositiveInfinity;
			return demoModule;
		}
		protected void UpdateRibbon() {
			(CurrentDemoModulePresenter.Content as DemoModuleOwner).With(x => x.Module).Do(x => {
				MergingProperties.SetElementMergingBehavior(x, ElementMergingBehavior.Undefined);
				RibbonMergingHelper.SetMergeWith(x, new RibbonControl());
			});
			(NextDemoModulePresenter.Content as DemoModuleOwner).With(x => x.Module).Do(module => {
				MergingProperties.SetElementMergingBehavior(module, ElementMergingBehavior.InternalWithExternal);
				ribbon?.SetCurrentValue(RibbonControl.ShowApplicationButtonProperty, module.ShowApplicationButton());
				ribbon.Do(x => {
					var style = module.RibbonStyle;
					x.Style = style;
					if(style == null)
						ApplyDefaultRibbonProperties(x);
				});
			});
		}
		protected void ReplaceCurrentDemoModuleByNext() {
			SetDemoModule(CurrentDemoModulePresenter, Option<Either<Exception, DemoModule>>.Empty);
			CurrentDemoModule.Do(x => {
				x.UpdateOwner(null);
				x.Clear();
			});
			HideModule(CurrentDemoModulePresenter);
			UnloadModule(CurrentDemoModulePresenter);
			SwitchDemoModules();
			ShowModule(CurrentDemoModulePresenter);
		}
		void SwitchDemoModules() {
			var presenter = NextDemoModulePresenter;
			NextDemoModulePresenter = CurrentDemoModulePresenter;
			CurrentDemoModulePresenter = presenter;
		}
		protected virtual void LoadFirstModule() {
			if(ActiveDemoModule != null || LoadingInProgress) return;
			ActiveDemoModule = SelectedModule ?? Modules.FirstOrDefault();
			if(ActiveDemoModule == null)
				return;
			LoadingInProgress = true;
			SelectedModule = ActiveDemoModule;
			var loadingDemoModule = ActiveDemoModule;
			Debug.Assert(CurrentDemoModule == null, "DemoBaseControl.LoadFirstModule"); 
			BeginLoadNextDemoModule(loadingDemoModule, x => new DemoModuleOwner(loadingDemoModule, x, Demo.Demo))
				.SelectUnit(() => {
					UpdateRibbon();
					ReplaceCurrentDemoModuleByNext();
					return Dispatcher.Queue(DispatcherPriority.Background, 3);
				}).SelectMany(r => TaskLinq.WithDefaultScheduler(() => {
					AppearDemoModule(r);
					Mouse.OverrideCursor = null;
					Window.GetWindow(this).Do(x => x.Activate());
					var done = (GetStartupMethod(this) ?? DemoBaseControlStartup.DemoExe).Done;
					SetStartupMethod(this, null);
					done();
					DemoTestingHelper.StartTestingIfNeeded(() => new DemoBaseTesting(this), ActiveDemoModule.ModuleType.Assembly);
					return default(UnitT).Promise();
				})).Schedule().Finish();
		}
		protected virtual bool LoadSelectedModule(bool forceReload = false, bool loadingContinue = false) {
			if(!forceReload && SelectedModule == ActiveDemoModule) return false;
			if(!loadingContinue && LoadingInProgress) return true;
			var loadingDemoModule = SelectedModule;
			ActiveDemoModule = loadingDemoModule;
			LoadingInProgress = true;
			ShowLoadingIndicator = true;
			LoadModuleCore(loadingDemoModule);
			return true;
		}
		protected virtual void LoadModuleCore(DemoModuleDescription loadingDemoModule) {
			CurrentDemoModule.Do(x => {
				x.Hide();
				x.UpdatePopupContent(hide: true);
			});
			Dispatcher.Queue(DispatcherPriority.ContextIdle, 3)
				.SelectMany(() => {
					var t = BeginLoadNextDemoModule(loadingDemoModule, x => new DemoModuleOwner(loadingDemoModule, x, Demo.Demo));
					return Dispatcher.Timer(30).SelectMany(() => t);
				})
				.SelectUnit(() => Dispatcher.Queue(DispatcherPriority.ContextIdle, 3))
				.Select(() => UpdateRibbon())
				.SelectUnit(() => Dispatcher.Queue(DispatcherPriority.ContextIdle, 3))
				.SelectUnit(() => demoTransfer.Run(0, false))
				.Select(() => ReplaceCurrentDemoModuleByNext())
				.Select(() => { ShowLoadingIndicator = false; })
				.SelectUnit(() => Dispatcher.Timer(150))
				.SelectUnit(() => demoTransfer.Run(1, true))
				.SelectUnit(() => Dispatcher.Queue(DispatcherPriority.ContextIdle, 3))
				.SelectMany(x => TaskLinq.WithDefaultScheduler(() => { AppearDemoModule(x); return default(UnitT).Promise(); }))
				.Schedule().Finish()
			;
		}
		protected void SetLoadingInProgress(bool visible) {
			LoadingInProgress = visible;
		}
		protected void SetShowLoadingIndicator(bool visible) {
			ShowLoadingIndicator = visible;
		}
		protected void PrepareCurrentModuleToHide() {
			CurrentDemoModule.Do(x => {
				x.Hide();
				x.UpdatePopupContent(hide: true);
			});
		}
		public void ReloadModule() {
			LoadSelectedModule(forceReload: true);
		}
		void SwitchToStartupModule() {
			var moduleName = GetStartupModuleName(this);
			if(string.IsNullOrEmpty(moduleName) || ActiveDemoModule.With(x => x.WpfModule.Name) == moduleName) return;
			if(Modules == null) return;
			Modules.FirstOrDefault(x => x.WpfModule.Name == moduleName).Do(x => LoadModule(x));
		}
		protected virtual void AppearDemoModule(Option<Either<Exception, DemoModule>> result) {
			var demoModule = result.Match(m => m.Match(_ => null, x => x), () => null);
			result.DoIfHasValue(m => m.Match(_ => { }, x => { x.UpdatePopupContent(hide: false); x.Show(); ((DemoModuleOwner)x.Owner).IsCompletelyLoaded = true; }));
			if(LoadSelectedModule(loadingContinue: true)) return;
			CurrentDemoModule = demoModule;
			CurrentDemoModuleException = result.Match(m => m.Match(x => x, _ => null), () => null);
			ApplyGridDemoModuleProperties();
			LoadingInProgress = false;
			StartFeedback(demoModule);
		}
		void StartFeedback(DemoModule demoModule) {
			if(demoModule == null)
				return;
			if(!FeedbackHelper.ShowMessage(demoModule.ToString()))
				return;
			var ownerRef = new WeakReference((DemoModuleOwner)demoModule.Owner);
			DelayRun(() => {
				if(!window.IsActive)
					return;
				DemoModuleOwner moduleOwner = ownerRef.Target as DemoModuleOwner;
				if(moduleOwner != null) {
					moduleOwner.ShowMessage = true;
					DelayRun(() => {
						DemoModuleOwner moduleOwner2 = ownerRef.Target as DemoModuleOwner;
						if(moduleOwner2 != null)
							moduleOwner2.ShowMessage = false;
					}, 30);
				}
			}, 150);
		}
		void DelayRun(Action action, int seconds) {
			EventHandler handler = null;
			RoutedEventHandler unloadHandler = null;
			var timer = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher) { Interval = TimeSpan.FromSeconds(seconds) };
			unloadHandler = (s, e) => {
				timer.Stop();
				timer.Tick -= handler;
				Unloaded -= unloadHandler;
			};
			Unloaded += unloadHandler;
			handler = (s, e) => {
				action();
				timer.Stop();
				timer.Tick -= handler;
			};
			timer.Tick += handler;
			timer.Start();
		}
		public void ShowPrev() { ShowPrevNext(false); }
		public void ShowNext() { ShowPrevNext(true); }
		void ShowPrevNext(bool next) {
			var currentModuleType = SelectedModule.With(x => x.WpfModule.TypeName) ?? CurrentDemoModule.With(x => x.GetType().FullName);
			if(string.IsNullOrEmpty(currentModuleType)) return;
			var modules = next ? Modules : Modules.AsEnumerable().Reverse();
			var searchText = SearchText;
			var nextModule = modules.Concat(modules).SkipWhile(x => x.WpfModule.TypeName != currentModuleType).Skip(1).FirstOrDefault(m => string.IsNullOrEmpty(searchText) || IsDemoModuleVisible(searchText, m));
			nextModule.Do(x => LoadModule(x));
		}
		void OnSearchTextChanged() {
			Logger.LogSearch("Demo", SearchText);
		}
		public void ShowAbout() {
			if(Demo == null) return;
			AboutHelper.ShowAbout(Demo.Demo.LicenseInfo, "The WPF " + Demo.Demo.Product.DisplayName, Window.GetWindow(this));
		}
		public bool IsDemoModuleVisible(string text, object demoModuleOrGroup) {
			var demoModule = demoModuleOrGroup as DemoModuleDescription;
			return demoModule != null && (TagsHelper.Contains(demoModule.WpfModule.Tags, text) || TagsHelper.Contains(demoModule.Title, text));
		}
		public void OpenCSSolution() {
			OpenSolution(() => DemoRunner.OpenSolution(Demo.Demo,
				getSolution: x => x.CsSolutionPath,
				openSolutionMessage: DemoRunner.GetOpenSolutionMessage(Demo.Demo, CallingSite.WpfDemo, false),
				openFiles: DemoRunner.GetModulePaths(SelectedModule, CodeLanguage.CS)));
		}
		public void OpenVBSolution() {
			OpenSolution(() => DemoRunner.OpenSolution(Demo.Demo,
				getSolution: x => x.VbSolutionPath,
				openSolutionMessage: DemoRunner.GetOpenSolutionMessage(Demo.Demo, CallingSite.WpfDemo, true),
				openFiles: DemoRunner.GetModulePaths(SelectedModule, CodeLanguage.VB)));
		}
		public void OpenCSNetCoreSolution() {
			OpenSolution(() => DemoRunner.OpenSolution(Demo.Demo,
				getSolution: DemoRunner.DemoToNetCoreFullPath(x => x.CsSolutionPath),
				openSolutionMessage: DemoRunner.GetOpenSolutionMessage(Demo.Demo, CallingSite.WpfDemo, false),
				openFiles: DemoRunner.GetModulePaths(SelectedModule, CodeLanguage.CS)
					.Select(x => NetCorePathHelper.Convert(Demo.Demo, x))));
		}
		public void OpenVBNetCoreSolution() {
			OpenSolution(() => DemoRunner.OpenSolution(Demo.Demo,
				getSolution: DemoRunner.DemoToNetCoreFullPath(x => x.VbSolutionPath),
				openSolutionMessage: DemoRunner.GetOpenSolutionMessage(Demo.Demo, CallingSite.WpfDemo, true),
				openFiles: DemoRunner.GetModulePaths(SelectedModule, CodeLanguage.VB)
					.Select(x => NetCorePathHelper.Convert(Demo.Demo, x))));
		}
		void OpenSolution(Action action) {
			Dispatcher.Queue(DispatcherPriority.Background).Execute(() => action());
		}
		public void CopyLink() {
			CopyLinkInfo(false);
		}
		public void CopyHtmlLink() {
			CopyLinkInfo(true);
		}
		void CopyLinkInfo(bool html) {
			if(Demo == null || SelectedModule == null) return;
			string argumentLink = string.IsNullOrWhiteSpace(CurrentDemoModule.GetArgument()) ? string.Empty : "?" + CurrentDemoModule.GetArgument();
			string link = string.Format("dxdemo://{0}/{1}/{2}/{3}{4}", platform.Name, Demo.Demo.Product.Name, Demo.Demo.Name,
				SelectedModule.WpfModule.Name, argumentLink);
			string info;
			if(html) {
				string linkFormat = "<a href=\"{0}\">{1}</a><br>";
				string linkText = string.Format("{0} module in the {1} {2}", SelectedModule.Title, Demo.Demo.Product.DisplayName, Demo.Demo.DisplayName);
				info = string.Format(linkFormat, link, linkText);
			} else {
				info = link;
			}
			SafeClipboardWpf.Instance.SetText(info);
		}
		void OnDemoChanged() {
			if(Demo == null)
				return;
			HideThemeSelector = Demo.HideThemeSelector;
		}
		bool showAdvanced = false;
		void DemoBaseControl_KeyDown(object sender, KeyEventArgs e) {
			if(e.Key == Key.P && Keyboard.Modifiers == (ModifierKeys.Shift | ModifierKeys.Control | ModifierKeys.Alt)) {
				showAdvanced = !showAdvanced;
				if(Demo.HideThemeSelector)
					HideThemeSelector = !showAdvanced;
				DebugTest = showAdvanced;
				e.Handled = true;
			}
		}
		void OnModulesChanged() {
			IsNavigationVisible = Modules.Return(x => x.Count > 1, () => false);
		}
	}
	public partial class SplashScreeenManagerServiceEx : SplashScreenManagerService {
		static SplashScreeenManagerServiceEx() {
			DependencyPropertyRegistrator<SplashScreeenManagerServiceEx>.New()
				.Register(nameof(ShowLoadingIndicator), out ShowLoadingIndicatorProperty, false, (d, e) => d.UpdateSplashScreen())
				.Register(nameof(IsCompletelyLoaded), out IsCompletelyLoadedProperty, false, (d, e) => d.UpdateSplashScreen());
		}
		void UpdateSplashScreen() {
			if(ShowLoadingIndicator && IsCompletelyLoaded)
				((Mvvm.ISplashScreenManagerService)this).Show();
			else
				((Mvvm.ISplashScreenManagerService)this).Close();
		}
	}
	public partial class ShowCodeVisibilityBehavior : Behavior<BarCheckItem> {
		static ShowCodeVisibilityBehavior() {
			DependencyPropertyRegistrator<ShowCodeVisibilityBehavior>.New()
				.Register(nameof(IsVisible), out IsVisibleProperty, false, new Action<ShowCodeVisibilityBehavior, bool, bool>(IsVisibleChanged));
		}
		static void IsVisibleChanged(ShowCodeVisibilityBehavior behavior, bool oldValue, bool newValue) {
			behavior.AssociatedObject.IsVisible = newValue;
		}
		protected override void OnAttached() {
			base.OnAttached();
			AssociatedObject.DataContextChanged += AssociatedObject_DataContextChanged;
		}
		protected override void OnDetaching() {
			base.OnDetaching();
			AssociatedObject.DataContextChanged -= AssociatedObject_DataContextChanged;
		}
		void AssociatedObject_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if(AssociatedObject.DataContext == null)
				return;
			((DemoBaseControl)((object[])AssociatedObject.DataContext)[1]).DependencyValue(x => x.CurrentDemoModule).Execute(x => {
				AssociatedObject.IsVisible = (x == null || x.Owner == null) ? AssociatedObject.IsVisible : (x.Owner as DemoModuleOwner).HasCodeTexts;
			});
		}
	}
	namespace ThemeLayoutHelpers {
		public static class LayoutHelperExtensions {
			public static T ByType<T>(this FrameworkElement treeRoot) where T : FrameworkElement {
				return LayoutHelper.FindElementByType<T>(treeRoot) as T;
			}
			public static T ByType<T>(this FrameworkElement treeRoot, string name) where T : FrameworkElement {
				return LayoutHelper.FindElement(treeRoot, x => x.Name == name && x is T) as T;
			}
		}
	}
}
