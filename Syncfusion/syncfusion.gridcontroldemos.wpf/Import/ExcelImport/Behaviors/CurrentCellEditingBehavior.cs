﻿using Microsoft.Xaml.Behaviors;
using Syncfusion.Windows.ComponentModel;
using Syncfusion.Windows.Controls.Grid;
using System.Windows;
using System.Windows.Media;
namespace syncfusion.gridcontroldemos.wpf
{
    public class CurrentCellEditingBehavior : Behavior<GridControl>
    {
        private HorizontalAlignment CurrentCellAlignment { get; set; }
        protected override void OnAttached()
        {
            this.AssociatedObject.PrepareRenderCell += AssociatedObject_PrepareRenderCell;
            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
            this.AssociatedObject.CurrentCellStartEditing += new GridCancelRoutedEventHandler(AssociatedObject_CurrentCellStartEditing);
            this.AssociatedObject.CurrentCellEditingComplete += new GridRoutedEventHandler(AssociatedObject_CurrentCellEditingComplete);
            this.AssociatedObject.CurrentCellValidating += new CurrentCellValidatingEventHandler(AssociatedObject_CurrentCellValidating);
            base.OnAttached();
        }



        private void AssociatedObject_SelectionChanged(object sender, GridSelectionChangedEventArgs e)
        {
            if (e.Reason == GridSelectionReason.MouseDown || e.Reason == GridSelectionReason.SetCurrentCell || e.Reason == GridSelectionReason.MouseMove || e.Reason == GridSelectionReason.SelectRange || e.Reason == GridSelectionReason.MouseUp)
            {
                this.AssociatedObject.InvalidateCell(GridRangeInfo.Row(0));
                this.AssociatedObject.InvalidateCell(GridRangeInfo.Col(0));
            }
        }

        private void AssociatedObject_PrepareRenderCell(object sender, GridPrepareRenderCellEventArgs e)
        {
            if (e.Cell.RowIndex == 0 && this.AssociatedObject.Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Col(e.Cell.ColumnIndex)))
            {
                e.Style.Background = Brushes.LightGray;
                e.Style.Font.FontWeight = FontWeights.Bold;
            }
            else if (e.Cell.ColumnIndex == 0 && this.AssociatedObject.Model.SelectedRanges.AnyRangeIntersects(GridRangeInfo.Row(e.Cell.RowIndex)))
            {
                e.Style.Background = Brushes.LightGray;
                e.Style.Font.FontWeight = FontWeights.Bold;
            }
            //To Design Led Control
            //else
            //{
            //    if (e.Style.CellValue.ToString() == "A")
            //    {
            //        e.Style.Background = Brushes.Red;
            //    }
            //}
        }

        private void AssociatedObject_CurrentCellStartEditing(object sender, SyncfusionCancelRoutedEventArgs args)
        {
            GridControl currentgrid = sender as GridControl;
            if (currentgrid != null && currentgrid.CurrentCell.HasCurrentCell)
            {
                var currentcell = currentgrid.CurrentCell;
                var currentCellStyle = currentgrid.Model[currentcell.RowIndex, currentcell.ColumnIndex];
                if (currentCellStyle != null && currentCellStyle.CellType == "FormulaCell" && currentCellStyle.FormulaTag != null)
                {
                    currentCellStyle.HorizontalAlignment = CurrentCellAlignment;
                }
            }
        }

        private void AssociatedObject_CurrentCellEditingComplete(object sender, SyncfusionRoutedEventArgs args)
        {
            GridControl currentgrid = sender as GridControl;
            if (currentgrid != null && currentgrid.CurrentCell.HasCurrentCell)
            {
                var currentcell = currentgrid.CurrentCell;
                var currentCellStyle = currentgrid.Model[currentcell.RowIndex, currentcell.ColumnIndex];
                if (currentCellStyle != null && currentCellStyle.CellType == "FormulaCell" && currentCellStyle.FormulaTag != null)
                {
                    CurrentCellAlignment = currentCellStyle.HorizontalAlignment;
                    currentCellStyle.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                }
            }
        }

        private void AssociatedObject_CurrentCellValidating(object sender, CurrentCellValidatingEventArgs e)
        {
            if (e.Style.HasIntegerEdit)
            {
                if (!OnValidateIntegerEdit(e))
                {
                    ShowDataValidationMsg(e);
                }
            }
            if (e.Style.HasDoubleEdit)
            {
                if (!OnValidateDoubleEdit(e))
                {
                    ShowDataValidationMsg(e);
                }
            }
        }

