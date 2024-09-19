using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.Windows.Shared;
using System.Windows;
using System.Windows.Controls;
namespace syncfusion.datagriddemos.wpf
{
    [TemplatePart(Name = "PART_ToolTip", Type = typeof(ToolTip))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TextBlock", Type = typeof(TextBlock))]
    public class FilterStatusBar : Control
    {

        private Button ClearFilterButton;
        private ToolTip Tooltip;
        private TextBlock Textblock;
        private bool isdisposed = false;

        public static readonly DependencyProperty DataGridProperty =
            DependencyProperty.Register("DataGrid", typeof(SfDataGrid), typeof(FilterStatusBar), new PropertyMetadata(null));
        public SfDataGrid DataGrid
        {
            get { return (SfDataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }

        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText", typeof(string), typeof(FilterStatusBar), new PropertyMetadata(null));
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            private set { SetValue(FilterTextProperty, value); }
        }

        static FilterStatusBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterStatusBar), new FrameworkPropertyMetadata(typeof(FilterStatusBar)));
        }

        public FilterStatusBar()
        {
        }

        public FilterStatusBar(SfDataGrid datagrid)
        {
            DataGrid = datagrid;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.UpdateFilterStatusBarVisibility(false);
            ClearFilterButton = this.GetTemplateChild("PART_CloseButton") as Button;
            Textblock = this.GetTemplateChild("PART_TextBlock") as TextBlock;
            Tooltip = this.GetTemplateChild("PART_ToolTip") as ToolTip;
            this.UnWireEvents();
            this.WireEvents();
        }

        private void WireEvents()
        {
            this.Loaded += OnFilterStatusBarLoaded;
            ClearFilterButton.Click += OnClearFilterButtonClick;
            Textblock.ToolTipOpening += OnTextblockToolTipOpening;
        }

        private void OnTextblockToolTipOpening(object sender, ToolTipEventArgs e)
        {
            var textblock = sender as TextBlock;

            if (textblock.Text == null)
                Tooltip.Visibility = Visibility.Collapsed;

            FrameworkElement textBlock = (FrameworkElement)textblock;

            textBlock.Measure(new System.Windows.Size(Double.PositiveInfinity, Double.PositiveInfinity));

            if (((FrameworkElement)textblock).ActualWidth < ((FrameworkElement)textblock).DesiredSize.Width)
                Tooltip.Visibility = Visibility.Visible;
            else
                Tooltip.Visibility = Visibility.Collapsed;
        }

        private void OnClearFilterButtonClick(object sender, RoutedEventArgs e)
        {
            if (DataGrid.SelectionController.CurrentCellManager.HasCurrentCell)
                DataGrid.SelectionController.CurrentCellManager.EndEdit();
            DataGrid.ClearFilters();
        }

        private void UnWireEvents()
        {
            this.Loaded -= OnFilterStatusBarLoaded;
            ClearFilterButton.Click -= OnClearFilterButtonClick;
            Textblock.ToolTipOpening -= OnTextblockToolTipOpening;
        }

        private void OnFilterStatusBarLoaded(object sender, RoutedEventArgs e)
        {
            DataGrid.FilterChanged += OnDataGridFilterChanged;
        }

        private void OnDataGridFilterChanged(object? sender, GridFilterEventArgs e)
        {
            string filters = "";
            foreach (var item in this.DataGrid.Columns)
            {
                if (item != null && item.FilterPredicates.Count > 0)
                {
                    foreach (var filterItem in item.FilterPredicates)
                    {
                        if (filters == "")
                            filters += GetFilterInfo(filterItem, item);
                        else
                            filters += " " + this.GetPredicateType(filterItem.PredicateType) + " " + GetFilterInfo(filterItem, item);
                    }
                }
            }
            UpdateFilterText(filters);
            this.UpdateFilterStatusBarVisibility(filters.Trim() != "");
        }



