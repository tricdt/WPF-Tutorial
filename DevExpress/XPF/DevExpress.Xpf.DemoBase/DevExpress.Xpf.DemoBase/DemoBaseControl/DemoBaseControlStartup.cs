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
using System.Reflection;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
namespace DevExpress.Xpf.DemoBase {
	public sealed class DemoBaseControlStartup {
		public static DemoBaseControlStartup DemoLauncher(string demoAssemblyName) => new DemoBaseControlStartup(false, () => { }, Either<string, Assembly>.Left(demoAssemblyName).AsOption());
		public static DemoBaseControlStartup DemoChooser(Action done) => new DemoBaseControlStartup(false, done, Option<Either<string, Assembly>>.Empty);
		public static readonly DemoBaseControlStartup DemoExe = new DemoBaseControlStartup(true, () => { }, Option<Either<string, Assembly>>.Empty);
		public static DemoBaseControlStartup Tests(Assembly demoAssembly) => new DemoBaseControlStartup(false, () => { }, Either<string, Assembly>.Right(demoAssembly).AsOption());
		DemoBaseControlStartup(bool isDemoExe, Action done, Option<Either<string, Assembly>> demoAssembly) {
			IsDemoExe = isDemoExe;
			Done = done;
			DemoAssembly = demoAssembly;
		}
		internal readonly bool IsDemoExe;
		internal readonly Option<Either<string, Assembly>> DemoAssembly;
		public readonly Action Done;
	}
}
