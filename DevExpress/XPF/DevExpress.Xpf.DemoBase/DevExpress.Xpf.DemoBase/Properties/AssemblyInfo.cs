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
using System.Security;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;
using DevExpress.Xpf.DemoBase;
using System.Runtime.CompilerServices;
using System.Resources;
[assembly: AssemblyTitle("DevExpress.Xpf.DemoBase")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(AssemblyInfo.AssemblyCompany)]
[assembly: AssemblyProduct("DevExpress.Xpf.DemoBase")]
[assembly: AssemblyCopyright("Copyright © 2000-2024 Developer Express Inc.")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
#if NET
[assembly: System.Runtime.Versioning.SupportedOSPlatform(AssemblyInfo.SupportedOSPlatform)]
#endif
[assembly: ComVisible(false)]
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]
[assembly: XmlnsPrefix(XmlNamespaceConstants.DemoBaseXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseXmlNamespacePrefix)]
[assembly: XmlnsDefinition(XmlNamespaceConstants.DemoBaseXmlNamespaceDefinition, XmlNamespaceConstants.DemoDataNamespace)]
[assembly: XmlnsDefinition(XmlNamespaceConstants.DemoBaseXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseNamespace)]
[assembly: XmlnsDefinition(XmlNamespaceConstants.DemoBaseXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseHelpersNamespace)]
[assembly: XmlnsDefinition(XmlNamespaceConstants.DemoBaseXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseDataClassesNamespace)]
[assembly: XmlnsPrefix(XmlNamespaceConstants.DemoBaseInternalXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseInternalXmlNamespacePrefix)]
[assembly: XmlnsDefinition(XmlNamespaceConstants.DemoBaseInternalXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseHelpersInternalNamespace)]
[assembly: XmlnsPrefix(XmlNamespaceConstants.DemoBaseGridXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseGridXmlNamespacePrefix)]
[assembly: XmlnsDefinition(XmlNamespaceConstants.DemoBaseGridXmlNamespaceDefinition, XmlNamespaceConstants.DemoBaseDemosHelpersGridNamespace)]
[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.FileVersion)]
[assembly: NeutralResourcesLanguage("en-us")]
#if DEBUGTEST
[assembly: InternalsVisibleTo("DevExpress.Xpf.DemoBase.HeavyTests, PublicKey=" + AssemblyInfo.PublicKey)]
#endif
