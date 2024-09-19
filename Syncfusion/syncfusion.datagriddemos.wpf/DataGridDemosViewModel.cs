using syncfusion.demoscommon.wpf;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace syncfusion.datagriddemos.wpf
{
    public class DataGridDemosViewModel : DemoBrowserViewModel
    {
        public override List<ProductDemo> GetDemosDetails()
        {
            var productdemos = new List<ProductDemo>();
            productdemos.Add(new DataGridProductDemos());
            return productdemos;
        }
    }
    public class DataGridProductDemos : ProductDemo
    {
        public DataGridProductDemos()
        {

            this.Product = "DataGrid";
            this.ProductCategory = "GRIDS";
            this.ListViewImagePathData = new System.Windows.Shapes.Path()
            {
                Data = Geometry.Parse("M12 1H2C1.44772 1 1 1.44772 1 2V4H13V2C13 1.44772 12.5523 1 12 1ZM9 5H5V8H9V5ZM10 8V5H13V8H10ZM9 9H5V13H9V9ZM10 13V9H13V12C13 12.5523 12.5523 13 12 13H10ZM2 14C0.89543 14 0 13.1046 0 12V2C0 0.89543 0.895431 0 2 0H12C13.1046 0 14 0.895431 14 2V12C14 13.1046 13.1046 14 12 14H2ZM1 5H4V8H1V5ZM1 9H4V13H2C1.44772 13 1 12.5523 1 12V9Z"),
                Width = 14,
                Height = 14,
            };
            this.Demos = new List<DemoInfo>();
            this.IsHighlighted = true;
            this.ControlDescription = "The DataGrid is a high-performance grid control that displays tabular and hierarchical data. It supports sorting, grouping, filtering, etc.";
            this.HeaderImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/ProductCategoryImages/Grids.png", UriKind.RelativeOrAbsolute));
            this.GalleryViewImageSource = new BitmapImage(new Uri(@"/syncfusion.demoscommon.wpf;component/Assets/GalleryViewImages/DataGrid.png", UriKind.RelativeOrAbsolute));
            List<Documentation> gettingStartedDocumentation = new List<Documentation>();
            gettingStartedDocumentation.Add(new Documentation { Content = "DataGrid - API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html") });
            gettingStartedDocumentation.Add(new Documentation { Content = "DataGrid - GettingStarted Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/getting-started") });
            this.Demos.Add(new DemoInfo() { SampleName = "Getting Started", GroupName = "GETTING STARTED", Description = "This sample showcases the basic features in SfDataGrid by simple ObservableCollection binding.", DemoViewType = typeof(GettingStarted), Documentations = gettingStartedDocumentation });

            List<Documentation> dataBindingDocumentation = new List<Documentation>();
            dataBindingDocumentation.Add(new Documentation { Content = "DataGrid - DataBinding Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-binding") });
            dataBindingDocumentation.Add(new Documentation { Content = "DataGrid - Binding With DataTable Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-binding#binding-with-datatable") });
            dataBindingDocumentation.Add(new Documentation { Content = "DataGrid - Binding with Dynamic Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-binding#binding-with-dynamic-data-object") });
            this.Demos.Add(new DemoInfo() { SampleName = "Data Binding", GroupName = "DATA BINDING", Description = "This sample showcases the data binding capabilities in SfDataGrid with data sources such as DataTable and Custom Collection such as List, BindingList and ObservableCollection.", DemoViewType = typeof(DataBindingDemo), ThemeMode = ThemeMode.None, Documentations = dataBindingDocumentation });

            List<Documentation> complexAndIndexerPropertiesDocumentation = new List<Documentation>();
            complexAndIndexerPropertiesDocumentation.Add(new Documentation { Content = "DataGrid - Complex Properties Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-binding#binding-complex-properties") });
            complexAndIndexerPropertiesDocumentation.Add(new Documentation { Content = "DataGrid - Indexer Properties Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-binding#binding-indexer-properties") });
            this.Demos.Add(new DemoInfo() { SampleName = "Complex and Indexer Properties", GroupName = "DATA BINDING", Description = "This sample showcases the complex and indexer properties binding capabilities in SfDataGrid. SfDataGrid supports to bind complex and indexer properties to its columns at any level.", DemoViewType = typeof(ComplexPropertyBindingDemo), Documentations = complexAndIndexerPropertiesDocumentation });

            List<Documentation> sortingDocumentation = new List<Documentation>();
            sortingDocumentation.Add(new Documentation { Content = "DataGrid - AllowSorting API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfGridBase.html#Syncfusion_UI_Xaml_Grid_SfGridBase_AllowSorting") });
            sortingDocumentation.Add(new Documentation { Content = "DataGrid - Sorting Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/sorting") });
            this.Demos.Add(new DemoInfo() { SampleName = "Sorting", GroupName = "DATA PRESENTATION", Description = "This sample showcases the sorting capabilities of data in SfDataGrid. The SfDataGrid control allows you to sort the data against one or more columns and provides some sorting functionalities like Tristate Sorting, Showing Sort Orders or Sort Numbers.", DemoViewType = typeof(SortingDemo), Documentations = sortingDocumentation });

            List<Documentation> groupingDocumentation = new List<Documentation>();
            groupingDocumentation.Add(new Documentation { Content = "DataGrid - AllowGrouping API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html#Syncfusion_UI_Xaml_Grid_SfDataGrid_AllowGrouping") });
            groupingDocumentation.Add(new Documentation { Content = "DataGrid - AllowFrozenGroupHeaders API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html#Syncfusion_UI_Xaml_Grid_SfDataGrid_AllowFrozenGroupHeaders") });
            groupingDocumentation.Add(new Documentation { Content = "DataGrid - Grouping Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/grouping") });
            this.Demos.Add(new DemoInfo() { SampleName = "Grouping", GroupName = "DATA PRESENTATION", Description = "This sample showcases the grouping capabilities of data in SfDataGrid. The SfDataGrid enables you to grouping the data by one or more columns.", DemoViewType = typeof(GroupingDemo), Documentations = groupingDocumentation });

            List<Documentation> summaryDocumentation = new List<Documentation>();
            summaryDocumentation.Add(new Documentation { Content = "Summaries Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/summaries") });
            this.Demos.Add(new DemoInfo() { SampleName = "Summaries", GroupName = "DATA PRESENTATION", Description = "This sample showcases the summary calculation capabilities of data in SfDataGrid. SfDataGrid provides three kinds of summary row collections, namely GroupSummaryRows, TableSummaryRows, and CaptionSummaryRows.", DemoViewType = typeof(SummariesDemo), Documentations = summaryDocumentation });

            List<Documentation> intervalGroupingDocumentation = new List<Documentation>();
            intervalGroupingDocumentation.Add(new Documentation { Content = "DataGrid - GroupColumnDescription_Comparer API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GroupColumnDescription.html#Syncfusion_UI_Xaml_Grid_GroupColumnDescription_Comparer") });
            intervalGroupingDocumentation.Add(new Documentation { Content = "DataGrid - GroupColumnDescription_Comparer API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GroupColumnDescription.html#Syncfusion_UI_Xaml_Grid_GroupColumnDescription_Converter") });
            this.Demos.Add(new DemoInfo() { SampleName = "Interval Grouping", GroupName = "DATA PRESENTATION", Description = "This sample showcases the interval grouping capabilities of data in SfDataGrid. The SfDataGrid Supports the interval grouping by enable the interval grouping logic.", DemoViewType = typeof(IntervalGroupingDemo), Documentations = intervalGroupingDocumentation });

            List<Documentation> customGroupingDocumentation = new List<Documentation>();
            customGroupingDocumentation.Add(new Documentation { Content = "DataGrid - Custom Grouping Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/grouping#custom-grouping") });
            this.Demos.Add(new DemoInfo() { SampleName = "Custom Grouping", GroupName = "DATA PRESENTATION", Description = "This sample showcases the custom grouping capabilities in SfDataGrid. The SfDataGrid supports custom grouping which enables you to implement custom grouping logic, if the standard grouping techniques does not meet your requirements.", DemoViewType = typeof(CustomGroupingDemo), Documentations = customGroupingDocumentation });

            List<Documentation> sortBySummaryDocumentation = new List<Documentation>();
            sortBySummaryDocumentation.Add(new Documentation { Content = "DataGrid - Sort by Summary Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/grouping#sorting-groups-based-on-summary-values") });
            this.Demos.Add(new DemoInfo() { SampleName = "Sort by Summary", GroupName = "DATA PRESENTATION", Description = "This sample showcases the sort by summary capabilities of data in SfDataGrid.", DemoViewType = typeof(SortBySummaryDemo), Documentations = sortBySummaryDocumentation });

            List<Documentation> filteringDocumentation = new List<Documentation>();
            filteringDocumentation.Add(new Documentation { Content = "DataGrid - ViewFilter API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.Data.CollectionViewAdv.html#Syncfusion_Data_CollectionViewAdv_Filter") });
            filteringDocumentation.Add(new Documentation { Content = "DataGrid - ViewFilter Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/filtering#view-filtering") });
            this.Demos.Add(new DemoInfo() { SampleName = "Filtering", GroupName = "FILTERING", Description = "This sample showcases the filtering capabilities of data in SfDataGrid.", DemoViewType = typeof(FilteringDemo), Documentations = filteringDocumentation });

            List<Documentation> advanceFilteringDocumentation = new List<Documentation>();
            advanceFilteringDocumentation.Add(new Documentation { Content = "DataGrid - AllowFiltering API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html#Syncfusion_UI_Xaml_Grid_SfDataGrid_AllowFiltering") });
            advanceFilteringDocumentation.Add(new Documentation { Content = "DataGrid - AdvanceFiltering Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/filtering#advanced-filter-ui") });
            this.Demos.Add(new DemoInfo() { SampleName = "Advanced Filtering", GroupName = "FILTERING", Description = "This sample showcases the Excel inspired filtering capabilities of data in SfDataGrid.", DemoViewType = typeof(ExcelLikeFilteringDemo), ThemeMode = ThemeMode.None, Documentations = advanceFilteringDocumentation });

            List<Documentation> filterRowDocumentation = new List<Documentation>();
            filterRowDocumentation.Add(new Documentation { Content = "DataGrid - FilterRowPosition API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.FilterRowPosition.html") });
            filterRowDocumentation.Add(new Documentation { Content = "DataGrid - FilterRow Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/filterrow") });
            this.Demos.Add(new DemoInfo() { SampleName = "Filter Row", GroupName = "FILTERING", Description = "This sample showcases the filter row functionalities of SfDataGrid.", DemoViewType = typeof(FilterRowDemo), Documentations = filterRowDocumentation });


            this.Demos.Add(new DemoInfo() { SampleName = "Filter Status Bar", GroupName = "FILTERING", Description = "This sample showcases the Filter Status Bar at the bottom of SfDataGrid which displays the filter conditions in simple context.", DemoViewType = typeof(FilterStatusBarDemo), ThemeMode = ThemeMode.None });
            List<Documentation> customFilterRowDocumentation = new List<Documentation>();
            customFilterRowDocumentation.Add(new Documentation { Content = "DataGrid - Custom Filter Row Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/filterrow#customizing-filter-row-cell") });
            this.Demos.Add(new DemoInfo() { SampleName = "Custom Filter Row", GroupName = "FILTERING", Description = "This sample showcases the customization of filter row editors and drop-down options in SfDataGrid.", DemoViewType = typeof(CustomFilterRowDemo), Documentations = customFilterRowDocumentation });

        }
    }
}
