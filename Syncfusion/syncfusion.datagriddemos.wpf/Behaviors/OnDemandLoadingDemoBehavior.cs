using Microsoft.Xaml.Behaviors;
using Syncfusion.Data;
using Syncfusion.Data.Extensions;
using System.Collections.Specialized;
using System.ComponentModel;
namespace syncfusion.datagriddemos.wpf
{
    public class OnDemandLoadingDemoBehavior : Behavior<OnDemandPagingDemo>
    {
        private EmployeeInfoViewModel viewmodel;
        private List<EmployeeInfo> source;
        protected override void OnAttached()
        {
            viewmodel = new EmployeeInfoViewModel();
            source = viewmodel.GetEmployeesList(1800);
            AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            base.OnAttached();
        }

        private void OnAssociatedObjectLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AssociatedObject.sfDataPager.OnDemandLoading += OnDemandLoading;
            AssociatedObject.dataGrid.SortColumnsChanging += OnSortColumnsChanging;
            AssociatedObject.sfDataPager.MoveToFirstPage();
        }

        private void OnSortColumnsChanging(object? sender, Syncfusion.UI.Xaml.Grid.GridSortColumnsChangingEventArgs e)
        {
            (this.AssociatedObject.sfDataPager.PagedSource as PagedCollectionView).ResetCache();
            (this.AssociatedObject.sfDataPager.PagedSource as PagedCollectionView).ResetCacheForPage(this.AssociatedObject.sfDataPager.PageIndex);
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                var sortDesc = e.AddedItems[0];
                if (sortDesc.SortDirection == ListSortDirection.Ascending)
                {
                    source = source.OfQueryable().AsQueryable().OrderBy(sortDesc.ColumnName).Cast<EmployeeInfo>().ToList();
                }
                else
                {
                    source =
                        source.OfQueryable()
                              .AsQueryable()
                              .OrderByDescending(sortDesc.ColumnName)
                              .Cast<EmployeeInfo>()
                              .ToList();
                }
                this.AssociatedObject.sfDataPager.MoveToPage(this.AssociatedObject.sfDataPager.PageIndex);
            }
        }

        private void OnDemandLoading(object sender, Syncfusion.UI.Xaml.Controls.DataPager.OnDemandLoadingEventArgs e)
        {
            AssociatedObject.sfDataPager.LoadDynamicItems(e.StartIndex, source.Skip(e.StartIndex).Take(e.PageSize));
        }
    }
}
