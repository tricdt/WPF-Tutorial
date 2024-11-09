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
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevExpress.Mvvm;
namespace DevExpress.Mvvm.ViewModel {
	public static class DocumentManagerServiceExtensions {
		public static IDocument ShowExistingEntityDocument<TEntity, TPrimaryKey>(this IDocumentManagerService documentManagerService, object parentViewModel, TPrimaryKey primaryKey, object parameter = null) {
			parameter = parameter ?? new ViewModelInitInfo {
				PrimaryKey = primaryKey
			};
			IDocument document = FindEntityDocument<TEntity, TPrimaryKey>(documentManagerService, primaryKey) ?? CreateDocument<TEntity>(documentManagerService, parameter, parentViewModel);
			if(document != null)
				document.Show();
			return document;
		}
		public static IDocument ShowNewEntityDocument<TEntity>(this IDocumentManagerService documentManagerService, object parentViewModel, Action<TEntity> newEntityInitializer = null) {
			IDocument document = CreateDocument<TEntity>(documentManagerService, newEntityInitializer ?? (x => DefaultEntityInitializer(x)), parentViewModel);
			if(document != null)
				document.Show();
			return document;
		}
		public static IDocument FindEntityDocument<TEntity, TPrimaryKey>(this IDocumentManagerService documentManagerService, TPrimaryKey primaryKey) {
			if(documentManagerService == null)
				return null;
			foreach(var document in documentManagerService.Documents) {
				ISingleObjectViewModel<TEntity, TPrimaryKey> entityViewModel = document.Content as ISingleObjectViewModel<TEntity, TPrimaryKey>;
				if(entityViewModel != null && object.Equals(entityViewModel.PrimaryKey, primaryKey))
					return document;
			}
			return null;
		}
		static void DefaultEntityInitializer<TEntity>(TEntity entity) { }
		public static IDocument CreateDocument<TEntity>(this IDocumentManagerService documentManagerService, object parameter, object parentViewModel) {
			if(documentManagerService == null)
				return null;
			var document = documentManagerService.CreateDocument(GetDocumentTypeName<TEntity>(), parameter, parentViewModel);
			document.Id = "_" + Guid.NewGuid().ToString().Replace('-', '_');
			document.DestroyOnClose = false;
			return document;
		}
		public static string GetDocumentTypeName<TEntity>() {
			return typeof(TEntity).Name + "View";
		}
	}
}
