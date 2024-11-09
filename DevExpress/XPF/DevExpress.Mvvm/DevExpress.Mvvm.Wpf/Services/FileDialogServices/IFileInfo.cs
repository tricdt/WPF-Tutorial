﻿#region Copyright (c) 2000-2024 Developer Express Inc.
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
namespace DevExpress.Mvvm {
	public interface IFileSystemInfo {
		string DirectoryName { get; }
		string Name { get; }
		bool Exists { get; }
		FileAttributes Attributes { get; set; }
		void MoveTo(string destinationFileName);
		void Delete();
	}
	public interface IFolderInfo : IFileSystemInfo {
		string Path { get; }
	}
	public interface IFileInfo : IFileSystemInfo {
		long Length { get; }
		StreamWriter AppendText();
		FileInfo CopyTo(string destinationFileName, bool overwrite);
		FileStream Create();
		StreamWriter CreateText();
		FileStream Open(FileMode mode, FileAccess access, FileShare share);
		FileStream OpenRead();
		StreamReader OpenText();
		FileStream OpenWrite();
	}
	public static class FileInfoExtensions {
		public static FileStream Open(this IFileInfo fileInfo, FileMode mode) {
			Verify(fileInfo);
			return fileInfo.Open(mode, FileAccess.ReadWrite, FileShare.None);
		}
		public static FileStream Open(this IFileInfo fileInfo, FileMode mode, FileAccess access) {
			Verify(fileInfo);
			return fileInfo.Open(mode, access, FileShare.None);
		}
		public static FileInfo CopyTo(this IFileInfo fileInfo, string destinationFileName) {
			Verify(fileInfo);
			return fileInfo.CopyTo(destinationFileName, false);
		}
		public static string GetFullName(this IFileInfo fileInfo) {
			Verify(fileInfo);
			return Path.Combine(fileInfo.DirectoryName, fileInfo.Name);
		}
		internal static void Verify(IFileInfo fileInfo) {
			if(fileInfo == null)
				throw new ArgumentNullException(nameof(fileInfo));
		}
	}
}
