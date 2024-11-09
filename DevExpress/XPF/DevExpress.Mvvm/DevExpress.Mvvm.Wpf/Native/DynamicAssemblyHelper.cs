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

using DevExpress.Internal;
using System;
using System.Collections;
using System.Reflection;
namespace DevExpress.Mvvm.Native {
	static class DynamicAssemblyHelper {
#if !FREE && !NET_CORE
		static Lazy<Assembly> dataAssembly = new Lazy<Assembly>(() => ResolveAssembly(AssemblyInfo.SRAssemblyData + AssemblyInfo.FullAssemblyVersionExtension));
		static Lazy<Assembly> xpfCoreAssembly = new Lazy<Assembly>(() => ResolveAssembly(AssemblyInfo.SRAssemblyXpfCore + AssemblyInfo.FullAssemblyVersionExtension));
		public static Assembly DataAssembly { get { return dataAssembly.Value; } }
		public static Assembly XpfCoreAssembly { get { return xpfCoreAssembly.Value; } }
#elif FREE
		static Lazy<Assembly> mvvmUIAssembly = new Lazy<Assembly>(() => ResolveAssembly(MvvmAssemblyHelper.MvvmUIAssemblyName));
		public static Assembly MvvmUIAssembly { get { return mvvmUIAssembly.Value; } }
#endif
		static Assembly ResolveAssembly(string asmName) {
			IEnumerable assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach(Assembly asm in assemblies) {
				if(PartialNameEquals(asm.FullName, asmName))
					return asm;
			}
#pragma warning disable DX0010
			return Assembly.Load(asmName);
#pragma warning restore DX0010
		}
		static bool PartialNameEquals(string asmName0, string asmName1) {
			return string.Equals(GetPartialName(asmName0), GetPartialName(asmName1),
				StringComparison.InvariantCultureIgnoreCase);
		}
		static string GetPartialName(string asmName) {
			int nameEnd = asmName.IndexOf(',');
			return nameEnd < 0 ? asmName : asmName.Remove(nameEnd);
		}
	}
}
