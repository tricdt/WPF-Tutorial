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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using DevExpress.Utils.Svg;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Internal;
using DevExpress.Xpf.Core.Native;
namespace DevExpress.Xpf.DemoCenterBase.Helpers {
	public abstract class DumbBaseSvgImageSourceExtension : MarkupExtension {
		public Size? Size { get; set; }
		public bool AutoSize { get; set; }
		public DumbBaseSvgImageSourceExtension() { AutoSize = true; }
		protected abstract Stream CreateSvgStream(IServiceProvider serviceProvider);
		public override object ProvideValue(IServiceProvider serviceProvider) {
			var stream = CreateSvgStream(serviceProvider);
			if (stream == null)
				return null;
			var image = SvgLoader.LoadFromStream(stream);
			var size = Size.HasValue ? Size.Value : new Size(image.Width, image.Height);
			if (serviceProvider == null)
				return WpfSvgRenderer.CreateImageSource(image, size, null, null, null, AutoSize);
			var service = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			if (service != null && service.TargetObject is ImageSelectorExtension) {
				return this;
			}
			if (service != null && (service.TargetObject is Setter || service.TargetProperty is DependencyProperty)) {
				MultiBinding multiBinding = new MultiBinding();
				multiBinding.Bindings.Add(new Binding() { RelativeSource = RelativeSource.Self });
				multiBinding.Bindings.Add(new Binding() { Path = new PropertyPath(CommonThemeHelper.TreeWalkerProperty), RelativeSource = RelativeSource.Self });
				multiBinding.Bindings.Add(new Binding() { Path = new PropertyPath(SvgImageHelper.StateProperty), RelativeSource = RelativeSource.Self });
				multiBinding.Bindings.Add(new Binding() { Path = new PropertyPath(WpfSvgPalette.PaletteProperty), RelativeSource = RelativeSource.Self });
				multiBinding.Converter = CreateConverter(image, size, AutoSize);
				if (service.TargetObject is Setter)
					return multiBinding;
				return multiBinding.ProvideValue(serviceProvider);
			}
			return WpfSvgRenderer.CreateImageSource(image, size, null, null, null, AutoSize);
		}
		protected virtual IMultiValueConverter CreateConverter(SvgImage image, Size size, bool autoSize) {
			return new TreeWalkerToSvgImageConverter(image, size, autoSize);
		}
		class TreeWalkerToSvgImageConverter : IMultiValueConverter {
			readonly SvgImage image;
			readonly Size size;
			readonly bool autoSize;
			public TreeWalkerToSvgImageConverter(SvgImage image, Size size, bool autoSize) {
				this.image = image;
				this.size = size;
				this.autoSize = autoSize;
			}
			public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
				var targetObject = values[0] as DependencyObject;
				var state = values[2] as string;
				return WpfSvgRenderer.CreateImageSource(image, size, null, null, state, autoSize);
			}
			public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
				throw new NotImplementedException();
			}
		}
	}
	public class DumbSvgImageSourceExtension : DumbBaseSvgImageSourceExtension {
		#region static
		static readonly Func<Uri, Stream> createRequestAndGetResponseStream;
		static DumbSvgImageSourceExtension() {
			var typeWpfWebRequestHelper = DevExpress.Data.Internal.SafeTypeResolver.GetKnownType(typeof(System.Windows.Media.Imaging.BitmapImage).Assembly, "MS.Internal.WpfWebRequestHelper");
			createRequestAndGetResponseStream = ReflectionHelper.CreateInstanceMethodHandler<Func<Uri, Stream>>(null, "CreateRequestAndGetResponseStream", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, typeWpfWebRequestHelper, parametersCount: 1);
		}
		#endregion
		public Uri Uri { get; set; }
		protected override Stream CreateSvgStream(IServiceProvider serviceProvider) {
			Uri baseUri, absoluteUri;
			if (UriQualifierHelper.MakeAbsoluteUri(serviceProvider, Uri, out absoluteUri, out baseUri))
				return CreateRequestAndGetResponseStream(absoluteUri);
			return null;
		}
		protected internal static Stream CreateRequestAndGetResponseStream(Uri absoluteUri) { return createRequestAndGetResponseStream(absoluteUri); }
	}
}
