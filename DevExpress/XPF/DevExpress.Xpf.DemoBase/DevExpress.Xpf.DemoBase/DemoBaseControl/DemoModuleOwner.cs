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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.Docking.Base;
namespace DevExpress.Xpf.DemoBase {
	public sealed class DemoModuleOwner : BindableBase {
		public DemoModuleOwner(DemoModuleDescription moduleDescription, DemoModule module, WpfDemo demo) {
			this.module = module;
			this.feedback = FeedbackHelper.GetFeedback(moduleDescription.WpfModule.TypeName);
			Func<DemoModule, UIElement> findOptionsRoot = m => {
				var partOptions = m.FindName("PART_Options");
				if(partOptions == null) return null;
				var scrollViewer = partOptions as ScrollViewer;
				var optionsContent = (scrollViewer == null ? partOptions : scrollViewer.Content) as UIElement;
				if(optionsContent == null) return null;
				var customDemoControl = optionsContent as UserControl;
				return customDemoControl == null ? optionsContent : customDemoControl.Content as UIElement;
			};
			optionsRoot = findOptionsRoot(module) ?? new UIElement() { Visibility = Visibility.Collapsed };
			description = moduleDescription.WpfModule.Description;
			AlwaysShowFullDescription = moduleDescription.WpfModule.AlwaysShowDescription;
			shortDescription = "<Paragraph>" + moduleDescription.WpfModule.ShortDescription + "</Paragraph>";
			optionsText = moduleDescription.WpfModule.OptionsText;
			optionsCaption = moduleDescription.WpfModule.OptionsCaption;
			windowTitle = "WPF " + demo.Product.DisplayName + " - " + moduleDescription.Title;
			unloadedCodeTexts = DemoHelper.GetCodeTexts(moduleDescription.ModuleType);
			allowTouchUI = moduleDescription.WpfModule.AllowTouchUI;
			optionsExpandState = moduleDescription.WpfModule.HideOptions ? AutoHideExpandState.Hidden : AutoHideExpandState.Visible;
#if NET
			IsVisibleOpenCSSolution = false;
			IsVisibleOpenVBSolution = false;
			IsVisibleOpenCSNetCoreSolution = true;
			IsVisibleOpenVBNetCoreSolution = true;
#else
			IsVisibleOpenCSSolution = true;
			IsVisibleOpenVBSolution = true;
			IsVisibleOpenCSNetCoreSolution = NetCorePathHelper.Exists(demo, x => x.CsSolutionPath);
			IsVisibleOpenVBNetCoreSolution = NetCorePathHelper.Exists(demo, x => x.VbSolutionPath);
#endif
		}
		DemoModule module;
		public DemoModule Module { get { return module; } }
		UIElement optionsRoot;
		public UIElement OptionsRoot { get { return optionsRoot; } }
		bool showCode;
		public bool ShowCode {
			get { return showCode; }
			set {
				if(value)
					LoadCodeTexts();
				module.UpdatePopupContent(hide: value);
				showCode = value;
				RaisePropertyChanged(() => ShowCode);
			}
		}
		bool isCompletelyLoaded;
		public bool IsCompletelyLoaded {
			get { return isCompletelyLoaded; }
			set { SetProperty(ref isCompletelyLoaded, value, () => IsCompletelyLoaded); }
		}
		void LoadCodeTexts() {
			if(CodeTexts != null || CodeTextsLoading) return;
			CodeTextsLoading = true;
			unloadedCodeTexts.Select(x => TaskLinq.ThreadPool().Select(() => { x.CodeText(); })).Aggregate(default(UnitT).Promise(), (r, c) => r.SelectMany(() => c)).Schedule(TaskScheduler.Default).Linq().Execute(() => {
				CodeTexts = unloadedCodeTexts;
				CodeTextsLoading = false;
			});
		}
		readonly List<CodeTextDescription> unloadedCodeTexts;
		public bool HasCodeTexts { get { return unloadedCodeTexts.Count > 0; } }
		public bool IsVisibleOpenCSSolution { get; }
		public bool IsVisibleOpenVBSolution { get; }
		public bool IsVisibleOpenCSNetCoreSolution { get; }
		public bool IsVisibleOpenVBNetCoreSolution { get; }
		List<CodeTextDescription> codeTexts;
		public List<CodeTextDescription> CodeTexts {
			get { return codeTexts; }
			private set { SetProperty(ref codeTexts, value, () => CodeTexts); }
		}
		bool codeTextsLoading;
		public bool CodeTextsLoading {
			get { return codeTextsLoading; }
			set { SetProperty(ref codeTextsLoading, value, () => CodeTextsLoading); }
		}
		readonly string description;
		public string Description { get { return description; } }
		readonly string shortDescription;
		public string ShortDescription { get { return shortDescription; } }
		public bool AlwaysShowFullDescription { get; }
		readonly string optionsText;
		public string OptionsText { get { return optionsText; } }
		readonly string optionsCaption;
		public string OptionsCaption { get { return optionsCaption; } }
		readonly string windowTitle;
		public string WindowTitle { get { return windowTitle; } }
		readonly bool allowTouchUI;
		public bool AllowTouchUI { get { return allowTouchUI; } }
		bool? feedback;
		public bool? Feedback {
			get { return feedback; }
			private set { SetProperty(ref feedback, value, () => Feedback); }
		}
		public void PostFeedback(bool value, string message) {
			Feedback = value;
			FeedbackHelper.PostFeedbackAsync(module.GetType().FullName, value, message);
		}
		bool showMessage;
		public bool ShowMessage {
			get { return showMessage; }
			set { SetProperty(ref showMessage, value, () => ShowMessage); }
		}
		AutoHideExpandState optionsExpandState;
		public AutoHideExpandState OptionsExpandState {
			get { return optionsExpandState; }
			set { SetProperty(ref optionsExpandState, value, () => OptionsExpandState); }
		}
		bool hideFeedback = false;
		public bool HideFeedback {
			get { return hideFeedback; }
			set { SetProperty(ref hideFeedback, value, () => HideFeedback, OnHideFeedbackChanged); }
		}
		void OnHideFeedbackChanged() {
			if(HideFeedback)
				FeedbackHelper.ApplyDoNotShow();
		}
		public void CleanReferences()
		{
			optionsRoot = null;
			module = null;
		}
	}
}
