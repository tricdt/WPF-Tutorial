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
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Mvvm.Internal;
using DevExpress.Mvvm.Native;
namespace DevExpress.Mvvm {
	public interface ISupportState<T> {
		T SaveState();
		void RestoreState(T state);
	}
	public interface IStateSerializer {
		string SerializeState(object state, Type stateType);
		object DeserializeState(string state, Type stateType);
	}
	public class StateSerializer : IStateSerializer {
		static IStateSerializer _defaultInstance = new StateSerializer();
		static IStateSerializer _default;
		public static IStateSerializer Default { get { return _default ?? _defaultInstance; } set { _default = value; } }
		public string SerializeState(object state, Type stateType) {
			string res = null;
			if(state == null) return res;
			return SerializationHelper.SerializeToString(x => XmlSerializerHelper.Serialize(x, state, state.GetType()));
		}
		public object DeserializeState(string state, Type stateType) {
			object res = null;
			SerializationHelper.DeserializeFromString(state, x => res = XmlSerializerHelper.Deserialize(x, stateType));
			return res;
		}
	}
}
namespace DevExpress.Mvvm.Native {
	public static class ISupportStateHelper {
		public static Type GetStateType(Type vmType) {
			Type iSupportSerialization = GetISupportStateImplementation(vmType);
			if(iSupportSerialization == null) return null;
			return iSupportSerialization.GetGenericArguments().First();
		}
		public static object GetState(object vm) {
			if(vm == null) return null;
			Type vmType = vm.GetType();
			Type iSupportSerialization = GetISupportStateImplementation(vmType);
			if(iSupportSerialization == null) return null;
			var saveStateMethod = iSupportSerialization.GetMethod("SaveState");
			return saveStateMethod.Invoke(vm, null);
		}
		public static void RestoreState(object vm, object state) {
			if(vm == null || state == null) return;
			Type vmType = vm.GetType();
			Type iSupportSerialization = GetISupportStateImplementation(vmType);
			if(iSupportSerialization == null) return;
			var restoreStateMethod = iSupportSerialization.GetMethod("RestoreState");
			restoreStateMethod.Invoke(vm, new object[] { state });
		}
		static Type GetISupportStateImplementation(Type vmType) {
			return vmType.GetInterfaces()
				.Where(x => x.IsGenericType)
				.Where(x => x.GetGenericTypeDefinition() == typeof(ISupportState<>))
				.FirstOrDefault();
		}
	}
	public static class SerializationHelper {
		public static string SerializeToString(Action<Stream> serializationMethod) {
			string res = null;
			using(var ms = new MemoryStream()) {
				serializationMethod(ms);
				ms.Seek(0, SeekOrigin.Begin);
				using(var reader = new StreamReader(ms))
					res = reader.ReadToEnd();
			}
			return res;
		}
		public static void DeserializeFromString(string state, Action<Stream> deserializationMethod) {
			using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(state)))
				deserializationMethod(ms);
		}
		public static T DeserializeFromString<T>(string state, Func<Stream,T> deserializationMethod) {
			using(var ms = new MemoryStream(Encoding.UTF8.GetBytes(state)))
				return deserializationMethod(ms);
		}
	}
}
