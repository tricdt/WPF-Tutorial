using CommunicationMVVM.Models;

namespace CommunicationMVVM.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly Product _product;

        public string Name => _product.Name;
        public double Price => _product.Price;
        public int Quantity => _product.Quantity;

        public ProductViewModel(Product product)
        {
            _product = product;
        }
    }
}
