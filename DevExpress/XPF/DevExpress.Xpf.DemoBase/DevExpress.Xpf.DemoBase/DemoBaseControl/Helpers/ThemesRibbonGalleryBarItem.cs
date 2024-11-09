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
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.Ribbon;
namespace DevExpress.Xpf.DemoBase.Helpers {	
	public class ThemesRibbonGalleryBarItem : RibbonGalleryBarItem {
		static ThemesRibbonGalleryBarItem() {
			BarItemLinkCreator.Default.RegisterObject(typeof(ThemesRibbonGalleryBarItem), typeof(ThemesRibbonGalleryBarItemLink), x=>new ThemesRibbonGalleryBarItemLink());
			BarItemLinkControlCreator.Default.RegisterObject(typeof(ThemesRibbonGalleryBarItemLink), typeof(ThemesRibbonGalleryBarItemLinkControl), x => new ThemesRibbonGalleryBarItemLinkControl());
		}
	}
	public class ThemesRibbonGalleryBarItemLink : RibbonGalleryBarItemLink { }
	public class ThemesRibbonGalleryBarItemLinkControl : RibbonGalleryBarItemLinkControl {
		static readonly Action<BarPopupBase, PopupItemClickBehaviour> set_ItemClickBehavior = ReflectionHelper.CreateInstanceMethodHandler<BarPopupBase, Action<BarPopupBase, PopupItemClickBehaviour>>(null, "set_ItemClickBehaviour", BindingFlags.Instance | BindingFlags.NonPublic);
		protected override void OnPopupGalleryChanged(GalleryDropDownPopupMenu oldValue) {
			base.OnPopupGalleryChanged(oldValue);
			if(PopupGallery != null)
				set_ItemClickBehavior(PopupGallery, PopupItemClickBehaviour.None);
		}
	}
	public class ThemesBarSplitButtonItem : BarSplitButtonItem {
		static ThemesBarSplitButtonItem() {
			BarItemLinkCreator.Default.RegisterObject(typeof(ThemesBarSplitButtonItem), typeof(ThemesBarSplitButtonItemLink), x => new ThemesBarSplitButtonItemLink());
			BarItemLinkControlCreator.Default.RegisterObject(typeof(ThemesBarSplitButtonItemLink), typeof(BarSplitButtonItemLinkControl), x => new BarSplitButtonItemLinkControl());
		}
	}
	public class ThemesBarSplitButtonItemLink : BarSplitButtonItemLink { }
}
