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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using DevExpress.Mvvm.Internal;
namespace DevExpress.Mvvm {
	public interface ISupportLogicalLayout {
		bool CanSerialize { get; }
		IDocumentManagerService DocumentManagerService { get; }
		IEnumerable<object> LookupViewModels { get; }
	}
	public interface ISupportLogicalLayout<T> : ISupportLogicalLayout {
		T SaveState();
		void RestoreState(T state);
	}
	public static class LogicalLayoutSerializationHelper {
		[DataContract]
		class SerializedDocument {
			[DataMember]
			public string DocumentType { get; set; }
			[DataMember]
			public string ViewModelState { get; set; }
			[DataMember]
			public string ViewModelStateList { get; set; }
			[DataMember]
			public string DocumentId { get; set; }
			[DataMember]
			public string DocumentTitle { get; set; }
			[DataMember]
			public bool IsVisible { get; set; }
			[DataMember]
			public List<SerializedDocument> Children { get; set; }
			public SerializedDocument() { }
			public SerializedDocument(string documentType, string viewModelState, string viewModelStateList, string documentId, string documentTitle, bool isVisible) {
				DocumentType = documentType;
				ViewModelState = viewModelState;
				ViewModelStateList = viewModelStateList;
				DocumentId = documentId;
				DocumentTitle = documentTitle;
				IsVisible = isVisible;
				Children = new List<SerializedDocument>();
			}
		}
		static object InvokeInterfaceMethod(object obj, Type interfaceType, string name, params object[] arguments) {
			var map = obj.GetType().GetInterfaceMap(interfaceType);
			MethodInfo method = null;
			for(int i = 0; i < map.InterfaceMethods.Length; i++) {
				if(map.InterfaceMethods[i].Name == name) {
					method = map.TargetMethods[i];
				}
			}
			return method.Invoke(obj, arguments);
		}
		static object GetParent(object viewModel) {
			var typed = viewModel as ISupportParentViewModel;
			if(typed == null || typed == typed.ParentViewModel)
				return null;
			return typed.ParentViewModel;
		}
		class LogicalNode {
			ISupportLogicalLayout primaryViewModel;
			public LogicalNode(LogicalNode parent, IDocument document, IDocumentManagerService service, ISupportLogicalLayout primaryViewModel = null) {
				Parent = parent;
				Document = document;
				Service = service;
				Children = new List<LogicalNode>();
				this.primaryViewModel = primaryViewModel;
			}
			public LogicalNode Parent { get; private set; }
			public IDocument Document { get; private set; }
			public IDocumentManagerService Service { get; private set; }
			public List<LogicalNode> Children { get; private set; }
			IDocumentInfo DocumentInfo { get { return Document as IDocumentInfo; } }
			public string DocumentType {
				get {
					return DocumentInfo == null ? null : DocumentInfo.DocumentType;
				}
			}
			public void Cull() {
				if(Parent != null) {
					Parent.Children.Remove(this);
				}
			}
			public bool IsVisible {
				get {
					return DocumentInfo != null && DocumentInfo.State == DocumentState.Visible;
				}
			}
			public ISupportLogicalLayout PrimaryViewModel {
				get { return primaryViewModel ?? Document.Content as ISupportLogicalLayout; }
			}
		}
		static void DepthFirstSearch(LogicalNode tree, Action<LogicalNode> action) {
			tree.Children.ForEach(c => DepthFirstSearch(c, action));
			action(tree);
		}
		static IEnumerable<LogicalNode> GetPath(LogicalNode node) {
			while(node != null) {
				yield return node;
				node = node.Parent;
			}
		}
		static LogicalNode BuildTree(IDocument document, ISupportLogicalLayout primaryViewModel, LogicalNode parent = null, bool includeNonSerializable = false) {
			var viewModels = new List<object>();
			viewModels.Add(primaryViewModel);
			if(primaryViewModel.LookupViewModels != null) {
				viewModels.AddRange(primaryViewModel.LookupViewModels);
			}
			var node = new LogicalNode(parent, document, primaryViewModel.DocumentManagerService, primaryViewModel);
			var primaryViewModelPath = GetPath(node).Skip(1).Select(n => n.PrimaryViewModel).ToList();
			if(!primaryViewModelPath.Contains(primaryViewModel)) { 
				node.Children.AddRange(
					from childDoc in GetImmediateChildren(primaryViewModel, viewModels)
					let childViewModel = childDoc.Content as ISupportLogicalLayout
					where childDoc != document
					where childViewModel != null
					where includeNonSerializable || childViewModel.CanSerialize
					select BuildTree(childDoc, childViewModel, node, includeNonSerializable));
			}
			return node;
		}
		static SerializedDocument SerializeTree(LogicalNode node) {
			var state = SerializeLogicalLayout(node.PrimaryViewModel);
			var id = node.Document != null ? node.Document.Id as string : "";
			var title = node.Document != null ? node.Document.Title as string : "";
			var serialized = new SerializedDocument(node.DocumentType, null, state, id, title, node.IsVisible);
			serialized.Children.AddRange(node.Children.Select(SerializeTree).Where(child => child != null));
			return serialized;
		}
		public static List<IDocument> GetOrphanedDocuments(this ISupportLogicalLayout viewModel) {
			return TrimLogicalTree(BuildTree(null, viewModel, null, true), viewModel).Select(n => n.Document).ToList();
		}
		static List<LogicalNode> GetOrphanedLeafs(LogicalNode root, ISupportLogicalLayout viewModel) {
			var orphans = new List<LogicalNode>();
			VisitOrphans(root, n => {
				if(n.Document != null) {
					orphans.Add(n);
				}
			});
			return orphans;
		}
		static List<LogicalNode> TrimLogicalTree(LogicalNode root, ISupportLogicalLayout viewModel) {
			var allOrphans = new List<LogicalNode>();
			List<LogicalNode> orphans;
			while((orphans = GetOrphanedLeafs(root, viewModel)).Count != 0) {
				allOrphans.AddRange(orphans);
				orphans.ForEach(n => n.Cull());
			}
			return allOrphans;
		}
		static List<SerializedDocument> SerializeViews(ISupportLogicalLayout viewModel) {
			LogicalNode tree = BuildTree(null, viewModel);
			TrimLogicalTree(tree, viewModel);
			return new[] { SerializeTree(tree) }.ToList();
		}
		static void VisitOrphans(LogicalNode tree, Action<LogicalNode> action) {
			DepthFirstSearch(tree, n => {
				if(!n.IsVisible && n.Children.Count == 0) {
					action(n);
				}
			});
		}
		static string SerializeLogicalLayout(object content) {
			var types = GetISupportLogicalLayout(content);
			var list = new List<string>();
			foreach(var type in types) {
				object objState = InvokeInterfaceMethod(content, type, "SaveState");
				list.Add(XmlSerializerHelper.Serialize(objState, type.GetGenericArguments().Single()));
			}
			return XmlSerializerHelper.Serialize(list, typeof(List<string>));
		}
		static void Deserialize(object content, string state, string stateList) {
			var types = GetISupportLogicalLayout(content);
			List<string> states;
			states = stateList != null ? XmlSerializerHelper.Deserialize<List<string>>(stateList) : new List<string> { state };
			if(types.Length != states.Count)
				return;
			for(int i = 0; i < types.Length; i++) {
				Deserialize(content, types[i], states[i]);
			}
		}
		static Type[] GetISupportLogicalLayout(object content) {
			return content?.GetType().GetInterfaces().Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ISupportLogicalLayout<>)).ToArray();
		}
		public static IEnumerable<IDocument> GetImmediateChildren(this ISupportLogicalLayout parent, IEnumerable<object> viewModels = null) {
			if(parent == null)
				yield break;
			var service = parent.DocumentManagerService;
			if(viewModels == null) {
				viewModels = new List<object> { parent };
			}
			if(parent.DocumentManagerService == null)
				yield break;
			foreach(var document in service.Documents) {
				var viewModel = document.Content as ISupportParentViewModel;
				if(viewModel != null && viewModels.Contains(viewModel.ParentViewModel)) {
					yield return document;
				}
			}
		}
		[Obsolete("Use the RestoreDocumentManagerService extension method instead.")]
		public static void RestoreDocumentManagerService(string state, ISupportLogicalLayout parent) {
			parent.RestoreDocumentManagerService(state);
		}
		public static void RestoreDocumentManagerService(this ISupportLogicalLayout parent, string state) {
			var list = DataContractSerializerHelper.Deserialize<List<SerializedDocument>>(state);
			if(list == null)
				return;
			RestoreDocumentManagerService(list, parent);
		}
		public static string SerializeDocumentManagerService(this ISupportLogicalLayout viewModel) {
			if(viewModel == null || viewModel.DocumentManagerService == null)
				return string.Empty;
			var views = SerializeViews(viewModel);
			return DataContractSerializerHelper.Serialize(views);
		}
		static void RestoreDocumentManagerService(List<SerializedDocument> children, ISupportLogicalLayout rootViewModel) {
			if(children.Count == 1) {
				var child = children.First();
				if(child.DocumentType == null && child.Children != null) {
					children = child.Children;
					Deserialize(rootViewModel, child.ViewModelState, child.ViewModelStateList);
				}
			}
			foreach(var document in GetImmediateChildren(rootViewModel).ToList()) {
				document.Close();
			}
			foreach(var child in children) {
				IDocument document = rootViewModel.DocumentManagerService.CreateDocument(child.DocumentType, null, rootViewModel);
				document.Id = child.DocumentId;
				document.Title = child.DocumentTitle;
				if(child.IsVisible) {
					document.DestroyOnClose = false;
					document.Show();
				}
				var viewModel = document.Content as ISupportLogicalLayout;
				if(viewModel != null) {
					Deserialize(document.Content, child.ViewModelState, child.ViewModelStateList);
					RestoreDocumentManagerService(child.Children, viewModel);
				}
			}
		}
		public static string Serialize(this ISupportLogicalLayout content) {
			return SerializeLogicalLayout(content);
		}
		static void Deserialize(object content, Type logicalLayoutType, string state) {
			var deserialized = XmlSerializerHelper.Deserialize(state, logicalLayoutType.GetGenericArguments().Single());
			InvokeInterfaceMethod(content, logicalLayoutType, "RestoreState", deserialized);
		}
		[DataContract]
		class StringPair {
			[DataMember]
			public string Key { get; set; }
			[DataMember]
			public string Value { get; set; }
		}
		public static string Serialize(Dictionary<string, string> dictionary) {
			var list = dictionary.Select(p => new StringPair { Key = p.Key, Value = p.Value }).ToList();
			return DataContractSerializerHelper.Serialize(list);
		}
		public static Dictionary<string, string> Deserialize(string serialized) {
			var res = DataContractSerializerHelper.Deserialize<List<StringPair>>(serialized);
			if(res == null)
				return new Dictionary<string, string>();
			return res.ToDictionary(p => p.Key, p => p.Value);
		}
	}
}
