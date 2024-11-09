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
using System.Text;
using DevExpress.Mvvm.UI;
using DevExpress.Mvvm.UI.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	using System.Windows;
	using System.Windows.Data;
	using System.Windows.Media;
	using System.Windows.Threading;
	using DevExpress.Mvvm.Native;
	public partial class WindowTitleService : WindowAwareServiceBase {
		static WindowTitleService() {
			DependencyPropertyRegistrator<WindowTitleService>.New()
				.Register(nameof(WindowTitle), out WindowTitleProperty, string.Empty, x => x.Update())
				.Register(nameof(WindowIcon), out WindowIconProperty, default(ImageSource), x => x.Update())
			;
		}
		string originalTitle;
		ImageSource originalIcon;
		protected override void OnActualWindowChanged(Window oldWindow) {
			oldWindow.Do(w => {
				w.Loaded -= OnLoaded;
			});
			ActualWindow.Do(w => {
				originalTitle = w.Title;
				originalIcon = w.Icon;
				if(w.IsLoaded)
					OnLoaded(null, null);
				else w.Loaded += OnLoaded;
			});
		}
		void OnLoaded(object sender, RoutedEventArgs e) {
			Update();
			ActualWindow.Loaded -= OnLoaded;
#if DEBUGTEST
			Dispatcher.InvokeAsync(() => {
				DemoBaseControl.StartupStopwatch.Stop();
				if(ActualWindow == null) return;
				var starupTime = DemoBaseControl.StartupStopwatch.ElapsedMilliseconds.ToString();
				ActualWindow.Title += $" ({starupTime}ms)";
			}, DispatcherPriority.ApplicationIdle);
#endif
		}
		void Update() {
			if(ActualWindow == null) return;
			ActualWindow.Title = WindowTitle ?? originalTitle;
			ActualWindow.Icon = WindowIcon ?? originalIcon;
		}
	}
}
