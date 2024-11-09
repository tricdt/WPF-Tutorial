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
using System.ComponentModel;
using System.Windows;
#if !WINUI
using System.Windows.Controls;
#endif
using System.Windows.Input;
using DevExpress.Mvvm.Native;
namespace DevExpress.Mvvm {
	public enum DialogButtonAlignment {
		Right,
		Center,
		Left,
	}
	public enum AsyncDisplayMode {
		None,
		Wait,
		WaitCancel
	}
	public enum GlyphAlignment {
		Left,
		Top,
		Right,
		Bottom
	}
	public class UICommand : BindableBase, IUICommand {
		object id = null;
		public object Id {
			get { return id; }
			set { SetProperty(ref id, value, nameof(Id)); }
		}
		object caption = null;
		public object Caption {
			get { return caption; }
			set {
				SetProperty(ref caption, value, nameof(Caption)); }
		}
		ICommand command = null;
		public ICommand Command {
			get { return command; }
			set { SetProperty(ref command, value, nameof(Command)); }
		}
		bool isDefault = false;
		public bool IsDefault {
			get { return isDefault; }
			set { SetProperty(ref isDefault, value, nameof(IsDefault)); }
		}
		bool isCancel = false;
		public bool IsCancel {
			get { return isCancel; }
			set { SetProperty(ref isCancel, value, nameof(IsCancel)); }
		}
		object tag = null;
		public object Tag {
			get { return tag; }
			set { SetProperty(ref tag, value, nameof(Tag)); }
		}
		bool allowCloseWindow = true;
		public bool AllowCloseWindow {
			get {
				return  allowCloseWindow;
			}
			set { SetProperty(ref allowCloseWindow, value, nameof(AllowCloseWindow)); }
		}
		AsyncDisplayMode asyncDisplayMode = AsyncDisplayMode.None;
		public AsyncDisplayMode AsyncDisplayMode {
			get {
				return asyncDisplayMode;
			}
			set { SetProperty(ref asyncDisplayMode, value, nameof(AsyncDisplayMode)); }
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public DialogButtonAlignment ActualAlignment {
			get {
				if(alignment != DialogButtonAlignment.Right)
					return alignment;
#if !WINUI
				if(placement != Dock.Right && placement.Equals(Dock.Left))
					return DialogButtonAlignment.Left;
#endif
				return alignment;
			}
		}
		DialogButtonAlignment alignment = DialogButtonAlignment.Right;
		public DialogButtonAlignment Alignment {
			get { return alignment; }
			set { SetProperty(ref alignment, value, nameof(Alignment)); }
		}
		object glyph = null;
		public object Glyph {
			get { return glyph; }
			set { SetProperty(ref glyph, value, nameof(Glyph)); }
		}
		GlyphAlignment glyphAlignment = GlyphAlignment.Left;
		public GlyphAlignment GlyphAlignment {
			get { return glyphAlignment; }
			set { SetProperty(ref glyphAlignment, value, nameof(GlyphAlignment)); }
		}
#if !WINUI
		Dock placement = Dock.Right;
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public Dock Placement {
			get { return placement; }
			set { SetProperty(ref placement, value, nameof(Placement)); }
		}
#endif
		public UICommand() { }
#if !WINUI
		public UICommand(object id, object caption, ICommand command, bool isDefault, bool isCancel, object tag = null, bool allowCloseWindow = true, Dock placement = Dock.Right, DialogButtonAlignment alignment = DialogButtonAlignment.Right, AsyncDisplayMode asyncDisplayMode = AsyncDisplayMode.None, object glyph = null, GlyphAlignment glyphAlignment = GlyphAlignment.Left) {
			this.id = id;
			this.command = command;
			this.isDefault = isDefault;
			this.isCancel = isCancel;
			this.tag = tag;
			this.allowCloseWindow = allowCloseWindow;
			this.placement = placement;
			this.alignment = alignment;
			this.caption = caption;
			this.glyph = glyph;
			this.glyphAlignment = glyphAlignment;
			AsyncDisplayMode = asyncDisplayMode;
		}
		public UICommand(object id, object caption, ICommand<CancelEventArgs> command, bool isDefault, bool isCancel, object tag = null, bool allowCloseWindow = true, Dock placement = Dock.Right, DialogButtonAlignment alignment = DialogButtonAlignment.Right, AsyncDisplayMode asyncDisplayMode = AsyncDisplayMode.None, object glyph = null, GlyphAlignment glyphAlignment = GlyphAlignment.Left) 
			: this(id: id, 
				  caption: caption, 
				  command: (ICommand)command, 
				  isDefault: isDefault, 
				  isCancel: isCancel, 
				  tag: tag, 
				  allowCloseWindow: 
				  allowCloseWindow, 
				  placement: placement, 
				  alignment: alignment,
				  glyph: glyph,
				  glyphAlignment: glyphAlignment,
				  asyncDisplayMode: asyncDisplayMode) {
		}
#endif
#if WINUI
		public UICommand(object id, object caption, ICommand command, bool isDefault, bool isCancel, object tag = null, bool allowCloseWindow = true, DialogButtonAlignment alignment = DialogButtonAlignment.Right, object glyph = null, GlyphAlignment glyphAlignment = GlyphAlignment.Left) {
			this.id = id;
			this.command = command;
			this.isDefault = isDefault;
			this.isCancel = isCancel;
			this.tag = tag;
			this.allowCloseWindow = allowCloseWindow; 
			this.alignment = alignment;
			this.caption = caption;
			this.glyph = glyph;
			this.glyphAlignment = glyphAlignment;
		}
#endif
		public static List<UICommand> GenerateFromMessageButton(MessageButton dialogButtons, IMessageButtonLocalizer buttonLocalizer, MessageResult? defaultButton = null, MessageResult? cancelButton = null) {
			return GenerateFromMessageButton(dialogButtons, false, buttonLocalizer, defaultButton, cancelButton);
		}
#if !WINUI
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public static List<UICommand> GenerateFromMessageBoxButton(MessageBoxButton dialogButtons, IMessageBoxButtonLocalizer buttonLocalizer, MessageBoxResult? defaultButton = null, MessageBoxResult? cancelButton = null) {
			return GenerateFromMessageBoxButton(dialogButtons, buttonLocalizer.ToMessageButtonLocalizer(), defaultButton, cancelButton);
		}
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public static List<UICommand> GenerateFromMessageBoxButton(MessageBoxButton dialogButtons, IMessageButtonLocalizer buttonLocalizer, MessageBoxResult? defaultButton = null, MessageBoxResult? cancelButton = null) {
			MessageResult? defaultResult = defaultButton == null ? (MessageResult?)null : defaultButton.Value.ToMessageResult();
			MessageResult? cancelResult = cancelButton == null ? (MessageResult?)null : cancelButton.Value.ToMessageResult();
			return GenerateFromMessageButton(dialogButtons.ToMessageButton(), true, buttonLocalizer, defaultResult, cancelResult);
		}
#endif
		static List<UICommand> GenerateFromMessageButton(MessageButton dialogButtons, bool usePlatformSpecificTag, IMessageButtonLocalizer buttonLocalizer, MessageResult? defaultButton, MessageResult? cancelButton) {
			List<UICommand> commands = new List<UICommand>();
			if(dialogButtons == MessageButton.OK) {
				UICommand okCommand = CreateDefaultButtonCommand(MessageResult.OK, usePlatformSpecificTag, buttonLocalizer.Localize);
				okCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.OK;
				okCommand.IsCancel = cancelButton == MessageResult.OK;
				commands.Add(okCommand);
				return commands;
			}
			if(dialogButtons == MessageButton.OKCancel) {
				UICommand okCommand = CreateDefaultButtonCommand(MessageResult.OK, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand cancelCommand = CreateDefaultButtonCommand(MessageResult.Cancel, usePlatformSpecificTag, buttonLocalizer.Localize);
				okCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.OK;
				cancelCommand.IsDefault = defaultButton == MessageResult.Cancel;
				okCommand.IsCancel = cancelButton == MessageResult.OK;
				cancelCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.Cancel;
				commands.Add(okCommand);
				commands.Add(cancelCommand);
				return commands;
			}
			if(dialogButtons == MessageButton.YesNo) {
				UICommand yesCommand = CreateDefaultButtonCommand(MessageResult.Yes, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand noCommand = CreateDefaultButtonCommand(MessageResult.No, usePlatformSpecificTag, buttonLocalizer.Localize);
				yesCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.Yes;
				noCommand.IsDefault = defaultButton == MessageResult.No;
				yesCommand.IsCancel = cancelButton == MessageResult.Yes;
				noCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.No;
				commands.Add(yesCommand);
				commands.Add(noCommand);
				return commands;
			}
			if(dialogButtons == MessageButton.YesNoCancel) {
				UICommand yesCommand = CreateDefaultButtonCommand(MessageResult.Yes, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand noCommand = CreateDefaultButtonCommand(MessageResult.No, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand cancelCommand = CreateDefaultButtonCommand(MessageResult.Cancel, usePlatformSpecificTag, buttonLocalizer.Localize);
				yesCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.Yes;
				noCommand.IsDefault = defaultButton == MessageResult.No;
				cancelCommand.IsDefault = defaultButton == MessageResult.Cancel;
				yesCommand.IsCancel = cancelButton == MessageResult.Yes;
				noCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.No;
				cancelCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.Cancel;
				commands.Add(yesCommand);
				commands.Add(noCommand);
				commands.Add(cancelCommand);
				return commands;
			}
#if WINUI
			if(dialogButtons == MessageButton.AbortRetryIgnore) {
				UICommand abortCommand = CreateDefaultButtonCommand(MessageResult.Abort, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand retryCommand = CreateDefaultButtonCommand(MessageResult.Retry, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand ignoreCommand = CreateDefaultButtonCommand(MessageResult.Ignore, usePlatformSpecificTag, buttonLocalizer.Localize);
				abortCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.Abort;
				retryCommand.IsDefault = defaultButton == MessageResult.Retry;
				ignoreCommand.IsDefault = defaultButton == MessageResult.Ignore;
				abortCommand.IsCancel = cancelButton == MessageResult.Abort;
				retryCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.Retry;
				ignoreCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.Ignore;
				commands.Add(abortCommand);
				commands.Add(retryCommand);
				commands.Add(ignoreCommand);
				return commands;
			}
			if(dialogButtons == MessageButton.Close) {
				UICommand closeCommand = CreateDefaultButtonCommand(MessageResult.Close, usePlatformSpecificTag, buttonLocalizer.Localize);
				closeCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.Close;
				closeCommand.IsCancel = cancelButton == MessageResult.Close;
				commands.Add(closeCommand);
				return commands;
			}
			if(dialogButtons == MessageButton.RetryCancel) {
				UICommand retryCommand = CreateDefaultButtonCommand(MessageResult.Retry, usePlatformSpecificTag, buttonLocalizer.Localize);
				UICommand cancelCommand = CreateDefaultButtonCommand(MessageResult.Cancel, usePlatformSpecificTag, buttonLocalizer.Localize);
				retryCommand.IsDefault = defaultButton == null || defaultButton == MessageResult.Retry;
				cancelCommand.IsDefault = defaultButton == MessageResult.Cancel;
				retryCommand.IsCancel = cancelButton == MessageResult.Retry;
				cancelCommand.IsCancel = cancelButton == null || cancelButton == MessageResult.Cancel;
				commands.Add(retryCommand);
				commands.Add(cancelCommand);
				return commands;
			}
#endif
			return commands;
		}
		static UICommand CreateDefaultButtonCommand(MessageResult result, bool usePlatformSpecificTag, Func<MessageResult, string> getButtonCaption) {
#if !WINUI
			object tag = usePlatformSpecificTag ? result.ToMessageBoxResult() : (object)result;
#else
			object tag = (object)result;
#endif
			return new DefaultButtonCommand(tag, getButtonCaption(result), tag);
		}
		#region IUICommand
		EventHandler executed;
		event EventHandler IUICommand.Executed {
			add { executed += value; }
			remove { executed -= value; }
		}
		void IUICommand.RaiseExecuted() {
			if(executed != null)
				executed(this, EventArgs.Empty);
		}
		#endregion
		#region DefaultButtonCommand
		class DefaultButtonCommand : UICommand {
			public DefaultButtonCommand(object id, string caption, object tag) {
				this.id = id;
				this.tag = tag;
				this.caption = caption;
			}
		}
		#endregion DefaultButtonCommand
	}
}
