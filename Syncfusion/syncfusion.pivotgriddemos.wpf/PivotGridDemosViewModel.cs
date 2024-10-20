﻿using syncfusion.demoscommon.wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace syncfusion.pivotgriddemos.wpf
{
    public class PivotGridDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
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
        }
    }
}
