using Syncfusion.UI.Xaml.Grid;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
namespace syncfusion.datagriddemos.wpf
{
    public class SearchControl : Control, IDisposable
    {
        #region Fields
        internal Button FindNextButton;
        internal Button FindPreviousButton;
        internal Button CloseButton;
        internal Button ClearFilterButton;
        internal TextBox SearchTextBox;
        internal CheckBox ApplyFilterCheckBox;
        internal CheckBox CaseSensitiveSearchCheckBox;
        internal AdornerDecorator AdornerLayer;
        internal ComboBox ComboBox;
        #endregion
        #region ctor
        static SearchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchControl), new FrameworkPropertyMetadata(typeof(SearchControl)));
        }

        public SearchControl()
        {
        }

        public SearchControl(SfDataGrid datagrid)
        {
            DataGrid = datagrid;
        }
        #endregion
        #region Properties
        public static readonly DependencyProperty DataGridProperty =
            DependencyProperty.Register("DataGrid", typeof(SfDataGrid), typeof(SearchControl), new PropertyMetadata(null));
        public SfDataGrid DataGrid
        {
            get { return (SfDataGrid)GetValue(DataGridProperty); }
            set { SetValue(DataGridProperty, value); }
        }
        #endregion
        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            FindNextButton = this.GetTemplateChild("PART_FindNext") as Button;
            ClearFilterButton = this.GetTemplateChild("PART_ClearButton") as Button;
            FindPreviousButton = this.GetTemplateChild("PART_FindPrevious") as Button;
            CloseButton = this.GetTemplateChild("PART_Close") as Button;
            ApplyFilterCheckBox = this.GetTemplateChild("PART_ApplyFiltering") as CheckBox;
            ComboBox = this.GetTemplateChild("PART_ComboBox") as ComboBox;
            SearchTextBox = this.GetTemplateChild("PART_TextBox") as TextBox;
            CaseSensitiveSearchCheckBox = this.GetTemplateChild("PART_CaseSensitiveSearch") as CheckBox;
            AdornerLayer = this.GetTemplateChild("PART_AdornerLayer") as AdornerDecorator;
            this.SearchTextBox.Focus();
            this.WireEvents();
        }
        #endregion
        #region Events
        private void WireEvents()
        {
            FindNextButton.Click += OnFindNextButtonClick;
            ClearFilterButton.Click += OnClearFilterButtonClick;
            FindPreviousButton.Click += OnFindPreviousButtonClick;
            CloseButton.Click += OnCloseButtonClick;
            SearchTextBox.TextChanged += OnSearchTextBoxTextChanged;
            ApplyFilterCheckBox.Click += OnApplyFilterCheckBoxClick;
            CaseSensitiveSearchCheckBox.Click += OnCaseSensitiveSearchCheckBoxClick;
            ComboBox.SelectionChanged += OnComboBoxSelectionChanged;
            AdornerLayer.KeyDown += OnAdornerLayerKeyDown;
        }

        private void OnAdornerLayerKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key == Key.F) && (e.KeyboardDevice.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                this.UpdateSearchControlVisiblity(true);
            else if (e.Key == Key.Escape)
                this.UpdateSearchControlVisiblity(false);
        }
        internal void UpdateSearchControlVisiblity(bool visible)
        {
            if (visible)
            {
                if (this.DataGrid.SelectedDetailsViewGrid != null)
                {
                    var detailsViewIndex = this.DataGrid.GetGridDetailsViewRowIndex(this.DataGrid.SelectedDetailsViewGrid as DetailsViewDataGrid);
                    ComboBox.SelectedIndex = this.GetDetailsViewDefinitionIndex(this.DataGrid, detailsViewIndex - 1) + 1;
                }
                else
                    ComboBox.SelectedIndex = 0;
                this.Visibility = Visibility.Visible;
                this.SearchTextBox.Focus();
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                this.SearchTextBox.Clear();
                this.DataGrid.SearchHelper.ClearSearch();
                this.DataGrid.Focus();
            }
        }
        private int GetDetailsViewDefinitionIndex(SfDataGrid dataGrid, int actualRowIdx)
        {
            var counter0 = 0;
            for (int i = actualRowIdx; i > 0; i--)
            {
                if (!dataGrid.IsInDetailsViewIndex(i))
                {
                    break;
                }
                counter0++;
            }
            return counter0;
        }


        private void OnComboBoxSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var oldItem = e.RemovedItems[0];
            var newItem = e.AddedItems[0];
            if (oldItem == null || newItem == null)
                return;
            var oldGrid = this.GetDataGrid(oldItem.ToString());
            var newGrid = this.GetDataGrid(newItem.ToString());
            oldGrid.SearchHelper.ClearSearch();
            newGrid.SearchHelper.AllowFiltering = (bool)this.ApplyFilterCheckBox.IsChecked;
            newGrid.SearchHelper.AllowCaseSensitiveSearch = (bool)this.CaseSensitiveSearchCheckBox.IsChecked;
            newGrid.SearchHelper.Search(SearchTextBox.Text);
        }

        private void OnCaseSensitiveSearchCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var item = this.ComboBox.SelectedItem;
            if (item == null)
                return;
            var grid = this.GetDataGrid(item.ToString());
            grid.SearchHelper.AllowCaseSensitiveSearch = (bool)this.CaseSensitiveSearchCheckBox.IsChecked;
        }

        private void OnApplyFilterCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var item = this.ComboBox.SelectedItem;
            if (item == null)
                return;
            var grid = this.GetDataGrid(item.ToString());
            grid.SearchHelper.AllowFiltering = (bool)this.ApplyFilterCheckBox.IsChecked;
        }

        private void OnSearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var item = this.ComboBox.SelectedItem;
            if (item == null)
                return;
            var grid = this.GetDataGrid(item.ToString());
            grid.SearchHelper.Search(SearchTextBox.Text);
            var searchRecords = grid.SearchHelper.GetSearchRecords();
            if (SearchTextBox.Text != string.Empty && (searchRecords == null || searchRecords.Count <= 0))
            {
                if (this.FindNextButton.IsEnabled)
                    this.FindNextButton.IsEnabled = false;
                if (this.FindPreviousButton.IsEnabled)
                    this.FindPreviousButton.IsEnabled = false;
            }
            else
            {
                if (!this.FindNextButton.IsEnabled)
                    this.FindNextButton.IsEnabled = true;
                if (!this.FindPreviousButton.IsEnabled)
                    this.FindPreviousButton.IsEnabled = true;
            }
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.SearchTextBox.Clear();
            var item = this.ComboBox.SelectedItem;
            if (item != null)
                this.GetDataGrid(item.ToString()).SearchHelper.ClearSearch();
            this.Visibility = Visibility.Collapsed;
            this.DataGrid.Focus();
        }

        private void OnFindPreviousButtonClick(object sender, RoutedEventArgs e)
        {
            var item = this.ComboBox.SelectedItem;
            if (item == null)
                return;
            var grid = this.GetDataGrid(item.ToString());
            grid.SearchHelper.FindPrevious(SearchTextBox.Text);
        }

        private void OnClearFilterButtonClick(object sender, RoutedEventArgs e)
        {
            this.SearchTextBox.Clear();
        }

        private void OnFindNextButtonClick(object sender, RoutedEventArgs e)
        {
            var item = this.ComboBox.SelectedItem;
            if (item == null)
                return;
            var grid = this.GetDataGrid(item.ToString());
            grid.SearchHelper.FindNext(SearchTextBox.Text);
        }
        private SfDataGrid GetDataGrid(string item)
        {
            switch (item)
            {
                case "Search first level Grid's":
                    return (this.DataGrid.DetailsViewDefinition[0] as GridViewDefinition).DataGrid;
                case "Search second level Grid's":
                    return (this.DataGrid.DetailsViewDefinition[1] as GridViewDefinition).DataGrid;
                default:
                    return this.DataGrid;
            }
        }
        #endregion

        public void Dispose()
        {
            this.UnWireEvents();
            this.DataGrid = null;
        }

        private void UnWireEvents()
        {
            FindNextButton.Click -= OnFindNextButtonClick;
            ClearFilterButton.Click -= OnClearFilterButtonClick;
            FindPreviousButton.Click -= OnFindPreviousButtonClick;
            CloseButton.Click -= OnCloseButtonClick;
            SearchTextBox.TextChanged -= OnSearchTextBoxTextChanged;
            ApplyFilterCheckBox.Click -= OnApplyFilterCheckBoxClick;
            CaseSensitiveSearchCheckBox.Click -= OnCaseSensitiveSearchCheckBoxClick;
            ComboBox.SelectionChanged -= OnComboBoxSelectionChanged;
            AdornerLayer.KeyDown -= OnAdornerLayerKeyDown;
        }
    }
}
