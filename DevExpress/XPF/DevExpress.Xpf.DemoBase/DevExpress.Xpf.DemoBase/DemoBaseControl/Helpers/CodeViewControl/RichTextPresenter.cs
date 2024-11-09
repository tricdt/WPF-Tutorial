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
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Data.Utils;
using DevExpress.Data.Utils.Clipboard.Wpf;
using DevExpress.Xpf.DemoBase.Helpers.TextColorizer.Internal;
using DevExpress.Xpf.DemoCenterBase;
using DevExpress.Xpf.Utils;
namespace DevExpress.Xpf.DemoBase.Helpers.Internal {
	[TemplatePart(Name = "InnerPresenter", Type = typeof(RichTextBox))]
	class RichTextPresenter : Control, IRichTextPresenter {
		public static readonly DependencyProperty TextWrappingProperty =
			DependencyPropertyManager.Register("TextWrapping", typeof(TextWrapping), typeof(RichTextPresenter), new PropertyMetadata(TextWrapping.Wrap));
		static RichTextPresenter() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextPresenter), new FrameworkPropertyMetadata(typeof(RichTextPresenter)));
		}
		List<Block> savedBlocks;
		public RichTextPresenter() {
			FocusHelper.SetFocusable(this, false);
			InnerPresenter = new RichTextBox() { Document = new FlowDocument(), IsUndoEnabled = false, UndoLimit = 0 };
		}
		public RichTextBox InnerPresenter { get; private set; }
		public TextWrapping TextWrapping { get { return (TextWrapping)GetValue(TextWrappingProperty); } set { SetValue(TextWrappingProperty, value); } }
		public TextPointer ContentStart {
			get {
				return InnerPresenter.Document.ContentStart;
			}
		}
		public TextPointer ContentEnd {
			get {
				return InnerPresenter.Document.ContentEnd;
			}
		}
		public void TextWidthMaxSet(double width) {
			InnerPresenter.MaxWidth = width;
		}
		public TextPointer SelectionStart {
			get { return InnerPresenter.Selection.Start; }
		}
		public TextPointer SelectionEnd {
			get { return InnerPresenter.Selection.End; }
		}
		public void Select(TextPointer start, TextPointer end) {
			InnerPresenter.Selection.Select(start, end);
		}
		public ICollection<Block> Blocks {
			get {
				if(savedBlocks != null) return savedBlocks;
				return InnerPresenter.Document.Blocks;
			}
		}
		public override void OnApplyTemplate() {
			base.OnApplyTemplate();
			SaveBlocks();
			InnerPresenter = (RichTextBox)GetTemplateChild("InnerPresenter");
			CommandManager.AddPreviewCanExecuteHandler(InnerPresenter, onPreviewCanExecute);
			CommandManager.AddPreviewExecutedHandler(InnerPresenter, onPreviewExecuted);
			RestoreBlocks();
		}
		void onPreviewCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			if(e.Command == ApplicationCommands.Copy) {
				e.CanExecute = true;
				e.Handled = true;
			}
		}
		void onPreviewExecuted(object sender, ExecutedRoutedEventArgs e) {
			if(e.Command != ApplicationCommands.Copy)
				return;
			e.Handled = true;
			CopyEventArgs args = new CopyEventArgs(InnerPresenter.Selection.Text);
			CopyTextToClipboard?.Invoke(this, args);
			DataObject data = new DataObject();
			data.SetData(DataFormats.UnicodeText, args.CopyText, false);
			var clipboardResult = SafeClipboardWpf.Instance.SetDataObject(data.ToPortable());
			if(!clipboardResult)
				new DefaultDemoRunnerMessageBox().Show("<Paragraph>The copy operation has failed. Another window might have the clipboard open.</Paragraph>", false, false);
		}
		void SaveBlocks() {
			if(InnerPresenter == null) return;
			savedBlocks = new List<Block>();
			foreach(Block block in InnerPresenter.Document.Blocks)
				savedBlocks.Add(block);
			InnerPresenter.Document.Blocks.Clear();
		}
		void RestoreBlocks() {
			if(savedBlocks == null || InnerPresenter == null) return;
			foreach(Block block in savedBlocks)
				InnerPresenter.Document.Blocks.Add(block);
			savedBlocks = null;
		}
		public event EventHandler<CopyEventArgs> CopyTextToClipboard;
	}
}