        private bool OnValidateIntegerEdit(CurrentCellValidatingEventArgs e)
        {
            try
            {
                //if have the value as empty, then we can’t convert it into int or double
                if (e.NewValue != null && string.IsNullOrEmpty(e.NewValue.ToString()))
                    return false;
                var integerEdit = e.Style.IntegerEdit;
                if (integerEdit.IsScrollingOnCircle)
                {
                    if (integerEdit.MinValue == integerEdit.MaxValue)
                    {
                        //Not Equal to
                        if (integerEdit.MinValue == Convert.ToDouble(e.NewValue.ToString()))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //Not between
                        if ((integerEdit.MinValue < Convert.ToDouble(e.NewValue.ToString())) && integerEdit.MaxValue > Convert.ToDouble(e.NewValue.ToString()))
                        {
                            return false;
                        }
                    }
                    return false;
                }
                if (integerEdit.HasMinValue && integerEdit.HasMaxValue && integerEdit.MinValue == integerEdit.MaxValue)
                {
                    if (integerEdit.MinValue == Convert.ToDouble(e.NewValue.ToString()))
                    {
                        return false;
                    }
                    return false;
                }
                if (integerEdit.HasMinValue)
                {
                    if (integerEdit.MinValue > Convert.ToDouble(e.NewValue.ToString()))
                    {
                        return false;
                    }
                }
                if (integerEdit.HasMaxValue)
                {
                    if (integerEdit.MaxValue < Convert.ToDouble(e.NewValue.ToString()))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        private bool OnValidateDoubleEdit(CurrentCellValidatingEventArgs e)
        {
            try
            {
                //if have the value as empty, then we can’t convert it into int or double
                if (e.NewValue != null && string.IsNullOrEmpty(e.NewValue.ToString()))
                    return false;
                var doubleEdit = e.Style.DoubleEdit;
                if (doubleEdit.IsScrollingOnCircle)
                {
                    if (doubleEdit.MinValue == doubleEdit.MaxValue)
                    {
                        //Not Equal to
                        if (doubleEdit.MinValue == Convert.ToDouble(e.NewValue.ToString()))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //Not between
                        if ((doubleEdit.MinValue < Convert.ToDouble(e.NewValue.ToString())) && doubleEdit.MaxValue > Convert.ToDouble(e.NewValue.ToString()))
                        {
                            return false;
                        }
                    }
                    return false;
                }
                if (doubleEdit.HasMinValue && doubleEdit.HasMaxValue && doubleEdit.MinValue == doubleEdit.MaxValue)
                {
                    if (doubleEdit.MinValue == Convert.ToDouble(e.NewValue.ToString()))
                    {
                        return false;
                    }
                    return false;
                }
                if (doubleEdit.HasMinValue)
                {
                    if (doubleEdit.MinValue > Convert.ToDouble(e.NewValue.ToString()))
                    {
                        return false;
                    }
                }
                if (doubleEdit.HasMaxValue)
                {
                    if (doubleEdit.MaxValue < Convert.ToDouble(e.NewValue.ToString()))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void ShowDataValidationMsg(CurrentCellValidatingEventArgs e)
        {
            MessageBoxResult result;
            if (!string.IsNullOrEmpty(e.Style.ErrorAlertText))
                result = MessageBox.Show(e.Style.ErrorAlertText, e.Style.ErrorAlertTitle, MessageBoxButton.OKCancel);
            else
                result = MessageBox.Show("Invalid cell value", "InvalidCellValue", MessageBoxButton.OKCancel);

            if (result != MessageBoxResult.OK)
            {
                if (e.OldValue != null && !string.IsNullOrEmpty(e.OldValue.ToString()))
                    e.NewValue = e.OldValue;
                else
                    e.NewValue = null;
            }
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.PrepareRenderCell -= AssociatedObject_PrepareRenderCell;
            this.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            this.AssociatedObject.CurrentCellStartEditing -= new GridCancelRoutedEventHandler(AssociatedObject_CurrentCellStartEditing);
            this.AssociatedObject.CurrentCellEditingComplete -= new GridRoutedEventHandler(AssociatedObject_CurrentCellEditingComplete);
            this.AssociatedObject.CurrentCellValidating -= new CurrentCellValidatingEventHandler(AssociatedObject_CurrentCellValidating);
            base.OnDetaching();
        }
    }
}
