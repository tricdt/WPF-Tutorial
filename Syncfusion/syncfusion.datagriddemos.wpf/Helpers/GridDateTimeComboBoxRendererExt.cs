using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.RowFilter;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace syncfusion.datagriddemos.wpf
{
    public class GridDateTimeComboBoxRendererExt : GridFilterRowComboBoxRendererBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private List<string> dateTimeComboBoxItems;
        public GridDateTimeComboBoxRendererExt() : base()
        {
            SetDateTimeComboBoxItemsList();
        }

        private void SetDateTimeComboBoxItemsList()
        {
            var date = DateTime.Today;
            var today = date.DayOfWeek;
            dateTimeComboBoxItems = new List<string>();
            dateTimeComboBoxItems.Add("Today");
            date = date.AddDays(-1);
            today = date.DayOfWeek;
            if (today.ToString() != "Saturday")
                dateTimeComboBoxItems.Add("Yesterday");
            for (int i = 0; i < 7; i++)
            {
                date = date.AddDays(-1);
                today = date.DayOfWeek;
                if (today.ToString() != "Saturday")
                    dateTimeComboBoxItems.Add(today.ToString());
                else
                    break;
            }
            dateTimeComboBoxItems.Add("LastWeek");
            dateTimeComboBoxItems.Add("LastMonth");
            dateTimeComboBoxItems.Add("Older");
        }
        public override void OnInitializeDisplayElement(DataColumnBase dataColumn, ContentControl uiElement, object dataContext)
        {
            base.OnInitializeDisplayElement(dataColumn, uiElement, dataContext);
            uiElement.Margin = new Thickness(5, 0, 0, 0);
        }
        protected override void InitializeEditBinding(Syncfusion.Windows.Tools.Controls.ComboBoxAdv uiElement, DataColumnBase dataColumn)
        {
            ObservableCollection<object> selItems = new ObservableCollection<object>();
            //Generate the items for FilterRow 
            uiElement.ItemsSource = dateTimeComboBoxItems;
            InitializeDateFilter(dataColumn, selItems);
            if (selItems.Count > 0)
                uiElement.SelectedItems = selItems;
            else if (uiElement.SelectedItems != null)
                uiElement.SelectedItems = null;
            uiElement.AllowMultiSelect = true;
            uiElement.AllowSelectAll = true;
            uiElement.EnableOKCancel = true;
            uiElement.IsEditable = false;
        }
        public override void ProcessMultipleFilters(List<object> filterValues, List<object> totalItems)
        {
            var selectedItems = filterValues.Cast<string>().ToList();
            var total = totalItems.Cast<string>().ToList();
            if (selectedItems == null || total == null || filterValues == null)
                return;

            if (selectedItems.Count == total.Count)
            {
                this.ApplyFilters(null, string.Empty);
                this.IsValueChanged = false;
                return;
            }
            var filterPredicates = new List<FilterPredicate>();
            if (filterValues.Count > 0)
            {
                selectedItems.ForEach(item =>
                {
                    //Create the FilterPredicates and Apply the filter 
                    ProcessDateFilter(filterPredicates, item);
                });
            }
            string _filterText = string.Empty;
            //Creates the FilterText
            if (filterPredicates.Count > 0)
            {
                var selectItems = ((IList)filterValues).Cast<string>().ToList();
                for (int i = 0; i < selectedItems.Count; i++)
                {
                    _filterText += selectedItems[i];
                    if (i != selectedItems.Count - 1)
                        _filterText += " - ";
                }
            }
            if (filterPredicates != null)
                this.ApplyFilters(filterPredicates, _filterText);
            this.IsValueChanged = false;
        }
        public void ProcessDateFilter(List<FilterPredicate> filterPredicates, string item)
        {
            var date = DateTime.Today;
            var day = date.DayOfWeek;
            switch (item)
            {
                case "Today":
                    filterPredicates.Add(GetFilterPredicates((DateTime)DateTime.Today, FilterType.Equals, PredicateType.OrElse));
                    break;
                case "Yesterday":
                    filterPredicates.Add(GetFilterPredicates((DateTime)DateTime.Today.AddDays(-1), FilterType.Equals, PredicateType.OrElse));
                    break;
                case "Thursday":
                    filterPredicates.Add(GetFilterPredicates((DateTime)DateTime.Today.AddDays(-2), FilterType.Equals, PredicateType.OrElse));
                    break;
                case "Wednesday":
                    if (DateTime.Today.AddDays(-3).DayOfWeek.ToString() == "Wednesday")
                        filterPredicates.Add(GetFilterPredicates((DateTime)DateTime.Today.AddDays(-3), FilterType.Equals, PredicateType.OrElse));
                    else if (DateTime.Today.AddDays(-2).DayOfWeek.ToString() == "Wednesday")
                        filterPredicates.Add(GetFilterPredicates((DateTime)DateTime.Today.AddDays(-2), FilterType.Equals, PredicateType.OrElse));
                    break;
                case "Tuesday":
                    for (int i = 2; i <= 5; i++)
                    {
                        date = date.AddDays(-1);
                        day = date.DayOfWeek;
                        if (day.ToString() == "Tuesday")
                        {
                            filterPredicates.Add(GetFilterPredicates((DateTime)date, FilterType.Equals, PredicateType.OrElse));
                            break;
                        }
                    }
                    break;
                case "Monday":
                    for (int i = 2; i <= 6; i++)
                    {
                        date = date.AddDays(-1);
                        day = date.DayOfWeek;
                        if (day.ToString() != "Saturday" && day.ToString() == "Monday")
                        {
                            filterPredicates.Add(GetFilterPredicates((DateTime)date, FilterType.Equals, PredicateType.OrElse));
                            break;
                        }
                    }
                    break;
                case "Sunday":
                    for (int i = 2; i <= 7; i++)
                    {
                        date = date.AddDays(-1);
                        day = date.DayOfWeek;
                        if (day.ToString() != "Saturday" && day.ToString() == "Sunday")
                        {
                            filterPredicates.Add(GetFilterPredicates((DateTime)date, FilterType.Equals, PredicateType.OrElse));
                            break;
                        }
                    }
                    break;
                case "LastWeek":
                    {
                        DateTime startingDate = DateTime.Today;

                        while (startingDate.DayOfWeek != DayOfWeek.Sunday)
                            startingDate = startingDate.AddDays(-1);

                        DateTime previousWeekStart = startingDate.AddDays(-7);
                        DateTime previousWeekEnd = startingDate.AddDays(-1);
                        filterPredicates.Add(GetFilterPredicates((DateTime)previousWeekStart, FilterType.GreaterThanOrEqual, PredicateType.OrElse));
                        filterPredicates.Add(GetFilterPredicates((DateTime)previousWeekEnd, FilterType.LessThanOrEqual, PredicateType.And));
                    }
                    break;

                case "LastMonth":
                    {
                        var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        var firstdate = month.AddMonths(-1);
                        var lastdate = month.AddDays(-1);
                        filterPredicates.Add(GetFilterPredicates((DateTime)firstdate, FilterType.GreaterThanOrEqual, PredicateType.OrElse));
                        filterPredicates.Add(GetFilterPredicates((DateTime)lastdate, FilterType.LessThanOrEqual, PredicateType.And));
                    }
                    break;
                case "Older":
                    {
                        var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        var firstdate = month.AddMonths(-1);
                        filterPredicates.Add(GetFilterPredicates((DateTime)firstdate, FilterType.LessThan, PredicateType.OrElse));
                    }
                    break;
            }
        }
        private FilterPredicate GetFilterPredicates(object value, FilterType filterType, PredicateType predType)
        {
            return new FilterPredicate()
            {
                FilterBehavior = FilterBehavior.StronglyTyped,
                FilterType = filterType,
                FilterValue = value,
                IsCaseSensitive = false,
                PredicateType = predType
            };
        }
        public void InitializeDateFilter(DataColumnBase dataColumn, ObservableCollection<object> SelectedItem)
        {
            if (dataColumn.GridColumn.FilteredFrom == FilteredFrom.FilterRow && dataColumn.GridColumn.FilterPredicates.Count > 0)
            {
                if (dateTimeComboBoxItems != null)
                {
                    dateTimeComboBoxItems.ForEach(element =>
                    {
                        //Check if the filter is already applied or not, if applied means again add the filter
                        bool needToAdd = false;
                        var tempdate = DateTime.Today;
                        var day = tempdate.DayOfWeek;
                        switch (element)
                        {
                            case "Today":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, DateTime.Today.ToString());
                                break;
                            case "Yesterday":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, DateTime.Today.AddDays(-1).ToString());
                                break;
                            case "Thursday":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, DateTime.Today.AddDays(-2).ToString());
                                break;
                            case "Wednesday":
                                needToAdd = DateTime.Today.AddDays(-3).DayOfWeek.ToString() == "Wednesday" ? this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, DateTime.Today.AddDays(-3).ToString())
                                           : DateTime.Today.AddDays(-2).DayOfWeek.ToString() == "Wednesday" ? this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, DateTime.Today.AddDays(-2).ToString()) : false;
                                break;
                            case "Tuesday":
                                for (int i = 2; i <= 5; i++)
                                {
                                    tempdate = tempdate.AddDays(-1);
                                    day = tempdate.DayOfWeek;
                                    if (day.ToString() == "Tuesday")
                                    {
                                        needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, tempdate.ToString());
                                        break;
                                    }
                                }
                                break;
                            case "Monday":
                                for (int i = 2; i <= 6; i++)
                                {
                                    tempdate = tempdate.AddDays(-1);
                                    day = tempdate.DayOfWeek;
                                    if (day.ToString() == "Monday")
                                    {
                                        needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, tempdate.ToString());
                                        break;
                                    }
                                }
                                break;
                            case "Sunday":
                                for (int i = 2; i <= 7; i++)
                                {
                                    tempdate = tempdate.AddDays(-1);
                                    day = tempdate.DayOfWeek;
                                    if (day.ToString() == "Sunday")
                                    {
                                        needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, tempdate.ToString());
                                        break;
                                    }
                                }
                                break;
                            case "LastWeek":
                                DateTime startingDate = DateTime.Today;
                                while (startingDate.DayOfWeek != DayOfWeek.Sunday)
                                    startingDate = startingDate.AddDays(-1);
                                DateTime previousWeekStart = startingDate.AddDays(-7);
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, previousWeekStart.ToString());
                                break;
                            case "LastMonth":
                                {
                                    var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                                    var lastdate = month.AddDays(-1);
                                    needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, lastdate.ToString());
                                }
                                break;
                            case "Older":
                                {
                                    var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                                    var firstdate = month.AddMonths(-1);
                                    needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, firstdate.ToString());
                                }
                                break;
                        }
                        if (needToAdd)
                            SelectedItem.Add(element);
                    });
                }
            }
        }
        private bool NeedToAdd(ObservableCollection<FilterPredicate> filterPredicate, string filterValue)
        {
            bool needToAdd = false;
            foreach (var item in filterPredicate)
            {
                if ((item as FilterPredicate).FilterValue.ToString() == filterValue)
                {
                    needToAdd = true;
                    break;
                }
            }
            return needToAdd;
        }
        private void OnPropertyChanged(String prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
