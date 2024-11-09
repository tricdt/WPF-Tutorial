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
using System.Windows.Controls;
using DevExpress.Mvvm.UI.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	using System.Windows;
	public partial class DocumentPresenterContentAdapter : Decorator {
		static DocumentPresenterContentAdapter() {
			DependencyPropertyRegistrator<DocumentPresenterContentAdapter>.New()
				.Register(nameof(DocumentMaxHeight), out DocumentMaxHeightProperty, 100.0, d => d.UseShortDocument = false)
				.Register(nameof(AlwaysShowFullDescription), out AlwaysShowFullDescriptionProperty, false, FrameworkPropertyMetadataOptions.AffectsRender)
				.Register(nameof(Document), out DocumentProperty, default(string), d => d.UseShortDocument = false)
				.RegisterReadOnly(nameof(UseShortDocument), out UseShortDocumentPropertyKey, out UseShortDocumentProperty, false)
			;
		}
		protected override Size MeasureOverride(Size constraint) {
			var child = Child;
			UseShortDocument = false;
			if(child == null) return new Size();
			child.Measure(constraint);
			if(AlwaysShowFullDescription)
				return child.DesiredSize;
			var maxHeight = DocumentMaxHeight;
			if(child.DesiredSize.Height > maxHeight) {
				UseShortDocument = true;
				child.InvalidateMeasure();
				child.Measure(constraint);
			}
			return new Size(child.DesiredSize.Width, Math.Min(maxHeight, child.DesiredSize.Height));
		}
	}
}
