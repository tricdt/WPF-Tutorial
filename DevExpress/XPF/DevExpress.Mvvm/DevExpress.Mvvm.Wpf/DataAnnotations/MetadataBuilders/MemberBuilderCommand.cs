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

using DevExpress.Mvvm.Native;
using System;
using System.Linq.Expressions;
namespace DevExpress.Mvvm.DataAnnotations {
	public abstract class CommandMethodMetadataBuilderBase<T, TBuilder> :
		CommandMetadataBuilderBase<T, TBuilder> 
		where TBuilder : CommandMethodMetadataBuilderBase<T, TBuilder> {
		readonly string methodName;
		internal CommandMethodMetadataBuilderBase(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent, string methodName) 
			: base(storage, parent) {
			this.methodName = methodName;
		}
		public TBuilder CanExecuteMethod(Expression<Func<T, bool>> canExecuteMethodExpression) {
			return AddOrModifyAttribute<CommandAttribute>(x => x.CanExecuteMethod = ExpressionHelper.GetArgumentFunctionStrict(canExecuteMethodExpression));
		}
		public TBuilder CommandName(string commandName) {
			return AddOrModifyAttribute<CommandAttribute>(x => x.Name = commandName);
		}
		public TBuilder UseMethodNameAsCommandName() {
			return CommandName(methodName);
		}
		public TBuilder DoNotUseCommandManager() {
			return AddOrModifyAttribute<CommandAttribute>(x => x.UseCommandManager = false);
		}
		public TBuilder DoNotCreateCommand() {
			return AddOrReplaceAttribute(new CommandAttribute(false));
		}
	}
	public class CommandMethodMetadataBuilder<T> :
		CommandMethodMetadataBuilderBase<T, CommandMethodMetadataBuilder<T>> {
		internal CommandMethodMetadataBuilder(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent, string methodName)
			: base(storage, parent, methodName) {
		}
	}
	public class AsyncCommandMethodMetadataBuilder<T> :
		CommandMethodMetadataBuilderBase<T, AsyncCommandMethodMetadataBuilder<T>> {
		internal AsyncCommandMethodMetadataBuilder(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent, string methodName)
			: base(storage, parent, methodName) {
		}
		public AsyncCommandMethodMetadataBuilder<T> AllowMultipleExecution() {
			return AddOrModifyAttribute<CommandAttribute>(x => x.AllowMultipleExecutionCore = true);
		}
	}
#if !FREE
	public class CommandMetadataBuilder<T> : 
		CommandMetadataBuilderBase<T, CommandMetadataBuilder<T>> {
		internal CommandMetadataBuilder(MemberMetadataStorage storage, ClassMetadataBuilder<T> parent)
			: base(storage, parent) {
		}
	}
#endif
}
