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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.DemoBase.DemoTesting;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoBase {
	public sealed class DemoBaseTesting : IDemoBaseTesting {
		readonly DemoBaseControl demoBaseControl;
		public DemoBaseTesting(DemoBaseControl demoBaseControl) {
			this.demoBaseControl = demoBaseControl;
			Modules = demoBaseControl.Modules;
			demoBaseControl
				.DependencyValue(x => x.CurrentDemoModule)
				.Select(dm => dm.With(x => (DemoModuleOwner)x.Owner))
				.Where(x => x != null)
				.SelectMany(dmo => dmo.NotifyValue(x => x.ShowCode))
				.DefaultValue(() => false)
				.Execute(x => currentDemoModuleShowCode = x);
			demoBaseControl
				.DependencyValue(x => x.CurrentDemoModule)
				.Select(dm => dm.With(x => (DemoModuleOwner)x.Owner))
				.Where(x => x != null)
				.SelectMany(dmo => dmo.NotifyValue(x => x.CodeTextsLoading))
				.DefaultValue(() => false)
				.Execute(x => codeTextsLoading = x);
			TaskLinq.Wait(d => { RoutedEventHandler h = (_, __) => d(); demoBaseControl.Loaded += h; return () => demoBaseControl.Loaded -= h; }, () => demoBaseControl.IsLoaded)
				.Execute(() => isLoaded = true);
			moduleLoaded = demoBaseControl
				.DependencyValue(x => x.CurrentDemoModule)
				.Where(x => x != null)
				.Select(m => {
					PrevModuleType = currentDemoModuleType;
					currentDemoModule = m;
					currentDemoModuleException = demoBaseControl.CurrentDemoModuleException;
					currentDemoModuleType = demoBaseControl.SelectedModule.ModuleType;
					codeViewControl = LayoutTreeHelper.GetVisualChildren(demoBaseControl).OfType<CodeViewControl>().FirstOrDefault(x => Equals(x.DataContext, currentDemoModule.Owner));
					return default(UnitT);
				})
				.Execute();
		}
		bool isLoaded;
		bool currentDemoModuleShowCode;
		bool codeTextsLoading;
		bool DemoModuleShowCode {
			get { return currentDemoModuleShowCode; }
			set { demoBaseControl.CurrentDemoModule.Do(x => ((DemoModuleOwner)x.Owner).ShowCode = value); }
		}
		public bool IsReady { get { return isLoaded && currentDemoModuleType != null && !codeTextsLoading; } }
		DemoModule currentDemoModule;
		Exception currentDemoModuleException;
		Type currentDemoModuleType;
		CodeViewControl codeViewControl;
		readonly Func<Task<UnitT>> moduleLoaded;
		public FrameworkElement ResetFocusElement { get { return this.demoBaseControl; } }
		public DemoModule CurrentDemoModule { get { return currentDemoModule; } }
		public Exception CurrentDemoModuleException { get { return currentDemoModuleException; } }
		public Type PrevModuleType { get; private set; }
		public Type CurrentDemoModuleType { get { return currentDemoModuleType; } }
		public IEnumerable<DemoModuleDescription> Modules { get; private set; }
		public int GetCurrentModuleSourcesCount() {
			return DemoHelper.GetCodeTexts(CurrentDemoModuleType.Assembly, CodeFileAttributeParser.GetCodeFiles(CurrentDemoModuleType)).Count;
		}
		public TaskLinq<UnitT> LoadModule(DemoModuleDescription module, bool reloadIfNeeded, Action<string> iAmAlive) {
			var moduleName = (module == null ? "<null>" : module.WpfModule.Name);
			iAmAlive("started DemoBaseTesting.LoadModule: " + moduleName);
			var dispatcher = Dispatcher.CurrentDispatcher;
			var result = moduleLoaded();
			if(module == null || currentDemoModuleType == module.ModuleType && reloadIfNeeded)
				demoBaseControl.ReloadModule();
			else
				demoBaseControl.LoadModule(module);
			iAmAlive("added to the queue DemoBaseTesting.LoadModule: " + moduleName);
			return result.Linq();
		}
		public void ShowCode() { DemoModuleShowCode = true; }
		public void ShowDemo() { DemoModuleShowCode = false; }
		public void ShowCodeFile(int index) {
			if(codeViewControl == null)
				throw new InvalidOperationException("ShowCodeFile");
			codeViewControl.SelectedItem = codeViewControl.ItemsSource[index];
		}
		public bool IsDemoOpen {
			get {
				return !DemoModuleShowCode && IsReady;
			}
		}
		public bool IsCodeOpen {
			get {
				return DemoModuleShowCode
					&& IsReady
					&& codeViewControl.IsTemplateApplied;
			}
		}
		public string CodeFileName {
			get {
				return codeViewControl.With(viewer => (CodeTextDescription)viewer.SelectedItem).With(x => x.FileName);
			}
		}
		public string CodeText {
			get {
				codeViewControl.ExpandAllRegions();
				return codeViewControl.GetDisplayedText();
			}
		}
		public string DemoModuleDescription {
			get {
				var descriptionContainer = LayoutTreeHelper.GetVisualChildren(CurrentDemoModule).OfType<FrameworkElement>().First(x => string.Equals(x.Name, "TEST_DemoModuleDescription", StringComparison.Ordinal));
				var richTextBox = LayoutTreeHelper.GetVisualChildren(descriptionContainer).OfType<RichTextBox>().First();
				return RichTextBoxHelper.GetText(richTextBox);
			}
		}
	}
}
