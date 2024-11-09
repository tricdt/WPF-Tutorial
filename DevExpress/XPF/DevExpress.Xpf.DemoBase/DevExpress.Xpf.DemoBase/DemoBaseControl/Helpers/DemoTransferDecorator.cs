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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DevExpress.Mvvm.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	class DemoTransferDecorator : Decorator {
		readonly ObservableCollection<Storyboard> storyboards = new ObservableCollection<Storyboard>();
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public ObservableCollection<Storyboard> Storyboards { get { return storyboards; } }
		TaskLinq<Storyboard> run = TaskLinq.Promise(default(Storyboard));
		public TaskLinq<UnitT> Run(int storyboardIndex, bool finish) {
			var sb = Storyboards[storyboardIndex];
			return Child.Return(child => {
				var cont = run;
				run = cont.SelectMany(s => {
					s.Do(x => x.Stop(this));
					return RunStoryboard(sb, child);
				}).Select(() => {
					if(finish) {
						child.RenderTransform = null;
						sb.Stop(this);
						return null;
					}
					return sb;
				});
				return run.Select(_ => default(UnitT));
			}, () => default(UnitT).Promise());
		}
		TaskLinq<UnitT> RunStoryboard(Storyboard sb, UIElement target) {
			target.RenderTransformOrigin = new Point(0.5, 0.5);
			if(!(target.RenderTransform is TransformGroup)) {
				TransformGroup group = new TransformGroup();
				group.Children.Add(new ScaleTransform());
				group.Children.Add(new TranslateTransform());
				target.RenderTransform = group;
			}
			Storyboard.SetTarget(sb, target);
			var done = new TaskCompletionSource<UnitT>();
			sb.Completed += LinqExtensions.WithReturnValue<EventHandler>(x => (_, __) => {
				sb.Completed -= x.Value;
				done.SetResult(default(UnitT));
			});
			sb.Begin(this, true);
			return done.Task.Linq();
		}
	}
}
