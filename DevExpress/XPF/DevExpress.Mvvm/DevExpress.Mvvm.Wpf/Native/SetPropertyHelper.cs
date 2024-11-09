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
using System.Collections.Generic;
namespace DevExpress.Mvvm.Native {
#if !FREE && !NET_CORE
	public static class SetPropertyHelper {
		public static bool Set<T>(ref T storage, T value) {
			return SetCore(ref storage, value);
		}
		public static bool Set<T>(ref T storage, T value, Action changedCallback) {
			if(!SetCore(ref storage, value)) return false;
			changedCallback();
			return true;
		}
		public static bool Set<T>(ref T storage, T value, Action<T> changedCallback) {
			T oldValue = storage;
			if(!SetCore(ref storage, value)) return false;
			changedCallback(oldValue);
			return true;
		}
		public static bool Set<T>(ref T storage, T value, Action<T, T> changedCallback) {
			T oldValue = storage;
			if(!SetCore(ref storage, value)) return false;
			changedCallback(oldValue, value);
			return true;
		}
		static bool SetCore<T>(ref T storage, T value) {
			if(EqualityComparer<T>.Default.Equals(value, storage)) {
				return false;
			}
			storage = value;
			return true;
		}
	}
#endif
}
