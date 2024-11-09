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
using System.Text;
using System.Windows;
using DevExpress.DemoData.Helpers;
using DevExpress.Internal;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public static class DemoHelper {
		const string CoSysString = "{0}/{1}.application?&";
		public static string GetSystemString(string platformLink, string clickOnceApplicationName) {
			return string.Format(CoSysString, platformLink, clickOnceApplicationName);
		}
		public static bool IsDebug() {
			var app = Application.Current;
			if(app == null) return false;
			var isDebugProperty = app.GetType().GetProperty("IsDebug");
			if(isDebugProperty == null) return false;
			return (bool)isDebugProperty.GetValue(app, null);
		}
		public static string GetPath(string path, Assembly assembly) {
			return GetDemoLanguage(assembly) == CodeLanguage.VB ? string.Empty : path;
		}
		public static string GetCodeSuffix(Assembly assembly, CodeLanguage? language = null) {
			return (language ?? GetDemoLanguage(assembly)) == CodeLanguage.VB ? ".vb" : ".cs";
		}
		internal static IEnumerable<string> GetCodeFiles(Type moduleType) {
			return CodeFileAttributeParser.GetCodeFiles(moduleType).SelectMany(CodeFileString.GetCodeFileStrings).Select(x => x.FilePath);
		}
		internal static List<CodeTextDescription> GetCodeTexts(Type moduleType) {
			return GetCodeTexts(moduleType.Assembly, CodeFileAttributeParser.GetCodeFiles(moduleType));
		}
		internal static List<CodeTextDescription> GetCodeTexts(Assembly demo, IList<string> codeFiles) {
			List<CodeTextDescription> codeTexts = new List<CodeTextDescription>();
			foreach(string codeFile in codeFiles) {
				AddCodeTextsForFile(codeTexts, demo, codeFile);
			}
			return codeTexts;
		}
		static void AddCodeTextsForFile(List<CodeTextDescription> codeTexts, Assembly demo, string codeFile) {
			foreach(CodeFileString source in CodeFileString.GetCodeFileStrings(codeFile)) {
				var codeText = LinqExtensions.Memoize<string>(() => {
					string code = DemoHelper.GetCodeText(source.Prefix == "demobase" ? typeof(DemoHelper).Assembly : demo, source.FilePath, false);
					return code == null ? null : code.Replace("\t", "    ");
				});
				codeTexts.Add(new CodeTextDescription(source.FileName, codeText));
			}
		}
		public static string GetCodeText(Assembly demo, string codeFileName, bool fullPath = false) {
			StringBuilder fileContent = new StringBuilder(GetText(GetCodeTextStream(demo, codeFileName, fullPath)));
			RemoveAttribute(fileContent, "[CodeFile", "]");
			RemoveAttribute(fileContent, "[DevExpress.Xpf.DemoBase.CodeFile", "]");
			RemoveAttribute(fileContent, "<CodeFile", "> _");
			RemoveAttribute(fileContent, "<DevExpress.Xpf.DemoBase.CodeFile", "> _");
			return fileContent.ToString();
		}
		static void RemoveAttribute(StringBuilder destinationText, string attributeBegin, string attributeEnd) {
			while(true) {
				int indexBegin = destinationText.ToString().IndexOf(attributeBegin);
				if(indexBegin < 0) break;
				int indexEnd = destinationText.ToString().IndexOf(attributeEnd, indexBegin);
				if(indexEnd < 0) break;
				indexEnd += attributeEnd.Length;
				while(true) {
					if(indexEnd >= destinationText.Length) break;
					char c = destinationText[indexEnd];
					if(c != ' ' && c != '\n' && c != '\r' && c != '\t') break;
					++indexEnd;
				}
				destinationText.Remove(indexBegin, indexEnd - indexBegin);
			}
		}
		public static Stream GetCodeTextStream(Assembly demo, string codeFileName, bool fullPath = false) {
			Stream s = GetModuleTextFromResources(demo, codeFileName, fullPath);
			if(s != null) return s;
			return GetModuleTextFromFile(demo, codeFileName, fullPath);
		}
		static string GetText(Stream s) {
			if(s == null) return string.Empty;
			using(StreamReader reader = new StreamReader(s))
				return reader.ReadToEnd();
		}
		static Stream GetModuleTextFromResources(Assembly assembly, string codeFileName, bool fullPath) {
			return AssemblyHelper.GetResourceStream(assembly, codeFileName, fullPath);
		}
		static Stream GetModuleTextFromFile(Assembly demo, string codeFileName, bool fullPath) {
			try {
				string file = GetModulePath(demo, codeFileName, fullPath);
				if(file == null) return null;
				return new FileStream(file, FileMode.Open, FileAccess.Read);
			} catch {
				return null;
			}
		}
		public static string GetModulePath(Assembly demo, string codeFileName, bool fullPath, CodeLanguage? codeLanguage = null) {
			if(EnvironmentHelper.IsClickOnce || EnvironmentHelper.IsWinStore) return null;
			var codeFolderName = GetCodeFolderName(codeLanguage, () => codeFileName.EndsWith(".vb") ? "VB" : "CS");
			return GetCodeFilePathCore(demo, codeFileName, codeFolderName, fullPath);
		}
		static string GetCodeFolderName(CodeLanguage? codeLanguage, Func<string> getDefault) {
			switch(codeLanguage) {
				case CodeLanguage.CS:
					return "CS";
				case CodeLanguage.VB:
					return "VB";
				default:
					return getDefault();
			}	
		}
		static string GetCodeFilePathCore(Assembly demo, string filename, string codeFolderName, bool fullPath) {
			try {
				string searchPath = demo.Location;
				var binIndex = searchPath.IndexOf("\\bin\\");
				if(binIndex != -1) {
					searchPath = searchPath.Substring(0, binIndex);
				}
				for(int i = 0; i < 4; i++) {
					searchPath += "\\..";
					string demoName = AssemblyHelper.GetPartialName(demo);
					if(demoName == "ReportWpfDemo")
						demoName = "ReportDemo.Wpf";
					var demoDirs = GetSafe(() => Directory.GetDirectories(searchPath, Path.Combine(codeFolderName, demoName + "*"), SearchOption.TopDirectoryOnly), () => Enumerable.Empty<string>());
					if(!demoDirs.Any())
						continue;
					var path = fullPath
						? Path.Combine(demoDirs.First(), filename.Replace('/', '\\'))
						: Directory.GetFiles(demoDirs.First(), filename, SearchOption.AllDirectories).FirstOrDefault();
					if(File.Exists(path))
						return path;
				}
				return null;
			} catch {
				return null;
			}
		}
		static T GetSafe<T>(Func<T> func, Func<T> getDefault) {
			try {
				return func();
			} catch {
				return getDefault();
			}
		}
		public static CodeLanguage GetDemoLanguage(Assembly assembly) {
			return assembly.GetReferencedAssemblies().Any(x => x.Name.StartsWith("Microsoft.VisualBasic", StringComparison.Ordinal)) ? CodeLanguage.VB : CodeLanguage.CS;
		}
	}
	class CodeFileString {
		public static IEnumerable<CodeFileString> GetCodeFileStrings(string fileName) {
			int prefixDelimiterIndex = fileName.IndexOf(':');
			string prefix;
			string path;
			if(prefixDelimiterIndex < 0) {
				prefix = string.Empty;
				path = fileName;
			} else {
				prefix = fileName.SafeRemove(prefixDelimiterIndex);
				path = fileName.SafeSubstring(prefixDelimiterIndex + 1);
			}
			var csPath = path.Replace("(cs)", "cs");
			var vbPath = csPath.Replace(".cs", ".vb");
			return csPath == vbPath
				? new [] { new CodeFileString(prefix, csPath) }
				: new [] { new CodeFileString(prefix, csPath), new CodeFileString(prefix, vbPath) }
			;
		}
		CodeFileString(string prefix, string filePath) {
			Prefix = prefix;
			FilePath = filePath;
			FileName = filePath.Split('/').LastOrDefault();
		}
		public string Prefix { get; private set; }
		public string FilePath { get; private set; }
		public string FileName { get; private set; }
	}
	public static class DemoCodeHelper {
		public static string LoadSourceCode(string codePath, Type type, CodeLanguage? language = null, bool isCodeBehind = false) {
			string codeFileName = codePath + "/" + type.Name + (isCodeBehind ? ".xaml" : string.Empty) + DemoHelper.GetCodeSuffix(type.Assembly, language);
			return DemoHelper.GetCodeText(type.Assembly, codeFileName, true);
		}
		public static string LoadXaml(string codePath, Type type) {
			return LoadCodeCore(codePath, type, ".xaml");
		}
		static string LoadCodeCore(string codePath, Type type, string extension) {
			string resourcePath =
				string.Format("{0}/{1}{2}",
					codePath,
					type.Name,
					extension);
			var stream = AssemblyHelper.GetResourceStream(type.Assembly, resourcePath, false);
			return stream != null ? new StreamReader(stream).ReadToEnd() : null;
		}
	}
}
