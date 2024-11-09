﻿#region Copyright (c) 2000-2024 Developer Express Inc.
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
using System.IO;
using System.Linq;
using System.Reflection;
using DevExpress.Data.Internal;
namespace DevExpress.Xpf.DemoBase {
	public static class AssemblyResolver {
		static readonly string _monoName = "Mono.Cecil";
		static bool _subcribed;
		public static void Subscribe() {
			if(!_subcribed) {
				AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
				_subcribed = true;
			}
		}
		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
			if(IsMono(args.Name))
				return ResolveMonoCecil(args);
			return null;
		}
		static Assembly ResolveMonoCecil(ResolveEventArgs args) {
			AssemblyName name = new AssemblyName(args.Name);
			var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Equals(name));
			if(assembly != null)
				return assembly;
			var embededMonoCecilAsmName = string.Format("DevExpress.Xpf.DemoBase.Dll.{0}.dll", _monoName);
			using(var reader = new BinaryReader(typeof(AssemblyResolver).Assembly.GetManifestResourceStream(embededMonoCecilAsmName)))
				return SafeTypeResolver.GetOrLoadAssemblyBytes(reader.ReadBytes((int)reader.BaseStream.Length));
		}
		static bool IsMono(string name) {
			return name.StartsWith(_monoName);
		}
	}
}
