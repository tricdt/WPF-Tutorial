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

            List<Documentation> searchDocumentation = new List<Documentation>();
            searchDocumentation.Add(new Documentation { Content = "DataGrid - Search API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SearchHelper.html#Syncfusion_UI_Xaml_Grid_SearchHelper_Search_System_String_") });
            searchDocumentation.Add(new Documentation { Content = "DataGrid - Search Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/search") });
            this.Demos.Add(new DemoInfo() { SampleName = "Search", GroupName = "FILTERING", Description = "This sample showcases the search support of SfDataGrid. This allows you to search the DataGrid with options to filter and highlight the search text in cells.", DemoViewType = typeof(SearchPanelDemo), ThemeMode = ThemeMode.None, Documentations = searchDocumentation });

            List<Documentation> masterDetailsViewDocumentation = new List<Documentation>();
            masterDetailsViewDocumentation.Add(new Documentation { Content = "DetailsViewDataGrid - API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.DetailsViewDataGrid.html") });
            masterDetailsViewDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/master-details-view") });
            this.Demos.Add(new DemoInfo() { SampleName = "Master Details View", GroupName = "MASTER DETAIL", Description = "This sample showcases the MasterDetailsView capability of SfDataGrid. The SfDataGrid displays hierarchical data in the form of nested tables using master-detail view configuration. In a hierarchical view, all tables in the data source are interconnected by means of relations.", DemoViewType = typeof(MasterDetailsViewDemo), Documentations = masterDetailsViewDocumentation });

            List<Documentation> detailsViewTemplateDocumentation = new List<Documentation>();
            detailsViewTemplateDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - TemplateViewDefinition API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.TemplateViewDefinition.html") });
            detailsViewTemplateDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - DetailsView Template Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/record-template-view") });
            this.Demos.Add(new DemoInfo() { SampleName = "Details View Template", GroupName = "MASTER DETAIL", Description = "This sample showcases the SfDataGrid with a DetailsView Template. It displays major values in each row and detailed information in the details section when the row is expanded. It can be used to load specific details for each row using RowTemplate property of TemplateViewDefinition. ", DemoViewType = typeof(DetailsViewTemplateDemo), ThemeMode = ThemeMode.None, Documentations = detailsViewTemplateDocumentation });

            List<Documentation> dataTableBindingDocumentation = new List<Documentation>();
            dataTableBindingDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - DataTable Binding Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-binding#binding-with-datatable") });
            this.Demos.Add(new DemoInfo() { SampleName = "DataTable Binding", GroupName = "MASTER DETAIL", Description = "This sample showcases the datatable binding capability of DetailsViewDataGrid. The DetailsViewDataGrid supports data sources such as DataTable and custom collection such as List, BindingList, ObservableCollection.", DemoViewType = typeof(DetailsViewDataTableBindingDemo), Documentations = dataTableBindingDocumentation });

            List<Documentation> stackedHeadersDocumentation = new List<Documentation>();
            stackedHeadersDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - StackedHeaders Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/stacked-headers") });
            this.Demos.Add(new DemoInfo() { SampleName = "Stacked Headers", GroupName = "MASTER DETAIL", Description = "This sample showcases the stacked headers capability in DetailsViewDataGrid of SfDataGrid.", DemoViewType = typeof(DetailsViewStackedHeaderRowsDemo), Documentations = stackedHeadersDocumentation });

            List<Documentation> columnTypesDocumentation = new List<Documentation>();
            columnTypesDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - ColumnTypes Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/column-types") });
            this.Demos.Add(new DemoInfo() { SampleName = "Column Types", GroupName = "MASTER DETAIL", Description = "This sample showcases the different column types capabilities in DetailsViewDataGrid. The SfDataGrid allows you to create various types of columns like Editable, Non-Editable and DropDown columns in DetailsViewDataGrid also.", DemoViewType = typeof(DetailsViewColumnTypesDemo), Documentations = columnTypesDocumentation });

            List<Documentation> conditionalFormattingDocumentation = new List<Documentation>();
            conditionalFormattingDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - ConditionalFormatting Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/conditional-styling") });
            this.Demos.Add(new DemoInfo() { SampleName = "Conditional Formatting", GroupName = "MASTER DETAIL", Description = "This sample showcases the conditional formatting capabilities of DetailsViewDataGrid. SfDataGrid control allows you to format the styles of cells and rows based on certain conditions by using converter and StyleSelector in DetailsViewDataGrid.", DemoViewType = typeof(ConditionalFormattingDetailsViewDataGridDemo), ThemeMode = ThemeMode.None, Documentations = conditionalFormattingDocumentation });

            List<Documentation> excelEportingDocumentation = new List<Documentation>();
            excelEportingDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - ExportToExcel API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.Converter.GridExcelExportExtension.html#Syncfusion_UI_Xaml_Grid_Converter_GridExcelExportExtension_ExportToExcel_Syncfusion_UI_Xaml_Grid_SfDataGrid_Syncfusion_Data_ICollectionViewAdv_Syncfusion_UI_Xaml_Grid_Converter_ExcelExportingOptions_") });
            excelEportingDocumentation.Add(new Documentation { Content = "DetailsView DataGrid - Excel Exporting Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/export-to-excel") });
            this.Demos.Add(new DemoInfo() { SampleName = "Excel Exporting", GroupName = "MASTER DETAIL", Description = "This sample showcases the excel exporting capability of MasterDetailsView in SfDataGrid.", DemoViewType = typeof(MasterDetailsExportingDemo), Documentations = excelEportingDocumentation });

            List<Documentation> dataVirtualizationDocumentation = new List<Documentation>();
            dataVirtualizationDocumentation.Add(new Documentation { Content = "DataGrid - EnableDataVirtualization API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html#Syncfusion_UI_Xaml_Grid_SfDataGrid_EnableDataVirtualization") });
            dataVirtualizationDocumentation.Add(new Documentation { Content = "DataGrid - DataVirtualization Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-virtualization") });
            this.Demos.Add(new DemoInfo() { SampleName = "Data Virtualization", GroupName = "DATA VIRTUALIZATION", Description = "This sample showcases the data virtualization capability of SfDataGrid. Data Virtualization support enables you to work with the huge data sources. SfDataGrid control creates records on-demand by automatically enabling data virtualization when EnableDataVirtualization property set to true.", DemoViewType = typeof(DataVirtualizationDemo), Documentations = dataVirtualizationDocumentation });

            List<Documentation> dataPagingDocumentation = new List<Documentation>();
            dataPagingDocumentation.Add(new Documentation { Content = "DataGrid - SfDataPager API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Controls.DataPager.SfDataPager.html") });
            dataPagingDocumentation.Add(new Documentation { Content = "DataGrid - DataPaging Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/paging") });
            this.Demos.Add(new DemoInfo() { SampleName = "Data Paging", GroupName = "DATA VIRTUALIZATION", Description = "This sample showcases the paging capability of SfDataGrid. Paging support is achieved by using SfDataPager control which return pages of data with entries where selection of the pages can be done using the numbered buttons. Paging loads the entire data collection to the SfDataPager and bind the PagedSource property of the SfDataPager to the ItemsSource property of SfDataGrid.", DemoViewType = typeof(DataPagingDemo), Documentations = dataPagingDocumentation });

            List<Documentation> onDemandPagingDocumentation = new List<Documentation>();
            onDemandPagingDocumentation.Add(new Documentation { Content = "DataGrid - On-DemandPaging Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/paging#load-data-in-on-demand") });
            this.Demos.Add(new DemoInfo() { SampleName = "On-Demand Paging", GroupName = "DATA VIRTUALIZATION", Description = "This sample showcases the on-demand paging capabilities of SfDataGrid. On-demand paging supports current page item source adds by on demand basis. The entire data is not needed to be fetched from the data source and we can get high performance even if there are millions of records. Also we can load millions of records in an efficient way.", DemoViewType = typeof(OnDemandPagingDemo), Documentations = onDemandPagingDocumentation });

            List<Documentation> editingDocumentation = new List<Documentation>();
            editingDocumentation.Add(new Documentation { Content = "DataGrid - AllowEditing API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfGridBase.html#Syncfusion_UI_Xaml_Grid_SfGridBase_AllowEditing") });
            editingDocumentation.Add(new Documentation { Content = "DataGrid - EditTrigger API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfGridBase.html#Syncfusion_UI_Xaml_Grid_SfGridBase_EditTrigger") });
            editingDocumentation.Add(new Documentation { Content = "DataGrid - Editing Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/editing") });
            this.Demos.Add(new DemoInfo() { SampleName = "Editing", GroupName = "EDITING", Description = "This sample showcases the editing capability in SfDataGrid. SfDataGrid provided options to trigger the edit mode by either with single or double click.", DemoViewType = typeof(EditingAndDataValidationDemo), Documentations = editingDocumentation });
            this.Demos.Add(new DemoInfo() { SampleName = "Editable Columns", GroupName = "EDITING", Description = "This sample showcases the editable columns capability of SfDataGrid. SfDataGrid provides different editable columns such as GridTextColumn, GridNumericColumn, GridCurrencyColumn, GridPercentColumn, GridMaskColumn, GridTimeSpanColumn, GridTemplateColumn and GridUnboundColumn.", DemoViewType = typeof(GridColumnsDemo) });
            this.Demos.Add(new DemoInfo() { SampleName = "Dropdown and Read Only Columns", GroupName = "EDITING", Description = "This sample showcases the dropdown and readonly columns capability of SfDataGrid. The SfDataGrid supports various types of dropdown and readonly columns like DateTimeColumn, ComboboxColumn, MultiColumnDropDownList, TemplateColumn, ImageColumn and HyperLinkColumn.", DemoViewType = typeof(GridDropDownAndReadOnlyColumnsDemo) });

            List<Documentation> comboBoxColumnDocumentation = new List<Documentation>();
            comboBoxColumnDocumentation.Add(new Documentation { Content = "DataGrid - GridComboBoxColumn API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.GridComboBoxColumn.html") });
            comboBoxColumnDocumentation.Add(new Documentation { Content = "DataGrid - ComboBox Column Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/column-types#gridcomboboxcolumn") });
            this.Demos.Add(new DemoInfo() { SampleName = "ComboBox Column", GroupName = "EDITING", Description = "This sample showcases the loading of different ItemsSource for different rows in GridComboBoxColumn based on data object.", DemoViewType = typeof(ComboBoxColumnsDemo), Documentations = comboBoxColumnDocumentation });

            List<Documentation> addNewRowDocumentation = new List<Documentation>();
            addNewRowDocumentation.Add(new Documentation { Content = "DataGrid - AddNewRowPosition API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.SfDataGrid.html#Syncfusion_UI_Xaml_Grid_SfDataGrid_AddNewRowPosition") });
            addNewRowDocumentation.Add(new Documentation { Content = "DataGrid - AddNewRow Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/data-manipulation#add-new-rows") });
            this.Demos.Add(new DemoInfo() { SampleName = "Add New Row", GroupName = "ROWS", Description = "This sample showcases adding the new record at runtime in SfDataGrid by AddNewRow feature. The AddNewRow is displayed, above or below the rows in the SfDataGrid based on AddNewRowPosition property.", DemoViewType = typeof(AddNewRowDemo), Documentations = addNewRowDocumentation });

            List<Documentation> stackedheadersDocumentation = new List<Documentation>();
            stackedheadersDocumentation.Add(new Documentation { Content = "DataGrid - StackedHeaderRow API Reference", Uri = new Uri("https://help.syncfusion.com/cr/wpf/Syncfusion.UI.Xaml.Grid.StackedHeaderRow.html") });
            stackedheadersDocumentation.Add(new Documentation { Content = "DataGrid - Stacked Headers Documentation", Uri = new Uri("https://help.syncfusion.com/wpf/datagrid/stacked-headers") });
            this.Demos.Add(new DemoInfo() { SampleName = "Stacked headers", GroupName = "ROWS", Description = "This sample showcases the stacked headers capability in DetailsViewDataGrid of SfDataGrid.", DemoViewType = typeof(StackedHeaderRowsDemo), Documentations = stackedheadersDocumentation });

        }
    }
}
