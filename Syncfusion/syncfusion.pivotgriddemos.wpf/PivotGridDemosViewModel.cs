using syncfusion.demoscommon.wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace syncfusion.pivotgriddemos.wpf
{
    public class PivotGridDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new PivotGridProductDemos());
            return productdemos;
        }
    }
    public class PivotGridProductDemos : ProductDemo
    {
        public PivotGridProductDemos()
        {
            this.Product = "Pivot Grid";
            this.ProductCategory = "GRIDS";
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Grids.png", UriKind.RelativeOrAbsolute));
#if NET50 || NETCORE
            this.ProductCategory = "GRIDS";
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Grids.png", UriKind.RelativeOrAbsolute));
#else
            //this.ProductCategory = "BUSINESS INTELLIGENCE";
#endif
            this.ListViewImagePathData = new System.Windows.Shapes.Path()
            {
                Data = Geometry.Parse("M2 1H12C12.5523 1 13 1.44772 13 2V12C13 12.5523 12.5523 13 12 13H2C1.44772 13 1 12.5523 1 12V2C1 1.44772 1.44772 1 2 1ZM0 2C0 0.89543 0.895431 0 2 0H12C13.1046 0 14 0.895431 14 2V12C14 13.1046 13.1046 14 12 14H2C0.89543 14 0 13.1046 0 12V2ZM4 5C4 4.72386 3.77614 4.5 3.5 4.5C3.22386 4.5 3 4.72386 3 5L3 11C3 11.2761 3.22386 11.5 3.5 11.5C3.77614 11.5 4 11.2761 4 11L4 5ZM4.5 3.5C4.5 3.22386 4.72386 3 5 3L11 3C11.2761 3 11.5 3.22386 11.5 3.5C11.5 3.77614 11.2761 4 11 4L5 4C4.72386 4 4.5 3.77614 4.5 3.5ZM9.1 7H9.5C9.5 8.80451 8.88509 9.67705 8.22991 10.0976C7.8456 10.3443 7.41026 10.4599 7 10.4911V10.1C7 9.85279 6.71777 9.71167 6.52 9.86L5.32 10.76C5.16 10.88 5.16 11.12 5.32 11.24L6.52 12.14C6.71777 12.2883 7 12.1472 7 11.9V11.4931C7.5735 11.4605 8.20053 11.3048 8.77009 10.9392C9.78157 10.2899 10.5 9.0529 10.5 7H10.9C11.1472 7 11.2883 6.71777 11.14 6.52L10.24 5.32C10.12 5.16 9.88 5.16 9.76 5.32L8.86 6.52C8.71167 6.71777 8.85279 7 9.1 7Z"),
                Width = 14,
                Height = 14,
            };
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/BusinessIntilegence.png", UriKind.RelativeOrAbsolute));
            this.ControlDescription = "The PivotGridControl organizes and summarizes business data in a cross-table format. Key features include custom calculations and export to Excel and more.";
            this.GalleryViewImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/GalleryViewImages/PivotGrid.png", UriKind.RelativeOrAbsolute));
            this.Demos = new List<DemoInfo>();

            List<Documentation> gettingStartedDocumentation = new List<Documentation>();
            gettingStartedDocumentation.Add(new Documentation { Content = "PivotGridControl - API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.PivotGrid.PivotGridControl.html") });
            gettingStartedDocumentation.Add(new Documentation { Content = "PivotGrid - Getting Started Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/pivot-grid/pivotgrid-getting-started") });
            this.Demos.Add(new DemoInfo() { SampleName = "Getting Started", GroupName = "PRODUCT SHOWCASE", DemoViewType = typeof(PivotGridDemo), Description = "This sample illustrate to show sales data across customer geography and product categories during each fiscal year.", Documentations = gettingStartedDocumentation });

            List<Documentation> customizationDocumentaion = new List<Documentation>();
            customizationDocumentaion.Add(new Documentation { Content = "PivotGrid - GridLayout API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.PivotGrid.PivotGridControl.html#Syncfusion_Windows_Controls_PivotGrid_PivotGridControl_GridLayout") });
            customizationDocumentaion.Add(new Documentation { Content = "PivotGrid - GridLineStroke API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.PivotGrid.PivotGridControl.html#Syncfusion_Windows_Controls_PivotGrid_PivotGridControl_GridLineStroke") });
            customizationDocumentaion.Add(new Documentation { Content = "PivotGrid - Grid Layout Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/pivot-grid/grid-layout") });
            this.Demos.Add(new DemoInfo() { SampleName = "Customization", GroupName = "PRODUCT SHOWCASE", DemoViewType = typeof(PivotGridCustomization), Description = "This sample illustrates customization of PivotGrid, such as changing GridLine color, freezing headers, showing/hiding sub-totals etc...", Documentations = customizationDocumentaion });

            List<Documentation> fieldCaptionDocumentation = new List<Documentation>();
            fieldCaptionDocumentation.Add(new Documentation { Content = "PivotGrid - PivotFields API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.PivotGrid.PivotGridControl.html#Syncfusion_Windows_Controls_PivotGrid_PivotGridControl_PivotFields") });
            fieldCaptionDocumentation.Add(new Documentation { Content = "PivotGrid - PivotCalculations API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.PivotGrid.PivotGridControl.html#Syncfusion_Windows_Controls_PivotGrid_PivotGridControl_PivotCalculations") });
            fieldCaptionDocumentation.Add(new Documentation { Content = "PivotGrid - Pivot Table Field List Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/pivot-grid/pivotgrid-field-list") });
            this.Demos.Add(new DemoInfo() { SampleName = "Field Caption", GroupName = "PRODUCT SHOWCASE", DemoViewType = typeof(FieldCaption), Description = "This sample illustrates setting caption and duplication of PivotField and PivotCalculation in PivotTable Field List.", Documentations = fieldCaptionDocumentation });

            this.Demos.Add(new DemoInfo() { SampleName = "UI Threading", GroupName = "PRODUCT SHOWCASE", DemoViewType = typeof(UIThreading), Description = "The pivotGrid supports to load data in a different UI thread. That is, PivotGrid control can perform long running operations in a background thread so that we can access other UI controls when PivotGrid is loading. It also loads uniquely for every layout change operation, such as filtering, sorting, drag and drop, FieldList and PivotSchemaDesigner." });

            List<Documentation> rowPivotsOnlyDocumentation = new List<Documentation>();
            rowPivotsOnlyDocumentation.Add(new Documentation { Content = "PivotGrid - RowPivotsOnly API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.PivotGrid.PivotGridControl.html#Syncfusion_Windows_Controls_PivotGrid_PivotGridControl_RowPivotsOnly") });
            rowPivotsOnlyDocumentation.Add(new Documentation { Content = "PivotGrid - Row Pivots Only Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/pivot-grid/defining-rowpivotsonly-mode-for-pivotgridcont") });
            this.Demos.Add(new DemoInfo() { SampleName = "Row Pivots Only", GroupName = "PRODUCT SHOWCASE", DemoViewType = typeof(RowPivotsOnly), Description = "This sample illustrates about multiple functionalities, implemented in PivotGrid by enabling RowPivotsOnly property.", Documentations = rowPivotsOnlyDocumentation });

        }
    }
}
