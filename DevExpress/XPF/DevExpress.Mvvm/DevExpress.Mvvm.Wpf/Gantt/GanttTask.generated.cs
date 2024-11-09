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

namespace DevExpress.Mvvm.Gantt {
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
	partial class GanttPredecessorLink {
		object _PredecessorTaskId = null;
		public object PredecessorTaskId {
			get { return _PredecessorTaskId; }
			set {
				if(Equals(value, _PredecessorTaskId)) return;
				_PredecessorTaskId = value;
				RaisePropertyChanged(nameof(PredecessorTaskId));
			}
		}
		PredecessorLinkType _LinkType = PredecessorLinkType.FinishToStart;
		[DefaultValue(PredecessorLinkType.FinishToStart)]
		public PredecessorLinkType LinkType {
			get { return _LinkType; }
			set {
				if(value == _LinkType) return;
				_LinkType = value;
				RaisePropertyChanged(nameof(LinkType));
			}
		}
		TimeSpan _Lag = default(TimeSpan);
		public TimeSpan Lag {
			get { return _Lag; }
			set {
				if(value == _Lag) return;
				_Lag = value;
				RaisePropertyChanged(nameof(Lag));
			}
		}
	}
	partial class GanttResource {
		string _Name = null;
		public string Name {
			get { return _Name; }
			set {
				if(value == _Name) return;
				_Name = value;
				RaisePropertyChanged(nameof(Name));
			}
		}
		object _Id = null;
		public object Id {
			get { return _Id; }
			set {
				if(Equals(value, _Id)) return;
				_Id = value;
				RaisePropertyChanged(nameof(Id));
			}
		}
		object _Color = null;
		public object Color {
			get { return _Color; }
			set {
				if(Equals(value, _Color)) return;
				_Color = value;
				RaisePropertyChanged(nameof(Color));
			}
		}
	}
	partial class GanttResourceLink {
		object _ResourceId = null;
		public object ResourceId {
			get { return _ResourceId; }
			set {
				if(Equals(value, _ResourceId)) return;
				_ResourceId = value;
				RaisePropertyChanged(nameof(ResourceId));
			}
		}
		double _AllocationPercentage = 1.0;
		[DefaultValue(1.0)]
		public double AllocationPercentage {
			get { return _AllocationPercentage; }
			set {
				if(value == _AllocationPercentage) return;
				_AllocationPercentage = value;
				RaisePropertyChanged(nameof(AllocationPercentage));
			}
		}
	}
}
