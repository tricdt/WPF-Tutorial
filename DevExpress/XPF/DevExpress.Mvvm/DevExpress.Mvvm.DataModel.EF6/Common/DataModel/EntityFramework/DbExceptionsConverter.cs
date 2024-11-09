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

using DevExpress.Mvvm.Localization;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
namespace DevExpress.Mvvm.DataModel.EF6 {
	public static class DbExceptionsConverter {
		public static DbException Convert(DbUpdateException exception) {
			Exception originalException = exception;
			while(originalException.InnerException != null) {
				originalException = originalException.InnerException;
			}
			return new DbException(originalException.Message, ScaffoldingLocalizer.GetString(ScaffoldingStringId.Exception_UpdateErrorCaption), exception);
		}
		public static DbException Convert(DbEntityValidationException exception) {
			StringBuilder stringBuilder = new StringBuilder();
			foreach(var validationResult in exception.EntityValidationErrors) {
				foreach(var error in validationResult.ValidationErrors) {
					if(stringBuilder.Length > 0)
						stringBuilder.AppendLine();
					stringBuilder.Append(error.PropertyName + ": " + error.ErrorMessage);
				}
			}
			return new DbException(stringBuilder.ToString(), ScaffoldingLocalizer.GetString(ScaffoldingStringId.Exception_ValidationErrorCaption), exception);
		}
	}
}
