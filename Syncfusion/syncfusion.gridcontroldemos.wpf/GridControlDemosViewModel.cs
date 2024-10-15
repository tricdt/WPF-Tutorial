﻿using syncfusion.demoscommon.wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace syncfusion.gridcontroldemos.wpf
{
    public class GridControlDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productDemos = new List<ProductDemo>();
            productDemos.Add(new GridControlProductDemos());
            return productDemos;
        }
    }
    public class GridControlProductDemos : ProductDemo
    {
        public GridControlProductDemos()
        {
            this.Product = "Grid Control";
            this.ProductCategory = "GRIDS";
            this.ListViewImagePathData = new System.Windows.Shapes.Path()
            {
                Data = Geometry.Parse("M2.5 0C1.11929 0 0 1.11929 0 2.5V11.5C0 12.8807 1.11929 14 2.5 14H5.5C5.77614 14 6 13.7761 6 13.5C6 13.2239 5.77614 13 5.5 13H5V10H5.5C5.77614 10 6 9.77614 6 9.5C6 9.22386 5.77614 9 5.5 9H5V5H8V7C8 7.27614 8.22386 7.5 8.5 7.5C8.77614 7.5 9 7.27614 9 7V5H13V7C13 7.27614 13.2239 7.5 13.5 7.5C13.7761 7.5 14 7.27614 14 7V2.5C14 1.11929 12.8807 0 11.5 0H2.5ZM13 4V2.5C13 1.67157 12.3284 1 11.5 1H2.5C1.67157 1 1 1.67157 1 2.5V4H13ZM4 5L4 9H1V5H4ZM1 10V11.5C1 12.3284 1.67157 13 2.5 13H4L4 10H1ZM13.1909 12.5909C13.1425 12.7006 13.1281 12.8223 13.1495 12.9402C13.1708 13.0582 13.2271 13.167 13.3109 13.2527L13.3327 13.2745C13.4003 13.3421 13.454 13.4223 13.4906 13.5106C13.5272 13.5989 13.546 13.6935 13.546 13.7891C13.546 13.8847 13.5272 13.9793 13.4906 14.0676C13.454 14.1559 13.4003 14.2361 13.3327 14.3036C13.2652 14.3713 13.185 14.4249 13.0967 14.4615C13.0084 14.4981 12.9138 14.5169 12.8182 14.5169C12.7226 14.5169 12.628 14.4981 12.5397 14.4615C12.4514 14.4249 12.3712 14.3713 12.3036 14.3036L12.2818 14.2818C12.1961 14.198 12.0873 14.1418 11.9693 14.1204C11.8513 14.099 11.7297 14.1134 11.62 14.1618C11.5124 14.2079 11.4207 14.2845 11.3561 14.382C11.2915 14.4796 11.2568 14.5939 11.2564 14.7109V14.7727C11.2564 14.9656 11.1797 15.1506 11.0434 15.287C10.907 15.4234 10.722 15.5 10.5291 15.5C10.3362 15.5 10.1512 15.4234 10.0148 15.287C9.87844 15.1506 9.80182 14.9656 9.80182 14.7727V14.74C9.799 14.6196 9.76004 14.5029 9.69 14.405C9.61996 14.3071 9.52209 14.2325 9.40909 14.1909C9.29941 14.1425 9.17775 14.1281 9.05979 14.1495C8.94182 14.1708 8.83297 14.2271 8.74727 14.3109L8.72545 14.3327C8.65791 14.4003 8.5777 14.454 8.48941 14.4906C8.40112 14.5272 8.30648 14.546 8.21091 14.546C8.11533 14.546 8.0207 14.5272 7.93241 14.4906C7.84412 14.454 7.76391 14.4003 7.69636 14.3327C7.62874 14.2652 7.5751 14.185 7.5385 14.0967C7.5019 14.0084 7.48306 13.9138 7.48306 13.8182C7.48306 13.7226 7.5019 13.628 7.5385 13.5397C7.5751 13.4514 7.62874 13.3712 7.69636 13.3036L7.71818 13.2818C7.80201 13.1961 7.85825 13.0873 7.87964 12.9693C7.90103 12.8513 7.88659 12.7297 7.83818 12.62C7.79209 12.5124 7.71555 12.4207 7.61799 12.3561C7.52043 12.2915 7.4061 12.2568 7.28909 12.2564H7.22727C7.03439 12.2564 6.8494 12.1797 6.71301 12.0434C6.57662 11.907 6.5 11.722 6.5 11.5291C6.5 11.3362 6.57662 11.1512 6.71301 11.0148C6.8494 10.8784 7.03439 10.8018 7.22727 10.8018H7.26C7.38036 10.799 7.49709 10.76 7.59502 10.69C7.69294 10.62 7.76753 10.5221 7.80909 10.4091C7.8575 10.2994 7.87194 10.1777 7.85055 10.0598C7.82916 9.94182 7.77292 9.83297 7.68909 9.74727L7.66727 9.72545C7.59965 9.65791 7.54601 9.5777 7.50941 9.48941C7.47281 9.40112 7.45397 9.30648 7.45397 9.21091C7.45397 9.11533 7.47281 9.0207 7.50941 8.93241C7.54601 8.84412 7.59965 8.76391 7.66727 8.69636C7.73482 8.62874 7.81503 8.5751 7.90332 8.5385C7.99161 8.5019 8.08624 8.48306 8.18182 8.48306C8.27739 8.48306 8.37203 8.5019 8.46032 8.5385C8.54861 8.5751 8.62882 8.62874 8.69636 8.69636L8.71818 8.71818C8.80388 8.80201 8.91273 8.85825 9.03069 8.87964C9.14866 8.90103 9.27032 8.88659 9.38 8.83818H9.40909C9.51664 8.79209 9.60837 8.71555 9.67298 8.61799C9.73759 8.52043 9.77226 8.4061 9.77273 8.28909V8.22727C9.77273 8.03439 9.84935 7.8494 9.98574 7.71301C10.1221 7.57662 10.3071 7.5 10.5 7.5C10.6929 7.5 10.8779 7.57662 11.0143 7.71301C11.1507 7.8494 11.2273 8.03439 11.2273 8.22727V8.26C11.2277 8.37701 11.2624 8.49134 11.327 8.5889C11.3916 8.68646 11.4834 8.763 11.5909 8.80909C11.7006 8.8575 11.8223 8.87194 11.9402 8.85055C12.0582 8.82916 12.167 8.77292 12.2527 8.68909L12.2745 8.66727C12.3421 8.59965 12.4223 8.54601 12.5106 8.50941C12.5989 8.47281 12.6935 8.45397 12.7891 8.45397C12.8847 8.45397 12.9793 8.47281 13.0676 8.50941C13.1559 8.54601 13.2361 8.59965 13.3036 8.66727C13.3713 8.73482 13.4249 8.81503 13.4615 8.90332C13.4981 8.99161 13.5169 9.08624 13.5169 9.18182C13.5169 9.27739 13.4981 9.37203 13.4615 9.46032C13.4249 9.54861 13.3713 9.62882 13.3036 9.69636L13.2818 9.71818C13.198 9.80388 13.1418 9.91273 13.1204 10.0307C13.099 10.1487 13.1134 10.2703 13.1618 10.38V10.4091C13.2079 10.5166 13.2845 10.6084 13.382 10.673C13.4796 10.7376 13.5939 10.7723 13.7109 10.7727H13.7727C13.9656 10.7727 14.1506 10.8494 14.287 10.9857C14.4234 11.1221 14.5 11.3071 14.5 11.5C14.5 11.6929 14.4234 11.8779 14.287 12.0143C14.1506 12.1507 13.9656 12.2273 13.7727 12.2273H13.74C13.623 12.2277 13.5087 12.2624 13.4111 12.327C13.3135 12.3916 13.237 12.4834 13.1909 12.5909ZM11.7 11.5C11.7 12.1627 11.1627 12.7 10.5 12.7C9.83726 12.7 9.3 12.1627 9.3 11.5C9.3 10.8373 9.83726 10.3 10.5 10.3C11.1627 10.3 11.7 10.8373 11.7 11.5Z"),
                Width = 15,
                Height = 16,
            };
            this.Demos = new List<DemoInfo>();
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Grids.png", UriKind.RelativeOrAbsolute));
            this.ControlDescription = "The GridControl is a customizable cell-oriented control that displays tabular data. It has a rich feature set, including cell styling, formulas, importing, and more.";
            this.GalleryViewImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/GalleryViewImages/GridControl.png", UriKind.RelativeOrAbsolute));

            List<Documentation> excelLikeUIDocumentation = new List<Documentation>();
            excelLikeUIDocumentation.Add(new Documentation { Content = "GridControl - Excel Like UI Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/gridcontrol/real-time-applications#excel-like-ui-applications") });
            this.Demos.Add(new DemoInfo() { SampleName = "Excel Like UI", Description = "This sample showcases the following capabilities of GridControl such as Selection Frame, Floating cells, Formula cells, Markup headers and Tab sheets and it provides us interactive user experience with an Excel-like appearance and characteristics .", GroupName = "PRODUCT SHOWCASE", DemoViewType = typeof(ExcelLikeUI), DemoLauchMode = DemoLauchMode.Window, ThemeMode = ThemeMode.None, Documentations = excelLikeUIDocumentation });
            this.Demos.Add(new DemoInfo() { SampleName = "Scroll Performance", Description = "This sample showcases the scrolling performance of GridControl.", GroupName = "PERFORMANCE", DemoViewType = typeof(ScrollPerformance), ThemeMode = ThemeMode.Default });
            this.Demos.Add(new DemoInfo() { SampleName = "TraderGrid Test", Description = "This sample showcases the real time updates capability of GridControl.It provides support to insert and remove rows or columns with a minimal CPU usage. It also handles very high frequency updates and refresh scenarios.", GroupName = "PERFORMANCE", DemoViewType = typeof(TraderGridTest), ThemeMode = ThemeMode.Default });

            List<Documentation> excelLikeDragDropDocumentation = new List<Documentation>();
            excelLikeDragDropDocumentation.Add(new Documentation { Content = "GridControl - AllowDragDrop API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Windows.Controls.Grid.GridControlBase.html#Syncfusion_Windows_Controls_Grid_GridControlBase_AllowDragDrop") });
            excelLikeDragDropDocumentation.Add(new Documentation { Content = "GridControl - Excel Like Drag And Drop Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/gridcontrol/interactive-features#excel-like---cell-drag-and-drop") });
            this.Demos.Add(new DemoInfo() { SampleName = "Excel Like Drag and Drop", Description = "This sample showcases Excel like Drag and Drop in GridControl. This feature enables you to select any range and click on any corner of the selected region to drag it and drop it anywhere into a Grid, or some other controls, in an application. You can enable this feature by setting the AllowDragDrop property of the GridControl as True.", GroupName = "EXCEL LIKE FEATURES", DemoViewType = typeof(ExcelLikeDragandDrop), ThemeMode = ThemeMode.Default, Documentations = excelLikeDragDropDocumentation });

        }
    }
}
