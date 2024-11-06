﻿using Microsoft.Win32;
using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.UI.Xaml.TreeGrid.Converter;
using Syncfusion.XlsIO;
using System.IO;
using System.Windows;
using System.Windows.Input;
namespace syncfusion.treegriddemos.wpf
{
    public static class ExcelExportCommand
    {
        static ExcelExportCommand()
        {
            CommandManager.RegisterClassCommandBinding(typeof(SfTreeGrid), new CommandBinding(ExportToExcel, OnExecuteExportToExcel, OnCanExecuteExportToExcel));
        }

        #region ExportToExcel Command

        public static RoutedCommand ExportToExcel = new RoutedCommand("ExportToExcel", typeof(SfTreeGrid));

        private static void OnExecuteExportToExcel(object sender, ExecutedRoutedEventArgs args)
        {
            var treeGrid = args.Source as SfTreeGrid;
            var optionsettings = args.Parameter as ExcelExportingOptionsWrapper;
            if (treeGrid == null || optionsettings == null)
                return;

            try
            {
                var options = new TreeGridExcelExportingOptions();
                options.ExcelVersion = ExcelVersion.Excel2016;
                options.ExportingEventHandler = ExportingHandler;
                options.AllowIndentColumn = optionsettings.AllowIndentColumn;
                options.AllowOutliningGroups = optionsettings.AllowOutlining;
                options.IsGridLinesVisible = optionsettings.isgridLinesVisible;
                options.NodeExpandMode = (NodeExpandMode)optionsettings.NodeExpandModeIndex;
                if (!optionsettings.CanCustomizeStyle)
                    options.CellsExportingEventHandler = CellExportingHandler;
                else
                    options.CellsExportingEventHandler = CustomizeCellExportingHandler;

                var excelEngine = treeGrid.ExportToExcel(options);

                var workBook = excelEngine.Excel.Workbooks[0];
                SaveFile(workBook);
            }
            catch (Exception)
            {

            }
        }

        private static void OnCanExecuteExportToExcel(object sender, CanExecuteRoutedEventArgs args)
        {
            args.CanExecute = true;
        }

        public static void SaveFile(IWorkbook workbook)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                FilterIndex = 2,
                Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2010 Files(*.xlsx)|*.xlsx",
                FileName = "Book1"
            };

            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    if (sfd.FilterIndex == 1)
                        workbook.Version = ExcelVersion.Excel97to2003;
                    else
                        workbook.Version = ExcelVersion.Excel2010;
                    workbook.SaveAs(stream);
                }

                //Message box confirmation to view the created spreadsheet.
                if (MessageBox.Show("Do you want to view the workbook?", "Workbook has been created",
                                    MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                    System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(sfd.FileName);
                    info.UseShellExecute = true;
                    System.Diagnostics.Process.Start(info);
                }
            }
        }


        private static void CustomizeCellExportingHandler(object sender, TreeGridCellExcelExportingEventArgs e)
        {
            if (e.CellType == TreeGridCellType.RecordCell && e.ColumnName == "Salary")
                e.Range.CellStyle.ColorIndex = ExcelKnownColors.Sky_blue;
        }
        private static void CellExportingHandler(object sender, TreeGridCellExcelExportingEventArgs e)
        {
            if (e.CellType == TreeGridCellType.RecordCell && e.ColumnName == "Title")
                e.Range.CellStyle.Font.FontName = "Segoe UI";
        }
        private static void ExportingHandler(object sender, TreeGridExcelExportingEventArgs e)
        {
            if (e.CellType == TreeGridCellType.HeaderCell)
            {
                e.Style.ColorIndex = ExcelKnownColors.Light_yellow;
                e.Style.Font.Color = ExcelKnownColors.Dark_red;
                e.Style.Font.Bold = true;
                e.Style.Font.Size = 12;
                e.Style.Font.FontName = "Segoe UI";
                e.Handled = true;
            }

            else if (e.CellType == TreeGridCellType.RecordCell)
            {
                e.Style.Font.Color = ExcelKnownColors.Dark_red;
                e.Handled = true;
            }
        }

        #endregion 

    }
}
