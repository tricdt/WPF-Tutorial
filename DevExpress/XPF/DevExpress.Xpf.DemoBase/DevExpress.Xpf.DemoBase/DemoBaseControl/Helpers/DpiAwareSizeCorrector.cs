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
using System.Windows;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core.Native;
namespace DevExpress.Xpf.DemoBase.Helpers.Internal {
	public static class DpiAwareSizeCorrector {
		static readonly PrimaryScreen screen = new PrimaryScreen();
		public static void Attach(Window window) {
			bool maximizeWindow;
			var originSize = GetSize(out maximizeWindow, window.Width, window.Height);
			RoutedEventHandler loadedHandler = null;
			loadedHandler = (s, __) => {
				Window sender = s as Window;
				if(sender == null)
					return;
				sender.Loaded -= loadedHandler;
				if(originSize.Width.IsNaN() || originSize.Height.IsNaN())
					originSize = GetSize(out maximizeWindow, sender.Width, sender.Height);
				screen.WorkingAreaChanged += () => UpdateSize(sender, sender.MinWidth, sender.MinHeight, originSize);
				UpdateSize(sender, sender.MinWidth, sender.MinHeight, originSize);
				if(maximizeWindow && sender.ResizeMode != ResizeMode.NoResize && sender.ResizeMode != ResizeMode.CanMinimize)
					sender.WindowState = WindowState.Maximized;
			};
			window.Loaded += loadedHandler;
		}
		static Size GetSize(out bool maximizeWindow, double defaultWidth, double defaultHeight) {
			Size workSize = SystemParameters.WorkArea.Size;
			Size maxSize = new Size(workSize.Width, workSize.Height);
			maximizeWindow = defaultWidth > maxSize.Width &&
				defaultHeight > maxSize.Height;
			double actualWidth = Math.Min(defaultWidth, maxSize.Width);
			double actualHeight = Math.Min(defaultHeight, maxSize.Height);
			return new Size(actualWidth, actualHeight);
		}
		static void UpdateSize(Window window, double minWidth, double minHeight, Size size) {
			if(PresentationSource.FromVisual(window) == null)
				return;
			var point = window.PointToScreen(new Point(window.Left, window.Top));
			var workingArea = screen.GetWorkingArea(point);
			window.MinWidth = Math.Min(workingArea.Width, minWidth);
			window.MinHeight = Math.Min(workingArea.Height, minHeight);
			window.Width = Math.Min(workingArea.Width, size.Width);
			window.Height = Math.Min(workingArea.Height, size.Height);
			window.Left = workingArea.Left + (workingArea.Width - window.Width) / 2;
			window.Top = workingArea.Top + (workingArea.Height - window.Height) / 2;
		}
	}
	public class DpiAwareSizeBehavior : Behavior<Window> {
		protected override void OnAttached() {
			base.OnAttached();
			AssociatedObject.Do(x => DpiAwareSizeCorrector.Attach(x));
		}
	}
}
