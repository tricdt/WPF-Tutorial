﻿using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Converter;
using Syncfusion.XlsIO;
using System.Windows.Input;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public static class ExportSelectedCommand
    {
        static ExportSelectedCommand()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfDataGrid), new CommandBinding(ExportToExcel, OnExecuteExportToExcel, OnCanExecuteExportToExcel));
        }

        private static void OnCanExecuteExportToExcel(object sender, CanExecuteRoutedEventArgs e)
        {
            var grid = e.Source as SfDataGrid;
            if (grid.SelectedItems != null && grid.SelectedItems.Any())
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private static void OnExecuteExportToExcel(object sender, ExecutedRoutedEventArgs args)
        {
            var dataGrid = args.Source as SfDataGrid;
            var optionsettings = args.Parameter as ExcelExportingOptionsWrapper;
            if (dataGrid == null || optionsettings == null)
                return;

            var options = new ExcelExportingOptions();
            options.ExcelVersion = ExcelVersion.Excel2010;
            options.AllowOutlining = optionsettings.AllowOutlining;
            if (!optionsettings.CanCustomizeStyle)
                options.ExportingEventHandler = ExportingHandler;
            else
                options.ExportingEventHandler = CustomizeExportingHandler;

            options.CellsExportingEventHandler = CellExportingHandler;

            ExcelEngine excelEngine = new ExcelEngine();
            IWorkbook workBook = excelEngine.Excel.Workbooks.Create();
            workBook.Worksheets.Create();

            dataGrid.ExportToExcel(dataGrid.SelectedItems, options, workBook.Worksheets[0]);
            ExcelExportCommand.SaveFile(workBook);
        }

        private static void CellExportingHandler(object sender, GridCellExcelExportingEventArgs e)
        {
            e.Range.CellStyle.Font.Size = 12;
            e.Range.CellStyle.Font.FontName = "Segoe UI";

            if (e.ColumnName == "UnitPrice" || e.ColumnName == "Quantity")
            {
                if (e.CellType == ExportCellType.HeaderCell)
                    return;
                double value = 0;
                if (double.TryParse(e.CellValue.ToString(), out value))
                {
                    e.Range.Number = value;
                }
                e.Handled = true;
            }
        }

        private static void CustomizeExportingHandler(object sender, GridExcelExportingEventArgs e)
        {
            if (e.CellType == ExportCellType.RecordCell)
            {
                e.CellStyle.BackGroundBrush = new SolidColorBrush(Colors.Violet);
                e.Handled = true;
            }
        }


        private static void ExportingHandler(object sender, GridExcelExportingEventArgs e)
        {
            if (e.CellType == ExportCellType.HeaderCell)
            {
                e.CellStyle.BackGroundBrush = new SolidColorBrush(Colors.LightSteelBlue);
                e.CellStyle.ForeGroundBrush = new SolidColorBrush(Colors.DarkRed);
                e.CellStyle.FontInfo.Bold = true;
            }
            e.CellStyle.FontInfo.Size = 12;
            e.CellStyle.FontInfo.FontName = "Segoe UI";
            e.Handled = true;
        }

        public static RoutedCommand ExportToExcel = new RoutedCommand("ExportToExcel", typeof(SfDataGrid));
    }
}
