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
using System.Linq.Expressions;
using System.Reflection;
namespace DevExpress.Mvvm {
	public abstract class LocatorBase {
		static Assembly entryAssembly;
		protected static Assembly EntryAssembly {
			get {
				if(entryAssembly == null) {
#if WINUI
					entryAssembly = Assembly.GetEntryAssembly();
#else
					entryAssembly = Assembly.GetEntryAssembly();
#endif
				}
				return entryAssembly;
			}
			set { entryAssembly = value; }
		}
		protected abstract IEnumerable<Assembly> Assemblies { get; }
		Dictionary<string, Type> registeredTypes = new Dictionary<string, Type>();
		Dictionary<string, Type> shortNameToTypeMapping = new Dictionary<string, Type>();
		Dictionary<string, Type> fullNameToTypeMapping = new Dictionary<string, Type>();
		IEnumerator<Type> enumerator;
		public void RegisterType(string name, Type type) {
			registeredTypes.Add(name, type);
		}
		protected Type ResolveType(string name, out IDictionary<string, string> properties) {
			ResolveTypeProperties(ref name, out properties);
			if(string.IsNullOrEmpty(name))
				return null;
			Type type;
			if(registeredTypes.TryGetValue(name, out type))
				return type;
			if(shortNameToTypeMapping.TryGetValue(name, out type) || fullNameToTypeMapping.TryGetValue(name, out type))
				return type;
			if(enumerator == null) enumerator = GetTypes();
			while(enumerator.MoveNext()) {
				if(!fullNameToTypeMapping.ContainsKey(enumerator.Current.FullName)) {
					shortNameToTypeMapping[enumerator.Current.Name] = enumerator.Current;
					fullNameToTypeMapping[enumerator.Current.FullName] = enumerator.Current;
				}
				if(enumerator.Current.Name == name || enumerator.Current.FullName == name)
					return enumerator.Current;
			}
			return null;
		}
		protected string ResolveTypeName(Type type, IDictionary<string, string> properties) {
			if(type == null) return null;
			var props = CreateTypeProperties(properties);
			return props + type.FullName;
		}
		protected virtual IEnumerator<Type> GetTypes() {
			foreach(Assembly asm in Assemblies) {
				Type[] types = EmptyArray<Type>.Instance;
				try {
#if !WINUI
					types = asm.GetTypes();
#else
					types = asm.GetTypes();
#endif
				} catch(ReflectionTypeLoadException e) {
					types = e.Types;
				}
				foreach(var type in types) {
					if(type != null)
						yield return type;
				}
			}
		}
		protected static void ResolveTypeProperties(ref string name, out IDictionary<string, string> properties) {
			Func<string, string> getPropName = x => {
				var ind = x.IndexOf('=');
				return ind == -1 ? null : x.Substring(0, ind);
			};
			Func<string, string> getPropValue = x => {
				var ind1 = x.IndexOf('=');
				var ind2 = x.IndexOf(';');
				if(ind1 == -1 || ind2 == -1) return null;
				return x.Substring(ind1 + 1, ind2 - ind1 - 1);
			};
			Func<string, string> removeProp = x => {
				var ind = x.IndexOf(';');
				return x.Remove(0, ind + 1);
			};
			properties = new Dictionary<string, string>();
			if(string.IsNullOrEmpty(name)) return;
			while(true) {
				var propName = getPropName(name);
				var propValue = getPropValue(name);
				if(propName == null || propValue == null) break;
				properties.Add(propName, propValue);
				name = removeProp(name);
			}
		}
		protected static string CreateTypeProperties(IDictionary<string, string> properties) {
			if(properties == null) return null;
			string res = string.Empty;
			foreach(var item in properties) {
				res += item.Key;
				res += "=";
				res += item.Value;
				res += ";";
			}
			return res;
		}
		protected virtual object CreateInstance(Type type, string typeName) {
			if(type == null) return null;
			object res = null;
			try {
#if WINUI
				var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
				var parameterlessCtor = ctors.FirstOrDefault(x => x.GetParameters().Count() == 0);
				if(parameterlessCtor != null)
					res = parameterlessCtor.Invoke(null);
				if(res == null) {
					var optionalParametersCtor = ctors.FirstOrDefault(x => x.GetParameters().All(p => p.IsOptional));
					if(optionalParametersCtor != null)
						res = optionalParametersCtor.Invoke(optionalParametersCtor.GetParameters().Select(x => Type.Missing).ToArray());
				}
#else
				var parameterlessCtor = type.GetConstructor(
					BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance,
					null, EmptyArray<Type>.Instance, null);
				if(parameterlessCtor != null)
					res = parameterlessCtor.Invoke(null);
				if(res == null) {
					res = Activator.CreateInstance(type,
						BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.Instance | BindingFlags.OptionalParamBinding,
						null, null, null);
				}
#endif
			} catch(Exception e) {
				throw new LocatorException(GetType().Name, typeName, e);
			}
			if(res == null) throw new LocatorException(GetType().Name, typeName, null);
			return res;
		}
	}
	public class LocatorException : Exception {
		public LocatorException(string locatorName, string type, Exception innerException)
			: base(string.Format("{0} cannot resolve the {1} type. See the inner exception for details.", locatorName, type),
				  innerException) { }
	}
}
