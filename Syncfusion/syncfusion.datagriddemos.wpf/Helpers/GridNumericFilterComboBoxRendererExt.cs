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
    public class GridNumericFilterComboBoxRendererExt : GridFilterRowComboBoxRenderer, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private List<string> numericComboBoxItems;
        public GridNumericFilterComboBoxRendererExt() : base()
        {
            SetNumericComboBoxItemsList();
        }

        private void SetNumericComboBoxItemsList()
        {
            numericComboBoxItems = new List<string>();
            numericComboBoxItems.Add("Between 10001 and 10020");
            numericComboBoxItems.Add("Between 10030 and 10050");
            numericComboBoxItems.Add("Between 10060 and 10080");
            numericComboBoxItems.Add("Between 10090 and 10110");
            numericComboBoxItems.Add("Between 10120 and 10150");
            numericComboBoxItems.Add(">10200");
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
            uiElement.ItemsSource = numericComboBoxItems;
            InitializeNumericFilter(dataColumn, selItems);

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
                    ProcessNumericFilter(filterPredicates, item);
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
        public void InitializeNumericFilter(DataColumnBase dataColumn, ObservableCollection<object> SelectedItem)
        {
            if (dataColumn.GridColumn.FilteredFrom == FilteredFrom.FilterRow && dataColumn.GridColumn.FilterPredicates.Count > 0)
            {
                if (numericComboBoxItems != null)
                {
                    numericComboBoxItems.ForEach(element =>
                    {
                        //Check if the filter is already applied or not, if applied means again add the filter
                        bool needToAdd = false;
                        switch (element)
                        {
                            case "Between 10001 and 10020":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "10001");
                                break;
                            case "Between 10030 and 10050":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "10030");
                                break;
                            case "Between 10060 and 10080":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "10060");
                                break;
                            case "Between 10090 and 10110":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "10090");
                                break;
                            case "Between 10120 and 10150":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "10120");
                                break;
                            case ">10200":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "10200");
                                break;
                        }
                        if (needToAdd)
                            SelectedItem.Add(element);
                    });
                }
            }
        }
        public void ProcessNumericFilter(List<FilterPredicate> filterPredicates, string item)
        {
            switch (item)
            {
                case "Between 10001 and 10020":
                    filterPredicates.Add(GetFilterPredicates((int)10001, FilterType.GreaterThan, PredicateType.OrElse));
                    filterPredicates.Add(GetFilterPredicates((int)10020, FilterType.LessThan, PredicateType.And));
                    break;
                case "Between 10030 and 10050":
                    filterPredicates.Add(GetFilterPredicates((int)10030, FilterType.GreaterThan, PredicateType.OrElse));
                    filterPredicates.Add(GetFilterPredicates((int)10050, FilterType.LessThan, PredicateType.And));
                    break;
                case "Between 10060 and 10080":
                    filterPredicates.Add(GetFilterPredicates((int)10060, FilterType.GreaterThan, PredicateType.OrElse));
                    filterPredicates.Add(GetFilterPredicates((int)10080, FilterType.LessThan, PredicateType.And));
                    break;
                case "Between 10090 and 10110":
                    filterPredicates.Add(GetFilterPredicates((int)10090, FilterType.GreaterThan, PredicateType.OrElse));
                    filterPredicates.Add(GetFilterPredicates((int)10110, FilterType.LessThan, PredicateType.And));
                    break;
                case "Between 10120 and 10150":
                    filterPredicates.Add(GetFilterPredicates((int)10120, FilterType.GreaterThan, PredicateType.OrElse));
                    filterPredicates.Add(GetFilterPredicates((int)10150, FilterType.LessThan, PredicateType.And));
                    break;
                case ">10200":
                    filterPredicates.Add(GetFilterPredicates((int)10200, FilterType.GreaterThan, PredicateType.Or));
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
    }
}
