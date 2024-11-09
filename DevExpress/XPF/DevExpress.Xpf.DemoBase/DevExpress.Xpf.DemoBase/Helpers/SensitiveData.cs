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
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Navigation;
using DevExpress.Utils;
namespace DevExpress.Xpf.DemoBase.Utils.Security {
	public abstract class SensitiveData : IEquatable<SensitiveData> {
		public static SensitiveData CreateForCurrentUser() {
			return new CurrentUserSensitiveData();
		}
		public static SensitiveData CreateForLocalMachine() {
			return new LocalMachineSensitiveData();
		}
		readonly static byte[] DefaultEntropy = new byte[20];
		static SensitiveData() {
			using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
				rng.GetBytes(DefaultEntropy);
		}
		SensitiveData() { }
		byte[] protectedData;
		public string Text {
			get {
				if(protectedData == null)
					return null;
				byte[] plainText = ProtectedData.Unprotect(protectedData, GetEntropy(), GetScope());
				return System.Text.Encoding.UTF8.GetString(plainText);
			}
			set {
				if(string.IsNullOrEmpty(value))
					protectedData = null;
				else {
					byte[] plainText = System.Text.Encoding.UTF8.GetBytes(value);
					protectedData = ProtectedData.Protect(plainText, GetEntropy(), GetScope());
				}
			}
		}
		protected virtual byte[] GetEntropy() => DefaultEntropy;
		protected abstract DataProtectionScope GetScope();
		#region Equals
		public override int GetHashCode() {
			return protectedData == null ? 0 : HashCodeHelper.CalculateByteList(protectedData);
		}
		public override bool Equals(object obj) {
			if(ReferenceEquals(obj, null))
				return false;
			var str = obj as string;
			if(str != null)
				return Equals(str);
			return Equals(obj as SensitiveData);
		}
		public bool Equals(string other) {
			if(other == null)
				return protectedData == null;
			byte[] plainText = System.Text.Encoding.UTF8.GetBytes(other);
			var otherData = ProtectedData.Protect(plainText, GetEntropy(), GetScope());
			return Equals(otherData);
		}
		public bool Equals(SensitiveData other) {
			if(other == null)
				return false;
			if(ReferenceEquals(this, other))
				return true;
			return Equals(other.protectedData);
		}
		bool Equals(byte[] otherProtectedData) {
			if(protectedData == null && otherProtectedData == null)
				return true;
			if(protectedData == null || otherProtectedData == null)
				return false;
			return Enumerable.SequenceEqual(protectedData, otherProtectedData);
		}
		#endregion
		sealed class CurrentUserSensitiveData : SensitiveData {
			protected sealed override DataProtectionScope GetScope() {
				return DataProtectionScope.CurrentUser;
			}
		}
		sealed class LocalMachineSensitiveData : SensitiveData {
			protected sealed override DataProtectionScope GetScope() {
				return DataProtectionScope.LocalMachine;
			}
		}
	}
}
