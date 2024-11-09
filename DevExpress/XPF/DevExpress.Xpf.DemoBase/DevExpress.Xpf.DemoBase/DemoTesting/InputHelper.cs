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
using System.Runtime.InteropServices;
namespace DevExpress.XtraTests {
	public enum KeyCodeType { ScanCode, VirtualCode };
	public delegate void InputHelperActionsDelegate();
	public interface IInputHelper {
		InputHelperActionsDelegate BeforeAction { get; set; }
		InputHelperActionsDelegate AfterAction { get; set; }
		void SendKeyDown(uint uCode, KeyCodeType keyCodeType);
		void SendKeyUp(uint uCode, KeyCodeType keyCodeType);
		void SendMouseLDown(int screenX, int screenY);
		void SendMouseLUp(int screenX, int screenY);
		void SendMouseRDown(int screenX, int screenY);
		void SendMouseRUp(int screenX, int screenY);
		void SendMouseMove(int screenX, int screenY, uint flags);
		void SendMouseWheel(int screenX, int screenY, bool forward);
		void SendMouseHWheel(int screenX, int screenY, bool rightward);		
	}
	public class InputHelper : IInputHelper {
		const int
			INPUT_MOUSE = 0,
			INPUT_KEYBOARD = 1,
			INPUT_HARDWARE = 2;
		const uint
			KEYEVENTF_EXTENDEDKEY = 0x0001,
			KEYEVENTF_KEYUP = 0x0002,
			KEYEVENTF_UNICODE = 0x0004,
			KEYEVENTF_SCANCODE = 0x0008;
		const double AbsoluteScreenSize = 65535.0;
		public const uint
			MOUSEEVENTF_MOVE = 0x0001,
			MOUSEEVENTF_LEFTDOWN = 0x0002,
			MOUSEEVENTF_LEFTUP = 0x0004,
			MOUSEEVENTF_RIGHTDOWN = 0x0008,
			MOUSEEVENTF_RIGHTUP = 0x0010,
			MOUSEEVENTF_MIDDLEDOWN = 0x0020,
			MOUSEEVENTF_MIDDLEUP = 0x0040,
			MOUSEEVENTF_XDOWN = 0x0080,
			MOUSEEVENTF_XUP = 0x0100,
			MOUSEEVENTF_WHEEL = 0x0800,
			MOUSEEVENTF_HWHEEL = 0x01000,
			MOUSEEVENTF_VIRTUALDESK = 0x4000,
			MOUSEEVENTF_ABSOLUTE = 0x8000;
		struct INPUT {
			public UInt32 Type;
			public MOUSEKEYBDHARDWAREINPUT Data;
		}
#pragma warning disable 649
		[StructLayout(LayoutKind.Explicit)]
		struct MOUSEKEYBDHARDWAREINPUT {
			[FieldOffset(0)]
			public MOUSEINPUT Mouse;
			[FieldOffset(0)]
			public KEYBDINPUT Keyboard;
			[FieldOffset(0)]
			public HARDWAREINPUT Hardware;
		}
#pragma warning restore 649
		[StructLayout(LayoutKind.Sequential)]
		struct MOUSEINPUT {
			public int dx;
			public int dy;
			public int mouseData;
			public uint dwFlags;
			public uint time;
			public IntPtr dwExtraInfo;
		}
		[StructLayout(LayoutKind.Sequential)]
		struct KEYBDINPUT {
			public ushort wVk;
			public ushort wScan;
			public uint dwFlags;
			public uint time;
			public IntPtr dwExtraInfo;
		}
		[StructLayout(LayoutKind.Sequential)]
		struct HARDWAREINPUT {
			public uint uMsg;
			public ushort wParamL;
			public ushort wParamH;
		}
		InputHelperActionsDelegate beforeAction;
		InputHelperActionsDelegate afterAction;
		public InputHelperActionsDelegate BeforeAction { get { return beforeAction; } set { beforeAction = value; } }
		public InputHelperActionsDelegate AfterAction { get { return afterAction; } set { afterAction = value; } }
		public static int MouseWheelDelta = 120;
		[DllImport("user32.dll", SetLastError = true)]
		static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
		[DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
		static extern int GetSystemMetricsCore(int nIndex);
		[System.Security.SecuritySafeCritical]
		static int GetSystemMetrics(int nIndex) {
			return GetSystemMetricsCore(nIndex);
		}
 	[System.Security.SecuritySafeCritical]
		void SendAction(INPUT input) {
			if(BeforeAction != null)
				BeforeAction.Invoke();
			uint res = SendInput(1, new INPUT[] { input }, Marshal.SizeOf(input));
			if(res != 1) {
				int error = Marshal.GetLastWin32Error();
				throw new Win32Exception(error, String.Format("SendKey returns 0x{0:X8}", error));
			}
			if(AfterAction != null)
				AfterAction.Invoke();
		}
		void SendMouseAction(double x, double y, uint flags, int data) {
			INPUT input = new INPUT();
			input.Type = INPUT_MOUSE;
			input.Data.Mouse.dwFlags = flags;
			input.Data.Mouse.mouseData = data;
			if ((flags & MOUSEEVENTF_ABSOLUTE) == 0) {
				input.Data.Mouse.dx = (int)x;
				input.Data.Mouse.dy = (int)y;
			} else {
				input.Data.Mouse.dx = (int)((x + 0.5) * Math.Abs(AbsoluteScreenSize / GetSystemMetrics(0)));
				input.Data.Mouse.dy = (int)((y + 0.5) * Math.Abs(AbsoluteScreenSize / GetSystemMetrics(1)));
			}
			SendAction(input);
		}
		public void SendMouseLDown(int screenX, int screenY) {
			SendMouseAction(screenX, screenY, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN,0);
		}
		public void SendMouseLUp(int screenX, int screenY) {
			SendMouseAction(screenX, screenY, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, 0);
		}
		public void SendMouseRDown(int screenX, int screenY) {
			SendMouseAction(screenX, screenY, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, 0);
		}
		public void SendMouseRUp(int screenX, int screenY) {
			SendMouseAction(screenX, screenY, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, 0);
		}
		public void SendMouseMove(int screenX, int screenY, uint flags) {
			SendMouseAction(screenX, screenY, flags | MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE, 0);
		}
		public void SendMouseWheel(int screenX, int screenY, bool forward) {
			SendMouseAction(screenX, screenY, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_WHEEL, forward ? MouseWheelDelta : -MouseWheelDelta);
		}
		public void SendMouseHWheel(int screenX, int screenY, bool rightward) {
			SendMouseAction(screenX, screenY, MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_HWHEEL, rightward ? MouseWheelDelta : -MouseWheelDelta);
		}
		void SendKey(uint uCode, KeyCodeType keyCodeType, bool push) {
			INPUT input = new INPUT();
			input.Type = INPUT_KEYBOARD;
			if (keyCodeType == KeyCodeType.VirtualCode) {
				input.Data.Keyboard.dwFlags = 0;
				input.Data.Keyboard.wVk = (ushort)uCode;
				input.Data.Keyboard.wScan = 0;
			} else {
				input.Data.Keyboard.dwFlags = KEYEVENTF_SCANCODE;
				input.Data.Keyboard.wVk = 0;
				if ((uCode & 0xFF00) == 0xE000) {
					input.Data.Keyboard.dwFlags |= KEYEVENTF_EXTENDEDKEY;
				}
				input.Data.Keyboard.wScan = (ushort)(uCode & 0xFF);
			}
			if (!push) {
				input.Data.Keyboard.dwFlags |= KEYEVENTF_KEYUP;
			}
			SendAction(input);
		}
		public void SendKeyDown(uint uCode, KeyCodeType keyCodeType) {
			SendKey(uCode, keyCodeType, true);
		}
		public void SendKeyUp(uint uCode, KeyCodeType keyCodeType) {
			SendKey(uCode, keyCodeType, false);
		}
	}
	public class ArgumentHelper {
		public static bool IsFormTestArguments(string[] args) {
			string lArgs = @"/All;/Start";
			foreach(string arg in args)
				if(lArgs.IndexOf(arg, StringComparison.CurrentCultureIgnoreCase) >= 0) return true;
			return false;
		}
	}
}
