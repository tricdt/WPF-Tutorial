using CommunicationMVVM.Models;
using CommunicationMVVM.Stores;
using System.Collections.ObjectModel;
namespace CommunicationMVVM.ViewModels
{
    public class ProductListingViewModel : ViewModelBase
    {
        private readonly ProductStore _productStore;
        private readonly ObservableCollection<ProductViewModel> _products;

        public ProductListingViewModel(ProductStore productStore)
        {
            _productStore = productStore;
            _products = new ObservableCollection<ProductViewModel>();
            _products.Add(new ProductViewModel(new Product()
            {
                Name = "T-Shirt",
                Price = 49.99,
                Quantity = 3
            }));
            _productStore.ProductCreated += OnProductCreated;
        }

        private void OnProductCreated(Product product)
        {
            _products.Add(new ProductViewModel(product));
        }
        public override void Dispose()
        {
            _productStore.ProductCreated -= OnProductCreated;
            base.Dispose();
        }
        public IEnumerable<ProductViewModel> Products => _products;
        public bool HasProducts => _products.Count > 0;
        public bool HasNoProducts => !HasProducts;
    }
}
