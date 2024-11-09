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
using System.Collections.Generic;
namespace DevExpress.Mvvm.DataAnnotations {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field 
		| AttributeTargets.Class | AttributeTargets.Interface 
		| AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
	public abstract class CommonEditorAttributeBase : Attribute {
		public object TemplateKey { get; set; }
#if !NETCORE3
		public override bool Equals(object obj) {
			if (obj == null || GetType() != obj.GetType())
				return false;
			var other = (CommonEditorAttributeBase)obj;
			return other != null && Equals(TemplateKey, other.TemplateKey);
		}
		public override int GetHashCode() {
			var hashCode = -1829533528;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(TemplateKey);
			return hashCode;
		}
#endif
	}
	public class DefaultEditorAttribute : CommonEditorAttributeBase { }
	public class GridEditorAttribute : CommonEditorAttributeBase { }
	public class LayoutControlEditorAttribute : CommonEditorAttributeBase { }
	public class PropertyGridEditorAttribute : CommonEditorAttributeBase { }
}
