﻿
using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace syncfusion.datagriddemos.wpf
{
    public class CustomPrintManager : GridPrintManager
    {


        public CustomPrintManager(SfDataGrid grid)
            : base(grid)
        {

            grid.PrintSettings.PrintPageHeaderHeight = 25;
            grid.PrintSettings.PrintPageFooterHeight = 25;
            grid.PrintSettings.PrintPageFooterTemplate = grid.Resources["footerTemplate"] as DataTemplate;
            grid.PrintSettings.PrintPageHeaderTemplate = grid.Resources["headerTemplate"] as DataTemplate;
        }

        /// <summary>
        /// Invokes to get the Column's Element to set content as for the Print Grid Cell 
        /// </summary>
        /// <param name="record"></param>
        /// <param name="mappingName"></param>
        /// <returns>object</returns>
        public override ContentControl GetPrintGridCell(object record, string mappingName)
        {
            var index = dataGrid.View.Records.IndexOfRecord(record);
            if (index % 2 == 0)
                return new PrintGridCell() { Background = new SolidColorBrush(Colors.Bisque) };
            return base.GetPrintGridCell(record, mappingName);
        }


        /// <summary>
        /// Invokes to Draw Each cell value and background 
        /// </summary>
        /// <param name="drawingcontext"></param>
        /// <param name="rowsinfolist"></param>
        /// <param name="cellinfo"></param>
        /// <returns></returns>
        protected override void OnRenderCell(DrawingContext drawingcontext, RowInfo rowinfo, CellInfo cellinfo)
        {
            var index = dataGrid.View.Records.IndexOfRecord(rowinfo.Record);
            var rect = new Rect((cellinfo.CellRect).X, (cellinfo.CellRect).Y + 0.5, (cellinfo.CellRect).Width, (cellinfo.CellRect).Height);
            if (index % 2 == 0)
                drawingcontext.DrawGeometry(new SolidColorBrush(Colors.Bisque), new Pen(), new RectangleGeometry(rect));
            base.OnRenderCell(drawingcontext, rowinfo, cellinfo);
        }

    }
}
