using System.Collections;
using System.ComponentModel;
using System.Windows.Input;
using ValidationMVVM.Commands;
namespace ValidationMVVM.ViewModels
{
    public class CreateProductViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        private readonly ErrorsViewModel _errorsViewModel;
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private double _price;

        public CreateProductViewModel()
        {
            CreateProductCommand = new CreateProductCommand(this);

            _errorsViewModel = new ErrorsViewModel();
            _errorsViewModel.ErrorsChanged += ErrorsViewModel_ErrorsChanged;
        }

        private void ErrorsViewModel_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
            OnPropertyChanged(nameof(CanCreate));
        }

        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                _errorsViewModel.ClearErrors(nameof(Price));
                if (_price > 50)
                {
                    _errorsViewModel.AddError(nameof(Price), "Invalid price. The max product price is $50.00.");
                }
                OnPropertyChanged(nameof(Price));
            }
        }


        public bool HasErrors => _errorsViewModel.HasErrors;
        public bool CanCreate => !HasErrors;
        public ICommand CreateProductCommand { get; }

        public IEnumerable GetErrors(string? propertyName)
        {
            return _errorsViewModel.GetErrors(propertyName);
        }
    }
}
