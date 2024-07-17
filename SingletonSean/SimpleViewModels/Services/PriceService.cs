using SimpleViewModels.Exceptions;
namespace SimpleViewModels.Services
{
    public class PriceService
    {
        public double GetPrice(string itemName)
        {
            switch (itemName.ToLower())
            {
                case "apple":
                    return 0.49;
                case "shirt":
                    return 19.99;
                case "phone":
                    return 499.99;
                case "burrito":
                    return 9.99;
                case "shoes":
                    return 119.99;
                default:
                    throw new ItemPriceNotFoundException(itemName);
            }
        }
    }
}
