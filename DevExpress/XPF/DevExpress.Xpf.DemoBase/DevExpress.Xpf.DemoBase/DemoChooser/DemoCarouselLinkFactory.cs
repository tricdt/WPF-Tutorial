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
using System.Windows.Input;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
namespace DevExpress.Xpf.DemoChooser {
	static class DemoCarouselLinkFactory {
		public static DemoCarouselLink CarouselLink(this OpenSolutionMenuItem i) =>
			new DemoCarouselLink { Title = DemoRunner.FormatSolution(i.IsVB, i.IsNetCore), SelectedCommand = i.OpenCommand };
		 static ICommand CreateCommand(string url, Platform platform) {
			return DemoRunner.CreateUrlCommand(url, platform, CallingSite.DemoChooser);
		}
		public static DemoCarouselLink CarouselLink(this Link link, Platform platform) => new DemoCarouselLink { Title = link.Title, SelectedCommand = CreateCommand(link.Url, platform) };
		public static IEnumerable<T> NullIfEmpty<T>(this IEnumerable<T> enumerable) {
			var enumerator = enumerable.GetEnumerator();
			if(!enumerator.MoveNext()) return null;
			return enumerator.Current.Yield().Concat(Enumerate(enumerator));
		}
		static IEnumerable<T> Enumerate<T>(IEnumerator<T> enumerator) {
			while(enumerator.MoveNext())
				yield return enumerator.Current;
		}
	}
}
