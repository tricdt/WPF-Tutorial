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

namespace DevExpress.Mvvm.Internal {
	using System;
	using System.IO;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Xml;
	using System.Xml.Serialization;
	static class XmlSerializerHelper {
#pragma warning disable DX0008 // internal utility tool in assembly that doesn't reference DX Data
		const DtdProcessing XxeSafeDtdProcessing = DtdProcessing.Prohibit;
		const XmlResolver SsrfSafeXmlResolver = (XmlResolver)null;
		internal static XmlTextReader CreateTextReader(Stream stream) {
			var reader = new XmlTextReader(stream);
			reader.DtdProcessing = XxeSafeDtdProcessing;
			reader.XmlResolver = SsrfSafeXmlResolver;
			return reader;
		}
		static XmlTextReader CreateTextReader(TextReader textReader) {
			var reader = new XmlTextReader(textReader);
			reader.DtdProcessing = XxeSafeDtdProcessing;
			reader.XmlResolver = SsrfSafeXmlResolver;
			return reader;
		}
		public static T Deserialize<T>(Stream stream, Type[] extraTypes = null)
			where T : class {
			return Deserialize(stream, typeof(T), extraTypes) as T;
		}
		public static T Deserialize<T>(string xmlString, Type[] extraTypes = null)
			where T : class {
			return Deserialize(xmlString, typeof(T), extraTypes) as T;
		}
		public static object Deserialize(Stream stream, Type type, Type[] extraTypes = null) {
			try {
				var serializer = CreateSerializerCore(type, extraTypes);
				var xmlTextReader = CreateTextReader(stream);
				return DeserializeCore(serializer, xmlTextReader.EnsureTextReaderForXmlSerializer());
			} catch { return null; }
		}
		public static object Deserialize(string xmlString, Type type, Type[] extraTypes = null) {
			try {
				using(var textReader = new StringReader(xmlString)) {
					var serializer = CreateSerializerCore(type, extraTypes);
					var xmlTextReader = CreateTextReader(textReader);
					return DeserializeCore(serializer, xmlTextReader.EnsureTextReaderForXmlSerializer());
				}
			} catch { return null; }
		}
		public static void Serialize<T>(Stream stream, object root, Type[] extraTypes = null)
			where T : class {
			Serialize(stream, root, typeof(T), extraTypes);
		}
		public static void Serialize(Stream stream, object root, Type type, Type[] extraTypes = null) {
			var serializer = CreateSerializerCore(type, extraTypes);
			SerializeCore(stream, root, serializer);
		}
		public static string Serialize<T>(T root, Type type) {
			using(var stream = new MemoryStream()) {
				var serializer = CreateSerializerCore(type, null);
				SerializeCore(stream, root, serializer);
				stream.Seek(0, SeekOrigin.Begin);
				using(var reader = new StreamReader(stream)) 
					return reader.ReadToEnd();
			}
		}
		static XmlSerializer CreateSerializerCore(Type type, Type[] extraTypes) {
			return new XmlSerializer(type, null, extraTypes, null, null, null);
		}
		static void SerializeCore(Stream stream, object root, XmlSerializer serializer) {
			serializer.Serialize(stream, root);
		}
		static object DeserializeCore(XmlSerializer serializer, XmlReader xmlTextReader) {
			if(xmlTextReader == null || !serializer.CanDeserialize(xmlTextReader))
				return null;
			return serializer.Deserialize(xmlTextReader);
		}
		static XmlTextReader EnsureTextReaderForXmlSerializer(this XmlTextReader reader) {
			reader.WhitespaceHandling = WhitespaceHandling.Significant;
			reader.Normalization = true;
			return reader;
		}
#pragma warning restore DX0008
	}
	static class DataContractSerializerHelper {
		public static string Serialize<T>(T value) {
			var serializer = new DataContractSerializer(typeof(T));
			using(var stream = new MemoryStream()) {
				serializer.WriteObject(stream, value);
				stream.Seek(0, SeekOrigin.Begin);
				using(var reader = new StreamReader(stream))
					return reader.ReadToEnd();
			}
		}
		public static T Deserialize<T>(string value) {
			if(string.IsNullOrEmpty(value))
				return default(T);
			try {
				using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(value))) {
#pragma warning disable DX0011 // safe usage for predefined types (SerializedDocument, List<StringPair>)
					var serializer = new DataContractSerializer(typeof(T));
					using(var reader = XmlSerializerHelper.CreateTextReader(stream)) {
						return (T)serializer.ReadObject(reader);
					}
#pragma warning restore DX0011
				}
			} catch { return default(T); }
		}
	}
}
