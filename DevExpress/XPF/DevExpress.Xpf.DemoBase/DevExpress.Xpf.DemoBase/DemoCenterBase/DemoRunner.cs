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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.DemoData.Helpers;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase;
using DevExpress.Xpf.DemoBase.Helpers;
using Module = DevExpress.DemoData.Model.Module;
using DevExpress.DemoData.Core;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core.Native;
using System.Windows.Threading;
using System.Threading;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.DemoBase.DemoTesting;
using DevExpress.Xpf.Utils;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
using System.Runtime.CompilerServices;
using DevExpress.Internal;
using DevExpress.Xpf.DemoLauncher;
using DevExpress.Xpf.DemoChooser.Internal;
using DevExpress.Mvvm.ModuleInjection;
#if !NET
using System.Deployment.Application;
#endif
namespace DevExpress.Xpf.DemoCenterBase {
	public interface IDemoRunnerMessageBox {
		MessageBoxHelperResult Show(string message, bool isError, bool showIgnoreButton, InjectedBehavior injected);
	}
	public enum CallingSite {
		DemoCenter, DemoChooser, WpfDemo
	}
	public static class DemoRunner {
		public const string StartModuleParameter = "/start:";
		const string ParagraphMask = "<Paragraph>{0}</Paragraph>";
		static SplashScreenManager currentManager;
		static bool LockSplashScreen { get; set; }
		public static bool IsSplashScreenActive { get { return currentManager != null && (currentManager.State == SplashScreenState.Showing || currentManager.State == SplashScreenState.Shown); } }
		public static DXSplashScreenViewModel ShowApplicationSplashScreen(bool autoClose = true, Action<DXSplashScreenViewModel> initSplashScreenViewModel = null, bool allowDrag = false, bool createWaitIndicator = false, string themeName = null) {
			if(IsSplashScreenActive || LockSplashScreen) {
				var viewModel = currentManager?.ViewModel;
				if(viewModel == null)
					return null;
				initSplashScreenViewModel?.Invoke(viewModel);
				return viewModel;
			}
			var currentViewModel = new DXSplashScreenViewModel() {
				Title = "When Only the Best Will Do",
				Subtitle = "DevExpress WPF Controls",
				Status = "Initializing...",
				Copyright = GetCopyright(),
				Logo = GetImageUri("DemoLauncher/Images/Logo.svg"),
				IsIndeterminate = true
			};
			initSplashScreenViewModel?.Invoke(currentViewModel);
			string oldThemeName = null;
			if(createWaitIndicator) {
				oldThemeName = GetThemeFromMainWindow();
			}
			themeName = string.IsNullOrEmpty(themeName) ? oldThemeName : themeName;
			Func<SplashScreenManager> createManager = () => {
				if(createWaitIndicator) {
					return SplashScreenManager.Create(() => {
						return new WaitIndicatorSplashScreen(themeName) { Topmost = true, WindowStartupLocation = WindowStartupLocation.CenterOwner };
					}, currentViewModel);
				} else {
					return SplashScreenManager.Create(() => new ThemedSplashScreen(themeName) { Topmost = true, AllowDrag = allowDrag }, currentViewModel);
				}
			};
			currentManager = createManager();
			if(createWaitIndicator) {
				currentManager.Show(Application.Current?.MainWindow, trackOwnerPosition: false);
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(CloseApplicationSplashScreen));
			} else
				currentManager.ShowOnStartup(autoClose);
			return currentManager.ViewModel;
		}
		public static void Preload() {
			if(CompatibilitySettings.UseLightweightThemes)
				ApplicationThemeHelper.PreloadAsync(PreloadCategories.Core);
			else
				ApplicationThemeHelper.Preload(PreloadCategories.Core);
		}
		static DemoRunner() {
			SubscribeApplicationTheme();
		}
		internal static DXSplashScreenViewModel ShowWpfSplashScreen(bool autoClose = true, string status = "Initializing...", bool isIndeterminate = true, bool allowDrag = false,
			string themeName = null) {
			return ShowSplashScreenCore(autoClose, "DevExpress WPF Controls", status, isIndeterminate, allowDrag, themeName);
		}
		internal static DXSplashScreenViewModel ShowWaitIndicatorSplashScreen(bool autoClose = true, string status = "Initializing...", bool isIndeterminate = true, bool allowDrag = false,
			string themeName = null) {
			return ShowSplashScreenCore(autoClose, "DevExpress WPF Controls", status, isIndeterminate, allowDrag, themeName, true);
		}
		internal static DXSplashScreenViewModel ShowSplashScreenCore(bool autoClose = true, string subTitle = null, string status = "Initializing...", bool isIndeterminate = true, bool allowDrag = false, string themeName = null, bool createWaitIndicator = false) {
			return ShowApplicationSplashScreen(
				autoClose,
				vm => {
					vm.Status = status;
					vm.Subtitle = subTitle;
					vm.IsIndeterminate = isIndeterminate;
				},
				allowDrag, createWaitIndicator, themeName);
		}
		public static void CloseApplicationSplashScreen() {
			if(currentManager != null) {
				currentManager.Close();
				currentManager = null;
			}
		}
		static string GetCopyright() {
			var copyright = AssemblyInfo.AssemblyCopyright;
			if(!copyright.Contains("All rights reserved"))
				copyright = copyright + "\nAll rights reserved.";
			return copyright;
		}
		public static bool UseGdi(this Product directXProduct) {
			return directXProduct == null || !directXProduct.SupportsDirectX || DemoChooser.Internal.ProductLink.PreferGdi(directXProduct.Name);
		}
		public static void TryStartReallifeDemoAndShowErrorMessage(ReallifeDemo reallifeDemo, CallingSite from) {
#if NET
			if(reallifeDemo.Platform == Repository.WpfPlatform) {
				var maybePath = VSHelper.Runner.GetDemoFullPath(Repository.WpfPlatform.DemoLauncherPath);
				var path = string.IsNullOrEmpty(maybePath) ? Repository.WpfPlatform.DemoLauncherPath : maybePath;
				TryStartAndShowErrorMessage(reallifeDemo.Requirements, path, new[] { Path.GetFileNameWithoutExtension(reallifeDemo.LaunchPath) }, Repository.WpfPlatform, "StartReallifeDemoLauncher:_", CallingSite.DemoCenter, true);
				return;
			}
#else
			if(EnvironmentHelper.IsClickOnce) {
				string uri;
#if DEBUGTEST
				uri = "http://localhost/demos/DemoLauncher.application";
#else
				try {
					uri = ApplicationDeployment.CurrentDeployment.ActivationUri.ToString();
				} catch {
					uri = ApplicationDeployment.CurrentDeployment.UpdateLocation.ToString();
				}
#endif
				uri = uri.Split('?').First();
				uri += "?" + Path.GetFileNameWithoutExtension(reallifeDemo.LaunchPath);
				var e = LaunchApplication(uri, IntPtr.Zero, 0);
				if(e != 0)
					throw new Win32Exception(e);
				return;
			}
#endif
			TryStartAndShowErrorMessage(reallifeDemo.Requirements, reallifeDemo.LaunchPath, EmptyArray<string>.Instance, reallifeDemo.Platform, string.Format("StartReallifeDemo:{0}", reallifeDemo.Name), from, true);
		}
		public static bool BypassSealedCheck {
			get { return CompatibilitySettings.BypassSealedCheck; }
			set { CompatibilitySettings.BypassSealedCheck = value; }
		}
		public static void StartWebLinkApp(WebLink webLink) {
			try {
				var process = Process.GetProcessesByName(webLink.AssemblyName).FirstOrDefault();
				if(process != null) {
					RestoreWindow(process.MainWindowHandle);
					return;
				}
				process = StartAppByWebLink(webLink);
				if(process == null)
					DocumentPresenter.OpenLink(webLink.Url);
			} catch(Exception) {
				DocumentPresenter.OpenLink(webLink.Url);
			}
		}
		static Process StartAppByWebLink(WebLink webLink) {
			var path = Path.Combine(webLink.Path, webLink.AssemblyName + ".exe");
			var expandedPath = Environment.ExpandEnvironmentVariables(path);
			return DevExpress.Data.Utils.SafeProcess.Start(expandedPath);
		}
		static void RestoreWindow(IntPtr handle) {
			if(IsIconic(handle)) {
				ShowWindow(handle, SW_RESTORE);
			}
			SetForegroundWindow(handle);
		}
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);
		const int SW_RESTORE = 9;
		[DllImport("User32.dll")]
		private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
		[DllImport("User32.dll")]
		private static extern bool IsIconic(IntPtr handle);
		public static void TryStartDemoAndShowErrorMessage(Demo demo, CallingSite from, bool doNotUseDirectX) {
			demo.Match(
				win: x => TryStartWinDemoAndShowErrorMessage(x, from, doNotUseDirectX ? null : x.Product),
				wpf: x => TryStartWpfDemoAndShowErrorMessage(x, from, false.AsLeft()),
				wpfCustom: x => TryStartWpfDemoAndShowErrorMessage(x, from, true.AsLeft()),
				web: x => TryStartWebDemoAndShowErrorMessage(x, from),
				framework: _ => { throw new NotSupportedException("DemoRunner"); }
			);
		}
		static readonly Dictionary<Assembly, Window> openedDemos = new Dictionary<Assembly, Window>();
		[DllImport("dfshim.dll", CharSet = CharSet.Unicode)]
		static extern int LaunchApplication(string url, IntPtr data, UInt32 flags);
		static Task<UnitT> loadWpfDemoQueue = default(UnitT).Future();
		public static void LoadAndRunWpfDemo(string assemblyName, Either<bool, string> isCustomDemoOrModuleName,
			bool debug = false, string[] startupArguments = null) {
			var done = new TaskCompletionSource<UnitT>();
			var load = loadWpfDemoQueue;
			loadWpfDemoQueue = done.Task;
			load.Linq().Execute(() => LoadAndRunWpfDemoCore(done, assemblyName, isCustomDemoOrModuleName, debug, startupArguments));
		}
		static IDemoRunnerMessageBox MessageBox = new DefaultDemoRunnerMessageBox();
