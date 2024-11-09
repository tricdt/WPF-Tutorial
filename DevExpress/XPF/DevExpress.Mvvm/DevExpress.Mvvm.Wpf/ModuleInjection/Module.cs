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
namespace DevExpress.Mvvm.ModuleInjection {
	public interface IModule {
		string Key { get; }
		Func<object> ViewModelFactory { get; }
		string ViewModelName { get; }
		string ViewName { get; }
		Type ViewType { get; }
	}
	public class Module : IModule {
		public string Key { get; private set; }
		public Func<object> ViewModelFactory { get; private set; }
		public string ViewModelName { get; private set; }
		public string ViewName { get; private set; }
		public Type ViewType { get; private set; }
		public Module(string key, Func<object> viewModelFactory)
			: this(key, viewModelFactory, null, null, null) {
			Verifier.VerifyViewModelFactory(viewModelFactory);
		}
		public Module(string key, Func<object> viewModelFactory, string viewName)
			: this(key, viewModelFactory, null, viewName, null) {
			Verifier.VerifyViewModelFactory(viewModelFactory);
		}
		public Module(string key, Func<object> viewModelFactory, Type viewType)
			: this(key, viewModelFactory, null, null, viewType) {
			Verifier.VerifyViewModelFactory(viewModelFactory);
		}
		public Module(string key, string viewModelName, string viewName)
			: this(key, null, viewModelName, viewName, null) {
			Verifier.VerifyViewModelName(viewModelName);
		}
		Module(string key, Func<object> viewModelFactory, string viewModelName, string viewName, Type viewType) {
			Verifier.VerifyKey(key);
			Key = key;
			ViewModelFactory = viewModelFactory;
			ViewModelName = viewModelName;
			ViewName = viewName;
			ViewType = viewType;
		}
	}
}
