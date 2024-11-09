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

using DevExpress.Mvvm.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
namespace DevExpress.Mvvm {
	class DefaultServiceContainer : ServiceContainer {
		public DefaultServiceContainer() : base(null) { }
		protected virtual ResourceDictionary GetApplicationResources() {
			bool hasAccess = Application.Current.Return(x => x.Dispatcher.CheckAccess(), () => false);
			return hasAccess ? Application.Current.Resources : null;
		}
		Dictionary<string, object> GetApplicationResources(Type type) {
			var appResources = GetApplicationResources();
			if(appResources == null)
				return new Dictionary<string, object>();
			return appResources.Keys.OfType<string>()
				.ToDictionary(x => x, x => appResources[x])
				.Where(x => x.Value != null && type.IsAssignableFrom(x.Value.GetType()))
				.ToDictionary(x => x.Key, x => x.Value);
		}
		protected override object GetServiceCore(Type type, string key, ServiceSearchMode searchMode, out bool serviceHasKey) {
			object res = base.GetServiceCore(type, key, searchMode, out serviceHasKey);
			if(res != null)
				return res;
			var appResources = GetApplicationResources(type);
			object service;
			if(!string.IsNullOrEmpty(key) && appResources.TryGetValue(key, out service))
				return service;
			serviceHasKey = true;
			return appResources.FirstOrDefault().Value;
		}
		protected override IEnumerable<object> GetServicesCore(Type type, bool localOnly) {
			foreach(var x in base.GetServicesCore(type, localOnly))
				yield return x;
			foreach(var x in GetApplicationResources(type).Values)
				yield return x;
		}
	}
}
