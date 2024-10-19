using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using syncfusion.demoscommon.wpf;
namespace syncfusion.chartdemos.wpf
{
    public class ChartDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new ChartProductDemos());
            return productdemos;
        }
    }
    public class ChartProductDemos: ProductDemo
    {
        public ChartProductDemos()
        {
            this.Product = "Charts";
            this.ProductCategory = "CHARTS";
            this.ListViewImagePathData = new System.Windows.Shapes.Path()
            {
                Data = Geometry.Parse("M9.62127 0.515047C9.35337 0.448073 9.0819 0.610953 9.01493 0.87885C8.94795 1.14675 9.11084 1.41822 9.37873 1.48519L10.1502 1.67805L2.25718 6.06304C2.01579 6.19715 1.92881 6.50155 2.06292 6.74294C2.19703 6.98433 2.50143 7.0713 2.74282 6.9372L10.6192 2.56144L10.359 3.342C10.2717 3.60398 10.4132 3.88714 10.6752 3.97446C10.9372 4.06178 11.2204 3.9202 11.3077 3.65823L11.9738 1.66C12.0172 1.53127 12.0083 1.38541 11.9371 1.2573C11.8762 1.1477 11.7802 1.06994 11.6709 1.03024C11.6547 1.02435 11.6382 1.01927 11.6213 1.01505M9.62127 0.515047L11.6198 1.01469L9.62127 0.515047ZM0.5 2.00012C0.776142 2.00012 1 2.22398 1 2.50012V12.5001C1 12.7763 1.22386 13.0001 1.5 13.0001H13C13.2761 13.0001 13.5 13.224 13.5 13.5001C13.5 13.7763 13.2761 14.0001 13 14.0001H1.5C0.671573 14.0001 0 13.3285 0 12.5001V2.50012C0 2.22398 0.223858 2.00012 0.5 2.00012ZM3.5 9.00012C3.77614 9.00012 4 9.22398 4 9.50012V11.5001C4 11.7763 3.77614 12.0001 3.5 12.0001C3.22386 12.0001 3 11.7763 3 11.5001V9.50012C3 9.22398 3.22386 9.00012 3.5 9.00012ZM7 7.50012C7 7.22398 6.77614 7.00012 6.5 7.00012C6.22386 7.00012 6 7.22398 6 7.50012V11.5001C6 11.7763 6.22386 12.0001 6.5 12.0001C6.77614 12.0001 7 11.7763 7 11.5001V7.50012ZM9.5 5.00012C9.77614 5.00012 10 5.22398 10 5.50012V11.5001C10 11.7763 9.77614 12.0001 9.5 12.0001C9.22386 12.0001 9 11.7763 9 11.5001V5.50012C9 5.22398 9.22386 5.00012 9.5 5.00012ZM13 5.50012C13 5.22398 12.7761 5.00012 12.5 5.00012C12.2239 5.00012 12 5.22398 12 5.50012V11.5001C12 11.7763 12.2239 12.0001 12.5 12.0001C12.7761 12.0001 13 11.7763 13 11.5001V5.50012Z"),
                Width = 14,
                Height = 14,
            };
            this.IsHighlighted = true;
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Charts.png", UriKind.RelativeOrAbsolute));
            this.ControlDescription = "The Chart provides a perfect way to visualize data with a high level of user interactivity. It also provides a variety of charting features that can be used to visualize large quantities of data.";
            this.Demos = new List<DemoInfo>();
            this.GalleryViewImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/GalleryViewImages/Charts.png", UriKind.RelativeOrAbsolute));
            DemoInfo columnDemo = new DemoInfo()
            {
                SampleName = "Column",
                GroupName = "Basic Charts",
                WhatsNewDescription = "This sample showcases a column chart that uses vertical bars to display different values or data points.",
            };
            DemoInfo columnChartSample = new DemoInfo() { SampleName = "Default column", GroupName = "Basic Charts", Description = "This column chart showcases the trends in population growth percentages of different countries.", DemoViewType = typeof(DefaultColumn) };
            List<Documentation> columnChartHelpDocuments = new List<Documentation>()
            {
                new Documentation(){ Content = "Column Series API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Charts.ColumnSeries.html#") },
                new Documentation(){ Content = "Column Series Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/charts/seriestypes/columnandbar#column-chart") },
             };
            columnChartSample.Documentations = columnChartHelpDocuments;

            DemoInfo columnRoundedCornerSample = new DemoInfo() { SampleName = "Column with rounded corners", GroupName = "Basic Charts", Description = "This sample demonstrates the land area of various cities using rounded columns.", DemoViewType = typeof(ColumnRoundedEdges) };

            DemoInfo columnWidthSample = new DemoInfo() { SampleName = "Column spacing", GroupName = "Basic Charts", Description = "This sample illustrates the number of medals that were won by various countries in the 2022 Beijing Olympics, using a customized segment width for visualization.", DemoViewType = typeof(ColumnWidthCustomization) };

            DemoInfo customizedColumnSample = new DemoInfo() { SampleName = "Customized column", GroupName = "Basic Charts", Description = "This sample visualizes the comparison of literacy rates across different states by using customized columns.", DemoViewType = typeof(CustomizedColumn) };

            List<DemoInfo> subColumnDemos = new List<DemoInfo>();
            subColumnDemos.Add(columnChartSample);
            subColumnDemos.Add(columnRoundedCornerSample);
            subColumnDemos.Add(columnWidthSample);
            subColumnDemos.Add(customizedColumnSample);

            columnDemo.SubCategoryDemos = subColumnDemos;
            this.Demos.Add(columnDemo);
        }
    }
}
