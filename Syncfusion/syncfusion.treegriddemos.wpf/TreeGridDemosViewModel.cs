using syncfusion.demoscommon.wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace syncfusion.treegriddemos.wpf
{
    public class TreeGridDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new TreeGridProductDemos());
            return productdemos;
        }
    }
    public class TreeGridProductDemos : ProductDemo
    {
        public TreeGridProductDemos()
        {
            this.Product = "TreeGrid";
            this.ProductCategory = "GRIDS";
            this.ListViewImagePathData = new System.Windows.Shapes.Path()
            {
                Data = Geometry.Parse("M2 1H12C12.5523 1 13 1.44772 13 2V4H1V2C1 1.44772 1.44772 1 2 1ZM6 5H1V12C1 12.5523 1.44772 13 2 13H12C12.5523 13 13 12.5523 13 12V9L6.5 9C6.22392 8.99993 6 8.7761 6 8.5V5ZM7 8V5H13V8L7 8ZM14 12V2C14 0.895431 13.1046 0 12 0H2C0.895431 0 0 0.89543 0 2V12C0 13.1046 0.89543 14 2 14H12C13.1046 14 14 13.1046 14 12ZM4.68 10.26L3.48 9.36C3.28223 9.21167 3 9.35279 3 9.6V11.4C3 11.6472 3.28223 11.7883 3.48 11.64L4.68 10.74C4.84 10.62 4.84 10.38 4.68 10.26ZM5.14 1.98L4.24 3.18C4.12 3.34 3.88 3.34 3.76 3.18L2.86 1.98C2.71167 1.78223 2.85279 1.5 3.1 1.5L4.9 1.5C5.14721 1.5 5.28833 1.78223 5.14 1.98Z"),
                Width = 14,
                Height = 14,
            };
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Grids.png", UriKind.RelativeOrAbsolute));
            this.Demos = new List<DemoInfo>();
            this.ControlDescription = "The TreeGrid control displays the hierarchical or self-relational data in a tree structure with multicolumn interface like multicolumn treeview.";
            this.GalleryViewImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/GalleryViewImages/TreeGrid.png", UriKind.RelativeOrAbsolute));
            this.Demos.Add(new DemoInfo()
            {
                SampleName = "File Explorer",
                Description = "This sample showcases the folder directory model with SfTreeGrid. The information is loaded on demand as the user opens each parent folder.",
                ImagePath = @"/syncfusion.treegriddemos.wpf;component/Assets/treegrid/FileExplorer_Icon.png",
                GroupName = "PRODUCT SHOWCASE",
                DemoViewType = typeof(FileExplorer),
                DemoLauchMode = DemoLauchMode.Window,
                ThemeMode = ThemeMode.Default
            });

            List<Documentation> gettingStartedDocumentation = new List<Documentation>();
            gettingStartedDocumentation.Add(new Documentation { Content = "TreeGrid - API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.TreeGrid.SfTreeGrid.html") });
            gettingStartedDocumentation.Add(new Documentation { Content = "TreeGrid - GettingStarted Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/getting-started") });
            gettingStartedDocumentation.Add(new Documentation { Content = "TreeGrid - Self-Relational Collection Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/getting-started#binding-self-relational-data-in-sftreegrid") });
            this.Demos.Add(new DemoInfo() { SampleName = "Self-Relational Collection", Description = "This sample showcases how to bind the Self-Relational data by specifying ChildPropertyName and ParentPropertyName in SfTreeGrid.", GroupName = "GETTING STARTED", DemoViewType = typeof(SelfRelationalDataBinding), Documentations = gettingStartedDocumentation });

            List<Documentation> nestedCollectionDocumentation = new List<Documentation>();
            nestedCollectionDocumentation.Add(new Documentation { Content = "TreeGrid - Nested Collection Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/getting-started#binding-nested-collection-with-sftreegrid") });
            this.Demos.Add(new DemoInfo() { SampleName = "Nested Collection", Description = "This sample showcases how to bind the Nested Collection data by specifying ChildPropertyName in SfTreeGrid.", GroupName = "GETTING STARTED", DemoViewType = typeof(NestedCollectionDemo), Documentations = nestedCollectionDocumentation });

            List<Documentation> onDemandLoadingDocumentation = new List<Documentation>();
            onDemandLoadingDocumentation.Add(new Documentation { Content = "TreeGrid - LoadOnDemandCommand API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.TreeGrid.SfTreeGrid.html#Syncfusion_UI_Xaml_TreeGrid_SfTreeGrid_LoadOnDemandCommand") });
            onDemandLoadingDocumentation.Add(new Documentation { Content = "TreeGrid - On-Demand Loading Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/load-on-demand") });
            this.Demos.Add(new DemoInfo() { SampleName = "On-Demand Loading", Description = "This sample exposes the OnDemand data loading of SfTreeGrid.", GroupName = "GETTING STARTED", DemoViewType = typeof(OnDemandLoadingDemo), Documentations = onDemandLoadingDocumentation });

            List<Documentation> sortingDocumentation = new List<Documentation>();
            sortingDocumentation.Add(new Documentation { Content = "TreeGrid - AllowSorting API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfGridBase.html#Syncfusion_UI_Xaml_Grid_SfGridBase_AllowSorting") });
            sortingDocumentation.Add(new Documentation { Content = "TreeGrid - Sorting Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/sorting") });
            this.Demos.Add(new DemoInfo() { SampleName = "Sorting", Description = "This sample showcases the sorting capabilities of data in SfTreeGrid. The SfTreeGrid control allows you to sort the data against one or more columns and provides some sorting functionalities like Tristate Sorting, Showing Sort Orders or Sort Numbers.", GroupName = "SORTING", DemoViewType = typeof(SortingDemo), Documentations = sortingDocumentation });

            List<Documentation> customSortingDocumentation = new List<Documentation>();
            customSortingDocumentation.Add(new Documentation { Content = "TreeGrid - Custom Sorting Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/sorting#custom-sorting") });
            this.Demos.Add(new DemoInfo() { SampleName = "Custom Sorting", Description = "This sample showcases how to sort a column based on length of the string using SortComparer in SfTreeGrid.", GroupName = "SORTING", DemoViewType = typeof(CustomSortingDemo), Documentations = customSortingDocumentation });

            List<Documentation> filteringDocumentation = new List<Documentation>();
            filteringDocumentation.Add(new Documentation { Content = "TreeGrid - FilterLevel API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.TreeGrid.SfTreeGrid.html#Syncfusion_UI_Xaml_TreeGrid_SfTreeGrid_FilterLevel") });
            filteringDocumentation.Add(new Documentation { Content = "TreeGrid - Filtering Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/filtering") });
            this.Demos.Add(new DemoInfo() { SampleName = "Filtering", Description = "This sample showcases the filtering capabilities of SfTreeGrid.", GroupName = "FILTERING", DemoViewType = typeof(FilteringDemo), Documentations = filteringDocumentation });

            List<Documentation> advancedFilteringDocumentation = new List<Documentation>();
            advancedFilteringDocumentation.Add(new Documentation { Content = "TreeGrid - AllowFiltering", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.TreeGrid.SfTreeGrid.html#Syncfusion_UI_Xaml_TreeGrid_SfTreeGrid_AllowFiltering") });
            advancedFilteringDocumentation.Add(new Documentation { Content = "TreeGrid - Advanced Filtering Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/filtering#advanced-filtering") });
            this.Demos.Add(new DemoInfo() { SampleName = "Advanced Filtering", Description = "This sample showcases Excel-inspired filtering capabilities in SfTreeGrid.", GroupName = "FILTERING", DemoViewType = typeof(ExcelLikeFilteringDemo), ThemeMode = ThemeMode.None, Documentations = advancedFilteringDocumentation });

            List<Documentation> selectionDocumentation = new List<Documentation>();
            selectionDocumentation.Add(new Documentation { Content = "TreeGrid - SelectionMode API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfGridBase.html#Syncfusion_UI_Xaml_Grid_SfGridBase_SelectionMode") });
            selectionDocumentation.Add(new Documentation { Content = "TreeGrid - NavigationMode API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfGridBase.html#Syncfusion_UI_Xaml_Grid_SfGridBase_NavigationMode") });
            selectionDocumentation.Add(new Documentation { Content = "TreeGrid - Selection Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/treegrid/selection") });
            this.Demos.Add(new DemoInfo() { SampleName = "Selection", Description = "This sample showcases the row selection capability of SfTreeGrid. SfTreeGrid control provides an interactive support for selecting rows in different mode with smooth and ease manner. It supports to select a specific row or group of rows programmatically or by Mouse and Keyboard interactions by SelectionMode property. This property provides options like Single, Multiple, Extended and None.", GroupName = "EDITING AND SELECTION", DemoViewType = typeof(SelectionDemo), Documentations = selectionDocumentation });

        }
    }
}
