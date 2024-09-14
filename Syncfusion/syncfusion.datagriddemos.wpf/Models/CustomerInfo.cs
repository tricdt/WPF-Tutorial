
using Syncfusion.Windows.Shared;

namespace syncfusion.datagriddemos.wpf
{
    public class CustomerInfo : NotificationObject
    {
        private string _customerName;
        private string _country;
        private string _city;

        public string CustomerName
        {
            get
            {
                return _customerName;
            }
            set
            {
                _customerName = value;
                RaisePropertyChanged("CustomerName");
            }
        }


        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                RaisePropertyChanged("City");
            }
        }

        public string Country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
                RaisePropertyChanged("Country");
            }
        }

    }
}
