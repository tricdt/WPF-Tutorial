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
using System.IO;
using System.Linq;
using System.Reflection;
using DevExpress.DemoData.Helpers;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoLauncher {
	static class WpfDemoLoader {
		static Dictionary<string, string> pathToDemo = new Dictionary<string, string>();
		internal static void RegisterPath(string demoName, string path) {
			pathToDemo.Add(demoName, path);
		}
		static Either<Exception, Assembly> LoadAssemblyCore(string name) {
			try {
				var assembly = LogHelper.GetLoadedAssembly(name);
				if(assembly != null) return assembly.AsRight();
				assembly = LogHelper.LoadDXAssembly(name);
				if(assembly != null) return assembly.AsRight();
				string assemblyFilePath = GetWpfDemoExePath(name);
				if(assemblyFilePath == null) throw new FileNotFoundException();
#if NET && DEBUGTEST
				if(assembliesResolve == null) {
					assembliesResolve = new Dictionary<string, string>();
					System.Runtime.Loader.AssemblyLoadContext.Default.Resolving += (_, depName) => {
						lock(assembliesResolve) {
							string depPath;
							if(assembliesResolve.TryGetValue(DevExpress.Utils.AssemblyHelper.GetPartialName(depName.FullName), out depPath))
								return DevExpress.Data.Internal.SafeTypeResolver.GetOrLoadAssemblyFrom(depPath);
							return null;
						}
					};
				}
				lock(assembliesResolve) {
					var assemblyFileDirectory = Path.GetDirectoryName(assemblyFilePath);
					foreach(var file in Directory.GetFiles(assemblyFileDirectory, "*.dll")) {
						var assemblyName = Path.GetFileNameWithoutExtension(file);
						if(!assembliesResolve.ContainsKey(assemblyName))
							assembliesResolve.Add(assemblyName, file);
					}
				}
#endif
				return DevExpress.Data.Internal.SafeTypeResolver.GetOrLoadAssemblyFrom(assemblyFilePath).AsRight();
			} catch(Exception e) {
				return new Exception($"The '{name}' assembly was not found.", e).AsCatched().AsLeft();
			}
		}
#if NET && DEBUGTEST
		static Dictionary<string, string> assembliesResolve;
#endif
		public static TaskLinq<Either<Exception, Assembly>> Load(string name) {
			return LoadAssemblyCore(name).Promise();
		}
		static readonly string[] BinFolders = new string[] {
			"",
			"..",
			"..\\..",
			"..\\..\\..\\..\\Bin",
			"..\\..\\Reporting\\Bin",
			"..\\..\\Dashboard\\Bin",
		};
		static readonly string[] DemoExtensions = new string[] {
			".dll",
#if !NET
			".exe"
#endif
		};
		internal static string GetWpfDemoExePath(string demoName) {
			string path;
			string directory = pathToDemo.TryGetValue(demoName, out path) ? path : AppDomain.CurrentDomain.BaseDirectory;
			foreach(string binFolder in BinFolders) {
				string filePath = Path.Combine(directory, Path.Combine(binFolder, demoName)).Replace("$DEMO$", demoName);
				foreach(string ext in DemoExtensions) {
					string filePathWithExt = filePath + ext;
					if(File.Exists(filePathWithExt))
						return filePathWithExt;
				}
			}
			return null;
		}
	}
}