#if DEBUGTEST
		public static void WithMessageBoxForTests(IDemoRunnerMessageBox messageBox, Action action) {
			var saved = MessageBox;
			MessageBox = messageBox;
			try {
				action();
			} finally {
				MessageBox = saved;
			}
		}
#endif
		[Conditional("DEBUG")]
		public static void RegisterDemoPath(string assemblyName, string path) {
			WpfDemoLoader.RegisterPath(assemblyName, string.Format(path, AssemblyInfo.VersionShort));
		}
		static void LoadAndRunWpfDemoCore(TaskCompletionSource<UnitT> done, string assemblyName, Either<bool, string> isCustomDemoOrModuleName,
			bool debug, string[] startupArguments = null) {
			DemoActions.LoadDemoAssemblyAsync(assemblyName).Execute(demoAssemblyOrError => demoAssemblyOrError.Match(
				e => {
					try {
						ShowMessageBox(WrapParagraph(e.Message), RequirementCheckResultType.Error, MessageBox);
					} finally {
						done.SetResult(default(UnitT));
					}
				},
				demoAssembly => {
					ViewLocator.Default = new ViewLocator(demoAssembly);
					DevExpress.Utils.AssemblyHelper.EntryAssembly = demoAssembly;
					if(EnvironmentHelper.IsClickOnce) {
						var reallifeClickOnceDemoEntryPoint = DevExpress.Data.Internal.SafeTypeResolver.GetKnownType(demoAssembly, "DevExpress.Internal.DemoLauncher.DemoLauncherEntryPoint");
						if(reallifeClickOnceDemoEntryPoint != null) {
							var runApp = (Tuple<Action, Task<Window>>)reallifeClickOnceDemoEntryPoint.GetMethod("Run", BindingFlags.Static | BindingFlags.Public).Invoke(null, EmptyArray<object>.Instance);
							runApp.Item2.Do(windowTask => {
								windowTask
									.Linq()
									.SelectMany(window => TaskLinq
									.Wait(h => { RoutedEventHandler d = (_, __) => h(); window.Loaded += d; return () => window.Loaded -= d; }, () => window.IsLoaded)
									.Select(() => window))
									.Execute(window => {
										CloseApplicationSplashScreen();
									});
							});
							if(IsSplashScreenActive) {
								ShowWpfSplashScreen(allowDrag: true, themeName: ApplicationThemeHelper.ApplicationThemeName);
							}
							runApp.Item1();
							return;
						}
					}
					Action run = () => RunWpfDemo(done, demoAssembly, isCustomDemoOrModuleName, startupArguments);
					if(Application.Current != null) {
						run();
						return;
					}
					var app = new App(debug);
					app.Startup += (_, __) => run();
					TaskLinq.WithDefaultScheduler(() => { app.Run(); return default(UnitT).Promise(); }).Schedule().Finish();
				}
			));
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void SubscribeThemeChanging() { }
		static void SubscribeApplicationTheme() {
			if(CommonThemeHelper.UseLightweightThemes)
				LightweightThemeManager.CurrentThemeChanging += (s, e) => OnApplicationThemeChanging(e.NewValue.Name);
			else ThemeManager.ApplicationThemeChanging += (s, e) => OnApplicationThemeChanging(e.ThemeName);
			ThemePaletteHelper.PaletteApplied += OnPaletteApplied;
		}
		class App : Application {
			public App(bool debug) {
				IsDebug = debug;
			}
			public bool IsDebug { get; }
		}
		static void RunWpfDemo(TaskCompletionSource<UnitT> done, Assembly demoAssembly, Either<bool, string> isCustomDemoOrModuleName, string[] startupArguments = null) {
			var moduleName = isCustomDemoOrModuleName.Match(_ => null, x => x);
			Window openedDemo;
			if(openedDemos.TryGetValue(demoAssembly, out openedDemo)) {
				if(openedDemo.WindowState == WindowState.Minimized)
					openedDemo.WindowState = WindowState.Normal;
				openedDemo.Activate();
				if(!string.IsNullOrEmpty(moduleName)) {
					DemoBaseControl.SetStartupModuleName(openedDemo, moduleName);
					DemoBaseControl.SetStartupArguments(openedDemo, startupArguments);
				}
				done.SetResult(default(UnitT));
				return;
			}
			Mouse.OverrideCursor = Cursors.Wait;
			ViewLocator.Default = new ViewLocator(demoAssembly);
			DevExpress.Utils.AssemblyHelper.EntryAssembly = demoAssembly;
			if(isCustomDemoOrModuleName.Match(x => !x, _ => true))
				ShowWpfSplashScreen(allowDrag: true);
			else {
				LockSplashScreen = true;
				CloseApplicationSplashScreen();
			}
			var window = (Window)Activator.CreateInstance(DevExpress.Data.Internal.SafeTypeResolver.GetKnownType(demoAssembly, AssemblyHelper.GetPartialName(demoAssembly) + ".MainWindow"));
			Application.Current.Do(x => x.MainWindow = window);
			openedDemos.Add(demoAssembly, window);
			window.Closed += (_, __) => openedDemos.Remove(demoAssembly);
			if(!isCustomDemoOrModuleName.Match(x => x, _ => false)) {
				DemoBaseControl.SetStartupMethod(window, DemoBaseControlStartup.DemoChooser(() => done.SetResult(default(UnitT))));
			}
			if(!string.IsNullOrEmpty(moduleName)) {
				DemoBaseControl.SetStartupModuleName(window, moduleName);
				DemoBaseControl.SetStartupArguments(window, startupArguments);
			}
			window.ShowActivated = true;
			window.Activated += (_, __) => {
				ViewLocator.Default = new ViewLocator(demoAssembly);
				DevExpress.Utils.AssemblyHelper.EntryAssembly = demoAssembly;
				Application.Current.Do(x => x.MainWindow = window);
			};
			window.Show();
			LockSplashScreen = false;
			if(isCustomDemoOrModuleName.Match(x => x, _ => false)) {
				Mouse.OverrideCursor = null;
				done.SetResult(default(UnitT));
			}
		}
		internal static void OnPaletteApplied(object sender, EventArgs e) {
			var palette = sender as ThemePaletteBase;
			if(palette != null)
				ShowThemeChangingSplashScreen(Theme.FindTheme(ThemeManager.ActualApplicationThemeName).FullName);
		}
		internal static void OnApplicationThemeChanging(string themeName) {
			var theme = Theme.FindTheme(themeName);
			if(theme == null)
				return;
			BenchmarkHelper.DispatchShutdown();
			var oldThemeName = GetThemeFromMainWindow();
			if(SetUnloadedTheme(ThemePaletteHelper.GetBaseTheme(theme)?.Name ?? themeName, () => { }, true, oldThemeName, false))
				Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => CloseApplicationSplashScreen()));
			ShowThemeChangingSplashScreen(theme.FullName);
		}
		static void ShowThemeChangingSplashScreen(string fullName) {
			ShowApplicationSplashScreen(createWaitIndicator: true).Do(x => x.Status = $"Applying the {fullName} theme...");
		}
		static string GetThemeFromMainWindow() {
			var mainWindow = Application.Current?.MainWindow;
			return mainWindow != null ? CommonThemeHelper.GetTreeWalker(mainWindow)?.ThemeName : null;
		}
		internal static bool SetUnloadedTheme(string themeName, Action setTheme, bool waitThemeDownload = false, string oldThemeName = null, bool closeSplashScreenOnDownload = true) {
			if(!EnvironmentHelper.IsClickOnce || themeName == Theme.DeepBlueName)
				return false;
			var assembly = Xpf.Editors.Helpers.ThemeHelper.GetThemeAssembly(themeName);
			if(assembly != null)
				return false;
#if !NET
			if(!IsSplashScreenActive)
				ShowWpfSplashScreen(false, "Downloading...", true, true, oldThemeName);
			string dllName = Xpf.Editors.Helpers.ThemeHelper.GetThemeAssemblyName(themeName);
			var task = DemoLauncher.ClickOnceDemoLoader.LoadAssemblyAsync(dllName).Schedule(TaskScheduler.Default).Linq().Execute(resultOrError => {
				try {
					resultOrError.Match(
						e => new DefaultDemoRunnerMessageBox().Show(WrapParagraph(e.Message), true, false),
						result => {
							var loadedAssembly = result.LoadAssembly();
							if(loadedAssembly == null)
								new DefaultDemoRunnerMessageBox().Show(WrapParagraph(string.Format(Errors.AssemblyIsNullErrorFormat, themeName)), true, false);
							else
								setTheme();
						}
					);
				} finally {
					if(closeSplashScreenOnDownload && IsSplashScreenActive)
						CloseApplicationSplashScreen();
				}
			});
			if(waitThemeDownload) {
				while(!task.IsCompleted) {
					Dispatcher.CurrentDispatcher.Invoke(() => !task.IsCompleted);
					DispatcherHelper.UpdateLayoutAndDoEvents();
				}
				task.GetAwaiter().GetResult();
			}
#endif
			return true;
		}
		public static void TryStartDemoModuleAndShowErrorMessage(Module module, CallingSite from) {
			module.Match(
				win: x => TryStartWinDemoModuleAndShowErrorMessage(x, EmptyArray<string>.Instance, from),
				wpf: x => TryStartWpfDemoAndShowErrorMessage(x.Demo, from, x.Name.AsRight()),
				web: x => TryStartWebDemoModuleAndShowErrorMessage(x, from)
			);
		}
		public static void TryStartWpfDemoAndShowErrorMessage(BaseWpfDemo demo, CallingSite from, Either<bool, string> isCustomDemoOrModuleName) {
			var runInProcess = EnvironmentHelper.IsClickOnce || !PlatformPageViewModel.GetUseLegacyThemes(demo.Product.Platform.Name);
			if(runInProcess)
				LoadAndRunWpfDemo(demo.AssemblyName, isCustomDemoOrModuleName.IsRight() ? isCustomDemoOrModuleName : (demo is WpfCustomDemo).AsLeft());
			else {
				string[] arguments = null;
				if(isCustomDemoOrModuleName.IsRight())
				  arguments = (StartModuleParameter + isCustomDemoOrModuleName.RightOrDefault()).Yield().ToArray();
				TryStartAndShowErrorMessage(demo.Requirements, WpfDemoLoader.GetWpfDemoExePath(demo.AssemblyName), arguments, demo.Product.Platform, string.Format("StartDemo:{0}", demo.Name), from, runInProcess);
			}
		}
		public static void TryStartWinDemoAndShowErrorMessage(WinDemo demo, CallingSite from, Product directXProduct) {
			var useGdi = directXProduct.UseGdi();
			TryStartAndShowErrorMessage(demo.Requirements, demo.LaunchPath, demo.GetArguments(useGdi), demo.Product.Platform, string.Format("StartDemo:{0}", demo.Name), from, useGdi);
		}
		public static void TryStartWebDemoAndShowErrorMessage(WebDemo demo, CallingSite from) {
			TryStartAndShowErrorMessage(demo.Requirements, demo.LaunchPath, demo.Argument.YieldIfNotEmptyToArray(), demo.Product.Platform, string.Format("StartDemo:{0}", demo.Name), from, true);
		}
		public static void TryStartWebDemoModuleAndShowErrorMessage(WebModule webModule, CallingSite from) {
			var arguments = new[] { webModule.Group, webModule.Name + (webModule.Demo.Product.Platform == Repository.AspPlatform || webModule.Demo.Product.Platform == Repository.BootstrapPlatform ? ".aspx" : "") };
			TryStartAndShowErrorMessage(webModule.Demo.Requirements, webModule.Demo.LaunchPath, arguments, webModule.Demo.Product.Platform, string.Format("StartDemo:{0}", webModule.Demo.Name), from, true);
		}
		public static void TryStartWinDemoModuleAndShowErrorMessage(WinModule winModule, string[] args, CallingSite from) {
			var useGdi = winModule.Demo.Product.UseGdi();
			string[] arguments = (StartModuleParameter + winModule.Type).Yield().Concat(args).ToArray();
			TryStartAndShowErrorMessage(winModule.Demo.Requirements, winModule.Demo.LaunchPath, arguments, winModule.Demo.Product.Platform, string.Format("StartDemo:{0}", winModule.Demo.Name), from, useGdi);
		}
		public static void TryStartDemoChooserAndShowErrorMessage(Platform platform, string[] args) {
			var maybePath = VSHelper.Runner.GetDemoFullPath(platform.DemoLauncherPath);
			var path = string.IsNullOrEmpty(maybePath) ? platform.DemoLauncherPath : maybePath;
			TryStartAndShowErrorMessage(platform.Requirements, path, platform.DemoLauncherArgument.YieldIfNotEmpty().Concat(args).ToArray(), platform, "StartDemoLauncher:_", CallingSite.DemoCenter, true);
		}
		public static void TryStartFrameworkDemoAndShowErrorMessage(FrameworkDemo demo, CallingSite from) {
			TryStartAndShowErrorMessage(demo.Requirements, demo.WinPath, demo.GetArguments(demo.WinPath), demo.Product.Platform, string.Format("StartDemo:{0}", demo.Name), from, true);
		}
		public static void TryStartAndShowErrorMessage(Requirement[] requirements, string launchPath, string[] arguments, Platform platform, string doEventMessage, CallingSite from, bool useGdi) {
#if DEBUGTEST
			if(platform.Name == "Wpf") {
				var originalLaunchPath = launchPath;
				launchPath = WpfDemoLoader.GetWpfDemoExePath(Path.GetFileNameWithoutExtension(launchPath)) ?? originalLaunchPath;
			}
#endif
			var failedRequirements = TryStart(requirements, launchPath, arguments, platform, doEventMessage, from, useGdi).OrderByDescending(r => r.Type);
			foreach(var r in failedRequirements) {
				var canIgnore = ShowMessageBox(r.Message, r.Type, MessageBox);
				if(!canIgnore || r.Type == RequirementCheckResultType.Error) return;
			}
		}
		static bool ShowMessageBox(string message, RequirementCheckResultType type, IDemoRunnerMessageBox messageBox) {
			var isError = type == RequirementCheckResultType.Error;
			var result = messageBox.Show(message, isError, false, null);
			return result == MessageBoxHelperResult.Ignore || type != RequirementCheckResultType.Error;
		}
		static List<RequirementCheckResult> TryStart(Requirement[] requirements, string launchPath, string[] arguments, Platform platform, string doEventMessage, CallingSite from, bool useGdi) {
			var failedRequirements = requirements
				.Select(r => r.GetResult())
				.Where(r => r.Type != RequirementCheckResultType.Satisfied)
				.ToList();
			if(failedRequirements.Count > 0)
				return failedRequirements;
			string doEventFormattedMessage = doEventMessage + ":From" + from + (useGdi ? "" : ":DirectX");
			WebServerType serverType = platform.ServerType();
			if (platform == Repository.DocsPlatform || platform == Repository.DashboardsPlatform)
				serverType = GetRequiredServerType(requirements);
			DXSplashScreenViewModel splashScreenModel = null;
			string message = VSHelper.OpenLinkOrRunVS(launchPath, arguments, serverType,
				showSplashScreen: () => {
					splashScreenModel = ShowWaitIndicatorSplashScreen(themeName: Theme.Office2019WhiteName);
				},
				hideSplashScreen: CloseApplicationSplashScreenWithDelay ,
				updateSplashScreenStatus: x => splashScreenModel.Do(s => s.Status = x));
			if(!string.IsNullOrEmpty(message)) {
				string xamlMessage = WrapParagraph(message);
				return new List<RequirementCheckResult> { new RequirementCheckResult(xamlMessage, RequirementCheckResultType.Error) };
			}
			return new List<RequirementCheckResult>();
		}
		static void CloseApplicationSplashScreenWithDelay() {
			Task.Delay(3000).ContinueWith(x => {
				if (DemoRunner.IsSplashScreenActive)
					CloseApplicationSplashScreen();
			}, CancellationToken.None, TaskContinuationOptions.LongRunning | TaskContinuationOptions.DenyChildAttach, TaskScheduler.FromCurrentSynchronizationContext());
		}
		static WebServerType GetRequiredServerType(IEnumerable<Requirement> requirements) {
			return requirements.Any(r => Platform.NetCoreRequirementTypes.Contains(r.GetType())) ? WebServerType.AspNetCore : WebServerType.Any;
		}
		public static bool OpenSolution(Demo demo, Func<Demo, string> getSolution, string openSolutionMessage, bool showMessageBox = true, IEnumerable<string> openFiles = null) {
			return VSHelper.OpenSolution(getSolution(demo), openSolutionMessage, demo.Product.Platform, showMessageBox, openFiles.With(x => x.ToArray()));
		}
		public static bool OpenSolution(BaseReallifeDemo demo, Func<BaseReallifeDemo, string> getSolution, string openSolutionMessage, bool showMessageBox = true) {
			return VSHelper.OpenSolution(getSolution(demo), openSolutionMessage, demo.Platform, showMessageBox, null);
		}
