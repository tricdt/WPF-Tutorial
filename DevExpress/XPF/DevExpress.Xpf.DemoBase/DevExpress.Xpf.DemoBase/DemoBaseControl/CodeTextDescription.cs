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
using DevExpress.Utils;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoBase {
	public sealed class CodeTextDescription : ImmutableObject {
		public CodeTextDescription(string fileName, Func<string> codeText) {
			Func<string, bool> isDependent = f =>
				f.EndsWith(CodeViewControl.XamlSuffix + CodeViewControl.CSSuffix)
				|| f.EndsWith(CodeViewControl.XamlSuffix + CodeViewControl.VBSuffix);
			this.fileName = fileName;
			this.isDependent = isDependent(fileName);
			this.codeText = codeText;
		}
		readonly string fileName;
		public string FileName { get { return fileName; } }
		readonly bool isDependent;
		public string DisplayName { get { return isDependent ? "    " + fileName : fileName; } }
		readonly Func<string> codeText;
		public Func<string> CodeText { get { return codeText; } }
	}
}
