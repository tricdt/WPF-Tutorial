namespace NavigationMVVM.ViewModels
{
    public class PersonViewModel : ViewModelBase
    {
        public string Name { get; }

        public PersonViewModel(string name)
        {
            Name = name;
        }
    }
}
