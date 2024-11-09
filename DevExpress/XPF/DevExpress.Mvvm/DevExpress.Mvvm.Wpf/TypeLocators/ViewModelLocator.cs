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

using DevExpress.Mvvm.POCO;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Linq;
using System.Globalization;
namespace DevExpress.Mvvm {
	public interface IViewModelLocator {
		object ResolveViewModel(string name);
		Type ResolveViewModelType(string name);
		string GetViewModelTypeName(Type type);
	}
	public class ViewModelLocator : LocatorBase, IViewModelLocator {
		static IViewModelLocator _defaultInstance = new ViewModelLocator(Application.Current);
		static IViewModelLocator _default;
		public static IViewModelLocator Default { get { return _default ?? _defaultInstance; } set { _default = value; } }
		readonly IEnumerable<Assembly> assemblies;
		protected override IEnumerable<Assembly> Assemblies { get { return assemblies; } }
		public ViewModelLocator(Application application)
#if !NET_CORE
			: this(EntryAssembly != null && !ViewModelBase.IsInDesignMode ? new[] { EntryAssembly } : Native.EmptyArray<Assembly>.Instance) {
#else
			: this(EntryAssembly != null && !DesignMode.DesignModeEnabled ? new[] { EntryAssembly } : Array.Empty<Assembly>())) {
#endif
		}
		public ViewModelLocator(params Assembly[] assemblies)
			: this((IEnumerable<Assembly>)assemblies) {
		}
		public ViewModelLocator(IEnumerable<Assembly> assemblies) {
			this.assemblies = assemblies;
		}
		public virtual Type ResolveViewModelType(string name) {
			IDictionary<string, string> properties;
			Type res = ResolveType(name, out properties);
			if (res == null) return null;
			bool isPOCO = GetIsPOCOViewModelType(res, properties);
			return isPOCO ? ViewModelSource.GetPOCOType(res) : res;
		}
		public virtual string GetViewModelTypeName(Type type) {
			Dictionary<string, string> properties = new Dictionary<string, string>();
			if(type.GetInterfaces().Any(x => x == typeof(IPOCOViewModel))) {
				SetIsPOCOViewModelType(properties, true);
				type = type.BaseType;
			}
			return ResolveTypeName(type, properties);
		}
		protected bool GetIsPOCOViewModelType(Type type, IDictionary<string, string> properties) {
			string isPOCO;
			if(type.GetCustomAttributes(typeof(DataAnnotations.POCOViewModelAttribute), true).Length != 0)
				return true;
			if(properties.TryGetValue("IsPOCOViewModel", out isPOCO))
				return bool.Parse(isPOCO);
			return false;
		}
		protected void SetIsPOCOViewModelType(IDictionary<string, string> properties, bool value) {
			properties.Add("IsPOCOViewModel", value.ToString());
		}
		object IViewModelLocator.ResolveViewModel(string name) {
			Type type = ((IViewModelLocator)this).ResolveViewModelType(name);
			if (type == null)
				return null;
			return CreateInstance(type, name);
		}
	}
}
