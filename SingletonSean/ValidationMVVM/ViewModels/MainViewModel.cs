namespace ValidationMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //public CreateProductViewModel CreateProductViewModel { get; set; }
        public IPAddressViewModel IPAddressViewModel { get; set; }
        public MainViewModel()
        {
            IPAddressViewModel = new IPAddressViewModel();
            //CreateProductViewModel = new CreateProductViewModel();
        }
    }
}
