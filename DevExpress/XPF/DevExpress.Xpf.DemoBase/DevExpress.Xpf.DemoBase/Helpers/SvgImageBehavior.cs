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

using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Mvvm.UI.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.DemoBase.Helpers;
namespace DevExpress.Xpf.DemoBase.Helpers {
	using System;
	using System.Windows;
	using DevExpress.Mvvm.Native;
	public partial class SvgImageBehavior : Behavior<DependencyObject> {
		static SvgImageBehavior() {
			DependencyPropertyRegistrator<SvgImageBehavior>.New()
				.Register(nameof(Uri), out UriProperty, default(Uri))
				.Register(nameof(UriString), out UriStringProperty, string.Empty)
				.Register(nameof(AutoSize), out AutoSizeProperty, true)
				.Register(nameof(Size), out SizeProperty, default(Size?))
				.Register(nameof(Target), out TargetProperty, default(DependencyProperty))
			;
		}
		sealed class ServiceProvider : IServiceProvider, IProvideValueTarget {
			object IServiceProvider.GetService(Type serviceType) {
				if(serviceType == typeof(IProvideValueTarget)) return this;
				return null;
			}
			object IProvideValueTarget.TargetObject { get { return new Setter(); } }
			object IProvideValueTarget.TargetProperty { get { throw new NotSupportedException(); } }
		}
		public SvgImageBehavior() {
			this
				.NotifyValue(x => x.AssociatedObject)
				.Where(x => x != null)
				.SelectMany(_ => this.DependencyValue(x => x.Target))
				.Where(x => x != null)
				.SelectMany(_ => this.DependencyValue(SizeProperty))
				.SelectMany(_ => this.DependencyValue(AutoSizeProperty))
				.SelectMany(_ => this.DependencyValue(x => x.Uri))
				.SelectMany(uri => uri != null ? uri.Always() : this.DependencyValue(x => x.UriString).Select(x => string.IsNullOrEmpty(x) ? default(Uri) : new Uri(x)))
				.Select(uri => uri.With(x => (BindingBase)new SvgImageSourceExtension() { Uri = x, Size = Size, AutoSize = AutoSize }.ProvideValue(new ServiceProvider())))
				.Execute(binding => {
					if(binding == null)
						BindingOperations.ClearBinding(AssociatedObject, Target);
					else
						BindingOperations.SetBinding(AssociatedObject, Target, binding);
				})
			;
		}
	}
}
