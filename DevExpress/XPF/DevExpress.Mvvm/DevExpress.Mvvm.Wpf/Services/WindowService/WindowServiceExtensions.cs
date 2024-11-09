﻿#region Copyright (c) 2000-2024 Developer Express Inc.
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
using System.Windows;
namespace DevExpress.Mvvm {
	public static class WindowServiceExtensions {
		public static void Show(this IWindowService service, object viewModel) {
			VerifyService(service);
			service.Show(null, viewModel, null, null);
		}
		public static void Show(this IWindowService service, string documentType, object viewModel) {
			VerifyService(service);
			service.Show(documentType, viewModel, null, null);
		}
		public static void Show(this IWindowService service, string documentType, object parameter, object parentViewModel) {
			VerifyService(service);
			service.Show(documentType, null, parameter, parentViewModel);
		}
		internal static void VerifyService(IWindowService service) {
			if(service == null)
				throw new ArgumentNullException(nameof(service));
		}
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class WindowServicePlatformExtensions {
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetWindowState(this IWindowService service, WindowState state) {
			WindowServiceExtensions.VerifyService(service);
			service.WindowState = DXWindowStateConverter.ToDXWindowState(state);
		}
	}
}
