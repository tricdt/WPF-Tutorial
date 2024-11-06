﻿using Microsoft.Win32;
using syncfusion.demoscommon.wpf;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Windows.Controls.Grid;
using Syncfusion.Windows.Controls.Grid.Converter;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
namespace syncfusion.gridcontroldemos.wpf
{
    /// <summary>
    /// Interaction logic for PdfExport.xaml
    /// </summary>
    public partial class PdfExport : DemoControl
    {
        Random random;
        DataTable dataTable;
        public PdfExport()
        {
            InitializeComponent();
            Width = 1000;
            Height = 600;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.LoadData();
            this.grid.Model.TableStyle.ReadOnly = true;
            this.grid.Model.RowCount = dataTable.Rows.Count;
            this.grid.Model.ColumnCount = dataTable.Columns.Count;
            this.grid.Model.RowHeights.DefaultLineSize = 29;
            this.grid.Model.ColumnWidths.DefaultLineSize = 115;
            this.grid.Model.ColumnWidths[0] = 115;
            this.grid.QueryCellInfo += new GridQueryCellInfoEventHandler(Grid_QueryCellInfo);
        }

        public PdfExport(string themename) : base(themename)
        {
            InitializeComponent();
            Width = 1000;
            Height = 600;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.LoadData();
            this.grid.Model.TableStyle.ReadOnly = true;
            this.grid.Model.RowCount = dataTable.Rows.Count;
            this.grid.Model.ColumnCount = dataTable.Columns.Count;
            this.grid.Model.RowHeights.DefaultLineSize = 29;
            this.grid.Model.ColumnWidths.DefaultLineSize = 115;
            this.grid.Model.ColumnWidths[0] = 115;
            this.grid.QueryCellInfo += new GridQueryCellInfoEventHandler(Grid_QueryCellInfo);
        }

