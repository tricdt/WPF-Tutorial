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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using DevExpress.DemoData;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoCenterBase;
using RunNonElevated;
namespace DevExpress.Xpf.DemoCenter {
	public static class Program {
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
		public static void DemoCenterMain(string[] args) {
			if(HandleDXDemoLink(args))
				return;
			DevExpress.Data.Utils.ProcessStartPolicy.RegisterTrustedProcess("DemoBase"); 
			if(args.Length > 0 && args[0] == "desktop") {
				Start();
			} else {
				using(Mutex mutex = new Mutex(true, "DevExpressDemoCenter" + AssemblyInfo.VersionShort)) {
					if(!mutex.WaitOne(0, false)) {
						Process current = Process.GetCurrentProcess();
						foreach(Process process in Process.GetProcessesByName(current.ProcessName)) {
							if(process.Id != current.Id && process.MainWindowHandle != IntPtr.Zero) {
								SetForegroundWindow(process.MainWindowHandle);
								ShowWindowAsync(process.MainWindowHandle, 9); 
								break;
							}
						}
						return;
					}
					bool exit = true;
					if(!IsWinVistaOrHigher()) {
						exit = false;
					} else {
						try {
							UAC.RunAsDesktopUser(GetLocation(), "desktop");
						} catch(Win32Exception) {
							exit = false;
						} catch { }
					}
					if(!exit) {
						Start();
					}
				}
			}
		}
		static string GetLocation() {
			var location = System.Reflection.Assembly.GetEntryAssembly().Location;
#if NET
			return System.IO.Path.ChangeExtension(location, "exe");
#else
			return location;
#endif
		}
		sealed class DXDemoLink {
#region Variants
			abstract class LinkVariant { }
			sealed class DemoChooserLinkVariant : LinkVariant {
				public readonly Platform Platform;
				public DemoChooserLinkVariant(Platform platform) {
					Platform = platform;
				}
			}
			sealed class ReallifeDemoLinkVariant : LinkVariant {
				public readonly ReallifeDemo Demo;
				public ReallifeDemoLinkVariant(ReallifeDemo demo) {
					Demo = demo;
				}
			}
			sealed class WinDemoLinkVariant : LinkVariant {
				public readonly WinDemo Demo;
				public WinDemoLinkVariant(WinDemo winDemo) {
					Demo = winDemo;
				}
			}
			sealed class WebDemoLinkVariant : LinkVariant {
				public readonly WebDemo Demo;
				public WebDemoLinkVariant(WebDemo webDemo) {
					Demo = webDemo;
				}
			}
			sealed class WinModuleLinkVariant : LinkVariant {
				public WinModuleLinkVariant(WinModule module, string moduleTab, string codeExample, string directX) {
					Module = module;
					CodeExample = codeExample;
					DirectX = directX;
					ModuleTab = moduleTab;
				}
				public readonly WinModule Module;
				public readonly string CodeExample;
				public readonly string DirectX;
				public readonly string ModuleTab;
			}
			sealed class WpfDemoLinkVariant : LinkVariant {
				public readonly BaseWpfDemo Demo;
				public WpfDemoLinkVariant(BaseWpfDemo demo) {
					Demo = demo;
				}
			}
			sealed class WpfModuleLinkVariant : LinkVariant {
				public readonly WpfModule Module;
				public readonly string CodeExample;
				public WpfModuleLinkVariant(WpfModule module, string codeExample) {
					Module = module;
					CodeExample = codeExample;
				}
			}
			sealed class FrameworkDemoLinkVariant : LinkVariant {
				public readonly FrameworkDemo Demo;
				public FrameworkDemoLinkVariant(FrameworkDemo frameworkDemo) {
					Demo = frameworkDemo;
				}
			}
#endregion
			public static DXDemoLink DemoChooser(Platform platform) { return new DXDemoLink(new DemoChooserLinkVariant(platform)); }
			public static DXDemoLink ReallifeDemo(ReallifeDemo demo) { return new DXDemoLink(new ReallifeDemoLinkVariant(demo)); }
			public static DXDemoLink WinDemo(WinDemo demo) { return new DXDemoLink(new WinDemoLinkVariant(demo)); }
			public static DXDemoLink WebDemo(WebDemo demo) { return new DXDemoLink(new WebDemoLinkVariant(demo)); }
			public static DXDemoLink WinModule(WinModule module, string moduleTab, string codeExample, string directX) {
				return new DXDemoLink(new WinModuleLinkVariant(module, moduleTab, codeExample, directX));
			}
			public static DXDemoLink WpfDemo(BaseWpfDemo demo) { return new DXDemoLink(new WpfDemoLinkVariant(demo)); }
			public static DXDemoLink WpfModule(WpfModule module, string codeExample) {
				return new DXDemoLink(new WpfModuleLinkVariant(module, codeExample));
			}
			public static DXDemoLink FrameworkDemo(FrameworkDemo demo) { return new DXDemoLink(new FrameworkDemoLinkVariant(demo)); }
			DXDemoLink(LinkVariant variant) {
				this.variant = variant;
			}
			readonly LinkVariant variant;
			public T Match<T>(Func<Platform, T> demoChooser, Func<ReallifeDemo, T> reallifeDemo, Func<WinDemo, T> winDemo, Func<WinModule, string, string, T> winModule, Func<WebDemo, T> webDemo, Func<BaseWpfDemo, T> wpfDemo, Func<WpfModule, string, T> wpfModule, Func<FrameworkDemo, T> frameworkDemo) {
				var asDemoChooser = variant as DemoChooserLinkVariant;
				if(asDemoChooser != null) return demoChooser(asDemoChooser.Platform);
				var asReallifeDemo = variant as ReallifeDemoLinkVariant;
				if(asReallifeDemo != null) return reallifeDemo(asReallifeDemo.Demo);
				var asWinDemo = variant as WinDemoLinkVariant;
				if(asWinDemo != null) return winDemo(asWinDemo.Demo);
				var asWinModule = variant as WinModuleLinkVariant;
				if(asWinModule != null) return winModule(asWinModule.Module, asWinModule.CodeExample, asWinModule.DirectX);
				var asWebDemo = variant as WebDemoLinkVariant;
				if(asWebDemo != null) return webDemo(asWebDemo.Demo);
				var asWpfDemo = variant as WpfDemoLinkVariant;
				if(asWpfDemo != null) return wpfDemo(asWpfDemo.Demo);
				var asWpfModule = variant as WpfModuleLinkVariant;
				if(asWpfModule != null) return wpfModule(asWpfModule.Module, asWpfModule.CodeExample);
				var asFrameworkDemo = variant as FrameworkDemoLinkVariant;
				if(asFrameworkDemo != null) return frameworkDemo(asFrameworkDemo.Demo);
				throw new InvalidOperationException();
			}
			public void Match(Action<Platform> demoChooser, Action<ReallifeDemo> reallifeDemo, Action<WinDemo> winDemo, Action<WinModule, string, string, string> winModule, Action<WebDemo> webDemo, Action<BaseWpfDemo> wpfDemo, Action<WpfModule, string> wpfModule, Action<FrameworkDemo> frameworkDemo) {
				var asDemoChooser = variant as DemoChooserLinkVariant;
				if(asDemoChooser != null) {
					demoChooser(asDemoChooser.Platform);
					return;
				}
				var asReallifeDemo = variant as ReallifeDemoLinkVariant;
				if(asReallifeDemo != null) {
					reallifeDemo(asReallifeDemo.Demo);
					return;
				}
				var asWinDemo = variant as WinDemoLinkVariant;
				if(asWinDemo != null) {
					winDemo(asWinDemo.Demo);
					return;
				}
				var asWinModule = variant as WinModuleLinkVariant;
				if(asWinModule != null) {
					winModule(asWinModule.Module, asWinModule.ModuleTab, asWinModule.CodeExample, asWinModule.DirectX);
					return;
				}
				var asWebDemo = variant as WebDemoLinkVariant;
				if(asWebDemo != null) {
					webDemo(asWebDemo.Demo);
					return;
				}
				var asWpfDemo = variant as WpfDemoLinkVariant;
				if(asWpfDemo != null) {
					wpfDemo(asWpfDemo.Demo);
					return;
				}
				var asWpfModule = variant as WpfModuleLinkVariant;
				if(asWpfModule != null) {
					wpfModule(asWpfModule.Module, asWpfModule.CodeExample);
					return;
				}
				var asFrameworkDemo = variant as FrameworkDemoLinkVariant;
				if(asFrameworkDemo != null) {
					frameworkDemo(asFrameworkDemo.Demo);
					return;
				}
				throw new InvalidOperationException();
			}
		}
		static string[] GetDXDemoLinkParams(string path) {
			const string prefix = "dxdemo://";
			if(path.Length <= prefix.Length)
				return null;
			var parametersSplit = path.Split('?');
			var runPath = parametersSplit[0];
			const int splitCount = 6;
			return runPath.Substring(prefix.Length).Split(new[] { '/' }, splitCount);
		}
		static DXDemoLink ParseDXDemoLink(string path) {
			var parametersSplit = path.Split('?');
			var split = GetDXDemoLinkParams(path);
			foreach(var platform in Repository.Platforms.Where(p => string.Equals(p.Name, split[0], StringComparison.InvariantCultureIgnoreCase))) {
				if(split.Length == 1) return DXDemoLink.DemoChooser(platform);
				foreach(var product in platform.Products.Where(p => string.Equals(p.Name, split[1], StringComparison.InvariantCultureIgnoreCase))) {
					if(split.Length == 2) return product.Demos.First().Match(x => DXDemoLink.WinDemo(x), x => DXDemoLink.WpfDemo(x), x => DXDemoLink.WpfDemo(x), _ => null, x => DXDemoLink.FrameworkDemo(x));
					foreach(var demo in product.Demos.Where(d => string.Equals(d.Name, split[2], StringComparison.InvariantCultureIgnoreCase))) {
						if(split.Length == 3) {
							var r = demo.Match(x => DXDemoLink.WinDemo(x), x => DXDemoLink.WpfDemo(x), x => DXDemoLink.WpfDemo(x), _ => null, x => DXDemoLink.FrameworkDemo(x));
							if(r != null) return r;
						} else if(split.Length > 3 && (platform.Name == "Asp" || platform.Name == "Mvc" || product.Name == "DocumentServerForAspMVC" || product.Name == "DocumentServerForAsp")) {
							var r = demo.Match(x => DXDemoLink.WinDemo(x), x => DXDemoLink.WpfDemo(x), x => DXDemoLink.WpfDemo(x), x => DXDemoLink.WebDemo(x), x => DXDemoLink.FrameworkDemo(x));
							if(r != null) return r;
						}
						foreach(var module in demo.Match(x => x.Modules, x => x.Modules, _ => EmptyArray<Module>.Instance, _ => EmptyArray<Module>.Instance, _ => Enumerable.Empty<Module>()).Where(m => string.Equals(m.Name, split[3], StringComparison.InvariantCultureIgnoreCase))) {
							var r = module.Match(
								x => DXDemoLink.WinModule(x,FindModuleTab(split), FindCodeExampleArg(split), FindDirectXArg(split)),
								x => DXDemoLink.WpfModule(x, parametersSplit.ElementAtOrDefault(1)),
								_ => null
							);
							if(r != null) return r;
						}
					}
				}
				var rlDemo = platform.ReallifeDemos.FirstOrDefault(p => string.Equals(p.Name, split[1], StringComparison.InvariantCultureIgnoreCase)).With(demo => demo.Match(x => x, _ => null, _ => null));
				if(rlDemo != null) return DXDemoLink.ReallifeDemo(rlDemo);
			}
			return null;
		}
		static string FindModuleTab(string[] split) {
			if(split.Length > 4 && !string.Equals(split[4], "directx", StringComparison.OrdinalIgnoreCase)) {
				if(split[4].Contains("tab_")) return split[4].Split('_')[1];
			}
			return null;
		}
		static string FindCodeExampleArg(string[] split) {
			if(split.Length > 4 && !string.Equals(split[4], "directx", StringComparison.OrdinalIgnoreCase) && !split[4].Contains("tab_")) {
				return split[4];
			}
			return null;
		}
		static string FindDirectXArg(string[] split) {
			return split.IndexOf(v => v.Equals("directx", StringComparison.OrdinalIgnoreCase)) > -1? "directx" : null;
		}
		static bool HandleDXDemoLink(string[] args) {
			var dxdemoLink = Environment.GetCommandLineArgs().FirstOrDefault(a => a.StartsWith("dxdemo://"));
			if(dxdemoLink == null)
				return false;
			var parseResult = ParseDXDemoLink(dxdemoLink);
			if(parseResult == null) return false;
			var platform = parseResult.Platform();
			if(!platform.IsInstalled) return false;
			parseResult.Match(
				demoChooser: demoChooser => {
					DemoRunner.TryStartDemoChooserAndShowErrorMessage(demoChooser, EmptyArray<string>.Instance);
				},
				reallifeDemo: reallifeDemo => {
					DemoRunner.TryStartReallifeDemoAndShowErrorMessage(reallifeDemo, CallingSite.DemoCenter);
				},
				winDemo: winDemo => {
					DemoRunner.TryStartWinDemoAndShowErrorMessage(winDemo, CallingSite.DemoCenter, null);
				},
				winModule: (winModule, moduleTab, codeExample, directX) => {
					Func<string, string, string, string[]> additionalParameters = (t, e, x) => {
						var moduleTabArg = string.IsNullOrEmpty(t) ? null : string.Format("/tab:{0}", t);
						var exampleArg = string.IsNullOrEmpty(e) ? null : string.Format("/codeexample:{0}", e);
						var directXArg = string.IsNullOrEmpty(x) ? null : string.Format("/{0}", x);
						return moduleTabArg.YieldIfNotEmptyToArray().Concat(exampleArg.YieldIfNotNull().Concat(directXArg.YieldIfNotNull())).ToArray();
					};
					DemoRunner.TryStartWinDemoModuleAndShowErrorMessage(winModule, additionalParameters(moduleTab, codeExample, directX), CallingSite.DemoCenter);
				},
				webDemo: webDemo => {
					string[] linkParams = GetDXDemoLinkParams(dxdemoLink).Skip(3).ToArray();
					string language = linkParams[linkParams.Length - 1].ToLower();
					string currentSolutionPath = language == "vb" ? webDemo.VbSolutionPath : webDemo.CsSolutionPath;
					string[] solutionSubFolders = currentSolutionPath.Split(new string[] { "\\" }, StringSplitOptions.None);
					string solutionRootFolder = string.Join("\\", solutionSubFolders.Take(solutionSubFolders.Length - 1));
					if(webDemo.Product.Platform.Name.ToLower().Contains("mvc") || webDemo.Product.Name.ToLower().Contains("mvc")) {
						string demoPagePath = solutionRootFolder + "\\" + string.Join("\\", GetDXDemoLinkParams(dxdemoLink).Skip(3));
						DemoRunner.OpenSolution(webDemo, x => x.CsSolutionPath, "", false, new string[] { demoPagePath + ".cshtml", demoPagePath + "Partial.cshtml", demoPagePath + "SpellCheckerPartial.cshtml" });
					}
					else if(webDemo.Product.Platform.Name.ToLower().Contains("asp") || webDemo.Product.Name.ToLower().Contains("asp")) {
						string demoPagePath = solutionRootFolder + "\\" + string.Join("\\", linkParams.Take(linkParams.Length - 1));
						if(language == "cs")
							DemoRunner.OpenSolution(webDemo, x => x.CsSolutionPath, "", false, new string[] { demoPagePath + ".aspx.cs", demoPagePath + ".aspx" });
						else
							DemoRunner.OpenSolution(webDemo, x => x.VbSolutionPath, "", false, new string[] { demoPagePath + ".aspx.vb", demoPagePath + ".aspx" });
					}
				},
				wpfDemo: wpfDemo => {
					DemoRunner.TryStartDemoChooserAndShowErrorMessage(Repository.WpfPlatform, new[] { wpfDemo.AssemblyName });
				},
				wpfModule: (wpfModule, codeExample) => {
					DemoRunner.TryStartDemoChooserAndShowErrorMessage(Repository.WpfPlatform, new[] { wpfModule.Demo.AssemblyName, wpfModule.Name, codeExample });
				},
				frameworkDemo: frameworkDemo => {
					DemoRunner.TryStartFrameworkDemoAndShowErrorMessage(frameworkDemo, CallingSite.DemoCenter);
				}
			);
			return true;
		}
		static Platform Platform(this DXDemoLink link) {
			return link.Match(x => x, x => x.Platform, x => x.Product.Platform, (x, _, __) => x.Demo.Product.Platform, x => x.Product.Platform, _ => Repository.WpfPlatform, (_, __) => Repository.WpfPlatform, x => x.Product.Platform);
		}
		static bool IsWinVistaOrHigher() {
			OperatingSystem OS = Environment.OSVersion;
			return (OS.Platform == PlatformID.Win32NT) && (OS.Version.Major >= 6);
		}
		static void Start() {
			ApplicationThemeHelper.ApplicationThemeName = DemoBase.DemoBaseControl.ActualDefaultTheme.Name;
			var application = new Application();
			application.Run(new DemoCenterWindow());
		}
	}
}
