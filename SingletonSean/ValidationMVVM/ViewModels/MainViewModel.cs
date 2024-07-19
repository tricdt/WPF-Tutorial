namespace ValidationMVVM.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public CreateProductViewModel CreateProductViewModel { get; set; }
        public MainViewModel()
        {
            CreateProductViewModel = new CreateProductViewModel();
        }
    }
}
