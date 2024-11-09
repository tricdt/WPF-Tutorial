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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
namespace DevExpress.Mvvm {
	public interface IDelegateCommand : ICommand {
		void RaiseCanExecuteChanged();
	}
	public interface IAsyncCommand : IDelegateCommand {
		bool IsExecuting { get; }
		[EditorBrowsable(EditorBrowsableState.Never), Obsolete("Use the IsCancellationRequested property instead.")]
		bool ShouldCancel { get; }
		CancellationTokenSource CancellationTokenSource { get; }
		bool IsCancellationRequested { get; }
		ICommand CancelCommand { get; }
#if !WINUI
#if DEBUG
		[Obsolete("Use 'await ExecuteAsync' instead.")]
#endif
		[EditorBrowsable(EditorBrowsableState.Never)]
		void Wait(TimeSpan timeout);
#endif
		Task ExecuteAsync(object parameter);
	}
}
#if !WINUI
namespace DevExpress.Mvvm {
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class IAsyncCommandExtensions {
#if DEBUG
		[Obsolete("Use 'await ExecuteAsync' instead.")]
#endif
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void Wait(this IAsyncCommand service) {
			VerifyService(service);
			service.Wait(TimeSpan.FromMilliseconds(-1));
		}
		static void VerifyService(IAsyncCommand service) {
			if(service == null) throw new ArgumentNullException(nameof(service));
		}
	}
}
namespace DevExpress.Mvvm {
	public static class ICommandExtensions {
		public static ICommand<T> ToTypedCommand<T>(this ICommand command) {
			return new Native.TypedCommandWrapper<T>(command);
		}
	}
}
namespace DevExpress.Mvvm.Native {
	public static class TypedCommandHelper {
		public static bool IsTypedCommand(Type type) {
			return IsTypedCommandCore(type) || type.GetInterfaces().Any(IsTypedCommandCore);
		}
		public static Type GetCommandGenericType(Type type) {
			if(IsTypedCommandCore(type))
				return type.GenericTypeArguments.Single();
			var genericType = type.GetInterfaces().Where(x => IsTypedCommandCore(x)).FirstOrDefault()?.GenericTypeArguments.Single();
			return genericType;
		}
		static bool IsTypedCommandCore(Type type) {
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICommand<>);
		}
		public static string ToDisplayString(this Type type) {
			var @namespace = type.Namespace;
			var name = type.Name;
			var i = name.IndexOf('`');
			if(i > 0)
				name = name.Remove(i);
			if(!type.IsGenericType)
				return $"{@namespace}.{name}";
			var genericArgs = string.Join(", ", type.GetGenericArguments().Select(x => x.ToDisplayString()));
			return $"{@namespace}.{name}<{genericArgs}>";
		}
	}
	public class TypedCommandWrapper<T> : ICommand<T> {
		readonly ICommand command;
		event EventHandler CanExecuteChanged;
		event EventHandler ICommand.CanExecuteChanged {
			add {
				if(CanExecuteChanged == null)
					command.CanExecuteChanged += OnCommandCanExecuteChanged;
				CanExecuteChanged += value;
			}
			remove { 
				CanExecuteChanged -= value;
				if(CanExecuteChanged == null)
					command.CanExecuteChanged -= OnCommandCanExecuteChanged;
			}
		}
		void OnCommandCanExecuteChanged(object sender, EventArgs e) {
			CanExecuteChanged(this, e);
		}
		public TypedCommandWrapper(ICommand command) {
			this.command = command;
		}
		bool ICommand<T>.CanExecute(T param) {
			return command.CanExecute(param);
		}
		bool ICommand.CanExecute(object parameter) {
			return command.CanExecute(parameter);
		}
		void ICommand<T>.Execute(T param) {
			command.Execute(param);
		}
		void ICommand.Execute(object parameter) {
			command.Execute(parameter);
		}
	}
}
#endif
