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
using System.Globalization;
using System.Linq;
using DevExpress.Mvvm.Native;
namespace DevExpress.Mvvm {
	[TypeConverter(typeof(TimeSpanRangeCollectionConverter))]
	public sealed class TimeSpanRangeCollection : ImmutableCollectionCore<TimeSpanRange, TimeSpanRangeCollection> {
		sealed class TimeSpanRangeCollectionConverter : TypeConverter {
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
				var str = value as string;
				if(str != null)
					return TimeSpanRangeCollection.Parse(str, culture);
				return base.ConvertFrom(context, culture, value);
			}
			public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
				return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
			}
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
				if(destinationType == typeof(string) && value != null)
					return ((TimeSpanRangeCollection)value).ToString(culture);
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}
		public TimeSpanRangeCollection() : base() { }
		public TimeSpanRangeCollection(IEnumerable<TimeSpanRange> values) : base(values) { }
		protected override TimeSpanRangeCollection Create(IEnumerable<TimeSpanRange> values) => new TimeSpanRangeCollection(values);
		public override string ToString() {
			return string.Join(",", this.Select(x => x.ToString()));
		}
		public string ToString(IFormatProvider provider) {
			return provider == null ? ToString() : ToStringCore(TimeSpanRange.DefaultFormat, provider);
		}
		public string ToString(string format, IFormatProvider provider) {
			if(string.IsNullOrEmpty(format))
				format = TimeSpanRange.DefaultFormat;
			return ToStringCore(format, provider);
		}
		string ToStringCore(string format, IFormatProvider provider) {
			ICustomFormatter customFormatter = provider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
			if(customFormatter != null)
				return customFormatter.Format(format, this, provider);
			return string.Join(",", this.Select(x => x.ToString(format, provider)));
		}
		public static TimeSpanRangeCollection Parse(string input, CultureInfo culture) {
			TimeSpanRangeCollection result;
			if(!TryParse(input, culture, out result))
				throw new FormatException();
			return result;
		}
		public static bool TryParse(string input, CultureInfo culture, out TimeSpanRangeCollection result) {
			var r = input.Split(',').AsEnumerable().WithState(true).SelectUntil((part, ok) => {
				TimeSpanRange range;
				ok &= TimeSpanRange.TryParse(part.Trim(), culture, out range);
				return range.WithState(ok);
			}, ok => !ok, x => new TimeSpanRangeCollection(x));
			if(!r.State) {
				result = null;
				return false;
			}
			result = r.Value;
			return true;
		}
	}
}