#if DEBUGTEST
		public static bool OpenCSSolution(Demo demo, CallingSite from, bool showMessageBox = true, IEnumerable<string> openFiles = null) {
			return OpenSolution(demo, x => x.CsSolutionPath, GetOpenSolutionMessage(demo, from, false), showMessageBox, openFiles.With(x => x.FilterFilesByExtension(".vb")));
		}
		public static bool OpenVBSolution(BaseReallifeDemo demo, CallingSite from, bool showMessageBox = true) {
			return OpenSolution(demo, x => x.VbSolutionPath, GetOpenSolutionMessage(demo, from, true), showMessageBox);
		}
		public static bool OpenVBSolution(Demo demo, CallingSite from, bool showMessageBox = true, IEnumerable<string> openFiles = null) {
			return OpenSolution(demo, x => x.VbSolutionPath, GetOpenSolutionMessage(demo, from, true), showMessageBox, openFiles.With(x => x.FilterFilesByExtension(".cs")));
		}
#endif
		internal static Func<Demo, string> DemoToNetCoreFullPath(Func<Demo, string> getSolution) {
#if NET
			return getSolution;
#else
			return d => StartApplicationHelper.GetDemoFullPath(NetCorePathHelper.Convert(d, getSolution(d)));
#endif
		}
		internal static Func<BaseReallifeDemo, string> ReallifeToNetCoreFullPath(Func<BaseReallifeDemo, string> getSolution) {
#if NET
			return getSolution;
#else
			return d => StartApplicationHelper.GetDemoFullPath(NetCorePathHelper.Convert(d, getSolution(d)));
#endif
		}
		internal static IEnumerable<string> GetModulePaths(DemoModuleDescription demoDescription, CodeLanguage codeLanguage) {
			return demoDescription
					.With(module => DemoHelper.GetCodeFiles(module.ModuleType)
						.Select(x => DemoHelper.GetModulePath(module.ModuleType.Assembly, x, true, codeLanguage))
						.Where(x => !string.IsNullOrEmpty(x))
						.FilterFilesByExtension(GetSkipExtension(codeLanguage)));
		}
		internal static string FormatOpenSolution(bool isVB, bool isNetCore) {
			return "Open " + FormatSolution(isVB, isNetCore);
		}
		internal static string FormatSolution(bool isVB, bool isNetCore) {
			var language = isVB ? "VB" : "C#";
			var framework = isNetCore ? "(.NET 6+)" : "(.NET Framework)";
			return $"{language} Solution {framework}";
		}
		static string GetSkipExtension(CodeLanguage codeLanguage) {
			switch(codeLanguage) {
				case CodeLanguage.CS:
					return ".vb";
				case CodeLanguage.VB:
					return ".cs";
			}
			throw new ArgumentException($"Unsupported code language {codeLanguage}");
		}
		internal static IEnumerable<string> FilterFilesByExtension(this IEnumerable<string> files, string excludeExtension) {
			if(files == null)
				return null;
			return files.Where(f => !f.ToLower().EndsWith(excludeExtension));
		}
		public static string GetOpenSolutionMessage(Demo demo, CallingSite site, bool isVb) {
			return GetOpenSolutionMessage(demo.Product.Name, demo.Name, site, isVb);
		}
		public static string GetOpenSolutionMessage(BaseReallifeDemo demo, CallingSite site, bool isVb) {
			return GetOpenSolutionMessage("ReallifeDemo", demo.Name, site, isVb);
		}
		public static bool CheckExists(Demo demo, Func<Demo, string> solutionPathSelector) {
#if !NET
			return NetCorePathHelper.Exists(demo, solutionPathSelector);
#else
			return true;
#endif
		}
		public static bool CheckExists(BaseReallifeDemo demo, Func<BaseReallifeDemo, string> solutionPathSelector) {
#if !NET
			return NetCorePathHelper.Exists(demo, solutionPathSelector);
#else
			return true;
#endif
		}
		static string GetOpenSolutionMessage(string product, string name, CallingSite site, bool isVb) {
			string lang = isVb ? "VB" : "CS";
			var message = string.Format("OpenSolution:{0}:{1}:{2}:From{3}", lang, product, name, site);
			Logger.Log(message);
			return message;
		}
		public static ICommand CreateUrlCommand(string url, Platform platform, CallingSite callingSite) {
			return new DelegateCommand(() => TryStartAndShowErrorMessage(EmptyArray<Requirement>.Instance, url, EmptyArray<string>.Instance, platform, string.Format("OpenBuildItLink:{0}", url), callingSite, true));
		}
		static bool IsShowErrorPending { get; set; }
		static HashSet<string> DBErrorMessages = new HashSet<string>();
		public static void CollectDBException(string message) {
			lock(DBErrorMessages) {
				DBErrorMessages.Add(message);
				if(!IsShowErrorPending) {
					IsShowErrorPending = true;
					Application.Current.Dispatcher.BeginInvoke((Action)(() => ShowDBExceptions()), DispatcherPriority.Loaded);
				}
			}
		}
		public const string DBFileFailedString = "";
		public static string GetDBFileSafe(string fileName) {
			try {
				return DataDirectoryHelper.GetFile(fileName, DataDirectoryHelper.DataFolderName);
			} catch(FileNotFoundException e) {
				CollectDBException(e.Message);
				return DBFileFailedString;
			}
		}
		public static void ShowDBExceptions() {
			CloseApplicationSplashScreen();
			var message = DBErrorMessages.Aggregate("", (s, m) => s + WrapParagraph(m));
			ShowMessageBox(message, RequirementCheckResultType.Error, MessageBox);
			IsShowErrorPending = false;
			DBErrorMessages.Clear();
		}
		static Uri GetImageUri(string path) {
			return AssemblyHelper.GetResourceUri(typeof(DemoRunner).Assembly, path);
		}
		static string WrapParagraph(string message) {
			return string.Format(ParagraphMask, message);
		}
	}
}
