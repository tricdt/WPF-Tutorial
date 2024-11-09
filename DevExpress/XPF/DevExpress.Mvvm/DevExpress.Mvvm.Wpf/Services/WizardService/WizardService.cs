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
namespace DevExpress.Mvvm {
	public interface IWizardService {
		object Current { get; }
		void GoBack(object param);
		void GoForward(object param);
		void Navigate(string viewType, object viewModel, object param, object parentViewModel);
	}
	public static class IWizardServiceExtensions {
		public static void GoBack(this IWizardService service) {
			VerifyService(service);
			service.GoBack(null);
		}
		public static void GoForward(this IWizardService service) {
			VerifyService(service);
			service.GoForward(null);
		}
		public static void Navigate(this IWizardService service, string viewType, object param = null, object parentViewModel = null) {
			NavigateCore(service, viewType, null, param, parentViewModel);
		}
		public static void Navigate(this IWizardService service, object viewModel, object param = null, object parentViewModel = null) {
			NavigateCore(service, null, viewModel, param, parentViewModel);
		}
		static void NavigateCore(IWizardService service, string viewType, object viewModel, object param, object parentViewModel) {
			VerifyService(service);
			service.Navigate(viewType, viewModel, param, parentViewModel);
		}
		static void VerifyService(IWizardService service) {
			if(service == null)
				throw new ArgumentNullException(nameof(service));
		}
	}
}
