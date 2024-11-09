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
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Diagnostics;
namespace RunNonElevated {
	public class UAC {
		[StructLayout(LayoutKind.Sequential)]
		struct LUID {
			public uint LowPart;
			public int HighPart;
		}
		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool LookupPrivilegeValue(string lpSystemName, string lpName,
			out LUID lpLuid);
		const UInt32 SE_PRIVILEGE_ENABLED = 0x00000002;
		[StructLayout(LayoutKind.Sequential)]
		struct LUID_AND_ATTRIBUTES {
			public LUID Luid;
			public UInt32 Attributes;
		}
		const Int32 ANYSIZE_ARRAY = 1;
		struct TOKEN_PRIVILEGES {
			public UInt32 PrivilegeCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = ANYSIZE_ARRAY)]
			public LUID_AND_ATTRIBUTES[] Privileges;
		}
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
		   [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
		   ref TOKEN_PRIVILEGES NewState,
		   UInt32 Zero,
		   IntPtr Null1,
		   IntPtr Null2);
		[DllImport("user32.dll")]
		static extern IntPtr GetShellWindow();
		const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
		const int ERROR_NOT_ALL_ASSIGNED = 1300;
		[DllImport("user32.dll", SetLastError = true)]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		[Flags()]
		enum ProcessAccessFlags : int {
			AllAccess = CreateThread | DuplicateHandle | QueryInformation | SetInformation
				| Terminate | VMOperation | VMRead | VMWrite | Synchronize,
			CreateThread = 0x2,
			DuplicateHandle = 0x40,
			QueryInformation = 0x400,
			SetInformation = 0x200,
			Terminate = 0x1,
			VMOperation = 0x8,
			VMRead = 0x10,
			VMWrite = 0x20,
			Synchronize = 0x100000
		}
		[DllImport("kernel32.dll")]
		static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] 
			bool bInheritHandle, uint dwProcessId);
		[Flags()]
		enum TokenAccessFlags : int {
			STANDARD_RIGHTS_REQUIRED = 0x000F0000,
			STANDARD_RIGHTS_READ = 0x00020000,
			TOKEN_ASSIGN_PRIMARY = 0x0001,
			TOKEN_DUPLICATE = 0x0002,
			TOKEN_IMPERSONATE = 0x0004,
			TOKEN_QUERY = 0x0008,
			TOKEN_QUERY_SOURCE = 0x0010,
			TOKEN_ADJUST_PRIVILEGES = 0x0020,
			TOKEN_ADJUST_GROUPS = 0x0040,
			TOKEN_ADJUST_DEFAULT = 0x0080,
			TOKEN_ADJUST_SESSIONID = 0x0100,
			TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY),
			TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY |
				TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE |
				TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT |
				TOKEN_ADJUST_SESSIONID)
		}
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool OpenProcessToken(IntPtr ProcessHandle,
			TokenAccessFlags DesiredAccess, out IntPtr TokenHandle);
		enum SECURITY_IMPERSONATION_LEVEL {
			SecurityAnonymous,
			SecurityIdentification,
			SecurityImpersonation,
			SecurityDelegation
		}
		enum TOKEN_TYPE {
			TokenPrimary = 1,
			TokenImpersonation
		}
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool DuplicateTokenEx(
			IntPtr hExistingToken,
			TokenAccessFlags dwDesiredAccess,
			IntPtr lpThreadAttributes,
			SECURITY_IMPERSONATION_LEVEL ImpersonationLevel,
			TOKEN_TYPE TokenType,
			out IntPtr phNewToken);
		[Flags]
		enum CreationFlags {
			CREATE_BREAKAWAY_FROM_JOB = 0x01000000,
			CREATE_DEFAULT_ERROR_MODE = 0x04000000,
			CREATE_NEW_CONSOLE = 0x00000010,
			CREATE_NEW_PROCESS_GROUP = 0x00000200,
			CREATE_NO_WINDOW = 0x08000000,
			CREATE_PROTECTED_PROCESS = 0x00040000,
			CREATE_PRESERVE_CODE_AUTHZ_LEVEL = 0x02000000,
			CREATE_SEPARATE_WOW_VDM = 0x00001000,
			CREATE_SUSPENDED = 0x00000004,
			CREATE_UNICODE_ENVIRONMENT = 0x00000400,
			DEBUG_ONLY_THIS_PROCESS = 0x00000002,
			DEBUG_PROCESS = 0x00000001,
			DETACHED_PROCESS = 0x00000008,
			EXTENDED_STARTUPINFO_PRESENT = 0x00080000
		}
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct STARTUPINFO {
			public Int32 cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public Int32 dwX;
			public Int32 dwY;
			public Int32 dwXSize;
			public Int32 dwYSize;
			public Int32 dwXCountChars;
			public Int32 dwYCountChars;
			public Int32 dwFillAttribute;
			public Int32 dwFlags;
			public Int16 wShowWindow;
			public Int16 cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}
		[StructLayout(LayoutKind.Sequential)]
		struct PROCESS_INFORMATION {
			public IntPtr hProcess;
			public IntPtr hThread;
			public int dwProcessId;
			public int dwThreadId;
		}
		[DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		static extern bool CloseHandle(IntPtr handle);
		[StructLayout(LayoutKind.Sequential)]
		struct SECURITY_ATTRIBUTES {
			public int nLength;
			public IntPtr lpSecurityDescriptor;
			public bool bInheritHandle;
		}
		[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		static extern bool CreateProcessAsUserW(
			IntPtr hToken,
			string lpApplicationName,
			string lpCommandLine,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes,
			bool bInheritHandles,
			CreationFlags dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);
		enum LogonFlags {
			WithProfile = 1,
			NetCredentialsOnly
		}
		[DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
		static extern bool CreateProcessWithTokenW(
			IntPtr hToken,
			LogonFlags dwLogonFlags,
			string lpApplicationName,
			string lpCommandLine,
			CreationFlags dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			[In] ref STARTUPINFO lpStartupInfo,
			out PROCESS_INFORMATION lpProcessInformation);
		public static int RunAsDesktopUser(string path, string args) {
			IntPtr currentToken;
			int lastError;
			if (!OpenProcessToken(System.Diagnostics.Process.GetCurrentProcess().Handle,
				TokenAccessFlags.TOKEN_ADJUST_PRIVILEGES, out currentToken)) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			LUID myLUID;
			if (!LookupPrivilegeValue(null, SE_INCREASE_QUOTA_NAME, out myLUID)) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			TOKEN_PRIVILEGES myTokenPrivileges;
			myTokenPrivileges.PrivilegeCount = 1;
			myTokenPrivileges.Privileges = new LUID_AND_ATTRIBUTES[1];
			myTokenPrivileges.Privileges[0].Attributes = SE_PRIVILEGE_ENABLED;
			myTokenPrivileges.Privileges[0].Luid = myLUID;
			if (!AdjustTokenPrivileges(currentToken, false, ref myTokenPrivileges, 0,
				IntPtr.Zero, IntPtr.Zero)) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			CloseHandle(currentToken);
			lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
			if (lastError == ERROR_NOT_ALL_ASSIGNED) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			IntPtr shellWindow = GetShellWindow();
			if (shellWindow == IntPtr.Zero) {
				throw new Win32Exception("Unable to get shell window.");
			}
			uint processID = 0;
			GetWindowThreadProcessId(shellWindow, out processID);
			if (processID == 0) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			IntPtr processHandle = OpenProcess(ProcessAccessFlags.QueryInformation, true, processID);
			if (processHandle == IntPtr.Zero) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			IntPtr shellProcessToken;
			TokenAccessFlags tokenAccess = TokenAccessFlags.TOKEN_QUERY | TokenAccessFlags.TOKEN_ASSIGN_PRIMARY |
				TokenAccessFlags.TOKEN_DUPLICATE | TokenAccessFlags.TOKEN_ADJUST_DEFAULT |
				TokenAccessFlags.TOKEN_ADJUST_SESSIONID;
			if (!OpenProcessToken(processHandle, tokenAccess, out shellProcessToken)) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			IntPtr newPrimaryToken;
			if (!DuplicateTokenEx(shellProcessToken, tokenAccess, IntPtr.Zero,
				SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, TOKEN_TYPE.TokenPrimary, out newPrimaryToken)) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			STARTUPINFO startupInfo = new STARTUPINFO();
			startupInfo.cb = System.Runtime.InteropServices.Marshal.SizeOf(startupInfo);
			startupInfo.lpDesktop = "";
			startupInfo.hStdInput = GetStdHandle(-10);
			startupInfo.hStdOutput = GetStdHandle(-11);
			startupInfo.hStdError = GetStdHandle(-12);
			startupInfo.dwFlags = 0x100;
			PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();
			if (!CreateProcessWithTokenW(newPrimaryToken, LogonFlags.WithProfile, path, "\"" + path + "\" " + args, CreationFlags.CREATE_NO_WINDOW, IntPtr.Zero, Environment.CurrentDirectory, ref startupInfo, out processInfo)) {
				lastError = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
				throw new Win32Exception(lastError);
			}
			Process p = Process.GetProcessById(processInfo.dwProcessId);
			p.WaitForExit();
			int exitCode;
			GetExitCodeProcess(processInfo.hProcess, out exitCode);
			CloseHandle(processInfo.hProcess);
			CloseHandle(processInfo.hThread);
			return exitCode;
		}
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool GetExitCodeProcess(IntPtr processHandle, out int exitCode);
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetStdHandle(int whichHandle);
	}
}
