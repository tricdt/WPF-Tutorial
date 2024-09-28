﻿using Microsoft.Xaml.Behaviors;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;

namespace syncfusion.datagriddemos.wpf
{
    public class AutoCellMergeBehavior : Behavior<SfDataGrid>
    {
        /// <summary>
        /// DataGrid defined thats defined in xaml.
        /// </summary>
        SfDataGrid dataGrid = null;

        /// <summary>
        /// to get the particular column data from a record.
        /// </summary>
        IPropertyAccessProvider reflector = null;

        /// <summary>
        /// to remove the conflict range once teh given range has been edited.
        /// </summary>
        bool isEditted = false;

        /// <summary>
        /// To enusre teh current cell value has been changed.
        /// </summary>
        bool isCurrentCellValueChanged = false;
        protected override void OnAttached()
        {
            dataGrid = this.AssociatedObject as SfDataGrid;
            dataGrid.ItemsSourceChanged += DataGrid_ItemsSourceChanged;
            dataGrid.QueryCoveredRange += DataGrid_QueryCoveredRange;
            dataGrid.CurrentCellValueChanged += DataGrid_CurrentCellValueChanged;
            dataGrid.CurrentCellEndEdit += DataGrid_CurrentCellEndEdit;
        }

        private void DataGrid_CurrentCellEndEdit(object? sender, CurrentCellEndEditEventArgs args)
        {
            if (!isCurrentCellValueChanged)
                return;

            var rowIndex = args.RowColumnIndex.RowIndex;
            var columnIndex = args.RowColumnIndex.ColumnIndex;

            var range = dataGrid.CoveredCells.GetCoveredCellInfo(rowIndex, columnIndex);
            dataGrid.RemoveRange(range);
            isEditted = true;
            dataGrid.GetVisualContainer().InvalidateMeasure();
        }

        private void DataGrid_CurrentCellValueChanged(object? sender, CurrentCellValueChangedEventArgs e)
        {
            isCurrentCellValueChanged = true;
        }

        private void DataGrid_ItemsSourceChanged(object? sender, GridItemsSourceChangedEventArgs e)
        {
            if (dataGrid.View != null)
                reflector = dataGrid.View.GetPropertyAccessProvider();
            else
                reflector = null;

            dataGrid.Columns["BirthDate"].TextAlignment = System.Windows.TextAlignment.Right;
            dataGrid.Columns["Rating"].Width = 70;

            dataGrid.Columns["Gender"].Width = 70;
            dataGrid.Columns["Salary"].Width = 75;
            dataGrid.Columns["ContactID"].Width = 90;
            dataGrid.Columns["BirthDate"].Width = 105;
            dataGrid.Columns["EmployeeID"].Width = 100;
            dataGrid.Columns["Name"].Width = 120;
        }

