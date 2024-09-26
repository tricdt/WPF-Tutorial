using Syncfusion.UI.Xaml.Grid;
using System.Collections;
using System.Collections.ObjectModel;

namespace syncfusion.datagriddemos.wpf
{
    public class CustomSelector : IItemsSourceSelector
    {
        public IEnumerable GetItemsSource(object record, object dataContext)
        {
            if (record == null) return null;
            var orderinfo = record as OrderInfo;
            var countryName = orderinfo.ShipCountry;
            var viewModel = dataContext as ComboBoxColumnsViewModel;
            if (viewModel.ShipCities.ContainsKey(countryName))
            {
                ObservableCollection<ShipCityDetails> shipcities = null;
                viewModel.ShipCities.TryGetValue(countryName, out shipcities);
                return shipcities.ToList();
            }
            return null;
        }
    }
}
