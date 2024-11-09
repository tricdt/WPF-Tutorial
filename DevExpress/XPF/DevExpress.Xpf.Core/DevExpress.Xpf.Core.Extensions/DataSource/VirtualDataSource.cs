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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Data;
using System.Reflection;
namespace DevExpress.Xpf.Core.DataSources {
	public class VirtualDataSource : SimpleDataSourceBase {
		static readonly DependencyPropertyKey DataPropertyKey;
		public static readonly DependencyProperty DataProperty;
		public static readonly DependencyProperty RowsCountProperty;
		public static readonly DependencyProperty PropertiesCountProperty;
		static VirtualDataSource() {
			Type ownerclass = typeof(VirtualDataSource);
			DataPropertyKey = DependencyProperty.RegisterReadOnly("Data", typeof(IListSource), ownerclass, new PropertyMetadata(null, (d, e) => ((VirtualDataSource)d).OnDataChanged((IListSource)e.OldValue, (IListSource)e.NewValue)));
			DataProperty = DataPropertyKey.DependencyProperty;
			RowsCountProperty = DependencyProperty.Register("RowsCount", typeof(int), ownerclass, new PropertyMetadata(0, (d, e) => ((VirtualDataSource)d).OnRowsCountChanged((int)e.NewValue)));
			PropertiesCountProperty = DependencyProperty.Register("PropertiesCount", typeof(int), ownerclass, new PropertyMetadata(0, (d, e) => ((VirtualDataSource)d).OnPropertiesCountChanged((int)e.NewValue)));
		}
		public VirtualDataSource() {
			UpdateData();
		}
		[Category(DataCategory), DXDescription("DevExpress.Xpf.Core.DataSources.VirtualDataSource,Data")]
		public IListSource Data {
			get { return (IListSource)GetValue(DataProperty); }
			protected internal set { SetValue(DataPropertyKey, value); }
		}
		[Category(DataCategory), DXDescription("DevExpress.Xpf.Core.DataSources.VirtualDataSource,RowsCount")]
		public int RowsCount {
			get { return (int)GetValue(RowsCountProperty); }
			set { SetValue(RowsCountProperty, value); }
		}
		[Category(DataCategory), DXDescription("DevExpress.Xpf.Core.DataSources.VirtualDataSource,PropertiesCount")]
		public int PropertiesCount {
			get { return (int)GetValue(PropertiesCountProperty); }
			set { SetValue(PropertiesCountProperty, value); }
		}
		protected override object DataCore {
			get { return Data; }
			set { Data = (VirtualSource)value; }
		}
		protected internal VirtualSource Source { get { return Data as VirtualSource; } }
		public void Refresh() {
			UpdateData();
		}
		void OnDataChanged(IListSource oldData, IListSource newData) {
			VirtualSource oldDataSource = oldData as VirtualSource;
			if(oldDataSource != null) {
				oldDataSource.ValueNeeded -= data_ValueNeeded;
				oldDataSource.ValuePushed -= data_ValuePushed;
				oldDataSource.PropertyNeeded -= data_PropertyNeeded;
			}
			VirtualSource newDataSource = newData as VirtualSource;
			if(newDataSource != null) {
				newDataSource.ValueNeeded += data_ValueNeeded;
				newDataSource.ValuePushed += data_ValuePushed;
				newDataSource.PropertyNeeded += data_PropertyNeeded;
			}
			RegenerateProperties();
		}
		void RegenerateProperties() {
			if(Source == null)
				return;
			if(PropertyNeededCore == null) {
				Source.GenerateProperties(0);
				return;
			}
			Source.GenerateProperties(PropertiesCount);
		}
		void OnRowsCountChanged(int newValue) {
			UpdateData();
		}
		void OnPropertiesCountChanged(int newValue) {
			UpdateData();
		}
		void data_PropertyNeeded(object sender, VirtualSourcePropertyNeededEventArgs e) {
			if(PropertyNeededCore != null)
				PropertyNeededCore(sender, e);
		}
		void data_ValuePushed(object sender, VirtualSourceValuePushedEventArgs e) {
			if(ValuePushedCore != null)
				ValuePushedCore(sender, e);
		}
		void data_ValueNeeded(object sender, VirtualSourceValueNeededEventArgs e) {
			if(ValueNeededCore != null)
				ValueNeededCore(sender, e);
		}
		protected override object CreateDesignTimeDataSourceCore() {
			var designTimeSource = new VirtualSource(DesignData.RowCount);
			return designTimeSource;
		}
		protected override object UpdateDataCore() {
			return new VirtualSource(RowsCount);
		}
		protected override Assembly GetImageAssembly() {
			return typeof(SimpleDataSourceBase).Assembly;
		}
		EventHandler<VirtualSourceValueNeededEventArgs> ValueNeededCore;
		EventHandler<VirtualSourceValuePushedEventArgs> ValuePushedCore;
		EventHandler<VirtualSourcePropertyNeededEventArgs> PropertyNeededCore;
		public event EventHandler<VirtualSourceValueNeededEventArgs> ValueNeeded {
			add {
				ValueNeededCore += value;
				RegenerateProperties();
			}
			remove {
				ValueNeededCore -= value;
				RegenerateProperties();
			}
		}
		public event EventHandler<VirtualSourceValuePushedEventArgs> ValuePushed {
			add {
				ValuePushedCore += value;
				RegenerateProperties();
			}
			remove {
				ValuePushedCore -= value;
				RegenerateProperties();
			}
		}
		public event EventHandler<VirtualSourcePropertyNeededEventArgs> PropertyNeeded {
			add {
				PropertyNeededCore += value;
				RegenerateProperties();
			}
			remove {
				PropertyNeededCore -= value;
				RegenerateProperties();
			}
		}
	}
}
