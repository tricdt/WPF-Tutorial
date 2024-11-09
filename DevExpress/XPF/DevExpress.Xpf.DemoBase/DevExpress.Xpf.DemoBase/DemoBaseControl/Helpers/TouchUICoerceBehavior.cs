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
using System.Text;
using System.Windows.Controls;
using DevExpress.Data.Utils;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	using System.Windows;
	using DevExpress.Mvvm.UI.Interactivity;
	using DevExpress.Xpf.Core;
	public partial class TouchUICoerceBehavior : Behavior<ContentPresenter> {
		static TouchUICoerceBehavior() {
			DependencyPropertyRegistrator<TouchUICoerceBehavior>.New()
				.Register(nameof(AllowTouchUI), out AllowTouchUIProperty, true)
			;
		}
		public TouchUICoerceBehavior() {
			this
				.OnChanged(AllowTouchUIProperty)
				.SelectMany(_ => this.NotifyValue(x => x.AssociatedObject))
				.Where(x => x != null)
				.SelectMany(a => a.OnChanged(CommonThemeHelper.TreeWalkerProperty).SelectMany(_ => a.OnChanged(ContentPresenter.ContentProperty)))
				.Execute(_ => UpdateTheme());
		}
		void UpdateTheme() {
			var content = AssociatedObject.Content as DependencyObject;
			if(content == null) return;
			var walker = CommonThemeHelper.GetTreeWalker(AssociatedObject);
			if(walker == null || string.IsNullOrEmpty(walker.ThemeName)) return;
			var touchUI = walker.IsTouch && AllowTouchUI;
			CommonThemeHelper.SetThemeName(content, ThemeProperties.Combine(walker.ThemeName, touchUI));
		}
	}
}