        private void DataGrid_QueryCoveredRange(object? sender, GridQueryCoveredRangeEventArgs e)
        {
            // Merging cells for flat grid                      
            var range = GetRange(e.GridColumn, e.RowColumnIndex.RowIndex, e.RowColumnIndex.ColumnIndex, e.Record);

            if (range == null)
                return;

            // While editing we need to remove the range.
            if (this.dataGrid.CoveredCells.IsInRange(range))
            {
                CoveredCellInfo coveredCellInfo = this.dataGrid.GetConflictRange(range);

                while (coveredCellInfo != null)
                {
                    if (isEditted)
                    {
                        this.dataGrid.CoveredCells.Remove(coveredCellInfo);
                        coveredCellInfo = this.dataGrid.GetConflictRange(range);
                        if (coveredCellInfo == null)
                            isEditted = false;
                    }
                }
            }

            e.Range = range;
            e.Handled = true;
        }
        /// <summary>
        /// Method that reruns coveredcellinfo based on same data.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="rowData"></param>
        /// <returns></returns>
        private CoveredCellInfo GetRange(GridColumn column, int rowIndex, int columnIndex, object rowData)
        {
            var range = new CoveredCellInfo(columnIndex, columnIndex, rowIndex, rowIndex);
            object data = reflector.GetFormattedValue(rowData, column.MappingName);


            GridColumn leftColumn = null;
            GridColumn rightColumn = null;


            // total rows count.
            int recordsCount = this.dataGrid.GroupColumnDescriptions.Count != 0 ? (this.dataGrid.View.TopLevelGroup.DisplayElements.Count + this.dataGrid.TableSummaryRows.Count + this.dataGrid.UnBoundRows.Count + (this.dataGrid.AddNewRowPosition == AddNewRowPosition.Top ? +1 : 0)) : (this.dataGrid.View.Records.Count + this.dataGrid.TableSummaryRows.Count + this.dataGrid.UnBoundRows.Count + (this.dataGrid.AddNewRowPosition == AddNewRowPosition.Top ? +1 : 0));

            // Merge Horizontally
            // compare right column               
            for (int i = dataGrid.Columns.IndexOf(column); i < this.dataGrid.Columns.Count - 1; i++)
            {
                var compareData = reflector.GetFormattedValue(rowData, dataGrid.Columns[i + 1].MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                rightColumn = dataGrid.Columns[i + 1];
            }

            // compare left column.
            for (int i = dataGrid.Columns.IndexOf(column); i > 0; i--)
            {
                var compareData = reflector.GetFormattedValue(rowData, dataGrid.Columns[i - 1].MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                leftColumn = dataGrid.Columns[i - 1];
            }

            if (leftColumn != null || rightColumn != null)
            {
                // set left index
                if (leftColumn != null)
                {
                    var leftColumnIndex = this.dataGrid.ResolveToScrollColumnIndex(this.dataGrid.Columns.IndexOf(leftColumn));
                    range = new CoveredCellInfo(leftColumnIndex, range.Right, range.Top, range.Bottom);
                }

                // set right index
                if (rightColumn != null)
                {
                    var rightColumIndex = this.dataGrid.ResolveToScrollColumnIndex(this.dataGrid.Columns.IndexOf(rightColumn));
                    range = new CoveredCellInfo(range.Left, rightColumIndex, range.Top, range.Bottom);
                }
                return range;
            }

            // Merge Vertically from the row index.

            int previousRowIndex = -1;
            int nextRowIndex = -1;

            // Get previous row data.                
            var startIndex = dataGrid.ResolveStartIndexBasedOnPosition();
            for (int i = rowIndex - 1; i >= startIndex; i--)
            {
                var previousData = this.dataGrid.GetRecordEntryAtRowIndex(i);
                if (previousData == null || !previousData.IsRecords)
                    break;

                var compareData = reflector.GetFormattedValue((previousData as RecordEntry).Data, column.MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                previousRowIndex = i;
            }

            // get next row data.
            for (int i = rowIndex + 1; i < recordsCount + 1; i++)
            {
                var nextData = this.dataGrid.GetRecordEntryAtRowIndex(i);
                if (nextData == null || !nextData.IsRecords)
                    break;

                var compareData = reflector.GetFormattedValue((nextData as RecordEntry).Data, column.MappingName);

                if (compareData == null)
                    break;

                if (!compareData.Equals(data))
                    break;
                nextRowIndex = i;
            }

            if (previousRowIndex != -1 || nextRowIndex != -1)
            {
                if (previousRowIndex != -1)
                    range = new CoveredCellInfo(range.Left, range.Right, previousRowIndex, range.Bottom);

                if (nextRowIndex != -1)
                    range = new CoveredCellInfo(range.Left, range.Right, range.Top, nextRowIndex);
                return range;
            }

            return null;
        }
        /// <summary>
        /// Detaching event for SfDataGrid.
        /// </summary>
        protected override void OnDetaching()
        {
            if (dataGrid != null)
            {
                dataGrid.ItemsSourceChanged -= DataGrid_ItemsSourceChanged;
                dataGrid.QueryCoveredRange -= DataGrid_QueryCoveredRange;
                dataGrid.CurrentCellValueChanged -= DataGrid_CurrentCellValueChanged;
                dataGrid.CurrentCellEndEdit -= DataGrid_CurrentCellEndEdit;
                dataGrid = null;
            }
        }
    }
}
