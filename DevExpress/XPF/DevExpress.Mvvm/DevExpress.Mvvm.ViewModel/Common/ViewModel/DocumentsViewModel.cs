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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.Utils;
using DevExpress.Mvvm.DataModel;
using DevExpress.Mvvm.ViewModel;
namespace DevExpress.Mvvm.ViewModel {
	public abstract class DocumentsViewModel<TModule, TUnitOfWork> : ISupportLogicalLayout
		where TModule : ModuleDescription<TModule>
		where TUnitOfWork : IUnitOfWork {
		const string ViewLayoutName = "DocumentViewModel";
		protected readonly IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory;
		protected DocumentsViewModel(IUnitOfWorkFactory<TUnitOfWork> unitOfWorkFactory) {
			this.unitOfWorkFactory = unitOfWorkFactory;
			Modules = CreateModules().ToArray();
			foreach(var module in Modules)
				Messenger.Default.Register<NavigateMessage<TModule>>(this, module, x => Show(x.Token));
			Messenger.Default.Register<DestroyOrphanedDocumentsMessage>(this, x => DestroyOrphanedDocuments(x));
		}
		void DestroyOrphanedDocuments(DestroyOrphanedDocumentsMessage message) {
			var orphans = this.GetOrphanedDocuments().Except(this.GetImmediateChildren()).Where(x => message.ShouldProcess(x.Content));
			foreach(var orphan in orphans) {
				orphan.DestroyOnClose = true;
				orphan.Close();
			}
		}
		public TModule[] Modules { get; private set; }
		public virtual TModule SelectedModule { get; set; }
		public virtual TModule ActiveModule { get; protected set; }
		public void SaveAll() {
			Messenger.Default.Send(new SaveAllMessage());
		}
		public virtual void OnClosing(CancelEventArgs cancelEventArgs) {
			SaveLogicalLayout();
			if(LayoutSerializationService != null) {
				PersistentLayoutHelper.PersistentViewsLayout[ViewLayoutName] = LayoutSerializationService.Serialize();
			}
			Messenger.Default.Send(new CloseAllMessage(cancelEventArgs, vm => true));
			PersistentLayoutHelper.SaveLayout();
		}
		public virtual NavigationPaneVisibility NavigationPaneVisibility { get; set; }
		public void Show(TModule module) {
			ShowCore(module);
		}
		public IDocument ShowCore(TModule module) {
			if(module == null || DocumentManagerService == null)
				return null;
			IDocument document = DocumentManagerService.FindDocumentByIdOrCreate(module.DocumentType, x => CreateDocument(module));
			document.Show();
			return document;
		}
		public void PinPeekCollectionView(TModule module) {
			if(WorkspaceDocumentManagerService == null)
				return;
			IDocument document = WorkspaceDocumentManagerService.FindDocumentByIdOrCreate(module.DocumentType, x => CreatePinnedPeekCollectionDocument(module));
			document.Show();
		}
		public virtual void OnLoaded(TModule module) {
			IsLoaded = true;
			DocumentManagerService.ActiveDocumentChanged += OnActiveDocumentChanged;
			if(!RestoreLogicalLayout()) {
				Show(module);
			}
			PersistentLayoutHelper.TryDeserializeLayout(LayoutSerializationService, ViewLayoutName);
		}
		bool documentChanging = false;
		void OnActiveDocumentChanged(object sender, ActiveDocumentChangedEventArgs e) {
			if(e.NewDocument == null) {
				ActiveModule = null;
			} else {
				documentChanging = true;
				ActiveModule = Modules.FirstOrDefault(m => m.DocumentType == (string)e.NewDocument.Id);
				documentChanging = false;
			}
		}
		protected virtual IDocumentManagerService DocumentManagerService { get { return this.GetService<IDocumentManagerService>(); } }
		protected ILayoutSerializationService LayoutSerializationService { get { return this.GetService<ILayoutSerializationService>("RootLayoutSerializationService"); } }
		protected IDocumentManagerService WorkspaceDocumentManagerService { get { return this.GetService<IDocumentManagerService>("WorkspaceDocumentManagerService"); } }
		public virtual TModule DefaultModule { get { return Modules.First(); } }
		protected bool IsLoaded { get; private set; }
		protected virtual void OnSelectedModuleChanged(TModule oldModule) {
			if(IsLoaded && !documentChanging)
				Show(SelectedModule);
		}
		protected virtual void OnActiveModuleChanged(TModule oldModule) {
			SelectedModule = ActiveModule;
		}
		IDocument CreateDocument(TModule module) {
			var document = DocumentManagerService.CreateDocument(module.DocumentType, null, this);
			document.Title = GetModuleTitle(module);
			document.DestroyOnClose = false;
			return document;
		}
		protected virtual string GetModuleTitle(TModule module) {
			return module.ModuleTitle;
		}
		IDocument CreatePinnedPeekCollectionDocument(TModule module) {
			var document = WorkspaceDocumentManagerService.CreateDocument("PeekCollectionView", module.CreatePeekCollectionViewModel());
			document.Title = module.ModuleTitle;
			return document;
		}
		protected Func<TModule, object> GetPeekCollectionViewModelFactory<TEntity, TPrimaryKey>(Func<TUnitOfWork, IRepository<TEntity, TPrimaryKey>> getRepositoryFunc) where TEntity : class {
			return module => PeekCollectionViewModel<TModule, TEntity, TPrimaryKey, TUnitOfWork>.Create(module, unitOfWorkFactory, getRepositoryFunc).SetParentViewModel(this);
		}
		protected abstract TModule[] CreateModules();
		protected TUnitOfWork CreateUnitOfWork() {
			return unitOfWorkFactory.CreateUnitOfWork();
		}
		public virtual void SaveLogicalLayout() {
			PersistentLayoutHelper.PersistentLogicalLayout = this.SerializeDocumentManagerService();
		}
		public virtual bool RestoreLogicalLayout() {
			if(string.IsNullOrEmpty(PersistentLayoutHelper.PersistentLogicalLayout))
				return false;
			this.RestoreDocumentManagerService(PersistentLayoutHelper.PersistentLogicalLayout);
			return true;
		}
		bool ISupportLogicalLayout.CanSerialize {
			get { return true; }
		}
		IDocumentManagerService ISupportLogicalLayout.DocumentManagerService {
			get { return DocumentManagerService; }
		}
		IEnumerable<object> ISupportLogicalLayout.LookupViewModels {
			get { return null; }
		}
	}
	public abstract partial class ModuleDescription<TModule> where TModule : ModuleDescription<TModule> {
		readonly Func<TModule, object> peekCollectionViewModelFactory;
		object peekCollectionViewModel;
		public ModuleDescription(string title, string documentType, string group, Func<TModule, object> peekCollectionViewModelFactory = null) {
			ModuleTitle = title;
			ModuleGroup = group;
			DocumentType = documentType;
			this.peekCollectionViewModelFactory = peekCollectionViewModelFactory;
		}
		public string ModuleTitle { get; private set; }
		public string ModuleGroup { get; private set; }
		public string DocumentType { get; private set; }
		public object PeekCollectionViewModel {
			get {
				if(peekCollectionViewModelFactory == null)
					return null;
				if(peekCollectionViewModel == null)
					peekCollectionViewModel = CreatePeekCollectionViewModel();
				return peekCollectionViewModel;
			}
		}
		public object CreatePeekCollectionViewModel() {
			return peekCollectionViewModelFactory((TModule)this);
		}
	}
	public enum NavigationPaneVisibility {
		Minimized,
		Normal,
		Off
	}
}
