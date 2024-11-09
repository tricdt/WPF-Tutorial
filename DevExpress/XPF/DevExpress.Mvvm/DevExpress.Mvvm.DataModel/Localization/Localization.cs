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

using System.Resources;
using DevExpress.Utils.Localization;
using DevExpress.Utils.Localization.Internal;
namespace DevExpress.Mvvm.Localization {
	public partial class ScaffoldingLocalizer : XtraLocalizer<ScaffoldingStringId> {
		public static new XtraLocalizer<ScaffoldingStringId> Active {
			get { return XtraLocalizer<ScaffoldingStringId>.Active; }
			set { XtraLocalizer<ScaffoldingStringId>.Active = value; }
		}  
		static ScaffoldingLocalizer() {
			SetActiveLocalizerProvider(new DefaultActiveLocalizerProvider<ScaffoldingStringId>(CreateDefaultLocalizer()));
		}
		static XtraLocalizer<ScaffoldingStringId> CreateDefaultLocalizer() {
			return new ScaffoldingResXLocalizer();
		}
		public override XtraLocalizer<ScaffoldingStringId> CreateResXLocalizer() {
			return new ScaffoldingResXLocalizer();
		}
		public static string GetString(ScaffoldingStringId id) {
			return Active.GetLocalizedString(id);
		}
		protected override void PopulateStringTable() {
			AddStrings();
		}
	}
	public class ScaffoldingResXLocalizer : XtraResXLocalizer<ScaffoldingStringId> {
		public ScaffoldingResXLocalizer()
			: base(new ScaffoldingLocalizer()) {
		}
		protected override ResourceManager CreateResourceManagerCore() {
			return CreateResourceManager(this, "DevExpress.Mvvm.DataModel.Localization.LocalizationRes", typeof(ScaffoldingResXLocalizer));
		}
	}
}
