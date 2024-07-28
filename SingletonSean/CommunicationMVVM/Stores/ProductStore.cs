using CommunicationMVVM.Models;
namespace CommunicationMVVM.Stores
{
    public class ProductStore
    {
        public event Action<Product> ProductCreated;

        public void CreateProduct(Product product)
        {
            ProductCreated?.Invoke(product);
        }
    }
}
