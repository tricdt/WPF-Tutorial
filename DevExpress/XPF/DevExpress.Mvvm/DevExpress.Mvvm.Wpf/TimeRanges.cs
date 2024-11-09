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
using System.ComponentModel;
using System.Globalization;
using DevExpress.Mvvm.Native;
namespace DevExpress.Mvvm {
	[TypeConverter(typeof(DateTimeRangeTypeConverter))]
	public struct DateTimeRange : IFormattable, IEquatable<DateTimeRange> {
		static readonly string[] DefaultDateTimeDelimeter = new string[] { ")-(" };
		internal static readonly string DefaultFormat = "({0})-({1})";
		static readonly DateTimeRange empty = new DateTimeRange(DateTime.MinValue, DateTime.MinValue);
		public static DateTimeRange Empty { get { return empty; } }
		public static DateTimeRange Today { get { return new DateTimeRange(DateTime.Now.Date, TimeSpan.FromDays(1)); } }
		public static bool operator ==(DateTimeRange x, DateTimeRange y) {
			return x.Equals(y);
		}
		public static bool operator !=(DateTimeRange x, DateTimeRange y) {
			return !x.Equals(y);
		}
		public static DateTimeRange Union(DateTimeRange x, DateTimeRange y) {
			if(x.IsEmpty)
				return y;
			if(y.IsEmpty)
				return x;
			var start = Math.Min(x.start.Ticks, y.start.Ticks);
			var end = Math.Max(x.end.Ticks, y.end.Ticks);
			return new DateTimeRange(new DateTime(start), new DateTime(end));
		}
		public static DateTimeRange Intersect(DateTimeRange x, DateTimeRange y, bool includeBounds = true) {
			long start = Math.Max(x.start.Ticks, y.start.Ticks);
			long end = Math.Min(x.end.Ticks, y.end.Ticks);
			bool intersect = includeBounds ? end >= start : x.start == y.start || end > start;
			return intersect ? new DateTimeRange(new DateTime(start), new DateTime(end)) : Empty;
		}
		public static bool IntersectsWith(DateTimeRange x, DateTimeRange y, bool includeBounds = true) {
			if(includeBounds)
				return x.End >= y.Start && x.Start <= y.End;
			return (x.End > y.Start && x.Start < y.End) || x.Start == y.Start;
		}
		public static bool TryParse(string input, CultureInfo culture, out DateTimeRange result) {
			string[] dateTimes = input.Split(DefaultDateTimeDelimeter, StringSplitOptions.None);
			if (dateTimes.Length == 2) {
				DateTime start;
				DateTime end;
				if (TryParse(dateTimes[0].Replace("(", ""), culture, out start) && TryParse(dateTimes[1].Replace(")", ""), culture, out end)) {
					result = new DateTimeRange(start, end);
					return true;
				}
			}
			result = Empty;
			return false;
		}
		static bool TryParse(string value, CultureInfo culture, out DateTime result) {
			try {
				result = Convert.ToDateTime(value, culture);
				return true;
			} catch {
				result = DateTime.Today;
				return false;
			}
		}
		DateTime start;
		DateTime end;
		public DateTimeRange(DateTime start, TimeSpan duration) {
			this.start = start;
			if (duration.Ticks >= 0) 
				this.end = duration.Ticks > DateTime.MaxValue.Ticks - start.Ticks ? DateTime.MaxValue : start.Add(duration);
			else if (duration == TimeSpan.MinValue || duration.Negate().Ticks > start.Ticks)
				this.end = DateTime.MinValue;
			else
				this.end = start.Add(duration);
		}
		public DateTimeRange(DateTime start, DateTime end) {
			this.start = start;
			this.end = end;
		}
		public DateTimeRange(DateTimeRange source) {
			this.start = source.start;
			this.end = source.end;
		}
		public DateTime Start { get { return this.start; } }
		public DateTime End { get { return this.end; } }
		public bool IsEmpty { get { return Equals(empty); } }
		public bool IsValid { get { return this.start <= this.end; } }
		public TimeSpan Duration { get { return this.end - this.start; } }
		public DateTimeRange Union(DateTimeRange x) {
			return Union(this, x);
		}
		public DateTimeRange Intersect(DateTimeRange x, bool includeBounds = true) {
			return Intersect(this, x, includeBounds);
		}
		public bool IntersectsWith(DateTimeRange x, bool includeBounds = true) {
			return IntersectsWith(this, x, includeBounds);
		}
		public bool Contains(DateTime date, bool includeRangeEnd = false) {
			if (start == end)
				return date == this.start;
			return !includeRangeEnd 
				? date >= this.start && date < this.end 
				: date >= this.start && date <= this.end;
		}
		public bool Contains(DateTimeRange x) {
			if (start == end && x.start == x.end)
				return start == x.start;
			if (x.start >= end) return false;
			return x.start >= this.start && x.end <= this.end;
		}
		public override bool Equals(object obj) {
			return obj is DateTimeRange ? Equals((DateTimeRange)obj) : false;
		}
		public bool Equals(DateTimeRange other) {
			return this.start == other.start && this.end == other.end;
		}
		public override int GetHashCode() {
			int startHash = this.start.GetHashCode();
			return (((startHash >> 24) & 0x000000FF) | ((startHash >> 8) & 0x0000FF00) | ((startHash << 8) & 0x00FF0000) | (unchecked((int)((startHash << 24) & 0xFF000000)))) ^ this.end.GetHashCode();
		}
		public override string ToString() {
			return string.Format(DefaultFormat, this.start, this.end);
		}
		public string ToString(IFormatProvider provider) {
			return provider == null ? ToString() : ToStringCore(DefaultFormat, provider);
		}
		public string ToString(string format, IFormatProvider provider) {
			if (string.IsNullOrEmpty(format))
				format = DefaultFormat;
			if (provider == null)
				return string.Format(format, this.start, this.end);
			return ToStringCore(format, provider);
		}
		string ToStringCore(string format, IFormatProvider provider) {
			ICustomFormatter customFormatter = provider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
			if (customFormatter == null)
				return string.Format(format, this.start.ToString(provider), this.end.ToString(provider));
			return customFormatter.Format(format, this, provider);
		}
	}
	[TypeConverter(typeof(TimeSpanRangeTypeConverter))]
	public struct TimeSpanRange : IFormattable, IEquatable<TimeSpanRange> {
		internal static readonly string DefaultTimeSpanFormat = "d\\.hh\\:mm\\:ss\\.fff";
		static readonly char DefaultTimeSpanDelimeter = '-';
		internal static readonly string DefaultFormat = "{0}-{1}";
		static readonly TimeSpanRange zero = new TimeSpanRange(TimeSpan.Zero, TimeSpan.Zero);
		static readonly TimeSpanRange day = new TimeSpanRange(TimeSpan.Zero, TimeSpan.FromDays(1));
		public static TimeSpanRange Zero { get { return zero; } }
		public static TimeSpanRange Day { get { return day; } }
		public static bool operator ==(TimeSpanRange x, TimeSpanRange y) {
			return x.Equals(y);
		}
		public static bool operator !=(TimeSpanRange x, TimeSpanRange y) {
			return !x.Equals(y);
		}
		public static TimeSpanRange Union(TimeSpanRange x, TimeSpanRange y) {
			TimeSpan start = x.start < y.start ? x.start : y.start;
			TimeSpan end = x.end > y.end ? x.end : y.end;
			return new TimeSpanRange(start, end);
		}
		public static TimeSpanRange Intersect(TimeSpanRange x, TimeSpanRange y, bool includeBounds = true) {
			TimeSpan start = x.start > y.start ? x.start : y.start;
			TimeSpan end = x.end < y.end ? x.end : y.end;
			bool intersect = includeBounds ? end >= start : x.start == y.start || end > start;
			return intersect ? new TimeSpanRange(start, end) : Zero;
		}
		public static bool IntersectsWith(TimeSpanRange x, TimeSpanRange y, bool includeBounds = true) {
			if(includeBounds)
				return x.End >= y.Start && x.Start <= y.End;
			return (x.End > y.Start && x.Start < y.End) || x.Start == y.Start;
		}
		public static TimeSpanRange Parse(string input, CultureInfo culture) {
			TimeSpanRange result;
			if(!TryParse(input, culture, out result))
				throw new FormatException();
			return result;
		}
		public static bool TryParse(string input, CultureInfo culture, out TimeSpanRange result) {
			string[] timeSpans = input.Split(DefaultTimeSpanDelimeter);
			if (timeSpans.Length == 2) {
				TimeSpan start;
				TimeSpan end;
				if (TimeSpan.TryParse(timeSpans[0], culture, out start) && TimeSpan.TryParse(timeSpans[1], culture, out end)) {
					result = new TimeSpanRange(start, end);
					return true;
				}
			}
			result = TimeSpanRange.zero;
			return false;
		}
		readonly TimeSpan start;
		readonly TimeSpan end;
		public TimeSpanRange(TimeSpan start, TimeSpan end) {
			this.start = start;
			this.end = end;
		}
		public TimeSpan Start { get { return this.start; } }
		public TimeSpan End { get { return this.end; } }
		public TimeSpan Duration { get { return this.end - this.start; } }
		public bool IsZero { get { return Equals(zero); } }
		public bool IsDay { get { return Equals(day); } }
		public bool IsValid { get { return this.start <= this.end; } }
		public TimeSpanRange Union(TimeSpanRange x) {
			return Union(this, x);
		}
		public bool Contains(TimeSpan time, bool includeRangeEnd = false) {
			if(start == end)
				return time == this.start;
			return !includeRangeEnd
			  ? time >= this.start && time < this.end
			  : time >= this.start && time <= this.end;
		}
		public bool Contains(TimeSpanRange x) {
			if(start == end && x.start == x.end)
				return start == x.start;
			if(x.start >= end) return false;
			return x.start >= this.start && x.end <= this.end;
		}
		public TimeSpanRange Intersect(TimeSpanRange x, bool includeBounds = true) {
			return Intersect(this, x, includeBounds);
		}
		public bool IntersectsWith(TimeSpanRange x, bool includeBounds = true) {
			return IntersectsWith(this, x, includeBounds);
		}
		public override bool Equals(object obj) {
			return obj is TimeSpanRange ? Equals((TimeSpanRange)obj) : false;
		}
		public bool Equals(TimeSpanRange other) {
			return this.start == other.start && this.end == other.end;
		}
		public override int GetHashCode() {
			int startHash = this.start.GetHashCode();
			return (((startHash >> 24) & 0x000000FF) | ((startHash >> 8) & 0x0000FF00) | ((startHash << 8) & 0x00FF0000) | (unchecked((int)((startHash << 24) & 0xFF000000)))) ^ this.end.GetHashCode();
		}
		public override string ToString() {
			return string.Format(DefaultFormat, this.start.ToString(DefaultTimeSpanFormat), this.end.ToString(DefaultTimeSpanFormat));
		}
		public string ToString(IFormatProvider provider) {
			return provider == null ? ToString() : ToStringCore(DefaultFormat, provider);
		}
		public string ToString(string format, IFormatProvider provider) {
			if (string.IsNullOrEmpty(format))
				format = DefaultFormat;
			if (provider == null)
				return string.Format(format, this.start.ToString(DefaultTimeSpanFormat), this.end.ToString(DefaultTimeSpanFormat));
			return ToStringCore(format, provider);
		}
		string ToStringCore(string format, IFormatProvider provider) {
			ICustomFormatter customFormatter = provider.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
			if (customFormatter == null)
				return string.Format(format, this.start.ToString(DefaultTimeSpanFormat, provider), this.end.ToString(DefaultTimeSpanFormat, provider));
			return customFormatter.Format(format, this, provider);
		}
	}
}
namespace DevExpress.Mvvm.Native {
	public class DateTimeRangeTypeConverter : StringConverter {
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
			string input = value as string;
			if (String.IsNullOrEmpty(input))
				return null;
			DateTimeRange result;
			if (DateTimeRange.TryParse(input, culture, out result))
				return result;
			return null;
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			return value is DateTimeRange ? ((DateTimeRange)value).ToString(culture) : null;
		}
	}
	public class TimeSpanRangeTypeConverter : StringConverter {
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value) {
			string input = value as string;
			if (String.IsNullOrEmpty(input))
				return null;
			TimeSpanRange result;
			if (TimeSpanRange.TryParse(input, culture, out result))
				return result;
			return null;
		}
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
			return value is TimeSpanRange ? ((TimeSpanRange)value).ToString(culture) : null;
		}
	}
}
