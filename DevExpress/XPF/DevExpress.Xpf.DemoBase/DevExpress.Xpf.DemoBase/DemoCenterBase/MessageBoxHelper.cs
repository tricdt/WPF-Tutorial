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
using DevExpress.DemoData.Helpers;
using DevExpress.Xpf.Core;
namespace DevExpress.Xpf.DemoCenterBase {
	public interface IMessageBoxHelperSupport {
		MessageBoxHelperResult Result { get; }
		event EventHandler Close;
	}
	public static class MessageBoxHelper {
		public static MessageBoxHelperResult Show(UIElement content, InjectedBehavior injected) {
			var dialog = new ThemedWindow() { ResizeMode = ResizeMode.NoResize, WindowStyle = WindowStyle.None, ShowInTaskbar = false, UseLayoutRounding = true, Padding = new Thickness(0) };
			injected?.OnAttach(dialog);
			var owner = Application.Current?.MainWindow;
			if(owner != null && dialog != owner)
				dialog.Owner = owner;
			dialog.WindowStartupLocation = owner == null ? WindowStartupLocation.CenterScreen : WindowStartupLocation.CenterOwner;
			dialog.SizeToContent = SizeToContent.WidthAndHeight;
			dialog.Content = content;
			var mbhs = content as IMessageBoxHelperSupport;
			if(mbhs != null)
				mbhs.Close += (s, e) => dialog.Close();
			dialog.ShowDialog();
			return mbhs == null ? MessageBoxHelperResult.OK : mbhs.Result;
		}
	}
}
