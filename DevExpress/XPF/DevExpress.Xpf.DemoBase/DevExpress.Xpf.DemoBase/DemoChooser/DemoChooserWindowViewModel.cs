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
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Utils;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
namespace DevExpress.Xpf.DemoChooser.Internal {
	public sealed class DemoChooserWindowViewModel : ViewModelBase {
		public readonly Platform Platform;
		public string PlatformName { get { return Platform.Name; } }
		public DemoChooserWindowViewModel(string platformName) {
			backCommand = new Lazy<ICommand>(() => new DelegateCommand(() => SwitchPage(null)));
			Platform = Repository.Platforms.Find(p => string.Equals(p.Name, platformName, StringComparison.OrdinalIgnoreCase));
			SwitchPage(null);
		}
		ViewModelBase pageViewModel;
		public ViewModelBase PageViewModel {
			get { return pageViewModel; }
			private set { SetProperty(ref pageViewModel, value, nameof(PageViewModel)); }
		}
		readonly Lazy<ICommand> backCommand;
		public ICommand BackCommand { get { return backCommand.Value; } }
		string title;
		public string Title {
			get { return title; }
			set { SetValue(ref title, value); }
		}
		bool showBackButton;
		public bool ShowBackButton {
			get { return showBackButton; }
			set { SetValue(ref showBackButton, value); }
		}
		public ImageSource Icon { get { return Platform.Icon.GetImageSource(); } }
		public string AppTitle { get { return string.Format("DevExpress {0} {1}", Platform.DisplayName, AssemblyInfo.VersionShort); } }
		public ImageSource AppIcon { get { return ImageSourceHelper.GetImageSource(AssemblyHelper.GetResourceUri(typeof(Program).Assembly, string.Format("/DemoChooser/Icons/{0}.ico", Platform.Name))); } }
		public void SwitchPage(Product product) {
			ShowBackButton = product != null;
			Title = product?.DisplayName ?? Platform.ProductListTitle;
			if(product != null)
				PageViewModel = new ProductPageViewModel(product);
			else if( Platform.Name == Repository.ReportingPlatform.Name || Platform.Name == Repository.DashboardsPlatform.Name || Platform.Name == Repository.DocsPlatform.Name)
				PageViewModel = new CrossPlatformPageViewModel(Platform);
			else if (Platform.Name == Repository.FrameworksPlatform.Name )
				PageViewModel = new FrameworkPageViewModel(Platform);
			else
				PageViewModel = new PlatformPageViewModel(Platform, SelectProduct);
		}
		void SelectProduct(Product product) {
			if(!product.IsInstalled)
				return;
			if(product.Demos.Count == 1)
				DemoRunner.TryStartDemoAndShowErrorMessage(product.Demos.First(), CallingSite.DemoChooser, false);
			else
				SwitchPage(product);
		}
	}
}
