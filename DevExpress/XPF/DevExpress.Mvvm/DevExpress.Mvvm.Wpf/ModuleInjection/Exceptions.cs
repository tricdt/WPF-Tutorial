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
using System.Text.RegularExpressions;
namespace DevExpress.Mvvm.ModuleInjection {
	public class ModuleInjectionException : Exception {
		const string moduleMissing = "Cannot find a module with the passed key. Register module before working with it.";
		const string moduleExists = "A module with the same key already exists.";
		const string cannotAttach = "This service can be only attached to a FrameworkElement or FrameworkContentElement";
		const string noStrategy = "Cannot find an appropriate strategy for the {0} container type.";
		const string nullVM = "A view model to inject cannot be null.";
		const string vmNotSupportServices = "This ViewModel does not implement the ISupportServices interface.";
		const string vmNotSupportParameter = "This ViewModel does not implement the ISupportParameter interface.";
		const string visualStateServiceName = "VisualStateService with the same Name already exists. If you are using several VisualStateServices in one View, be sure that they have different names.";
		const string cannotResolveVM = "Cannot create a view model instance by the {0} type name. Setup ViewModelLocator.";
		public static void ModuleMissing(string regionName, string key) {
			throw new ModuleInjectionException(regionName, key, moduleMissing);
		}
		public static void ModuleAlreadyExists(string regionName, string key) {
			throw new ModuleInjectionException(regionName, key, moduleExists);
		}
		public static void NullVM() {
			throw new ModuleInjectionException(null, null, nullVM);
		}
		public static void VMNotSupportServices() {
			throw new ModuleInjectionException(null, null, vmNotSupportServices);
		}
		public static void VMNotSupportParameter() {
			throw new ModuleInjectionException(null, null, vmNotSupportParameter);
		}
		public static void VisualStateServiceName() {
			throw new ModuleInjectionException(null, null, visualStateServiceName);
		}
		public static void CannotResolveVM(string vmName) {
			throw new ModuleInjectionException(null, null, string.Format(cannotResolveVM, vmName));
		}
		public static void CannotAttach() {
			throw new ModuleInjectionException(null, null, cannotAttach);
		}
		public static void NoStrategy(Type containerType) {
			throw new ModuleInjectionException(null, null, string.Format(noStrategy, containerType.Name));
		}
		public string Key { get; private set; }
		public string RegionName { get; private set; }
		ModuleInjectionException(string regionName, string key, string message) : base(message) {
			RegionName = regionName;
		}
	}
	static class Verifier {
		public static void VerifyManager(IModuleManagerBase manager) {
			if(manager == null) throw new ArgumentNullException(nameof(manager));
		}
		public static void VerifyRegionName(string regionName) {
			if(string.IsNullOrEmpty(regionName)) throw new ArgumentNullException(nameof(regionName));
		}
		public static void VerifyKey(string key) {
			if(string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
		}
		public static void VerifyModule(IModule module) {
			if(module == null) throw new ArgumentNullException(nameof(module));
		}
		public static void VerifyViewModel(object viewModel) {
			if(viewModel == null) throw new ArgumentNullException(nameof(viewModel));
		}
		public static void VerifyVisualSerializationService(IVisualStateService service) {
			if(service == null) throw new ArgumentNullException(nameof(service));
		}
		public static void VerifyViewModelFactory(Func<object> viewModelFactory) {
			if(viewModelFactory == null) throw new ArgumentNullException(nameof(viewModelFactory));
		}
		public static void VerifyViewModelName(string viewModelName) {
			if(string.IsNullOrEmpty(viewModelName)) throw new ArgumentNullException(nameof(viewModelName));
		}
		public static void VerifyViewModelISupportServices(object viewModel) {
			if(!(viewModel is ISupportServices))
				ModuleInjectionException.VMNotSupportServices();
		}
		public static void VerifyViewModelISupportParameter(object viewModel) {
			if(!(viewModel is ISupportParameter))
				ModuleInjectionException.VMNotSupportParameter();
		}
	}
}
