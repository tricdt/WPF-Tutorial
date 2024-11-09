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
using System.Linq;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Utils;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.DemoCenterBase.Helpers;
namespace DevExpress.Xpf.DemoCenter.Internal {
	public class DemoCenterFeatureDemos : ImmutableObject {
		public DemoCenterFeatureDemos() {
			FeatureDemos = Repository.Platforms
				.SelectMany(x => x.ReallifeDemos)
				.Where(x => x.ShowInDemoCenter)
				.OrderBy(x => x.DemoCenterPosition)
				.Select(demo => new DemoCarouselItem(
					isUniversal: false,
					name: demo.Name,
					title: demo.DisplayName,
					platformLabel: PlatformTitles.GetLabel(demo.Platform),
					onRunCommand: new DelegateCommand(() => {
						demo.Match(
							rlDemo => DemoRunner.TryStartReallifeDemoAndShowErrorMessage(rlDemo, CallingSite.DemoCenter),
							wpfDemo => DemoRunner.LoadAndRunWpfDemo(wpfDemo.AssemblyName, false.AsLeft()),
							link => DemoRunner.StartWebLinkApp(link)
						);
					}),
					preview: demo.MediumImage.Uri,
					isAvailable: demo.Platform.IsInstalled,
					links: demo.CreateOpenSolutionMenu(CallingSite.DemoCenter)
						.Select(i => new DemoCarouselLink { Title = DemoRunner.FormatOpenSolution(i.IsVB, i.IsNetCore), SelectedCommand = i.OpenCommand })
						.ToReadOnlyObservableCollection()
				)).ToReadOnlyObservableCollection();
		}
		public ReadOnlyObservableCollection<DemoCarouselItem> FeatureDemos { get; }
	}
}
