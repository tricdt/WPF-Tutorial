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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.DemoData;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoCenterBase.Helpers {
	public class LinkInfo {
		public string Text { get; set; }
		public ICommand OpenCommand { get; set; }
		public ImageSource Preview { get; set; }
		public ImageSource PreviewHover { get; set; }
	}
	public static class DemoImageExtensions {
		public static ImageSource GetImageSource(this DemoImage self) {
			if(self.Uri.OriginalString.EndsWith(".svg")) {
				return (ImageSource)new DumbSvgImageSourceExtension { Uri = self.Uri }.ProvideValue(null);
			}
			return self.ImageSource;
		}
	}
	public class DemoScreenFooterControl : Control {
		public string Version { get; private set; }
		public bool IsRegistered { get; private set; }
		public string Copyright { get; private set; }
		public IEnumerable<LinkInfo> TextLinks { get; private set; }
		public ReadOnlyCollection<LinkInfo> ImageLinks { get; private set; }
		public bool IsCompactMode {
			get { return (bool)GetValue(IsCompactModeProperty); }
			set { SetValue(IsCompactModeProperty, value); }
		}
		public static readonly DependencyProperty IsCompactModeProperty =
			DependencyProperty.Register("IsCompactMode", typeof(bool), typeof(DemoScreenFooterControl), new PropertyMetadata(false));
		public string PlatformName {
			get { return (string)GetValue(PlatformNameProperty); }
			set { SetValue(PlatformNameProperty, value); }
		}
		public static readonly DependencyProperty PlatformNameProperty =
			DependencyProperty.Register("PlatformName", typeof(string), typeof(DemoScreenFooterControl), new PropertyMetadata(string.Empty));
		static DemoScreenFooterControl() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(DemoScreenFooterControl), new FrameworkPropertyMetadata(typeof(DemoScreenFooterControl)));
		}
		void CreateTextLinks() {
			if(!IsRegistered)
				return;
			TextLinks = new[] {
				CreateTextLinkItem("What's New", LinksHelper.WhatsNewLink),
				CreateTextLinkItem("Breaking Changes", LinksHelper.BreakingChangesLink),
				CreateTextLinkItem("Known Issues", LinksHelper.KnownIssuesLink)
			};
		}
		LinkInfo CreateTextLinkItem(string text, string link) {
			return new LinkInfo {
				Text = text,
				OpenCommand = new DelegateCommand(() => DocumentPresenter.OpenLink(link))
			};
		}
		static ImageSource CreateImageSource(string path) {
			var uri = string.Format(path, AssemblyInfo.VersionShort);
			return new DemoImage(uri).GetImageSource();
		}
		void CreateLinks() {
			var list = new List<LinkInfo>();
			list.Add(new LinkInfo {
				Text = "Support",
				OpenCommand = new DelegateCommand(() => DocumentPresenter.OpenLink(LinksHelper.GetSupport(PlatformName))),
				Preview = CreateImageSource("pack://application:,,,/DevExpress.Xpf.DemoBase.v{0};component/DemoCenterBase/Images/Footer/Support.svg"),
				PreviewHover = CreateImageSource("pack://application:,,,/DevExpress.Xpf.DemoBase.v{0};component/DemoCenterBase/Images/Footer/SupportHover.svg"),
			});
			if(!IsRegistered) {
				list.Add(new LinkInfo {
					Text = "Buy Now",
					OpenCommand = new DelegateCommand(() => DocumentPresenter.OpenLink(LinksHelper.BuyNow(PlatformName))),
					Preview = CreateImageSource("pack://application:,,,/DevExpress.Xpf.DemoBase.v{0};component/DemoCenterBase/Images/Footer/BuyNow.svg"),
					PreviewHover = CreateImageSource("pack://application:,,,/DevExpress.Xpf.DemoBase.v{0};component/DemoCenterBase/Images/Footer/BuyNowHover.svg"),
				});
			}
			ImageLinks = list.AsReadOnly();
		}
		public DemoScreenFooterControl() {
			Version = "VERSION " + AssemblyInfo.Version;
			Copyright = AssemblyInfo.AssemblyCopyright.Replace("(c)", "©");
			IsRegistered = Linker.IsRegistered;
			CreateLinks();
			CreateTextLinks();
		}
	}
}
