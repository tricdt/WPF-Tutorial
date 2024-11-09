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
using DevExpress.Mvvm.Native;
namespace DevExpress.Mvvm.Native {
#else
using DevExpress.Internal;
namespace DevExpress.Diagram.Core.Native {
#endif
	public struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>> {
		public static implicit operator Either<TLeft, TRight>(Either.Left<TLeft> val) {
			return Left(val.Value);
		}
		public static implicit operator Either<TLeft, TRight>(Either.Right<TRight> val) {
			return Right(val.Value);
		}
		public static Either<TLeft, TRight> Left(TLeft value) {
			return new Either<TLeft, TRight>(EitherType.Left, value);
		}
		public static Either<TLeft, TRight> Right(TRight value) {
			return new Either<TLeft, TRight>(EitherType.Right, value);
		}
		static IEqualityComparer<TLeft> LeftComparer => EqualityComparer<TLeft>.Default;
		static IEqualityComparer<TRight> RightComparer => EqualityComparer<TRight>.Default;
		Either(EitherType type, object value) {
			this.type = type;
			this.value = value;
		}
		#region private
		readonly EitherType type;
		readonly object value;
		TLeft LeftValue => (TLeft)value;
		TRight RightValue => (TRight)value;
		void ThrowInvalidOperationIfNotInitialized() {
			if(type == EitherType.Default)
				throw new InvalidOperationException("Invalid operation on default Either struct.");
		} 
		#endregion
		public bool IsRight() {
			ThrowInvalidOperationIfNotInitialized();
			return type == EitherType.Right;
		}
		public bool IsLeft() {
			ThrowInvalidOperationIfNotInitialized();
			return type == EitherType.Left;
		}
		public T Match<T>(Func<TLeft, T> left, Func<TRight, T> right) {
			return IsLeft() ? left(LeftValue) : right(RightValue);
		}
		public void Match(Action<TLeft> left, Action<TRight> right) {
			if(IsLeft())
				left(LeftValue);
			else
				right(RightValue);
		}
		public TLeft ToLeft() {
			if(IsLeft())
				return LeftValue;
			throw new InvalidOperationException();
		}
		public TRight ToRight() {
			if(IsRight())
				return RightValue;
			throw new InvalidOperationException();
		}
		public TLeft LeftOrDefault() {
			return IsLeft() ? LeftValue : default(TLeft);
		}
		public TRight RightOrDefault() {
			return IsRight() ? RightValue : default(TRight);
		}
		#region overrides
		public static bool operator ==(Either<TLeft, TRight> a, Either<TLeft, TRight> b) {
			return a.Equals(b);
		}
		public static bool operator !=(Either<TLeft, TRight> a, Either<TLeft, TRight> b) {
			return !(a == b);
		}
		public override int GetHashCode() {
			var hashCode = 1148455455;
			hashCode = hashCode * -1521134295 + type.GetHashCode();
			hashCode = hashCode * -1521134295 + (IsLeft() ? LeftComparer.GetHashCode(LeftValue) : RightComparer.GetHashCode(RightValue));
			return hashCode;
		}
		public override bool Equals(object obj) {
			ThrowInvalidOperationIfNotInitialized();
			if(!(obj is Either<TLeft, TRight>))
				return false;
			return Equals((Either<TLeft, TRight>)obj);
		}
		public bool Equals(Either<TLeft, TRight> other) {
			ThrowInvalidOperationIfNotInitialized();
			other.ThrowInvalidOperationIfNotInitialized();
			if(type != other.type)
				return false;
			if(IsLeft())
				return LeftComparer.Equals(LeftValue, other.LeftValue);
			return RightComparer.Equals(RightValue, other.RightValue);
		}
		public override string ToString() {
			ThrowInvalidOperationIfNotInitialized();
			if(IsLeft())
				return $"Left: {LeftValue}";
			return $"Right: {RightValue}";
		}
		#endregion
	}
	enum EitherType { Default, Left, Right } 
	public static class Either {
		public struct Left<T> {
			internal readonly T Value;
			public Left(T value) {
				Value = value;
			}
			public Either<T, TRight> AsEither<TRight>() => this;
		}
		public struct Right<T> {
			internal readonly T Value;
			public Right(T value) {
				Value = value;
			}
			public Either<TLeft, T> AsEither<TLeft>() => this;
		}
#if MVVM
		public
#else
		internal
#endif
		static Left<T> AsLeft<T>(this T value) => new Left<T>(value);
#if MVVM
		public
#else
		internal
#endif
		static Right<T> AsRight<T>(this T value) => new Right<T>(value);
		public static Either<TLeft, TRight> MaybeLeft<TLeft, TRight>(TLeft maybeLeft, TRight right) where TLeft : class {
			return maybeLeft.Return(x => Either<TLeft, TRight>.Left(x), () => right.AsRight());
		}
		public static Either<TLeft, TRight> MaybeRight<TLeft, TRight>(TRight maybeRight, TLeft left) where TRight : class {
			return maybeRight.Return(x => Either<TLeft, TRight>.Right(x), () => left.AsLeft());
		}
		public static TRight UnwrapException<TException, TRight>(this Either<TException, TRight> value) where TException : Exception {
			return value.Match(left => { throw left; }, right => right);
		}
		public static Either<TLeft, TRightNew> Select<TLeft, TRight, TRightNew>(this Either<TLeft, TRight> value, Func<TRight, TRightNew> selector) {
			return value.Match(
				left => Either<TLeft, TRightNew>.Left(left),
				right => Either<TLeft, TRightNew>.Right(selector(right))
			);
		}
		public static Either<TLeftNew, TRight> SelectLeft<TLeft, TRight, TLeftNew>(this Either<TLeft, TRight> value, Func<TLeft, TLeftNew> selector) {
			return value.Match(
				left => Either<TLeftNew, TRight>.Left(selector(left)),
				right => Either<TLeftNew, TRight>.Right(right)
			);
		}
		public static Either<TLeft, TRightNew> SelectMany<TLeft, TRight, TRightNew>(this Either<TLeft, TRight> value, Func<TRight, Either<TLeft, TRightNew>> selector) {
			return value.Match(
				left => Either<TLeft, TRightNew>.Left(left),
				right => selector(right)
			);
		}
		public static T Collapse<T>(this Either<T, T> value) {
			return value.Match(left => left, right => right);
		}
		public static Either<Exception, TResult> Try<TResult>(Func<Either<Exception, TResult>> func) {
			try {
				return func();
			} catch(Exception e) {
				return e.AsLeft();
			}
		}
		public static Either<TLeft, TRight[]> Sequence<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source) {
			var left = Option<TLeft>.Empty;
			var right = source.SelectWhile(item => item.Match(x => { left = x.AsOption(); return Option<TRight>.Empty; }, x => x.AsOption())).ToArray();
			return left.Match<Either<TLeft, TRight[]>>(x => x.AsLeft(), () => right.AsRight());
		}
		public static TResult Partition<TLeft, TRight, TAccumulate, TResult>(this IEnumerable<Either<TLeft, TRight>> source, TAccumulate seed, Func<TAccumulate, TLeft, TAccumulate> func, Func<TAccumulate, TRight[], TResult> resultSelector) {
			var lefts = seed;
			var rights = source.SelectMany(item => item.Match(x => { lefts = func(lefts, x); return Enumerable.Empty<TRight>(); }, x => x.Yield())).ToArray();
			return resultSelector(lefts, rights);
		}
		public static IEnumerable<TRight> Rights<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source) {
			return source.Where(x => x.IsRight()).Select(x => x.ToRight());
		}
		public static IEnumerable<TLeft> Lefts<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> source) {
			return source.Where(x => x.IsLeft()).Select(x => x.ToLeft());
		}
