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

using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Mvvm.Xpf;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace DevExpress.Xpf.Grid {
	public class DialogEditFormBehavior : Behavior<GridControl> {
		public ICommand<EditFormRowValidationArgs> ValidateRowCommand {
			get { return (ICommand<EditFormRowValidationArgs>)GetValue(ValidateRowCommandProperty); }
			set { SetValue(ValidateRowCommandProperty, value); }
		}
		public static readonly DependencyProperty ValidateRowCommandProperty =
			DependencyProperty.Register(nameof(ValidateRowCommand), typeof(ICommand<EditFormRowValidationArgs>), typeof(DialogEditFormBehavior), new PropertyMetadata(null));
		public ICommand<EditFormValidateRowDeletionArgs> ValidateRowDeletionCommand {
			get { return (ICommand<EditFormValidateRowDeletionArgs>)GetValue(ValidateRowDeletionCommandProperty); }
			set { SetValue(ValidateRowDeletionCommandProperty, value); }
		}
		public static readonly DependencyProperty ValidateRowDeletionCommandProperty =
			DependencyProperty.Register(nameof(ValidateRowDeletionCommand), typeof(ICommand<EditFormValidateRowDeletionArgs>), typeof(DialogEditFormBehavior), new PropertyMetadata(null));
		public ICommand<CreateEditItemViewModelArgs> CreateEditItemViewModelCommand {
			get { return (ICommand<CreateEditItemViewModelArgs>)GetValue(CreateEditItemViewModelCommandProperty); }
			set { SetValue(CreateEditItemViewModelCommandProperty, value); }
		}
		public static readonly DependencyProperty CreateEditItemViewModelCommandProperty =
			DependencyProperty.Register(nameof(CreateEditItemViewModelCommand), typeof(ICommand<CreateEditItemViewModelArgs>), typeof(DialogEditFormBehavior), new PropertyMetadata(null));
		public bool AllowCancelAsyncOperations {
			get { return (bool)GetValue(AllowCancelAsyncOperationsProperty); }
			set { SetValue(AllowCancelAsyncOperationsProperty, value); }
		}
		public static readonly DependencyProperty AllowCancelAsyncOperationsProperty =
			DependencyProperty.Register(nameof(AllowCancelAsyncOperationsProperty), typeof(bool), typeof(DialogEditFormBehavior), new PropertyMetadata(false));
		public event EventHandler<EditFormRowValidationArgs> ValidateRow;
		public event EventHandler<EditFormValidateRowDeletionArgs> ValidateRowDeletion;
		public event EventHandler<CreateEditItemViewModelArgs> CreateEditItemViewModel;
		SplashScreenManager splashScreenManager;
		SplashScreenManager SplashScreenManager {
			get { return splashScreenManager ?? (splashScreenManager = SplashScreenManager.CreateWaitIndicator()); }
		}
#if DEBUGTEST
		public bool IsBusyForTests => isBusy;
		public bool SkipSplashScreenForTests { get; set; }
		public int SplashScreenShownCount { get; set; }
		public int SplashScreenClosedCount { get; set; }
		public IDialogService DialogServiceForTests { get; set; }
		public IMessageBoxService MessageBoxServiceForTests { get; set; }
		public void RaiseValidateRowForTests(EditFormRowValidationArgs e) => ValidateRow(this, e);
		public void RaiseValidateRowDeletionForTests(EditFormValidateRowDeletionArgs e) => ValidateRowDeletion(this, e);
		public void RaiseCreateEditItemViewModelForTests(CreateEditItemViewModelArgs e) => CreateEditItemViewModel(this, e);
#endif
		public DataTemplate EditTemplate {
			get { return (DataTemplate)GetValue(EditTemplateProperty); }
			set { SetValue(EditTemplateProperty, value); }
		}
		public static readonly DependencyProperty EditTemplateProperty =
			DependencyProperty.Register(nameof(EditTemplate), typeof(DataTemplate), typeof(DialogEditFormBehavior), new PropertyMetadata(null));
		public string KeyProperty {
			get { return (string)GetValue(KeyPropertyProperty); }
			set { SetValue(KeyPropertyProperty, value); }
		}
		public static readonly DependencyProperty KeyPropertyProperty =
			DependencyProperty.Register(nameof(KeyProperty), typeof(string), typeof(DialogEditFormBehavior), new PropertyMetadata(null));
		public ICommand CreateCommand { get; }
		public ICommand UpdateCommand { get; }
		public ICommand DeleteCommand { get; }
		public ICommand<RowClickArgs> RowDoubleClickCommand { get; }
		public DialogEditFormBehavior() {
			CreateCommand = new DelegateCommand(DoCreate, CanCreate);
			UpdateCommand = new DelegateCommand(DoUpdate, CanUpdate);
			DeleteCommand = new DelegateCommand(DoDelete, CanDelete);
			RowDoubleClickCommand = new DelegateCommand<RowClickArgs>(args => DoUpdate(args.SourceIndex));
		}
		void DoUpdate(int listIndex) {
			UpdateCore(GetRowKey(AssociatedObject.GetRowHandleByListIndex(listIndex)));
		}
		void DoUpdate() {
			UpdateCore(GetFocusedRowKey());
		}
		object[] GetSelectedRowsKeys() {
			return AssociatedObject.DataController.Selection.GetSelectedRows().Select(GetRowKey).ToArray();
		}
		object GetFocusedRowKey() {
			return GetRowKey(AssociatedObject.View.FocusedRowHandle);
		}
		object GetRowKey(int rowHandle) {
			return AssociatedObject.GetCellValue(rowHandle, KeyProperty);
		}
		async Task<bool> RaiseEventWithErrorHandling<T>(Action<object, T> eventAction, ICommand<T> command, T args, Func<T, Task> getTask) {
			try {
				await RaiseEvent(eventAction, command, args, getTask);
				return true;
			} catch(Exception e) {
				ShowError(e);
				return false;
			}
		}
		async Task RaiseEvent<T>(Action<object, T> eventAction, ICommand<T> command, T args, Func<T, Task> getTask) {
			command?.Execute(args);
			eventAction(this, args);
			var task = getTask(args);
			if(task != null) {
				isBusy = true;
				ShowSplashScreen();
				try {
					await task;
				} finally {
					CloseSplashScreen();
					isBusy = false;
				}
			}
		}
		void ShowSplashScreen() {
#if DEBUGTEST
			SplashScreenShownCount++;
			if(!SkipSplashScreenForTests)
#endif
				SplashScreenManager.Show(200, 200, AssociatedObject, inputBlock: InputBlockMode.Owner);
		}
		void CloseSplashScreen() {
#if DEBUGTEST
			SplashScreenClosedCount++;
			if(!SkipSplashScreenForTests)
#endif
				SplashScreenManager.Close();
		}
		bool isBusy;
		async void UpdateCore(object key) {
			IDialogService dialogService = null;
			DialogService realDialogService = null;
#if DEBUGTEST
			if(DialogServiceForTests != null) {
				dialogService = DialogServiceForTests;
			}
#endif
			AsyncCommand<CancelEventArgs> asyncCommand = null;
			if(dialogService == null) {
				realDialogService = GetDialogService(() => !AllowCancelAsyncOperations && asyncCommand.IsExecuting);
				dialogService = realDialogService;
				Interaction.GetBehaviors(AssociatedObject).Add(realDialogService);
			}
			var createArgs = new CreateEditItemViewModelArgs(key);
			await RaiseEvent(
				(o, e) => CreateEditItemViewModel?.Invoke(o, e), CreateEditItemViewModelCommand,
				createArgs,
				async a => {
				   if(createArgs.GetViewModelAsync != null) {
					   createArgs.ViewModel = await createArgs.GetViewModelAsync;
				   }
			   }
			);
			var commands = new UICommand[2];
			asyncCommand = new AsyncCommand<CancelEventArgs>(async cancelArgs => {
				var validateArgs = AllowCancelAsyncOperations 
					? new EditFormRowValidationArgs(createArgs.ViewModel.Item, createArgs.IsNewItem, createArgs.ViewModel.EditOperationContext, asyncCommand.CancellationTokenSource.Token)
					: new EditFormRowValidationArgs(createArgs.ViewModel.Item, createArgs.IsNewItem, createArgs.ViewModel.EditOperationContext);
				bool success = await RaiseEventWithErrorHandling((o, e) => ValidateRow?.Invoke(o, e), ValidateRowCommand, validateArgs, a => a.ValidateAsync);
				cancelArgs.Cancel = !success;
				if(success) {
					Refresh();
					if(createArgs.IsNewItem) {
						var handle = await AssociatedObject.FindRowByValueAsync(
							KeyProperty,
							TypeDescriptor.GetProperties(createArgs.ViewModel.Item)[KeyProperty].GetValue(createArgs.ViewModel.Item)
						);
						AssociatedObject.View.FocusedRowHandle = handle;
					}
				}
			});
			var saveLabel = AssociatedObject.View.LocalizationDescriptor.GetValue(GridControlStringId.DialogEditFormBehavior_Save.ToString());
			var cancelLabel = AssociatedObject.View.LocalizationDescriptor.GetValue(GridControlStringId.DialogEditFormBehavior_Cancel.ToString());
			commands[0] = new UICommand(null, saveLabel, asyncCommand, isDefault: true, isCancel: false, asyncDisplayMode: AllowCancelAsyncOperations ? AsyncDisplayMode.WaitCancel : AsyncDisplayMode.Wait);
			var cancelCommand = new DelegateCommand<CancelEventArgs>(e => { }, e =>AllowCancelAsyncOperations || !asyncCommand.IsExecuting);
			commands[1] = new UICommand(null, cancelLabel, cancelCommand, isDefault: false, isCancel: true);
			var resultCommand = dialogService.ShowDialog(commands, createArgs.ViewModel.Title, createArgs.ViewModel);
			if((resultCommand == null || resultCommand == commands[1]) && AllowCancelAsyncOperations) 
				asyncCommand.CancellationTokenSource?.Cancel();
			createArgs.ViewModel.Dispose();
			if(realDialogService != null) {
				Interaction.GetBehaviors(AssociatedObject).Remove(realDialogService);
			}
		}
		private void ShowError(Exception e) {
			IMessageBoxService messageBoxService = new DXMessageBoxService();
#if DEBUGTEST
			if(MessageBoxServiceForTests != null) {
				messageBoxService = MessageBoxServiceForTests;
			}
#endif
			messageBoxService.Show(ExceptionMessageHelper.GetExceptionMessage(e), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		DialogService GetDialogService(Func<bool> cancelClosingFunc) {
			var dialogService = new DialogServiceWithCancel(cancelClosingFunc);
			dialogService.DialogWindowStartupLocation = WindowStartupLocation.CenterOwner;
			var style = new Style(typeof(Window));
			style.Setters.Add(new Setter(Window.SizeToContentProperty, SizeToContent.WidthAndHeight));
			style.Setters.Add(new Setter(Window.WindowStyleProperty, WindowStyle.ToolWindow));
			style.Setters.Add(new Setter(Window.ResizeModeProperty, ResizeMode.NoResize));
			style.Seal();
			dialogService.DialogStyle = style;
			dialogService.ViewTemplate = EditTemplate;
			return dialogService;
		}
		class DialogServiceWithCancel: DialogService {
			readonly Func<bool> cancelClosingFunc;
			public DialogServiceWithCancel(Func<bool> cancelClosingFunc) {
				this.cancelClosingFunc = cancelClosingFunc;
			}
			protected override void OnDialogWindowClosing(object sender, CancelEventArgs e) {
				base.OnDialogWindowClosing(sender, e);
				e.Cancel = cancelClosingFunc();
			}
		}
		async void Refresh() {
			await AssociatedObject.View.RefreshDataSourceAsync();
		}
		bool CanUpdate() {
			return !isBusy && CanCreate() && HasCurrentItem();
		}
		bool CanCreate() {
			return !isBusy && HasCreateHandler() && HasValidateHandler();
		}
		bool HasValidateHandler() {
			return ValidateRowCommand != null || ValidateRow != null;
		}
		bool HasCreateHandler() {
			return CreateEditItemViewModelCommand != null || CreateEditItemViewModel != null;
		}
		bool HasCurrentItem() {
			return AssociatedObject?.CurrentItem != null;
		}
		void DoCreate() {
			UpdateCore(null);
		}
		async void DoDelete() {
			bool multipleRowDeletion = AssociatedObject.SelectionMode == MultiSelectMode.Row || AssociatedObject.SelectionMode == MultiSelectMode.MultipleRow;
			int lastRowHandle = !multipleRowDeletion ? AssociatedObject.View.FocusedRowHandle : Math.Max(AssociatedObject.View.FocusedRowHandle, AssociatedObject.DataController.Selection.GetSelectedRows().Last());
			bool willRemoveLastRow = lastRowHandle == AssociatedObject.VisibleRowCount - 1;
			var deletionArgs = new EditFormValidateRowDeletionArgs(multipleRowDeletion ? GetSelectedRowsKeys() : new[] {GetFocusedRowKey()});
			if(!await RaiseEventWithErrorHandling((o, e) => ValidateRowDeletion?.Invoke(o, e), ValidateRowDeletionCommand, deletionArgs, a => a.ValidateAsync))
				return;
			Refresh();
			if(willRemoveLastRow)
				AssociatedObject.View.FocusedRowHandle = AssociatedObject.VisibleRowCount - 1;
		}
		bool CanDelete() {
			return !isBusy && HasCurrentItem() && (ValidateRowDeletion != null || ValidateRowDeletionCommand != null);
		}
	}
}
