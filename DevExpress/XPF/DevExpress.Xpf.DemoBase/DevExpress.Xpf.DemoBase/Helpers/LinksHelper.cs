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
using DevExpress.DemoData;
using DevExpress.DemoData.Model;
namespace DevExpress.Xpf.DemoBase.Helpers {
	static class LinksHelper {
		static string GetGoLinkDataParameter(string platformName, string moduleTypeFullName) {
			var suffix = "?gldata=" + AssemblyInfo.VersionShort;
			if(!string.IsNullOrEmpty(moduleTypeFullName))
				suffix += "_" + moduleTypeFullName;
			return suffix + $"|{platformName}&platform={platformName}";
		}
		public static string BuyNow(string platformName, string moduleTypeFullName = null) {
			return SubscriptionsBuyLink + GetGoLinkDataParameter(platformName, moduleTypeFullName);
		}
		public static string GetSupport(string platformName, string moduleTypeFullName = null) {
			return AssemblyInfo.DXLinkGetSupport + GetGoLinkDataParameter(platformName, moduleTypeFullName);
		}
		public static string GetStarted(string platformName, string moduleTypeFullName = null) {
			var platform = Repository.Platforms.FirstOrDefault(p => string.Equals(p.Name, platformName, StringComparison.OrdinalIgnoreCase));
			var link = platform == null ? AssemblyInfo.DXLinkGetStarted : platform.GetStartedLink;
			return link + GetGoLinkDataParameter(platformName, moduleTypeFullName);
		}
		static readonly string Version = AssemblyInfo.Version.Remove(AssemblyInfo.Version.LastIndexOf('.'));
		public const string UniversalSubscriptionLink = "Https://go.devexpress.com/Demo_UniversalSubscription.aspx";
		public const string SubscriptionsBuyLink = "Https://go.devexpress.com/Demo_Subscriptions_Buy.aspx";
		public static readonly string WhatsNewLink = string.Format("Http://www.devexpress.com/Support/WhatsNew/DXperience/files/{0}.xml", Version);
		public static readonly string BreakingChangesLink = string.Format("Http://www.devexpress.com/Support/WhatsNew/DXperience/files/{0}.bc.xml", Version);
		public static readonly string KnownIssuesLink = string.Format("Http://www.devexpress.com/Support/WhatsNew/DXperience/files/{0}.ki.xml", Version);
	}
}
