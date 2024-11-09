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

using System.ComponentModel;
using System;
namespace DevExpress.Mvvm.Localization {
	#region enum ScaffoldingStringId
	public enum ScaffoldingStringId {
		Confirmation_Delete,
		Confirmation_Save,
		Confirmation_Reset,
		Confirmation_ResetLayout,
		Confirmation_Caption,
		Confirmation_SaveParent,
		Warning_Caption,
		Warning_SomeFieldsContainInvalidData,
		Exception_UpdateErrorCaption,
		Exception_ValidationErrorCaption,
		Exception_DataServiceRequestErrorCaption,
		Entity_Changed,
		Entity_New,
		Entity_Deleted,
		AddRemoveDetailEntities_SelectObjects,
	}
	#endregion
	#region ScaffoldingLocalizer 
	public partial class ScaffoldingLocalizer {
		public static string Confirmation_Delete { get { return Active.GetLocalizedString(ScaffoldingStringId.Confirmation_Delete); } }
		public static string Confirmation_Save { get { return Active.GetLocalizedString(ScaffoldingStringId.Confirmation_Save); } }
		public static string Confirmation_Reset { get { return Active.GetLocalizedString(ScaffoldingStringId.Confirmation_Reset); } }
		public static string Confirmation_ResetLayout { get { return Active.GetLocalizedString(ScaffoldingStringId.Confirmation_ResetLayout); } }
		public static string Confirmation_Caption { get { return Active.GetLocalizedString(ScaffoldingStringId.Confirmation_Caption); } }
		public static string Confirmation_SaveParent { get { return Active.GetLocalizedString(ScaffoldingStringId.Confirmation_SaveParent); } }
		public static string Warning_Caption { get { return Active.GetLocalizedString(ScaffoldingStringId.Warning_Caption); } }
		public static string Warning_SomeFieldsContainInvalidData { get { return Active.GetLocalizedString(ScaffoldingStringId.Warning_SomeFieldsContainInvalidData); } }
		public static string Exception_UpdateErrorCaption { get { return Active.GetLocalizedString(ScaffoldingStringId.Exception_UpdateErrorCaption); } }
		public static string Exception_ValidationErrorCaption { get { return Active.GetLocalizedString(ScaffoldingStringId.Exception_ValidationErrorCaption); } }
		public static string Exception_DataServiceRequestErrorCaption { get { return Active.GetLocalizedString(ScaffoldingStringId.Exception_DataServiceRequestErrorCaption); } }
		public static string Entity_Changed { get { return Active.GetLocalizedString(ScaffoldingStringId.Entity_Changed); } }
		public static string Entity_New { get { return Active.GetLocalizedString(ScaffoldingStringId.Entity_New); } }
		public static string Entity_Deleted { get { return Active.GetLocalizedString(ScaffoldingStringId.Entity_Deleted); } }
		public static string AddRemoveDetailEntities_SelectObjects { get { return Active.GetLocalizedString(ScaffoldingStringId.AddRemoveDetailEntities_SelectObjects); } }
		void AddStrings() {
			AddString(ScaffoldingStringId.Confirmation_Delete, "Do you want to delete this {0}?");
			AddString(ScaffoldingStringId.Confirmation_Save, "Do you want to save changes?");
			AddString(ScaffoldingStringId.Confirmation_Reset, "Are you sure you want to reset unsaved changes?");
			AddString(ScaffoldingStringId.Confirmation_ResetLayout, "Are you sure you want to reset layout? Reopen this view for the new layout to take effect.");
			AddString(ScaffoldingStringId.Confirmation_Caption, "Confirmation");
			AddString(ScaffoldingStringId.Confirmation_SaveParent, "You need to save the parent {0} entity before adding a new record. Do you want to save it now?");
			AddString(ScaffoldingStringId.Warning_Caption, "Warning");
			AddString(ScaffoldingStringId.Warning_SomeFieldsContainInvalidData, "Some fields contain invalid data. Click OK to close the page and lose unsaved changes. Press Cancel to continue editing data.");
			AddString(ScaffoldingStringId.Exception_UpdateErrorCaption, "Update Error");
			AddString(ScaffoldingStringId.Exception_ValidationErrorCaption, "Validation Error");
			AddString(ScaffoldingStringId.Exception_DataServiceRequestErrorCaption, "DataService Request Error");
			AddString(ScaffoldingStringId.Entity_Changed, " *");
			AddString(ScaffoldingStringId.Entity_New, " (New)");
			AddString(ScaffoldingStringId.Entity_Deleted, " (Deleted)");
			AddString(ScaffoldingStringId.AddRemoveDetailEntities_SelectObjects, "Select objects to add");
		}
	}
	 #endregion
}
