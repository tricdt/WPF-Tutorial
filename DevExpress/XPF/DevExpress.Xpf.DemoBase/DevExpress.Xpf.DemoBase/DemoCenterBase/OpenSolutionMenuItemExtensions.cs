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
using DevExpress.DemoData.Helpers;
using DevExpress.DemoData.Model;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer;
namespace DevExpress.Xpf.DemoCenterBase {
	public static class OpenSolutionMenuItemExtensions {
		sealed class DelegateCommand : ICommand {
			readonly Action action;
			public DelegateCommand(Action action) {
				this.action = action;
			}
			public void Execute(object parameter) {
				if(action != null)
					action();
			}
			public bool CanExecute(object parameter) { return action != null; }
			public event EventHandler CanExecuteChanged { add { } remove { } }
		}
		public static IEnumerable<OpenSolutionMenuItem> CreateOpenSolutionMenu(this Demo demo, CallingSite site) {
#if !NET
			if(demo.CsSolutionPath != null && !EnvironmentHelper.IsClickOnce)
				yield return new OpenSolutionMenuItem(isVB: false, isNetCore: NetCorePathHelper.IsNetCoreByDefault(demo),
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, x => x.CsSolutionPath, DemoRunner.GetOpenSolutionMessage(demo, site, false))));
			if(demo.VbSolutionPath != null && !EnvironmentHelper.IsClickOnce)
				yield return new OpenSolutionMenuItem(isVB: true, isNetCore: NetCorePathHelper.IsNetCoreByDefault(demo),
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, x => x.VbSolutionPath, DemoRunner.GetOpenSolutionMessage(demo, site, true))));
#endif
			if(demo.CsSolutionPath != null && !EnvironmentHelper.IsClickOnce && DemoRunner.CheckExists(demo, x => x.CsSolutionPath))
				yield return new OpenSolutionMenuItem(isVB: false, isNetCore: true,
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, DemoRunner.DemoToNetCoreFullPath(x => x.CsSolutionPath), DemoRunner.GetOpenSolutionMessage(demo, site, false))));
			if(demo.VbSolutionPath != null && !EnvironmentHelper.IsClickOnce && DemoRunner.CheckExists(demo, x => x.VbSolutionPath))
				yield return new OpenSolutionMenuItem(isVB: true, isNetCore: true,
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, DemoRunner.DemoToNetCoreFullPath(x => x.VbSolutionPath), DemoRunner.GetOpenSolutionMessage(demo, site, true))));
		}
		public static IEnumerable<OpenSolutionMenuItem> CreateOpenSolutionMenu(this BaseReallifeDemo demo, CallingSite site) {
#if !NET
			if(demo.CsSolutionPath != null && !EnvironmentHelper.IsClickOnce)
				yield return new OpenSolutionMenuItem(isVB: false, isNetCore: NetCorePathHelper.IsNetCoreByDefault(demo),
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, x => x.CsSolutionPath, DemoRunner.GetOpenSolutionMessage(demo, site, false))));
			if(demo.VbSolutionPath != null && !EnvironmentHelper.IsClickOnce)
				yield return new OpenSolutionMenuItem(isVB: true, isNetCore: NetCorePathHelper.IsNetCoreByDefault(demo),
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, x => x.VbSolutionPath, DemoRunner.GetOpenSolutionMessage(demo, site, true))));
#endif
			if(demo.CsSolutionPath != null && !EnvironmentHelper.IsClickOnce && DemoRunner.CheckExists(demo, x => x.CsSolutionPath))
				yield return new OpenSolutionMenuItem(isVB: false, isNetCore: true,
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, DemoRunner.ReallifeToNetCoreFullPath(x => x.CsSolutionPath), DemoRunner.GetOpenSolutionMessage(demo, site, false))));
			if(demo.VbSolutionPath != null && !EnvironmentHelper.IsClickOnce && DemoRunner.CheckExists(demo, x => x.VbSolutionPath))
				yield return new OpenSolutionMenuItem(isVB: true, isNetCore: true,
					openCommand: new DelegateCommand(() => DemoRunner.OpenSolution(demo, DemoRunner.ReallifeToNetCoreFullPath(x => x.VbSolutionPath), DemoRunner.GetOpenSolutionMessage(demo, site, true))));
		}
	}
}
