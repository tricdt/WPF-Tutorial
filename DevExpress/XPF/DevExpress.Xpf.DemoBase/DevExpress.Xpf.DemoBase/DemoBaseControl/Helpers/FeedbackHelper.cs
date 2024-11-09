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
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
#if !NET
using DevExpress.Mvvm.Native;
#endif
namespace DevExpress.Xpf.DemoBase.Helpers {
	public class FeedbackHelper {
		static Dictionary<string, RatingDto> fbModules = new Dictionary<string, RatingDto>();
#if DEBUG
		static Uri address = new Uri("http://internalserver/DemoCenterFeedback/api/v1/feedback");
#else
		static Uri address = new Uri("https://services.devexpress.com/customerfeedback/api/v1/Feedback");
#endif
		const string xmlContentType = "application/xml";
		public static bool? GetFeedback(string moduleName) {
			switch(fbModules.GetValueOrDefault(moduleName, RatingDto.Neutral)) {
			case RatingDto.Neutral: return null;
			case RatingDto.Positive: return true;
			case RatingDto.Negative: return false;
			default: throw new InvalidOperationException();
			}
		}
		public static void PostFeedbackAsync(string moduleName, bool value, string message) {
			var rating = value ? RatingDto.Positive : RatingDto.Negative;
			if(fbModules.ContainsKey(moduleName))
				fbModules[moduleName] = rating;
			else
				fbModules.Add(moduleName, rating);
			new Thread(() => {
				try {
#pragma warning disable SYSLIB0014 // Type or member is obsolete
					using(var client = new WebClient()) {
						client.Headers[HttpRequestHeader.ContentType] = xmlContentType;
						client.Encoding = Encoding.UTF8;
						Task.Factory.StartNew(() => {
							var eventFeedback = GetXmlString(new FeedbackObject() { ModuleName = moduleName, Feedback = message ?? string.Empty, Value = rating, Email = string.Empty });
							var res = client.UploadString(address, eventFeedback);
#if DEBUG
							Debug.WriteLine(string.Format("{0} - {1}", eventFeedback, res));
#endif
						}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
					}
#pragma warning restore SYSLIB0014 // Type or member is obsolete
				} catch {
#if DEBUG
					Debug.WriteLine(string.Format("Connect Error")); 
#endif
				}
			}).Start();
		}
#pragma warning disable DX0008 // data for demos is manually checked
		static string GetXmlString(FeedbackObject fbObj) {
			string xmlString;
			using(var ms = new MemoryStream()) {
				var xmlSerializer = new Native.Serialization.GeneratedSerializers.FeedbackObject.FeedbackObjectSerializer();
				using(var streamWriter = new StreamWriter(ms, Encoding.UTF8)) {
					xmlSerializer.Serialize(streamWriter, fbObj);
					ms.Seek(0, SeekOrigin.Begin);
					using(var sr = new StreamReader(ms, Encoding.UTF8)) {
						xmlString = sr.ReadToEnd();
					}
				}
			}
			return xmlString;
		}
#pragma warning restore DX0008
		public static List<string> modules = new List<string>();
		public static bool ShowMessage(string moduleId) {
			if(modules.Contains(moduleId))
				return false;
			else
				modules.Add(moduleId);
			return AllowShowMessage;
		}
		bool? showFeedback = null;
		public bool ShowFeedback {
			get {
				if(!showFeedback.HasValue)
					showFeedback = IsExistFeedbackMessageVer;
				return (bool)showFeedback;
			}
		}
		const string FeedbackMessageVer = "WPFFeedbackMessageVersion";
		const string DesignerPath = "Software\\Developer Express\\Designer\\";
		static bool IsExistFeedbackMessageVer {
			get {
#if DEBUGTEST
				return true;
#else
				string ret = ReadRegistryValue(DesignerPath, FeedbackMessageVer);
				return !string.IsNullOrEmpty(ret) && AssemblyInfo.VersionShort == ret;
#endif
			}
		}
		static bool AllowShowMessage { get { return !IsExistFeedbackMessageVer; } }
		static string ReadRegistryValue(string path, string valueName) {
			try {
				using(var key = Registry.CurrentUser.OpenSubKey(path)) {
					if(key == null) return null;
					var value = key.GetValue(valueName);
					key.Close();
					return value == null ? null : value.ToString();
				}
			} catch {
				return null;
			}
		}
		public static void ApplyDoNotShow() {
			WriteRegistryValue(DesignerPath, FeedbackMessageVer, AssemblyInfo.VersionShort);
		}
		static void WriteRegistryValue(string path, string valueName, string value) {
			try {
				using(var key = Registry.CurrentUser.OpenSubKey(path, true) ?? Registry.CurrentUser.CreateSubKey(path))
					key.SetValue(valueName, value);
			} catch { }
		}
		public enum RatingDto {
			Negative = -1,
			Neutral = 0,
			Positive = 1
		}
		[XmlRoot("ApprisePostDto"), Serializable]
		public sealed class FeedbackObject {
			string moduleName, feedback, email;
			string FullModuleName { get { return string.Format("{0}_{1}", GetString(moduleName, 100), AssemblyInfo.VersionShort); } }
			public string ModuleName { get { return FullModuleName; } set { moduleName = value; } }
			public string Email { get { return GetString(email, 254); } set { email = value; } }
			public string Feedback { get { return GetString(feedback, 1000); } set { feedback = value; } }
			public RatingDto Value { get; set; }
			public string UserId { get { return GetId().ToString(); } set { } }
			string GetString(string s, int length) {
				if(s.Length > length) return s.Substring(0, length);
				return s;
			}
			static Guid GetId() {
				return Guid.Empty;
			}
		}
	}
}