#region combine
		public static Either<TLeftNew, TRightNew> CombineMany<TLeft, TRight, TLeftNew, TRightNew>(
			this IEnumerable<Either<TLeft, TRight>> values,
			Func<IEnumerable<TLeft>, TLeftNew> combineLefts,
			Func<IEnumerable<TRight>, TRightNew> combineRights
			) {
			var lefts = values.Where(x => x.IsLeft());
			if(lefts.Any())
				return combineLefts(lefts.Select(x => x.ToLeft())).AsLeft();
			return combineRights(values.Select(x => x.ToRight())).AsRight();
		}
		public static Either<IEnumerable<TLeft>, TResult> Combine2<TLeft, T1, T2, TResult>(
			Either<TLeft, T1> x1,
			Either<TLeft, T2> x2,
			Func<T1, T2, TResult> combine
		) {
			IEnumerable<TLeft> lefts = Lefts(x1, x2);
			if(lefts.Any())
				return Either<IEnumerable<TLeft>, TResult>.Left(lefts);
			return combine(x1.ToRight(), x2.ToRight()).AsRight();
		}
		static IEnumerable<TLeft> Lefts<TLeft, T1, T2>(
			Either<TLeft, T1> x1,
			Either<TLeft, T2> x2) {
			if(x1.IsLeft())
				yield return x1.ToLeft();
			if(x2.IsLeft())
				yield return x2.ToLeft();
		}
		public static Either<IEnumerable<TLeft>, TResult> Combine3<TLeft, T1, T2, T3, TResult>(
			Either<TLeft, T1> x1,
			Either<TLeft, T2> x2,
			Either<TLeft, T3> x3,
			Func<T1, T2, T3, TResult> combine
		) {
			IEnumerable<TLeft> lefts = Lefts(x1, x2, x3);
			if(lefts.Any())
				return Either<IEnumerable<TLeft>, TResult>.Left(lefts);
			return combine(x1.ToRight(), x2.ToRight(), x3.ToRight()).AsRight();
		}
		static IEnumerable<TLeft> Lefts<TLeft, T1, T2, T3>(
			Either<TLeft, T1> x1,
			Either<TLeft, T2> x2,
			Either<TLeft, T3> x3) {
			if(x1.IsLeft())
				yield return x1.ToLeft();
			if(x2.IsLeft())
				yield return x2.ToLeft();
			if(x3.IsLeft())
				yield return x3.ToLeft();
		}
#endregion
	}
}
