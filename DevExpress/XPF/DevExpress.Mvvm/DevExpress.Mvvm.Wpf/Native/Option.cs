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
#if MVVM
namespace DevExpress.Mvvm.Native {
#else
#if GANTT
namespace DevExpress.Gantt.Core.Native {
#else
namespace DevExpress.Diagram.Core.Native {
#endif
#endif
	public struct Option<T> : IEquatable<Option<T>> {
		public static readonly Option<T> Empty = new Option<T>();
		public static readonly Option<T> Default = new Option<T>(default(T)); 
		public readonly bool HasValue;
		readonly T value;
		public Option(T value)
			: this() {
			this.value = value;
			HasValue = true;
		}
		public T ToValue() {
			if(!HasValue)
				throw new InvalidOperationException();
			return value;
		}
		public void Match(Action<T> action, Action noValueAction) {
			if(HasValue)
				action(value);
			else
				noValueAction();
		}
		public TOut Match<TOut>(Func<T, TOut> getValue, Func<TOut> getNoValue) {
			return HasValue ? getValue(value) : getNoValue();
		}
		public TOut Match<TOut>(Func<T, TOut> getValue, TOut noValue) {
			return HasValue ? getValue(value) : noValue;
		}
		public T GetValueOrDefault(Func<T> getDefaultValue) {
			return HasValue ? value : getDefaultValue();
		}
		public T GetValueOrDefault(T defaultValue = default(T)) {
			return HasValue ? value : defaultValue;
		}
		public void DoIfHasValue(Action<T> action) {
			if(HasValue)
				action(value);
		}
		#region Equality
		public static bool operator ==(Option<T> a, Option<T> b) {
			if(!a.HasValue && !b.HasValue) return true;
			if(!a.HasValue || !b.HasValue) return false;
			return EqualityComparer<T>.Default.Equals(a.value, b.value);
		}
		public override int GetHashCode() {
			return !HasValue ? -1 : EqualityComparer<T>.Default.GetHashCode(value);
		}
		public static bool operator !=(Option<T> a, Option<T> b) {
			return !(a == b);
		}
		public override bool Equals(object obj) {
			return obj is Option<T> && Equals((Option<T>)obj);
		}
		public bool Equals(Option<T> other) {
			return this == other;
		}
		#endregion
	}
	public static class OptionExtensions {
		public static Option<TOut> Apply<TIn, TOut>(this Option<TIn> t, Func<TIn, TOut> apply) {
			return t.Match(x => apply(x).AsOption(), Option<TOut>.Empty);
		}
		public static Option<TOut> Bind<TIn, TOut>(this Option<TIn> t, Func<TIn, Option<TOut>> apply) {
			return t.Match(x => apply(x), Option<TOut>.Empty);
		}
#if !MVVM
		internal
#else
		public
#endif
		static Option<T> AsOption<T>(this T value) {
			return new Option<T>(value);
		}
#if !MVVM
		internal
#else
		public
#endif
		static Option<T> ToOption<T>(this T? value) where T : struct {
			return value.HasValue ? new Option<T>(value.Value) : Option<T>.Empty;
		}
#if !MVVM
		internal
#else
		public
#endif
		static Option<T> ToOption<T>(this T value) where T : class {
			return value != null ? new Option<T>(value) : Option<T>.Empty;
		}
		public static T? ToMaybe<T>(this Option<T> t) where T : struct {
			return t.Match(x => x, default(T?));
		}
		public static Option<T> Or<T>(this Option<T> t, Option<T> fallback) {
			return t.HasValue ? t : fallback;
		}
#if !MVVM
		internal
#else
		public
#endif
		static Option<TOut> WithOption<TIn, TOut>(this TIn value, Func<TIn, TOut> selector) where TIn : class {
			if(value == null)
				return Option<TOut>.Empty;
			return selector(value).AsOption();
		}
		public static IEnumerable<TOut> SelectWhile<TIn, TOut>(this IEnumerable<TIn> enumerable, Func<TIn, Option<TOut>> selector) {
			return enumerable.Select(selector).TakeWhile(x => x.HasValue).Select(x => x.ToValue());
		}
		public static IEnumerable<T> Values<T>(this IEnumerable<Option<T>> enumerable) {
			return enumerable.Where(x => x.HasValue).Select(x => x.ToValue());
		}
		public static Option<TValue> TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) {
			TValue result;
			return dictionary.TryGetValue(key, out result) ? result.AsOption() : Option<TValue>.Empty;
		}
	}
}
