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
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Mvvm.UI.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	using System.Windows;
	using DevExpress.Xpf.Ribbon;
	public partial class RibbonDynamicCategoryBehavior : Behavior<RibbonPageCategory> {
		static RibbonDynamicCategoryBehavior() {
			DependencyPropertyRegistrator<RibbonDynamicCategoryBehavior>.New()
				.RegisterAttached<RibbonPage, bool?>(nameof(GetIsVisible), out IsVisibleProperty, default(bool?))
			;
		}
		public RibbonDynamicCategoryBehavior() {
			this.NotifyValue(x => x.AssociatedObject)
				.Where(x => x != null)
				.SelectMany(c => c.DependencyValue(x => x.Ribbon))
				.Where(x => x != null)
				.SelectMany(r => r.OnChanged(RibbonControlHelper.MergingStatusProperty).Select(a=>r))
				.SelectMany(r => r
					.ActualCategories.Hierarchy((RibbonPageCategoryBase c) => c
					.ActualPages.Hierarchy((RibbonPage p) => p
					.DependencyValue(IsVisibleProperty).Void()))
					.Select(() => r.ActualCategories.SelectMany(x => x.ActualPages).Any(x => !GetIsVisible(x).HasValue))
				)
				.DefaultValue(() => false)
				.Execute(useCategories => {
					var ribbon = AssociatedObject.With(x => x.Ribbon);
					if(ribbon == null || RibbonControlHelper.GetMergingStatus(ribbon) != MergingStatus.Completed) return;
					AssociatedObject.IsVisible = useCategories;
					AssociatedObject.ActualPages.ForEach(x => x.IsVisible = GetIsVisible(x) ?? false);
					ribbon.ActualCategories.Where(x => x.IsDefault).SelectMany(x => x.ActualPages).ForEach(page => {
						var isVisible = GetIsVisible(page);
						if(isVisible.HasValue)
							page.IsVisible = isVisible.Value && !useCategories;
					});
				})
			;
		}
	}
}