        private void UpdateFilterStatusBarVisibility(bool visible)
        {
            if (visible)
                this.Visibility = Visibility.Visible;
            else
                this.Visibility = Visibility.Collapsed;
        }
        protected virtual string GetFilterInfo(FilterPredicate filter, GridColumn visColumn)
        {
            string filtervalue = (filter.FilterValue != null) ? filter.FilterValue.ToString() : null;
            FilterType filterType = filter.FilterType;
            string colname = "";

            if (filtervalue != null)
            {
                if (visColumn.CellType == "DateTime")
                    filtervalue = this.DateTimeFormatString(visColumn as GridDateTimeColumn, Convert.ToDateTime(filtervalue));
                else if (visColumn.CellType == "Currency")
                    filtervalue = "$" + (String.Format("{0:0.00}", Convert.ToDecimal(filtervalue)));
                else if (visColumn.CellType == "Percent")
                    filtervalue += "%";
            }

            if (visColumn != null)
                colname = (visColumn.HeaderText != "") ? visColumn.HeaderText : (visColumn.MappingName != "" ? visColumn.MappingName : "");
            switch (filterType)
            {
                case FilterType.LessThan:
                    return "[" + colname + "]" + " < '" + filtervalue + "'";
                case FilterType.LessThanOrEqual:
                    return "[" + colname + "]" + " <= '" + filtervalue + "'";
                case FilterType.Equals:
                    if (filtervalue == "")
                        return "[" + colname + "]" + " == Empty";
                    if (filtervalue != null)
                        return "[" + colname + "]" + " == '" + filtervalue + "'";
                    return "[" + colname + "]" + " = Null";
                case FilterType.NotEquals:
                    if (filtervalue == "")
                        return "[" + colname + "]" + " != Empty";
                    if (filtervalue != null)
                        return "[" + colname + "]" + " != '" + filtervalue + "'";
                    return "[" + colname + "]" + " != Null";
                case FilterType.GreaterThanOrEqual:
                    return "[" + colname + "]" + " >= '" + filtervalue + "'";
                case FilterType.GreaterThan:
                    return "[" + colname + "]" + " > '" + filtervalue + "'";
                case FilterType.EndsWith:
                    return "[" + colname + "]" + " Ends with '" + filtervalue + "'";
                case FilterType.NotEndsWith:
                    return "[" + colname + "]" + " Does Not Ends with '" + filtervalue + "'";
                case FilterType.StartsWith:
                    return "[" + colname + "]" + " Begins with '" + filtervalue + "'";
                case FilterType.NotStartsWith:
                    return "[" + colname + "]" + " Does Not Begins with '" + filtervalue + "'";
                case FilterType.Contains:
                    return "[" + colname + "]" + " Contains '" + filtervalue + "'";
                case FilterType.NotContains:
                    return "[" + colname + "]" + " Does Not Contain '" + filtervalue + "'";
                default:
                    return "[" + colname + "]" + " '" + filtervalue + "'";
            }

        }
        private string DateTimeFormatString(GridDateTimeColumn column, DateTime columnValue)
        {
            switch (column.Pattern)
            {
                case DateTimePattern.ShortDate:
                    return columnValue.ToString("d", column.DateTimeFormat);
                case DateTimePattern.LongDate:
                    return columnValue.ToString("D", column.DateTimeFormat);
                case DateTimePattern.LongTime:
                    return columnValue.ToString("T", column.DateTimeFormat);
                case DateTimePattern.ShortTime:
                    return columnValue.ToString("t", column.DateTimeFormat);
                case DateTimePattern.FullDateTime:
                    return columnValue.ToString("F", column.DateTimeFormat);
                case DateTimePattern.RFC1123:
                    return columnValue.ToString("R", column.DateTimeFormat);
                case DateTimePattern.SortableDateTime:
                    return columnValue.ToString("s", column.DateTimeFormat);
                case DateTimePattern.UniversalSortableDateTime:
                    return columnValue.ToString("u", column.DateTimeFormat);
                case DateTimePattern.YearMonth:
                    return columnValue.ToString("Y", column.DateTimeFormat);
                case DateTimePattern.MonthDay:
                    return columnValue.ToString("M", column.DateTimeFormat);
                case DateTimePattern.CustomPattern:
                    return columnValue.ToString(column.CustomPattern, column.DateTimeFormat);
                default:
                    return columnValue.ToString("MMMM", column.DateTimeFormat);
            }
        }
        private string GetPredicateType(PredicateType predicate)
        {
            if (predicate == PredicateType.And || predicate == PredicateType.AndAlso)
                return "&&";
            else
                return "||";
        }
        private string UpdateFilterText(string filter)
        {
            return this.FilterText = filter;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (isdisposed) return;

            UnWireEvents();
            if (isDisposing)
            {
                this.DataGrid = null;
            }

            isdisposed = true;
        }
    }
}
