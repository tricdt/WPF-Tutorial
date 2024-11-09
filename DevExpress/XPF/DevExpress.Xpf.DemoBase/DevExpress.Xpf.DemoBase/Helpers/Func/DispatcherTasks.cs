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
using System.Threading.Tasks;
using System.Windows.Threading;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public static class DispatcherTasks {
		public static TaskLinq<UnitT> Timer(this Dispatcher dispatcher, int milliseconds, DispatcherPriority priority = DispatcherPriority.Background) {
			Guard.ArgumentNotNull(dispatcher, "dispatcher");
			var timer = new DispatcherTimer(priority, dispatcher);
			timer.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
			var chain = new TaskLinq.Chain();
			return TaskLinq.On(
				x => {
					EventHandler h = (_, __) => {
						chain.Run(TaskScheduler.FromCurrentSynchronizationContext());
						x();
					};
					timer.Tick += h;
					timer.Start();
					return () => { timer.Stop(); timer.Tick -= h; };
				},
				chain
			);
		}
		public static TaskLinq<UnitT> Queue(this Dispatcher dispatcher, DispatcherPriority priority = DispatcherPriority.Normal, int count = 1) {
			Guard.ArgumentNotNull(dispatcher, "dispatcher");
			var chain = new TaskLinq.Chain();
			var taskSource = new TaskCompletionSource<UnitT>();
			Action setResultCore = () => {
				chain.Run(TaskScheduler.FromCurrentSynchronizationContext());
				taskSource.SetResult(default(UnitT));
			};
			Enumerable.Range(0, count).Aggregate(setResultCore, (a, _) => () => dispatcher.BeginInvoke(a, priority))();
			return taskSource.Task.Linq(chain);
		}
		public static void SetAsRethrowAsyncExceptionsContext(this Dispatcher dispatcher) {
			TaskLinq.RethrowAsyncExceptionsContext = new DispatcherSynchronizationContext(dispatcher);
		}
	}
}
