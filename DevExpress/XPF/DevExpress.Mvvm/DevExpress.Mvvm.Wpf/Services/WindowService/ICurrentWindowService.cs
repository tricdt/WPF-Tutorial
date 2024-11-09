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
	public enum DXWindowState { Normal = 0, Minimized = 1, Maximized = 2 }
	public interface ICurrentWindowService {
		void Close();
		DXWindowState WindowState { get; set; }
		void Activate();
		void Hide();
		void Show();
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class CurrentWindowServicePlatformExtensions {
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void SetWindowState(this ICurrentWindowService service, WindowState state) {
			VerifyService(service);
			service.WindowState = DXWindowStateConverter.ToDXWindowState(state);
		}
		static void VerifyService(ICurrentWindowService service) {
			if(service == null)
				throw new ArgumentNullException(nameof(service));
		}
	}
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class DXWindowStateConverter {
		public static WindowState ToWindowState(this DXWindowState x) {
			switch(x) {
				case DXWindowState.Normal: return WindowState.Normal;
				case DXWindowState.Minimized: return WindowState.Minimized;
				case DXWindowState.Maximized: return WindowState.Maximized;
				default: throw new NotImplementedException();
			}
		}
		public static DXWindowState ToDXWindowState(this WindowState x) {
			switch(x) {
				case WindowState.Normal: return DXWindowState.Normal;
				case WindowState.Minimized: return DXWindowState.Minimized;
				case WindowState.Maximized: return DXWindowState.Maximized;
				default: throw new NotImplementedException();
			}
		}
	}
}
