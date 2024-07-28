namespace CommunicationMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(CreateProductViewModel createProductViewModel, ProductListingViewModel productListingViewModel)
        {
            CreateProductViewModel = createProductViewModel;
            ProductListingViewModel = productListingViewModel;
        }

        public CreateProductViewModel CreateProductViewModel { get; }
        public ProductListingViewModel ProductListingViewModel { get; }
    }
}
