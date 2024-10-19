﻿using Microsoft.Xaml.Behaviors;
using Syncfusion.UI.Xaml.Grid;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    public class MappingNameComboBoxTrigger : TargetedTriggerAction<ComboBox>
    {
        protected override void Invoke(object parameter)
        {
            List<String> mappingNameCol = new List<string>();
            SerializationDemo mainwnd = SerializationDemo.demoControl;
            ManipulatorView manipulatorwnd = ManipulatorView.manipulatorView;

            if (manipulatorwnd.addcolarea.Visibility == Visibility.Collapsed)
            {
                foreach (var col in mainwnd.dataGrid.Columns)
                {
                    if (!(col is GridUnBoundColumn) && col is GridTemplateColumn)
                        mappingNameCol.Add(col.HeaderText + " (TemplateColumn)");
                    else if (col is GridUnBoundColumn)
                        mappingNameCol.Add(col.HeaderText + " (UnBoundColumn)");
                    else
                        mappingNameCol.Add(col.HeaderText);

                }
            }
            else
            {
                PropertyInfo[] properties;
                OrderInfo product = new OrderInfo();
                MappingNameDictionary dic = new MappingNameDictionary();
                properties = product.GetType().GetProperties();
                foreach (var property in properties)
                {
                    string headerText;
                    dic.TryGetValue(property.Name, out headerText);
                    if (!string.IsNullOrEmpty(headerText))
                        mappingNameCol.Add(headerText);
                }
                foreach (var col in mainwnd.dataGrid.Columns)
                {
                    if (mappingNameCol.Contains(col.HeaderText))
                        mappingNameCol.Remove(col.HeaderText);
                }
            }
            this.Target.ItemsSource = mappingNameCol;
        }
    }
}
