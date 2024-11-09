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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoBase.Helpers;
using DevExpress.Xpf.DemoBase.Helpers.Internal;
using DevExpress.Xpf.DemoChooser.Internal;
namespace DevExpress.Xpf.DemoChooser {
	public partial class DemoChooserWindow : ThemedWindow {
		public DemoChooserWindow() {
			InitializeComponent();
			CommonThemeHelper.SetThemeName(this, DemoBase.DemoBaseControl.ActualDefaultTheme.Name);
			DpiAwareSizeCorrector.Attach(this);
			Activated += (_, __) => Application.Current.Do(x => x.MainWindow = this);
		}
	}
	public sealed class PageViewSelector : DataTemplateSelector {
		static readonly DataTemplate CrossPlatformPage = XamlTemplateHelper.CreateDataTemplate(typeof(CrossPlatformPage));
#if !NET
		static readonly DataTemplate ReportsFullPage = XamlTemplateHelper.CreateDataTemplate(typeof(ReportsFullPage));
#endif
		static readonly DataTemplate FrameworkPage = XamlTemplateHelper.CreateDataTemplate(typeof(FrameworkPage));
		static readonly DataTemplate ProductPage = XamlTemplateHelper.CreateDataTemplate(typeof(ProductPage));
		static readonly DataTemplate PlatformPage = XamlTemplateHelper.CreateDataTemplate(typeof(PlatformPage));
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			if(item is CrossPlatformPageViewModel) {
#if !NET
				if(((CrossPlatformPageViewModel)item).Platform == Repository.ReportingPlatform)
					return ReportsFullPage;
#endif
				if(((CrossPlatformPageViewModel)item).Platform == Repository.FrameworksPlatform)
					return FrameworkPage;
				return CrossPlatformPage;
			}
			if(item is ProductPageViewModel)
				return ProductPage;
			if(item is PlatformPageViewModel)
				return PlatformPage;
			return null;
		}
	}
}
