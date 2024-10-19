using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.RowFilter;
using Syncfusion.Windows.Tools.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
namespace syncfusion.datagriddemos.wpf
{
    public class GridTextFilterComboBoxRendererExt : GridFilterRowComboBoxRenderer, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private List<string> textComboBoxItems;
        public GridTextFilterComboBoxRendererExt() : base()
        {
            SetTextComboBoxItemsList();
        }

        private void SetTextComboBoxItemsList()
        {
            textComboBoxItems = new List<string>();
            textComboBoxItems.Add("A to D");
            textComboBoxItems.Add("E to H");
            textComboBoxItems.Add("I to L");
            textComboBoxItems.Add("M to P");
            textComboBoxItems.Add("Q to U");
            textComboBoxItems.Add("V to Z");
        }
        public override void OnInitializeDisplayElement(DataColumnBase dataColumn, ContentControl uiElement, object dataContext)
        {
            base.OnInitializeDisplayElement(dataColumn, uiElement, dataContext);
            uiElement.Margin = new Thickness(5, 0, 0, 0);
        }
        protected override void InitializeEditBinding(ComboBoxAdv uiElement, DataColumnBase dataColumn)
        {
            ObservableCollection<object> selItems = new ObservableCollection<object>();

            //Generate the items for FilterRow 
            uiElement.ItemsSource = textComboBoxItems;
            InitializeTextFilter(dataColumn, selItems);
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
                    ProcessTextFilter(filterPredicates, item);
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
        public void ProcessTextFilter(List<FilterPredicate> filterPredicates, string item)
        {
            switch (item)
            {
                case "A to D":
                    filterPredicates.Add(GetStringFilterPredicates("A", FilterType.StartsWith, PredicateType.OrElse));
                    filterPredicates.Add(GetStringFilterPredicates("B", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("C", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("D", FilterType.StartsWith, PredicateType.Or));
                    break;
                case "E to H":
                    filterPredicates.Add(GetStringFilterPredicates("E", FilterType.StartsWith, PredicateType.OrElse));
                    filterPredicates.Add(GetStringFilterPredicates("F", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("G", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("H", FilterType.StartsWith, PredicateType.Or));
                    break;
                case "I to L":
                    filterPredicates.Add(GetStringFilterPredicates("I", FilterType.StartsWith, PredicateType.OrElse));
                    filterPredicates.Add(GetStringFilterPredicates("J", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("K", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("L", FilterType.StartsWith, PredicateType.Or));
                    break;
                case "M to P":
                    filterPredicates.Add(GetStringFilterPredicates("M", FilterType.StartsWith, PredicateType.OrElse));
                    filterPredicates.Add(GetStringFilterPredicates("N", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("O", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("P", FilterType.StartsWith, PredicateType.Or));
                    break;
                case "Q to U":
                    filterPredicates.Add(GetStringFilterPredicates("Q", FilterType.StartsWith, PredicateType.OrElse));
                    filterPredicates.Add(GetStringFilterPredicates("R", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("S", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("T", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("U", FilterType.StartsWith, PredicateType.Or));
                    break;
                case "V to Z":
                    filterPredicates.Add(GetStringFilterPredicates("V", FilterType.StartsWith, PredicateType.OrElse));
                    filterPredicates.Add(GetStringFilterPredicates("W", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("X", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("Y", FilterType.StartsWith, PredicateType.Or));
                    filterPredicates.Add(GetStringFilterPredicates("Z", FilterType.StartsWith, PredicateType.Or));
                    break;
            }
        }
        private FilterPredicate GetStringFilterPredicates(object value, FilterType filterType, PredicateType predType)
        {
            return new FilterPredicate()
            {
                FilterBehavior = FilterBehavior.StringTyped,
                FilterType = filterType,
                FilterValue = value,
                IsCaseSensitive = false,
                PredicateType = predType
            };
        }
        private void InitializeTextFilter(DataColumnBase dataColumn, ObservableCollection<object> SelectedItem)
        {
            if (dataColumn.GridColumn.FilteredFrom == FilteredFrom.FilterRow && dataColumn.GridColumn.FilterPredicates.Count > 0)
            {
                if (textComboBoxItems != null)
                {
                    textComboBoxItems.ForEach(element =>
                    {
                        //Check if the filter is already applied or not, if applied means again add the filter
                        bool needToAdd = false;
                        switch (element)
                        {
                            case "A to D":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "A");
                                break;
                            case "E to H":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "E");
                                break;
                            case "I to L":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "I");
                                break;
                            case "M to P":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "M");
                                break;
                            case "Q to U":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "Q");
                                break;
                            case "V to Z":
                                needToAdd = this.NeedToAdd(dataColumn.GridColumn.FilterPredicates, "V");
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
