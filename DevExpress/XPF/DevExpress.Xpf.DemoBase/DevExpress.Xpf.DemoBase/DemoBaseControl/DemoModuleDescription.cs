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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DevExpress.DemoData.Model;
using DevExpress.Mvvm;
namespace DevExpress.Xpf.DemoBase {
	public sealed class DemoModuleDescription : BindableBase {
		[EditorBrowsable(EditorBrowsableState.Never)]
		public DemoModuleDescription(WpfModule wpfModule, WpfDemo demo) {
			WpfModule = wpfModule;
			Demo = demo;
		}
		public readonly WpfModule WpfModule;
		public readonly WpfDemo Demo;
		[Obsolete("For preventing binding errors (CollectionView's Group has Name property)")]
		public string Name { get { return null; } }
		public string GroupName { get { return WpfModule.Group; } }
		public string Title { get { return WpfModule.DisplayName; } }
		public string TypeName { get { return WpfModule.TypeName; } }
		public string ShortInfo { get { return "[" + AssemblyInfo.VersionShort + "]: " + TypeName; } }
		Type moduleType;
		public void ResolveModuleType(Assembly demoAssembly) {
#if DEBUG
			try {
#endif
				moduleType = DevExpress.Data.Internal.SafeTypeResolver.GetKnownType(demoAssembly, TypeName);
				if(moduleType == null)
					throw new Exception(string.Format("DemoModule *{0}* not found", TypeName));
#if DEBUG
			} catch { } 
#endif
		}
		public Type ModuleType { get { return moduleType; } }
		public override string ToString() {
			return WpfModule.Name;
		}
	}
}
