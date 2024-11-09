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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
namespace DevExpress.Mvvm.Native {
	public abstract class FreezableBase : BindableBase {
		public static void ThrowCannotModifyFrozenObject() {
			throw new InvalidOperationException("Cannot modify a frozen object.");
		}
		public bool IsFrozen { get; private set; }
		public void Freeze() {
			if(IsFrozen) return;
			FreezeCore();
			IsFrozen = true;
			RaisePropertyChanged(nameof(IsFrozen));
		}
		protected abstract void FreezeCore();
		protected override void VerifyAccess() {
			if(IsFrozen)
				ThrowCannotModifyFrozenObject();
		}
		public class FreezableCollectionBase<T> : ObservableCollection<T> where T : FreezableBase {
			public FreezableCollectionBase() : base() { }
			public FreezableCollectionBase(List<T> list) : base(list) { }
			public FreezableCollectionBase(IEnumerable<T> collection) : base(collection) { }
			public bool IsFrozen { get; private set; }
			public void Freeze() {
				if(IsFrozen) return;
				FreezeCore();
				IsFrozen = true;
			}
			void FreezeCore() {
				foreach(var child in this)
					child.Freeze();
			}
			protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
				if(IsFrozen)
					FreezableBase.ThrowCannotModifyFrozenObject();
				base.OnCollectionChanged(e);
			}
		}
	}
}
