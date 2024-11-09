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
using System.IO;
using System.IO.Pipes;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
namespace DevExpress.Xpf.DemoBase.Helpers {
	public sealed class RunOnceHelper {
		readonly string id;
		Mutex mutex;
		bool needReleaseMutex = false;
		RunOnceHelper(string id) {
			this.id = id;
		}
		public static bool BeginApplicationStart(string id, byte runParameter, Action<RunOnceHelper> run) {
			return new RunOnceHelper(id).BeginApplicationStart(runParameter, run);
		}
		bool BeginApplicationStart(byte runParameter, Action<RunOnceHelper> run) {
			if(mutex != null)
				throw new InvalidOperationException();
			mutex = new Mutex(false, $"{id}_m");
			try {
				mutex.WaitOne();
			} catch(AbandonedMutexException) { }
			var serverConnection = CreateFileW($@"\\.\pipe\{id}_p", FileAccess.Write, FileShare.None, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
			if(serverConnection == new IntPtr(-1)) {
				needReleaseMutex = true;
				try {
					run(this);
				} finally {
					if(needReleaseMutex)
						mutex.ReleaseMutex();
					mutex = null;
				}
				return true;
			}
			uint written;
			WriteFile(serverConnection, new byte[] { runParameter }, 1, out written, IntPtr.Zero);
			CloseHandle(serverConnection);
			mutex.ReleaseMutex();
			mutex = null;
			return false;
		}
		public void EndApplicationStart(Action<byte> onAnotherInstanceRun) {
			if(mutex == null)
				throw new InvalidOperationException();
			var server = new NamedPipeServerStream($"{id}_p", PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.WriteThrough);
			LinqExtensions.WithReturnValue<Action>(waitForConnection => () => {
				server
					.WaitForConnectionAsync(TaskScheduler.FromCurrentSynchronizationContext())
					.SelectMany(() => server.ReadAsync(1, TaskScheduler.FromCurrentSynchronizationContext()))
					.Execute(c => {
						try {
							mutex.WaitOne();
						} catch(AbandonedMutexException) { }
						try {
							server.Disconnect();
							waitForConnection.Value();
							if(c.Count == 1)
								onAnotherInstanceRun(c.Array[0]);
						} finally {
							mutex.ReleaseMutex();
						}
					});
			})();
			mutex.ReleaseMutex();
			needReleaseMutex = false;
		}
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		static extern IntPtr CreateFileW(
			 [MarshalAs(UnmanagedType.LPWStr)] string filename,
			 [MarshalAs(UnmanagedType.U4)] FileAccess access,
			 [MarshalAs(UnmanagedType.U4)] FileShare share,
			 IntPtr securityAttributes,
			 [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			 [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
			 IntPtr templateFile
		);
		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
		[DllImport("kernel32.dll", SetLastError = true)]
#pragma warning disable SYSLIB0004 // Type or member is obsolete
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#pragma warning restore SYSLIB0004 // Type or member is obsolete
		[SuppressUnmanagedCodeSecurity]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool CloseHandle(IntPtr hObject);
	}
}
