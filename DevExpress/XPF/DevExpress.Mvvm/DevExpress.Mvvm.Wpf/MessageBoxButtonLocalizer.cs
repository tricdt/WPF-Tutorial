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

using DevExpress.Mvvm.Native;
using System.ComponentModel;
using System.Windows;
namespace DevExpress.Mvvm {
	public interface IMessageButtonLocalizer {
		string Localize(MessageResult button);
	}
#if !WINUI
	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
	public interface IMessageBoxButtonLocalizer {
		string Localize(MessageBoxResult button);
	}
#endif
	public class DefaultMessageButtonLocalizer : IMessageButtonLocalizer {
		public string Localize(MessageResult button) {
			switch(button) {
				case MessageResult.OK:
					return "OK";
				case MessageResult.Cancel:
					return "Cancel";
				case MessageResult.Yes:
					return "Yes";
				case MessageResult.No:
					return "No";
#if WINUI
				case MessageResult.Close:
					return "Close";
				case MessageResult.Ignore:
					return "Ignore";
				case MessageResult.Retry:
					return "Retry";
				case MessageResult.Abort:
					return "Abort";
#endif
			}
			return string.Empty;
		}
	}
#if !WINUI
	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
	public class DefaultMessageBoxButtonLocalizer : IMessageBoxButtonLocalizer {
		DefaultMessageButtonLocalizer localizer = new DefaultMessageButtonLocalizer();
		public string Localize(MessageBoxResult button) {
			return localizer.Localize(button.ToMessageResult());
		}
	}
	[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
	public static class MessageBoxButtonLocalizerExtensions {
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public static IMessageButtonLocalizer ToMessageButtonLocalizer(this IMessageBoxButtonLocalizer localizer) {
			return new MessageBoxButtonLocalizerWrapper(localizer);
		}
		class MessageBoxButtonLocalizerWrapper : IMessageButtonLocalizer {
			IMessageBoxButtonLocalizer localizer;
			public MessageBoxButtonLocalizerWrapper(IMessageBoxButtonLocalizer localizer) {
				this.localizer = localizer;
			}
			string IMessageButtonLocalizer.Localize(MessageResult button) {
				return localizer.Localize(button.ToMessageBoxResult());
			}
		}
	}
#endif
}
