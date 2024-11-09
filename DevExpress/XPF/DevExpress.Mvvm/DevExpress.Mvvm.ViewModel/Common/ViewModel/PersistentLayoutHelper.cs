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
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Mvvm;
using DevExpress.Mvvm.ViewModel;
namespace DevExpress.Mvvm.ViewModel {
	public class PersistentLayoutHelper {
		public static string PersistentLogicalLayout {
			get { return LayoutSettings.Default.LogicalLayout; }
			set { LayoutSettings.Default.LogicalLayout = value; }
		}
		static Dictionary<string, string> persistentViewsLayout;
		public static Dictionary<string, string> PersistentViewsLayout {
			get {
				if(persistentViewsLayout == null) {
					persistentViewsLayout = LogicalLayoutSerializationHelper.Deserialize(LayoutSettings.Default.ViewsLayout);
				}
				return persistentViewsLayout;
			}
		}
		public static void TryDeserializeLayout(ILayoutSerializationService service, string viewName) {
			string state = null;
			if(service != null && PersistentViewsLayout.TryGetValue(viewName, out state)) {
				service.Deserialize(state);
			}
		}
		public static void TrySerializeLayout(ILayoutSerializationService service, string viewName) {
			if(service != null) {
				PersistentViewsLayout[viewName] = service.Serialize();
			}
		}
		public static void SaveLayout() {
#if DEBUGTEST
			SaveCalledCount++;
#endif
			LayoutSettings.Default.ViewsLayout = LogicalLayoutSerializationHelper.Serialize(PersistentViewsLayout);
			LayoutSettings.Default.Save();
		}
		public static void ResetLayout() {
			PersistentViewsLayout.Clear();
			PersistentLogicalLayout = null;
			SaveLayout();
		}
	}
}
