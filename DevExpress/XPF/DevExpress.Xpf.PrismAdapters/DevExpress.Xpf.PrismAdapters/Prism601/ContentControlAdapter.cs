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

using DevExpress.Xpf.Native.PrismWrappers.Prism601;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Windows.Data;
using System.Windows.Controls;
using DevExpress.Xpf.Native.Prism;
namespace DevExpress.Xpf.Native.Prism601 {
	public class ContentControlAdapterImpl {
		class Adapter : AdapterBase<ContentControl> {
			bool updating = false;
			public Adapter(IRegionRuntimeWrapper region, ContentControl target) : base(region, target) {
				BindingOperations.SetBinding(target, ContentControl.ContentProperty,
					new Binding("Content") { Source = this, Mode = BindingMode.OneWayToSource });
			}
			object content;
			public object Content {
				get { return content; }
				set {
					if (content != value && !updating) {
						if (!((IEnumerable<object>)Region.Views.Object).Contains(value)) {
							Region.Add(value);
						}
						Region.Activate(value);
					}
					content = value;
				}
			}
			protected override void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
				updating = true;
				try {
					if(e.Action == NotifyCollectionChangedAction.Add && !((IEnumerable<object>)Region.ActiveViews.Object).Any()) {
						Region.Activate(e.NewItems[0]);
					}
				} finally {
					updating = false;
				}
			}
			protected override void OnActiveViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
				updating = true;
				try {
					if(e.Action == NotifyCollectionChangedAction.Add) {
						Target.Content = e.NewItems[0];
					} else if(e.Action == NotifyCollectionChangedAction.Remove) {
						if(Target.Content == e.OldItems[0]) {
							Target.Content = null;
						}
					}
				} finally {
					updating = false;
				}
			}
		}
		public ContentControlAdapterImpl(int prismVersion) {
		}
		public void Adapt(IRegionRuntimeWrapper region, ContentControl regionTarget) {
			new Adapter(region, regionTarget);
		}
	}
}
