﻿#region Copyright (c) 2000-2024 Developer Express Inc.
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
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DevExpress.Mvvm;
using System.Text.RegularExpressions;
using System.IO;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using System.Threading;
using DevExpress.Xpf.DemoCenterBase;
using System.Diagnostics;
namespace DevExpress.Xpf.DemoBase.DemoTesting {
	public static class CheckDirectivesHelper {
		class ErrorInfo {
			public string Text { get; set; }
			public int Line { get; set; }
		}
		public static string CheckXAML(string xaml) {
			IList<ErrorInfo> errors = GetErrors(xaml);
			if(errors.Count == 0)
				return null;
			StringBuilder result = new StringBuilder();
			result.Append("wpf2sl directives detected: ");
			foreach(ErrorInfo info in errors) {
				result.Append(string.Format("'{0}' (line {1}), ", info.Text, info.Line));
			}
			return result.ToString();
		}
		static IList<ErrorInfo> GetErrors(string source) {
			List<ErrorInfo> result = new List<ErrorInfo>();
			string[] lines = source.Split('\n');
			for(int i = 0; i < lines.Length; i++) {
				string error = GetXamlError(lines[i]);
				if(!string.IsNullOrEmpty(error))
					result.Add(new ErrorInfo() { Line = i + 1, Text = error });
			}
			return result;
		}
		static string[] XamlDirectives = new string[] {
			"<!--BEGIN WPF-->",
			"<!--END WPF-->",
			"<!--ONLY SL",
			"ONLY SL-->",
			"<!--BEGIN REPLACE",
			"<!--END REPLACE",
			"<!--BEGIN TOSLONLY REPLACE",
			"<!--END TOSLONLY REPLACE",
			"<!--PROCESS SETTERS WITH BINDING-->",
		};
		static string GetXamlError(string line) {
			foreach(string directive in XamlDirectives) {
				if(line.Contains(directive))
					return directive;
			}
			return null;
		}
	}
	internal static class BenchmarkHelper {
		public const string Theme = "theme";
		public const string Startup = "startup";
		public const string NextModule = "module";
		internal static bool IsBenchmark { get; set; }
		internal static string[] BenchmarkArgs { get; set; }
		public static void DispatchShutdown() {
			if(!IsBenchmark)
				return;
			Dispatcher.CurrentDispatcher
				.Queue(DispatcherPriority.ApplicationIdle)
				.Execute(() => Application.Current.Shutdown());
		}
		public static void DispatchStartup(DemoBaseControl demoBaseControl) {
			if(!IsBenchmark)
				return;
			Dispatcher.CurrentDispatcher
				.Queue(DispatcherPriority.ApplicationIdle)
				.Execute(() => {
					string actionName = BenchmarkArgs[0];
					switch(actionName) {
					case Theme:
						Console.WriteLine(actionName);
						ApplicationThemeHelper.ApplicationThemeName = BenchmarkArgs[1];
						break;
					case NextModule:
						Console.WriteLine(actionName);
						demoBaseControl.LoadModule(BenchmarkArgs.Length > 1 ? demoBaseControl.Modules.FirstOrDefault(x => x.WpfModule.Name == BenchmarkArgs[1]) : demoBaseControl.Modules.Skip(1).FirstOrDefault());
						break;
					case Startup:
						Application.Current.Shutdown();
						break;
					}
				});
		}
	}
	public static class DemoTestingHelper {
		static object lastException = null;
		public static IWPFDemoTestService Service { get; private set; }
		internal static FrameworkElement RootElementInstance { get; set; }
		internal static string TestFixtureTypeName { get; set; }
		internal static int Seed { get; set; }
		internal static bool TestVB = true;
		public static void TestComplete() {
			Service.TestComplete();
		}
		internal static void IAmAlive(string currentState) {
			Service.IAmAlive(currentState);
			System.Diagnostics.Debug.WriteLine(currentState);
		}
		public static void Log(string message, params object[] parameters) {
			if (!IsLogging)
				return;
			if (Service == null)
				return;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(message);
			for(int i = 0; i < parameters.Length - 1; i++) {
				stringBuilder.Append(", ");
				stringBuilder.Append(parameters[i]);
			}
			if(parameters.Length > 0) {
				stringBuilder.Append(", ");
				stringBuilder.Append(parameters[parameters.Length - 1]);
			}
			stringBuilder.Append(" - ");
			stringBuilder.Append(DateTime.Now);
			stringBuilder.Append("\r\n");
			Logs += stringBuilder.ToString();
		}
		public static string Logs { get; set; }
		public static bool IsTesting { get; private set; }
		public static bool IsLogging { get; set; }
		static BaseDemoTestingFixture fixture;
		public static void StartTestingIfNeeded(Func<IDemoBaseTesting> demoBaseTesting, Assembly entryAssembly) {
			var app = Application.Current;
			if(app == null) return;
			StartTestingIfNeeded(demoBaseTesting, entryAssembly, app.MainWindow);
		}
		public static bool CheckIsTesting(Assembly entryAssembly) {
			if(string.IsNullOrEmpty(DemoTestingHelper.TestFixtureTypeName) || TestFixtureTypeName.StartsWith("//"))
				return false;
			try {
#if NET
				DeleteErrorLogFile();
#endif
				Type fixtureType = entryAssembly.GetTypes().FirstOrDefault(t => t.Name == TestFixtureTypeName) ?? DevExpress.Data.Internal.SafeTypeResolver.GetKnownType(entryAssembly, TestFixtureTypeName);
				fixture = (BaseDemoTestingFixture)Activator.CreateInstance(fixtureType);
				IsTesting = fixture != null;
				Dispatcher.CurrentDispatcher.UnhandledException += (sender, e) => OnUnhandledException(sender, e.Exception);
				AppDomainHelper.AddUnhandledExceptionHandler(OnUnhandledException);
			} catch(Exception ex) {
				var exceptionDetails = string.Format("Unable to load the '{0}' test fixture type:\n{1}", TestFixtureTypeName, ex.ToString());
#if NET
				AddExceptionDetailsToFile(exceptionDetails);
#endif
				MessageBox.Show(exceptionDetails);
			}
			return IsTesting;
		}
		public static void StartTestingIfNeeded(Func<IDemoBaseTesting> demoBaseTesting, Assembly entryAssembly, FrameworkElement rootElementInstance) {
			if(demoBaseTesting == null)
				return;
			if(!IsTesting)
				CheckIsTesting(entryAssembly);
			if(!IsTesting)
				return;
			RootElementInstance = rootElementInstance;
			fixture.DemoBaseTesting = demoBaseTesting();
			fixture.Start();
		}
		static void OnUnhandledException(object sender, object exception) {
			if(exception == lastException) return;
			lastException = exception;
			var exceptionDetails = "unhandled exception: " + GetExceptionDetails(exception);
#if NET
			AddExceptionDetailsToFile(exceptionDetails);
#endif
			Service.OnError(exceptionDetails);
		}
		static string GetExceptionDetails(object exceptionObject) {
			Exception exception = exceptionObject as Exception;
			if(exception == null)
				return "Unknown exception";
			StringBuilder s = new StringBuilder();
			s.AppendLine(exception.ToString());
			if(DemoTestingHelper.Logs != null)
				s.AppendLine(DemoTestingHelper.Logs);
			return s.ToString();
		}
#if NET
		static void AddExceptionDetailsToFile(string details, string filePath = "ErrorLog.txt") {
			File.AppendAllText(filePath, details);
	}
	static void DeleteErrorLogFile(string filePath = "ErrorLog.txt") {
			if(File.Exists(filePath)) {
				File.Delete(filePath);
				System.Threading.Thread.Sleep(2000);
			}
		}
#endif
		public static void ProcessArgs(string[] args) {
			bool setSeedValue = false;
			for(int i = 0, count = args.Length; i < count; i++) {
				var arg = args[i];
				if(arg.Length == 0) {
					setSeedValue = false;
					continue;
				}
				if(arg.IndexOf(DemoRunner.StartModuleParameter) == 0)
					continue;
				if(arg == nameof(CompatibilitySettings.UseLightweightThemes))
					continue;
				if(arg == "benchmark") {
					BenchmarkHelper.IsBenchmark = true;
					BenchmarkHelper.BenchmarkArgs = args.Skip(i + 1).ToArray();
					break;
				}
				if(arg == "test") {
					PrepareServerSideTesting();
					break;
				} else if(arg == "-seed") {
					setSeedValue = true;
				} else if(setSeedValue) {
					int seed = 0;
					int.TryParse(arg, out seed);
					Seed = seed;
					setSeedValue = false;
				} else {
					setSeedValue = false;
					PrepareLocalTesting(arg);
				}
			}
		}
		public static void PrepareClickOnceTesting(string serviceUri) {
			PrepareServerSideTesting(serviceUri);
		}
		public static void PrepareClickOnceTesting(Type fixtureType) {
			if(fixtureType == null)
				PrepareServerSideTesting(ServiceHelper.ServiceUri);
			else
				PrepareLocalTesting(fixtureType.FullName);
		}
		static void PrepareLocalTesting(string fixtureTypeName) {
			TestVB = false;
			SetService(new EmptyTestService(fixtureTypeName));
		}
		static void PrepareServerSideTesting(string serviceUri) {
			IWPFDemoTestService serviceCandidate = null;
			try {
				serviceCandidate = TestServiceHelper.GetTestService(serviceUri);
				SetService(TestServiceHelper.GetTestService(serviceUri));
				TestFixtureTypeName = Service.GetFixtureClassName();
			} catch { }
		}
		static void PrepareServerSideTesting() {
			PrepareServerSideTesting(ServiceHelper.ServiceUri);
		}
		static void SetService(IWPFDemoTestService serviceCandidate) {
			Service = serviceCandidate;
			TestFixtureTypeName = Service.GetFixtureClassName();
		}
	}
}
