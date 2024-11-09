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
using System.Reflection;
using DevExpress.Mvvm.UI;
namespace DevExpress.Mvvm.UI {
	public interface IViewLocator {
		object ResolveView(string name);
		Type ResolveViewType(string name);
		string GetViewTypeName(Type type);
	}
}
#if !WINUI
namespace DevExpress.Mvvm.Native {
	static class ViewLocatorHelper {
		const string ViewLocatorTypeName = "DevExpress.Mvvm.UI.ViewLocator";
		static PropertyInfo ViewLocatorDefaultProperty;
		public static IViewLocator Default {
			get {
				if(ViewLocatorDefaultProperty == null) {
#pragma warning disable DX0004
#if !FREE
					var viewLocatorType = DynamicAssemblyHelper.XpfCoreAssembly.GetType(ViewLocatorTypeName);
#elif FREE
					var viewLocatorType = DynamicAssemblyHelper.MvvmUIAssembly.GetType(ViewLocatorTypeName);
#endif
#pragma warning restore DX0004
					ViewLocatorDefaultProperty = viewLocatorType.GetProperty("Default", BindingFlags.Static | BindingFlags.Public);
				}
				return (IViewLocator)ViewLocatorDefaultProperty.GetValue(null, null);
			}
		}
	}
}
#endif