        private void Grid_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (e.Style.ColumnIndex == 0 || e.Style.RowIndex == 0)
            {
                e.Style.CellType = "DataBoundTemplate";
                e.Style.CellItemTemplateKey = "TextBlocktemplate";
                if (e.Style.RowIndex == 0)
                    e.Style.CellValue = dataTable.Columns[e.Style.ColumnIndex];
                else
                    e.Style.CellValue = dataTable.Rows[e.Style.RowIndex][e.Style.ColumnIndex];
            }
            else
            {
                e.Style.CellValue = dataTable.Rows[e.Style.RowIndex][e.Style.ColumnIndex];
                e.Style.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                e.Style.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                e.Style.DataValidationTooltip = " " + dataTable.Rows[e.Style.RowIndex][0].ToString() +
                                ": \n Population rate in " +
                                dataTable.Columns[e.Style.ColumnIndex] + " is " +
                                e.Style.CellValue.ToString();
                e.Style.ShowDataValidationTooltip = true;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = "Adobe PDF Files(*.pdf)|*.pdf",
                FilterIndex = 1,
                RestoreDirectory = true
            };
            PdfDocument document;
            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    DrawingHeaderFooter();
                    document = this.grid.Model.ExportToPdfGridDocument(GridRangeInfo.Table());
                    document.Save(stream);
                    stream.Close();
                    ProcessStartInfo info = new ProcessStartInfo(sfd.FileName);
                    info.UseShellExecute = true;
                    Process.Start(info);
                    //Process.Start(sfd.FileName);
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = "Adobe PDF Files(*.pdf)|*.pdf",
                FilterIndex = 1
            };
            PdfDocument document;
            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    DrawingHeaderFooter();
                    document = this.grid.Model.ExportToPdfLightTableDocument(GridRangeInfo.Table());
                    document.Save(stream);
                    Process.Start(sfd.FileName);
                }
            }
        }

        private void DrawingHeaderFooter()
        {
            if (checkBox1.IsChecked == true)
                GridPdfExportExtension.DrawPdfHeader += new DrawPdfHeaderFooterEventHandler(GridPdfExportExtension_DrawPdfHeader);
            else
                GridPdfExportExtension.DrawPdfHeader -= new DrawPdfHeaderFooterEventHandler(GridPdfExportExtension_DrawPdfHeader);

            if (checkBox2.IsChecked == true)
                GridPdfExportExtension.DrawPdfFooter += new DrawPdfHeaderFooterEventHandler(GridPdfExportExtension_DrawPdfFooter);
            else
                GridPdfExportExtension.DrawPdfFooter -= new DrawPdfHeaderFooterEventHandler(GridPdfExportExtension_DrawPdfFooter);
        }

        void GridPdfExportExtension_DrawPdfHeader(object sender, PdfHeaderFooterEventArgs e)
        {
            PdfPageTemplateElement header = e.HeaderFooterTemplate;

            PdfSolidBrush brush = new PdfSolidBrush(new PdfColor(System.Drawing.Color.FromArgb(44, 71, 120)));
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);

            //Set formatting's for the text.
            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Center;
            format.LineAlignment = PdfVerticalAlignment.Middle;

            //Draw title.
            header.Graphics.DrawString("Syncfusion Essential PDF", font, brush, new System.Drawing.RectangleF(0, 0, header.Width, header.Height), format);
        }

        void GridPdfExportExtension_DrawPdfFooter(object sender, PdfHeaderFooterEventArgs e)
        {
            PdfPageTemplateElement footer = e.HeaderFooterTemplate;
            PdfSolidBrush brush = new PdfSolidBrush(new PdfColor(System.Drawing.Color.Gray));
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 6, PdfFontStyle.Bold);
            PdfStringFormat format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Center;
            format.LineAlignment = PdfVerticalAlignment.Middle;
            footer.Graphics.DrawString("@Copyright 2020", font, brush, new System.Drawing.RectangleF(0, footer.Height - 40, footer.Width, footer.Height), format);

            format = new PdfStringFormat();
            format.Alignment = PdfTextAlignment.Right;
            format.LineAlignment = PdfVerticalAlignment.Bottom;

            //Create page number field.
            PdfPageNumberField pageNumber = new PdfPageNumberField(font, brush);

            //Create page count field.
            PdfPageCountField count = new PdfPageCountField(font, brush);

            PdfCompositeField compositeField = new PdfCompositeField(font, brush, "Page {0} of {1}", pageNumber, count);
            compositeField.Bounds = footer.Bounds;
            compositeField.Draw(footer.Graphics, new System.Drawing.PointF(470, footer.Height - 10));
            //header.Graphics.DrawImage(PdfImage.FromFile(@"pack:/application:,,,/syncfusion.gridcontroldemos.wpf;component/Assets/GridControl/Footer.png"), 0, 0, header.Width, header.Height);
        }


        #region DataTable

        public void LoadData()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("Country"));
            dataTable.Columns.Add(new DataColumn("2005"));
            dataTable.Columns.Add(new DataColumn("2006"));
            dataTable.Columns.Add(new DataColumn("2007"));
            dataTable.Columns.Add(new DataColumn("2008"));
            dataTable.Columns.Add(new DataColumn("2009"));
            dataTable.Columns.Add(new DataColumn("2010"));
            dataTable.Columns.Add(new DataColumn("2011"));
            random = new Random();

            var row = dataTable.NewRow();
            row["Country"] = "USA";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "India";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "China";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Japan";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Russia";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Canada";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Germany";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Iran";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Thailand";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Bangladesh";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Nigeria";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Mexico";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "Egypt";
            LoadCellValues(row);
            dataTable.Rows.Add(row);

            row = dataTable.NewRow();
            row["Country"] = "France";
            LoadCellValues(row);
            dataTable.Rows.Add(row);
        }

        public void LoadCellValues(DataRow dataRow)
        {
            for (int row = 1; row <= 7; row++)
            {
                dataRow[row] = ((double)random.Next(2, 18)).ToString() + "%";
            }
        }

        #endregion
    }
}
