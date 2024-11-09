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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using DevExpress.Mvvm.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	static class Observing {
		public static Observing<T> Create<T>(Option<T> initialValue, Action stop) {
			return new Observing<T>(initialValue, stop);
		}
	}
	sealed class Observing<T> {
		public Observing(Option<T> initialValue, Action stop) {
			InitialValue = initialValue;
			Stop = stop;
		}
		public readonly Option<T> InitialValue;
		public readonly Action Stop;
	}
	sealed class Observable<T> {
		public Observable(Func<Action<Option<T>>, Observing<T>> observe) {
			Observe = observe;
		}
		public readonly Func<Action<Option<T>>, Observing<T>> Observe;
	}
	static class Observable {
		public static Observable<UnitT> Clock(Func<Action, Action> @event) {
			return Clock<UnitT>(x => @event(() => x(default(UnitT))));
		}
		public static Observable<T> Clock<T>(Func<Action<T>, Action> @event) {
			return new Observable<T>(x => Observing.Create(Option<T>.Empty, @event(x == null ? default(Action<T>) : e => x(e.AsOption()))));
		}
		public static Observable<T> Always<T>(this T value) { return new Observable<T>(_ => Observing.Create(value.AsOption(), () => { })); }
		public static Observable<T> Never<T>() { return new Observable<T>(_ => Observing.Create(Option<T>.Empty, () => { })); }
		public static Observable<UnitT> Select<TIn>(this Observable<TIn> observable, Action<TIn> selector) {
			return observable.Select(x => { selector(x); return default(UnitT); });
		}
		public static Observable<TOut> Select<TOut>(this Observable<UnitT> observable, Func<TOut> selector) {
			return observable.Select(_ => selector());
		}
		public static Observable<TOut> Select<TIn, TOut>(this Observable<TIn> observable, Func<TIn, TOut> selector) {
			return new Observable<TOut>(s => {
				var r = observable.Observe(x => s(x.Apply(selector)));
				return Observing.Create(r.InitialValue.Apply(selector), r.Stop);
			});
		}
		public static Observable<T> Where<T>(this Observable<T> observable, Func<T, bool> predicate) {
			return new Observable<T>(s => {
				var r = observable.Observe(v => s(v.Bind(x => predicate(x) ? x.AsOption() : Option<T>.Empty)));
				return Observing.Create(r.InitialValue.Bind(x => predicate(x) ? x.AsOption() : Option<T>.Empty), r.Stop);
			});
		}
		public static Observable<T> DefaultValue<T>(this Observable<T> observable, Func<T> defaultValue) {
			return new Observable<T>(s => {
				var r = observable.Observe(x => s(x.GetValueOrDefault(defaultValue).AsOption()));
				return Observing.Create(r.InitialValue.GetValueOrDefault(defaultValue).AsOption(), r.Stop);
			});
		}
		public static Func<Task<T>> Execute<T>(this Observable<T> observable) {
			var execute = new TaskCompletionSource<T>();
			observable.Execute(r => {
				var task = execute;
				execute = new TaskCompletionSource<T>();
				task.SetResult(r);
			});
			return () => execute.Task;
		}
		public static Action Execute<T>(this Observable<T> observable, Action<T> action) {
			var r = observable.Observe(x => x.DoIfHasValue(action));
			r.InitialValue.DoIfHasValue(action);
			return r.Stop;
		}
		public static Action Execute(this Observable<UnitT> observable, Action action) {
			return observable.Execute(_ => action());
		}
		public static Observable<TR> SelectMany<TI, TR>(this Observable<TI> observable, Func<TI, Observable<TR>> selector) {
			return observable.SelectMany(selector, (_, x) => x);
		}
		public static Observable<TR> SelectMany<TR>(this Observable<UnitT> observable, Func<Observable<TR>> selector) {
			return observable.SelectMany(_ => selector());
		}
		public static Observable<TR> SelectMany<TI, TC, TR>(this Observable<TI> observable, Func<TI, Observable<TC>> selector, Func<TI, TC, TR> projector) {
			return new Observable<TR>(s => {
				Action stopBase = null;
				var si = observable.Observe(oi => {
					stopBase();
					var ssc = oi.Match(i => {
						var r = selector(i).Observe(oc => s(oc.Apply(x => projector(i, x))));
						return Observing.Create(r.InitialValue.Apply(x => projector(i, x)), r.Stop);
					}, () => Observing.Create(Option<TR>.Empty, () => { }));
					stopBase = ssc.Stop;
					s(ssc.InitialValue);
				});
				var sc = si.InitialValue.Match(i => {
					var r = selector(i).Observe(oc => s(oc.Apply(x => projector(i, x))));
					return Observing.Create(r.InitialValue.Apply(x => projector(i, x)), r.Stop);
				}, () => Observing.Create(Option<TR>.Empty, () => { }));
				stopBase = sc.Stop;
				var stopSelector = si.Stop;
				return Observing.Create(sc.InitialValue, () => { stopSelector(); stopSelector = null; stopBase(); stopBase = null; });
			});
		}
		sealed class Listener : DependencyObject {
			readonly Action<DependencyPropertyChangedEventArgs> changed;
			public Listener(Action<DependencyPropertyChangedEventArgs> changed) {
				this.changed = changed;
			}
			static readonly object DefaultValue = new object();
			public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(Listener), new PropertyMetadata(DefaultValue,
				(d, e) => { if(DefaultValue != e.OldValue && DefaultValue != e.NewValue) ((Listener)d).changed(e); }
			));
		}
		static readonly DependencyProperty ListenersProperty = DependencyProperty.RegisterAttached("Listeners", typeof(HashSet<Listener>), typeof(Observable), new PropertyMetadata(null));
		static HashSet<Listener> GetListeners(DependencyObject d) {
			var listeners = (HashSet<Listener>)d.GetValue(ListenersProperty);
			if(listeners != null) return listeners;
			listeners = new HashSet<Listener>();
			d.SetValue(ListenersProperty, listeners);
			return listeners;
		}
		public static Observable<DependencyPropertyChangedEventArgs> OnChanged(this DependencyObject d, DependencyProperty p) {
			return new Observable<DependencyPropertyChangedEventArgs>(s => {
				var v = d.GetValue(p);
				var e = new DependencyPropertyChangedEventArgs(p, v, v).AsOption();
				if(s == null)
					return Observing.Create(e, () => { });
				var listener = new Listener(x => s(x.AsOption()));
				BindingOperations.SetBinding(listener, Listener.ValueProperty, new Binding() { Path = new PropertyPath(p), Source = d, Mode = BindingMode.OneWay });
				GetListeners(d).Add(listener);
				return Observing.Create(e, () => {
					BindingOperations.ClearBinding(listener, Listener.ValueProperty);
					GetListeners(d).Remove(listener);
				});
			});
		}
		public static Observable<NotifyCollectionChangedEventArgs> OnChanged(this INotifyCollectionChanged collection) {
			return new Observable<NotifyCollectionChangedEventArgs>(s => {
				var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset).AsOption();
				if(s == null)
					return Observing.Create(e, () => { });
				NotifyCollectionChangedEventHandler h = (_, x) => s(x.AsOption());
				collection.CollectionChanged += h;
				return Observing.Create(e, () => collection.CollectionChanged -= h);
			});
		}
		public static Observable<PropertyChangedEventArgs> OnChanged<TOwner, T>(this TOwner obj, Expression<Func<TOwner, T>> property) where TOwner : INotifyPropertyChanged {
			return OnChanged(obj, ExpressionHelper.GetPropertyName(property));
		}
		public static Observable<PropertyChangedEventArgs> OnChanged(this INotifyPropertyChanged obj, string propertyName) {
			return new Observable<PropertyChangedEventArgs>(s => {
				var e = new PropertyChangedEventArgs(propertyName).AsOption();
				if(s == null)
					return Observing.Create(e, () => { });
				PropertyChangedEventHandler h = (_, x) => {
					if(string.IsNullOrEmpty(x.PropertyName) || string.IsNullOrEmpty(propertyName) || propertyName == x.PropertyName)
						s(x.AsOption());
				};
				obj.PropertyChanged += h;
				return Observing.Create(e, () => obj.PropertyChanged -= h);
			});
		}
		public static Observable<T> NotifyValue<TOwner, T>(this TOwner obj, Expression<Func<TOwner, T>> property) where TOwner : INotifyPropertyChanged {
			var getValue = property.Compile();
			return OnChanged(obj, property).Select(_ => getValue(obj));
		}
		public static Observable<T> DependencyValue<TOwner, T>(this TOwner obj, Expression<Func<TOwner, T>> property) where TOwner : DependencyObject {
			var propertyName = ExpressionHelper.GetPropertyName(property);
			var depProperty = (DependencyProperty)obj.GetType().GetField(propertyName + "Property", BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy).GetValue(null);
			return OnChanged(obj, depProperty).Select(_ => (T)obj.GetValue(depProperty));
		}
		public static Observable<object> DependencyValue(this DependencyObject obj, DependencyProperty property) {
			return OnChanged(obj, property).Select(_ => obj.GetValue(property));
		}
		public static Observable<T> Memoize<T>(this Observable<T> observable, Func<T, T, bool> equals = null) {
			equals = equals ?? ((a, b) => Equals(a, b));
			var memoizedValue = Option<Option<T>>.Empty;
			return new Observable<T>(s => {
				var r = observable.Observe(v => {
					if(memoizedValue.Match(u => u.Match(uv => v.Match(vv => equals(uv, vv), () => false), () => v.Match(_ => false, () => true)), () => false)) return;
					memoizedValue = v.AsOption();
					s(v);
				});
				memoizedValue = r.InitialValue.AsOption();
				return r;
			});
		}
		public static Observable<T> Trigger<T>(this Observable<T>[] input) {
			return new Observable<T>(s => {
				var stop = input.Select(x => x.Observe(s).Stop).Aggregate<Action, Action>(() => { }, (a, b) => a + b);
				return Observing.Create(Option<T>.Empty, stop);
			});
		}
		public static Observable<T> Trigger<TCollection, TItem, T>(this TCollection collection, Func<TItem, Observable<T>> item) where TCollection : IList<TItem>, INotifyCollectionChanged {
			return new Observable<T>(s => {
				List<Action> stopItems = null;
				var stopCollection = collection.OnChanged().Observe(oc => oc.Match(c => {
					switch(c.Action) {
					case NotifyCollectionChangedAction.Reset:
						stopItems.ForEach(x => x());
						stopItems = collection.Select(x => item(x).Observe(s).Stop).ToList();
						break;
					case NotifyCollectionChangedAction.Add:
						stopItems.InsertRange(c.NewStartingIndex, c.NewItems.Cast<TItem>().Select(x => item(x).Observe(s).Stop));
						break;
					case NotifyCollectionChangedAction.Remove:
						stopItems.Skip(c.OldStartingIndex).Take(c.OldItems.Count).ForEach(x => x());
						stopItems.RemoveRange(c.OldStartingIndex, c.OldItems.Count);
						break;
					case NotifyCollectionChangedAction.Replace:
						c.NewItems.Cast<TItem>().ForEach((e, i) => { stopItems[c.OldStartingIndex + i](); stopItems[c.OldStartingIndex + i] = item(e).Observe(s).Stop; });
						break;
					case NotifyCollectionChangedAction.Move:
						stopItems.Skip(c.OldStartingIndex).Take(c.OldItems.Count).ForEach(x => x());
						stopItems.RemoveRange(c.OldStartingIndex, c.OldItems.Count);
						stopItems.InsertRange(c.NewStartingIndex, c.NewItems.Cast<TItem>().Select(x => item(x).Observe(s).Stop));
						break;
					}
				}, () => {
					stopItems.ForEach(x => x());
					stopItems.Clear();
				})).Stop;
				stopItems = collection.Select(x => item(x).Observe(s).Stop).ToList();
				return Observing.Create(Option<T>.Empty, () => { stopCollection(); stopCollection = null; stopItems.ForEach(x => x()); stopItems = null; });
			});
		}
		public static Observable<T> InitialValue<T>(this Observable<T> observable, T initialValue) {
			return new Observable<T>(s => Observing.Create(initialValue.AsOption(), observable.Observe(s).Stop));
		}
		public static Observable<UnitT> Hierarchy<TCollection, TItem>(this TCollection collection, Func<TItem, Observable<UnitT>> item) where TCollection : IList<TItem>, INotifyCollectionChanged {
			return collection.Trigger(item).InitialValue(default(UnitT)).SelectMany(() => collection.OnChanged()).Void();
		}
		public static Observable<UnitT> Void<T>(this Observable<T> observable) {
			return observable.Select(_ => default(UnitT));
		}
	}
}
