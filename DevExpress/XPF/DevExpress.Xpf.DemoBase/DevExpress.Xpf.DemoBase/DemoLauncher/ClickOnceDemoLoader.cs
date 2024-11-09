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
using System.Collections.ObjectModel;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoCenterBase;
namespace DevExpress.Xpf.DemoLauncher {
	static class ClickOnceDemoLoader {
		internal sealed class AssemblyDownloadResult {
			public AssemblyDownloadResult(string assemblyName, string dataName, Func<Assembly> loadAssembly) {
				AssemblyName = assemblyName;
				DataName = dataName;
				LoadAssembly = loadAssembly;
			}
			public readonly string AssemblyName;
			public readonly string DataName;
			public readonly Func<Assembly> LoadAssembly;
		}
		internal sealed class AssemblyLoadResult {
			public AssemblyLoadResult(string dataName, Assembly assembly, ReadOnlyCollection<string> dependentAssemblies) {
				DataName = dataName;
				Assembly = assembly;
				DependentAssemblies = dependentAssemblies;
			}
			public readonly string DataName;
			public readonly Assembly Assembly;
			public readonly ReadOnlyCollection<string> DependentAssemblies;
		}
		static readonly Lazy<Assembly> clickOnceEntryAssembly = new Lazy<Assembly>(() => Assembly.GetEntryAssembly());
		static readonly Assembly ClickOnceEntryAssembly = clickOnceEntryAssembly.Value;
		static void GetDependentAssemblies(Assembly assembly, ICollection<string> dependentAssemblies) {
			Stream stream = AssemblyHelper.GetEmbeddedResourceStream(assembly, "clickonce.txt", false);
			string manifestString = StreamToStringWithDispose(stream);
			AppendDependentAssemblies(manifestString, dependentAssemblies);
		}
		static string StreamToStringWithDispose(Stream stream) {
			using(stream) {
				using(StreamReader reader = new StreamReader(stream)) {
					return reader.ReadToEnd();
				}
			}
		}
		static void AppendDependentAssemblies(string streamString, ICollection<string> result) {
			foreach(string item in streamString.Split('\n')) {
				if(string.IsNullOrEmpty(item)) continue;
				string trimmedItem = item.Trim();
				if(!string.IsNullOrEmpty(trimmedItem)) {
					result.Add(trimmedItem.Replace("{DXVersion}", AssemblyInfo.VersionShort));
				}
			}
		}
		static ReadOnlyCollection<string> GetDependentAssemblies(Assembly assembly) {
			var allDependentAssemblies = new List<string>();
			GetDependentAssemblies(assembly, allDependentAssemblies);
			GetDependentAssemblies(ClickOnceEntryAssembly, allDependentAssemblies);
			List<string> assembliesToLoad = new List<string>();
			foreach(string dependendAssembly in allDependentAssemblies) {
				if(LogHelper.GetLoadedAssembly(dependendAssembly) != null)
					continue;
				assembliesToLoad.Add(dependendAssembly);
			}
			return assembliesToLoad.AsReadOnly();
		}
		static TaskLinq<Either<Exception, AssemblyDownloadResult>> LoadAssemblyCore(string name) {
			var assembly = LogHelper.GetLoadedAssembly(name);
			if(assembly != null) {
				Either<Exception, AssemblyDownloadResult> loadedAssembly = new AssemblyDownloadResult(name, null, () => assembly).AsRight();
				return loadedAssembly.Promise(TaskScheduler.Default);
			}
			string downloadGroupName;
			switch(name) {
			case "DiagramDesigner": downloadGroupName = "DiagramDemo"; break;
			case "VisualStudioDocking": downloadGroupName = "DockingDemo"; break;
			default: downloadGroupName = name; break;
			}
			var appDeployment = ApplicationDeployment.CurrentDeployment;
			var result = TaskLinq
				.On<DownloadFileGroupCompletedEventArgs>(x => { DownloadFileGroupCompletedEventHandler h = (_, e) => { if(e.Group == downloadGroupName) x(e); }; appDeployment.DownloadFileGroupCompleted += h; return () => appDeployment.DownloadFileGroupCompleted -= h; }, TaskScheduler.Default)
				.Select(e => {
					var error = e.Error;
					LogFile.Current.WriteLine(string.Format("End download: {0}", downloadGroupName));
					Either<Exception, AssemblyDownloadResult> downloadedAssembly;
					if(error != null) {
						LogFile.Current.WriteLine(string.Format("Downloading the assembly {0} has failed: {1}", name, error.ToString()));
						downloadedAssembly = error.AsLeft();
					} else {
						downloadedAssembly = new AssemblyDownloadResult(name, downloadGroupName, () => LogHelper.GetAssembly(name)).AsRight();
					}
					return downloadedAssembly;
				});
			LogFile.Current.WriteLine(string.Format("Begin download: {0}", downloadGroupName));
			appDeployment.DownloadFileGroupAsync(downloadGroupName);
			return result;
		}
		static readonly Dictionary<string, TaskLinq<Either<Exception, AssemblyDownloadResult>>> loadings = new Dictionary<string, TaskLinq<Either<Exception, AssemblyDownloadResult>>>();
		internal static TaskLinq<Either<Exception, AssemblyDownloadResult>> LoadAssemblyAsync(string name) {
			lock(loadings) {
				TaskLinq<Either<Exception, AssemblyDownloadResult>> result;
				if(loadings.TryGetValue(name, out result))
					return result;
				result = TaskLinq.ThreadPool().SelectMany(() => {
					var assembly = LoadAssemblyCore(name);
					lock(loadings) {
						loadings.Remove(name);
					}
					return assembly;
				});
				loadings.Add(name, result);
				return result;
			}
		}
		public static TaskLinq<Either<Exception, Assembly>> Load(string demoName) {
			Action<double> progress;
			if(!DemoRunner.IsSplashScreenActive) {
				progress = x => DemoRunner.ShowWpfSplashScreen(false, "Downloading...", false, true)
					.Do(s => s.Progress = x * 100d);
			} else {
				progress = null;
			}
			var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			progress.Do(x => x(0.0d));
			LogFile.Current.WriteLine("Downloading primary assembly {0}", demoName);
			return LoadAssemblyAsync(demoName)
				.Schedule(TaskScheduler.Default).Linq(scheduler)
				.Select(
					resultOrError => resultOrError.SelectMany(downloadResult => {
						LogFile.Current.WriteLine("Primary assembly downloaded {0}", demoName);
						var demoAssembly = downloadResult.LoadAssembly();
						if(demoAssembly == null)
							return Either<Exception, AssemblyLoadResult>.Left(new Exception(string.Format(Errors.AssemblyIsNullErrorFormat, demoName)).AsCatched());
						var dependencies = GetDependentAssemblies(demoAssembly);
						LogFile.Current.WriteLine("Primary assembly loaded into domain {0}, dependencies: {1}", demoName, dependencies.Count);
						return Either<Exception, AssemblyLoadResult>.Right(new AssemblyLoadResult(downloadResult.DataName, demoAssembly, dependencies));
					})
				)
				.Schedule(scheduler).Linq(TaskScheduler.Default)
				.SelectMany(
					resultOrError => resultOrError.Match(e => Either<Exception, AssemblyLoadResult>.Left(e).Promise(), result => {
						var dependenciesLoaded = 0;
						return Task
							.WhenAll(result.DependentAssemblies.Select(
								dep => {
									LogFile.Current.WriteLine("Downloading dependency {0}", dep);
									return LoadAssemblyAsync(dep)
										.Select(depResult => {
											Interlocked.Increment(ref dependenciesLoaded);
											progress.Do(x => x((double)dependenciesLoaded / result.DependentAssemblies.Count));
											return depResult;
										}).Schedule(TaskScheduler.Default);
								}
							))
							.Linq(TaskScheduler.Default).Select(x => x.Sequence())
							.Schedule(TaskScheduler.Default).Linq(scheduler)
							.Select(
								depsOrError => depsOrError.SelectMany(depsToLoad => depsToLoad.Select(depToLoad =>
									depToLoad.LoadAssembly() == null
										? new Exception(string.Format(Errors.AssemblyIsNullErrorFormat, depToLoad.AssemblyName)).AsCatched().AsLeft()
										: Either<Exception, UnitT>.Right(default(UnitT))
								).Sequence().Select(_ => result))
							)
							.Schedule(scheduler).Linq(TaskScheduler.Default);
					})
				).Select(
					resultOrError => {
						try {
							return resultOrError.Select(result => {
								LogFile.Current.WriteLine("Postprocessing");
								LogFile.Current.WriteLine(string.Format("UnpackData: SharedData.zip, {0}", result.DataName + "Data.zip"));
								DevExpress.DemoData.Helpers.DataFilesHelper.UnpackData("SharedData.zip", result.DataName + "Data.zip");
								return result.Assembly;
							});
						} finally {
						}
					}
				)
			;
		}
	}
}
